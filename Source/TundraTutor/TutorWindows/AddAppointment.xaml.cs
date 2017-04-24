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
                //orLabel.IsEnabled = true;
                //orLabel.Visibility = Visibility.Visible;
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
            dateLabel.Content = ttcaList.ElementAt(index).Appointment.Date.ToString("ddd, MMM dd");
            timeLabel.Content = ttcaList.ElementAt(index).Appointment.Time.ToString(@"hh\:mm");
            tutorNameLabel.Content = ttcaList.ElementAt(index).Tutor.FirstName +' '+ ttcaList.ElementAt(index).Tutor.LastName;
            appointmentDurationLabel.Content = "Duration: "+ttcaList.ElementAt(index).Appointment.Duration.ToString(/*@"hh\:mm"*/);
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
            db.TuteeBusyTimes.Load();
            db.TutorTuteeCourseAppointments.Load();
            var current = db.CurrentUsers.FirstOrDefault();
            TutoringDB.Tutee user = new TutoringDB.Tutee();
            var tutBusyTimes = db.TuteeBusyTimes.Where(i => i.Tutee.Username == user.Username).ToList();
            var tutAppointmentTimes = db.TutorTuteeCourseAppointments.Where(i => i.Tutee.Username == user.Username).ToList();
            user = db.Tutees.Where(i => i.Username == current.UserName).FirstOrDefault();
            db.TutorTuteeCourseAppointments.Load();
            List<TutoringDB.Tutor> matches = new List<TutoringDB.Tutor>();
            foreach (TutoringDB.TutorCourse i in db.TutorCourses)
            {
                if (i.Cours.CourseName == selectedCourse.CourseName)
                {
                    matches.Add(i.Tutor);
                }
            }
            if (matches.Count() > 0) {
                foreach (var matchedTutor in matches)
                {
                    var busyTimes = db.TutorBusyTimes.Where(i => i.Tutor.UserName == matchedTutor.UserName).ToList();
                    var appointmentTimes = db.TutorTuteeCourseAppointments.Where(i => i.Tutor.UserName == matchedTutor.UserName).ToList();
                    int adjacentFree = 0;
                    DateTime temp = DateTime.Today.AddDays(1);
                    temp = temp.AddHours(-temp.Hour);
                    temp = temp.AddMinutes(-temp.Minute);
                    temp = temp.AddHours(7);
                    for (int i = 0; i < 5; i++)
                    {
                        temp = temp.AddDays(1);
                        for (int j = 0; j < 24; j++)
                        {
                            temp = temp.AddMinutes(30);
                            bool busyConflict = false;
                            foreach (var bt in busyTimes)
                            {
                                if (bt.BusyTime.Date.Month == temp.Month &&
                                    bt.BusyTime.Date.Day == temp.Day &&
                                      bt.BusyTime.Time.Hours == temp.Hour &&
                                        bt.BusyTime.Time.Minutes == temp.Minute)
                                {
                                    busyConflict = true;
                                    break;
                                }
                            }
                            if (!busyConflict)
                            {
                                foreach (var apt in appointmentTimes)
                                {
                                    if (apt.Appointment.Date.Month == temp.Month &&
                                        apt.Appointment.Date.Day == temp.Day &&
                                          apt.Appointment.Time.Hours == temp.Hour &&
                                            apt.Appointment.Time.Minutes == temp.Minute)
                                    {
                                        busyConflict = true;
                                        break;
                                    }
                                }
                            }
                            if (!busyConflict)
                            {
                                foreach (var tbt in tutBusyTimes)
                                {
                                    if (tbt.BusyTime.Date.Month == temp.Month &&
                                          tbt.BusyTime.Date.Day == temp.Day &&
                                             tbt.BusyTime.Time.Hours == temp.Hour &&
                                               tbt.BusyTime.Time.Minutes == temp.Minute)
                                    {
                                        busyConflict = true;
                                        break;
                                    }
                                }
                            }
                            if (!busyConflict)
                            {
                                foreach (var tApt in tutAppointmentTimes)
                                {
                                    if (tApt.Appointment.Date.Month == temp.Month &&
                                          tApt.Appointment.Date.Day == temp.Day &&
                                             tApt.Appointment.Time.Hours == temp.Hour &&
                                               tApt.Appointment.Time.Minutes == temp.Minute)
                                    {
                                        busyConflict = true;
                                        break;
                                    }
                                }
                            }
                            if (busyConflict)
                            {
                                adjacentFree = 0;
                            }
                            else
                            {
                                adjacentFree++;
                                if (adjacentFree == durationComboBox.SelectedIndex + 1)
                                {
                                    TutoringDB.Appointment tempAppointment = new TutoringDB.Appointment();
                                    TutoringDB.TutorTuteeCourseAppointment tempTTCA = new TutoringDB.TutorTuteeCourseAppointment();
                                    tempAppointment.Time = temp.TimeOfDay;
                                    tempAppointment.Date = temp.Date;
                                    DateTime length = new DateTime(1, 1, 1, 0, 0, 0);
                                    tempAppointment.Duration = length.AddMinutes((durationComboBox.SelectedIndex + 1) * 30).TimeOfDay;
                                    tempTTCA.Appointment = tempAppointment;
                                    tempTTCA.Tutor = matchedTutor;
                                    tempTTCA.Tutee = db.Tutees.Where(t => t.Username == db.CurrentUsers.FirstOrDefault().UserName).FirstOrDefault();
                                    tempTTCA.Cours = courseListBox.SelectedItem as TutoringDB.Cours;
                                    ttcaList.Add(tempTTCA);
                                    adjacentFree = 0;
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
            db.TuteeBusyTimes.Load();
            db.TutorTuteeCourseAppointments.Load();
            var current = db.CurrentUsers.FirstOrDefault();
            TutoringDB.Tutee user = new TutoringDB.Tutee();
            var tutBusyTimes = db.TuteeBusyTimes.Where(i => i.Tutee.Username == user.Username).ToList();
            var tutAppointmentTimes = db.TutorTuteeCourseAppointments.Where(i => i.Tutee.Username == user.Username).ToList();
            user = db.Tutees.Where(i => i.Username == current.UserName).FirstOrDefault();
            db.TutorTuteeCourseAppointments.Load();
            List<TutoringDB.Tutor> matches = new List<TutoringDB.Tutor>();
            foreach(TutoringDB.TutorCourse i in db.TutorCourses)
            {
                if(i.Cours.CourseName == selectedCourse.CourseName)
                {
                    matches.Add(i.Tutor);
                }
            }
            if (matches.Count() > 0)
            {
                foreach (var matchedTutor in matches)
                {
                   var busyTimes = db.TutorBusyTimes.Where(i => i.Tutor.UserName == matchedTutor.UserName).ToList();
                    var appointmentTimes = db.TutorTuteeCourseAppointments.Where(i => i.Tutor.UserName == matchedTutor.UserName).ToList();
                    int adjacentFree = 0;
                    DateTime temp = DateTime.Today.AddDays(1);
                    temp = temp.AddHours(-temp.Hour);
                    temp = temp.AddHours(7);
                    temp = temp.AddMinutes(-temp.Minute);
                    for (int j = 0; j < 12; j++)
                    {
                        temp = temp.AddMinutes(30);
                        bool busyConflict = false;
                        foreach(var bt in busyTimes)
                        {
                            if(bt.BusyTime.Date.Month == temp.Month&&
                                bt.BusyTime.Date.Day == temp.Day&&
                                  bt.BusyTime.Time.Hours == temp.Hour&&
                                    bt.BusyTime.Time.Minutes == temp.Minute)
                            {
                                busyConflict = true;
                                break;
                            }
                        }
                        if (!busyConflict)
                        {
                            foreach(var apt in appointmentTimes)
                            {
                                if(apt.Appointment.Date.Month == temp.Month &&
                                    apt.Appointment.Date.Day == temp.Day &&
                                      apt.Appointment.Time.Hours == temp.Hour &&
                                        apt.Appointment.Time.Minutes == temp.Minute)
                                {
                                    busyConflict = true;
                                    break;
                                }
                            }
                        }
                        if (!busyConflict)
                        {
                            foreach (var tbt in tutBusyTimes)
                            {
                                if (tbt.BusyTime.Date.Month == temp.Month &&
                                      tbt.BusyTime.Date.Day == temp.Day &&
                                         tbt.BusyTime.Time.Hours == temp.Hour &&
                                           tbt.BusyTime.Time.Minutes == temp.Minute)
                                {
                                    busyConflict = true;
                                    break;
                                }
                            }
                        }
                        if (!busyConflict)
                        {
                            foreach (var tApt in tutAppointmentTimes)
                            {
                                if (tApt.Appointment.Date.Month == temp.Month &&
                                      tApt.Appointment.Date.Day == temp.Day &&
                                         tApt.Appointment.Time.Hours == temp.Hour &&
                                           tApt.Appointment.Time.Minutes == temp.Minute)
                                {
                                    busyConflict = true;
                                    break;
                                }
                            }
                        }
                        if (busyConflict)
                        {
                            adjacentFree = 0;
                        }
                    else
                    {
                            adjacentFree++;
                            if (adjacentFree == durationComboBox.SelectedIndex + 1)
                            {
                                TutoringDB.Appointment tempAppointment = new TutoringDB.Appointment();
                                TutoringDB.TutorTuteeCourseAppointment tempTTCA = new TutoringDB.TutorTuteeCourseAppointment();
                                tempAppointment.Time = temp.TimeOfDay;
                                tempAppointment.Date = temp.Date;
                                DateTime length = new DateTime(1, 1, 1, 0, 0, 0);
                                tempAppointment.Duration = length.AddMinutes((durationComboBox.SelectedIndex + 1) * 30).TimeOfDay;
                                tempTTCA.Appointment = tempAppointment;
                                tempTTCA.Tutor = matchedTutor;
                                tempTTCA.Tutee = db.Tutees.Where(t => t.Username == db.CurrentUsers.FirstOrDefault().UserName).FirstOrDefault();
                                tempTTCA.Cours = courseListBox.SelectedItem as TutoringDB.Cours;
                                ttcaList.Add(tempTTCA);
                                adjacentFree--;
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

            //Add a notification
            var tuto = ttcaList.ElementAt(index - 1).Tutor;
            var tute = ttcaList.ElementAt(index - 1).Tutee;
            var tutoid = db.Tutors.Where(tut => tut.UserName == tuto.UserName).FirstOrDefault().Id;
            var tuteid = db.Tutees.Where(tut => tut.Username == tute.Username).FirstOrDefault().Id;
            var time = ttcaList.ElementAt(index - 1).Appointment.Time;
            var date = ttcaList.ElementAt(index - 1).Appointment.Date;

            //For the tutee who scheduled it
            db.TutorTuteeNotifications.Load();
            TutoringDB.TutorTuteeNotification newNot = new TutoringDB.TutorTuteeNotification();
            newNot.Message = "Appointment scheduled with " + tuto.FirstName + " " + tuto.LastName + " for " + ttcaList.ElementAt(index - 1).Cours.CourseName;
            newNot.Type = "TutorTuteeCourseAppointments";
            newNot.targetId = db.TutorTuteeCourseAppointments.Where(apt => apt.TuteeId == tuteid && apt.TutorId == tutoid
                                                              && apt.Appointment.Date == date && apt.Appointment.Time == time).FirstOrDefault().Id;
            newNot.TuteeId = tuteid;
            db.TutorTuteeNotifications.Add(newNot);

            //Add the tutor with whom it is scheduled
            newNot.Message = "Appointment scheduled with " + ttcaList.ElementAt(index - 1).Tutee.FirstName + " " + ttcaList.ElementAt(index - 1).Tutee.LastName;
            newNot.Type = "TutorTuteeCourseAppointments";
            newNot.targetId = db.TutorTuteeCourseAppointments.Where(apt => apt.TuteeId == tuteid && apt.TutorId == tutoid
                                                              && apt.Appointment.Date == date && apt.Appointment.Time == time).FirstOrDefault().Id;
            newNot.TutorId = tutoid;
            db.TutorTuteeNotifications.Add(newNot);
            db.SaveChanges();

            MessageBox.Show("Appointment scheduled.");
            this.Close();
        }
    }
}
