//Written By Makena
//This screen was eventually replaced by the click on calendar to add busy
using System;
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

namespace TutorWindows
{
    /// <summary>
    /// Interaction logic for AddBusy.xaml
    /// </summary>
    public partial class AddBusy : TundraControls.CustomWindow
    {
        private DateTime timeSelected;
        private DateTime timeSpan;
        TutoringDB.TutorDatabaseEntities db = new TutoringDB.TutorDatabaseEntities();
        
        public AddBusy()
        {
            InitializeComponent();
            dateDatePicker.DisplayDate = DateTime.Today;
        }

        private void submitButton_Click(object sender, RoutedEventArgs e)
        {
            if ((string)durationTimeMenu.Tag == "true" && (string)startTimeMenu.Tag == "true") {
                db.BusyTimes.Load();
                db.CurrentUsers.Load();
                TutoringDB.BusyTime tempBusyTime = new TutoringDB.BusyTime();
                tempBusyTime.Date = (DateTime)dateDatePicker.SelectedDate;
                tempBusyTime.Time = timeSelected.TimeOfDay;
                tempBusyTime.Id = db.BusyTimes.Count();
                tempBusyTime.Duration = timeSpan.TimeOfDay;
                db.BusyTimes.Add(tempBusyTime);
                if (db.CurrentUsers.First().Type == "tutee")
                    {
                    db.Tutees.Load();
                        db.TuteeBusyTimes.Load();
                        TutoringDB.TuteeBusyTime joiner = new TutoringDB.TuteeBusyTime();
                        joiner.BusyTimeId = tempBusyTime.Id;
                        TutoringDB.Tutee tutee = db.Tutees.Where(i => i.Username == db.CurrentUsers.FirstOrDefault().UserName).First();
                        joiner.TuteeId = tutee.Id;
                        joiner.Id = db.TuteeBusyTimes.Count();
                        db.TuteeBusyTimes.Add(joiner);
                    }
                else
                {
                    db.TutorBusyTimes.Load();
                    db.Tutors.Load();
                    TutoringDB.TutorBusyTime joiner = new TutoringDB.TutorBusyTime();
                    joiner.BusyTimesId = tempBusyTime.Id;
                    TutoringDB.Tutor tutor = db.Tutors.Where(i => i.UserName == db.CurrentUsers.FirstOrDefault().UserName).First();
                    joiner.TutorId = tutor.Id;
                    joiner.Id = db.TutorBusyTimes.Count();
                    db.TutorBusyTimes.Add(joiner);
                }
                db.SaveChanges();
                this.Close();
            }
            else
            {
                MessageBox.Show("All Fields Must be Filled to Add Busy Time");
            }
        }
        private void TimeSelected(object sender, RoutedEventArgs e)
        {
            MenuItem m = sender as MenuItem;
            timeSelected = Convert.ToDateTime(m.Tag);
            selectItemMenuItem.Header = timeSelected.TimeOfDay;
            startTimeMenu.Tag = "true";
        }

        private void DurationSelected(object sender, RoutedEventArgs e)
        {
            MenuItem m = sender as MenuItem;
            timeSpan = Convert.ToDateTime(m.Tag);
            selectDurationMenuItem.Header = timeSpan.TimeOfDay;
            durationTimeMenu.Tag = "true";
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
