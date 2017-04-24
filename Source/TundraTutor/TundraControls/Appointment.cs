//Written by Victor
using System;

namespace TundraControls
{
    public class Appointment
    {

        public Appointment()
            : this("Unknown Tutor", "Unknown Tutee", new TimeSpan(0, 0, 0), "Unknown Course")
        {
        }

        public Appointment(string tr, string te, TimeSpan ti, string cc)
        {
            Tutor = tr;
            Tutee = te;
            Time = ti;
            courseCode = cc;
        }

        public Appointment(string tr, string te, TimeSpan ti, string cc, DateTime d)
        {
            Tutor = tr;
            Tutee = te;
            Time = ti;
            courseCode = cc;
            Date = d;
        }

        public Appointment(Appointment toCopy)
        {
            Tutor = toCopy.Tutor;
            Tutee = toCopy.Tutee;
            Time = toCopy.Time;
            courseCode = toCopy.courseCode;
        }

        public string apptLabel => courseCode + " " + Time;

        public DateTime Date { get => date; set => date = value; }
        public string Tutor { get => tutor; set => tutor = value; }
        public string Tutee { get => tutee; set => tutee = value; }
        public TimeSpan Time { get => time; set => time = value; }

        private string tutor;
        private string tutee;
        private TimeSpan time;
        private DateTime date;
        public string courseCode;
    }
}
