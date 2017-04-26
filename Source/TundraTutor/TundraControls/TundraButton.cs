//Written by Victor
using System.Windows;
using System.Windows.Controls;

namespace TundraControls
{
    public class TundraButton : Button
    {
        static TundraButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TundraButton), //Ensure the template is applied
            new FrameworkPropertyMetadata(typeof(TundraButton)));
        }
    }
}
