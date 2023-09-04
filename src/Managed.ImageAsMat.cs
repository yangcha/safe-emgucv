using System;
using System.Runtime.InteropServices;
using Emgu.CV;
using Emgu.CV.CvEnum;

namespace Managed
{
    /// <summary>
    /// Base class <c>ImageAsMat</c> gets a Mat object from Image without owning the data.
    /// It is used for applying the OpenCV functions.
    ///
    /// Sample usage:
    /// Managed.Image8u bimg = new(400, 200, 3);
    /// using (Managed.ImageAsMat m1 = new(bimg))
    /// {
    ///     CvInvoke.BitwiseNot(m1.Mat, m1.Mat);
    /// }
    /// </summary>
    public class ImageAsMat : IDisposable
    {
        private GCHandle handle;
        public Mat Mat { get; set; }
        private bool disposed = false;

        private ImageAsMat(Object data)
        {
            handle = GCHandle.Alloc(data, GCHandleType.Pinned);
        }

        public ImageAsMat(Image8u image) : this(image.Data)
        {
            Mat = new Mat(image.Height, image.Width, DepthType.Cv8U, image.Channels, handle.AddrOfPinnedObject(), image.Width * image.Channels);
        }

        public ImageAsMat(Image8s image) : this(image.Data)
        {
            Mat = new Mat(image.Height, image.Width, DepthType.Cv8S, image.Channels, handle.AddrOfPinnedObject(), image.Width * image.Channels);
        }

        public ImageAsMat(Image16u image) : this(image.Data)
        {
            Mat = new Mat(image.Height, image.Width, DepthType.Cv16U, image.Channels, handle.AddrOfPinnedObject(), image.Width * image.Channels * sizeof(ushort));
        }

        public ImageAsMat(Image16s image) : this(image.Data)
        {
            Mat = new Mat(image.Height, image.Width, DepthType.Cv16S, image.Channels, handle.AddrOfPinnedObject(), image.Width * image.Channels * sizeof(short));
        }

        public ImageAsMat(Image32s image) : this(image.Data)
        {
            Mat = new Mat(image.Height, image.Width, DepthType.Cv32S, image.Channels, handle.AddrOfPinnedObject(), image.Width * image.Channels * sizeof(int));
        }

        public ImageAsMat(Image32f image) : this(image.Data)
        {
            Mat = new Mat(image.Height, image.Width, DepthType.Cv32F, image.Channels, handle.AddrOfPinnedObject(), image.Width * image.Channels * sizeof(float));
        }

        public ImageAsMat(Image64f image) : this(image.Data)
        {
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

