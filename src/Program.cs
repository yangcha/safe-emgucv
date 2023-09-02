using System;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System.Runtime.InteropServices;


using Image8u = CV.Image<byte>;
using Image8s = CV.Image<sbyte>;
using Image16u = CV.Image<ushort>;
using Image16s = CV.Image<short>;
using Image32s = CV.Image<int>;
using Image32f = CV.Image<float>;
using Image64f = CV.Image<double>;

using Image8uAsMat = CV.Image8uAsMat;
using Image8sAsMat = CV.Image8sAsMat;
using Image16uAsMat = CV.Image16uAsMat;
using Image16sAsMat = CV.Image16sAsMat;
using Image32sAsMat = CV.Image32sAsMat;
using Image32fAsMat = CV.Image32fAsMat;
using Image64fAsMat = CV.Image64fAsMat;

namespace CV
{
    /// <summary>
    /// Class <c>Image</c> stores data for a two-dimensional image. It is used for storing data.
    /// </summary>
    /// <typeparam name="T"></typeparam>    
    public class Image<T>
    {
        public T[] Data { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Channels { get; set; }

        public Image(int width, int height, int channels)
        {
            Data = new T[width * height * channels];
            Width = width;
            Height = height;
            Channels = channels;
        }
    }

    /// <summary>
    /// Base class <c>ImageAsMat</c> gets a Mat object from Image without owning the data.
    /// It is used for OpenCV function.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ImageAsMat<T> : IDisposable
    {
        protected GCHandle handle;
        public Mat Mat { get; set; }
        private bool disposed = false;

        protected ImageAsMat(Image<T> image)
        {
            handle = GCHandle.Alloc(image.Data, GCHandleType.Pinned);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // Dispose managed resources.
                    Mat.Dispose();
                }

                // free unmanaged resources (unmanaged objects) and override finalizer
                // set large fields to null
                handle.Free();
                disposed = true;
            }
        }

        // Use C# finalizer syntax for finalization code.
        // This finalizer will run only if the Dispose method does not get called.
        // It gives your base class the opportunity to finalize.
        // Do not provide finalizer in types derived from this class.
        ~ImageAsMat() => Dispose(disposing: false);
        
        // Implement IDisposable.
        // Do not make this method virtual. A derived class should not be able to override this method.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            // This object will be cleaned up by the Dispose method.
            // Therefore, you should call GC.SuppressFinalize to take this object off the finalization queue
            // and prevent finalization code for this object from executing a second time.
            GC.SuppressFinalize(this);
        }
    }

    public class Image8uAsMat : ImageAsMat<byte>
    {
        public Image8uAsMat(Image8u image) : base(image)
        {
            Mat = new Mat(image.Height, image.Width, DepthType.Cv8U, image.Channels, handle.AddrOfPinnedObject(), image.Width * image.Channels);
        }
    }

    public class Image8sAsMat : ImageAsMat<sbyte>
    {
        public Image8sAsMat(Image8s image) : base(image)
        {
            Mat = new Mat(image.Height, image.Width, DepthType.Cv8S, image.Channels, handle.AddrOfPinnedObject(), image.Width * image.Channels);
        }
    }

    public class Image16uAsMat : ImageAsMat<ushort>
    {
        public Image16uAsMat(Image16u image) : base(image)
        {
            Mat = new Mat(image.Height, image.Width, DepthType.Cv16U, image.Channels, handle.AddrOfPinnedObject(), image.Width * image.Channels * sizeof(ushort));
        }
    }

    public class Image16sAsMat : ImageAsMat<short>
    {
        public Image16sAsMat(Image16s image) : base(image)
        {
            Mat = new Mat(image.Height, image.Width, DepthType.Cv16S, image.Channels, handle.AddrOfPinnedObject(), image.Width * image.Channels * sizeof(short));
        }
    }

    public class Image32sAsMat : ImageAsMat<int>
    {
        public Image32sAsMat(Image32s image) : base(image)
        {
            Mat = new Mat(image.Height, image.Width, DepthType.Cv32S, image.Channels, handle.AddrOfPinnedObject(), image.Width * image.Channels * sizeof(int));
        }
    }

    public class Image32fAsMat : ImageAsMat<float>
    {
        public Image32fAsMat(Image32f image) : base(image)
        {
            Mat = new Mat(image.Height, image.Width, DepthType.Cv32F, image.Channels, handle.AddrOfPinnedObject(), image.Width * image.Channels * sizeof(float));
        }
    }

    public class Image64fAsMat : ImageAsMat<double>
    {
        public Image64fAsMat(Image64f image) : base(image)
        {
            Mat = new Mat(image.Height, image.Width, DepthType.Cv64F, image.Channels, handle.AddrOfPinnedObject(), image.Width * image.Channels * sizeof(double));
        }
    }
}

namespace emgucv_example
{
    class Program
    {
        static void Main(string[] args)
        {
           
            var bimg = new Image8u(400, 200, 3);

            using (var m = new Image8uAsMat(bimg))
            {
                CvInvoke.BitwiseNot(m.Mat, m.Mat);
            }
               
            String win1 = "Test Window"; //The name of the window
            CvInvoke.NamedWindow(win1); //Create the window using the specific name

            unsafe
            {
                fixed (byte* p = bimg.Data)
                {
                    using (Mat m2 = new Mat(200, 400, DepthType.Cv8U, 3, (IntPtr)p, bimg.Width * 3))
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
