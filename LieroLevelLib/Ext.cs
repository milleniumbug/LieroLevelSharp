using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LieroLevelLib
{
	internal static class Ext
	{
		public static TSource MinBy<TSource, TCompared>(
			this IEnumerable<TSource> elements,
			Func<TSource, TCompared> comparedCriterionSelector,
			IComparer<TCompared> comparer)
		{
			TSource currentMinimum = default(TSource);
			bool isFirst = true;
			foreach(var element in elements)
			{
				if(isFirst)
				{
					currentMinimum = element;
				}
				else
				{
					var comparisonResult = comparer.Compare(
						comparedCriterionSelector(element),
						comparedCriterionSelector(currentMinimum));
					currentMinimum = comparisonResult < 0
						? element
						: currentMinimum;
				}

				isFirst = false;
			}

			if(!isFirst)
			{
				return currentMinimum;
			}
			else
			{
				throw new ArgumentException("can't be an empty sequence", nameof(elements));
			}
		}

		public static TSource MinBy<TSource, TCompared>(
			this IEnumerable<TSource> elements,
			Func<TSource, TCompared> comparedCriterionSelector)
		{
			return MinBy(elements, comparedCriterionSelector, Comparer<TCompared>.Default);
		}

		public static IEnumerable<int> Indices<TElement>(this IReadOnlyList<TElement> list)
		{
			return Enumerable.Range(0, list.Count);
		}

		public static void ReadInto(this Stream stream, byte[] buffer, int offset, int count)
		{
			int numBytesToRead = count;
			int numBytesRead = 0;
			do
			{
				int n = stream.Read(buffer, numBytesRead + offset, numBytesToRead);
				if(n == 0)
					throw new EndOfStreamException();
				numBytesRead += n;
				numBytesToRead -= n;
			} while(numBytesToRead > 0);
		}
	}
}
