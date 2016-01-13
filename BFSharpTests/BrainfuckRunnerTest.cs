using System;
using BFSharp;
using FakeItEasy;
using NUnit.Framework;

namespace BFSharpTests
{
    internal class BrainfuckRunnerTest
    {
        private IMemory _memoryMock;
        private Func<int> _readFunc;
        private Action<char> _writeAction;

        [SetUp]
        public void SetUp()
        {
            _memoryMock = A.Fake<IMemory>();
            _writeAction = A.Fake<Action<char>>();
            _readFunc = A.Fake<Func<int>>();
        }

        [Test]
        public void RunShouldThrowExceptionForInvalidOperation()
        {
            var runner = new BrainfuckRunner(_memoryMock, "%", _readFunc, _writeAction);
            Assert.Throws<ArgumentException>(() => runner.Run());
        }

        [Test]
        public void RunShouldPrintCharCorrectly()
        {
            var runner = new BrainfuckRunner(_memoryMock, ".", _readFunc, _writeAction);
            runner.Run();
            A.CallTo(() => _writeAction('\0')).MustHaveHappened(Repeated.Exactly.Once);
        }

        [Test]
        public void RunShouldReadCharCorrectly()
        {
            const string program = ",";
            var runner = new BrainfuckRunner(_memoryMock, program, _readFunc, _writeAction);
            A.CallTo(() => _readFunc()).Returns('A');

            runner.Run();

            A.CallTo(() => _readFunc()).MustHaveHappened(Repeated.Exactly.Once);
            A.CallTo(_memoryMock)
                .Where(call => call.Method.Name == "set_CurrentValue" && call.Arguments.Get<byte>(0) == 'A')
                .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Test]
        public void RunShouldIncrementCorrectly()
        {
            var program = new string('+', 65) + ".";
            var runner = new BrainfuckRunner(_memoryMock, program, _readFunc, _writeAction);

            runner.Run();

            A.CallTo(() => _memoryMock.Increment()).MustHaveHappened(Repeated.Exactly.Times(65));
        }

        [Test]
        public void RunShouldDecrementCorrectly()
        {
            var program = new string('-', 191);
            var runner = new BrainfuckRunner(_memoryMock, program, _readFunc, _writeAction);

            runner.Run();

            A.CallTo(() => _memoryMock.Decrement()).MustHaveHappened(Repeated.Exactly.Times(191));
        }

        [Test]
        public void RunShouldMoveLeftCorrectly()
        {
            var program = new string('<', 2);
            var runner = new BrainfuckRunner(_memoryMock, program, _readFunc, _writeAction);

            runner.Run();

            A.CallTo(() => _memoryMock.MoveLeft()).MustHaveHappened(Repeated.Exactly.Twice);
        }

        [Test]
        public void RunShouldMoveRightCorrectly()
        {
            var program = new string('>', 2);
            var runner = new BrainfuckRunner(_memoryMock, program, _readFunc, _writeAction);

            runner.Run();

            A.CallTo(() => _memoryMock.MoveRight()).MustHaveHappened(Repeated.Exactly.Twice);
        }

        [Test]
        public void RunShouldHandleSimpleLoopCorrectly()
        {
            const string program = "+++[-].";
            var runner = new BrainfuckRunner(_memoryMock, program, _readFunc, _writeAction);

            runner.Run();

            A.CallTo(() => _writeAction('\0')).MustHaveHappened(Repeated.Exactly.Once);
        }

        [Test]
        [TestCase("[>[+]]")]
        [TestCase("+[>+[+]]")]
        public void RunShouldHandleInnerLoopsCorrectly(string program)
        {
            var currentPosition = 0;
            var array = new byte[30000];
            A.CallTo(() => _memoryMock.Increment()).Invokes(() => ++array[currentPosition]);
            A.CallTo(() => _memoryMock.CurrentValue).ReturnsLazily(() => array[currentPosition]);
            A.CallTo(() => _memoryMock.MoveRight()).Invokes(() => currentPosition = (currentPosition + 1)%3000);
            var runner = new BrainfuckRunner(_memoryMock, program, _readFunc, _writeAction);

            runner.Run();

            Assert.AreEqual(0, _memoryMock.CurrentValue);
        }

        [Test]
        [ExpectedException(typeof (ArgumentException))]
        public void RunShouldThrowExceptionForUnmatchedClosingBracket()
        {
            // ReSharper disable once UnusedVariable
            var runner = new BrainfuckRunner(_memoryMock, "+++]", _readFunc, _writeAction);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void RunShouldThrowExceptionForUnmatchedOpeningBracket()
        {
            // ReSharper disable once UnusedVariable
            var runner = new BrainfuckRunner(_memoryMock, "[+++", _readFunc, _writeAction);
        }
    }
}