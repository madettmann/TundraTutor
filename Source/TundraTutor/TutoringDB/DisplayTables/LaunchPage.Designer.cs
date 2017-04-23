namespace DisplayTables
{
    partial class LaunchPage
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tutorsbutton = new System.Windows.Forms.Button();
            this.tuteesbutton = new System.Windows.Forms.Button();
            this.coursesbutton = new System.Windows.Forms.Button();
            this.displayFacultyButton = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.displayBusyTimesButton = new System.Windows.Forms.Button();
            this.startEndButton = new System.Windows.Forms.Button();
            this.fillFacultyButton = new System.Windows.Forms.Button();
            this.fillTutorsTutees = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tutorsbutton
            // 
            this.tutorsbutton.Location = new System.Drawing.Point(13, 13);
            this.tutorsbutton.Name = "tutorsbutton";
            this.tutorsbutton.Size = new System.Drawing.Size(120, 23);
            this.tutorsbutton.TabIndex = 0;
            this.tutorsbutton.Text = "Display Tutors";
            this.tutorsbutton.UseVisualStyleBackColor = true;
            this.tutorsbutton.Click += new System.EventHandler(this.tutorsbutton_Click);
            // 
            // tuteesbutton
            // 
            this.tuteesbutton.Location = new System.Drawing.Point(13, 42);
            this.tuteesbutton.Name = "tuteesbutton";
            this.tuteesbutton.Size = new System.Drawing.Size(120, 23);
            this.tuteesbutton.TabIndex = 1;
            this.tuteesbutton.Text = "Display Tutees";
            this.tuteesbutton.UseVisualStyleBackColor = true;
            this.tuteesbutton.Click += new System.EventHandler(this.tuteesbutton_Click);
            // 
            // coursesbutton
            // 
            this.coursesbutton.Location = new System.Drawing.Point(13, 71);
            this.coursesbutton.Name = "coursesbutton";
            this.coursesbutton.Size = new System.Drawing.Size(120, 23);
            this.coursesbutton.TabIndex = 2;
            this.coursesbutton.Text = "Display Courses";
            this.coursesbutton.UseVisualStyleBackColor = true;
            this.coursesbutton.Click += new System.EventHandler(this.coursesbutton_Click);
            // 
            // displayFacultyButton
            // 
            this.displayFacultyButton.Location = new System.Drawing.Point(13, 100);
            this.displayFacultyButton.Name = "displayFacultyButton";
            this.displayFacultyButton.Size = new System.Drawing.Size(120, 23);
            this.displayFacultyButton.TabIndex = 3;
            this.displayFacultyButton.Text = "Display Faculty";
            this.displayFacultyButton.UseVisualStyleBackColor = true;
            this.displayFacultyButton.Click += new System.EventHandler(this.displayFacultyButton_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(13, 129);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(120, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "Display Appointments";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // displayBusyTimesButton
            // 
            this.displayBusyTimesButton.Location = new System.Drawing.Point(13, 158);
            this.displayBusyTimesButton.Name = "displayBusyTimesButton";
            this.displayBusyTimesButton.Size = new System.Drawing.Size(120, 23);
            this.displayBusyTimesButton.TabIndex = 5;
            this.displayBusyTimesButton.Text = "Display Busy Times";
            this.displayBusyTimesButton.UseVisualStyleBackColor = true;
            this.displayBusyTimesButton.Click += new System.EventHandler(this.displayBusyTimesButton_Click);
            // 
            // startEndButton
            // 
            this.startEndButton.Location = new System.Drawing.Point(13, 188);
            this.startEndButton.Name = "startEndButton";
            this.startEndButton.Size = new System.Drawing.Size(120, 23);
            this.startEndButton.TabIndex = 6;
            this.startEndButton.Text = "Set Start/End Date";
            this.startEndButton.UseVisualStyleBackColor = true;
            this.startEndButton.Click += new System.EventHandler(this.startEndButton_Click);
            // 
            // fillFacultyButton
            // 
            this.fillFacultyButton.Location = new System.Drawing.Point(139, 13);
            this.fillFacultyButton.Name = "fillFacultyButton";
            this.fillFacultyButton.Size = new System.Drawing.Size(120, 23);
            this.fillFacultyButton.TabIndex = 7;
            this.fillFacultyButton.Text = "Fill Faculty/Courses";
            this.fillFacultyButton.UseVisualStyleBackColor = true;
            this.fillFacultyButton.Click += new System.EventHandler(this.fillFacultyButton_Click);
            // 
            // fillTutorsTutees
            // 
            this.fillTutorsTutees.Location = new System.Drawing.Point(139, 42);
            this.fillTutorsTutees.Name = "fillTutorsTutees";
            this.fillTutorsTutees.Size = new System.Drawing.Size(120, 23);
            this.fillTutorsTutees.TabIndex = 8;
            this.fillTutorsTutees.Text = "Fill Tutors/Tutees";
            this.fillTutorsTutees.UseVisualStyleBackColor = true;
            this.fillTutorsTutees.Click += new System.EventHandler(this.fillTutorsTutees_Click);
            // 
            // LaunchPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.fillTutorsTutees);
            this.Controls.Add(this.fillFacultyButton);
            this.Controls.Add(this.startEndButton);
            this.Controls.Add(this.displayBusyTimesButton);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.displayFacultyButton);
            this.Controls.Add(this.coursesbutton);
            this.Controls.Add(this.tuteesbutton);
            this.Controls.Add(this.tutorsbutton);
            this.Name = "LaunchPage";
            this.Text = "Edit Tables";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button tutorsbutton;
        private System.Windows.Forms.Button tuteesbutton;
        private System.Windows.Forms.Button coursesbutton;
        private System.Windows.Forms.Button displayFacultyButton;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button displayBusyTimesButton;
        private System.Windows.Forms.Button startEndButton;
        private System.Windows.Forms.Button fillFacultyButton;
        private System.Windows.Forms.Button fillTutorsTutees;
    }
}