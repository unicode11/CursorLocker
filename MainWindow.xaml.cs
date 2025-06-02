using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;

namespace CursorLocker2;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private bool isLocked;

    public MainWindow()
    {
        InitializeComponent();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        Focus();
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

    [DllImport("user32.dll")]
    private static extern bool ClipCursor(ref RECT rect);

    [DllImport("user32.dll")]
    private static extern bool ClipCursor(IntPtr rect);

    private void LockCursorToCenter()
    {
        var screen = Screen.PrimaryScreen.Bounds;
        var x = screen.Width / 2;
        var y = screen.Height / 2;

        var rect = new RECT
        {
            Left = x, Top = y, Right = x + 1, Bottom = y + 1
        };

        ClipCursor(ref rect);
    }

    private void UnlockCursor()
    {
        ClipCursor(IntPtr.Zero);
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct RECT
    {
        public int Left, Top, Right, Bottom;
    }
}