namespace BFSharp
{
    public class ArrayMemory : IMemory
    {
        private const ushort Size = 30000;
        private readonly byte[] _memory = new byte[Size];
        public uint CursorPosition { get; private set; }

        public byte CurrentValue
        {
            get { return _memory[CursorPosition]; }
            set { _memory[CursorPosition] = value; }
        }

        public void MoveLeft()
        {
            if (CursorPosition == 0)
                CursorPosition = Size - 1;
            else
                --CursorPosition;
        }

        public void MoveRight()
        {
            CursorPosition = ++CursorPosition%Size;
        }

        public void Reset()
        {
            for (var i = 0; i < _memory.Length; ++i)
                _memory[i] = 0;
            CursorPosition = 0;
        }

        public void Increment() => ++CurrentValue;
        public void Decrement() => --CurrentValue;
    }
}