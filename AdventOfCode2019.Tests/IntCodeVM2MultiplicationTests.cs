using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace AdventOfCode2019.Tests
{
    [TestFixture]
    class IntCodeVM2MultiplicationTests
    {
        [Test]
        [TestCase(2, 3, 6)]
        [TestCase(70, 75, 5250)]
        [TestCase(7896455, 9853463, 77807427173665)]
        public void TestAdditionAddresses(long a, long b, long answer)
        {
            List<long> program = new List<long>()
            {
                2, 5, 6, 0,
                99,
                a, b
            };

            IntCodeVM2 vm = new IntCodeVM2(program);
            vm.ExecuteProgram();
            Assert.That(vm.GetMemory(0), Is.EqualTo(answer));
        }

        [Test]
        [TestCase(2, 3, 6)]
        [TestCase(70, 75, 5250)]
        [TestCase(7896455, 9853463, 77807427173665)]
        public void TestAdditionDirect(long a, long b, long answer)
        {
            List<long> program = new List<long>()
            {
                1102, a, b, 0,
                99,
                2, 3
            };

            IntCodeVM2 vm = new IntCodeVM2(program);
            vm.ExecuteProgram();
            Assert.That(vm.GetMemory(0), Is.EqualTo(answer));
        }

        [Test]
        [TestCase(2, 3, 6)]
        [TestCase(70, 75, 5250)]
        [TestCase(7896455, 9853463, 77807427173665)]
        public void TestAdditionBaseOffset(long a, long b, long answer)
        {
            List<long> program = new List<long>()
            {
                109, 7,
                2202, 0, 1, 0,
                99,
                a, b
            };

            IntCodeVM2 vm = new IntCodeVM2(program);
            vm.ExecuteProgram();
            Assert.That(vm.GetMemory(0), Is.EqualTo(answer));
        }
    }
}
