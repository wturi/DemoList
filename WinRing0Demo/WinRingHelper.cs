using System;
using System.Runtime.InteropServices;

namespace WinRing0Demo
{
    public class WinRingHelper
    {
        public enum Key
        {
            ABSOLUTE = 0x8000,
            LEFTDOWN = 2,
            LEFTUP = 4,
            MIDDLEDOWN = 0x20,
            MIDDLEUP = 0x40,
            MOVE = 1,
            RIGHTDOWN = 8,
            RIGHTUP = 0x10,
            VIRTUALDESK = 0x4000,
            VK_A = 0x41,
            VK_ADD = 0x6b,
            VK_B = 0x42,
            VK_BACK = 8,
            VK_C = 0x43,
            VK_CANCEL = 3,
            VK_CAPITAL = 20,
            VK_CLEAR = 12,
            VK_CONTROL = 0x11,
            VK_D = 0x44,
            VK_DECIMAL = 110,
            VK_DELETE = 0x2e,
            VK_DIVIDE = 0x6f,
            VK_DOWN = 40,
            VK_E = 0x45,
            VK_END = 0x23,
            VK_ESCAPE = 0x1b,
            VK_EXECUTE = 0x2b,
            VK_F = 70,
            VK_F1 = 0x70,
            VK_F10 = 0x79,
            VK_F11 = 0x7a,
            VK_F12 = 0x7b,
            VK_F2 = 0x71,
            VK_F3 = 0x72,
            VK_F4 = 0x73,
            VK_F5 = 0x74,
            VK_F6 = 0x75,
            VK_F7 = 0x76,
            VK_F8 = 0x77,
            VK_F9 = 120,
            VK_G = 0x47,
            VK_H = 0x48,
            VK_HELP = 0x2f,
            VK_HOME = 0x24,
            VK_I = 0x49,
            VK_INSERT = 0x2d,
            VK_J = 0x4a,
            VK_K = 0x4b,
            VK_L = 0x4c,
            VK_LBUTTON = 1,
            VK_LEFT = 0x25,
            VK_M = 0x4d,
            VK_MBUTTON = 4,
            VK_MENU = 0x12,
            VK_N = 0x4e,
            VK_NEXT = 0x22,
            VK_NULTIPLY = 0x6a,
            VK_NUM0 = 0x30,
            VK_NUM1 = 0x31,
            VK_NUM2 = 50,
            VK_NUM3 = 0x33,
            VK_NUM4 = 0x34,
            VK_NUM5 = 0x35,
            VK_NUM6 = 0x36,
            VK_NUM7 = 0x37,
            VK_NUM8 = 0x38,
            VK_NUM9 = 0x39,
            VK_NUMLOCK = 0x90,
            VK_NUMPAD0 = 0x60,
            VK_NUMPAD1 = 0x61,
            VK_NUMPAD2 = 0x62,
            VK_NUMPAD3 = 0x63,
            VK_NUMPAD4 = 100,
            VK_NUMPAD5 = 0x65,
            VK_NUMPAD6 = 0x66,
            VK_NUMPAD7 = 0x67,
            VK_NUMPAD8 = 0x68,
            VK_NUMPAD9 = 0x69,
            VK_O = 0x4f,
            VK_P = 80,
            VK_PAUSE = 0x13,
            VK_PRINT = 0x2a,
            VK_PRIOR = 0x21,
            VK_Q = 0x51,
            VK_R = 0x52,
            VK_RBUTTON = 2,
            VK_RETURN = 13,
            VK_RIGHT = 0x27,
            VK_S = 0x53,
            VK_SCROLL = 0x91,
            VK_SELECT = 0x29,
            VK_SEPARATOR = 0x6c,
            VK_SHIFT = 0x10,
            VK_SNAPSHOT = 0x2c,
            VK_SPACE = 0x20,
            VK_SUBTRACT = 0x6d,
            VK_T = 0x54,
            VK_TAB = 9,
            VK_U = 0x55,
            VK_UP = 0x26,
            VK_V = 0x56,
            VK_W = 0x57,
            VK_X = 0x58,
            VK_Y = 0x59,
            VK_Z = 90,
            WHEEL = 0x800,
            XDOWN = 0x80,
            XUP = 0x100
        }

        private static Ols ols = null;

        [DllImport("user32.dll")]
        public static extern int MapVirtualKey(uint Ucode, uint uMapType);

        public static Boolean init()
        {
            ols = new Ols();
            return ols.GetStatus() == (uint)Ols.Status.NO_ERROR;
        }

        private static void KBCWait4IBE()
        {
            byte dwVal = 0;
            do
            {
                ols.ReadIoPortByteEx(0x64, ref dwVal);
            }
            while ((dwVal & 0x2) > 0);
        }

        public static void KeyDown(char str)
        {
            int btScancode = MapVirtualKey((uint)GetKey(str), 0);
            KBCWait4IBE();
            ols.WriteIoPortByte(0x64, 0xd2);
            KBCWait4IBE();
            ols.WriteIoPortByte(0x60, (byte)btScancode);
        }

        public static void KeyDown(Key k)
        {
            int btScancode = MapVirtualKey((uint)k, 0);
            KBCWait4IBE();
            ols.WriteIoPortByte(0x64, 0xd2);
            KBCWait4IBE();
            ols.WriteIoPortByte(0x60, (byte)btScancode);
        }

        public static void KeyUp(char str)
        {
            int btScancode = MapVirtualKey((uint)GetKey(str), 0);
            KBCWait4IBE();
            ols.WriteIoPortByte(0x64, 0xd2);
            KBCWait4IBE();
            ols.WriteIoPortByte(0x60, (byte)(btScancode | 0x80));
        }

        public static void KeyUp(Key k)
        {
            int btScancode = MapVirtualKey((uint)k, 0);
            KBCWait4IBE();
            ols.WriteIoPortByte(0x64, 0xd2);
            KBCWait4IBE();
            ols.WriteIoPortByte(0x60, (byte)(btScancode | 0x80));
        }

        private static Key GetKey(char str)
        {
            var result = Key.VK_A;

            switch (str.ToString().ToLower())
            {
                #region 字母

                case "a":
                    result = Key.VK_A; break;
                case "b":
                    result = Key.VK_B; break;
                case "c":
                    result = Key.VK_C; break;
                case "d":
                    result = Key.VK_D; break;
                case "e":
                    result = Key.VK_E; break;
                case "f":
                    result = Key.VK_F; break;
                case "g":
                    result = Key.VK_G; break;
                case "h":
                    result = Key.VK_H; break;
                case "i":
                    result = Key.VK_I; break;
                case "j":
                    result = Key.VK_J; break;
                case "k":
                    result = Key.VK_K; break;
                case "l":
                    result = Key.VK_L; break;
                case "m":
                    result = Key.VK_M; break;
                case "n":
                    result = Key.VK_N; break;
                case "o":
                    result = Key.VK_O; break;
                case "p":
                    result = Key.VK_P; break;
                case "q":
                    result = Key.VK_Q; break;
                case "r":
                    result = Key.VK_R; break;
                case "s":
                    result = Key.VK_S; break;
                case "t":
                    result = Key.VK_T; break;
                case "u":
                    result = Key.VK_U; break;
                case "v":
                    result = Key.VK_V; break;
                case "w":
                    result = Key.VK_W; break;
                case "x":
                    result = Key.VK_X; break;
                case "y":
                    result = Key.VK_Y; break;
                case "z":
                    result = Key.VK_Z; break;

                #endregion 字母

                #region 数字

                case "0":
                    result = Key.VK_NUM0; break;
                case "1":
                    result = Key.VK_NUM1; break;
                case "2":
                    result = Key.VK_NUM2; break;
                case "3":
                    result = Key.VK_NUM3; break;
                case "4":
                    result = Key.VK_NUM4; break;
                case "5":
                    result = Key.VK_NUM5; break;
                case "6":
                    result = Key.VK_NUM6; break;
                case "7":
                    result = Key.VK_NUM7; break;
                case "8":
                    result = Key.VK_NUM8; break;
                case "9":
                    result = Key.VK_NUM9; break;

                #endregion 数字

                #region 符号

                case "*":
                    result = Key.VK_NULTIPLY; break;
                case "+":
                    result = Key.VK_ADD; break;
                case "-":
                    result = Key.VK_SUBTRACT; break;
                case ".":
                    result = Key.VK_DECIMAL; break;
                case "/":
                    result = Key.VK_DIVIDE; break;

                #endregion 符号

                #region 空格

                case " ":
                    result = Key.VK_SPACE; break;

                #endregion 空格

                default:
                    break;
            }
            return result;
        }
    }
}