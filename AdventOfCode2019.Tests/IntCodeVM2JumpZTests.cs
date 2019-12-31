using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace AdventOfCode2019.Tests
{
    [TestFixture]
    public class IntCodeVM2JumpZTests
    {
        [Test]
        [TestCase(1, false)]
        [TestCase(0, true)]
        public void TestOutputAddresses(long answer, bool jump)
        {
            List<long> program = new List<long>()
            {
                6, 9, 10,
                104, 0,
                99,
                104, 1,
                99,
                answer,
                6
            };

            IntCodeVM2 vm = new IntCodeVM2(program);
            vm.ExecuteProgram();
            if (jump)
            {
                Assert.That(vm.GetLastOutput(), Is.EqualTo(1));
            }
            else
            {
                Assert.That(vm.GetLastOutput(), Is.EqualTo(0));
            }

        }

        [Test]
        [TestCase(1, false)]
        [TestCase(0, true)]
        public void TestOutputDirect(long answer, bool jump)
        {
            List<long> program = new List<long>()
            {
                1106, answer, 6,
                104, 0,
                99,
                104, 1,
                99,
            };

            IntCodeVM2 vm = new IntCodeVM2(program);
            vm.ExecuteProgram();
            if (jump)
            {
                Assert.That(vm.GetLastOutput(), Is.EqualTo(1));
            }
            else
            {
                Assert.That(vm.GetLastOutput(), Is.EqualTo(0));
            }

        }

        [Test]
        [TestCase(1, false)]
        [TestCase(0, true)]
        public void TestOutputBaseOffset(long answer, bool jump)
        {
            List<long> program = new List<long>()
            {
                109, 3,
                2206, answer + 8, 10,
                104, 0,
                99,
                104, 1,
                99,
                0, 6,
                8
            };

            IntCodeVM2 vm = new IntCodeVM2(program);
            vm.ExecuteProgram();
            if (jump)
            {
                Assert.That(vm.GetLastOutput(), Is.EqualTo(1));
            }
            else
            {
                Assert.That(vm.GetLastOutput(), Is.EqualTo(0));
            }
        }
    }
}
