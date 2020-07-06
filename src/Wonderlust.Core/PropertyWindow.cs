﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
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

        private static MemoryStream CreateShellIDList(StringCollection filenames)
        {
            // first convert all files into pidls list
            int pos = 0;
            byte[][] pidls = new byte[filenames.Count][];
            foreach (var filename in filenames)
            {
                // Get pidl based on name
                IntPtr pidl = ILCreateFromPath(filename);
                int pidlSize = ILGetSize(pidl);
                // Copy over to our managed array
                pidls[pos] = new byte[pidlSize];
                Marshal.Copy(pidl, pidls[pos++], 0, pidlSize);
                ILFree(pidl);
            }

            // Determine where in CIDL we will start pumping PIDLs
            int pidlOffset = 4 * (filenames.Count + 2);
            // Start the CIDL stream
            var memStream = new MemoryStream();
            var sw = new BinaryWriter(memStream);
            // Initialize CIDL witha count of files
            sw.Write(filenames.Count);
            // Calcualte and write relative offsets of every pidl starting with root
            sw.Write(pidlOffset);
            pidlOffset += 4; // root is 4 bytes
            foreach (var pidl in pidls)
            {
                sw.Write(pidlOffset);
                pidlOffset += pidl.Length;
            }

            // Write the root pidl (0) followed by all pidls
            sw.Write(0);
            foreach (var pidl in pidls) sw.Write(pidl);
            // stream now contains the CIDL
            return memStream;
        }

        public static int Open(IEnumerable<string> filenames)
        {
            StringCollection Files = new StringCollection();
            foreach (string s in filenames) Files.Add(s);
            var data = new DataObject();
            data.SetFileDropList(Files);
            data.SetData("Preferred DropEffect", new MemoryStream(new byte[] { 5, 0, 0, 0 }), true);
            data.SetData("Shell IDList Array", CreateShellIDList(Files), true);
            return SHMultiFileProperties(data, 0);
        }
    }
}
