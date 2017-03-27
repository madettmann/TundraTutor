using System.Windows;
using System.Windows.Controls;

namespace TundraControls
{
    public class TundraToolbar : ToolBar
    {
        static TundraToolbar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TundraToolbar),
            new FrameworkPropertyMetadata(typeof(TundraToolbar)));
        }
    }
}
