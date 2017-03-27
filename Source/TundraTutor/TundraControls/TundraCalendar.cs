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
    public class TundraCalendar : Control
    {
        public ObservableCollection<Day> Days { get; set; }
        public ObservableCollection<string> DayNames { get; set; }
        public static readonly DependencyProperty CurrentDateProperty = DependencyProperty.Register("CurrentDate", typeof(DateTime), typeof(TundraCalendar));
        private TutoringDB.TutorDatabaseEntities readUser;
        private TutoringDB.Tutee user;

        public event EventHandler<DayChangedEventArgs> DayChanged;

        public DateTime CurrentDate
        {
            get { return (DateTime)GetValue(CurrentDateProperty); }
            set { SetValue(CurrentDateProperty, value); }
        }

        static TundraCalendar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TundraCalendar), new FrameworkPropertyMetadata(typeof(TundraCalendar)));
        }

        public TundraCalendar()
        {
            DataContext = this;
            CurrentDate = DateTime.Today;

            DayNames = new ObservableCollection<string> { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" };

            //Get the current user
            user = new TutoringDB.Tutee();
            readUser = new TutoringDB.TutorDatabaseEntities();
            readUser.CurrentUsers.Load();
            var userList = from i in readUser.CurrentUsers select i;
            foreach (var oneUser in userList) { user.Username = oneUser.UserName; }
            
            Days = new ObservableCollection<Day>();
            BuildCalendar(DateTime.Today);
        }

        public void BuildCalendar(DateTime targetDate)
        {
            Days.Clear();
            

            TutoringDB.TutorDatabaseEntities tutorSchedule = new TutoringDB.TutorDatabaseEntities();

            tutorSchedule.TutorTuteeCourseAppointments
                .Where(apt => (apt.Tutee.Username == user.Username || apt.Tutor.UserName == user.Username) && apt.Appointment.Date.Month == CurrentDate.Month)
                .OrderBy(apt => apt.Appointment.Date)
                .ThenBy(apt => apt.Appointment.Time)
                .Load();

            //Calculate when the first day of the month is and work out an 
            //offset so we can fill in any boxes before that.
            DateTime d = new DateTime(targetDate.Year, targetDate.Month, 1);
            int offset = DayOfWeekNumber(d.DayOfWeek);
            if (offset != 0/*1?*/)
                d = d.AddDays(-offset);

            //Show 6 weeks each with 7 days = 42
            for (int box = 1; box <= 42; box++)
            {
                Day day = new Day { Date = d, Enabled = true, IsTargetMonth = targetDate.Month == d.Month };
                day.PropertyChanged += day_Changed;
                day.IsToday = d == DateTime.Today;

                //Add appointments from database to calendar
                var dayAppointments = from i in tutorSchedule.TutorTuteeCourseAppointments
                                      where i.Appointment.Date.Day == day.Date.Day
                                      orderby i.Appointment.Time
                                      select i;
                day.appointmentList = new ObservableCollection<Appointment>();
                foreach (var appointment in dayAppointments) day.appointmentList.Add(new Appointment(appointment.Tutor.FirstName + appointment.Tutor.LastName,
                                                                                       appointment.Tutor.FirstName + appointment.Tutor.LastName,
                                                                                       appointment.Appointment.Time,
                                                                                       "Not Implemented"));


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

        private void day_Changed(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != "Notes") return;
            if (DayChanged == null) return;

            DayChanged(this, new DayChangedEventArgs(sender as Day));   
        }

        private static int DayOfWeekNumber(DayOfWeek dow)
        {
            return Convert.ToInt32(dow.ToString("D"));
        }
    }

    public class DayChangedEventArgs : EventArgs
    {
        public Day Day { get; private set; }

        public DayChangedEventArgs(Day day)
        {
            this.Day = day;
        }
    }
}
