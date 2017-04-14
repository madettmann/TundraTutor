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
using System.ComponentModel;
using System.Collections.ObjectModel;
using TutoringDB;

namespace TutorWindows
{


    /// <summary>
    /// Interaction logic for FacultyView.xaml
    /// </summary>
    public partial class FacultyView : TundraControls.CustomWindow
    {
        bool finished;
        TutoringDB.TutorDatabaseEntities db = new TutoringDB.TutorDatabaseEntities();
        public FacultyView()
        {
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            InitializeComponent();
            finished = true;
        }
        private void CustomWindow_Loaded(object sender, RoutedEventArgs e)
        {
            TutoringDB.FacultyCours temp = new TutoringDB.FacultyCours();
            temp.Cours = db.Courses.FirstOrDefault();
            temp.Faculty = db.Faculties.FirstOrDefault();
            temp.Id = db.FacultyCourses.Count();
            db.FacultyCourses.Add(temp);
            db.SaveChanges();
            this.Width = SystemParameters.WorkArea.Width;
            this.Height = SystemParameters.WorkArea.Height;
            this.Left = SystemParameters.WorkArea.Left;
            this.Top = SystemParameters.WorkArea.Top;
            System.Windows.Data.CollectionViewSource facultyCoursViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("facultyCoursViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // facultyCoursViewSource.Source = [generic data source]
            TutoringDB.Faculty tempFaculty = new TutoringDB.Faculty();
            tempFaculty = db.Faculties.Where(i => i.Username == db.CurrentUsers.FirstOrDefault().UserName).FirstOrDefault();
            db.FacultyCourses.Where(i => i.FacultyId == tempFaculty.Id).Load();
            var facultyNames = from i in db.FacultyCourses.Local
                            select (i.Faculty.First_Name);
            var courseNames = from i in db.FacultyCourses.Local
                              select (i.Cours.CourseName);
            facultyCoursViewSource.Source = "Name: " + facultyNames.ElementAt(0) + "\n Course: " + courseNames.ElementAt(0);
        }
        private void CustomWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (finished) Application.Current.Shutdown();
        }
    }
}
