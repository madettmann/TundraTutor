using System;
using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Collections.Generic;
namespace TundraControls
{
    public class TundraWeek : Control
    {
        public ObservableCollection<Timeslot> times { get; set; }
        public ObservableCollection<string> DayNames { get; set; }
        public static readonly DependencyProperty CurrentDateProperty = DependencyProperty.Register("CurrentDate", typeof(DateTime), typeof(TundraWeek));
        public static readonly DependencyProperty CurrentTimeProperty = DependencyProperty.Register("CurrentTime", typeof(TimeSpan), typeof(TundraWeek));
        bool isTutor;

        TutoringDB.CurrentUser user;

        public DateTime CurrentDate
        {
            get { return (DateTime)GetValue(CurrentDateProperty);}
            set { SetValue(CurrentDateProperty, value);}
        }

        public TimeSpan CurrentTime { get => (TimeSpan)GetValue(CurrentTimeProperty); set => SetValue(CurrentTimeProperty, value); }

        static TundraWeek()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TundraWeek), new FrameworkPropertyMetadata(typeof(TundraWeek)));
        }

        public TundraWeek()
        {
            DataContext = this;
            CurrentDate = DateTime.Today;
            CurrentTime = DateTime.Today.TimeOfDay;

            DayNames = new ObservableCollection<string> { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" };

            //Get the current user
            user = new TutoringDB.CurrentUser();
            var readUser = new TutoringDB.TutorDatabaseEntities();
            readUser.CurrentUsers.Load();
            var userList = from i in readUser.CurrentUsers select i;
            TutoringDB.Tutee oneUser = new TutoringDB.Tutee();
            foreach (var element in userList) { oneUser.Username = element.UserName; }
            user.UserName = oneUser.Username;

            

            times = new ObservableCollection<Timeslot>();
            BuildWeek( CurrentDate );
        }

        public void BuildWeek(DateTime startDate)
        {
            times.Clear();

            DateTime endDate = startDate.AddDays(7);
            TutoringDB.TutorDatabaseEntities tutorAppointments = new TutoringDB.TutorDatabaseEntities();
            TutoringDB.TutorDatabaseEntities tutorBusyTime = new TutoringDB.TutorDatabaseEntities();

            //Align day names
            int dow = (int)startDate.DayOfWeek;
            switch (dow)
            {
                case 1:
                    DayNames = new ObservableCollection<string> { "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" };
                    break;
                case 2:
                    DayNames = new ObservableCollection<string> { "Tue", "Wed", "Thu", "Fri", "Sat", "Sun", "Mon" };
                    break;
                case 3:
                    DayNames = new ObservableCollection<string> { "Wed", "Thu", "Fri", "Sat", "Sun", "Mon", "Tue" };
                    break;
                case 4:
                    DayNames = new ObservableCollection<string> { "Thu", "Fri", "Sat", "Sun", "Mon", "Tue", "Wed" };
                    break;
                case 5:
                    DayNames = new ObservableCollection<string> { "Fri", "Sat", "Sun", "Mon", "Tue", "Wed", "Thu" };
                    break;
                case 6:
                    DayNames = new ObservableCollection<string> { "Sat", "Sun", "Mon", "Tue", "Wed", "Thu", "Fri"};
                    break;
                default:
                    break;

            };

            //Load appointments
            tutorAppointments.TutorTuteeCourseAppointments
                .Load();

            isTutor = user.Type == "tutor";
            //Load busy times <----------- this might have the same problem as appointments (being non-user-specific) - LOOK HERE IF SO!
            if(isTutor)
            tutorBusyTime.TutorBusyTimes
                .Where(busy => busy.BusyTime.Date >= startDate && busy.BusyTime.Date <= endDate)
                .OrderBy(busy => busy.BusyTime.Date)
                .ThenBy(busy => busy.BusyTime.Time)
                .Load();
            else
            {
                tutorBusyTime.TuteeBusyTimes
                .Where(busy => busy.BusyTime.Date >= startDate && busy.BusyTime.Date <= endDate)
                .OrderBy(busy => busy.BusyTime.Date)
                .ThenBy(busy => busy.BusyTime.Time)
                .Load();
                isTutor = false;
            }

            //Set the first day to generate
            DateTime d = new DateTime(startDate.Year, startDate.Month, startDate.Day);
            TimeSpan t = new TimeSpan(08, 00, 00);

            //24 timeslots (08:00 to 20:00) * 7 days in a week
            for (int box = 0; box < 168; box++)
            {
                Timeslot time = new Timeslot(d, t, (( t < CurrentTime && d == CurrentDate ) || d < CurrentDate) );

                //Add appointments from database to weekview
                var dayAppointments = from i in tutorAppointments.TutorTuteeCourseAppointments
                                      where ( i.Tutee.Username == user.UserName || i.Tutor.UserName == user.UserName )
                                            && i.Appointment.Date.Day == time.Date.Day && i.Appointment.Time == time.Time
                                      orderby i.Appointment.Time
                                      select i;

                time.Appointment = new ObservableCollection<Appointment>(); //Might not need to, to check null after adding the busy time
                foreach (var appt in dayAppointments)
                {
                    time.Appointment.Add(new Appointment(appt.Tutor.UserName, appt.Tutee.Username, appt.Appointment.Time, "Not implemented"));
                }
                //Must assign timeinfo to either the busy or appointment, depending

                //Add whether the time is busy or not (although it never will be if there's an appointment but ... whatever)
                time.Busy = new ObservableCollection<Busy>();
                if (isTutor)
                {
                    var dayBusy = from i in tutorBusyTime.TutorBusyTimes
                                  where i.BusyTime.Time == time.Date.TimeOfDay
                                  select i;
                    foreach (var bus in dayBusy)
                    {
                        time.Busy.Add(new Busy());//Add new constructor to busy? Or no?
                    }
                }
                else
                {
                    var dayBusy = from i in tutorBusyTime.TuteeBusyTimes
                                  where i.BusyTime.Time == time.Date.TimeOfDay
                                  select i;
                    foreach (var bus in dayBusy)
                    {
                        time.Busy.Add(new Busy());//Add new constructor to busy? Or no?
                    }
                }

                times.Add(time);

                //Increment time
                if(t.Hours == 20)
                {
                    d.AddDays(1);
                    t = new TimeSpan(08, 00, 00);
                }
                else
                {
                    t = t.Add(new TimeSpan(00, 30, 00));
                }


            }
        }

        private static int DayOfWeekNumber(DayOfWeek dow)
        {
            return Convert.ToInt32(dow.ToString("D"));
        }
    }
}
