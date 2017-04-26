//Written by victor
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
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
    /// <summary>
    /// Interaction logic for DayMenu.xaml
    /// </summary>
    public struct Info : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {

            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private string dateString;
        private ObservableCollection<TundraControls.Appointment> appointmentList;

        public string DateString { get => dateString; set { dateString = value; NotifyPropertyChanged("DateString"); } }
        public ObservableCollection<TundraControls.Appointment> AppointmentList { get => appointmentList; set { appointmentList = value; NotifyPropertyChanged("AppointmentList"); } }

    }
    public partial class DayMenu : TundraControls.CustomWindow
    {
        //Never gets used, was going to display the appointments in a day when clicked in the month view -- not high enmough priority
        //With a little work, the display could easily be implemented, click commands to go to appointment info may take a little longer

        TutoringDB.TutorDatabaseEntities db = new TutoringDB.TutorDatabaseEntities();
        public Info info;
        TutoringDB.CurrentUser user;

        public DayMenu(DateTime date)
        {
            user = new TutoringDB.CurrentUser();
            db = new TutoringDB.TutorDatabaseEntities();
            var userList = from i in db.CurrentUsers select i;
            user = userList.FirstOrDefault();

            InitializeComponent();

            info = new Info();

            info.DateString = date.ToShortDateString();
            info.AppointmentList = new ObservableCollection<TundraControls.Appointment>();
            var appointments = from i in db.TutorTuteeCourseAppointments
                               where ((i.Tutee.Username == user.UserName || i.Tutor.UserName == user.UserName)
                                      && i.Appointment.Date == date)
                               select i;
            foreach (var appt in appointments)
            {
                info.AppointmentList.Add(new TundraControls.Appointment(appt.Tutor.FirstName + appt.Tutor.LastName, 
                                                                        appt.Tutee.FirstName + appt.Tutee.LastName,
                                                                        appt.Appointment.Time, appt.Cours.CourseName,
                                                                        appt.Appointment.Date));
            }
            DataContext = this;
        }

    }
}
