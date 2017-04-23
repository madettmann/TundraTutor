<<<<<<< HEAD
﻿using System;
using System.Windows;

namespace TutorWindows
{
    /// <summary>
    /// Interaction logic for BaseSchedule.xaml
    /// </summary>
    public partial class BaseSchedule : TundraControls.CustomWindow
    {
        public BaseSchedule()
        {
            InitializeComponent();
        }

        private void CustomWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.Width = SystemParameters.WorkArea.Width;
            this.Height = SystemParameters.WorkArea.Height;
            this.Left = SystemParameters.WorkArea.Left;
            this.Top = SystemParameters.WorkArea.Top;
        }

        private void scheduler_TimeClick(object sender, RoutedEventArgs e)
        {
            scheduler.markTime(scheduler.SelectedIndex);
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void confirmButton_Click(object sender, RoutedEventArgs e)
        {
            scheduler.saveMarked();
            scheduler.BuildWeek();
        }

        private void scheduler_DOWClicked(object sender, RoutedEventArgs e)
        {
            for (int box = scheduler.SelectedIndex; box < 168; box+=7)
            {
                scheduler.markTime(box);
            }
        }
    }
}
=======
﻿using System;
using System.Windows;

namespace TutorWindows
{
    /// <summary>
    /// Interaction logic for BaseSchedule.xaml
    /// </summary>
    public partial class BaseSchedule : TundraControls.CustomWindow
    {
        public BaseSchedule()
        {
            InitializeComponent();
        }

        private void CustomWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.Width = SystemParameters.WorkArea.Width;
            this.Height = SystemParameters.WorkArea.Height;
            this.Left = SystemParameters.WorkArea.Left;
            this.Top = SystemParameters.WorkArea.Top;
        }

        private void scheduler_TimeClick(object sender, RoutedEventArgs e)
        {
            scheduler.markTime(scheduler.SelectedIndex);
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void confirmButton_Click(object sender, RoutedEventArgs e)
        {
            scheduler.saveMarked();
            scheduler.BuildWeek();
        }

        private void scheduler_DOWClicked(object sender, RoutedEventArgs e)
        {
            for (int box = scheduler.SelectedIndex; box < 168; box+=7)
            {
                scheduler.markTime(box);
            }
        }
    }
}
>>>>>>> parent of 5bbedca... Merge pull request #10 from madettmann/master
