namespace BFSharp
{
    public class ArrayMemory : IMemory
    {
        private readonly byte[] _memory = new byte[30000];
        public ushort CursorPosition { get; private set; }

        public byte CurrentValue => _memory[CursorPosition];

        public void MoveLeft()
        {
            if (CursorPosition == 0)
                CursorPosition = 29999;
            else
                CursorPosition -= 1;
        }

        public void MoveRight()
        {
            if (CursorPosition == 29999)
                CursorPosition = 0;
            else
                CursorPosition += 1;
        }

        public void Clear()
        {
            for (var i = 0; i < _memory.Length; ++i)
                _memory[i] = 0;
        }

        public void Increment() => ++_memory[CursorPosition];
        public void Decrement() => --_memory[CursorPosition];
    }
}