using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;

namespace FaceDetectorManager
{

    class Manager
    {
        static void MakeTrainTxt(String folderPath)
        {
            List<String> imagesPath = GetFolderFiles(folderPath);
            MakeTxt(imagesPath, "train.txt");
        }

        static void MakeValidTxt(String folderPath)
        {
            List<String> imagesPath = GetFolderFiles(folderPath);
            MakeTxt(imagesPath, "vaild.txt");
        }

        static void MakeTxt(List<String> imagesPath, String fileName)
        {
            // 파일 경로를 지정 합니다.
            string savePath = System.String.Format("{0}\\{1}", Config.BP, fileName);

            foreach (var path in imagesPath)
            {
                // 기존 파일에 text 값을 추가 합니다.
                string imgPath = path + "\r\n";
                System.IO.File.AppendAllText(savePath, imgPath, Encoding.Default);
            }
            Console.WriteLine("Done");
        }


        static void MakeImageInfo(List<String> images, String path)
        {
            String line = " 0.5 0.5 1 1";

            for (int i = 0; i < images.Count; i++)
            {
                String filePath = images[i];

                for (int j = 0; j < Config.bPMember.Length; j++)
                {
                    if (filePath.Contains(Config.bPMember[j]))
                    {
                        String fileName = Path.GetFileNameWithoutExtension(filePath);
                        Console.WriteLine(fileName);
                        using (StreamWriter outputFile = new StreamWriter(path + "\\" + fileName + ".txt"))
                        {
                            outputFile.Write(j + line);
                        }
                    }
                }
            }
            Console.WriteLine("Done");
        }

        static List<String> GetFolderFiles(String folderPath)
        {
            List<String> files = new List<String>();
            if (System.IO.Directory.Exists(folderPath))
            {
                System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(folderPath);
                int i = 0;
                foreach (var File in di.GetFiles())
                {
                    if (File.Extension.ToLower().CompareTo(".jpg") == 0)
                    {
                        String FullFileName = File.FullName;
                        files.Add(FullFileName);
                        i++;
                    }
                }
                Console.WriteLine("Done");
            }
            else
            {
                Console.WriteLine("wrong directory");
            }
            return files;
        }

        static void temp()
        {
            List<String> imagesPath = GetFolderFiles("D:\\BlackPink\\validate");

            // 파일 경로를 지정 합니다.
            string savePath = System.String.Format("{0}", "C:\\Users\\nipa0\\Desktop\\valid.txt");

            string imgPath = "";
           

            foreach (var path in imagesPath)
            {
                for(int i=0; i<4; i++)
                {
                    if(path.Contains(Config.bPMember[i])==true)
                    {
                        imgPath = System.String.Format(path + " {0}\r\n", i);
                    }
                }
                // 기존 파일에 text 값을 추가 합니다.
               
                System.IO.File.AppendAllText(savePath, imgPath, Encoding.Default);
            }
            Console.WriteLine("Done");
        }

        static void Menu()
        {
            Console.WriteLine("select menu");
            Console.WriteLine("1. run face detector");
            Console.WriteLine("2. run make Train text file");
            Console.WriteLine("3. run make Valid text file");
            Console.WriteLine("4. Deduplicate Original");
            Console.WriteLine("5. MakeImageInfo");
            Console.WriteLine("6. Face Deduplicate");
            Console.WriteLine("7. run make Caffe Train text file");
            Console.WriteLine("0. exit");
        }

        static void Main(string[] args)
        {

            bool flag = true;
            while (flag)
            {
                Menu();
                Console.Write("input : ");
                int select = int.Parse(Console.ReadLine());

                switch (select)
                {
                    case 1:
                        FaceDetector detector = new FaceDetector();
                        String protoTxt = (Config.desktop + "\\deploy.prototxt");
                        String caffeModel = (Config.desktop + "\\res10_300x300_ssd_iter_140000.caffemodel");
                        Process process = new Process();
                        process.StartInfo.FileName = "faceOnly.exe";
                        process.StartInfo.CreateNoWindow = true;
                        process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

                        int memberCount = Config.bPMember.Length;
                        for (int i = 0; i < memberCount; i++)
                        {
                            String folderPath = System.String.Format("{0}\\{1}\\original", Config.BP, Config.bPMember[i]);
                            String trainPath = System.String.Format("{0}\\{1}\\train", Config.BP, Config.bPMember[i]);
                            List<String> imagesPath = GetFolderFiles(folderPath);
                            Console.WriteLine(Config.bPMember[i] + " Start!");

                            int imageCount = imagesPath.Count;
                            for (int j = 0; j < imageCount; j++)
                            {
                                String filePath = imagesPath[j];
                                String fileName = Path.GetFileName(filePath);
                                string argument = System.String.Format(" {0} {1} {2} {3} {4}", protoTxt, caffeModel, filePath, trainPath, fileName);
                                process.StartInfo.Arguments = argument; // Put your arguments here
                                Console.WriteLine("Processing : " + filePath);
                                process.Start();
                            }

                            //foreach (var filePath in imagesPath)
                            //{
                            //    String fileName = Path.GetFileName(filePath);
                            //    string argument = System.String.Format(" {0} {1} {2} {3} {4}", protoTxt, caffeModel, filePath, trainPath, fileName);
                            //    process.StartInfo.Arguments = argument; // Put your arguments here
                            //    process.StartInfo.CreateNoWindow = true;
                            //    process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                            //    Console.WriteLine("Processing : " + filePath);
                            //    process.Start();
                            //}
                            Console.WriteLine(Config.bPMember[i]+" Done");
                        }
                        process.Close();
                        break;
                    case 2:
                        MakeTrainTxt(Config.BP+"\\train");
                        break;
                    case 3:
                        MakeValidTxt(Config.BP + "\\validate");
                        break;
                    case 4:
                        FileDedup fileDedup = new FileDedup();
                        for (int i = 0; i < Config.bPMember.Length; i++)
                        {
                            String folderPath = System.String.Format("{0}\\{1}\\original", Config.BP, Config.bPMember[i]);
                            fileDedup.RemoveDuplicate(folderPath);
                        }
                        break;
                    case 5:
                        for (int i = 0; i < Config.bPMember.Length; i++)
                        {
                            String folderPath = System.String.Format("{0}\\{1}\\train", Config.BP, Config.bPMember[i]);
                            List<String> imagesPath = GetFolderFiles(folderPath);
                            MakeImageInfo(imagesPath, folderPath);
                        }
                        break;
                    case 6:
                        FileDedup fd = new FileDedup();
                        for (int i = 0; i < Config.bPMember.Length; i++)
                        {
                            String folderPath = System.String.Format("{0}\\{1}\\train", Config.BP, Config.bPMember[i]);
                            fd.RemoveDuplicate(folderPath);
                        }
                        break;
                    case 7:
                        temp();
                        break;
                    case 0:
                        flag = false;
                        Console.WriteLine("program end");
                        break;
                    default:
                        Console.WriteLine("Wrong input");
                        break;
                }
            }
        }
    }
}
