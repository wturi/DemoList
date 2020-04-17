using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;

namespace WinRing0.Helpers
{
    public class WinRingHelper
    {
        public enum Key
        {
            KC_A = 0x41,
            KC_B = 0x42,
            KC_C = 0x43,
            KC_D = 0x44,
            KC_E = 0x45,
            KC_F = 0x46,
            KC_G = 0x47,
            KC_H = 0x48,
            KC_I = 0x49,
            KC_J = 0x4A,
            KC_K = 0x4B,
            KC_L = 0x4C,
            KC_M = 0x4D,
            KC_N = 0x4E,
            KC_O = 0x4F,
            KC_P = 0x50,
            KC_Q = 0x51,
            KC_R = 0x52,
            KC_S = 0x53,
            KC_T = 0x54,
            KC_U = 0x55,
            KC_V = 0x56,
            KC_W = 0x57,
            KC_X = 0x58,
            KC_Y = 0x59,
            KC_Z = 0x5A,
            KC_0 = 0x30,
            KC_1 = 0x31,
            KC_2 = 0x32,
            KC_3 = 0x33,
            KC_4 = 0x34,
            KC_5 = 0x35,
            KC_6 = 0x36,
            KC_7 = 0x37,
            KC_8 = 0x38,
            KC_9 = 0x39,
            KC_SMALL_0 = 0x60,
            KC_SMALL_1 = 0x61,
            KC_SMALL_2 = 0x62,
            KC_SMALL_3 = 0x63,
            KC_SMALL_4 = 0x64,
            KC_SMALL_5 = 0x65,
            KC_SMALL_6 = 0x66,
            KC_SMALL_7 = 0x67,
            KC_SMALL_8 = 0x68,
            KC_SMALL_9 = 0x69,
            KC_SMALL_Nultiply = 0x6A,
            KC_SMALL_Add = 0x6B,
            KC_SMALL_Enter = 0x6C,
            KC_SMALL_Subtract = 0x6D,
            KC_SMALL_Decimal = 0x6E,
            KC_SMALL_Divide = 0x6F,
            KC_FUNCTION_F1 = 0x70,
            KC_FUNCTION_F2 = 0x71,
            KC_FUNCTION_F3 = 0x72,
            KC_FUNCTION_F4 = 0x73,
            KC_FUNCTION_F5 = 0x74,
            KC_FUNCTION_F6 = 0x75,
            KC_FUNCTION_F7 = 0x76,
            KC_FUNCTION_F8 = 0x77,
            KC_FUNCTION_F9 = 0x78,
            KC_FUNCTION_F10 = 0x79,
            KC_FUNCTION_F11 = 0x7A,
            KC_FUNCTION_F12 = 0x7B,
            KC_CONTROL_BackSpace = 0x8,
            KC_CONTROL_Tab = 0x9,
            KC_CONTROL_Clear = 0xC,
            KC_CONTROL_Enter = 0xD,
            KC_CONTROL_Shift = 0x10,
            KC_CONTROL_Control = 0x11,
            KC_CONTROL_Alt = 0x12,
            KC_CONTROL_CapeLock = 0x14,
            KC_CONTROL_Esc = 0x1B,
            KC_CONTROL_Spacebar = 0x20,
            KC_CONTROL_PageUp = 0x21,
            KC_CONTROL_PageDown = 0x22,
            KC_CONTROL_End = 0x23,
            KC_CONTROL_Home = 0x24,
            KC_CONTROL_LeftArrow = 0x25,
            KC_CONTROL_UpArrow = 0x26,
            KC_CONTROL_RightArrow = 0x27,
            KC_CONTROL_DwArrow = 0x28,
            KC_CONTROL_Insert = 0x2D,
            KC_CONTROL_Delete = 0x2E,
            KC_CONTROL_NumLock = 0x90,
            KC_CONTROL_Semicolon = 0xBA,
            KC_CONTROL_Equal = 0xBB,
            KC_CONTROL_Comma = 0xBC,
            KC_CONTROL_Minus = 0xBD,
            KC_CONTROL_Decimal = 0xBE,
            KC_CONTROL_Divide = 0xBF,
            KC_CONTROL_Apostrophe = 0xC0,
            KC_CONTROL_LeftSquareBrackets = 0xDB,
            KC_CONTROL_RightSquareBrackets = 0xDD,
            KC_CONTROL_RightSlash = 0xDC,
            KC_CONTROL_SingleQuotes = 0xDE,
            KC_MULTIMEDIA_VolumeAdd = 0xAF,
            KC_MULTIMEDIA_VolumeSubtract = 0xAE,
            KC_MULTIMEDIA_Stop = 0xB3,
            KC_MULTIMEDIA_Mute = 0xAD,
            KC_MULTIMEDIA_Browser = 0xAC,
            KC_MULTIMEDIA_Mail = 0xB4,
            KC_MULTIMEDIA_Search = 0xAA,
            KC_MULTIMEDIA_Collection = 0xAB,
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
            var btScancode = MapVirtualKey((uint)GetKey(str), 0);
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
            int btScancode = MapVirtualKey(str, 0);
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
            var result = Key.KC_A;

            switch (str.ToString().ToLower())
            {
                #region 字母

                case "a":
                    result = Key.KC_A; break;
                case "b":
                    result = Key.KC_B; break;
                case "c":
                    result = Key.KC_C; break;
                case "d":
                    result = Key.KC_D; break;
                case "e":
                    result = Key.KC_E; break;
                case "f":
                    result = Key.KC_F; break;
                case "g":
                    result = Key.KC_G; break;
                case "h":
                    result = Key.KC_H; break;
                case "i":
                    result = Key.KC_I; break;
                case "j":
                    result = Key.KC_J; break;
                case "k":
                    result = Key.KC_K; break;
                case "l":
                    result = Key.KC_L; break;
                case "m":
                    result = Key.KC_M; break;
                case "n":
                    result = Key.KC_N; break;
                case "o":
                    result = Key.KC_O; break;
                case "p":
                    result = Key.KC_P; break;
                case "q":
                    result = Key.KC_Q; break;
                case "r":
                    result = Key.KC_R; break;
                case "s":
                    result = Key.KC_S; break;
                case "t":
                    result = Key.KC_T; break;
                case "u":
                    result = Key.KC_U; break;
                case "v":
                    result = Key.KC_V; break;
                case "w":
                    result = Key.KC_W; break;
                case "x":
                    result = Key.KC_X; break;
                case "y":
                    result = Key.KC_Y; break;
                case "z":
                    result = Key.KC_Z; break;

                #endregion 字母

                #region 数字

                case "0":
                    result = Key.KC_0; break;
                case "1":
                    result = Key.KC_1; break;
                case "2":
                    result = Key.KC_2; break;
                case "3":
                    result = Key.KC_3; break;
                case "4":
                    result = Key.KC_4; break;
                case "5":
                    result = Key.KC_5; break;
                case "6":
                    result = Key.KC_6; break;
                case "7":
                    result = Key.KC_7; break;
                case "8":
                    result = Key.KC_8; break;
                case "9":
                    result = Key.KC_9; break;

                #endregion 数字

                #region 符号

                case ";":
                    result = Key.KC_CONTROL_Semicolon; break;
                case "=":
                    result = Key.KC_CONTROL_Equal; break;
                case ",":
                    result = Key.KC_CONTROL_Comma; break;
                case "-":
                    result = Key.KC_CONTROL_Minus; break;
                case ".":
                    result = Key.KC_CONTROL_Decimal; break;
                case "/":
                    result = Key.KC_CONTROL_Divide; break;
                case "`":
                    result = Key.KC_CONTROL_Apostrophe; break;
                case "[":
                    result = Key.KC_CONTROL_LeftSquareBrackets; break;
                case @"\":
                    result = Key.KC_CONTROL_RightSlash; break;
                case "]":
                    result = Key.KC_CONTROL_RightSquareBrackets; break;
                case "'":
                    result = Key.KC_CONTROL_SingleQuotes; break;

                #endregion 符号

                #region 空格

                case " ":
                    result = Key.KC_CONTROL_Spacebar; break;

                #endregion 空格

                default:
                    break;
            }
            return result;
        }

        private static bool JudgeShift(char chr)
        {
            if (chr >= 'A' && chr <= 'Z') return true;
            return SymbolChar.Contains(chr);
        }

        private static List<char> SymbolChar = new List<char> { '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '_', '+', '{', '}', '|', ':', '"', '<', '>', '?', '~', };

        private static char ShiftConvert(char chr)
        {
            char resultChr;
            switch (chr)
            {
                case '!': resultChr = '1'; break;
                case '@': resultChr = '2'; break;
                case '#': resultChr = '3'; break;
                case '$': resultChr = '4'; break;
                case '%': resultChr = '5'; break;
                case '^': resultChr = '6'; break;
                case '&': resultChr = '7'; break;
                case '*': resultChr = '8'; break;
                case '(': resultChr = '9'; break;
                case ')': resultChr = '0'; break;
                case '_': resultChr = '-'; break;
                case '+': resultChr = '='; break;
                case '{': resultChr = '['; break;
                case '}': resultChr = ']'; break;
                case '|': resultChr = '\\'; break;
                case ':': resultChr = ';'; break;
                case '"': resultChr = '\''; break;
                case '<': resultChr = ','; break;
                case '>': resultChr = '.'; break;
                case '?': resultChr = '/'; break;
                case '~': resultChr = '`'; break;
                default:
                    resultChr = chr;
                    break;
            }
            return resultChr;
        }

        public static void Send(char chr)
        {
            var isShift = JudgeShift(chr);

            if (isShift)
            {
                KeyDown(Key.KC_CONTROL_Shift);
                chr = ShiftConvert(chr);
            }
            KeyDown(chr);
            Thread.CurrentThread.Join(200);
            KeyUp(chr);
            if (isShift)
                KeyUp(Key.KC_CONTROL_Shift);
            Thread.CurrentThread.Join(200);
        }
    }
}