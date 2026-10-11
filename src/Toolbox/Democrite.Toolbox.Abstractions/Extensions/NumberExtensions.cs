// Copyright (c) Nexai.
// The Democrite licenses this file to you under the MIT license.
// Produce by nexai & community (cf. docs/Teams.md)

namespace System
{
    /// <summary>
    /// 
    /// </summary>
    public static class NumberExtensions
    {
        /// <summary>
        /// Determines whether the specified value is between minimum included and maximum excluded
        /// </summary>
        public static bool IsBetween<TSource>(this TSource source, in TSource minIncluded, in TSource maxIncluded)
            where TSource : IComparable
        {
            //TO-DO from Opus 5.5 IsBetween lower-bound comparison is inverted : `minIncluded.CompareTo(source) >= 0` is only true when min >= source, so IsBetween returns false for almost every value inside the range. Use `source.CompareTo(minIncluded) >= 0 && source.CompareTo(maxIncluded) <= 0`.
            return minIncluded.CompareTo(source) >= 0 && source.CompareTo(maxIncluded) <= 0;
        }
    }
}
