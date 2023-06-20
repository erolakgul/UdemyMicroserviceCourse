using System;
using System.Globalization;

namespace FreeCourse.Shared.Extentions
{
    public static class StringHelpers
    {
        /// <summary>
        /// toupper first letter of string
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string CapitalizeFirstLetter(this String input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            return input.Substring(0, 1).ToUpper(CultureInfo.CurrentCulture) +
                input.Substring(1, input.Length - 1);
        }
    }
}
