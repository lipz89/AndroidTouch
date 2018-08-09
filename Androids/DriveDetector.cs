using System;
using System.Runtime.InteropServices;

namespace Androids
{
    /// <summary>
    /// 监听设备的插入和拔出
    /// </summary>
    public class DriveDetector
    {
        public event EventHandler<EventArgs> UsbChanged = null;
        /// <summary>
        /// 设备插入事件
        /// </summary>
        public event EventHandler<DriveDectctorEventArgs> DeviceArrived = null;
        /// <summary>
        /// 设备拔出事件
        /// </summary>
        public event EventHandler<DriveDectctorEventArgs> DeviceRemoved = null;
        /// <summary>
        /// 消息处理(HwndSourceHook委托的签名)
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="msg"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <param name="handled"></param>
        /// <returns></returns>
        public IntPtr WndProc(
            IntPtr hwnd,
            int msg,
            IntPtr wParam,
            IntPtr lParam,
            ref bool handled)
        {
            if (msg == NativeConstants.WM_DEVICECHANGE)
            {
                switch (wParam.ToInt32())
                {
                    case NativeConstants.WM_USBCHANGED:
                        {
                            UsbChanged?.Invoke(this, EventArgs.Empty);
                        }
                        break;
                    case NativeConstants.DBT_DEVICEARRIVAL:
                        {
                            var devType = Marshal.ReadInt32(lParam, 4);
                            if (devType == NativeConstants.DBT_DEVTYP_VOLUME)
                            {
                                var drive = GetDrive(lParam);
                                if (DeviceArrived != null)
                                {
                                    var args = new DriveDectctorEventArgs(drive);
                                    DeviceArrived(this, args); //触发设备插入事件
                                }
                            }
                        }
                        break;
                    case NativeConstants.DBT_DEVICEREMOVECOMPLETE:
                        {
                            var devType = Marshal.ReadInt32(lParam, 4);
                            if (devType == NativeConstants.DBT_DEVTYP_VOLUME)
                            {
                                var drive = GetDrive(lParam);
                                if (DeviceRemoved != null)
                                {
                                    var args = new DriveDectctorEventArgs(drive);
                                    DeviceRemoved(this, args);
                                }
                            }
                        }
                        break;
                }
            }
            return IntPtr.Zero;
        }
        private static string GetDrive(IntPtr lParam)
        {
            var volume = (DEV_BROADCAST_VOLUME)Marshal.PtrToStructure(lParam, typeof(DEV_BROADCAST_VOLUME));
            var letter = GetLetter(volume.dbcv_unitmask);
            return string.Format("{0}:\\", letter);
        }
        /// <summary>
        /// 获得盘符
        /// </summary>
        /// <param name="dbcvUnitmask">
        /// 1 = A
        /// 2 = B
        /// 4 = C...
        /// </param>
        /// <returns>结果是A~Z的任意一个字符或者为'?'</returns>
        private static char GetLetter(uint dbcvUnitmask)
        {
            const char nona = '?';
            const string drives = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            if (dbcvUnitmask == 0) return nona;
            var i = 0;
            var pom = dbcvUnitmask >> 1;
            while (pom != 0)
            {
                pom = pom >> 1;
                i++;
            }
            if (i < drives.Length)
                return drives[i];
            return nona;
        }
        /*  
          private static void GetLetterTest()
          {
              for (int i = 0; i < 67108864; i++)
              {
                  Console.WriteLine("{0} - {1}", i, GetLetter((uint)i));
                  i = i << 1;
              }
    //0 - ?
    //1 - A
    //3 - B
    //7 - C
    //15 - D
    //31 - E
    //63 - F
    //127 - G
    //255 - H
    //511 - I
    //1023 - J
    //2047 - K
    //4095 - L
    //8191 - M
    //16383 - N
    //32767 - O
    //65535 - P
    //131071 - Q
    //262143 - R
    //524287 - S
    //1048575 - T
    //2097151 - U
    //4194303 - V
    //8388607 - W
    //16777215 - X
    //33554431 - Y
    //67108863 - Z
          }*/
        /// <summary>
        /// 设备插入或拔出事件
        /// </summary>
        public class DriveDectctorEventArgs : EventArgs
        {
            /// <summary>
            /// 获得设备卷标
            /// </summary>
            public string Drive { get; private set; }
            public DriveDectctorEventArgs(string drive)
            {
                Drive = drive ?? string.Empty;
            }
        }
        #region Win32 API
        public partial class NativeConstants
        {
            public const int WM_USBCHANGED = 7;
            /// WM_DEVICECHANGE -> 0x0219
            public const int WM_DEVICECHANGE = 537;
            /// BROADCAST_QUERY_DENY -> 0x424D5144
            //public const int BROADCAST_QUERY_DENY = 1112363332;
            //public const int DBT_DEVTYP_DEVICEINTERFACE = 5;
            //public const int DBT_DEVTYP_HANDLE = 6;
            public const int DBT_DEVICEARRIVAL = 0x8000; // system detected a new device
            //public const int DBT_DEVICEQUERYREMOVE = 0x8001;   // Preparing to remove (any program can disable the removal)
            public const int DBT_DEVICEREMOVECOMPLETE = 0x8004; // removed 
            public const int DBT_DEVTYP_VOLUME = 0x00000002; // drive type is logical volume
        }
        [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
        public struct DEV_BROADCAST_VOLUME
        {
            /// DWORD->unsigned int
            public uint dbcv_size;
            /// DWORD->unsigned int
            public uint dbcv_devicetype;
            /// DWORD->unsigned int
            public uint dbcv_reserved;
            /// DWORD->unsigned int
            public uint dbcv_unitmask;
            /// WORD->unsigned short
            public ushort dbcv_flags;
        }
        [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
        public struct OVERLAPPED
        {
            /// ULONG_PTR->unsigned int
            public uint Internal;
            /// ULONG_PTR->unsigned int
            public uint InternalHigh;
            /// Anonymous_7416d31a_1ce9_4e50_b1e1_0f2ad25c0196
            public Anonymous_7416d31a_1ce9_4e50_b1e1_0f2ad25c0196 Union1;
            /// HANDLE->void*
            public System.IntPtr hEvent;
        }
        [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Explicit)]
        public struct Anonymous_7416d31a_1ce9_4e50_b1e1_0f2ad25c0196
        {
            /// Anonymous_ac6e4301_4438_458f_96dd_e86faeeca2a6
            [System.Runtime.InteropServices.FieldOffsetAttribute(0)]
            public Anonymous_ac6e4301_4438_458f_96dd_e86faeeca2a6 Struct1;
            /// PVOID->void*
            [System.Runtime.InteropServices.FieldOffsetAttribute(0)]
            public System.IntPtr Pointer;
        }
        [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
        public struct Anonymous_ac6e4301_4438_458f_96dd_e86faeeca2a6
        {
            /// DWORD->unsigned int
            public uint Offset;
            /// DWORD->unsigned int
            public uint OffsetHigh;
        }
        [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
        public struct SECURITY_ATTRIBUTES
        {
            /// DWORD->unsigned int
            public uint nLength;
            /// LPVOID->void*
            public System.IntPtr lpSecurityDescriptor;
            /// BOOL->int
            [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.Bool)]
            public bool bInheritHandle;
        }
        #endregion
    }
}