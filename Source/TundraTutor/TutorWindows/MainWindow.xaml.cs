using System;
using System.Collections.Generic;
using System.IO;
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

namespace TutorWindows
{

    public partial class MainWindow : TundraControls.CustomWindow
    {
        List<string> months;
        int selectedMonth;
        int currYear;
        string user;
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

            StreamReader getUser = new StreamReader("..\\..\\..\\Temp\\CurrentUser.txt");
            user = getUser.ReadLine();
            getUser.Close();

            finished = true;

            monthLabel.Content = months.FirstOrDefault(w => w == DateTime.Today.ToString("MMMM"));

            selectedMonth = DateTime.Today.Month;
            currYear = DateTime.Today.Year;
            calendarMonth = (MonthCounter)DateTime.Today.Month;

            nextButton.Click += (o, e) => refreshCalendar(1);
            prevButton.Click += (o, e) => refreshCalendar(2);
            
        }

        //There is work to be done here, the calendar numbers can often get mixed up badly when 
        //vlicking through several months
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
            StreamWriter eraseUser = new StreamWriter("..\\..\\..\\Temp\\CurrentUser.txt", false);
            eraseUser.WriteLine("");
            eraseUser.Close();
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
            AddAppointment newAppt = new AddAppointment();
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
    }
}
