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
            tutor = tr;
            tutee = te;
            time = ti;
            courseCode = cc;
        }

        public Appointment(string tr, string te, TimeSpan ti, string cc, DateTime d)
        {
            tutor = tr;
            tutee = te;
            time = ti;
            courseCode = cc;
            Date = d;
        }

        public Appointment(Appointment toCopy)
        {
            tutor = toCopy.tutor;
            tutee = toCopy.tutee;
            time = toCopy.time;
            courseCode = toCopy.courseCode;
        }

        public string apptLabel => courseCode + " " + time;

        public DateTime Date { get => date; set => date = value; }

        private string tutor;
        private string tutee;
        public TimeSpan time;
        private DateTime date;
        public string courseCode;
    }
}
