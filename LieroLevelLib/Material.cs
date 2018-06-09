namespace LieroLevelLib
{
	public struct Material
	{
		private int value;

		public byte Index { get; }

		internal Material(int value, byte index)
		{
			this.value = value;
			this.Index = index;
		}

		// worm passes through it
		public bool WormTransparent => (value & 1) != 0;

		// weapon fire passes through it
		public bool WeaponTransparent => (value & 2) != 0;

		// worms can dig through it
		public bool Diggable => (value & 4) != 0;

		// weapons can destroy it
		public bool Destructible => (value & 8) != 0;

		public bool WeaponTrailsVisible => (value & 16) != 0;

		public bool Shadow => (value & 32) != 0;

		// palette color cycling applies on this color
		public bool IsColorCycled => (value & 8192) != 0;

		// its color is dependent on a worm color
		// (worm color is user-configurable)
		public bool MatchesWormColor => (value & 16384) != 0;
	}
}
