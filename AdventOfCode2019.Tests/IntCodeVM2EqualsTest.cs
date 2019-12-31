using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace AdventOfCode2019.Tests
{
    [TestFixture]
    public class IntCodeVM2EqualsTest
    {
        [Test]
        [TestCase(1, 1, true)]
        [TestCase(2, 1, false)]
        public void TestLessThanPosition(long a, long b, bool aEqualsB)
        {
            List<long> program = new List<long>()
            {
                8, 13, 14, 15,    // 3
                5, 15, 16,        // 6
                104, 0,           // 8
                99,               // 9
                104, 1,           // 11
                99,               // 12
                a, b, -1, 10      // 15
            };

            IntCodeVM2 vm = new IntCodeVM2(program);
            vm.ExecuteProgram();
            if (aEqualsB)
            {
                Assert.That(vm.GetLastOutput(), Is.EqualTo(1));
            }
            else
            {
                Assert.That(vm.GetLastOutput(), Is.EqualTo(0));
            }
        }

        [Test]
        [TestCase(1, 1, true)]
        [TestCase(2, 1, false)]
        public void TestLessThanDirect(long a, long b, bool aEqualsB)
        {
            List<long> program = new List<long>()
            {
                1108, a, b, 15,   // 3
                5, 15, 16,        // 6
                104, 0,           // 8
                99,               // 9
                104, 1,           // 11
                99,               // 12
                a, b, -1, 10      // 15
            };

            IntCodeVM2 vm = new IntCodeVM2(program);
            vm.ExecuteProgram();
            if (aEqualsB)
            {
                Assert.That(vm.GetLastOutput(), Is.EqualTo(1));
            }
            else
            {
                Assert.That(vm.GetLastOutput(), Is.EqualTo(0));
            }
        }

        [Test]
        [TestCase(1, 1, true)]
        [TestCase(2, 1, false)]
        public void TestLessThanBaseOffset(long a, long b, bool aEqualsB)
        {
            List<long> program = new List<long>()
            {
                109, 2,
                22208, 13, 14, 15,     // 3
                5, 17, 18,          // 6
                104, 0,             // 8
                99,               // 9
                104, 1,           // 11
                99,               // 12
                a, b, -1, 12          // 15
            };

            IntCodeVM2 vm = new IntCodeVM2(program);
            vm.ExecuteProgram();
            if (aEqualsB)
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
