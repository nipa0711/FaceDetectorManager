using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;

namespace FaceDetectorManager
{
    public static class Config
    {
        public static readonly String[] bPMember = { "Jennie", "Lisa", "Rose", "Jisoo" };
        public static readonly String BP = "C:\\BlackPink";
        public static readonly String desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        public static readonly String protoTxt = "asset\\deploy.prototxt";
        public static readonly String caffeModel = "asset\\res10_300x300_ssd_iter_140000.caffemodel";
        public static readonly String faceOnly = "asset\\faceOnly.exe";
    }
}
