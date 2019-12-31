using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2019
{
    public class ASCIIHelper
    {
        public bool KeepSpaces { get; set; } = true;
        private StringBuilder stringBuilder = new StringBuilder();

        /// <summary>
        /// Add a line to be converted
        /// </summary>
        /// <param name="input">Line to be converted</param>
        /// <param name="end">Char to end in, newline by default</param>
        public void AddLine(string input, char end = '\n')
        {
            if (!KeepSpaces)
            {
                input = input.Replace(" ", "");
            }

            stringBuilder.Append(input);
            stringBuilder.Append(end);
        }

        /// <summary>
        /// Convert input lines into list of longs
        /// </summary>
        /// <returns></returns>
        public List<long> Convert()
        {
            return stringBuilder.ToString().ToCharArray().Select(c => (long) c).ToList();
        }
    }
}
