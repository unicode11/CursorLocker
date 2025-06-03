using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace CursorLocker2;

public partial class MainWindow : Window
{
    private const int HOTKEY_ID_F11 = 9000;
    private const int HOTKEY_ID_F10 = 9001;
    private const uint MOD_NONE = 0x0000;
    private const uint VK_F11 = 0x7A;
    private const uint VK_F10 = 0x79;

    private bool isCursorLocked;
    private bool isCursorVisible = true;

    public MainWindow()
    {
        InitializeComponent();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        var hwnd = new WindowInteropHelper(this).Handle;
        var source = HwndSource.FromHwnd(hwnd);
        source.AddHook(HwndHook);

        RegisterHotKey(hwnd, HOTKEY_ID_F11, MOD_NONE, VK_F11);
    }

    private void Window_Closed(object sender, EventArgs e)
    {
        var hwnd = new WindowInteropHelper(this).Handle;
        UnregisterHotKey(hwnd, HOTKEY_ID_F11);
    }

    private IntPtr HwndHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
    {
        const int WM_HOTKEY = 0x0312;

        if (msg == WM_HOTKEY)
        {
            var id = wParam.ToInt32();
            if (id == HOTKEY_ID_F11)
            {
                ToggleCursorLock();
                handled = true;
            }
        }

        return IntPtr.Zero;
    }

    private void ToggleCursorLock()
    {
        if (!isCursorLocked)
        {
            LockCursorToCenter();
            asd.Text = "Cursor locked.";
        }
        else
        {
            UnlockCursor();
            asd.Text = "Cursor unlocked.";
        }

        isCursorLocked = !isCursorLocked;
    }
    

    private void LockCursorToCenter()
    {
        var screen = Screen.PrimaryScreen.Bounds;
        var x = screen.Width / 2;
        var y = screen.Height / 2;

        var rect = new RECT
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

    [DllImport("user32.dll")]
    private static extern bool ClipCursor(ref RECT rect);

    [DllImport("user32.dll")]
    private static extern bool ClipCursor(IntPtr rect);

    [DllImport("user32.dll")]
    private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

    [DllImport("user32.dll")]
    private static extern bool UnregisterHotKey(IntPtr hWnd, int id);
    


    [StructLayout(LayoutKind.Sequential)]
    private struct RECT
    {
        public int Left, Top, Right, Bottom;
    }
}