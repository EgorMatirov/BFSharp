using BFSharp;
using NUnit.Framework;

namespace BFSharpTests
{
    [TestFixture]
    internal class ArrayMemoryTest
    {
        private ArrayMemory _memory;

        [SetUp]
        public void SetUp()
        {
            _memory = new ArrayMemory();
        }

        [Test]
        public void IncrementShouldWorkCorrectly()
        {
            _memory.Increment();
            Assert.AreEqual(1, _memory.CurrentValue);
        }

        [Test]
        public void DecrementShouldWorkCorrectly()
        {
            _memory.Decrement();
            Assert.AreEqual(255, _memory.CurrentValue);
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

        [Test]
        public void MoveLeftShouldMoveOneStepToLeftIfCurrentPositionIsNotStart()
        {
            _memory.MoveRight();
            _memory.MoveLeft();
            Assert.AreEqual(0, _memory.CursorPosition);
        }
    }
}
