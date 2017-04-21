//Written by Victor
using System.Windows;
using System.Windows.Controls;

namespace TundraControls
{
    public class TundraButton : Button
    {
        static TundraButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TundraButton),
            new FrameworkPropertyMetadata(typeof(TundraButton)));
        }
    }
}
