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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.Entity;
using System.Data.Entity.Validation;

namespace TutorWindows
{
    /// <summary>
    /// Interaction logic for AddAccount.xaml
    /// </summary>
    public partial class AddAccount : TundraControls.CustomWindow
    {
        public AddAccount()
        {
            InitializeComponent();
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            TutoringDB.TutorDatabaseEntities db = new TutoringDB.TutorDatabaseEntities();
            db.Tutees.Load();
            db.Tutors.Load();
            db.Faculties.Load();
            int numTutees = db.Tutees.Count();

            if(firstNameTextBox.Text != "" &&
                lastNameTextBox.Text != "" &&
                usernameTextBox.Text != "" &&
                passwordTextBox.Text != "" &&
                yearTextBox.Text != "" &&
                emailTextBox.Text != "")
            {
                TutoringDB.Tutee temp = new TutoringDB.Tutee();
                temp.FirstName = firstNameTextBox.Text;
                temp.LastName = lastNameTextBox.Text;
                temp.Username = usernameTextBox.Text;
                temp.Password = passwordTextBox.Text;
                temp.Year = Convert.ToInt32( yearTextBox.Text);
                temp.Email = emailTextBox.Text;
                //temp.Id = ;

                db.Tutees.Add(temp);
                db.SaveChanges();
                this.Close();
                Label1.Text = "";
            }
            else
            {
                Label1.Text = "Completely fill out form";
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Login loginWindow = new Login();
            loginWindow.Show();
            //this.Close();
        }
    }
}
