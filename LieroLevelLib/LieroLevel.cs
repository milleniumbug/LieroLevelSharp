using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace LieroLevelLib
{
	// Liero level format pseudo-specification
	// Levels are 2D arrays of size 504 x 350, each elements contains one of 256 values
	// Each value corresponds to a color in a given palette
	// Specific values have special properties:
	// - air (worm can pass through it, and so can weapon projectiles)
	// - stone (neither worm nor weapons can pass through it)
	// - dirt (worm can't walk through it, but can dig through it, also some weapons are capable of destroying it)
	// - ??? (worm can't walk nor dig through it, but weapon projectiles pass through)
	// - ??? (worm can walk through it, and dig through it, but weapon projectiles are stopped)
	// a common extension is a "powerlevel", where behind the level data you place a signature "POWERLEVEL" (without quotes)
	// followed 
	public class LieroLevel : ICloneable
	{
		private const int width = 504;
		private const int height = 350;

		private byte[] levelData;

		public Palette Palette { get; }

		public bool IsPowerlevel { get; }

		public Material this[int i, int j]
		{
			get => Palette.MaterialFromIndex(levelData[i + j * width]);
			set => levelData[i + j * width] = value.Index;
		}

		public Material this[Point point]
		{
			get => this[point.X, point.Y];
			set => this[point.X, point.Y] = value;
		}

		private LieroLevel(byte[] levelData, Palette palette, bool isPowerlevel)
		{
			if(levelData.Length != width * height)
				throw new ArgumentException("invalid input size", nameof(levelData));
			this.levelData = levelData;
			this.Palette = palette;
			this.IsPowerlevel = isPowerlevel;
		}

		public static LieroLevel CreateEmpty(Material material, Palette palette = null)
		{
			var levelData = new byte[width * height];
			for(int i = 0; i < levelData.Length; ++i)
			{
				levelData[i] = material.Index;
			}
			bool isPowerlevel = palette != null;
			palette = palette ?? Palette.Default;
			return new LieroLevel(levelData, palette, isPowerlevel);
		}

		public static LieroLevel LoadFromStream(Stream stream, Palette palette = null)
		{
			var levelData = new byte[width * height];
			stream.ReadInto(levelData, 0, levelData.Length);
			if(palette != null)
				return new LieroLevel(levelData, palette, true);

			var powerlevelSig = new byte[10];
			if(stream.Read(powerlevelSig, 0, powerlevelSig.Length) == 10 &&
			   Encoding.ASCII.GetString(powerlevelSig) == "POWERLEVEL")
			{
				palette = Palette.LoadFromStream(stream);
				return new LieroLevel(levelData, palette, true);
			}
			else
			{
				palette = Palette.Default;
				return new LieroLevel(levelData, palette, false);
			}
		}

		public void SaveToStream(Stream stream)
		{
			stream.Write(levelData, 0, levelData.Length);
		}

		public async Task SaveToStreamAsync(Stream stream)
		{
			await stream.WriteAsync(levelData, 0, levelData.Length);
		}

		object ICloneable.Clone()
		{
			return Clone();
		}

		public LieroLevel Clone()
		{
			return new LieroLevel((byte[])levelData.Clone(), Palette, IsPowerlevel);
		}

		public int Width => width;
		public int Height => height;
	}
}
