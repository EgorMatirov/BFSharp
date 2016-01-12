namespace BFSharp
{
    public class ArrayMemory : IMemory
    {
        public ushort CursorPosition { get; private set; }

        public byte CurrentValue => _memory[CursorPosition];

        private readonly byte[] _memory = new byte[30000];

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

        public void Increment() => _memory[CursorPosition]++;
        public void Decrement() => _memory[CursorPosition]--;
    }
}