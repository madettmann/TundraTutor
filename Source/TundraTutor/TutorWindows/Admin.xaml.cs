//Written by Victor
using DisplayTables;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
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
using TutoringDB;

namespace TutorWindows
{
    public struct Lists : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {

            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private List<TutoringDB.Tutor> tutorList;
        private List<TutoringDB.Tutee> tuteeList;
        private List<TutoringDB.Faculty> facultyList;
        private int listSelected;
        private SolidColorBrush tutorListBack;
        private SolidColorBrush tuteeListBack;
        private SolidColorBrush facultyListBack;

        public List<Tutor> TutorList { get => tutorList; set { tutorList = value; NotifyPropertyChanged("TutorList"); NotifyPropertyChanged("TutorNames"); } }
        public List<Tutee> TuteeList { get => tuteeList; set { tuteeList = value; NotifyPropertyChanged("TuteeList"); NotifyPropertyChanged("TuteeNames"); } }
        public List<Faculty> FacultyList { get => facultyList; set { facultyList = value; NotifyPropertyChanged("FacultyList"); NotifyPropertyChanged("FacultyNames"); } }
        public int ListSelected { get => listSelected; set { listSelected = value; NotifyPropertyChanged("ListSelected"); } }
        public SolidColorBrush TutorListBack { get => tutorListBack; set { tutorListBack = value; NotifyPropertyChanged("TutorListBack"); } }
        public SolidColorBrush TuteeListBack { get => tuteeListBack; set { tuteeListBack = value; NotifyPropertyChanged("TuteeListBack"); } }
        public SolidColorBrush FacultyListBack { get => facultyListBack; set { facultyListBack = value; NotifyPropertyChanged("FacultyListBack"); } }

        public ObservableCollection<string> TutorNames { get {
                ObservableCollection<string> outList = new ObservableCollection<string>();
                foreach (var tutor in tutorList) outList.Add(tutor.FirstName + " " + tutor.LastName);
                return outList;} }

        public ObservableCollection<string> TuteeNames
        {
            get
            {
                ObservableCollection<string> outList = new ObservableCollection<string>();
                foreach (var tutee in tuteeList) outList.Add(tutee.FirstName + " " + tutee.LastName);
                return outList;
            }
        }
        public ObservableCollection<string> FacultyNames
        {
            get
            {
                ObservableCollection<string> outList = new ObservableCollection<string>();
                foreach (var faculty in facultyList) outList.Add(faculty.First_Name + " " + faculty.LastName);
                return outList;
            }
        }

        
    }

    /// <summary>
    /// Interaction logic for Admin.xaml
    /// </summary>
    public partial class Admin : TundraControls.CustomWindow
    {
        Lists userLists;
        TutoringDB.TutorDatabaseEntities userCreds;

        public Admin()
        {
            userLists.TutorList = new List<Tutor>();
            userLists.TuteeList = new List<Tutee>();
            userLists.FacultyList = new List<Faculty>();


            userCreds = new TutoringDB.TutorDatabaseEntities();
            userCreds.Tutors.Load();
            userCreds.Tutees.Load();
            userCreds.Faculties.Load();
            var tempListTutor = userCreds.Tutors.OrderBy(tut => tut.LastName).ToList();
            foreach (var element in tempListTutor) if(element != null) userLists.TutorList.Add(element);
            var tempListTutee = userCreds.Tutees.OrderBy(tut => tut.LastName).ToList();
            foreach (var element in tempListTutee) if (element != null) userLists.TuteeList.Add(element);
            var tempListFaculty = userCreds.Faculties.OrderBy(fac => fac.LastName).ToList();
            foreach (var element in tempListFaculty) if (element != null) userLists.FacultyList.Add(element);

            InitializeComponent();

            DataContext = userLists;
        }

        private void selectButton_Click(object sender, RoutedEventArgs e)
        {
            //if (userLists.ListSelected == 1) MessageBox.Show(userLists.TutorNames.ElementAt(tutorList.SelectedIndex));
            //else if (userLists.ListSelected == 2) MessageBox.Show(userLists.TuteeNames.ElementAt(tuteeList.SelectedIndex));
            //else if (userLists.ListSelected == 3) MessageBox.Show(userLists.FacultyNames.ElementAt(facultyList.SelectedIndex));
            userCreds.CurrentUsers.Load();
            foreach (var curUser in userCreds.CurrentUsers) userCreds.CurrentUsers.Remove(curUser);
            userCreds.SaveChanges();
            TutoringDB.CurrentUser user = new CurrentUser();
            switch (userLists.ListSelected) { case 1: user.Type = "adminTutor"; break;
                                              case 2: user.Type = "adminTutee"; break;
                                              case 3: user.Type = "adminFaculty"; break; }
            switch (userLists.ListSelected) {   case 1: user.UserName = userLists.TutorList.ElementAt(tutorList.SelectedIndex).UserName; break;
                                                case 2: user.UserName = userLists.TuteeList.ElementAt(tuteeList.SelectedIndex).Username; break;
                                                case 3: user.UserName = userLists.FacultyList.ElementAt(facultyList.SelectedIndex).Username; break;
            }
            userCreds.CurrentUsers.Add(user);
            userCreds.SaveChanges();
            MainWindow userView = new MainWindow();
            userView.ShowDialog();
        }

        private void lofOffButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void tutorList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            changeIndex(1);
        }

        private void tuteeList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            changeIndex(2);
        }

        private void facultyList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            changeIndex(3);
        }

        private void tutorList_MouseDown(object sender, MouseButtonEventArgs e)
        {
            changeIndex(1);
        }

        private void tuteeList_MouseDown(object sender, MouseButtonEventArgs e)
        {
            changeIndex(2);
        }

        private void facultyList_MouseDown(object sender, MouseButtonEventArgs e)
        {
            changeIndex(3);
        }

        private void changeIndex(int column)
        {
            switch (column)
            {
                case 1:
                    userLists.ListSelected = 1;
                    tutorList.Background = new SolidColorBrush(Color.FromArgb(30, 0, 30, 75));
                    tuteeList.Background = null;
                    facultyList.Background = null;
                    break;
                case 2:
                    userLists.ListSelected = 2;
                    tuteeList.Background = new SolidColorBrush(Color.FromArgb(30, 0, 30, 75));
                    tutorList.Background = null;
                    facultyList.Background = null;
                    break;
                case 3:
                    userLists.ListSelected = 3;
                    facultyList.Background = new SolidColorBrush(Color.FromArgb(30, 0, 30, 75));
                    tutorList.Background = null;
                    tuteeList.Background = null;
                    break;
                default:
                    break;
            }
        }

        private void databaseButton_Click(object sender, RoutedEventArgs e)
        {
            LaunchPage lp = new LaunchPage();
            lp.ShowDialog();
        }

        private void databaseButton_MouseEnter(object sender, MouseEventArgs e)
        {
            databaseButton.Foreground = new SolidColorBrush(Colors.Red);
        }

        private void databaseButton_MouseLeave(object sender, MouseEventArgs e)
        {
            databaseButton.Foreground = new SolidColorBrush(Colors.Black);
        }
    }
}
