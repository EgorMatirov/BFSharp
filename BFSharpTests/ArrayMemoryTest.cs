using BFSharp;
using NUnit.Framework;

namespace BFSharpTests
{
    [TestFixture]
    internal class ArrayMemoryTest
    {
        [SetUp]
        public void SetUp()
        {
            _memory = new ArrayMemory();
        }

        private ArrayMemory _memory;

        [Test]
        public void ResetShouldClearAndResetPosition()
        {
            _memory.MoveLeft();
            _memory.CurrentValue = 5;
            _memory.Reset();
            Assert.AreEqual(0, _memory.CurrentValue);
            Assert.AreEqual(0, _memory.CursorPosition);
        }

        [Test]
        public void DecrementShouldWorkCorrectly()
        {
            _memory.Decrement();
            Assert.AreEqual(255, _memory.CurrentValue);
        }

        [Test]
        public void GetAndSetShouldWorkCorrectly()
        {
            _memory.CurrentValue = 1;
            Assert.AreEqual(1, _memory.CurrentValue);
        }

        [Test]
        public void IncrementShouldWorkCorrectly()
        {
            _memory.Increment();
            Assert.AreEqual(1, _memory.CurrentValue);
        }

        [Test]
        public void MoveLeftShouldMoveOneStepToLeftIfCurrentPositionIsNotStart()
        {
            _memory.MoveRight();
            _memory.MoveLeft();
            Assert.AreEqual(0, _memory.CursorPosition);
        }

        [Test]
        public void MoveLeftShouldMoveToEndIfCurrentPositionIsStart()
        {
            _memory.MoveLeft();
            Assert.AreEqual(29999, _memory.CursorPosition);
        }

        [Test]
        public void MoveRightShouldMoveOneStepToRightIfCurrentPositionIsNotEnd()
        {
            _memory.MoveRight();
            Assert.AreEqual(1, _memory.CursorPosition);
        }


        [Test]
        public void MoveRightShouldMoveToStartIfCurrentPositionIsEnd()
        {
            _memory.MoveLeft();
            _memory.MoveRight();
            Assert.AreEqual(0, _memory.CursorPosition);
        }
    }
}