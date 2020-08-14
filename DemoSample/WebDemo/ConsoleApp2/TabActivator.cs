using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

using Accessibility;

namespace ConsoleApp2
{
    public class TabActivator

    {
        #region Nested type: OBJID

        private enum ObjId : uint

        {
            ObjIdWindow = 0x00000000,
        }

        #endregion Nested type: OBJID

        #region Declarations

        private const int CHILDID_SELF = 0;

        private readonly IntPtr _hWnd;

        private IAccessible _accessible;

        [DllImport("oleacc.dll")]
        private static extern int AccessibleObjectFromWindow(IntPtr hWnd, uint id, ref Guid iid, [In, Out, MarshalAs(UnmanagedType.IUnknown)] ref object ppvObject);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr FindWindowEx(IntPtr hWndParent, IntPtr hWndChildAfter, string lpszClass, string lpszWindow);

        [DllImport("oleacc.dll")]
        private static extern int AccessibleChildren(IAccessible paccContainer, int iChildStart, int cChildren, [In, Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] object[] rgvarChildren, out int pcObtained);

        #endregion Declarations

        #region Constructors

        internal TabActivator(IntPtr ieHandle)

        {
            _hWnd = ieHandle;

            AccessibleObjectFromWindow(GetDirectUihWnd(ieHandle), ObjId.ObjIdWindow, ref _accessible);

            CheckForAccessible();
        }

        private TabActivator(IAccessible acc)

        {
            _accessible = acc ?? throw new Exception("Could not get accessible");
        }

        #endregion Constructors

        private TabActivator[] Children

        {
            get
            {
                var res = GetAccessibleChildren(_accessible, out _);

                if (res == null)

                    return new TabActivator[0];

                var list = new List<TabActivator>(res.Length);

                list.AddRange(res.OfType<IAccessible>().Select(acc => new TabActivator(acc)));

                return list.ToArray();
            }
        }

        private int ChildCount => _accessible.accChildCount;

        /// <summary>

        /// Gets LocationUrl of the tab

        /// </summary>

        private string LocationUrl

        {
            get

            {
                var url = _accessible.accDescription[CHILDID_SELF];

                if (url.Contains(Environment.NewLine))

                    url = url.Split('\n')[1];

                return url;
            }
        }

        private void CheckForAccessible()

        {
            if (_accessible == null)

                throw new Exception("Could not get accessible.  Accessible can't be null");
        }

        internal void ActivateByTabsUrl(string tabsUrl)

        {
            var tabIndexToActivate = GetTabIndexToActivate(tabsUrl);

            AccessibleObjectFromWindow(GetDirectUihWnd(_hWnd), ObjId.ObjIdWindow, ref _accessible);

            CheckForAccessible();

            var index = 0;

            var ieDirectUihWnd = new TabActivator(_hWnd);

            foreach (var accessor in ieDirectUihWnd.Children)

            {
                foreach (var child in accessor.Children)

                {
                    foreach (var tab in child.Children)

                    {
                        if (tabIndexToActivate >= child.ChildCount - 1)

                            return;

                        if (index == tabIndexToActivate)

                        {
                            tab.ActivateTab();

                            return;
                        }

                        index++;
                    }
                }
            }
        }

        private void ActivateTab()

        {
            _accessible.accDoDefaultAction(CHILDID_SELF);
        }

        private int GetTabIndexToActivate(string tabsUrl)

        {
            AccessibleObjectFromWindow(GetDirectUihWnd(_hWnd), ObjId.ObjIdWindow, ref _accessible);

            CheckForAccessible();

            var index = 0;

            var ieDirectUihWnd = new TabActivator(_hWnd);

            foreach (var accessor in ieDirectUihWnd.Children)

            {
                foreach (var child in accessor.Children)

                {
                    foreach (var tab in child.Children)

                    {
                        var tabUrl = tab.LocationUrl;

                        if (!string.IsNullOrEmpty(tabUrl))

                        {
                            if (tab.LocationUrl.Contains(tabsUrl))

                                return index;
                        }

                        index++;
                    }
                }
            }

            return -1;
        }

        private IntPtr GetDirectUihWnd(IntPtr ieFrame)

        {
            // For IE 8:

            var directUI = FindWindowEx(ieFrame, IntPtr.Zero, "CommandBarClass", null);

            directUI = FindWindowEx(directUI, IntPtr.Zero, "ReBarWindow32", null);

            directUI = FindWindowEx(directUI, IntPtr.Zero, "TabBandClass", null);

            directUI = FindWindowEx(directUI, IntPtr.Zero, "DirectUIHWND", null);

            if (directUI != IntPtr.Zero) return directUI;

            // For IE 9:

            //directUI = FindWindowEx(ieFrame, IntPtr.Zero, "WorkerW", "Navigation Bar");

            directUI = FindWindowEx(ieFrame, IntPtr.Zero, "WorkerW", null);

            directUI = FindWindowEx(directUI, IntPtr.Zero, "ReBarWindow32", null);

            directUI = FindWindowEx(directUI, IntPtr.Zero, "TabBandClass", null);

            directUI = FindWindowEx(directUI, IntPtr.Zero, "DirectUIHWND", null);

            return directUI;
        }

        private static int AccessibleObjectFromWindow(IntPtr hWnd, ObjId idObject, ref IAccessible acc)

        {
            var guid = new Guid("{618736e0-3c3d-11cf-810c-00aa00389b71}"); 

            object obj = null;

            var num = AccessibleObjectFromWindow(hWnd, (uint)idObject, ref guid, ref obj);

            acc = (IAccessible)obj;

            return num;
        }

        private static object[] GetAccessibleChildren(IAccessible ao, out int childs)

        {
            childs = 0;

            object[] ret = null;

            var count = ao.accChildCount;

            if (count <= 0) return ret;

            ret = new object[count];

            AccessibleChildren(ao, 0, count, ret, out childs);

            return ret;
        }
    }
}