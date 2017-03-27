using System;

namespace TundraControls
{
    public class Appointment
    {

        public Appointment()
            : this("John Doe", "Jinkies McGee", new TimeSpan(0, 0, 0), "CS-125")
        {
        }

        public Appointment(string tr, string te, TimeSpan ti, string cc)
        {
            tutor = tr;
            tutee = te;
            time = ti;
            courseCode = cc;
        }

        public Appointment(Appointment toCopy)
        {
            tutor = toCopy.tutor;
            tutee = toCopy.tutee;
            time = toCopy.time;
            courseCode = toCopy.courseCode;
        }

        public string apptLabel => courseCode + " " + time;

        private string tutor;
        private string tutee;
        public TimeSpan time;
        public string courseCode;
    }
}
