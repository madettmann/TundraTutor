using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DisplayTables;

namespace TutorWindows
{
    /// <summary>
    /// Interaction logic for AddBusy.xaml
    /// </summary>
    public partial class ScheduleAppointment : TundraControls.CustomWindow
    {
        private DateTime timeSelected;
        private DateTime timeSpan;
        TutoringDB.TutorDatabaseEntities db = new TutoringDB.TutorDatabaseEntities();

        public ScheduleAppointment()
        {
            InitializeComponent();
            dateDatePicker.DisplayDate = DateTime.Today;
        }

        private void submitButton_Click(object sender, RoutedEventArgs e)
        {
            if ((string)durationTimeMenu.Tag == "true" && (string)startTimeMenu.Tag == "true")
            {
                db.Appointments.Load();
                db.CurrentUsers.Load();
                TutoringDB.Appointment tempAppointment = new TutoringDB.Appointment();
                tempAppointment.Date = (DateTime)dateDatePicker.SelectedDate;
                tempAppointment.Time = timeSelected.TimeOfDay;
                tempAppointment.Id = db.Appointments.Count();
                tempAppointment.Duration = timeSpan.TimeOfDay;
                db.Appointments.Add(tempAppointment);

                db.Tutors.Load();
                db.TutorTuteeCourseAppointments.Load();
                TutoringDB.TutorTuteeCourseAppointment joiner = new TutoringDB.TutorTuteeCourseAppointment();
                joiner.AppointmentId = tempAppointment.Id;
                TutoringDB.Tutee tutee = db.Tutees.Where(i => i.Username == db.CurrentUsers.FirstOrDefault().UserName).First();
                joiner.TuteeId = tutee.Id;
                joiner.Id = db.TutorTuteeCourseAppointments.Count();
                joiner.Cours = courseComboBox.SelectedItem as TutoringDB.Cours;
                joiner.Tutor = tutorNameComboBox.SelectedItem as TutoringDB.Tutor;
                db.TutorTuteeCourseAppointments.Add(joiner);

                db.SaveChanges();
                this.Close();
            }
            else
            {
                MessageBox.Show("All Fields Must be Filled to Schedule Appointment");
            }
        }
        private void TimeSelected(object sender, RoutedEventArgs e)
        {
            MenuItem m = sender as MenuItem;
            timeSelected = Convert.ToDateTime(m.Tag);
            selectItemMenuItem.Header = timeSelected.TimeOfDay;
            startTimeMenu.Tag = "true";
        }

        private void DurationSelected(object sender, RoutedEventArgs e)
        {
            MenuItem m = sender as MenuItem;
            timeSpan = Convert.ToDateTime(m.Tag);
            selectDurationMenuItem.Header = timeSpan.TimeOfDay;
            durationTimeMenu.Tag = "true";
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void dateLabel_Loaded(object sender, RoutedEventArgs e)
        {
            db.Courses.Load();
            db.Tutors.Load();
            System.Windows.Data.CollectionViewSource tutorViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("tutorViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // tutorViewSource.Source = [generic data source]
            tutorViewSource.Source = db.Tutors.Local;
            System.Windows.Data.CollectionViewSource coursViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("coursViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // coursViewSource.Source = [generic data source]
            coursViewSource.Source = db.Courses.Local;
        }

        private void TundraButton_Click(object sender, RoutedEventArgs e)
        {
            if ((string)durationTimeMenu.Tag == "true" && (string)startTimeMenu.Tag == "true")
            {
                db.Appointments.Load();
                db.CurrentUsers.Load();
                TutoringDB.Appointment tempAppointment = new TutoringDB.Appointment();
                tempAppointment.Date = (DateTime)dateDatePicker.SelectedDate;
                tempAppointment.Time = timeSelected.TimeOfDay;
                tempAppointment.Id = db.Appointments.Count();
                tempAppointment.Duration = timeSpan.TimeOfDay;
                db.Appointments.Add(tempAppointment);

                db.Tutors.Load();
                db.TutorTuteeCourseAppointments.Load();
                TutoringDB.TutorTuteeCourseAppointment joiner = new TutoringDB.TutorTuteeCourseAppointment();
                joiner.AppointmentId = tempAppointment.Id;
                TutoringDB.Tutee tutee = db.Tutees.Where(i => i.Username == db.CurrentUsers.FirstOrDefault().UserName).First();
                joiner.TuteeId = tutee.Id;
                joiner.Id = db.TutorTuteeCourseAppointments.Count();
                joiner.Cours = courseComboBox.SelectedItem as TutoringDB.Cours;
                joiner.Tutor = db.Tutors.First();
                db.TutorTuteeCourseAppointments.Add(joiner);

                db.SaveChanges();
                this.Close();
            }
            else
            {
                MessageBox.Show("Time, Duration, and Course must be Selected");
            }
        }
    }
}
