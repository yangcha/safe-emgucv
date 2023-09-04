using System;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

namespace emgucv_example
{
    class Program
    {
        static void Main(string[] args)
        {
            Managed.Image8u bimg = new(400, 200, 3);

            using (Managed.ImageAsMat m1 = new(bimg))
            {
                CvInvoke.BitwiseNot(m1.Mat, m1.Mat);
            }

            bimg[0, 2, 1] = 10;
               
            String win1 = "Test Window"; //The name of the window
            CvInvoke.NamedWindow(win1); //Create the window using the specific name

            unsafe
            {
                fixed (byte* p = bimg.Data)
                {
                    using (Mat m2 = new(200, 400, DepthType.Cv8U, 3, (IntPtr)p, bimg.Width * 3))
                    {
                        CvInvoke.BitwiseNot(m2, m2);
                    }
                }
            }
            Mat img = new Mat(200, 400, DepthType.Cv8U, 3); //Create a 3 channel image of 400x200
            img.SetTo(new Bgr(255, 0, 0).MCvScalar); // set it to Blue color

            //Draw "Hello, world." on the image using the specific font
            CvInvoke.PutText(
               img,
               "Hello, world",
               new System.Drawing.Point(10, 80),
               FontFace.HersheyComplex,
               1.0,
               new Bgr(0, 255, 0).MCvScalar);


            CvInvoke.Imshow(win1, img); //Show the image
            CvInvoke.WaitKey(0);  //Wait for the key pressing event
            CvInvoke.DestroyWindow(win1); //Destroy the window if key is pressed
        }
    }
}
