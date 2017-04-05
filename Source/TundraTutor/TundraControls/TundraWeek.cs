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
        private List<Appointment> appointmentBlocks;
        private List<Busy> busyBlocks;
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

            #region Day Name Labeling
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
            #endregion

            #region Database Loading

            TutoringDB.TutorDatabaseEntities tutorAppointments = new TutoringDB.TutorDatabaseEntities();
            TutoringDB.TutorDatabaseEntities tutorBusyTime = new TutoringDB.TutorDatabaseEntities();

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
            }

            //Add appointments from database to a var, filter out unecessary ones
            var dayAppointments = from i in tutorAppointments.TutorTuteeCourseAppointments
                                  where ( (i.Appointment.Date.Month == startDate.Date.Month || i.Appointment.Date.Month == startDate.Date.Month + 1 )
                                  && (i.Tutee.Username == user.UserName || i.Tutor.UserName == user.UserName))
                                  orderby i.Appointment.Date, i.Appointment.Time
                                  select i;

            //List for use in blocking
            List<Busy> busyList = new List<Busy>();
            //Add busies from database to a var, filter out unecessary ones
            if (isTutor)
            {
                var busies = from i in tutorBusyTime.TutorBusyTimes
                             where (i.BusyTime.Date.Month == startDate.Date.Month || i.BusyTime.Date.Month == startDate.Date.Month + 1)
                                    && (i.Tutor.UserName == user.UserName)
                             orderby i.BusyTime.Date, i.BusyTime.Time
                             select i;
                //Add all the busies to a list (to avoid type conflict)
                foreach (var onebusy in busies) busyList.Add(new Busy(onebusy.BusyTime.Time, onebusy.BusyTime.Duration));
            }
            else
            {
                var busies = from i in tutorBusyTime.TuteeBusyTimes
                             where (i.BusyTime.Date.Month == startDate.Date.Month || i.BusyTime.Date.Month == startDate.Date.Month + 1)
                                    && (i.Tutee.Username == user.UserName)
                             orderby i.BusyTime.Date, i.BusyTime.Time
                             select i;
                //Add all the busies to a list (to avoid type conflict)
                foreach (var onebusy in busies) busyList.Add(new Busy(onebusy.BusyTime.Time, onebusy.BusyTime.Duration));
            }
#endregion

            

            #region Time Blocking
            //Keep track of the busies and appts as time blocks
            appointmentBlocks = new List<Appointment>();
            busyBlocks = new List<Busy>();

            

            //Break the appts into 30-minute blocks
            foreach (var appt in dayAppointments)
            {
                //Note the start time
                TimeSpan startTime = appt.Appointment.Time;
                //Add the appointment
                appointmentBlocks.Add(new Appointment(appt.Tutor.FirstName + appt.Tutor.LastName,
                                                      appt.Tutee.FirstName + appt.Tutee.LastName,
                                                      appt.Appointment.Time, "Not Implemented"));
                //Find how many 30-minute timeslots the appointment takes up
                int numMore = (appt.Appointment.Duration.Value.Hours * 2) + (appt.Appointment.Duration.Value.Minutes / 30) - 1;
                for (int i = 0; i < numMore; i++)
                {
                    //Add the appointment in 30-minute blocks to the list
                    appointmentBlocks.Add(new Appointment(appt.Tutor.FirstName + appt.Tutor.LastName,
                                                      appt.Tutee.FirstName + appt.Tutee.LastName,
                                                      startTime.Add(new TimeSpan(00, 30, 00)), "Not Implemented"));

                }
            }

            //Break the busies into 30-minute blocks
            foreach (var oneBusy in busyList)
            {
                //Note the start time
                TimeSpan startTime = oneBusy.Time;
                //Add the appointment
                busyBlocks.Add(new Busy(oneBusy.Time, new TimeSpan(00,30,00)));
                //Find how many 30-minute timeslots the appointment takes up
                int numMore = (oneBusy.Duration.Hours * 2) + (oneBusy.Duration.Minutes / 30) - 1;
                for (int i = 0; i < numMore; i++)
                {
                    //Add the appointment in 30-minute blocks to the list
                    busyBlocks.Add(new Busy(startTime.Add(new TimeSpan(00,30,00)), new TimeSpan(00, 30, 00)));

                }
            }

            #endregion

            //Set the first day to generate
            DateTime d = new DateTime(startDate.Year, startDate.Month, startDate.Day);
            TimeSpan t = new TimeSpan(08, 00, 00);

            #region Week Generation
            //24 timeslots (08:00 to 20:00) * 7 days in a week
            for (int box = 0; box < 168; box++)
            {
                Timeslot time = new Timeslot(d, t, (( t < CurrentTime && d == CurrentDate ) || d < CurrentDate) );

                //Add appointments to weekview
                time.Appointment = new ObservableCollection<Appointment>();
                foreach (var appt in appointmentBlocks) if(appt.time == t) time.Appointment.Add(appt);

                //Add busy to weekview
                time.Busy = new ObservableCollection<Busy>();
                foreach (var oneBusy in busyBlocks) if (oneBusy.Time == t) time.Busy.Add(oneBusy);

                //Add timeslot to the collection so it appears in the weekview
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
#endregion
        }

        private static int DayOfWeekNumber(DayOfWeek dow)
        {
            return Convert.ToInt32(dow.ToString("D"));
        }
    }
}
