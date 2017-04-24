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
            Refresh1();
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

            //TutoringDB.TutorConfirmationRequest temp2 = new TutorConfirmationRequest();
            //temp.Tutor = db.Tutors.FirstOrDefault();
            //temp.Cours = db.Courses.FirstOrDefault();
            //db.TutorConfirmationRequests.Add(temp);
            //temp2.Tutor = db.Tutors.FirstOrDefault();
            //temp2.Cours = db.Courses.FirstOrDefault();
            //db.TutorConfirmationRequests.Add(temp2);


        }
        private void CustomWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (finished) Application.Current.Shutdown();
        }

        private void Accept__Click(object sender, RoutedEventArgs e)
        {
            if (Pending1.SelectedItem == null)
            {
                label3.Content = "Select an Item";

            }
            string item2;
            item2 = Pending1.SelectedItem.ToString();
            char[] delimiterChars = { ' ' };
            var i = item2.Split(delimiterChars);
            string id = i.FirstOrDefault();
            TutoringDB.TutorCourse temp3 = new TutorCourse();
            TutoringDB.TutorConfirmationRequest temp4 = new TutorConfirmationRequest();
            foreach (var tt in db.TutorConfirmationRequests.Local)
            {
                if (tt.TutorId.ToString() == id)
                {
                    temp4 = tt;
                    temp3.TutorId = tt.TutorId;
                    temp3.CourseId = tt.CourseId;

                }

            }
            db.TutorCourses.Add(temp3);
            db.TutorConfirmationRequests.Remove(temp4);
            db.SaveChanges();
            Refresh1();

        }

        private void Courses_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            refresh2();
        }

        private void Apply_Click(object sender, RoutedEventArgs e)
        {
            db.Tutors.Load();
            db.Tutees.Load();
            db.Courses.Load();
            db.TutorCourses.Load();
            string item2;
            item2 = PotentialTutors.SelectedItem.ToString();
            char[] delimiterChars = { ' ' };
            var i = item2.Split(delimiterChars);
            string fn = i.ElementAt(0);
            string ln = i.ElementAt(1);
            TutoringDB.TutorCourse temp3 = new TutorCourse();
            TutoringDB.Cours tempc2 = new Cours();
            foreach (var c in db.Tutors)
            {
                if (fn == c.FirstName && ln == c.LastName)
                {
                    TutoringDB.Tutor tt = new Tutor();
                    tt = c;
                    temp3.Tutor = tt;

                    foreach (var l in db.Courses)
                    {
                        if (l.CourseName == Courses.SelectedItem.ToString())
                        {
                            tempc2 = l;
                        }
                    }

                }
            }
            if (temp3 != null)
            {
                temp3.Cours = tempc2;
                db.TutorCourses.Add(temp3);
                db.SaveChanges();
                refresh2();
                return;
            }
            TutoringDB.Tutee temp = new TutoringDB.Tutee();
            foreach (var d in db.Tutees)
            {
                if (fn == d.FirstName && ln == d.LastName)
                {

                    temp = d;

                }
            }
            TutoringDB.Tutor temp2 = new TutoringDB.Tutor();
            temp2.FirstName = temp.FirstName;
            temp2.LastName = temp.LastName;
            temp2.Password = temp.Password;
            temp2.UserName = temp.Username;
            temp2.Year = temp.Year;
            temp2.Email = temp.Email;
            temp2.Id = db.Tutors.Count();
            db.Tutors.Add(temp2);
            db.Tutees.Remove(temp);
            db.SaveChanges();
            TutoringDB.Cours tempc = new Cours();
            foreach (var l in db.Courses)
            {
                if (l.CourseName == Courses.SelectedItem.ToString())
                {
                    tempc = l;
                }
            }
            TutoringDB.TutorCourse temp4 = new TutorCourse();
            temp4.Cours = tempc;
            temp4.Tutor = temp2;
            db.TutorCourses.Add(temp4);
            db.SaveChanges();
            refresh2();
        }

        private void Deny_Click(object sender, RoutedEventArgs e)
        {
            db.TutorConfirmationRequests.Load();
            string item;
            item = Pending1.SelectedItem.ToString();
            char[] delimiterChars = { ' ' };
            var i = item.Split(delimiterChars);
            string id = item.ElementAt(0).ToString();
            foreach (var tt in db.TutorConfirmationRequests.Local)
            {
                if (tt.TutorId.ToString() == id)
                    db.TutorConfirmationRequests.Remove(tt);

            }
            Refresh1();

        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            TutoringDB.TutorCourse temp3 = new TutorCourse();
            TutoringDB.Cours tempc2 = new Cours();
            TutoringDB.Tutor tt = new Tutor();
            db.Tutors.Load();
            db.Courses.Load();
            db.TutorCourses.Load();
            string item;
            item = CurrentTutors.SelectedItem.ToString();
            char[] delimiterChars = { ' ' };
            var i = item.Split(delimiterChars);
            string fn = i.ElementAt(0).ToString();
            string ln = i.ElementAt(1).ToString();
            foreach (var c in db.Tutors)
            {
                if (fn == c.FirstName && ln == c.LastName)
                {
                    tt = c;


                    foreach (var l in db.Courses)
                    {
                        if (l.CourseName == Courses.SelectedItem.ToString())
                        {
                            tempc2 = l;

                        }
                    }

                }
            }
            temp3.Cours = tempc2;
            temp3.CourseId = tempc2.Id;
            temp3.Tutor = tt;
            temp3.TutorId = tt.Id;
            foreach (var x in db.TutorCourses)
            {
                if (temp3.Cours == x.Cours && temp3.Tutor == x.Tutor)
                    temp3 = x;
            }
            db.TutorCourses.Remove(temp3);
            db.SaveChanges();
            refresh2();
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            finished = false;
            Login logoutScreen = new Login();
            logoutScreen.Show();

            this.Close();
        }

        public void Refresh1()
        {
            int cd = Pending1.Items.Count;
            while (cd != 0)
            {
                Pending1.Items.RemoveAt(cd - 1);
                cd--;
            }

            db.CurrentUsers.Load();
            db.FacultyCourses.Load();
            db.Faculties.Load();
            db.Courses.Load();
            db.Tutors.Load();
            db.TutorCourses.Load();
            db.TutorConfirmationRequests.Load();
            db.Tutees.Load();

            TutoringDB.TutorConfirmationRequest temp = new TutorConfirmationRequest();
            string item = "i";

            TutoringDB.Faculty user = new Faculty();
            user = db.Faculties.Where(i => i.Username == db.CurrentUsers.FirstOrDefault().UserName).FirstOrDefault();
            List<TutoringDB.Cours> courseList = new List<TutoringDB.Cours>();
            foreach (var fc in db.FacultyCourses.Local)
            {
                if (fc.Faculty.Username == user.Username)
                    courseList.Add(fc.Cours);
            }
            int count2 = courseList.Count;
            while (count2 != 0)
            {
                item = courseList.ElementAt(count2 - 1).CourseName;
                Courses.Items.Add(item);
                count2--;
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

                item = trList.ElementAt(count - 1).TutorId + " \nName: " + trList.ElementAt(count - 1).Tutor.FirstName + " " + trList.ElementAt(count - 1).Tutor.LastName + "\nCourse: " + trList.ElementAt(count - 1).Cours.CourseName;
                Pending1.Items.Add(item);
                count--;
            }
        }
        public void refresh2()
        {
            int c = CurrentTutors.Items.Count;
            while (c != 0)
            {
                CurrentTutors.Items.RemoveAt(c - 1);
                c--;
            }
            int c2 = PotentialTutors.Items.Count;
            while (c2 != 0)
            {
                PotentialTutors.Items.RemoveAt(c2 - 1);
                c2--;
            }
            List<TutoringDB.TutorCourse> tutList = new List<TutorCourse>();
            foreach (var tutr in db.TutorCourses)
            {
                if (tutr.Cours.CourseName == Courses.SelectedItem.ToString())
                    tutList.Add(tutr);
            }
            int count3 = tutList.Count;
            while (count3 != 0)
            {
                string item = tutList.ElementAt(count3 - 1).Tutor.FirstName + " " + tutList.ElementAt(count3 - 1).Tutor.LastName;
                CurrentTutors.Items.Add(item);
                count3--;
            }

            List<TutoringDB.Tutor> tutorList = new List<Tutor>();
            foreach (var tutr in db.Tutors)
            {
                tutorList.Add(tutr);
                foreach (var tutor in tutList)
                {
                    if (tutr.Id == tutor.Tutor.Id)
                        tutorList.Remove(tutr);
                }
            }
            count3 = tutorList.Count();
            while (count3 != 0)
            {
                string item = tutorList.ElementAt(count3 - 1).FirstName + " " + tutorList.ElementAt(count3 - 1).LastName;
                PotentialTutors.Items.Add(item);
                count3--;
            }
            List<TutoringDB.Tutee> tuteeList = new List<Tutee>();
            foreach (var tute in db.Tutees)
            {
                tuteeList.Add(tute);
            }
            count3 = tuteeList.Count();
            while (count3 != 0)
            {
                string item = tuteeList.ElementAt(count3 - 1).FirstName + " " + tuteeList.ElementAt(count3 - 1).LastName;
                PotentialTutors.Items.Add(item);
                count3--;
            }
        }

    }
}