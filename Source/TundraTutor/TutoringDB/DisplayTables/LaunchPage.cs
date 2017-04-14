using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DisplayTables
{
    public partial class LaunchPage : Form
    {
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
    }
}
