using FFMpegCore;
using FFMpegCore.Pipes;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace SciTIF;

public class VideoAnnotator
{
    public readonly double FrameRate;
    public readonly double FramePeriod;
    Image[] Images;

    public VideoAnnotator(Image[] images, double fps)
    {
        FrameRate = fps;
        FramePeriod = 1.0 / fps;
        Images = images;
    }

    public void SaveWebm(string saveAs)
    {
        saveAs = Path.GetFullPath(saveAs);
        var frames = CreateFrames(count: Images.Length);
        RawVideoPipeSource videoFramesSource = new(frames) { FrameRate = FrameRate };
        bool success = FFMpegArguments
            .FromPipeInput(videoFramesSource)
            .OutputToFile(saveAs, overwrite: true, options => options.WithVideoCodec("libvpx-vp9"))
            .ProcessSynchronously();
        Console.WriteLine($"Wrote: {saveAs}");
    }

    private IEnumerable<IVideoFrame> CreateFrames(int count)
    {
        using SKFont textFont = new(SKTypeface.FromFamilyName("consolas"), size: 16);
        using SKPaint textPaint = new(textFont) { Color = SKColors.Yellow };

        for (int i = 0; i < count; i++)
        {
            Console.WriteLine($"Rendering frame {i + 1} of {count}");

            Images[i].AutoScale();
            byte[] bytes = Images[i].GetBitmapBytes();

            SKBitmap bmp = SKBitmap.Decode(bytes);
            using SKCanvas canvas = new(bmp);
            canvas.DrawText($"Frame {i}", 10, 20, textPaint);

            using SKBitmapFrame frame = new(bmp);
            yield return frame;
        }
    }
}
