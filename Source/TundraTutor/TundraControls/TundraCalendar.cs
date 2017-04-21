//Written by Victor
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
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;

namespace TundraControls
{
    public class TundraCalendar : Control
    {
        #region Fields
        private TutoringDB.TutorDatabaseEntities readUser;
        private TutoringDB.CurrentUser user;
        private DateTime selectedDate;
        private int selectedIndex;
        #endregion

        #region Properties
        public ObservableCollection<Day> Days { get; set; }
        public ObservableCollection<string> DayNames { get; set; }
        public static readonly DependencyProperty CurrentDateProperty = DependencyProperty.Register("CurrentDate", typeof(DateTime), typeof(TundraCalendar));
        public DateTime CurrentDate{ get => (DateTime)GetValue(CurrentDateProperty); set => SetValue(CurrentDateProperty, value); }
        public DateTime SelectedDate { get => selectedDate; set => selectedDate = value; }
        public int SelectedIndex { get => selectedIndex; set => selectedIndex = value; }
        #endregion

        #region Events
        public ICommand dayClickedCommand { get; private set; }
        public event EventHandler<DayChangedEventArgs> DayChanged;
        public static RoutedEvent DayClickEvent = EventManager.RegisterRoutedEvent("DayClick", RoutingStrategy.Bubble, 
                                                                                    typeof(RoutedEventHandler), typeof(TundraCalendar));
        public event RoutedEventHandler DayClick { add => AddHandler(DayClickEvent, value); remove => RemoveHandler(DayClickEvent, value); }
        #endregion


        static TundraCalendar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TundraCalendar), new FrameworkPropertyMetadata(typeof(TundraCalendar)));
        }

        /// <summary>
        /// Constructor for calendar
        /// </summary>
        public TundraCalendar()
        {
            DataContext = this;
            CurrentDate = DateTime.Today;
            dayClickedCommand = new RelayCommand<object>(onCurrentDayClicked); //Relay command is a common implementation
                                                                               //of the abstract ICommand type; I'm using
                                                                               //the version contained in the MVVMLightLibs NuGet 
                                                                               //package - it is very similar to the 
                                                                               //standard implementation popularized by MS on MSDN

            DayNames = new ObservableCollection<string> { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" };

            //Get the current user
            user = new TutoringDB.CurrentUser();
            readUser = new TutoringDB.TutorDatabaseEntities();
            readUser.CurrentUsers.Load();
            var userList = from i in readUser.CurrentUsers select i;
            TutoringDB.Tutee oneUser = new TutoringDB.Tutee();
            foreach (var element in userList) { oneUser.Username = element.UserName; }
            user.UserName = oneUser.Username;

            Days = new ObservableCollection<Day>();
            BuildCalendar(DateTime.Today);
        }

        
        /// <summary>
        /// Builds the calendar for the selected month
        /// </summary>
        /// <param name="targetDate">Month for which to generate the calendar</param>
        public void BuildCalendar(DateTime targetDate)
        {
            Days.Clear();

            TutoringDB.TutorDatabaseEntities tutorSchedule = new TutoringDB.TutorDatabaseEntities();

            tutorSchedule.TutorTuteeCourseAppointments
                .Load();

            //Calculate when the first day of the month is and work out an 
            //offset so we can fill in any boxes before that.
            DateTime d = new DateTime(targetDate.Year, targetDate.Month, 1);
            int offset = DayOfWeekNumber(d.DayOfWeek);
            if (offset != 0/*1? - no...*/)
                d = d.AddDays(-offset);

            //Show 6 weeks each with 7 days = 42
            for (int box = 1; box <= 42; box++)
            {
                Day day = new Day { Date = d, Enabled = true, IsTargetMonth = targetDate.Month == d.Month };
                day.PropertyChanged += day_Changed;
                day.IsToday = d == DateTime.Today;

                //Add appointments from database to calendar, filter out unecessary ones
                var dayAppointments = from i in tutorSchedule.TutorTuteeCourseAppointments
                                      where ( i.Appointment.Date.Month == d.Date.Month && i.Appointment.Date.Day == d.Date.Day 
                                      && (i.Tutee.Username == user.UserName || i.Tutor.UserName == user.UserName))
                                      orderby i.Appointment.Time
                                      select i;

                day.appointmentList = new ObservableCollection<Appointment>();
                foreach (var appointment in dayAppointments) day.appointmentList.Add(new Appointment(appointment.Tutor.FirstName + appointment.Tutor.LastName,
                                                                                       appointment.Tutor.FirstName + appointment.Tutor.LastName,
                                                                                       appointment.Appointment.Time,
                                                                                       appointment.Cours.CourseNumber.ToString()));


                if (day.NumAppointments > 0)
                    day.AppointmentInfo = day.appointmentList[0].apptLabel;
                else
                    day.AppointmentInfo = null;
                if (day.NumAppointments > 2)
                    day.AppointmentInfo2 = day.appointmentList[1].apptLabel + " + " + (day.NumAppointments - 2) + " more";
                else if (day.NumAppointments > 1)
                    day.AppointmentInfo2 = day.appointmentList[1].apptLabel;
                else
                    day.AppointmentInfo2 = null;

                Days.Add(day);
                //Adds the day just made to the list of Days in the constructor, which then is bound to the ItemsControl in Generic.XAML
                d = d.AddDays(1);
            }
        }

        //No longer used - should probably dispose of
        private void day_Changed(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != "Notes") return;
            if (DayChanged == null) return;

            DayChanged(this, new DayChangedEventArgs(sender as Day));   
        }

        /// <summary>
        /// Intercepts a click on a day of the month
        /// </summary>
        /// <param name="obj">The day clicked</param>
        private void onCurrentDayClicked(object obj)
        {
            selectedDate = (obj as Day).Date;
            selectedIndex = Days.IndexOf(obj as Day);
            RaiseEvent(new RoutedEventArgs(DayClickEvent));
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
    }

    //No longer used - should probably dispose of
    public class DayChangedEventArgs : EventArgs
    {
        public Day Day { get; private set; }

        public DayChangedEventArgs(Day day)
        {
            this.Day = day;
        }
    }
}
