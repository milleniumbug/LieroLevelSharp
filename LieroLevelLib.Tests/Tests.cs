using System.Drawing;
using System.IO;
using LieroLevelLib;
using NUnit.Framework;

namespace Tests
{
	public class Tests
	{
		[SetUp]
		public void SetUp()
		{
			level = LieroLevel.CreateEmpty(Palette.Default.MaterialFromIndex(161));
			var content = new Material?[21, 21];
			for(int i = 0; i < content.GetLength(0); ++i)
			{
				for(int j = 0; j < content.GetLength(1); ++j)
				{
					content[i, j] = Palette.Default.MaterialFromIndex(129);
				}
			}
			content[9, 9] = null;
			content[9, 11] = null;
			content[11, 9] = null;
			content[11, 11] = null;
			content[10, 10] = Palette.Default.MaterialFromIndex(161);
			brushCenter = new Brush(content, new Point(10, 10));
			brushLeftTop = new Brush(content, new Point(0, 0));
		}

		[Ignore("nope")]
		[Test]
		public void Sandbox()
		{
			var width = 20;
			var z = width * 250;
			var data = new byte[504 * 350];
			for(int i = 0; i < 504; ++i)
			{
				for(int j = 0; j < 345; ++j)
				{
					data[i + j * 504] = 161;
				}

				data[i + 312 * 504] = (byte)(((z / width) % 200 == 0) ? 0 : 161);
				data[i + 328 * 504] = (byte)(((z / width) % 100 == 0) ? 0 : 161);
				data[i + 336 * 504] = (byte)(((z / width) % 50 == 0) ? 0 : 161);
				data[i + 340 * 504] = (byte)(((z / width) % 25 == 0) ? 0 : 161);
				data[i + 342 * 504] = (byte)(((z / width) % 10 == 0) ? 0 : 161);
				data[i + 343 * 504] = (byte)(((z / width) % 5 == 0) ? 0 : 161);
				for(int j = 345; j < 350; ++j)
				{
					data[i + j * 504] = (byte)(z / width);
				}

				for(int j = 0; j < 50; ++j)
				{
					data[i + j * 504] = (byte)(z / width);
				}
				z++;
			}
			using(var memstream = new MemoryStream(data))
			using(var stream = File.OpenWrite("/media/milleniumbug/7b99189a-98b5-49c0-ab32-5987e3642f9b/Gry/Liero/liero/levels/aaaa.lev"))
			{
				LieroLevel.LoadFromStream(memstream).SaveToStream(stream);
			}
		}

		private LieroLevel level;
		private Brush brushCenter;
		private Brush brushLeftTop;

		[Test]
		public void BasicDraw()
		{
			brushLeftTop.DrawTo(level, new Point(0, 0));
			brushCenter.DrawTo(level, new Point(level.Width / 2, level.Height / 2));
			using(var stream = File.OpenWrite("/media/milleniumbug/7b99189a-98b5-49c0-ab32-5987e3642f9b/Gry/Liero/liero/levels/aaaa.lev"))
			{
				level.SaveToStream(stream);
			}
		}

		[Test]
		public void ClippingDraw()
		{
			brushCenter.DrawTo(level, new Point(0, level.Height / 2));
			brushCenter.DrawTo(level, new Point(level.Width, level.Height / 2));
			brushCenter.DrawTo(level, new Point(level.Width / 2, 0));
			brushCenter.DrawTo(level, new Point(level.Width / 2, level.Height));
			using(var stream = File.OpenWrite("/media/milleniumbug/7b99189a-98b5-49c0-ab32-5987e3642f9b/Gry/Liero/liero/levels/aaaa.lev"))
			{
				level.SaveToStream(stream);
			}
		}
	}
}