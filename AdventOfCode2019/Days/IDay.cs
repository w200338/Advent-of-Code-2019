namespace AdventOfCode2019.Days
{
    public interface IDay
    {
        /// <summary>
        /// Read input from given file
        /// </summary>
        void ReadInput();

        /// <summary>
        /// Complete part 1 and return the output as a string
        /// </summary>
        /// <returns></returns>
        string Part1();

        /// <summary>
        /// Complete part 2 and return the output as a string
        /// </summary>
        /// <returns></returns>
        string Part2();
    }
}
