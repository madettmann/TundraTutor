using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace TundraControls
{
    public class Day : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private DateTime date;
        private string notes;
        private bool enabled;
        private bool isTargetMonth;
        private bool isToday;
        private string appointmentInfo;
        private string appointmentInfo2;

        public string AppointmentInfo { get => appointmentInfo; set => appointmentInfo = value; }

        public string AppointmentInfo2 { get => appointmentInfo2; set => appointmentInfo2 = value; }

        public ObservableCollection<Appointment> appointmentList;

        public int NumAppointments => appointmentList.Count;

        public bool IsToday
        {
            get { return isToday; }
            set
            {
                isToday = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("IsToday"));
                }
            }
        }

        public bool IsTargetMonth
        {
            get { return isTargetMonth; }
            set
            {
                isTargetMonth = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("IsTargetMonth"));
                }
            }
        }

        public bool Enabled
        {
            get { return enabled; }
            set
            {
                enabled = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Enabled"));
                }
            }
        }

        public string Notes
        {
            get { return notes; }
            set
            {
                notes = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Notes"));
                }
            }
        }

        public DateTime Date
        {
            get { return date; }
            set
            {
                date = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Date"));
                }
            }
        }
        
    }
}
