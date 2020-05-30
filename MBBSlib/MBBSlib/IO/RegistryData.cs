using Microsoft.Win32;
using System;
using System.IO;

namespace MBBSlib.IO
{
    /// <summary>
    /// Class for predefined registry manipulation
    /// </summary>
    public static class RegistryData
    {
        /// <summary>
        /// Prevides default path for storage, if doeant exist then creates one
        /// </summary>
        /// <returns>Default path, can be altered</returns>
        public static string GetPath()
        {
            string data = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\SyncTool";
            try
            {

                RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Mabat\SyncTool", true);
                data = (string)key.GetValue("path");
                key.Close();
            }
            catch
            {
                ConfigureDirectory("");
            }
            if(!Directory.Exists(data))
            {
                Directory.CreateDirectory(data);
            }
            return data;
        }
        /// <summary>
        /// Creates default path and saves it in the registry
        /// </summary>
        /// <param name="name">Location in user documents</param>
        public static void ConfigureDirectory(string name)
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software", true);
            key.CreateSubKey("MBBS");
            key = Registry.CurrentUser.OpenSubKey(@"Software\MBBS", true);
            key.CreateSubKey(name);
            key = Registry.CurrentUser.OpenSubKey(@"Software\Mabat\" + name, true);
            key.SetValue("path", Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\MBBS\" + name);
            key.Close();
        }
    }
}
