//Written by Victor
using System.Linq;
using System.Data.Entity;
using System.Windows;
using System.ComponentModel;
using System;

namespace TutorWindows
{
    public struct ApptInfo : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {

            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private string appointmentTime;
        private string appointmentDuration;
        private string appointmentDate;
        private string appointmentTutor;
        private string appointmentCourse;

        public string AppointmentTime { get => appointmentTime; set { appointmentTime = value; NotifyPropertyChanged("AppointmentTime"); } }
        public string AppointmentDuration { get => appointmentDuration; set { appointmentDuration = value; NotifyPropertyChanged("AppointmentDuration"); } }
        public string AppointmentDate { get => appointmentDate; set { appointmentDate = value; NotifyPropertyChanged("AppointmentDate"); } }
        public string AppointmentTutor { get => appointmentTutor; set { appointmentTutor = value; NotifyPropertyChanged("AppointmentTutor"); } }
        public string AppointmentCourse { get => appointmentCourse; set { appointmentCourse = value; NotifyPropertyChanged("AppointmentCourse"); } }
    }
    /// <summary>
    /// Interaction logic for AppointmentInfo.xaml
    /// </summary>
    public partial class AppointmentInfo : TundraControls.CustomWindow
    {
        ApptInfo appointmentInfo;
        private TutoringDB.Tutee user;
        TutoringDB.Appointment appt;
        TutoringDB.TutorDatabaseEntities readUser;
        TutoringDB.TutorTuteeCourseAppointment link;

        public AppointmentInfo(DateTime date, TimeSpan time)
        {
            //Get the current user
            
            user = new TutoringDB.Tutee();
            readUser = new TutoringDB.TutorDatabaseEntities();
            readUser.CurrentUsers.Load();
            var multipleUsers = from i in readUser.CurrentUsers select i;
            foreach (var oneUser in multipleUsers) { user.Username = oneUser.UserName; }

            readUser.Appointments.Load();
            var appointments = from i in readUser.Appointments
                               where i.Time == time && i.Date == date
                               select i;
            appt = appointments.FirstOrDefault();
            appointmentInfo.AppointmentTime = appt.Time.Hours + ":" + appt.Time.Minutes + ":00";
            appointmentInfo.AppointmentDuration = appt.Duration.ToString();
            appointmentInfo.AppointmentDate = appt.Date.Month + "/" + appt.Date.Day + "/" + appt.Date.Year;
            link = appt.TutorTuteeCourseAppointments.Where(appoin => appoin.Tutee.Username == user.Username
                                                                                        && appoin.Appointment.Time == time
                                                                                        && appoin.Appointment.Date == date).FirstOrDefault();
            appointmentInfo.AppointmentCourse = link.Cours.CourseName + " (" + link.Cours.CourseNumber + ")";
            appointmentInfo.AppointmentTutor = link.Tutor.FirstName + " " + link.Tutor.LastName;

            DataContext = appointmentInfo;

            InitializeComponent();
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Are you sure?", "Delete Confirmation", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                readUser.Appointments.Remove(appt);
                readUser.TutorTuteeCourseAppointments.Remove(link);
                readUser.SaveChanges();
                Close();
            }
        }
    }
}
