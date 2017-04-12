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

namespace DisplayTables
{
    public partial class DisplayCourses : Form
    {
        public DisplayCourses()
        {
            InitializeComponent();
        }
        //EntityFramework dbcontext
        private TutoringDB.TutorDatabaseEntities dbcontext = new TutoringDB.TutorDatabaseEntities();
        private void DisplayCourses_Load(object sender, EventArgs e)
        {
            dbcontext.Courses
                .OrderBy(course => course.CourseName)
                .ThenBy(course => course.CourseNumber)
                .Load();

            //specify DataSource for tutorBindingSource
            coursBindingSource.DataSource = dbcontext.Courses.Local;
        }

        private void courseBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            TutoringDB.Tutor b = new TutoringDB.Tutor();
            //b.Id = 3; b.FirstName = "mike"; b.LastName = "Johnson"; b.UserName = "mikkee"; b.Password = "aloha"; b.Year = 2; b.Email = "mike@mike.mike";
            //dbcontext.Tutors.Add(b);
            Validate();
            coursBindingSource.EndEdit();
            try
            {
                dbcontext.SaveChanges();
            }
            catch (DbEntityValidationException)
            {
                MessageBox.Show("All Fields must contain values", "Entity Validation Exception");
            }
        }
    }
}
