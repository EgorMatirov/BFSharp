using System;

namespace BFSharp
{
    public class BrainfuckRunner
    {
        public int CurrentPosition => _memory.CursorPosition;
        private readonly IMemory _memory;
        private int _currentOperationPosition;

        public BrainfuckRunner(IMemory memory)
        {
            _memory = memory;
        }

        public void Run(string program, Func<int> read, Action<char> write)
        {
            _currentOperationPosition = 0;
            while (_currentOperationPosition < program.Length)
            {
                var currentOperation = program[_currentOperationPosition];
                switch (currentOperation)
                {
                    case '.':
                        write(Convert.ToChar(_memory.CurrentValue));
                        break;
                    case '+':
                        _memory.Increment();
                        break;
                    case '-':
                        _memory.Decrement();
                        break;
                    case '>':
                        _memory.MoveRight();
                        break;
                    case '<':
                        _memory.MoveLeft();
                        break;
                    default:
                        throw new ArgumentException("Invalid operation " + currentOperation);
                }
                _currentOperationPosition++;
            }
        }
    }
}