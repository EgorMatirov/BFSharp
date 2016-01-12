namespace BFSharp
{
    public interface IMemory
    {
        byte CurrentValue { get; }
        ushort CursorPosition { get; }

        void Decrement();
        void Increment();
        void MoveLeft();
        void MoveRight();
        void Clear();
    }
}