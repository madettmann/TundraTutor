This solution contains 2 projects, Custom Window Demo and My Cutom Controls.
to create a WPF page using the window theme in My Custom Controls, just start by copying the Demo /*project*/ code into another WPF Window set of files/*, rename and use a you need*/.
If you want to add a new project from scratch, create a new project in the solution called whatever you want. Left click the project on the
solution explorer and set it to the launch project. Alternativley, add the window template project to one you create and make sure it isn't set to load.
In order to user the theme, you'll need to add a reference (again via the solution explorer) to My Custom Controls (underneath the projects tab) 
in your new project. Then, you will need to make your MainWindow class inherit the custom window theme. To do so, you will need to change the 
MainWindow.xaml and MainWindow.xaml.cs files accordingly:

.xaml:
<control:CustomWindow x:Class="[Insert your project name here].MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:control="clr-namespace:MyCustomControls;assembly=MyCustomControls"
        Title="MainWindow" Height="350" Width="525">
    <Grid>
        <TextBlock Text="The content goes here />
    </Grid>
</control:CustomWindow>

.cs:
namespace [Insert your project name here]
{
    public partial class MainWindow : MyCustomControls.CustomWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }
    }
}
