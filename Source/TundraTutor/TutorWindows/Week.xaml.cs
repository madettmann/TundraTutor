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
    public partial class Week : TundraControls.CustomWindow
    {
        bool finished;

        public Week()
        {
            Application.Current.MainWindow.Width = SystemParameters.WorkArea.Width;
            Application.Current.MainWindow.Height = SystemParameters.WorkArea.Height;
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;

            finished = true;

            InitializeComponent();
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
            AddAppointment newAppt = new AddAppointment();
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
    }
}
