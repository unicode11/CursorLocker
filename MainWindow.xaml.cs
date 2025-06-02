using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Forms;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;

namespace CursorLocker2;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private bool isLocked = false;

    public MainWindow()
    {
        InitializeComponent();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        this.Focus();
    }

    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.F11)
        {
            if (!isLocked)
            {
                LockCursorToCenter();
                asd.Text = "Locked.";
            }
            else
            {
                UnlockCursor();
                asd.Text = "Unlocked.";
            }

            isLocked = !isLocked;
        }
    }

    // Lock to center using WinAPI
    [DllImport("user32.dll")]
    static extern bool ClipCursor(ref RECT rect);

    [DllImport("user32.dll")]
    static extern bool ClipCursor(IntPtr rect);

    [StructLayout(LayoutKind.Sequential)]
    private struct RECT
    {
        public int Left, Top, Right, Bottom;
    }

    private void LockCursorToCenter()
    {
        var screen = Screen.PrimaryScreen.Bounds;
        int x = screen.Width / 2;
        int y = screen.Height / 2;

        RECT rect = new RECT
        {
            Left = x,
            Top = y,
            Right = x + 1,
            Bottom = y + 1
        };

        ClipCursor(ref rect);
    }

    private void UnlockCursor()
    {
        ClipCursor(IntPtr.Zero);
    }
}