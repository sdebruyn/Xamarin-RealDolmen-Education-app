using System.Collections.Generic;
using System.Linq;

namespace EducationApp.Extensions
{
    public static class StringExtensions
    {
        public static bool IsNullOrWhiteSpace(this string input) => string.IsNullOrWhiteSpace(input);
        public static bool AreNullOrWhiteSpace(this IEnumerable<string> input) => input.ToList().All(IsNullOrWhiteSpace);
        public static bool AreNullOrWhiteSpace(params string[] input) => input.AreNullOrWhiteSpace();
        public static bool IsNotNullOrWhiteSpace(this string input) => !string.IsNullOrWhiteSpace(input);

        public static bool AreNotNullOrWhiteSpace(this IEnumerable<string> input)
            => input.ToList().All(IsNotNullOrWhiteSpace);

        public static bool AreNotNullOrWhiteSpace(params string[] input) => input.AreNotNullOrWhiteSpace();
        public static bool IsNullOrEmpty(this string input) => string.IsNullOrEmpty(input);
        public static bool AreNullOrEmpty(this IEnumerable<string> input) => input.ToList().All(IsNullOrEmpty);
        public static bool AreNullOrEmpty(params string[] input) => input.AreNullOrEmpty();
        public static bool IsNotNullOrEmpty(this string input) => !string.IsNullOrEmpty(input);
        public static bool AreNotNullOrEmpty(this IEnumerable<string> input) => input.ToList().All(IsNotNullOrEmpty);
        public static bool AreNotNullOrEmpty(params string[] input) => input.AreNotNullOrEmpty();
    }
}