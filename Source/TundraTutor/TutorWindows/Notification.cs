//Written by Victor
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TutorWindows
{
    public class Notification : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {

            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private string message;
        private string type;
        private int id;
        public string Message { get => message; set { message = value; NotifyPropertyChanged("Message"); } }
        public string Type { get => type; set => type = value; }
        public int Id { get => id; set => id = value; }

        public Notification(string mes, string typ, int ID) { message = mes; Type = typ; Id = ID; }
    }
}
