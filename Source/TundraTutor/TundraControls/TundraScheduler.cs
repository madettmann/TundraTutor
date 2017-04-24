//Written by Victor
using System;
using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using System.Data.Entity.Core.Objects;

namespace TundraControls
{
    public class TundraScheduler : Control
    {
        #region Fields
        private TutoringDB.CurrentUser user;
        private bool isTutor;
        private TimeSpan selectedTime;
        private DateTime selectedDate;
        private int selectedIndex;
        private string selectedDOW;
        #endregion

        #region Properties
        public ObservableCollection<Timeslot> times { get; set; }
        public ObservableCollection<string> DayNames { get; set; }
        public TimeSpan SelectedTime { get => selectedTime; set => selectedTime = value; }
        public DateTime SelectedDate { get => selectedDate; set => selectedDate = value; }
        public int SelectedIndex { get => selectedIndex; set => selectedIndex = value; }
        #endregion


        #region Events 
        //Command that listens to each timeslot and wait for a click on them - assigned in constructor (also day of week)
        public ICommand timeClickedCommand { get; private set; }
        public ICommand DOWClickedCommand { get; private set; }
        //Event for exposing timeClick (and dow)
        public static RoutedEvent TimeClickEvent = EventManager.RegisterRoutedEvent("TimeClicked", RoutingStrategy.Bubble,
                                                                                    typeof(RoutedEventHandler), typeof(TundraCalendar));
        public static RoutedEvent DOWClickEvent = EventManager.RegisterRoutedEvent("DOWClicked", RoutingStrategy.Bubble,
                                                                                    typeof(RoutedEventHandler), typeof(TundraCalendar));
        //Event handler for TimeClickEvent (and dow)
        public event RoutedEventHandler TimeClicked { add => AddHandler(TimeClickEvent, value); remove => RemoveHandler(TimeClickEvent, value); }
        public event RoutedEventHandler DOWClicked { add => AddHandler(DOWClickEvent, value); remove => RemoveHandler(DOWClickEvent, value); }
        #endregion

        //For style
        static TundraScheduler()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TundraScheduler), new FrameworkPropertyMetadata(typeof(TundraScheduler)));
        }

        /// <summary>
        /// Constructor for scheduler
        /// </summary>
        public TundraScheduler()
        {
            DataContext = this;

            //Executes the onCurrentTimeClicked method (to expose event), see bottom of file for implementation
            timeClickedCommand = new RelayCommand<object>(onCurrentTimeClicked); //Relay command is a common implementation
            DOWClickedCommand = new RelayCommand<object>(onDOWClicked);          //of the abstract ICommand type; I'm using
                                                                                 //the version contained in the MVVMLightLibs NuGet 
                                                                                 //package - it is very similar to the 
                                                                                 //standard implementation popularized by MS on MSDN

            //Get the current user
            user = new TutoringDB.CurrentUser();
            var readUser = new TutoringDB.TutorDatabaseEntities();
            readUser.CurrentUsers.Load();
            var userList = from i in readUser.CurrentUsers select i;
            TutoringDB.Tutee oneUser = new TutoringDB.Tutee();
            foreach (var element in userList) { oneUser.Username = element.UserName; }
            user.UserName = oneUser.Username;



            times = new ObservableCollection<Timeslot>();
            BuildWeek();
        }

        /// <summary>
        /// Generates the selected week in the TundraWeek control
        /// </summary>
        /// <param name="startDate">First day of the selected week</param>
        public void BuildWeek()
        {
            times.Clear();

            //Align day names
            DayNames = new ObservableCollection<string> { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" };

            //Load user schedule
            TutoringDB.TutorDatabaseEntities schedules = new TutoringDB.TutorDatabaseEntities();
            schedules.BaseSchedules.Load();
            var userSchedule = from schedule in schedules.BaseSchedules
                              where schedule.Tutee.Username == user.UserName || schedule.Tutor.UserName == user.UserName
                              select schedule;
            isTutor = schedules.CurrentUsers.FirstOrDefault().Type == "tutor";

            //Set the first day to generate
            DateTime d = new DateTime(2014, 1, 5); //5 Jan, 2014 is a Sunday; it is also passed, and so should not be overridden ... unless by some clever idiot ...
            TimeSpan t = new TimeSpan(08, 00, 00);

            #region Week Generation
            //24 timeslots (08:00 to 20:00) * 7 days in a week
            for (int box = 0; box < 168; box++)
            {
                var daySchedule = from schedTime in userSchedule
                                  where schedTime.BusyTime.Date == d
                                  select schedTime;

                Timeslot time = new Timeslot(d, t, false);

                //Set appointment to an empty list
                time.Appointment = new ObservableCollection<Appointment>();

                //Add busy to time if applicable
                time.Busy = new ObservableCollection<Busy>();
                foreach (var oneBusy in daySchedule) if (oneBusy.BusyTime.Time == t) time.Busy.Add(new Busy(oneBusy.BusyTime.Time, new TimeSpan(00,30,00), d));

                //Add timeslot to the collection so it appears in the schedule
                times.Add(time);

                //Increment time
                if (t.Hours == 19 && t.Minutes == 30)
                {
                    d = d.AddDays(1);
                    t = new TimeSpan(08, 00, 00);
                }
                else
                {
                    t = t.Add(new TimeSpan(00, 30, 00));
                }


            }

            #endregion

            #region Times Rearranging

            //Necessary because the times goe left -> right -> down rather than up -> down -> left
            List<Timeslot> timeflipper = new List<Timeslot>();
            for (int i = 0; i < 24; i++)
            {
                for (int box = 0; box < 7; box++)
                {
                    timeflipper.Add(times.ElementAt(i + (24 * box)));
                }
            }
            times.Clear();
            //Enjoy your newly rearranged times collection!
            foreach (var box in timeflipper)
            {
                times.Add(box);
            }


            #endregion
        }

        /// <summary>
        /// Marks individual times, for use in busy scheduling
        /// </summary>
        /// <param name="index">Index of the time within the ItemsControl (use SelectedIndex property)</param>
        public void markTime(int index)
        {
            if (times.ElementAt(index).Marked > 0)
            {
                times.ElementAt(index).Marked = 0;
            }
            else
            {
                if (times.ElementAt(index).Busy.Count > 0)
                    times.ElementAt(index).Marked = 2;
                else
                    times.ElementAt(index).Marked = 1;
            }
        }

        /// <summary>
        /// Saves the times marked as busy by the user to the database (in 30-minute chunks)
        /// </summary>
        public void saveMarked()
        {
            TutoringDB.TutorDatabaseEntities addBusy = new TutoringDB.TutorDatabaseEntities();
            //addBusy.StartEnds.Load();
            DateTime start = (DateTime)addBusy.StartEnds.FirstOrDefault().StartDate;

            foreach (var time in times)
            {
                if (time.Marked == 1)
                {
                    TutoringDB.BusyTime tempBusy = new TutoringDB.BusyTime();
                    addBusy.BaseSchedules.Where(baseS => (baseS.Tutor.UserName == user.UserName || baseS.Tutee.Username == user.UserName)
                                                          && baseS.BusyTime.Date == time.Date
                                                          && baseS.BusyTime.Time == time.Time).Load();
                    addBusy.BusyTimes.Where(bus => bus.Date == time.Date && bus.Time == time.Time).Load();
                    tempBusy.Time = time.Time;
                    tempBusy.Date = time.Date;
                    tempBusy.Duration = new TimeSpan(00, 30, 00);
                    addBusy.BusyTimes.Add(tempBusy);
                    if (isTutor)
                    {
                        //addBusy.Tutors.Where(tut => tut.UserName == user.UserName).Load();
                        //addBusy.BaseSchedules.Where(baseS => baseS.Tutor.UserName == user.UserName).Load();
                        //addBusy.TutorBusyTimes.Where(bus => bus.Tutor.UserName == user.UserName
                        //                                    && bus.BusyTime.Date == time.Date
                        //                                    && bus.BusyTime.Time == time.Time).Load();
                        //addBusy.BusyTimes.Where(bus => bus.Date == time.Date && bus.Time == time.Time).Load();

                        TutoringDB.BaseSchedule modSched = new TutoringDB.BaseSchedule();
                        modSched.BusyTimeId = tempBusy.Id;
                        TutoringDB.Tutor tempTutor = addBusy.Tutors.Where(i => i.UserName == addBusy.CurrentUsers.FirstOrDefault().UserName).First();
                        modSched.TutorId = tempTutor.Id;
                        addBusy.BaseSchedules.Add(modSched);

                        addBusy.SaveChanges();

                        //var busies = from i in addBusy.TutorBusyTimes
                        //             where i.BusyTime.Time == time.Time && i.Tutor.UserName == user.UserName
                        //             select i;


                        int difference = DayOfWeekNumber(time.Date.DayOfWeek) - DayOfWeekNumber(start.DayOfWeek);
                        start = start.AddDays(difference);

                        for (DateTime i = start; i < (DateTime)addBusy.StartEnds.FirstOrDefault().EndDate; i = i.AddDays(7))
                        {
                            //addBusy.BusyTimes.Where(bus => bus.Date == time.Date && bus.Time == time.Time).Load();
                            i = i.AddHours(-i.TimeOfDay.Hours); i = i.AddMinutes(-i.TimeOfDay.Minutes); i = i.AddSeconds(-i.TimeOfDay.Seconds);

                            var busies = addBusy.TutorBusyTimes.Where(bus => bus.BusyTime.Date == time.Date
                                                                      && bus.BusyTime.Time == time.Time
                                                                      && bus.TutorId == tempTutor.Id).ToList();
                            var appointments = addBusy.TutorTuteeCourseAppointments.Where(appt => appt.Appointment.Date == time.Date
                                                                                          && appt.Appointment.Time == time.Time
                                                                                          && appt.TutorId == tempTutor.Id).ToList();
                            //from bus in addBusy.TuteeBusyTimes
                            //where bus.BusyTime.Time == time.Time && bus.Tutee.Username == user.UserName && bus.BusyTime.Date == time.Date
                            //select i;
                            if (busies.Count == 0 && appointments.Count == 0)
                            {
                                TutoringDB.BusyTime newBusy = new TutoringDB.BusyTime();
                                newBusy.Date = i;
                                newBusy.Time = time.Time;
                                TutoringDB.TutorBusyTime newLink = new TutoringDB.TutorBusyTime();
                                newLink.BusyTimesId = newBusy.Id;
                                newLink.TutorId = addBusy.Tutors.Where(tut => tut.UserName == user.UserName).FirstOrDefault().Id;

                                addBusy.BusyTimes.Add(newBusy);
                                addBusy.TutorBusyTimes.Add(newLink);
                            }
                            addBusy.SaveChanges();
                        }
                    }
                    else
                    {
                        //addBusy.Tutees.Where(tut => tut.Username == user.UserName).Load();
                        //addBusy.BaseSchedules.Where(baseS => baseS.Tutee.Username == user.UserName).Load();
                        //addBusy.TuteeBusyTimes.Where(bus => bus.Tutee.Username == user.UserName 
                        //                                    && bus.BusyTime.Date == time.Date 
                        //                                    && bus.BusyTime.Time == time.Time).Load();
                        //addBusy.BusyTimes.Where(bus => bus.Date == time.Date && bus.Time == time.Time).Load();

                        TutoringDB.BaseSchedule modSched = new TutoringDB.BaseSchedule();
                        modSched.BusyTimeId = tempBusy.Id;
                        TutoringDB.Tutee tempTutee = addBusy.Tutees.Where(i => i.Username == addBusy.CurrentUsers.FirstOrDefault().UserName).First();
                        modSched.TuteeId = tempTutee.Id;
                        addBusy.BaseSchedules.Add(modSched);

                        addBusy.SaveChanges();

                        //var busies = from i in addBusy.TuteeBusyTimes
                        //             where i.BusyTime.Time == time.Time && i.Tutee.Username == user.UserName
                        //             select i;

                        int difference = DayOfWeekNumber(time.Date.DayOfWeek) - DayOfWeekNumber(start.DayOfWeek);
                        start = start.AddDays(difference);

                        for (DateTime i = start; i < (DateTime)addBusy.StartEnds.FirstOrDefault().EndDate; i = i.AddDays(7))
                        {
                            //addBusy.BusyTimes.Where(bus => bus.Date == time.Date && bus.Time == time.Time).Load();
                            i = i.AddHours(-i.TimeOfDay.Hours); i = i.AddMinutes(-i.TimeOfDay.Minutes); i = i.AddSeconds(-i.TimeOfDay.Seconds);

                            var busies = addBusy.TuteeBusyTimes.Where(bus => bus.BusyTime.Date == time.Date
                                                                      && bus.BusyTime.Time == time.Time
                                                                      && bus.TuteeId == tempTutee.Id).ToList();
                            var appointments = addBusy.TutorTuteeCourseAppointments.Where(appt => appt.Appointment.Date == time.Date
                                                                                          && appt.Appointment.Time == time.Time
                                                                                          && appt.TuteeId == tempTutee.Id).ToList();
                            //from bus in addBusy.TuteeBusyTimes
                            //where bus.BusyTime.Time == time.Time && bus.Tutee.Username == user.UserName && bus.BusyTime.Date == time.Date
                            //select i;
                            if (busies.Count == 0 && appointments.Count == 0)
                            {
                                TutoringDB.BusyTime newBusy = new TutoringDB.BusyTime();
                                newBusy.Date = i;
                                newBusy.Time = time.Time;
                                TutoringDB.TuteeBusyTime newLink = new TutoringDB.TuteeBusyTime();
                                newLink.BusyTimeId = newBusy.Id;
                                newLink.TuteeId = addBusy.Tutees.Where(tut => tut.Username == user.UserName).FirstOrDefault().Id;

                                addBusy.BusyTimes.Add(newBusy);
                                addBusy.TuteeBusyTimes.Add(newLink);
                            }
                            addBusy.SaveChanges();
                        }
                    }
                    //addBusy.SaveChanges();
                }

                if (time.Marked == 2)
                {
                    addBusy.BusyTimes.Where(bus => bus.Date == time.Date && bus.Time == time.Time).Load();
                    if (isTutor)
                    {
                        addBusy.TutorBusyTimes.Where(tutBus => tutBus.Tutor.UserName == user.UserName
                                                     && tutBus.BusyTime.Date == time.Date
                                                     && tutBus.BusyTime.Time == time.Time).Load();
                        //int difference = DayOfWeekNumber(time.Date.DayOfWeek) - DayOfWeekNumber(start.DayOfWeek);
                        //start = start.AddDays(difference);
                        var tutBusies = from i in addBusy.TutorBusyTimes
                                        where i.Tutor.UserName == user.UserName &&
                                              //i.BusyTime.Date.DayOfWeek == time.Date.DayOfWeek &&
                                              DbFunctions.DiffDays(time.Date, i.BusyTime.Date) % 7 == 0 &&
                                              i.BusyTime.Time == time.Time
                                        select i;
                        foreach (var oneBusy in tutBusies)
                        {
                            addBusy.TutorBusyTimes.Remove(oneBusy);
                            var busies = from i in addBusy.BusyTimes
                                         where oneBusy.BusyTime == i
                                         select i;
                            var toRemove = busies.FirstOrDefault();
                            if (toRemove != null) addBusy.BusyTimes.Remove(toRemove);

                        }

                        var baseBusies = from i in addBusy.BaseSchedules
                                         where i.Tutor.UserName == user.UserName
                                               && i.BusyTime.Time == time.Time
                                               && i.BusyTime.Date == time.Date
                                         select i;
                        addBusy.BaseSchedules.Remove(baseBusies.FirstOrDefault());
                    }
                    else
                    {
                        addBusy.TuteeBusyTimes.Where(tutBus => tutBus.Tutee.Username == user.UserName
                                                     && tutBus.BusyTime.Date == time.Date
                                                     && tutBus.BusyTime.Time == time.Time).Load();
                        var tutBusies = from i in addBusy.TuteeBusyTimes
                                        where i.Tutee.Username == user.UserName &&
                                              //i.BusyTime.Date.DayOfWeek == time.Date.DayOfWeek &&
                                              DbFunctions.DiffDays(time.Date, i.BusyTime.Date) % 7 == 0 &&
                                              i.BusyTime.Time == time.Time
                                        select i;
                        foreach (var oneBusy in tutBusies)
                        {
                            addBusy.TuteeBusyTimes.Remove(oneBusy);
                            var busies = from i in addBusy.BusyTimes
                                         where oneBusy.BusyTime == i
                                         select i;
                            var toRemove = busies.FirstOrDefault();
                            if(toRemove != null) addBusy.BusyTimes.Remove(toRemove);
                            
                        }

                        var baseBusies = from i in addBusy.BaseSchedules
                                         where i.Tutee.Username == user.UserName
                                               && i.BusyTime.Time == time.Time
                                               && i.BusyTime.Date == time.Date
                                         select i;
                        addBusy.BaseSchedules.Remove(baseBusies.FirstOrDefault());
                    }
                    addBusy.SaveChanges();
                }
            }

        }

        /// <summary>
        /// Intercepts a click on a time of the week
        /// </summary>
        /// <param name="obj">The time clicked</param>
        private void onCurrentTimeClicked(object obj)
        {
            //Records the selected, time, date and index in times
            selectedTime = (obj as Timeslot).Time;
            selectedDate = (obj as Timeslot).Date;
            selectedIndex = times.IndexOf(obj as Timeslot);
            //Raises the TimeClick event to expose time click to the parent window
            RaiseEvent(new RoutedEventArgs(TimeClickEvent));
        }

        /// <summary>
        /// Intercepts a click on a dow of the week
        /// </summary>
        /// <param name="obj">Object clicked</param>
        private void onDOWClicked(object obj)
        {
            selectedDOW = obj as string;
            switch(obj as string){
                case "Sun":
                    selectedIndex = 0;
                    break;
                case "Mon":
                    selectedIndex = 1;
                    break;
                case "Tue":
                    selectedIndex = 2;
                    break;
                case "Wed":
                    selectedIndex = 3;
                    break;
                case "Thu":
                    selectedIndex = 4;
                    break;
                case "Fri":
                    selectedIndex = 5;
                    break;
                case "Sat":
                    selectedIndex = 6;
                    break;

            }
            RaiseEvent(new RoutedEventArgs(DOWClickEvent));
        }

        /// <summary>
        /// Returns day of week number
        /// </summary>
        /// <param name="dow">Day of week to convert</param>
        /// <returns></returns>
        private static int DayOfWeekNumber(DayOfWeek dow)
        {
            return Convert.ToInt32(dow.ToString("D"));
        }

        /// <summary>
        /// ToString for just dates
        /// </summary>
        /// <param name="date">DateTime to convert</param>
        /// <returns></returns>
        private string dateString(DateTime date)
        {
            return date.Month.ToString() + "/" + date.Day.ToString();
        }
    }
}
