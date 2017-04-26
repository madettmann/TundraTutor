//Written by Makena
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TutorWindows
{
    /// <summary>
    /// Interaction logic for Splash.xaml
    /// </summary>
    public partial class Splash : Window
    {
        public Splash()
        {
            InitializeComponent();
            this.Width = SystemParameters.WorkArea.Width;
            this.Height = this.Width * 536 / 1660;
            this.Left = SystemParameters.WorkArea.Left;
            this.Top = SystemParameters.WorkArea.Height/2 + this.Height/2;
            splashImage.Height = this.Height;
            splashImage.Width = this.Width;
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
    }
}
