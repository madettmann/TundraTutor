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
    public partial class SetStartEnd : Form
    {
        TutoringDB.TutorDatabaseEntities db = new TutoringDB.TutorDatabaseEntities();
        public SetStartEnd()
        {
            InitializeComponent();
        }

        private void updateButton_Click(object sender, EventArgs e)
        {
            if (endDateTimePicker.Value < startDateTimePicker.Value)
            {
                MessageBox.Show("End Date must be after Start Date.");
            }
            else
            {
                if (db.StartEnds.Count() > 0)
                {
                    foreach (TutoringDB.StartEnd i in db.StartEnds)
                    {
                        db.StartEnds.Remove(i);
                    }
                }
                TutoringDB.StartEnd tempStartEnd = new TutoringDB.StartEnd();
                tempStartEnd.StartDate = startDateTimePicker.Value;
                tempStartEnd.EndDate = endDateTimePicker.Value;
                db.StartEnds.Add(tempStartEnd);
                MessageBox.Show("Database Updated.");
                db.SaveChanges();
                this.Close();
            }
        }

        private void SetStartEnd_Load(object sender, EventArgs e)
        {
            db.StartEnds.Load();
            if(db.StartEnds.Count() > 0)
            {
                TutoringDB.StartEnd temp = new TutoringDB.StartEnd();
                temp = db.StartEnds.FirstOrDefault();
                startDateTimePicker.Value = Convert.ToDateTime(temp.StartDate);
                endDateTimePicker.Value = Convert.ToDateTime(temp.EndDate);
            }
        }
    }
}
