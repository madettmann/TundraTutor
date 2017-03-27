﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DisplayTables;
using System.Windows.Forms;

namespace TutorWindows
{

    public partial class Login : TundraControls.CustomWindow
    {
        public static TutoringDB.Tutee user;
        TutoringDB.TutorDatabaseEntities userCreds;
        bool usernamePasswordCorrect
        {
            get => (userCreds.Tutors.Any(user => user.UserName == Username1.Text && user.Password == Password1.Password) ||
                   userCreds.Tutees.Any(user => user.Username == Username1.Text && user.Password == Password1.Password) ||
                   userCreds.Faculties.Any(user => user.Username == Username1.Text && user.Password == Password1.Password));
        }

        public Login()
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            Username1.Focus();

            userCreds = new TutoringDB.TutorDatabaseEntities();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Username1.Text == "admin" && Password1.Password == "admin")
            {
                LaunchPage lp = new LaunchPage();
                lp.Show();
                TutoringDB.CurrentUser user = new TutoringDB.CurrentUser();
                user.UserName = Username1.Text;
                user.Type = "admin";
                userCreds.CurrentUsers.Load();
                if (userCreds.CurrentUsers.Count() > 0)
                {
                    foreach (TutoringDB.CurrentUser i in userCreds.CurrentUsers)
                    {
                        userCreds.CurrentUsers.Remove(i);
                    }
                }
                userCreds.CurrentUsers.Add(user);
                userCreds.SaveChanges();
            }

            else if (Username1.Text == "" && Password1.Password == "")
            {
                Label3.Content = "Please enter a username and password";
            }
            else if (usernamePasswordCorrect)
            {
                Label3.Content = "";
                TutoringDB.CurrentUser user = new TutoringDB.CurrentUser();
                user.UserName = Username1.Text;
                userCreds.CurrentUsers.Load();
                if(userCreds.CurrentUsers.Count() > 0)
                {
                    foreach (TutoringDB.CurrentUser i in userCreds.CurrentUsers){
                        userCreds.CurrentUsers.Remove(i);
                    }
                }
                userCreds.CurrentUsers.Add(user);
                userCreds.SaveChanges();
                StreamWriter saveUser = new StreamWriter("..\\..\\..\\Temp\\CurrentUser.txt", false);
                saveUser.WriteLine(Username1.Text);
                saveUser.Close();
                MainWindow monthView = new MainWindow();
                monthView.Show();
                this.Hide();
            }
            else
            {
                Label3.Content = "Incorrect username or passowrd";
            }
        }

        private void TundraButton_Click(object sender, RoutedEventArgs e)
        {
            AddAccount addAccountWindow = new AddAccount();
            addAccountWindow.ShowDialog();
        }

        private void CustomWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void Password1_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if(e.Key == Key.Return)
            {
                Button_Click(sender, e);
            }
        }

        private void Username1_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                Button_Click(sender, e);
            }
        }
    }
}
