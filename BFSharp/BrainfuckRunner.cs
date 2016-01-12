using System;
using System.Collections.Generic;

namespace BFSharp
{
    public class BrainfuckRunner
    {
        private readonly IMemory _memory;
        private Stack<int> _bracketsStack;
        private int _currentOperationPosition;
        private string _program;

        public BrainfuckRunner(IMemory memory)
        {
            _memory = memory;
        }

        private char CurrentOperation => _program[_currentOperationPosition];

        public int CurrentPosition => _memory.CursorPosition;

        private void HandleOpeningBracket()
        {
            if (_memory.CurrentValue == 0)
            {
                var innerLoopsCount = 0;
                ++_currentOperationPosition;
                while (CurrentOperation != ']' || innerLoopsCount != 0)
                {
                    if (CurrentOperation == '[')
                        ++innerLoopsCount;
                    if (CurrentOperation == ']')
                        --innerLoopsCount;
                    ++_currentOperationPosition;
                }
            }
            else
                _bracketsStack.Push(_currentOperationPosition);
        }

        private void HandleClosingBracket()
        {
            if (_memory.CurrentValue == 0)
            {
                _bracketsStack.Pop();
                return;
            }
            if (_bracketsStack.Count == 0)
                throw new ArgumentException("Unmatched ]");
            _currentOperationPosition = _bracketsStack.Peek();
        }

        public void Run(string program, Func<int> read, Action<char> write)
        {
            _currentOperationPosition = 0;
            _program = program;
            _bracketsStack = new Stack<int>();
            var operators = new Dictionary<char, Action>
            {
                ['.'] = () => write(Convert.ToChar(_memory.CurrentValue)),
                ['+'] = _memory.Increment,
                ['-'] = _memory.Decrement,
                ['>'] = _memory.MoveRight,
                ['<'] = _memory.MoveLeft,
                ['['] = HandleOpeningBracket,
                [']'] = HandleClosingBracket
            };

            while (_currentOperationPosition < program.Length)
            {
                try
                {
                    operators[CurrentOperation]();
                }
                catch (KeyNotFoundException)
                {
                    throw new ArgumentException("Invalid operator: " + CurrentOperation);
                }

                _currentOperationPosition++;
            }
        }
    }
}