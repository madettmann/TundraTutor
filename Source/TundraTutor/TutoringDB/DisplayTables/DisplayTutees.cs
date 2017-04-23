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
    public partial class DisplayTutees : Form
    {
        public DisplayTutees()
        {
            InitializeComponent();
        }
        private TutoringDB.TutorDatabaseEntities dbcontext = new TutoringDB.TutorDatabaseEntities();
        private void DisplayTutees_Load(object sender, EventArgs e)
        {
            dbcontext.Tutees
                .OrderBy(tutee => tutee.LastName)
                .ThenBy(tutee => tutee.FirstName)
                .Load();

            //specify DataSource for tutorBindingSource
            tuteeBindingSource.DataSource = dbcontext.Tutees.Local;
        }

        private void tuteeBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            Validate();
            tuteeBindingSource.EndEdit();
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
