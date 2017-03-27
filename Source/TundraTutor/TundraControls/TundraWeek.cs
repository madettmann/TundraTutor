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

        TutoringDB.Tutee user;

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

            user = new TutoringDB.Tutee();
            user.Username = "jim";

            DayNames = new ObservableCollection<string> { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" };

            times = new ObservableCollection<Timeslot>();
            BuildWeek( CurrentDate );
        }

        public void BuildWeek(DateTime startDate)
        {
            times.Clear();

            DateTime endDate = startDate.AddDays(7);
            TutoringDB.TutorDatabaseEntities tutorAppointments = new TutoringDB.TutorDatabaseEntities();
            TutoringDB.TutorDatabaseEntities tutorBusyTime = new TutoringDB.TutorDatabaseEntities();

            //Load appointments
            tutorAppointments.TutorTuteeCourseAppointments
                .Where(apt => (apt.Tutee.Username == user.Username || apt.Tutor.UserName == user.Username) && apt.Appointment.Date.Month == CurrentDate.Month
                                && (apt.Appointment.Date >= CurrentDate && apt.Appointment.Date <= endDate))
                .OrderBy(apt => apt.Appointment.Date)
                .ThenBy(apt => apt.Appointment.Time)
                .Load();

            //Load busy times
            tutorBusyTime.TutorBusyTimes
                .Where(busy => busy.BusyTime.Date >= startDate && busy.BusyTime.Date <= endDate)
                .OrderBy(busy => busy.BusyTime.Date)
                .ThenBy(busy => busy.BusyTime.Time)
                .Load();

            if (tutorBusyTime.TuteeBusyTimes.Count() == 0) // <----Will this work??????????
            {
                tutorBusyTime.TuteeBusyTimes
                .Where(busy => busy.BusyTime.Date >= startDate && busy.BusyTime.Date <= endDate)
                .OrderBy(busy => busy.BusyTime.Date)
                .ThenBy(busy => busy.BusyTime.Time)
                .Load();
            }
            

            DateTime d = CurrentDate;
            TimeSpan t = new TimeSpan(8, 00, 00);

            //24 timeslots (08:00 to 20:00) * 7 days in a week
            for (int box = 0; box < 168; box++)
            {
                Timeslot time = new Timeslot(d, t, ( t < CurrentTime && d == CurrentDate ) || d < CurrentDate);
                //Add appointments from database to weekview
                var dayAppointments = from i in tutorAppointments.TutorTuteeCourseAppointments
                                      where i.Appointment.Date.Day == time.Date.Day && i.Appointment.Time == time.Time
                                      orderby i.Appointment.Time
                                      select i;

                time.Appointment = new Appointment(); //Might not need to, to check null after adding the busy time
                foreach (var appt in dayAppointments)
                {
                    time.Appointment = new Appointment(appt.Tutor.UserName, appt.Tutee.Username, appt.Appointment.Time, "Not implemented");
                }
                //Must assign timeinfo to either the busy or appointment, depending
            }
        }

        private static int DayOfWeekNumber(DayOfWeek dow)
        {
            return Convert.ToInt32(dow.ToString("D"));
        }
    }
}
