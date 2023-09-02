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

using ImageAsMat = CV.ImageAsMat;

namespace CV
{
    /// <summary>
    /// Class <c>Image</c> stores data for a two-dimensional image. It is used for storing data.
    /// </summary>
    /// <typeparam name="T">Numeric types, use IBinaryNumber for .net 7 or later</typeparam>    
    public class Image<T> where T : IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
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
    /// It is used for applying the OpenCV functions.
    /// </summary>
    public class ImageAsMat : IDisposable
    {
        private GCHandle handle;
        public Mat Mat { get; set; }
        private bool disposed = false;

        public ImageAsMat(Image8u image)
        {
            handle = GCHandle.Alloc(image.Data, GCHandleType.Pinned);
            Mat = new Mat(image.Height, image.Width, DepthType.Cv8U, image.Channels, handle.AddrOfPinnedObject(), image.Width * image.Channels);
        }

        public ImageAsMat(Image8s image)
        {
            handle = GCHandle.Alloc(image.Data, GCHandleType.Pinned);
            Mat = new Mat(image.Height, image.Width, DepthType.Cv8S, image.Channels, handle.AddrOfPinnedObject(), image.Width * image.Channels);
        }

        public ImageAsMat(Image16u image)
        {
            handle = GCHandle.Alloc(image.Data, GCHandleType.Pinned);
            Mat = new Mat(image.Height, image.Width, DepthType.Cv16U, image.Channels, handle.AddrOfPinnedObject(), image.Width * image.Channels * sizeof(ushort));
        }

        public ImageAsMat(Image16s image)
        {
            handle = GCHandle.Alloc(image.Data, GCHandleType.Pinned);
            Mat = new Mat(image.Height, image.Width, DepthType.Cv16S, image.Channels, handle.AddrOfPinnedObject(), image.Width * image.Channels * sizeof(short));
        }

        public ImageAsMat(Image32s image)
        {
            handle = GCHandle.Alloc(image.Data, GCHandleType.Pinned);
            Mat = new Mat(image.Height, image.Width, DepthType.Cv32S, image.Channels, handle.AddrOfPinnedObject(), image.Width * image.Channels * sizeof(int));
        }

        public ImageAsMat(Image32f image)
        {
            handle = GCHandle.Alloc(image.Data, GCHandleType.Pinned);
            Mat = new Mat(image.Height, image.Width, DepthType.Cv32F, image.Channels, handle.AddrOfPinnedObject(), image.Width * image.Channels * sizeof(float));
        }

        public ImageAsMat(Image64f image)
        {
            handle = GCHandle.Alloc(image.Data, GCHandleType.Pinned);
            Mat = new Mat(image.Height, image.Width, DepthType.Cv64F, image.Channels, handle.AddrOfPinnedObject(), image.Width * image.Channels * sizeof(double));
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
}

namespace emgucv_example
{
    class Program
    {
        static void Main(string[] args)
        {
            Image8u bimg = new(400, 200, 3);

            using (ImageAsMat m1 = new(bimg))
            {
                CvInvoke.BitwiseNot(m1.Mat, m1.Mat);
            }
               
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
