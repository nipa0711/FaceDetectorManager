using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FaceDetectorManager
{   
    class FileDedup
    {        
        public string GetChecksum(string sPathFile)
        {
            using (FileStream stream = File.OpenRead(sPathFile))
            {
                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] byteChecksum = md5.ComputeHash(stream);
                return BitConverter.ToString(byteChecksum).Replace("-", String.Empty);
            }
        }

        public void RemoveDuplicate(String folderPath)
        {
            Console.WriteLine("Current path : " + folderPath);
            Hashtable ht = new Hashtable();
            if (System.IO.Directory.Exists(folderPath))
            {
                System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(folderPath);
                foreach (var File in di.GetFiles())
                {
                    if (File.Extension.ToLower().CompareTo(".jpg") == 0)
                    {
                        String FullFileName = File.FullName;
                        String hash = GetChecksum(FullFileName);
                        bool isContain = ht.ContainsKey(hash);
                        if (isContain == false)
                        {
                            ht.Add(hash, FullFileName);
                        }
                        else
                        {
                            try
                            {
                                Console.WriteLine(FullFileName + " DELETED!");
                                System.IO.File.Delete(FullFileName);
                            }
                            catch (System.IO.IOException e)
                            {
                                Console.WriteLine(e.Message);
                                return;
                            }
                        }
                    }
                }
                Console.WriteLine("Done");
            }
            else
            {
                Console.WriteLine("wrong directory");
            }
        }
    }
}
