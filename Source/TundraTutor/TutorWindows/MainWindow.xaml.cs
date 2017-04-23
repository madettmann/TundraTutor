<<<<<<< HEAD
﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Windows;

namespace TutorWindows
{

    public partial class MainWindow : TundraControls.CustomWindow
    {
        List<string> months;
        int selectedMonth;
        int currYear;
        bool finished;

        enum MonthCounter { January = 1, February, March, April, May, June, July, August, September, October, November, December };
        MonthCounter calendarMonth;

        public MainWindow()
        {
            Application.Current.MainWindow.Width = SystemParameters.WorkArea.Width;
            Application.Current.MainWindow.Height = SystemParameters.WorkArea.Height;
            //Application.Current.MainWindow.Left = SystemParameters.WorkArea.Left;
            //Application.Current.MainWindow.Top = SystemParameters.WorkArea.Top;
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;

            months = new List<string> { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };

            InitializeComponent();

            finished = true;

            monthLabel.Content = months.FirstOrDefault(w => w == DateTime.Today.ToString("MMMM"));

            selectedMonth = DateTime.Today.Month;
            currYear = DateTime.Today.Year;
            calendarMonth = (MonthCounter)DateTime.Today.Month;

            nextButton.Click += (o, e) => refreshCalendar(1);
            prevButton.Click += (o, e) => refreshCalendar(2);

            TutoringDB.TutorDatabaseEntities reset = new TutoringDB.TutorDatabaseEntities();
            reset.BusyTimes.Load();
            reset.BaseSchedules.Load();
            foreach(var item in reset.BusyTimes) { reset.BusyTimes.Remove(item); }
            foreach(var item in reset.BaseSchedules) { reset.BaseSchedules.Remove(item); }
            
        }

        //There is work to be done here, the calendar numbers can often get mixed up badly when 
        //clicking through years
        private void refreshCalendar(int x)
        {
            switch (x)
            {
                case 1:
                    selectedMonth = (((selectedMonth - 1) + 1) % 12) + 1;
                    if (selectedMonth == 0) { currYear += 1; selectedMonth++; } 
                    calendarMonth = (MonthCounter)selectedMonth;
                    break;
                case 2:
                    if (selectedMonth != 1)
                    {
                        selectedMonth = (((selectedMonth - 1) - 1) % 12) + 1;
                    }
                    //Handle the going back a year crashing
                    else { selectedMonth = 12; currYear -= 1; }
                    calendarMonth = (MonthCounter)selectedMonth;
                    break;
                default:
                    break;
            }
            DateTime targetDate = new DateTime(currYear, selectedMonth, 1);
            //monthLabel.Content = months.FirstOrDefault(w => w == DateTime.Today.Month.ToString("MMMM"));
            monthLabel.Content = calendarMonth.ToString();

            calendar.BuildCalendar(targetDate);
        }

        private void calendar_DayChanged(object sender, TundraControls.DayChangedEventArgs e)
        {
            //throw new NotImplementedException();
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
            //ScheduleAppointment newAppt = new ScheduleAppointment();
            //newAppt.ShowDialog();
            AddAppointment f = new AddAppointment();
            f.ShowDialog();
        }

        private void infoButton_Click(object sender, RoutedEventArgs e)
        {
            AccountInfo infoPage = new AccountInfo();
            infoPage.ShowDialog();
        }

        private void weekButton_Click(object sender, RoutedEventArgs e)
        {
            finished = false;
            Week week = new Week();
            week.Show();
            this.Close();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            AddBusy addBusyWindow = new AddBusy();
            addBusyWindow.ShowDialog();
        }

        private void CustomWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.Width = SystemParameters.WorkArea.Width;
            this.Height = SystemParameters.WorkArea.Height;
            this.Left = SystemParameters.WorkArea.Left;
            this.Top = SystemParameters.WorkArea.Top;
        }

        private void TundraButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("No use helping you...");
        }

        private void calendar_DayClick(object sender, RoutedEventArgs e)
        {

        }

        private void calendar_DayClick_1(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Clicked " + calendar.SelectedDate.ToShortDateString());
        }
    }
}
=======
﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Windows;

namespace TutorWindows
{

    public partial class MainWindow : TundraControls.CustomWindow
    {
        List<string> months;
        int selectedMonth;
        int currYear;
        bool finished;

        enum MonthCounter { January = 1, February, March, April, May, June, July, August, September, October, November, December };
        MonthCounter calendarMonth;

        public MainWindow()
        {
            Application.Current.MainWindow.Width = SystemParameters.WorkArea.Width;
            Application.Current.MainWindow.Height = SystemParameters.WorkArea.Height;
            //Application.Current.MainWindow.Left = SystemParameters.WorkArea.Left;
            //Application.Current.MainWindow.Top = SystemParameters.WorkArea.Top;
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;

            months = new List<string> { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };

            InitializeComponent();

            finished = true;

            monthLabel.Content = months.FirstOrDefault(w => w == DateTime.Today.ToString("MMMM"));

            selectedMonth = DateTime.Today.Month;
            currYear = DateTime.Today.Year;
            calendarMonth = (MonthCounter)DateTime.Today.Month;

            nextButton.Click += (o, e) => refreshCalendar(1);
            prevButton.Click += (o, e) => refreshCalendar(2);

            TutoringDB.TutorDatabaseEntities reset = new TutoringDB.TutorDatabaseEntities();
            reset.BusyTimes.Load();
            reset.BaseSchedules.Load();
            foreach(var item in reset.BusyTimes) { reset.BusyTimes.Remove(item); }
            foreach(var item in reset.BaseSchedules) { reset.BaseSchedules.Remove(item); }
            
        }

        //There is work to be done here, the calendar numbers can often get mixed up badly when 
        //clicking through years
        private void refreshCalendar(int x)
        {
            switch (x)
            {
                case 1:
                    selectedMonth = (((selectedMonth - 1) + 1) % 12) + 1;
                    if (selectedMonth == 0) { currYear += 1; selectedMonth++; } 
                    calendarMonth = (MonthCounter)selectedMonth;
                    break;
                case 2:
                    if (selectedMonth != 1)
                    {
                        selectedMonth = (((selectedMonth - 1) - 1) % 12) + 1;
                    }
                    //Handle the going back a year crashing
                    else { selectedMonth = 12; currYear -= 1; }
                    calendarMonth = (MonthCounter)selectedMonth;
                    break;
                default:
                    break;
            }
            DateTime targetDate = new DateTime(currYear, selectedMonth, 1);
            //monthLabel.Content = months.FirstOrDefault(w => w == DateTime.Today.Month.ToString("MMMM"));
            monthLabel.Content = calendarMonth.ToString();

            calendar.BuildCalendar(targetDate);
        }

        private void calendar_DayChanged(object sender, TundraControls.DayChangedEventArgs e)
        {
            //throw new NotImplementedException();
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

        private void weekButton_Click(object sender, RoutedEventArgs e)
        {
            finished = false;
            Week week = new Week();
            week.Show();
            this.Close();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            AddBusy addBusyWindow = new AddBusy();
            addBusyWindow.ShowDialog();
        }

        private void CustomWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.Width = SystemParameters.WorkArea.Width;
            this.Height = SystemParameters.WorkArea.Height;
            this.Left = SystemParameters.WorkArea.Left;
            this.Top = SystemParameters.WorkArea.Top;
        }

        private void TundraButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("No use helping you...");
        }

        private void calendar_DayClick(object sender, RoutedEventArgs e)
        {

        }

        private void calendar_DayClick_1(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Clicked " + calendar.SelectedDate.ToShortDateString());
        }
    }
}
>>>>>>> parent of 5bbedca... Merge pull request #10 from madettmann/master
