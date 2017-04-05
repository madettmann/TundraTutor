using System;
using System.Windows;

namespace TundraControls
{
    public class Busy
    {
        public Busy() { }
        public Busy(TimeSpan t) { time = t;  }

        private TimeSpan time;

        public TimeSpan Time { get => time; set => time = value; }
    }
}
