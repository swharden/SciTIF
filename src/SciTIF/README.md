**SciTIF is a .NET Standard library for working with scientific imaging data.** Many libraries that read TIF files struggle to interpret 16-bit, 32-bit, and 8-bit indexed color TIFF files used for scientific imaging. If you've ever tried to open a scientific TIF file and been presented with an all-black or all-white image, you've experienced this problem too! SciTIF supports 5D TIF files with separate axes for X, Y, C (channel), Z (slice), and T (frame).

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

// helper methods simplify common operations (e.g., scaling to 0-255)
slice.AutoScale();

// Data can be exported in various non-TIF file formats
slice.SavePng("output.png");
```

## Notes

* Warning: SciTIF is still version 0 so its API may change as it continues to evolve.

* Note: RGB images are automatically split into 5D TIF files containing 4 channels: Red, Green, Blue, and Alpha.