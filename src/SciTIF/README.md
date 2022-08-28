
**SciTIF is a .NET Standard library for working with scientific imaging data TIFF files.** Many libraries that read TIF files struggle to interpret 16-bit, 32-bit, and 8-bit indexed color TIFF files used for scientific imaging. If you've ever tried to open a scientific TIF file and been presented with an all-black or all-white image, you've experienced this problem too! SciTIF supports 5D TIF files with separate axes for X, Y, C (channel), Z (slice), and T (frame).

## Quickstart

```cs
// Open a 16-bit TIFF with pixel values that exceed 255
var tif = new TifFile("input.tif");

// Get a channel of interest from the 5D TIFF file
Image slice = tif.GetImage(frame: 0, slice: 0, channel: 0);

// Analyze or manipulate images by iterating the flat pixel value array
double[] allValues = slice.Values;

// ...or manipulate individual pixels as desired
double pxValue = slice.GetPixel(13, 42);
slice.SetPixel(13, 42, 123);

// Scale the pixel values down to 0-255 and save as a PNG file
slice.AutoScale(max: 255);
slice.Save("output.png");
```

![](https://raw.githubusercontent.com/swharden/SciTIF/main/dev/diagrams/autoscale.png)

## Lookup Table (LUT)

This example takes a grayscale image and applies a lookup table (LUT) to represent pixel values as colors.

```cs
TifFile tif = new("graycale.tif");
Image slice = tif.GetImage(frame: 0, slice: 0, channel: 0);
slice.AutoScale();
slice.LUT = new LUTs.Viridis(); // apply a custom color lookup table
slice.Save_TEST("viridis.png");
```

![](https://raw.githubusercontent.com/swharden/SciTIF/main/dev/diagrams/autoscale-viridis.png)

## RGB Merge

If you have 3 grayscale images representing red, green, and blue, you can easily merge them into a color image.

```cs
// read grayscale images from a multi-channel TIF
TifFile tif = new("multichannel.tif");
Image red = tif.GetImage(channel: 0);
Image green = tif.GetImage(channel: 1);
Image blue = tif.GetImage(channel: 2);

ImageRGB rgb = new(red, green, blue); // merge 3 grayscale images
rgb.Save("merge.png");
```

![](https://raw.githubusercontent.com/swharden/SciTIF/main/dev/diagrams/merge-rgb.png)

## Multi-Channel Merge

This example shows how to merge two grayscale channels into a color image using custom colors (Magenta and Green).

```cs
TifFile tif = new("multichannel.tif");

// scale each channel (0-255) and set the color lookup table (LUT)
Image ch1 = tif.GetImage(channel: 0);
ch1.AutoScale();
ch1.LUT = new LUTs.Magenta();

Image ch2 = tif.GetImage(channel: 1);
ch2.AutoScale();
ch2.LUT = new LUTs.Green();

// create a new stack containing just the channels to merge
Image[] images = { ch1, ch2 };
ImageStack stack = new(images);

// project the stack by merging colors
ImageRGB merged = stack.Merge();
merged.Save_TEST("merge.png");
```

![](https://raw.githubusercontent.com/swharden/SciTIF/main/dev/diagrams/merge-channels.png)

## 3D Image Projection

This examples shows how to create a maximum-intensity projection along the Z axis of a 5D TIF image. This can be used to create all-in-focus maximum projection of a collection of single optical sections.

```cs
TifFile tif = new("stack.tif");
ImageStack stack = tif.GetImageStack();
Image projection = stack.ProjectMax();
projection.AutoScale(); 
projection.Save("projection.png");
```

![](https://raw.githubusercontent.com/swharden/SciTIF/main/dev/diagrams/projection-max.png)

## 3D Image Projection with LUT

A stack of grayscale images can be projected such that each slice is given a different color according to a lookup table (LUT). This achieves a depth-coded effect where color indicates the Z position of the structures visible in the final image.

```cs
TifFile tif = new("stack.tif");
ImageStack stack = tif.GetImageStack();
stack.AutoScale(); 
ILUT lut = new LUTs.Jet();
ImageRGB projection = stack.Project(lut);
projection.Save("rainbow.png");
```

![](https://raw.githubusercontent.com/swharden/SciTIF/main/dev/diagrams/projection-color.png)

## Notes

* Warning: SciTIF is still version 0 so its API may change as it continues to evolve.

* Note: RGB images are automatically split into 5D TIF files containing 4 channels: Red, Green, Blue, and Alpha.