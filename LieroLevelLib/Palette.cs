using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LieroLevelLib
{
	public class Palette : IEnumerable<KeyValuePair<Material, Color18>>
	{
		private IReadOnlyList<Color18> allColors;

		public Palette(IReadOnlyDictionary<byte, Color18> colorMapping)
		{
			var list = new List<Color18>();
			for(int i = 0; i < 256; ++i)
			{
				list.Add(colorMapping[(byte)i]);
			}
			allColors = list.AsReadOnly();
		}

		private Palette(IReadOnlyList<Color18> colors)
		{
			allColors = colors;
		}

		public static readonly Palette Default = CreateDefaultPalette();

		public Color18 ColorFromIndex(byte index)
		{
			return allColors[index];
		}

		public Material MaterialFromIndex(byte index)
		{
			return materials[index];
		}

		public Material MaterialFromColor(Color color, int squareTolerance = 27, Predicate<Material> predicate = null)
		{
			int Square(int x)
			{
				return x * x;
			}

			int CalculateSquareDistance(Color l, Color r)
			{
				return Square(l.R - r.R) + Square(l.G - r.G) + Square(l.B - r.B);
			}

			predicate = predicate ?? (_ => true);
			var colorIndexPair = allColors
				.Select((c, i) => new { Color = c, Index = i })
				.Where(s => predicate(materials[s.Index]))
				.MinBy(s => CalculateSquareDistance(s.Color, color));
			var closestColor = colorIndexPair.Color;
			var closestColorIndex = colorIndexPair.Index;
			if(CalculateSquareDistance(closestColor, color) < squareTolerance)
				return MaterialFromIndex((byte)closestColorIndex);

			throw new InvalidOperationException("no matching color");
		}

		public static Palette LoadFromStream(Stream stream)
		{
			var rawColor = new byte[3];
			var list = new List<Color18>();
			for(int i = 0; i < 256; ++i)
			{
				stream.ReadInto(rawColor, 0, rawColor.Length);
				list.Add(new Color18(
					(byte)(rawColor[0] & 63),
					(byte)(rawColor[1] & 63),
					(byte)(rawColor[2] & 63)));
			}
			return new Palette(list.AsReadOnly());
		}

		private byte[] GetBytes()
		{
			return allColors
				.SelectMany(c => new byte[] { c.R, c.G, c.B })
				.ToArray();
		}

		public void SaveToStream(Stream stream)
		{
			var bytes = GetBytes();
			stream.Write(bytes, 0, bytes.Length);
		}

		public async Task SaveToStreamAsync(Stream stream)
		{
			var bytes = GetBytes();
			await stream.WriteAsync(bytes, 0, bytes.Length);
		}

		private static readonly IReadOnlyList<Material> materials = new int[]{
			2, // 0
			1 | 4 | 8, // 1
			1 | 4 | 8, // 2
			2, // 3
			2, // 4
			2, // 5
			2, // 6
			2, // 7
			2, // 8
			2, // 9
			2, // 10
			2, // 11
			4 | 8, // 12
			4 | 8, // 13
			4 | 8, // 14
			4 | 8, // 15
			4 | 8, // 16
			4 | 8, // 17
			4 | 8, // 18
			0, // 19
			0, // 20
			0, // 21
			0, // 22
			0, // 23
			0, // 24
			0, // 25
			0, // 26
			0, // 27
			0, // 28
			0, // 29
			16384 | 2, // 30
			16384 | 2, // 31
			16384 | 2, // 32
			16384 | 2, // 33
			16384 | 2, // 34
			2, // 35
			2, // 36
			2, // 37
			2, // 38
			16384 | 2, // 39
			16384 | 2, // 40
			16384 | 2, // 41
			16384 | 2, // 42
			16384 | 2, // 43
			2, // 44
			2, // 45
			2, // 46
			2, // 47
			2, // 48
			2, // 49
			2, // 50
			2, // 51
			2, // 52
			2, // 53
			2, // 54
			4 | 8, // 55
			4 | 8, // 56
			4 | 8, // 57
			4 | 8, // 58
			0, // 59
			0, // 60
			0, // 61
			2, // 62
			2, // 63
			2, // 64
			2, // 65
			2, // 66
			2, // 67
			2, // 68
			2, // 69
			2, // 70
			2, // 71
			2, // 72
			2, // 73
			2, // 74
			2, // 75
			2, // 76
			1 | 4 | 8, // 77
			1 | 4 | 8, // 78
			1 | 4 | 8, // 79
			2, // 80
			2, // 81
			4 | 8, // 82
			4 | 8, // 83
			4 | 8, // 84
			0, // 85
			0, // 86
			0, // 87
			4 | 8 | 16384, // 88
			4 | 8 | 16384, // 89
			4 | 8 | 16384, // 90
			0 | 16384, // 91
			0 | 16384, // 92
			0 | 16384, // 93
			4 | 8, // 94
			4 | 8, // 95
			4 | 8, // 96
			4 | 8, // 97
			4 | 8, // 98
			4 | 8, // 99
			4 | 8, // 100
			4 | 8, // 101
			4 | 8, // 102
			4 | 8, // 103
			2, // 104
			2, // 105
			2, // 106
			2, // 107
			2, // 108
			2, // 109
			2, // 110
			2, // 111
			2, // 112
			2, // 113
			2, // 114
			2, // 115
			2, // 116
			2, // 117
			2, // 118
			2, // 119         
			4 | 8 | 16384, // 120
			4 | 8 | 16384, // 121
			4 | 8 | 16384, // 122
			16384, // 123
			16384, // 124
			16384, // 125
			2, // 126
			2, // 127
			2, // 128
			2 | 8192 | 16384, // 129
			2 | 8192 | 16384, // 130
			2 | 8192 | 16384, // 131
			2, // 132
			2 | 8192 | 16384, // 133
			2 | 8192 | 16384, // 134
			2 | 8192 | 16384, // 135
			2 | 8192 | 16384, // 136
			2, // 137
			2, // 138
			2, // 139
			2, // 140
			2, // 141
			2, // 142
			2, // 143
			2, // 144
			2, // 145
			2, // 146
			2, // 147
			2, // 148
			2, // 149
			2, // 150
			2, // 151
			2 | 8192, // 152
			2 | 8192, // 153
			2 | 8192, // 154
			2 | 8192, // 155
			2 | 8192, // 156
			2 | 8192, // 157
			2 | 8192, // 158
			2 | 8192, // 159
			1 | 2 | 16, // 160
			1 | 2 | 16, // 161
			1 | 2 | 16, // 162
			1 | 2 | 16, // 163
			1 | 2 | 32, // 164
			1 | 2 | 32, // 165
			1 | 2 | 32, // 166
			1 | 2 | 32, // 167
			2 | 8192, // 168
			2 | 8192, // 169
			2 | 8192, // 170
			2 | 8192, // 171
			2, // 172
			2, // 173
			2, // 174
			2, // 175
			4 | 8, // 176
			4 | 8, // 177
			4 | 8, // 178
			4 | 8, // 179
			4 | 8, // 180
			2, // 181
			2, // 182
			2, // 183
			2, // 184
			2, // 185
			2, // 186
			2, // 187
			2, // 188
			2, // 189
			2, // 190
			2, // 191
			2, // 192
			2, // 193
			2, // 194
			2, // 195
			2, // 196
			2, // 197
			2, // 198
			2, // 199
			2, // 200
			2, // 201
			2, // 202
			2, // 203
			2, // 204
			2, // 205
			2, // 206
			2, // 207
			2, // 208
			2, // 209
			2, // 210
			2, // 211
			2, // 212
			2, // 213
			2, // 214
			2, // 215
			2, // 216
			2, // 217
			2, // 218
			2, // 219
			2, // 220
			2, // 221
			2, // 222
			2, // 223
			2, // 224
			2, // 225
			2, // 226
			2, // 227
			2, // 228
			2, // 229
			2, // 230
			2, // 231
			2, // 232
			2, // 233
			2, // 234
			2, // 235
			2, // 236
			2, // 237
			2, // 238
			2, // 239
			2, // 240
			2, // 241
			2, // 242
			2, // 243
			2, // 244
			2, // 245
			2, // 246
			2, // 247
			2, // 248
			2, // 249
			2, // 250
			2, // 251
			2, // 252
			2, // 253
			2, // 254
			2, // 255
		}.Select((x, i) => new Material(x, (byte)i)).ToList().AsReadOnly();

		private static Palette CreateDefaultPalette()
		{
			var paletteRawBytes = Convert.FromBase64String("AAAAADhsAFBsgJSkAJAAPKw8VFT8qKioVFRU/FRUVNhU/PxUCEB4CESADEiIEFCQFFSYGFigHGCsTExMVFRUXFxcZGRkbGxsdHR0fHx8hISEjIyMlJSUnJyciDg4wFBQ+Gho9JCQ9Li4bGxskJCQtLS02NjYIGAgLIQsPKw8cLxwpNSkbGxskJCQtLS02NjY+Kio9NDQ9Pz8AFA8AHBYAJB0ALCUNEh4WHicfKjEoNjsWHicfKjEoNjsAGTIAFCgSEhIbGxskJCQtLS02NjY/Pz8xMTEkJCQADyYAGS0AIzQALTsAFSoAADYAAC8AACkAADIAACsAADYAAC8AACkAADYAAC8AACkwFBQ+Gho9JCQwFBQ+Gho9JCQAIiUAHyIAHB8AGR0KFyESISgaLC8iNzYvPj4/PT0AAD8BBj4CDT4EFD4FGz4GIj4IKT4JMD4KNz4POj0UPT0cPT0lPT0tPDw1PDw+PDwLIQsPKw8cLxwLIQsPKw8cLxwPDz4fHz0vLz0+Gho9JCQ9Li49JCQPKw8cLxwpNSkcLxwAIiUAHSIAGB8AExwADhkAChYiGhowJCQ+Ly89MjI9NzcKHAoLIQsNJg0PKw8yMj8pKT0XFz4TEz0PDz0TEz0XFz0pKT0AChUAChYACxcADBgABw8ABxAACBEACRI/Pz83NzcvLy8nJycfHx8nJycvLy83NzcLExsMFR8OGCMQGycSHisAAAACCQoFExQHHR4KJigMMDIPOj0AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD8ACT8AEj8AGz8AJD8ALT8ANj8APz8APCoAOhUAOAAAAD8FAToLAzYRBTEWBi0cCCgiCiQnCx8tDRszDxY5ERI");
			using(var stream = new MemoryStream(paletteRawBytes))
			{
				return LoadFromStream(stream);
			}
		}

		public IEnumerator<KeyValuePair<Material, Color18>> GetEnumerator()
		{
			for(int i = 0; i < 256; ++i)
			{
				yield return new KeyValuePair<Material, Color18>(materials[i], allColors[i]);
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}

}
