namespace BFSharp
{
    public interface IMemory
    {
        byte CurrentValue { get; set; }
        uint CursorPosition { get; }

        void Decrement();
        void Increment();
        void MoveLeft();
        void MoveRight();
        void Reset();
    }
}