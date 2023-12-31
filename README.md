# safe-emgucv
Make EmguCV memory and resource management safe.

EmguCV's `Mat` holds unmanaged native memory which needs to be disposed of manually. It is not safe to use it to store data or to pass the data around.

This project is to show how to store the data in the managed array, then there not no need to dispose of it manually. To apply the EmguCV's function to it, an adapter class `ImageAsMat` is used to convert the `Image` type to `CVMat` without owning it. 

The `GCHandle` is used to pin the memory for EmguCV, and it is automatically created and safely disposed of with Dispose Pattern.

Example usage:

```csharp
Managed.Image8u bimg = new(400, 200, 3);

using (Managed.ImageAsMat m1 = new(bimg))
{
     CvInvoke.BitwiseNot(m1.Mat, m1.Mat);
}
```

The memory allocation for cv::Mat class can be changed any time. For example, in many cases, a Mat is allocated using the empty constructor:

```
Mat result = new Mat()
```

New memory is allocated when a CvInvoke function is called:

```
CvInvoke.BitwiseNot(src, result)
```

In this case, the memory referenced by Mat can be changed after the function call. Here `Debug.Assert` is used to check if the memory referenced by Mat is changed or not.

