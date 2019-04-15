using System;
using System.IO;
using Microsoft.Win32;

namespace MBBSlib.IO
{
    public class RegistryData
    {
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
                ConfigureDirectory();
            }
            if (!Directory.Exists(data))
            {
                Directory.CreateDirectory(data);
            }
            if (!Directory.Exists(data + @"\Saves"))
            {
                Directory.CreateDirectory(data + @"\Saves");
            }
            if (!Directory.Exists(data + @"\Saves\OSS"))
            {
                Directory.CreateDirectory(data + @"\Saves\OSS");
            }
            return data;
        }
        public static void ConfigureDirectory()
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software", true);
            key.CreateSubKey("Mabat");
            key = Registry.CurrentUser.OpenSubKey(@"Software\Mabat", true);
            key.CreateSubKey("SyncTool");
            key = Registry.CurrentUser.OpenSubKey(@"Software\Mabat\SyncTool", true);
            key.SetValue("path", Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\SyncTool");
            key.Close();
        }
    }
}
