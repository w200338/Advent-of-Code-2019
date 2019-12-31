using System.Collections.Generic;
using NUnit.Framework;

namespace AdventOfCode2019.Tests
{
    [TestFixture]
    public class IntCodeVM2AdditionTests
    {
        [Test]
        [TestCase(2, 3, 5)]
        [TestCase(70, 75, 145)]
        [TestCase(78964556654, 9853463453, 88818020107)]
        public void TestAdditionAddresses(long a, long b, long answer)
        {
            List<long> program = new List<long>()
            {
                1, 5, 6, 0, 
                99,
                a, b
            };

            IntCodeVM2 vm = new IntCodeVM2(program);
            vm.ExecuteProgram();
            Assert.That(vm.GetMemory(0), Is.EqualTo(answer));
        }

        [Test]
        [TestCase(2, 3, 5)]
        [TestCase(70, 75, 145)]
        [TestCase(78964556654, 9853463453, 88818020107)]
        public void TestAdditionDirect(long a, long b, long answer)
        {
            List<long> program = new List<long>()
            {
                1101, a, b, 0,
                99,
                2, 3
            };

            IntCodeVM2 vm = new IntCodeVM2(program);
            vm.ExecuteProgram();
            Assert.That(vm.GetMemory(0), Is.EqualTo(answer));
        }

        [Test]
        [TestCase(2, 3, 5)]
        [TestCase(70, 75, 145)]
        [TestCase(78964556654, 9853463453, 88818020107)]
        public void TestAdditionBaseOffset(long a, long b, long answer)
        {
            List<long> program = new List<long>()
            {
                109, 7,
                2201, 0, 1, 0,
                99,
                a, b
            };

            IntCodeVM2 vm = new IntCodeVM2(program);
            vm.ExecuteProgram();
            Assert.That(vm.GetMemory(0), Is.EqualTo(answer));
        }
    }
}