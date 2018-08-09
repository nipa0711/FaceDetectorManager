using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;
using OpenCvSharp.Dnn;

namespace FaceDetectorManager
{
    public class FaceDetector
    {
        const uint inWidth = 300;
        const uint inHeight = 300;
        const double inScaleFactor = 1.0;
        String protoTxt = (Config.desktop + "\\deploy.prototxt");
        String caffeModel = (Config.desktop + "\\res10_300x300_ssd_iter_140000.caffemodel");

        public void LoadCaffeModel(String source)
        {
            Mat img = Cv2.ImRead(source);
            using (var net = CvDnn.ReadNetFromCaffe(protoTxt, caffeModel))
            {

                Console.WriteLine("Layer names: {0}", string.Join(", ", net.GetLayerNames()));
               // Assert.Equal(1, net.GetLayerId(net.GetLayerNames()[0]));

                // Convert Mat to batch of images
                using (var inputBlob = CvDnn.BlobFromImage(img, 1.0, new Size(300, 300), new Scalar(104, 117, 123), false, false))
                {
                    net.SetInput(inputBlob, "data");
                    using (var detection = net.Forward("detection_out"))
                    {
                        // find the best class
                        Console.WriteLine(detection);
                        Console.WriteLine(detection.Size(2));
                        GetMaxClass(detection, out int classId, out double classProb);
                        Console.WriteLine("Best class: #{0} ", classId);
                        Console.WriteLine("Probability: {0:P2}", classProb);
                        // Pause();
                        //Assert.Equal(812, classId);
                    }
                }
            } 
        }
        private static void GetMaxClass(Mat probBlob, out int classId, out double classProb)
        {
            // reshape the blob to 1x1000 matrix
            using (var probMat = probBlob.Reshape(1, 1))
            {
                Cv2.MinMaxLoc(probMat, out _, out classProb, out _, out var classNumber);
                classId = classNumber.X;
            }
        }
    }
}
