using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TutorWindows
{
    /// <summary>
    /// Interaction logic for HelpWindow.xaml
    /// </summary>
    public partial class HelpWindow : TundraControls.CustomWindow
    {
        public HelpWindow()
        {
            InitializeComponent();
            string sub1 = "Change Password";
            string sub2 = "Logout";
            string sub3 = "Become Tutor";
            string sub4 = "Make Appointment";
            string sub5 = "Add Busy Time";
            Subjects.Items.Add(sub3);
            Subjects.Items.Add(sub1);
            Subjects.Items.Add(sub2);
            Subjects.Items.Add(sub4);
            Subjects.Items.Add(sub5);

        }


        private void Subjects_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Subjects.SelectedItem.ToString() == "Logout")
            {
                Info.Text = "On the main window screen, click the user icon. Once clicked a menu should come up" +
                    "with multiple options. Click the option that says logout.";
            }
            if (Subjects.SelectedItem.ToString() == "Change Password")
            {
                Info.Text = "On the main window screen, click the user icon. Once clicked a menu should come up" +
                    "with multiple options. Click the option that says account. A new window with your account information" +
                    "will show up. Enter old and new password where specified and close click change password.";
            }
            if (Subjects.SelectedItem.ToString() == "Become Tutor" )
            {
                Info.Text = "On the main window screen, click the user icon. Once clicked a menu should come up" +
                    "with multiple options. Click the option that says Tutor a Class. Choose which class you wish to tutor and click submit.";
            }
            if (Subjects.SelectedItem.ToString() == "Make Appointment")
            {
                Info.Text = "On the main window screen, click the schedule icon. Once clicked a menu should come up" +
                    "with multiple options. Click the option that states make an appointment. Specify which class" +
                    "and then specify which time frame. Then choose appointment time that works for you.";
            }
            if (Subjects.SelectedItem.ToString() == "Add Busy Time")
            {
                Info.Text = "On the main window screen, click the calendar icon. Once clicked a the week view should" +
                    "come up. Click on certain times of the week that you are busy. Click save icon on the left once finished.";
            }
        }
    }
}
