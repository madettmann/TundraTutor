//Written by Victor
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace TutorWindows
{

    public partial class MainWindow : TundraControls.CustomWindow
    {
        List<string> months;
        int selectedMonth;
        int currYear;
        bool finished;
        public ObservableCollection<Notification> Notifications;
        TutoringDB.CurrentUser user;
        TutoringDB.TutorDatabaseEntities readUser;
        public ICommand NotificationCommand { get; set; }

        enum MonthCounter { January = 1, February, March, April, May, June, July, August, September, October, November, December };
        MonthCounter calendarMonth;

        

        public MainWindow()
        {
            months = new List<string> { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
            

            user = new TutoringDB.CurrentUser();
            readUser = new TutoringDB.TutorDatabaseEntities();
            readUser.CurrentUsers.Load();
            var userList = from i in readUser.CurrentUsers select i;
            user = userList.FirstOrDefault();

            InitializeComponent();

            finished = true;

            monthLabel.Content = months.FirstOrDefault(w => w == DateTime.Today.ToString("MMMM"));

            selectedMonth = DateTime.Today.Month;
            currYear = DateTime.Today.Year;
            calendarMonth = (MonthCounter)DateTime.Today.Month;

            nextButton.Click += (o, e) => refreshCalendar(1);
            prevButton.Click += (o, e) => refreshCalendar(2);

            Notifications = new ObservableCollection<Notification>();
            NotificationCommand = new RelayCommand<object>(onNotificationClicked);
            //readUser.TutorTuteeNotifications.Load();
            //var nots = from i in readUser.TutorTuteeNotifications
            //           where i.Tutee.Username == user.UserName || i.Tutor.UserName == user.UserName
            //           select i;
            //foreach (var not in nots) Notifications.Add(new Notification(not.Message, not.Type, (int)not.targetId));
            //if (Notifications.Count == 0)
            //{
            //    Notifications.Add(new Notification("Nothing here!", "empty", -1));
            //}
            //else Notifications.Add(new Notification("Clear", "clear", -1));

            //NotificationsList.ItemsSource = Notifications;
            refreshNotifications();

            DataContext = this;
        }

        
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

        private void logoutButton_Click(object sender, RoutedEventArgs e)
        {
            finished = false;
            Login logoutScreen = new Login();
            logoutScreen.Show();
            
            this.Close();
        }

        private void CustomWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(finished && !user.Type.ToLower().Contains("admin")) Application.Current.Shutdown();
        }

        private void appointmentButton_Click_1(object sender, RoutedEventArgs e)
        {
            //ScheduleAppointment newAppt = new ScheduleAppointment();
            //newAppt.ShowDialog();
            AddAppointment f = new AddAppointment();
            f.ShowDialog();
            refreshNotifications();
            refreshCalendar(0);
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

        private void CustomWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.Width = SystemParameters.WorkArea.Width;
            this.Height = SystemParameters.WorkArea.Height;
            this.Left = SystemParameters.WorkArea.Left;
            this.Top = SystemParameters.WorkArea.Top;

            if (userMenu.Width == 0) { userMenu.Width = toolbarPanel.ActualWidth; userMenu.Height = toolbarPanel.ActualWidth; }
            if (notificationsMenu.Width == 0) { notificationsMenu.Width = toolbarPanel.ActualWidth; notificationsMenu.Height = toolbarPanel.ActualWidth; }
            if (actionsMenu.Width == 0) { actionsMenu.Width = toolbarPanel.ActualWidth; actionsMenu.Height = toolbarPanel.ActualWidth; }
        }

        private void TundraButton_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("No use helping you...");
            HelpWindow hWindow = new HelpWindow();
            hWindow.ShowDialog();
        }

        private void calendar_DayClick_1(object sender, RoutedEventArgs e)
        {
            //DayMenu dayView = new DayMenu(calendar.SelectedDate);
            //dayView.ShowDialog();
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            Credits creditsPage = new Credits();
            creditsPage.ShowDialog();
        }

        private void onNotificationClicked(object obj)
        {
            Notification selection = obj as Notification;
            switch (selection.Type)
            {
                case "clear":
                    Notifications.Clear();
                    Notifications.Add(new Notification("Nothing here!", "empty", -1));
                    var clearer = from i in readUser.TutorTuteeNotifications
                                  where i.Tutor.UserName == user.UserName || i.Tutee.Username == user.UserName
                                  select i;
                    foreach (var element in clearer)
                    {
                        readUser.TutorTuteeNotifications.Remove(element);
                    }
                    readUser.SaveChanges();
                    refreshNotifications();
                    break;
                case "empty":
                    break;
                case "TutorTuteeCourseAppointments":
                    if (readUser.TutorTuteeCourseAppointments.Any(appointment => appointment.Id == selection.Id))
                    {
                        var appt = readUser.TutorTuteeCourseAppointments.Where(appointment => appointment.Id == selection.Id).FirstOrDefault();
                        AppointmentInfo seeAppt = new AppointmentInfo(appt.Appointment.Date, appt.Appointment.Time);
                        seeAppt.ShowDialog();
                        refreshCalendar(0);
                    }
                    else MessageBox.Show("Appointment was cancelled");
                    break;

            };
            refreshNotifications();
        }

        private void modSchedButton_Click(object sender, RoutedEventArgs e)
        {
            BaseSchedule modSched = new BaseSchedule();
            modSched.ShowDialog();
        }

        private void requesttoTutorButton_Click(object sender, RoutedEventArgs e)
        {
            RequestTutor reqTutorWindow = new RequestTutor();
            reqTutorWindow.ShowDialog();
        }

        private void refreshNotifications()
        {
            Notifications = new ObservableCollection<Notification>();
            readUser.TutorTuteeNotifications.Load();
            var nots = from i in readUser.TutorTuteeNotifications
                       where i.Tutee.Username == user.UserName || i.Tutor.UserName == user.UserName
                       select i;
            foreach (var not in nots) Notifications.Add(new Notification(not.Message, not.Type, (int)not.targetId));
            if (Notifications.Count == 0)
            {
                Notifications.Add(new Notification("Nothing here!", "empty", -1));
            }
            else Notifications.Add(new Notification("Clear", "clear", -1));

            NotificationsList.ItemsSource = Notifications;

            DataContext = null;
            DataContext = this;
        }
    }
}
