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
            //TutoringDB.FacultyCours temp = new TutoringDB.FacultyCours();
            //temp.Cours = db.Courses.FirstOrDefault();
            //temp.Faculty = db.Faculties.FirstOrDefault();
            //temp.Id = db.FacultyCourses.Count();
            //db.FacultyCourses.Add(temp);
            //db.SaveChanges();
            ////bool end = false;
            ////int count = 0;
            ////TutoringDB.Faculty tempf = new TutoringDB.Faculty();
            ////tempf = db.Faculties.Where(i => i.Username == db.CurrentUsers.FirstOrDefault().UserName).FirstOrDefault();
            ////db.FacultyCourses.Where(i => i.FacultyId == tempf.Id).Load();
            ////var courseNames = from i in db.FacultyCourses.Local
            ////                  select (i.Cours.CourseName);

            //this.Width = SystemParameters.WorkArea.Width;
            //this.Height = SystemParameters.WorkArea.Height;
            //this.Left = SystemParameters.WorkArea.Left;
            //this.Top = SystemParameters.WorkArea.Top;
            ////System.Windows.Data.CollectionViewSource facultyCoursViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("facultyCoursViewSource")));
            //// Load data by setting the CollectionViewSource.Source property:
            //// facultyCoursViewSource.Source = [generic data source]
            //TutoringDB.Faculty tempFaculty = new TutoringDB.Faculty();
            //tempFaculty = db.Faculties.Where(i => i.Username == db.CurrentUsers.FirstOrDefault().UserName).FirstOrDefault();
            //db.FacultyCourses.Where(i => i.FacultyId == tempFaculty.Id).Load();
            //int count = db.FacultyCourses.Count();
            //while(count != 0)
            //{
            //    db.TutorConfirmationRequests.Where(i => i.CourseId == db.FacultyCourses.ElementAt(count).CourseId).Load();
            //    count--;
            //}


            //var firstNames = from i in db.TutorConfirmationRequests.Local
            //                   select (i.Tutor.FirstName);
            //var lastNames = from i in db.TutorConfirmationRequests.Local
            //                select (i.Tutor.LastName);
            //var courseNames = from i in db.TutorConfirmationRequests.Local
            //                  select (i.Cours.CourseName);
            ////int count = facultyNames.Count();
            //int count2 = firstNames.Count();
            //while (count2 != 0)
            //{
            //    string item = "Name: " + firstNames.ElementAt(count2) + " " + lastNames.ElementAt(count2) + "\nCourse: " + courseNames.ElementAt(count2);
            //    Pending1.Items.Add(item);
            //    count2--;
            //}

            ////tempFaculty.LastName = "Name: " + facultyNames + "\n Course: " + courseNames;
            ////facultyCoursViewSource.Source = tempFaculty.LastName;

            //facultyCoursViewSource.Source = facultyNames.FirstOrDefault();
            db.CurrentUsers.Load();
            db.FacultyCourses.Load();
            db.Faculties.Load();
            db.Courses.Load();
            db.Tutors.Load();
            db.TutorCourses.Load();
            db.TutorConfirmationRequests.Load();

            TutoringDB.TutorConfirmationRequest temp = new TutorConfirmationRequest();
            //TutoringDB.TutorConfirmationRequest temp2 = new TutorConfirmationRequest();
            temp.Tutor = db.Tutors.FirstOrDefault();
            temp.Cours = db.Courses.FirstOrDefault();
            db.TutorConfirmationRequests.Add(temp);
            //temp2.Tutor = db.Tutors.FirstOrDefault();
            //temp2.Cours = db.Courses.FirstOrDefault();
            //db.TutorConfirmationRequests.Add(temp2);
            string item = "i";

            TutoringDB.Faculty user = new Faculty();
            user = db.Faculties.Where(i => i.Username == db.CurrentUsers.FirstOrDefault().UserName).FirstOrDefault();
            List<TutoringDB.Cours> courseList = new List<TutoringDB.Cours>();
            foreach (var fc in db.FacultyCourses.Local)
            {
                if (fc.Faculty.Username == user.Username)
                    courseList.Add(fc.Cours);
            }
            List<TutoringDB.TutorConfirmationRequest> trList = new List<TutoringDB.TutorConfirmationRequest>();
            foreach (var tut in db.TutorConfirmationRequests.Local)
            {
                foreach (var c in courseList)
                {
                    if (tut.Cours.CourseName == c.CourseName)
                        trList.Add(tut);
                }
            }
            int count = trList.Count;
            while (count != 0)
            {

                item = trList.ElementAt(count-1).TutorId +"\nName: " + trList.ElementAt(count-1).Tutor.FirstName + " " + trList.ElementAt(count-1).Tutor.LastName + "\nCourse: " + trList.ElementAt(count-1).Cours.CourseName;
                Pending1.Items.Add(item);
                count--;
            }

        }
        private void CustomWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (finished) Application.Current.Shutdown();
        }

        private void Accept__Click(object sender, RoutedEventArgs e)
        {
            if(Pending1.SelectedItem == null)
            {
                label3.Content = "Select an Item";

            }
            string item2;
            item2  = Pending1.SelectedItem.ToString();
            char[] delimiterChars = { ' ' };
            var i = item2.Split(delimiterChars);
            string id = i.FirstOrDefault();
            foreach (var tt in db.TutorConfirmationRequests.Local)
            {
                if (tt.TutorId.ToString() == id)
                {
                    TutoringDB.TutorCourse temp3 = new TutorCourse();
                    temp3.TutorId = tt.TutorId;
                    temp3.CourseId = tt.CourseId;
                    db.TutorCourses.Add(temp3);
                    db.SaveChanges();
                    db.TutorCourses.Remove(temp3);
                    db.TutorConfirmationRequests.Remove(tt);
                    db.SaveChanges();
                }

            }
            
        }
    }
}
