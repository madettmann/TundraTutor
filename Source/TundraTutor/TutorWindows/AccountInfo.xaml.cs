//Written by Victor
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
        UserInfo info;
        private TutoringDB.TutorDatabaseEntities readUser;
        private TutoringDB.CurrentUser user;

        public AccountInfo()
        {
            //userName = "jim";

            //Get the current user
            user = new TutoringDB.CurrentUser();
            readUser = new TutoringDB.TutorDatabaseEntities();
            readUser.CurrentUsers.Load();
            user = readUser.CurrentUsers.FirstOrDefault();

            TutoringDB.TutorDatabaseEntities tutorSchedule = new TutoringDB.TutorDatabaseEntities();
            

            if (user.Type.ToLower().Contains("tutee"))
            {
                tutorSchedule.Tutees
                    .Where(userF => (userF.Username == user.UserName))
                    .Load();
                var userList = from i in tutorSchedule.Tutees
                               where i.Username == user.UserName
                                select i;
                var tuteeUser = userList.FirstOrDefault();
                    info.UserFullName = tuteeUser.FirstName + " " + tuteeUser.LastName;
                    info.UserUsernameValue = tuteeUser.Username;
                    info.UserYearValue = tuteeUser.Year.ToString();
                    info.UserApprovedValue = "No";
            }
            else if (user.Type.ToLower().Contains("tutor"))
            {
                tutorSchedule.Tutors
                    .Where(userF => (userF.UserName == user.UserName))
                    .Load();
                var userList = from i in tutorSchedule.Tutors
                               where i.UserName == user.UserName
                               select i;

                var tutorUser = userList.FirstOrDefault();
                info.UserFullName = tutorUser.FirstName + " " + tutorUser.LastName;
                info.UserUsernameValue = tutorUser.UserName;
                info.UserYearValue = tutorUser.Year.ToString();
                info.UserApprovedValue = tutorUser.Authorized.ToString();
            }
            else
            {
                tutorSchedule.Faculties
                    .Where(fac => fac.Username == user.UserName)
                    .Load();
                var userList = from i in tutorSchedule.Faculties
                               where i.Username == user.UserName
                               select i;

                var facultyUser = userList.FirstOrDefault();
                info.UserFullName = facultyUser.First_Name + " " + facultyUser.LastName;
                info.UserUsernameValue = facultyUser.Username;
                info.UserYearValue = "Not applicable";
                info.UserApprovedValue = "Can approve or deny requests to tutor";
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
                bool correctOldPassword = (userCreds.Tutors.Any(userF => userF.UserName == user.UserName && userF.Password == oldPasswordBox.Password) ||
                       userCreds.Tutees.Any(userF => userF.Username == user.UserName && userF.Password == oldPasswordBox.Password) ||
                       userCreds.Faculties.Any(userF => userF.Username == user.UserName && userF.Password == oldPasswordBox.Password));
                if (correctOldPassword)
                {
                    info.WrongOldPassword = "Password change successful";
                    if (user.Type.ToLower().Contains("tutee")) userCreds.Tutees.Where(userT => userT.Username == user.UserName)
                                                               .FirstOrDefault().Password = newPasswordBox.Password;
                    else if (user.Type.ToLower().Contains("tutor")) userCreds.Tutors.Where(userT => userT.UserName == user.UserName)
                                                                    .FirstOrDefault().Password = newPasswordBox.Password;

                    else userCreds.Faculties.Where(userT => userT.Username == user.UserName)
                         .FirstOrDefault().Password = newPasswordBox.Password;
                    userCreds.SaveChanges();
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
