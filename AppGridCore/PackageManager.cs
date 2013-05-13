using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICSharpCode.SharpZipLib;
using ICSharpCode.SharpZipLib.Zip;
using System.IO;
namespace AppGridCore
{
    public class PackageManager
    {
        public bool ProcessNewApp(Stream file, string appname,AppGrid.Configuration configuration)
        {
            ZipEntry entry;
            string domainPath = Path.Combine(ProcessManager.ProcManager.Instance.AppDomainPath, appname);
            string entryName;
            if (!Directory.Exists(domainPath))
            {
                Directory.CreateDirectory(domainPath);
            }
            ZipInputStream input = new ZipInputStream(file);
            while((entry=input.GetNextEntry())!=null)
            {
                entryName = entry.Name;
                if (entry.IsFile)
                {
                    if (!string.IsNullOrEmpty(entry.Name))
                    {
                        string strNewFile = Path.Combine(domainPath, entryName);
                        if (File.Exists(strNewFile))
                        {
                            continue;
                        }
                        using (FileStream strWrt = File.Create(strNewFile))
                        {
                            int size = 2048;
                            byte[] data = new byte[2048];
                            while (true)
                            {
                                size = input.Read(data, 0, data.Length);
                                if (size > 0)
                                {
                                    strWrt.Write(data, 0, size);
                                }
                                else
                                {
                                    break;
                                }
                            }
                            strWrt.Close();
                        }
                    }                    
                }
                else if (entry.IsDirectory)
                {
                    string strNewDir = Path.Combine(domainPath, entryName);
                    if (!Directory.Exists(strNewDir))
                    {
                        Directory.CreateDirectory(strNewDir);
                    }
                }
            }
            input.Close();
            AppGrid.Utils.ConfigurationAdmin.ConfigurationSave(configuration, Path.Combine(domainPath, "Configuration.xml"));
            return true;
        }
    }
}
