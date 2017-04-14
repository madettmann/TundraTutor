using System;
using System.Windows;

namespace TundraControls
{
    public class Busy
    {
        public Busy() { }
        public Busy(TimeSpan t, TimeSpan dur, DateTime dat) { time = t; duration = dur; date = new DateTime(dat.Year, dat.Month, dat.Day); } // Is not saving the date properly

        private TimeSpan time;
        private TimeSpan duration;
        private DateTime date;

        public TimeSpan Time { get => time; set => time = value; }
        public TimeSpan Duration { get => duration; set => duration = value; }
        public DateTime Date { get => date; set => date = value; }
    }
}

