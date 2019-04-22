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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Runtime.InteropServices;
using System.Windows.Threading;

namespace Lenovo_Autokeyboard_Backlight
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        enum LightStatus { Off, Mid, Full, Overwrite };
        LightStatus _LightStatus = LightStatus.Off;
        Keyboard_Core.KeyboardControl LaptopKeyboard = new Keyboard_Core.KeyboardControl();
        private KeyboardHook KeyboardHook;
        Point DragPoint = new Point();
        bool Draging = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            KeyboardHook = new KeyboardHook();
            KeyboardHook.KeyDown += new KeyboardHook.HookEventHandler(OnHookKeyDown);
        }

        async void OnHookKeyDown(object sender, HookEventArgs e)
        {
            if (_LightStatus == LightStatus.Off)
            {
                try
                {
                    SetKeyboardState(LightStatus.Full);
                }
                catch { }
                await Timer();
            }
            else
            {
                SetKeyboardState(LightStatus.Overwrite);
            }
        }

        async Task Timer()
        {
            await Task.Delay(Int32.Parse(DelayTextBox.Text));
            while (_LightStatus != LightStatus.Off)
            {
                try
                {
                    if (_LightStatus == LightStatus.Mid)
                    {
                        SetKeyboardState(LightStatus.Off);
                    }

                    if (_LightStatus == LightStatus.Full)
                    {
                        SetKeyboardState(LightStatus.Mid);
                    }

                    if (_LightStatus == LightStatus.Overwrite)
                    {
                        SetKeyboardState(LightStatus.Full);
                    }
                }
                catch { }
                await Task.Delay(Int32.Parse(DelayTextBox.Text));
            }
        }

        void SetKeyboardState(LightStatus State)
        {
            if (State == LightStatus.Off)
            {
                LaptopKeyboard.SetKeyboardBackLightStatus(0);
                StatusSlider.Value = 0;
                StatusTextBlock.Content = "Off";
            }
            if (State == LightStatus.Mid)
            {
                LaptopKeyboard.SetKeyboardBackLightStatus(1);
                StatusSlider.Value = 1;
                StatusTextBlock.Content = "Mid";
            }
            if (State == LightStatus.Full)
            {
                LaptopKeyboard.SetKeyboardBackLightStatus(2);
                StatusSlider.Value = 2;
                StatusTextBlock.Content = "Full";
            }
            if (State == LightStatus.Overwrite)
            {
                LaptopKeyboard.SetKeyboardBackLightStatus(2);
                StatusSlider.Value = 2;
                StatusTextBlock.Content = "Full";
            }
            _LightStatus = State;
        }

        private void SetTextBoxToOnlyNumbers(object sender, TextChangedEventArgs e)
        {
            string OutString = "";
            TextBox SenderTextBox = sender as TextBox;
            foreach (char Cha in SenderTextBox.Text)
            {
                if (Cha >= '0' && Cha <= '9' || Cha == ',' && SenderTextBox.Text.Count(f => f == ',') == 1)
                {
                    OutString += Cha;
                }
            }
            SenderTextBox.Text = OutString;
            SenderTextBox.SelectionLength = 0;
            SenderTextBox.SelectionStart = SenderTextBox.Text.Length;
        }

        private void DragBarGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragPoint = e.GetPosition(this);
            Draging = true;

            Mouse.Capture(DragBarGrid);
        }

        private void DragBarGrid_MouseMove(object sender, MouseEventArgs e)
        {
            if (Draging)
            {
                Left = (System.Windows.Forms.Cursor.Position.X - DragPoint.X);
                Top = (System.Windows.Forms.Cursor.Position.Y - DragPoint.Y);
            }
        }

        private void DragBarGrid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Draging = false;
            Mouse.Capture(null);
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            LaptopKeyboard.SetKeyboardBackLightStatus(0);
            Application.Current.Shutdown();
        }
        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }
    }

    public class KeyboardHook
    {
        #region pinvoke details

        private enum HookType : int
        {
            WH_JOURNALRECORD = 0,
            WH_JOURNALPLAYBACK = 1,
            WH_KEYBOARD = 2,
            WH_GETMESSAGE = 3,
            WH_CALLWNDPROC = 4,
            WH_CBT = 5,
            WH_SYSMSGFILTER = 6,
            WH_MOUSE = 7,
            WH_HARDWARE = 8,
            WH_DEBUG = 9,
            WH_SHELL = 10,
            WH_FOREGROUNDIDLE = 11,
            WH_CALLWNDPROCRET = 12,
            WH_KEYBOARD_LL = 13,
            WH_MOUSE_LL = 14
        }

        public struct KBDLLHOOKSTRUCT
        {
            public UInt32 vkCode;
            public UInt32 scanCode;
            public UInt32 flags;
            public UInt32 time;
            public IntPtr extraInfo;
        }

        [DllImport("user32.dll")]
        private static extern IntPtr SetWindowsHookEx(
            HookType code, HookProc func, IntPtr instance, int threadID);

        [DllImport("user32.dll")]
        private static extern int UnhookWindowsHookEx(IntPtr hook);

        [DllImport("user32.dll")]
        private static extern int CallNextHookEx(
            IntPtr hook, int code, IntPtr wParam, ref KBDLLHOOKSTRUCT lParam);

        #endregion

        HookType _hookType = HookType.WH_KEYBOARD_LL;
        IntPtr _hookHandle = IntPtr.Zero;
        HookProc _hookFunction = null;

        // hook method called by system
        private delegate int HookProc(int code, IntPtr wParam, ref KBDLLHOOKSTRUCT lParam);

        // events
        public delegate void HookEventHandler(object sender, HookEventArgs e);
        public event HookEventHandler KeyDown;
        public event HookEventHandler KeyUp;

        public KeyboardHook()
        {
            _hookFunction = new HookProc(HookCallback);
            Install();
        }

        ~KeyboardHook()
        {
            Uninstall();
        }

        // hook function called by system
        private int HookCallback(int code, IntPtr wParam, ref KBDLLHOOKSTRUCT lParam)
        {
            if (code < 0)
                return CallNextHookEx(_hookHandle, code, wParam, ref lParam);

            // KeyUp event
            if ((lParam.flags & 0x80) != 0 && this.KeyUp != null)
                this.KeyUp(this, new HookEventArgs(lParam.vkCode));

            // KeyDown event
            if ((lParam.flags & 0x80) == 0 && this.KeyDown != null)
                this.KeyDown(this, new HookEventArgs(lParam.vkCode));

            return CallNextHookEx(_hookHandle, code, wParam, ref lParam);
        }

        private void Install()
        {
            // make sure not already installed
            if (_hookHandle != IntPtr.Zero)
                return;

            // need instance handle to module to create a system-wide hook
            System.Reflection.Module[] list = System.Reflection.Assembly.GetExecutingAssembly().GetModules();
            System.Diagnostics.Debug.Assert(list != null && list.Length > 0);

            // install system-wide hook
            _hookHandle = SetWindowsHookEx(_hookType,
                _hookFunction, Marshal.GetHINSTANCE(list[0]), 0);
        }

        private void Uninstall()
        {
            if (_hookHandle != IntPtr.Zero)
            {
                // uninstall system-wide hook
                UnhookWindowsHookEx(_hookHandle);
                _hookHandle = IntPtr.Zero;
            }
        }
    }

    // The callback method converts the low-level keyboard data into something more .NET friendly with the HookEventArgs class.

    public class HookEventArgs : EventArgs
    {
        // using Windows.Forms.Keys instead of Input.Key since the Forms.Keys maps
        // to the Win32 KBDLLHOOKSTRUCT virtual key member, where Input.Key does not
        public System.Windows.Forms.Keys Key;
        public bool Alt;
        public bool Control;
        public bool Shift;

        public HookEventArgs(UInt32 keyCode)
        {
            // detect what modifier keys are pressed, using 
            // Windows.Forms.Control.ModifierKeys instead of Keyboard.Modifiers
            // since Keyboard.Modifiers does not correctly get the state of the 
            // modifier keys when the application does not have focus
            this.Key = (System.Windows.Forms.Keys)keyCode;
            this.Alt = (System.Windows.Forms.Control.ModifierKeys & System.Windows.Forms.Keys.Alt) != 0;
            this.Control = (System.Windows.Forms.Control.ModifierKeys & System.Windows.Forms.Keys.Control) != 0;
            this.Shift = (System.Windows.Forms.Control.ModifierKeys & System.Windows.Forms.Keys.Shift) != 0;
        }
    }
}
