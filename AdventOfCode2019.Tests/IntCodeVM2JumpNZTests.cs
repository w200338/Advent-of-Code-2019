using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace AdventOfCode2019.Tests
{
    [TestFixture]
    public class IntCodeVM2JumpNZTests
    {
        [Test]
        [TestCase(1, true)]
        [TestCase(0, false)]
        public void TestOutputAddresses(long answer, bool jump)
        {
            List<long> program = new List<long>()
            {
                5, 9, 10,
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
        [TestCase(1, true)]
        [TestCase(0, false)]
        public void TestOutputDirect(long answer, bool jump)
        {
            List<long> program = new List<long>()
            {
                1105, answer, 6,
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
        [TestCase(1, true)]
        [TestCase(0, false)]
        public void TestOutputBaseOffset(long answer, bool jump)
        {
            List<long> program = new List<long>()
            {
                109, 3,
                2205, answer + 8, 10,
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
