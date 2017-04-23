<<<<<<< HEAD
﻿using System;
using System.Windows;

namespace TutorWindows
{
    public partial class Week : TundraControls.CustomWindow
    {
        private bool finished;
        private DateTime selectedDate;

        public Week()
        {
            Width = SystemParameters.WorkArea.Width;
            Height = SystemParameters.WorkArea.Height;
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            finished = true;
            selectedDate = DateTime.Today;

            InitializeComponent();

            nextButton.Click += (o,e) => refreshWeek(7);
            prevButton.Click += (o, e) => refreshWeek(-7);
        }

        private void refreshWeek(int x)
        {
            selectedDate = selectedDate.AddDays(x);
            week.BuildWeek(selectedDate);
        }

        private void logoutButton_Click(object sender, RoutedEventArgs e)
        {
            finished = false;

            Login logoutScreen = new Login();
            logoutScreen.Show();
            this.Close();
        }

        private void CustomWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(finished) Application.Current.Shutdown();
        }

        private void appointmentButton_Click_1(object sender, RoutedEventArgs e)
        {
            ScheduleAppointment newAppt = new ScheduleAppointment();
            newAppt.ShowDialog();
        }

        private void infoButton_Click(object sender, RoutedEventArgs e)
        {
            AccountInfo infoPage = new AccountInfo();
            infoPage.ShowDialog();
        }

        private void monthButton_Click(object sender, RoutedEventArgs e)
        {
            finished = false;
            MainWindow month = new MainWindow();
            month.Show();
            this.Close();
        }

        private void CustomWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.Width = SystemParameters.WorkArea.Width;
            this.Height = SystemParameters.WorkArea.Height;
            this.Left = SystemParameters.WorkArea.Left;
            this.Top = SystemParameters.WorkArea.Top;
        }

        private void week_TimeClick(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("Clicked" + week.SelectedDate.ToShortDateString() + " " + week.SelectedTime.ToString());
            week.markTime(week.SelectedIndex);
        }

        private void saveChangesButton_Click(object sender, RoutedEventArgs e)
        {
            week.saveMarked();
            refreshWeek(0);
        }

        private void modSchedButton_Click(object sender, RoutedEventArgs e)
        {
            BaseSchedule modSched = new BaseSchedule();
            modSched.ShowDialog();
        }
    }
}
=======
﻿using System;
using System.Windows;

namespace TutorWindows
{
    public partial class Week : TundraControls.CustomWindow
    {
        private bool finished;
        private DateTime selectedDate;

        public Week()
        {
            Width = SystemParameters.WorkArea.Width;
            Height = SystemParameters.WorkArea.Height;
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            finished = true;
            selectedDate = DateTime.Today;

            InitializeComponent();

            nextButton.Click += (o,e) => refreshWeek(7);
            prevButton.Click += (o, e) => refreshWeek(-7);
        }

        private void refreshWeek(int x)
        {
            selectedDate = selectedDate.AddDays(x);
            week.BuildWeek(selectedDate);
        }

        private void logoutButton_Click(object sender, RoutedEventArgs e)
        {
            finished = false;

            Login logoutScreen = new Login();
            logoutScreen.Show();
            this.Close();
        }

        private void CustomWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(finished) Application.Current.Shutdown();
        }

        private void appointmentButton_Click_1(object sender, RoutedEventArgs e)
        {
            ScheduleAppointment newAppt = new ScheduleAppointment();
            newAppt.ShowDialog();
        }

        private void infoButton_Click(object sender, RoutedEventArgs e)
        {
            AccountInfo infoPage = new AccountInfo();
            infoPage.ShowDialog();
        }

        private void monthButton_Click(object sender, RoutedEventArgs e)
        {
            finished = false;
            MainWindow month = new MainWindow();
            month.Show();
            this.Close();
        }

        private void CustomWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.Width = SystemParameters.WorkArea.Width;
            this.Height = SystemParameters.WorkArea.Height;
            this.Left = SystemParameters.WorkArea.Left;
            this.Top = SystemParameters.WorkArea.Top;
        }

        private void week_TimeClick(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("Clicked" + week.SelectedDate.ToShortDateString() + " " + week.SelectedTime.ToString());
            week.markTime(week.SelectedIndex);
        }

        private void saveChangesButton_Click(object sender, RoutedEventArgs e)
        {
            week.saveMarked();
            refreshWeek(0);
        }

        private void modSchedButton_Click(object sender, RoutedEventArgs e)
        {
            BaseSchedule modSched = new BaseSchedule();
            modSched.ShowDialog();
        }
    }
}
>>>>>>> parent of 5bbedca... Merge pull request #10 from madettmann/master
