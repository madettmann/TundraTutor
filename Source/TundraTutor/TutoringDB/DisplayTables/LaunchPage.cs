//Written by Makena
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace DisplayTables
{
    public partial class LaunchPage : Form
    {
        public static TutoringDB.Faculty f;
        public LaunchPage()
        {
            InitializeComponent();
        }

        private void tutorsbutton_Click(object sender, EventArgs e)
        {
            DisplayTutors f = new DisplayTutors();
            f.ShowDialog();
        }

        private void tuteesbutton_Click(object sender, EventArgs e)
        {
            DisplayTutees f = new DisplayTutees();
            f.Show();
        }

        private void coursesbutton_Click(object sender, EventArgs e)
        {
            DisplayCourses f = new DisplayCourses();
            f.Show();
        }

        private void displayFacultyButton_Click(object sender, EventArgs e)
        {
            DisplayFaculty f = new DisplayFaculty();
            f.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DisplayAppointments f = new DisplayAppointments();
            f.ShowDialog();
        }

        private void displayBusyTimesButton_Click(object sender, EventArgs e)
        {
            BusyTime f = new BusyTime();
            f.ShowDialog();
        }

        private void startEndButton_Click(object sender, EventArgs e)
        {
            SetStartEnd f = new SetStartEnd();
            f.ShowDialog();
        }

        private void fillFacultyButton_Click(object sender, EventArgs e)
        {

            TutoringDB.TutorDatabaseEntities db = new TutoringDB.TutorDatabaseEntities();
            db.Faculties.Load();
            db.Courses.Load();
            db.FacultyCourses.Load();
            //Clear database first
            foreach (TutoringDB.FacultyCours f in db.FacultyCourses)
            {
                db.FacultyCourses.Remove(f);
            }
            foreach (TutoringDB.Cours fc in db.Courses)
            {
                db.Courses.Remove(fc);
            }
            foreach (TutoringDB.Faculty f in db.Faculties)
            {
                db.Faculties.Remove(f);
            }
            db.SaveChanges();

            TutoringDB.Faculty tempFaculty = new TutoringDB.Faculty();
            TutoringDB.Cours tempCourse = new TutoringDB.Cours();
            TutoringDB.FacultyCours tempFacultyCourse = new TutoringDB.FacultyCours();
            //Read file line-by-line and load into database
            foreach(string line in File.ReadLines(@"courses-fall16.txt"))
            {
                db.Faculties.Load();
                //Fill Course Fields
                var pieces = line.Split('\t');
                tempCourse.CourseNumber = pieces[0];
                tempCourse.CourseName = pieces[1];
                var code = pieces[0].Split(' ');
                tempCourse.Department = code[0];
                db.Courses.Add(tempCourse);
                //Fill Faculty Fields
                tempFaculty.Username = tempFaculty.First_Name = pieces[2];
                tempFaculty.Password = tempFaculty.LastName = pieces[3];
                tempFaculty.Email = pieces[2] + pieces[3] + "@coe.edu";
                var temps = db.Faculties.Where(i => i.First_Name == tempFaculty.First_Name && i.LastName == tempFaculty.LastName);
                if (temps.Count() == 0)
                {
                    db.Faculties.Add(tempFaculty);
                    tempFacultyCourse.Faculty = tempFaculty;
                    tempFacultyCourse.Cours = tempCourse;
                    db.FacultyCourses.Add(tempFacultyCourse);
                    db.SaveChanges();
                }
                else
                {
                    var first = from i in db.Faculties
                                where i.Email == tempFaculty.Email
                                select i;
                    tempFacultyCourse.FacultyId = first.FirstOrDefault().Id;
                    tempFacultyCourse.Cours = tempCourse;
                    db.FacultyCourses.Add(tempFacultyCourse);
                    db.SaveChanges();
                }
            }
            MessageBox.Show("Successfully Loaded.");
        }

        private void fillTutorsTutees_Click(object sender, EventArgs e)
        {
            TutoringDB.TutorDatabaseEntities db = new TutoringDB.TutorDatabaseEntities();
            db.Tutors.Load();
            db.Tutees.Load();
            db.TutorCourses.Load();
            //Clear database first
            foreach (TutoringDB.Tutor t in db.Tutors)
            {
                db.Tutors.Remove(t);
            }
            foreach (TutoringDB.Tutee f in db.Tutees)
            {
                db.Tutees.Remove(f);
            }
            foreach(TutoringDB.TutorCourse tc in db.TutorCourses)
            {
                db.TutorCourses.Remove(tc);
            }
            db.SaveChanges();

            TutoringDB.Tutor tempTutor = new TutoringDB.Tutor();
            TutoringDB.Tutee tempTutee = new TutoringDB.Tutee();
            
            //Fill Tutors
            foreach (string line in File.ReadLines(@"fakeTutors.csv"))
            {
                db.Tutors.Load();
                db.Tutees.Load();
                db.TutorCourses.Load();
                var split = line.Split('|');
                tempTutor.FirstName = split[0];
                tempTutor.LastName = split[1];
                tempTutor.UserName = split[2];
                tempTutor.Password = split[3];
                tempTutor.Email = split[4];
                Random year = new Random();
                tempTutor.Year = year.Next(1, 4);
                db.Tutors.Add(tempTutor);

                //Randomly matches tutors with courses
                Random rnd = new Random();
                int limit = rnd.Next(4);
                db.SaveChanges();
                db.Courses.Load();
                //var courselist = db.Courses.Local;
                //for (int i = 0; i< limit; i++)
                //{
                //    int max = db.Courses.Count();
                //    tempTutorCourse.Cours = courselist[rnd.Next(0,max)];
                //    tempTutorCourse.TutorId = tempTutor.Id+1;
                //    db.TutorCourses.Add(tempTutorCourse);
                //}
                //db.SaveChanges();
            }
            //Fill Tutees
            foreach (string line in File.ReadLines(@"fakeTutees.csv"))
            {
                db.Tutors.Load();
                db.Tutees.Load();
                db.TutorCourses.Load();
                var split = line.Split('|');
                tempTutee.FirstName = split[0];
                tempTutee.LastName = split[1];
                tempTutee.Username = split[2];
                tempTutee.Password = split[3];
                tempTutee.Email = split[4];
                Random year = new Random();
                tempTutee.Year = year.Next(1, 4);
                db.Tutees.Add(tempTutee);
                db.SaveChanges();
            }
            List<TutoringDB.TutorCourse> tcList = new List<TutoringDB.TutorCourse>();
            db.Courses.Load();
            db.Tutors.Load();
            var tutorlist = db.Tutors.Local.ToList();
            var courselist = db.Courses.Local.ToList();
            int index = 0;
            foreach(TutoringDB.Cours c in courselist)
            {
                Random rnd = new Random();
                int lim = rnd.Next(2, 5);
                
                for(int i = 0; i< lim; i++)
                {
                    TutoringDB.TutorCourse tempTutorCourse = new TutoringDB.TutorCourse();
                    tempTutorCourse.Cours = c;
                    tempTutorCourse.Tutor = tutorlist[index % tutorlist.Count()];
                    index++;
                    tcList.Add(tempTutorCourse);
                }
                
            }
            for (int i = 0; i < tcList.Count; i++)
            {
                db.TutorCourses.Load();
                db.TutorCourses.Add(tcList.ElementAt(i));
                db.SaveChanges();
            }
            db.SaveChanges();
            MessageBox.Show("Successfully Loaded.");
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            TutoringDB.TutorDatabaseEntities db = new TutoringDB.TutorDatabaseEntities();
            db.TutorTuteeCourseAppointments.Load();
            foreach(var i in db.TutorTuteeCourseAppointments)
            {
                db.TutorTuteeCourseAppointments.Remove(i);
            }
            db.TutorTuteeNotifications.Load();
            foreach (var i in db.TutorTuteeNotifications)
            {
                db.TutorTuteeNotifications.Remove(i);
            }
            db.TutorConfirmationRequests.Load();
            foreach (var i in db.TutorConfirmationRequests)
            {
                db.TutorConfirmationRequests.Remove(i);
            }
            db.TutorCourses.Load();
            foreach (var i in db.TutorCourses)
            {
                db.TutorCourses.Remove(i);
            }
            db.TutorBusyTimes.Load();
            foreach (var i in db.TutorBusyTimes)
            {
                db.TutorBusyTimes.Remove(i);
            }
            db.FacultyCourses.Load();
            foreach (var i in db.FacultyCourses)
            {
                db.FacultyCourses.Remove(i);
            }
            db.TuteeBusyTimes.Load();
            foreach (var i in db.TuteeBusyTimes)
            {
                db.TuteeBusyTimes.Remove(i);
            }
            db.Appointments.Load();
            foreach (var i in db.Appointments)
            {
                db.Appointments.Remove(i);
            }
            db.BaseSchedules.Load();
            foreach (var i in db.BaseSchedules)
            {
                db.BaseSchedules.Remove(i);
            }
            db.BusyTimes.Load();
            foreach (var i in db.BusyTimes)
            {
                db.BusyTimes.Remove(i);
            }
            db.Courses.Load();
            foreach (var i in db.Courses)
            {
                db.Courses.Remove(i);
            }
            db.CurrentUsers.Load();
            foreach (var i in db.CurrentUsers)
            {
                db.CurrentUsers.Remove(i);
            }
            db.Faculties.Load();
            foreach (var i in db.Faculties)
            {
                db.Faculties.Remove(i);
            }
            db.StartEnds.Load();
            foreach (var i in db.StartEnds)
            {
                db.StartEnds.Remove(i);
            }
            db.Tutees.Load();
            foreach (var i in db.Tutees)
            {
                db.Tutees.Remove(i);
            }
            db.Tutors.Load();
            foreach (var i in db.Tutors)
            {
                db.Tutors.Remove(i);
            }
            db.SaveChanges();
            MessageBox.Show("Successfully Cleared.");
        }
    }
}
