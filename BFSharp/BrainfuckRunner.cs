using System;
using System.Collections.Generic;
using System.Linq;

namespace BFSharp
{
    public class BrainfuckRunner
    {
        private readonly Dictionary<int, int> _bracketsDictionary;
        private readonly IMemory _memory;
        private readonly Dictionary<char, Action> _operators;
        private readonly string _program;
        private int _currentOperationPosition;
        private char CurrentOperation => _program[_currentOperationPosition];

        public BrainfuckRunner(IMemory memory, string program, Func<int> read, Action<char> write)
        {
            _memory = memory;
            _program = program;
            _bracketsDictionary = new Dictionary<int, int>();
            _operators = new Dictionary<char, Action>
            {
                ['.'] = () => write(Convert.ToChar(_memory.CurrentValue)),
                [','] = () => _memory.CurrentValue = Convert.ToByte(read()),
                ['+'] = _memory.Increment,
                ['-'] = _memory.Decrement,
                ['>'] = _memory.MoveRight,
                ['<'] = _memory.MoveLeft,
                ['['] = HandleBracket,
                [']'] = HandleBracket
            };

            // Parse brackets.
            var parsingResult = _program
                .ToList()
                .Select((ch, i) => new {type = ch, pos = i})
                .Where(x => x.type == '[' || x.type == ']')
                .Aggregate(new {dictionary = new Dictionary<int, int>(), stack = new Stack<int>()}, (status, current) =>
                {
                    if (current.type == '[')
                        status.stack.Push(current.pos);
                    else
                    {
                        if (status.stack.Count == 0)
                            throw new ArgumentException("Unmatched ']' closing bracket");
                        var begin = status.stack.Pop();
                        var end = current.pos;
                        status.dictionary[begin] = end;
                        status.dictionary[end] = begin;
                    }
                    return new {status.dictionary, status.stack};
                });

            if (parsingResult.stack.Count != 0)
                throw new ArgumentException("Unmatched '[' opening bracket");

            _bracketsDictionary = parsingResult.dictionary;
        }

        private void HandleBracket()
        {
            var condition = CurrentOperation == '[' ? _memory.CurrentValue == 0 : _memory.CurrentValue != 0;
            if (condition)
            {
                _currentOperationPosition = _bracketsDictionary[_currentOperationPosition];
            }
        }

        public void Run()
        {
            _memory.Reset();
            _currentOperationPosition = 0;

            while (_currentOperationPosition < _program.Length)
            {
                if (!_operators.ContainsKey(CurrentOperation))
                    throw new ArgumentException("Invalid operator: " + CurrentOperation);
                _operators[CurrentOperation]();
                _currentOperationPosition++;
            }
        }
    }
}