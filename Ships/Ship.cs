using System.Collections.Generic;

namespace statkigra
{
    abstract class Ship
    {
        public string Name { get; protected set; }
        public int Size { get; protected set; }

        private readonly List<(int r, int c)> coords = new();
        public IReadOnlyList<(int r, int c)> Coordinates => coords.AsReadOnly();

        private int hitCount = 0;
        public bool IsSunk => hitCount >= Size;

        public void Place(IEnumerable<(int r, int c)> positions)
        {
            coords.Clear();
            coords.AddRange(positions);
        }

        public void RegisterHit()
        {
            if (hitCount < Size)
                hitCount++;
        }

        public abstract string OnSunkMessage();
    }
}
