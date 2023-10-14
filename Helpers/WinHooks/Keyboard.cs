using System.Diagnostics;
using System.Runtime.InteropServices;

namespace LiteView.Helpers
{
    public class Keyboard
    {
        private const int WH_KEYBOARD_LL = 13;

        class ListenType
        {
            public Keys Key { get; }
            public Action<IntPtr> Action { get; }

            public ListenType(Keys key, Action<IntPtr> action)
            {
                Key = key;
                Action = action;
            }
        }

        private readonly IntPtr hook;
        private readonly List<ListenType> listenTypes;

        IntPtr CallBack(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                foreach (ListenType listenType in listenTypes)
                {
                    if (listenType.Key == (Keys)Marshal.ReadInt32(lParam))
                    {
                        listenType.Action.Invoke(wParam);
                    }
                }
            }

            return WinApi.CallNextHookEx(hook, nCode, wParam, lParam);
        }

        public Keyboard()
        {
            listenTypes = new List<ListenType>();
            hook = WinApi.SetWindowsHookEx(WH_KEYBOARD_LL, CallBack, WinApi.GetModuleHandle(null), 0);
        }

        public Func<bool> Listen(Keys key, Action<IntPtr> action)
        {
            ListenType listenType = new(key, action);
            listenTypes.Add(listenType);

            return () =>
            {
                listenTypes.Remove(listenType);
                return true;
            };
        }

        public void ClearKeys()
        {
            listenTypes.Clear();

            if (WinApi.UnhookWindowsHookEx(hook) == 0)
            {
                Debug.WriteLine("Error");
            }
            else
            {

            }
        }
    }
}
