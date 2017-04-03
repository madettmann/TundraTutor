using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace TundraControls
{
    public class Timeslot : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public Timeslot(DateTime d, TimeSpan t, bool ip) { Date = d; Time = t; IsPassed = ip; }

        private DateTime date;
        private TimeSpan time;
        private ObservableCollection<Appointment> appointment;
        private ObservableCollection<Busy> busy;
        private bool isPassed;
        private string timeInfo;

        public DateTime Date { get => date; set { date = value; if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("Date")); } }
        public TimeSpan Time { get => time; set { time = value; if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("Time")); } }
        public ObservableCollection<Appointment >Appointment { get => appointment; set { appointment = value; if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("Appointment")); } }
        public bool IsPassed { get => isPassed; set { isPassed = value; if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("IsPassed")); } }
        public ObservableCollection<Busy> Busy { get => busy; set { busy = value; if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("Busy")); } }
        public string TimeInfo { get => timeInfo; set { timeInfo = value; if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("TimeInfo")); } }
        //Has to return either the appointment or busy info
        //Need to create a busy class
        public int BusyOrAppt { get { if (appointment.Count > 0) return 2; else if (busy.Count > 0) return 1; else return 0; } }
    }
}
