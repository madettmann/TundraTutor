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
    public partial class BusyTime : Form
    {
        public BusyTime()
        {
            InitializeComponent();
        }
        private TutoringDB.TutorDatabaseEntities dbcontext = new TutoringDB.TutorDatabaseEntities();
        private void DisplayBusyTime_Load(object sender, EventArgs e)
        {
            dbcontext.BusyTimes
                .OrderBy(busytime => busytime.Time)
                .Load();

            //specify DataSource for busyTimeBindingSource
            busyTimeBindingSource.DataSource = dbcontext.Faculties.Local;
        }
        private void busyTimeBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            Validate();
            busyTimeBindingSource.EndEdit();
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
