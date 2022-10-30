using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;
using System.Diagnostics;

using WinShell;

namespace Logging_App.Utility
{
    public class WinShell
    {
        public static void OpenFile(string fullname)
        {
            if (System.Environment.OSVersion.Version.Major > 5)
            {
                Process.Start(fullname);
            }
            else
            {
                IntPtr ppshf, ppidl;
                uint pchEaten, pdwAttributes = 0;
                var desktopFolder = API.GetDesktopFolder(out ppshf);
                IShellFolder parentFolder;
                desktopFolder.ParseDisplayName(IntPtr.Zero, IntPtr.Zero, Path.GetDirectoryName(fullname), out pchEaten, out ppidl, ref pdwAttributes);
                desktopFolder.BindToObject(ppidl, IntPtr.Zero, ref Guids.IID_IShellFolder, out parentFolder);
                parentFolder.ParseDisplayName(IntPtr.Zero, IntPtr.Zero, Path.GetFileName(fullname), out pchEaten, out ppidl, ref pdwAttributes);
                IntPtr[] pidls = new IntPtr[1] { ppidl };
                IntPtr iContextMenuPtr = IntPtr.Zero;
                iContextMenuPtr = parentFolder.GetUIObjectOf(IntPtr.Zero, (uint)pidls.Length,
                    pidls, ref Guids.IID_IContextMenu, out iContextMenuPtr);
                IContextMenu iContextMenu = (IContextMenu)Marshal.GetObjectForIUnknown(iContextMenuPtr);

                //提供一个弹出式菜单的句柄
                IntPtr contextMenu = API.CreatePopupMenu();
                iContextMenu.QueryContextMenu(contextMenu, 0,
                    API.CMD_FIRST, API.CMD_LAST, CMF.NORMAL | CMF.EXPLORE);

                //获取默认的命令项
                int defaultCommand = API.GetMenuDefaultItem(contextMenu, false, 0);

                CMINVOKECOMMANDINFOEX invoke = new CMINVOKECOMMANDINFOEX();
                invoke.cbSize = Marshal.SizeOf(typeof(CMINVOKECOMMANDINFOEX));
                invoke.lpVerb = (IntPtr)(defaultCommand - API.CMD_FIRST);
                invoke.lpDirectory = string.Empty;
                invoke.fMask = 0;
                //invoke.ptInvoke = new POINT(MousePosition.X, MousePosition.Y);
                invoke.nShow = 1;
                iContextMenu.InvokeCommand(ref invoke);
            }
        }
    }
}
