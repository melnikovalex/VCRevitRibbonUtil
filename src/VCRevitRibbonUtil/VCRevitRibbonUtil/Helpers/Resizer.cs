using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace VCRevitRibbonUtil.Helpers
{
	class Resizer
	{
		/// <summary>
		/// Resize the image to the specified width and height.
		/// </summary>
		/// <param name="image">The image to resize.</param>
		/// <param name="width">The width to resize to.</param>
		/// <param name="height">The height to resize to.</param>
		/// <returns>The resized image.</returns>
		public static Bitmap ResizeImage(
		  Image image,
		  int width,
		  int height)
		{
			var destRect = new System.Drawing.Rectangle(
			  0, 0, width, height);

			var destImage = new Bitmap(width, height);

			destImage.SetResolution(image.HorizontalResolution,
			  image.VerticalResolution);

			using (var g = Graphics.FromImage(destImage))
			{
				g.CompositingMode = CompositingMode.SourceCopy;
				g.CompositingQuality = CompositingQuality.HighQuality;
				g.InterpolationMode = InterpolationMode.HighQualityBicubic;
				g.SmoothingMode = SmoothingMode.HighQuality;
				g.PixelOffsetMode = PixelOffsetMode.HighQuality;

				using (var wrapMode = new ImageAttributes())
				{
					wrapMode.SetWrapMode(WrapMode.TileFlipXY);
					g.DrawImage(image, destRect, 0, 0, image.Width,
					  image.Height, GraphicsUnit.Pixel, wrapMode);
				}
			}
			return destImage;
		}
	}
}
