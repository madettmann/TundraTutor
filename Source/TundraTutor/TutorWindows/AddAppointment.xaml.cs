using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

namespace TutorWindows
{
    public class AvailableTutors : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {

            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private string firstName;
        private string lastName;
        public string fullName;

        public string FirstName { get => firstName; set { firstName = value; NotifyPropertyChanged("FirstName"); } }
        public string LastName { get => lastName; set { lastName = value; NotifyPropertyChanged("FirstName"); } }
        public string FullName { get => firstName + ' ' + lastName; }
    }

    /// <summary>
    /// Interaction logic for AddAppointment.xaml
    /// </summary>
    public partial class AddAppointment : TundraControls.CustomWindow
    {
        TutoringDB.TutorDatabaseEntities db = new TutoringDB.TutorDatabaseEntities();
        public List<TutoringDB.TutorTuteeCourseAppointment> ttcaList = new List<TutoringDB.TutorTuteeCourseAppointment>();
        public int index;
        public ObservableCollection<AvailableTutors> tutorList = new ObservableCollection<AvailableTutors>();
        public TutoringDB.Cours selectedCourse = new TutoringDB.Cours();

        public AddAppointment()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            db.Courses.OrderBy(i=>i.CourseName).Load();
            var deptList = db.Courses.Select(i => i.Department).Distinct();

            System.Windows.Data.CollectionViewSource coursViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("coursViewSource")));
            //System.Windows.Data.CollectionViewSource deptViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("deptViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // coursViewSource.Source = [generic data source]
            coursViewSource.Source = db.Courses.Local;
            //deptViewSource.Source = deptList;
            
        }

        private void submitCourseButton_Click(object sender, RoutedEventArgs e)
        {
            db.Courses.Load();
            if (courseListBox.SelectedItems.Count != 0)
            {
                selectedCourse = courseListBox.SelectedItem as TutoringDB.Cours;
                //Hide all controls from first screen
                courseListBox.IsEnabled = false;
                courseListBox.Visibility = Visibility.Hidden;
                submitCourseButton.IsEnabled = false;
                submitCourseButton.Visibility = Visibility.Hidden;
                CourseLabel.IsEnabled = false;
                CourseLabel.Visibility = Visibility.Hidden;

                //Show all controls from second screen
                scheduleTomorrowButton.IsEnabled = true;
                scheduleTomorrowButton.Visibility = Visibility.Visible;
                thisWeekButton.IsEnabled = true;
                thisWeekButton.Visibility = Visibility.Visible;
                durationLabel.IsEnabled = true;
                durationLabel.Visibility = Visibility.Visible;
                durationComboBox.IsEnabled = true;
                durationComboBox.Visibility = Visibility.Visible;
                orLabel.IsEnabled = true;
                orLabel.Visibility = Visibility.Visible;
                //pickTutorLabel.IsEnabled = true;
                //pickTutorLabel.Visibility = Visibility.Visible;
                //pickTutorComboBox.IsEnabled = true;
                //pickTutorComboBox.Visibility = Visibility.Visible;

                db.Tutors.Load();
                db.TutorCourses.Load();
                foreach (TutoringDB.TutorCourse tc in db.TutorCourses)
                {
                    AvailableTutors temp = new AvailableTutors();
                    if(tc.Cours == courseListBox.SelectedItem as TutoringDB.Cours)
                    {
                        temp.FirstName = tc.Tutor.FirstName;
                        temp.LastName = tc.Tutor.LastName;
                        tutorList.Add(temp);
                    }
                }
                //pickTutorComboBox.Items.Refresh();
                foreach (AvailableTutors t in tutorList)
                {
                    pickTutorComboBox.Items.Add(t.fullName);
                }


                DataContext = tutorList;
            }
        }

        public void DisplayNextAppointment()
        {
            dateLabel.Content = ttcaList.ElementAt(index).Appointment.Date.ToString();
            timeLabel.Content = ttcaList.ElementAt(index).Appointment.Time.ToString();
            tutorNameLabel.Content = ttcaList.ElementAt(index).Tutor.FirstName +' '+ ttcaList.ElementAt(index).Tutor.LastName;
            appointmentDurationLabel.Content = ttcaList.ElementAt(index).Appointment.Duration.ToString();
            index++;
        }
        private void thisWeekButton_Click(object sender, RoutedEventArgs e)
        {
            //Hide Screen 2
            scheduleTomorrowButton.IsEnabled = false;
            scheduleTomorrowButton.Visibility = Visibility.Hidden;
            thisWeekButton.IsEnabled = false;
            thisWeekButton.Visibility = Visibility.Hidden;
            durationLabel.IsEnabled = false;
            durationLabel.Visibility = Visibility.Hidden;
            durationComboBox.IsEnabled = false;
            durationComboBox.Visibility = Visibility.Hidden;
            orLabel.IsEnabled = false;
            orLabel.Visibility = Visibility.Hidden;

            


            db.TutorCourses.Load();
            db.Tutors.Load();
            db.Appointments.Load();
            db.CurrentUsers.Load();
            var current = db.CurrentUsers.FirstOrDefault();
            TutoringDB.Tutee user = new TutoringDB.Tutee();
            var tutBusyTimes = db.TuteeBusyTimes.Where(i => i.Tutee == user);
            var tutAppointmentTimes = db.TutorTuteeCourseAppointments.Where(i => i.Tutee == user);
            user = db.Tutees.Where(i=>i.Username == current.UserName).FirstOrDefault();
            db.TutorTuteeCourseAppointments.Load();
            TutoringDB.Appointment tempAppointment = new TutoringDB.Appointment();
            TutoringDB.TutorTuteeCourseAppointment tempTTCA = new TutoringDB.TutorTuteeCourseAppointment();
            var matches = db.TutorCourses.Where(i => i.Cours == selectedCourse)
                .OrderByDescending(i=>i.Tutor.TutorTuteeCourseAppointments).Select(i=>i.Tutor);
            if (matches.Count() > 0) {
                foreach (var matchedTutor in matches) {
                    //TutoringDB.Tutor matchedTutor = matches.ElementAt(0).Tutor;
                    var busyTimes = db.TutorBusyTimes.Where(i => i.Tutor == matchedTutor);

                    var appointmentTimes = db.TutorTuteeCourseAppointments.Where(i => i.Tutor == matchedTutor);
                    int adjacentFree = 0;
                    for (int i = 0; i < 5; i++)
                    {
                        for (int j = 0; j < 24; j++)
                        {
                            DateTime time = new DateTime(1, 1, 1, 7, 0, 0);
                            DateTime temp = DateTime.Today.AddDays(i+2);
                            if ((busyTimes.Any(k => k.BusyTime.Date == temp.Date && k.BusyTime.Time == time.AddMinutes(30 * j).TimeOfDay)
                                && appointmentTimes.Any(m => m.Appointment.Date == temp.Date && m.Appointment.Time == time.AddMinutes(30 * j).TimeOfDay)) || (
                                tutBusyTimes.Any(k => k.BusyTime.Date == temp.Date && k.BusyTime.Time == time.AddMinutes(30 * j).TimeOfDay)
                                && tutAppointmentTimes.Any(m => m.Appointment.Date == temp.Date && m.Appointment.Time == time.AddMinutes(30 * j).TimeOfDay)))
                            {
                                adjacentFree = 0;
                            }
                        else
                        {
                                adjacentFree++;
                                if (adjacentFree == durationComboBox.SelectedIndex + 1)
                                {
                                    tempAppointment.Time = time.AddMinutes(-j * 30).TimeOfDay;
                                    tempAppointment.Date = temp.Date;
                                    DateTime length = new DateTime(1, 1, 1, 0, 0, 0);
                                    tempAppointment.Duration = length.AddMinutes(durationComboBox.SelectedIndex + 1 * 30).TimeOfDay;
                                    //db.Appointments.Add(tempAppointment);
                                    tempTTCA.Appointment = tempAppointment;
                                    tempTTCA.Tutor = matchedTutor;
                                    tempTTCA.Tutee = db.Tutees.Where(t => t.Username == db.CurrentUsers.FirstOrDefault().UserName).FirstOrDefault();
                                    tempTTCA.Cours = courseListBox.SelectedItem as TutoringDB.Cours;
                                    //db.TutorTuteeCourseAppointments.Add(tempTTCA);
                                    ttcaList.Add(tempTTCA);
                                }
                            }
                        }
                        adjacentFree = 0;
                    }
                }
                //Show Screen 3
                selectorRectangle.Visibility = Visibility.Visible;
                selectorRectangle.IsEnabled = true;
                dateLabel.Visibility = Visibility.Visible;
                dateLabel.IsEnabled = true;
                timeLabel.Visibility = Visibility.Visible;
                timeLabel.IsEnabled = true;
                tutorNameLabel.Visibility = Visibility.Visible;
                tutorNameLabel.IsEnabled = true;
                appointmentDurationLabel.Visibility = Visibility.Visible;
                appointmentDurationLabel.IsEnabled = true;
                confirmAppointmentButton.Visibility = Visibility.Visible;
                confirmAppointmentButton.IsEnabled = true;
                rejectAppointmentButton.Visibility = Visibility.Visible;
                rejectAppointmentButton.IsEnabled = true;
                DisplayNextAppointment();
            }
            else
            {
                MessageBox.Show("No available tutors at this time.");
            }
        }

        private void scheduleTomorrowButton_Click(object sender, RoutedEventArgs e)
        {
            //Hide Screen 2
            scheduleTomorrowButton.IsEnabled = false;
            scheduleTomorrowButton.Visibility = Visibility.Hidden;
            thisWeekButton.IsEnabled = false;
            thisWeekButton.Visibility = Visibility.Hidden;
            durationLabel.IsEnabled = false;
            durationLabel.Visibility = Visibility.Hidden;
            durationComboBox.IsEnabled = false;
            durationComboBox.Visibility = Visibility.Hidden;
            orLabel.IsEnabled = false;
            orLabel.Visibility = Visibility.Hidden;




            db.TutorCourses.Load();
            db.Tutors.Load();
            db.Appointments.Load();
            db.CurrentUsers.Load();
            var current = db.CurrentUsers.FirstOrDefault();
            TutoringDB.Tutee user = new TutoringDB.Tutee();
            var tutBusyTimes = db.TuteeBusyTimes.Where(i => i.Tutee == user);
            var tutAppointmentTimes = db.TutorTuteeCourseAppointments.Where(i => i.Tutee == user);
            user = db.Tutees.Where(i => i.Username == current.UserName).FirstOrDefault();
            db.TutorTuteeCourseAppointments.Load();
            TutoringDB.Appointment tempAppointment = new TutoringDB.Appointment();
            TutoringDB.TutorTuteeCourseAppointment tempTTCA = new TutoringDB.TutorTuteeCourseAppointment();
            List<TutoringDB.Tutor> matches = new List<TutoringDB.Tutor>();
            foreach(TutoringDB.TutorCourse i in db.TutorCourses)
            {
                if(i.Cours == selectedCourse)
                {
                    matches.Add(i.Tutor);
                }
            }
            if (matches.Count() > 0)
            {
                foreach (var matchedTutor in matches)
                {
                    //TutoringDB.Tutor matchedTutor = matches.ElementAt(0).Tutor;
                    var busyTimes = db.TutorBusyTimes.Where(i => i.Tutor == matchedTutor);

                    var appointmentTimes = db.TutorTuteeCourseAppointments.Where(i => i.Tutor == matchedTutor);
                    int adjacentFree = 0;
                    DateTime temp = DateTime.Today.AddDays(1);
                    for (int j = 0; j < 24; j++)
                    {
                        DateTime time = new DateTime(1, 1, 1, 7, 0, 0);
                        if ((busyTimes.Any(k => k.BusyTime.Date == temp.Date && k.BusyTime.Time == time.AddMinutes(30 * j).TimeOfDay)
                            || appointmentTimes.Any(m => m.Appointment.Date == temp.Date && m.Appointment.Time == time.AddMinutes(30 * j).TimeOfDay)) || (
                            tutBusyTimes.Any(k => k.BusyTime.Date == temp.Date && k.BusyTime.Time == time.AddMinutes(30 * j).TimeOfDay)
                            || tutAppointmentTimes.Any(m => m.Appointment.Date == temp.Date && m.Appointment.Time == time.AddMinutes(30 * j).TimeOfDay)))
                        {
                            adjacentFree = 0;
                        }
                    else
                    {
                            adjacentFree++;
                            if (adjacentFree == durationComboBox.SelectedIndex + 1)
                            {
                                tempAppointment.Time = time.AddMinutes(-j * 30).TimeOfDay;
                                tempAppointment.Date = temp.Date;
                                DateTime length = new DateTime(1, 1, 1, 0, 0, 0);
                                tempAppointment.Duration = length.AddMinutes(durationComboBox.SelectedIndex + 1 * 30).TimeOfDay;
                                //db.Appointments.Add(tempAppointment);
                                tempTTCA.Appointment = tempAppointment;
                                tempTTCA.Tutor = matchedTutor;
                                tempTTCA.Tutee = db.Tutees.Where(t => t.Username == db.CurrentUsers.FirstOrDefault().UserName).FirstOrDefault();
                                tempTTCA.Cours = courseListBox.SelectedItem as TutoringDB.Cours;
                                //db.TutorTuteeCourseAppointments.Add(tempTTCA);
                                ttcaList.Add(tempTTCA);
                            }
                        }
                    }
                    adjacentFree = 0;
                }
                //Show Screen 3
                selectorRectangle.Visibility = Visibility.Visible;
                selectorRectangle.IsEnabled = true;
                dateLabel.Visibility = Visibility.Visible;
                dateLabel.IsEnabled = true;
                timeLabel.Visibility = Visibility.Visible;
                timeLabel.IsEnabled = true;
                tutorNameLabel.Visibility = Visibility.Visible;
                tutorNameLabel.IsEnabled = true;
                appointmentDurationLabel.Visibility = Visibility.Visible;
                appointmentDurationLabel.IsEnabled = true;
                confirmAppointmentButton.Visibility = Visibility.Visible;
                confirmAppointmentButton.IsEnabled = true;
                rejectAppointmentButton.Visibility = Visibility.Visible;
                rejectAppointmentButton.IsEnabled = true;
                DisplayNextAppointment();
            }
            else
            {
                MessageBox.Show("No available tutors at this time.");
            }
        }

        private void rejectAppointmentButton_Click(object sender, RoutedEventArgs e)
        {
           if(index < ttcaList.Count)
            {
                DisplayNextAppointment();
            }
            else
            {
                MessageBox.Show("No more available matches.");
            }
        }

        private void confirmAppointmentButton_Click(object sender, RoutedEventArgs e)
        {
            db.Appointments.Add(ttcaList.ElementAt(index - 1).Appointment);
            db.TutorTuteeCourseAppointments.Add(ttcaList.ElementAt(index - 1));
            db.SaveChanges();
        }
    }
}
