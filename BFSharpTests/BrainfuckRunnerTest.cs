using System;
using BFSharp;
using FakeItEasy;
using NUnit.Framework;

namespace BFSharpTests
{
    internal class BrainfuckRunnerTest
    {
        private Func<int> _readFunc;
        private BrainfuckRunner _runner;
        private Action<char> _writeAction;

        [SetUp]
        public void SetUp()
        {
            _runner = new BrainfuckRunner(new ArrayMemory());
            _writeAction = A.Fake<Action<char>>();
            _readFunc = A.Fake<Func<int>>();
        }

        [Test]
        public void RunShouldThrowExceptionForInvalidOperation()
        {
            Assert.Throws<ArgumentException>(() => _runner.Run("%", _readFunc, _writeAction));
        }

        [Test]
        public void RunShouldPrintCharCorrectly()
        {
            _runner.Run(".", _readFunc, _writeAction);
            A.CallTo(() => _writeAction('\0')).MustHaveHappened();
        }

        [Test]
        public void RunShouldIncrementCorrectly()
        {
            var program = new string('+', 65) + ".";
            _runner.Run(program, _readFunc, _writeAction);
            A.CallTo(() => _writeAction('A')).MustHaveHappened();
        }

        [Test]
        public void RunShouldDecrementCorrectly()
        {
            var program = new string('-', 191) + ".";
            _runner.Run(program, _readFunc, _writeAction);
            A.CallTo(() => _writeAction('A')).MustHaveHappened();
        }

        [Test]
        public void RunShouldMoveLeftCorrectly()
        {
            var program = new string('<', 2);
            _runner.Run(program, _readFunc, _writeAction);
            Assert.AreEqual(29998, _runner.CurrentPosition);
        }

        [Test]
        public void RunShouldMoveRightCorrectly()
        {
            var program = new string('>', 3);
            _runner.Run(program, _readFunc, _writeAction);
            Assert.AreEqual(3, _runner.CurrentPosition);
        }

        [Test]
        public void RunShouldHandleSimpleLoopCorrectly()
        {
            const string program = "+++[-].";
            _runner.Run(program, _readFunc, _writeAction);
            A.CallTo(() => _writeAction('\0')).MustHaveHappened();
        }

        [Test]
        [TestCase("[>[+]].")]
        [TestCase("+[>[+]].")]
        public void RunShouldHandleInnerLoopsCorrectly(string program)
        {
            _runner.Run(program, _readFunc, _writeAction);
            A.CallTo(() => _writeAction('\0')).MustHaveHappened();
        }

        [Test]
        public void RunShouldThrowExceptionForUnmatchedCloseinBracket()
        {
            Assert.Throws<ArgumentException>(() => _runner.Run("+++]", _readFunc, _writeAction));
        }
    }
}