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

namespace DisplayTables
{
    public partial class DisplayFaculty : Form
    {
        public DisplayFaculty()
        {
            InitializeComponent();
        }
        private TutoringDB.TutorDatabaseEntities dbcontext = new TutoringDB.TutorDatabaseEntities();
        private void DisplayFaculty_Load(object sender, EventArgs e)
        {
            dbcontext.Faculties
                .OrderBy(faculty => faculty.LastName)
                .ThenBy(faculty => faculty.First_Name)
                .Load();

            //specify DataSource for facultyBindingSource
            facultyBindingSource.DataSource = dbcontext.Faculties.Local;
        }

        private void facultyBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            Validate();
            facultyBindingSource.EndEdit();
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
