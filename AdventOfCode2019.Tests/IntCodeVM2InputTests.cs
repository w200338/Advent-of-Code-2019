using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace AdventOfCode2019.Tests
{
    [TestFixture]
    class IntCodeVM2InputTests
    {
        [Test]
        [TestCase(6)]
        [TestCase(5250)]
        [TestCase(77807427173665)]
        public void TestInputAddresses(long answer)
        {
            List<long> program = new List<long>()
            {
                3, 0,
                99,
                4, 5, 6
            };

            IntCodeVM2 vm = new IntCodeVM2(program);
            vm.AddInput(answer);
            vm.ExecuteProgram();
            Assert.That(vm.GetMemory(0), Is.EqualTo(answer));
        }

        [Test]
        [TestCase(6)]
        [TestCase(5250)]
        [TestCase(77807427173665)]
        public void TestInputDirect(long answer)
        {
            List<long> program = new List<long>()
            {
                103, 5,
                99,
                3, 4, 5
            };

            IntCodeVM2 vm = new IntCodeVM2(program);
            vm.AddInput(answer);
            vm.ExecuteProgram();
            Assert.That(vm.GetMemory(1), Is.EqualTo(answer));
        }

        [Test]
        [TestCase(6)]
        [TestCase(5250)]
        [TestCase(77807427173665)]
        public void TestInputBaseOffset(long answer)
        {
            List<long> program = new List<long>()
            {
                109, 3,
                203, 4,
                99,
                5, 6, 7
            };

            IntCodeVM2 vm = new IntCodeVM2(program);
            vm.AddInput(answer);
            vm.ExecuteProgram();
            Assert.That(vm.GetMemory(7), Is.EqualTo(answer));
        }
    }
}
