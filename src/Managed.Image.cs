using System;

namespace Managed
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

        /// <summary>
        /// Access pixel in image
        /// </summary>
        /// <param name="row">row</param>
        /// <param name="col">column</param>
        /// <param name="cha">channel</param>
        /// <returns>pixel value</returns>
        public T this[int row, int col, int cha]
        {
            get { return Data[(row * Width + col) * Channels + cha]; }
            set { Data[(row * Width + col) * Channels + cha] = value; }
        }

        /// <summary>
        /// Access pixel in grayscale image, assume Channels is 1
        /// </summary>
        /// <param name="row">row</param>
        /// <param name="col">column</param>
        /// <returns>pixel value</returns>
        public T this[int row, int col]
        {
            get { return Data[(row * Width + col) * Channels]; }
            set { Data[(row * Width + col) * Channels] = value; }
        }
        public Image(int width, int height, int channels)
        {
            Data = new T[width * height * channels];
            Width = width;
            Height = height;
            Channels = channels;
        }
    }

    /// <summary>
    ///  Workaround of "global using" for C# older than 10
    /// </summary>
    public class Image8u : Image<byte> 
    {
        public Image8u(int width, int height, int channels) : base(width, height, channels) { }
    }

    public class Image8s : Image<sbyte>
    {
        public Image8s(int width, int height, int channels) : base(width, height, channels) { }
    }

    public class Image16u : Image<ushort>
    {
        public Image16u(int width, int height, int channels) : base(width, height, channels) { }
    }

    public class Image16s : Image<short>
    {
        public Image16s(int width, int height, int channels) : base(width, height, channels) { }
    }

    public class Image32s : Image<int>
    {
        public Image32s(int width, int height, int channels) : base(width, height, channels) { }
    }

    public class Image32f : Image<float>
    {
        public Image32f(int width, int height, int channels) : base(width, height, channels) { }
    }

    public class Image64f : Image<double>
    {
        public Image64f(int width, int height, int channels) : base(width, height, channels) { }
    }
}
