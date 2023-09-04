# safe-emgucv
Make EmguCV memory and resource management safe.

EmguCV's `Mat` holds unmanaged native memory which needs to dispose manually. It is not safe to use it to store data or to pass the data around.

This project is to show how to store the data in the managed array, then there not no need to dispose it manually. To apply the EmguCV's function to it, an adpater class `ImageAsMat` is used to convert the `Image` type tp `CVMat` without owning it. 

The `GCHandle` is used to pin the memory for EmguCV, and it is automatically created and safely disposed.

Example usage:

```csharp
Managed.Image8u bimg = new(400, 200, 3);

using (Managed.ImageAsMat m1 = new(bimg))
{
     CvInvoke.BitwiseNot(m1.Mat, m1.Mat);
}
```
