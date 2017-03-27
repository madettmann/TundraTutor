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
using System.Windows.Forms;
using System.ComponentModel;

namespace TutorWindows
{
    /// <summary>
    /// Interaction logic for AddBusy.xaml
    /// </summary>
    public partial class AddBusy : TundraControls.CustomWindow
    {
        public  void fillCombo()
        {

        }
        public AddBusy()
        {
            InitializeComponent();


        }

        private void submitButton_Click(object sender, RoutedEventArgs e)
        {
            //if(startTimeHourComboBox.SelectedIndex != -1 &&
            //    durationHourComboBox.SelectedIndex != -1)
            //{
            //    TutoringDB.BusyTime tempBusy = new TutoringDB.BusyTime();
            //    DateTime temp = new DateTime();
                
            //}
        }
    }
}
