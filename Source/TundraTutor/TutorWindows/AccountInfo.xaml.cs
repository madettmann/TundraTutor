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
using System.Windows.Forms;
using System.ComponentModel;

namespace TutorWindows
{
    public struct UserInfo : INotifyPropertyChanged
    {
        private string userFullName;
        private string userUsernameValue;
        private string userYearValue;
        private string userApprovedValue;
        private string wrongOldPassword;

        public string UserFullName {
            get { return userFullName; }
            set { userFullName = value; NotifyPropertyChanged("UserFullName"); }
        }
        public string UserUsernameValue {
            get { return userUsernameValue; }
            set{ userUsernameValue = value; NotifyPropertyChanged("UserUsernameValue"); }
        }
        public string UserYearValue {
            get { return userYearValue; }
            set { userYearValue = value; NotifyPropertyChanged("UserYearValue"); }
        }
        public string UserApprovedValue {
            get { return userApprovedValue; }
            set { userApprovedValue = value; NotifyPropertyChanged("UserApprovedValue"); }
        }
        public string WrongOldPassword {
            get { return wrongOldPassword; }
            set { wrongOldPassword = value; NotifyPropertyChanged("WrongOldPassword"); }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {

            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
    public partial class AccountInfo : TundraControls.CustomWindow
    {
        private string userName;
        UserInfo info;

        public AccountInfo()
        {
            //userName = "jim";
            
            info = new UserInfo();
            StreamReader getUser = new StreamReader("..\\..\\..\\Temp\\CurrentUser.txt");
            userName = getUser.ReadLine();
            getUser.Close();
            TutoringDB.TutorDatabaseEntities tutorSchedule = new TutoringDB.TutorDatabaseEntities();
            

            if (tutorSchedule.Tutees.Any(user => user.Username == userName))
            {
                tutorSchedule.Tutees
                    .Where(user => (user.Username == userName))
                    .Load();
                var userList = from i in tutorSchedule.Tutees
                                select i;

                foreach (var tuteeUser in userList) {
                    info.UserFullName = tuteeUser.FirstName + " " + tuteeUser.LastName;
                    info.UserUsernameValue = tuteeUser.Username;
                    info.UserYearValue = tuteeUser.Year.ToString();
                    info.UserApprovedValue = "No";
                }
            }
            else if (tutorSchedule.Tutors.Any(user => user.UserName == userName))
            {
                tutorSchedule.Tutors
                    .Where(user => (user.UserName == userName))
                    .Load();
                var userList = from i in tutorSchedule.Tutors
                                             select i;

                foreach (var tutorUser in userList)
                {
                    info.UserFullName = tutorUser.FirstName + " " + tutorUser.LastName;
                    info.UserUsernameValue = tutorUser.UserName;
                    info.UserYearValue = tutorUser.Year.ToString();
                    info.UserApprovedValue = tutorUser.Authorized.ToString();
                }
                
            }
            else
            {
                tutorSchedule.Faculties
                    .Where(fac => fac.Username == userName)
                    .Load();
                var userList = from i in tutorSchedule.Faculties
                               select i;

                foreach (var facultyUser in userList)
                {
                    info.UserFullName = facultyUser.First_Name + " " + facultyUser.LastName;
                    info.UserUsernameValue = facultyUser.Username;
                    info.UserYearValue = "Not applicable";
                    info.UserApprovedValue = "Can approve or deny requests to tutor";
                }
                
            }
            info.WrongOldPassword = "";

            DataContext = info;         

            InitializeComponent();
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void submitButton_Click(object sender, RoutedEventArgs e)
        {
            if (oldPasswordBox.Password == "" || newPasswordBox.Password == "")
            {
                info.WrongOldPassword = "Fill out all fields to change password";
            }
            else
            {
                TutoringDB.TutorDatabaseEntities userCreds = new TutoringDB.TutorDatabaseEntities();
                bool correctOldPassword = (userCreds.Tutors.Any(user => user.UserName == userName && user.Password == oldPasswordBox.Password) ||
                       userCreds.Tutees.Any(user => user.Username == userName && user.Password == oldPasswordBox.Password) ||
                       userCreds.Faculties.Any(user => user.Username == userName && user.Password == oldPasswordBox.Password));
                if (correctOldPassword)
                {
                    info.WrongOldPassword = "Password change successful / NOT IMPLEMENTED";
                }
                else
                {
                    info.WrongOldPassword = "Incorrect old Password";
                }
            }

            DataContext = info;
        }
    }
}
