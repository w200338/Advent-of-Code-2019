using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace AdventOfCode2019.Tests
{
    [TestFixture]
    public class IntCodeVM2OutputTests
    {
        [Test]
        [TestCase(6)]
        [TestCase(5250)]
        [TestCase(77807427173665)]
        public void TestOutputAddresses(long answer)
        {
            List<long> program = new List<long>()
            {
                4, 4,
                99,
                3, answer, 5
            };

            IntCodeVM2 vm = new IntCodeVM2(program);
            vm.ExecuteProgram();
            Assert.That(vm.GetLastOutput(), Is.EqualTo(answer));
        }

        [Test]
        [TestCase(6)]
        [TestCase(5250)]
        [TestCase(77807427173665)]
        public void TestOutputDirect(long answer)
        {
            List<long> program = new List<long>()
            {
                104, answer,
                99,
                3, 4, 5
            };

            IntCodeVM2 vm = new IntCodeVM2(program);
            vm.ExecuteProgram();
            Assert.That(vm.GetLastOutput(), Is.EqualTo(answer));
        }

        [Test]
        [TestCase(6)]
        [TestCase(5250)]
        [TestCase(77807427173665)]
        public void TestOutputBaseOffset(long answer)
        {
            List<long> program = new List<long>()
            {
                109, 3,
                204, 4,
                99,
                3, 4, answer
            };

            IntCodeVM2 vm = new IntCodeVM2(program);
            vm.ExecuteProgram();
            Assert.That(vm.GetLastOutput(), Is.EqualTo(answer));
        }
    }
}
