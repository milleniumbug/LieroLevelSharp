using System;
using System.Drawing;

namespace LieroLevelLib
{
	public struct Color18 : IEquatable<Color18>
	{
		public byte R { get; }

		public byte G { get; }

		public byte B { get; }

		public Color18(byte r, byte g, byte b)
		{
			byte ValidateParameter(byte value, string name)
			{
				if(value >= 64)
					throw new ArgumentException("must be value from 0-63", name);
				return value;
			}
			this.R = ValidateParameter(r, nameof(r));
			this.G = ValidateParameter(g, nameof(g));
			this.B = ValidateParameter(b, nameof(b));
		}

		public static implicit operator Color(Color18 color)
		{
			return Color.FromArgb((byte)(color.R << 2), (byte)(color.G << 2), (byte)(color.B << 2));
		}

		public static bool operator ==(Color18 l, Color18 r)
		{
			return
				l.R == r.R &&
				l.G == r.G &&
				l.B == r.B;
		}

		public static bool operator !=(Color18 l, Color18 r)
		{
			return !(l == r);
		}

		public bool Equals(Color18 other)
		{
			return this == other;
		}

		public override bool Equals(object obj)
		{
			if(obj == null) return false;
			return Equals((Color18)obj);
		}

		public override int GetHashCode()
		{
			var hashCode = -1058441243;
			hashCode = hashCode * -1521134295 + R.GetHashCode();
			hashCode = hashCode * -1521134295 + G.GetHashCode();
			hashCode = hashCode * -1521134295 + B.GetHashCode();
			return hashCode;
		}
	}
}
