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
        //Fields
        private List<Appointment> appointmentBlocks;
        private List<Busy> busyBlocks;
        private TutoringDB.CurrentUser user;
        private bool isTutor;
        private DateTime CurrentDate { get => (DateTime)GetValue(CurrentDateProperty); set => SetValue(CurrentDateProperty, value); }
        private TimeSpan CurrentTime { get => (TimeSpan)GetValue(CurrentTimeProperty); set => SetValue(CurrentTimeProperty, value); }

        //Properties
        public ObservableCollection<Timeslot> times { get; set; }
        public ObservableCollection<string> DayNames { get; set; }
        public static readonly DependencyProperty CurrentDateProperty = DependencyProperty.Register("CurrentDate", typeof(DateTime), typeof(TundraWeek));
        public static readonly DependencyProperty CurrentTimeProperty = DependencyProperty.Register("CurrentTime", typeof(TimeSpan), typeof(TundraWeek));
        

        static TundraWeek()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TundraWeek), new FrameworkPropertyMetadata(typeof(TundraWeek)));
        }

        public TundraWeek()
        {
            DataContext = this;
            CurrentDate = DateTime.Today;
            CurrentTime = DateTime.Today.TimeOfDay;

            

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

            DateTime endDate = startDate.AddDays(6);

            #region Day Name Labeling

            //Align day names
            DayNames = new ObservableCollection<string> { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" };
            int dow = (int)startDate.DayOfWeek;
            switch (dow)
            {
                case 0:
                    DayNames = new ObservableCollection<string> { "Sun " + dateString(startDate), "Mon " + dateString(startDate.AddDays(1)),
                                                                "Tue " + dateString(startDate.AddDays(2)), "Wed " + dateString(startDate.AddDays(3)),
                                                                "Thu " + dateString(startDate.AddDays(4)), "Fri " + dateString(startDate.AddDays(5)),
                                                                "Sat " + dateString(endDate) };
                    break;
                case 1:
                    DayNames = new ObservableCollection<string> { "Mon " + dateString(startDate), "Tue " + dateString(startDate.AddDays(1)),
                                                                "Wed " + dateString(startDate.AddDays(2)), "Thu " + dateString(startDate.AddDays(3)),
                                                                "Fri " + dateString(startDate.AddDays(4)), "Sat " + dateString(startDate.AddDays(5)),
                                                                "Sun " + dateString(endDate) };
                    break;
                case 2:
                    DayNames = new ObservableCollection<string> { "Tue " + dateString(startDate), "Wed " + dateString(startDate.AddDays(1)),
                                                                "Thu " + dateString(startDate.AddDays(2)), "Fri " + dateString(startDate.AddDays(3)),
                                                                "Sat " + dateString(startDate.AddDays(4)), "Sun " + dateString(startDate.AddDays(5)),
                                                                "Mon " + dateString(endDate) };
                    break;
                case 3:
                    DayNames = new ObservableCollection<string> { "Wed " + dateString(startDate), "Thu " + dateString(startDate.AddDays(1)),
                                                                "Fri " + dateString(startDate.AddDays(2)), "Sat " + dateString(startDate.AddDays(3)),
                                                                "Sun " + dateString(startDate.AddDays(4)), "Mon " + dateString(startDate.AddDays(5)),
                                                                "Tue " + dateString(endDate) };
                    break;
                case 4:
                    DayNames = new ObservableCollection<string> { "Thu " + dateString(startDate), "Fri " + dateString(startDate.AddDays(1)),
                                                                "Sat " + dateString(startDate.AddDays(2)), "Sun " + dateString(startDate.AddDays(3)),
                                                                "Mon " + dateString(startDate.AddDays(4)), "Tue " + dateString(startDate.AddDays(5)),
                                                                "Wed " + dateString(endDate) };
                    break;
                case 5:
                    DayNames = new ObservableCollection<string> { "Fri " + dateString(startDate), "Sat " + dateString(startDate.AddDays(1)),
                                                                "Sun " + dateString(startDate.AddDays(2)), "Mon " + dateString(startDate.AddDays(3)),
                                                                "Tue " + dateString(startDate.AddDays(4)), "Wed " + dateString(startDate.AddDays(5)),
                                                                "Thu " + dateString(endDate) };
                    break;
                case 6:
                    DayNames = new ObservableCollection<string> { "Sat " + dateString(startDate), "Sun " + dateString(startDate.AddDays(1)),
                                                                "Mon " + dateString(startDate.AddDays(2)), "Tue " + dateString(startDate.AddDays(3)),
                                                                "Wed " + dateString(startDate.AddDays(4)), "Thu " + dateString(startDate.AddDays(5)),
                                                                "Fri " + dateString(endDate) };
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
            //Load busy times <---------------- SOMETIMES BUSIES JUST DON'T GET SHOWN ... TRY JIM (IF STILL APPLICABLE)
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
                foreach (var onebusy in busies) busyList.Add(new Busy(onebusy.BusyTime.Time, onebusy.BusyTime.Duration, onebusy.BusyTime.Date));
            }
            else
            {
                var busies = from i in tutorBusyTime.TuteeBusyTimes
                             where (i.BusyTime.Date.Month == startDate.Date.Month || i.BusyTime.Date.Month == startDate.Date.Month + 1)
                                    && (i.Tutee.Username == user.UserName)
                             orderby i.BusyTime.Date, i.BusyTime.Time
                             select i;
                //Add all the busies to a list (to avoid type conflict)
                foreach (var onebusy in busies) busyList.Add(new Busy(onebusy.BusyTime.Time, onebusy.BusyTime.Duration, onebusy.BusyTime.Date));
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
                                                      appt.Appointment.Time, "Not Implemented", appt.Appointment.Date));
                //Find how many 30-minute timeslots the appointment takes up
                int numMore = (appt.Appointment.Duration.Value.Hours * 2) + (appt.Appointment.Duration.Value.Minutes / 30) - 1;
                for (int i = 0; i < numMore; i++)
                {
                    //Add the appointment in 30-minute blocks to the list
                    appointmentBlocks.Add(new Appointment(appt.Tutor.FirstName + appt.Tutor.LastName,
                                                      appt.Tutee.FirstName + appt.Tutee.LastName,
                                                      startTime.Add(new TimeSpan(00, (i+1)*30, 00)), "Not Implemented", appt.Appointment.Date));

                }
            }

            //Break the busies into 30-minute blocks
            foreach (var oneBusy in busyList)
            {
                //Note the start time
                TimeSpan startTime = oneBusy.Time;
                //Add the appointment
                busyBlocks.Add(new Busy(oneBusy.Time, new TimeSpan(00,30,00), oneBusy.Date));
                //Find how many 30-minute timeslots the appointment takes up
                int numMore = (oneBusy.Duration.Hours * 2) + (oneBusy.Duration.Minutes / 30) - 1;
                for (int i = 0; i < numMore; i++)
                {
                    //Add the appointment in 30-minute blocks to the list
                    busyBlocks.Add(new Busy(startTime.Add(new TimeSpan(00,(i+1)*30,00)), new TimeSpan(00, 30, 00), oneBusy.Date));

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
                foreach (var appt in appointmentBlocks) if(appt.time == t && appt.Date == d) time.Appointment.Add(appt);

                //Add busy to weekview
                time.Busy = new ObservableCollection<Busy>();
                foreach (var oneBusy in busyBlocks) if (oneBusy.Time == t && oneBusy.Date == d) time.Busy.Add(oneBusy);

                //Add timeslot to the collection so it appears in the weekview
                times.Add(time);

                //Increment time
                if(t.Hours == 19 && t.Minutes == 30)
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

        private static int DayOfWeekNumber(DayOfWeek dow)
        {
            return Convert.ToInt32(dow.ToString("D"));
        }

        private string dateString(DateTime date)
        {
            return date.Month.ToString() + "/" + date.Day.ToString();
        }
    }
}
