using System;
using System.Collections.Generic;
using System.Text;

using static Wonderlust.Core.ShellAPI;

namespace Wonderlust.Core
{
    public class PropertyWindow
    {
        public static void Open(string path)
        {
            SHELLEXECUTEINFO sei = new SHELLEXECUTEINFO();
            sei.cbSize = System.Runtime.InteropServices.Marshal.SizeOf(sei);
            sei.fMask = (uint)(ShellExecuteMaskFlags.SEE_MASK_NOASYNC | ShellExecuteMaskFlags.SEE_MASK_INVOKEIDLIST);
            sei.lpVerb = "properties";
            sei.lpFile = path;
            sei.nShow = (int)ShowCommands.SW_SHOWNORMAL;

            bool result = ShellExecuteEx(ref sei);
        }
    }
}
