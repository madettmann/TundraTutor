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
    /// <summary>
    /// Interaction logic for RequestTutor.xaml
    /// </summary>
    public partial class RequestTutor : TundraControls.CustomWindow
    {
        TutoringDB.TutorDatabaseEntities db = new TutoringDB.TutorDatabaseEntities();
        public RequestTutor()
        {
            InitializeComponent();
            db.CurrentUsers.Load();
            db.Courses.Load();
            db.TutorConfirmationRequests.Load();
            db.TutorCourses.Load();
            if (db.CurrentUsers.FirstOrDefault().Type == "tutee")
            {
                List<TutoringDB.Cours> courseList = new List<TutoringDB.Cours>();
                foreach (var t in db.Courses)
                {
                    courseList.Add(t);
                }
                int count = courseList.Count();
                while (count != 0)
                {
                    CourseList.Items.Add(courseList.ElementAt(count - 1).CourseName);
                    count--;
                }
            }
            else
            {
                List<TutoringDB.Cours> courseList = new List<TutoringDB.Cours>();
                List<TutoringDB.TutorConfirmationRequest> confList = new List<TutoringDB.TutorConfirmationRequest>();
                List<TutoringDB.TutorCourse> tutcorsList = new List<TutoringDB.TutorCourse>();
                foreach (var t in db.Courses)
                {
                    courseList.Add(t);
                }
                foreach (var conf in db.TutorConfirmationRequests)
                {
                    if (db.CurrentUsers.FirstOrDefault().UserName == conf.Tutor.UserName)
                        confList.Add(conf);
                }
                foreach (var tut in db.TutorCourses)
                {
                    if (db.CurrentUsers.FirstOrDefault().UserName == tut.Tutor.UserName)
                        tutcorsList.Add(tut);
                }
                foreach (var cor in db.Courses)
                {
                    foreach (var confi in confList)
                    {
                        if (confi.Cours.CourseName == cor.CourseName)
                        {
                            courseList.Remove(cor);
                        }
                        else
                        {
                            foreach (var tutor in tutcorsList)
                            {
                                if (tutor.Cours.CourseName == cor.CourseName)
                                    courseList.Remove(cor);
                            }
                        }
                    }
                }
                courseList = courseList.OrderBy(i => i.CourseName).ToList();
                int count = courseList.Count;
                while (count != 0)
                {
                    CourseList.Items.Add(courseList.ElementAt(count - 1).CourseName);
                    count--;
                }
            }
        }

        private void Apply_Click(object sender, RoutedEventArgs e)
        {
            db.CurrentUsers.Load();
            db.TutorCourses.Load();
            db.Tutees.Load();
            db.Tutors.Load();
            db.Courses.Load();
            db.TutorConfirmationRequests.Load();
            TutoringDB.Tutee temp = new TutoringDB.Tutee();
            TutoringDB.Tutor temp2 = new TutoringDB.Tutor();
            TutoringDB.Cours tempcs = new TutoringDB.Cours();
            TutoringDB.TutorConfirmationRequest temptcs = new TutoringDB.TutorConfirmationRequest();
            if (db.CurrentUsers.FirstOrDefault().Type == "tutee")
            {
                foreach (var tte in db.Tutees)
                {
                    if (db.CurrentUsers.FirstOrDefault().UserName == tte.Username)
                        temp = tte;
                }
                temp2.FirstName = temp.FirstName;
                temp2.LastName = temp.LastName;
                temp2.Password = temp.Password;
                temp2.UserName = temp.Username;
                temp2.Year = temp.Year;
                temp2.Email = temp.Email;
                //temp2.Id = db.Tutors.Count();
                //K:LWEKFN:OINF:EWIOLFNSMDNF:OILEKNF:SOIDLKFNEOIL?JFEWFDSFS
                db.Tutors.Add(temp2);
                //db.Tutees.Remove(temp);
                //FKLJFEPLDKSFNPNEIKNELFPDIMOS:LDKFN:OEIJS
                db.SaveChanges();
            }

            foreach (var tut in db.Tutors)
            {
                if (db.CurrentUsers.FirstOrDefault().UserName == tut.UserName)
                    temp2 = tut;
            }
            foreach (var cs in db.Courses)
            {
                if (cs.CourseName == CourseList.SelectedItem.ToString())
                {
                    tempcs = cs;
                    temptcs.Cours = tempcs;
                    temptcs.Tutor = temp2;
                }
            }
            db.TutorConfirmationRequests.Add(temptcs);
            db.SaveChanges();
            this.Close();
        }
    }
}