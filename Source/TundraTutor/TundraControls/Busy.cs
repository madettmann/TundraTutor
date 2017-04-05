using System;
using System.Windows;

namespace TundraControls
{
    public class Busy
    {
        public Busy() { }
        public Busy(TimeSpan t, TimeSpan d) { time = t; duration = d; }

        private TimeSpan time;
        private TimeSpan duration;

        public TimeSpan Time { get => time; set => time = value; }
        public TimeSpan Duration { get => duration; set => duration = value; }
    }
}
