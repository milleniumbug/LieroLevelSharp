using System;
using System.Drawing;

namespace LieroLevelLib
{
	public class Brush
	{
		private readonly Point origin;

		private readonly Material?[,] content;

		public Brush(Material?[,] content, Point origin)
		{
			this.content = (Material?[,])content.Clone();
			this.origin = origin;
		}

		public void DrawTo(LieroLevel level, Point target)
		{
			target.Offset(-origin.X, -origin.Y);
			var w = content.GetLength(0);
			w = Math.Min(w, level.Width - target.X);
			var h = content.GetLength(1);
			h = Math.Min(h, level.Height - target.Y);
			var initialI = Math.Max(-target.X, 0);
			var initialJ = Math.Max(-target.Y, 0);
			for(int i = initialI; i < w; ++i)
			{
				for(int j = initialJ; j < h; ++j)
				{
					if(content[i, j] != null)
					{
						var t = target;
						t.Offset(i, j);
						level[t] = content[i, j].Value;
					}
				}
			}
		}
	}
}
