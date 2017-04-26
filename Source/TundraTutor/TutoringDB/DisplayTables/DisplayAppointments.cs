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
    public partial class DisplayAppointments : Form
    {
        public DisplayAppointments()
        {
            InitializeComponent();
        }
        //EntityFramework dbcontext
        private TutoringDB.TutorDatabaseEntities dbcontext = new TutoringDB.TutorDatabaseEntities();
        private void DisplayAppointment_Load(object sender, EventArgs e)
        {
            dbcontext.Appointments
                .OrderBy(appointment => appointment.Time)
                .Load();

            //specify DataSource for appointmentBindingSource
            appointmentBindingSource.DataSource = dbcontext.Faculties.Local;
        }

        private void appointmentBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            Validate();
            appointmentBindingSource.EndEdit();
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
