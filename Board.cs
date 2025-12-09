using System;
using System.Collections.Generic;
using System.Linq;

namespace statkigra
{
    class Board
    {
        private readonly int size;
        private readonly CellState[,] grid;
        private readonly List<Ship> ships = new();
        private readonly Random rnd = new();

        public Board(int size)
        {
            this.size = size;
            grid = new CellState[size, size];
        }

        public void AddShip(Ship ship) => ships.Add(ship);

        public bool AllSunk() => ships.Count > 0 && ships.All(s => s.IsSunk);

        private bool InBounds(int r, int c) => r >= 0 && r < size && c >= 0 && c < size;

        private bool AreaClear(int r, int c)
        {
            for (int rr = r - 1; rr <= r + 1; rr++)
                for (int cc = c - 1; cc <= c + 1; cc++)
                    if (InBounds(rr, cc) && grid[rr, cc] == CellState.Ship)
                        return false;

            return true;
        }

        private bool CanPlace(int r, int c, int length, bool horizontal)
        {
            for (int i = 0; i < length; i++)
            {
                int rr = r + (horizontal ? 0 : i);
                int cc = c + (horizontal ? i : 0);

                if (!InBounds(rr, cc) || !AreaClear(rr, cc))
                    return false;
            }
            return true;
        }

        public bool PlaceShipRandomly(Ship ship)
        {
            for (int attempts = 0; attempts < 300; attempts++)
            {
                int r = rnd.Next(size);
                int c = rnd.Next(size);
                bool horiz = rnd.Next(2) == 0;

                if (!CanPlace(r, c, ship.Size, horiz)) continue;

                var coords = new List<(int r, int c)>();

                for (int i = 0; i < ship.Size; i++)
                {
                    int rr = r + (horiz ? 0 : i);
                    int cc = c + (horiz ? i : 0);

                    grid[rr, cc] = CellState.Ship;
                    coords.Add((rr, cc));
                }

                ship.Place(coords);
                AddShip(ship);
                return true;
            }

            return false;
        }

        public (FireResult result, string sunkName) FireAt(int r, int c)
        {
            if (!InBounds(r, c)) return (FireResult.Miss, null);

            var state = grid[r, c];

            if (state == CellState.Miss || state == CellState.Hit)
                return (FireResult.AlreadyTried, null);

            if (state == CellState.Empty)
            {
                grid[r, c] = CellState.Miss;
                return (FireResult.Miss, null);
            }

            if (state == CellState.Ship)
            {
                grid[r, c] = CellState.Hit;

                foreach (var ship in ships)
                {
                    if (ship.Coordinates.Contains((r, c)))
                    {
                        ship.RegisterHit();
                        if (ship.IsSunk)
                            return (FireResult.Sunk, ship.Name);

                        return (FireResult.Hit, null);
                    }
                }
            }

            return (FireResult.Miss, null);
        }

        public void Print(bool hideShips, bool revealAsS)
        {
            Console.Write("   ");
            for (int c = 1; c <= size; c++)
                Console.Write($" {c} ");
            Console.WriteLine();

            for (int r = 0; r < size; r++)
            {
                Console.Write($" {(char)('A' + r)} ");

                for (int c = 0; c < size; c++)
                {
                    var st = grid[r, c];
                    char ch = st switch
                    {
                        CellState.Empty => hideShips ? '.' : '.',
                        CellState.Miss => 'o',
                        CellState.Hit => 'X',
                        CellState.Ship => hideShips ? '.' : (revealAsS ? 'S' : '#'),
                        _ => '.'
                    };

                    Console.Write($" {ch} ");
                }

                Console.WriteLine();
            }
        }
    }
}
