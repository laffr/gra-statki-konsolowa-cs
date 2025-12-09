using System;

namespace statkigra
{
    class Game
    {
        private readonly Board enemyBoard;
        private readonly int maxShots;
        private readonly bool testMode;

        public Game(int maxShots, bool testMode)
        {
            this.maxShots = maxShots;
            this.testMode = testMode;

            enemyBoard = new Board(10);
            enemyBoard.PlaceShipRandomly(new Destroyer());
            enemyBoard.PlaceShipRandomly(new Cruiser());
            enemyBoard.PlaceShipRandomly(new Carrier());
        }

        public void Run()
        {
            int shots = 0;

            while (shots < maxShots)
            {
                Console.Clear();
                Console.WriteLine($"Strzał {shots + 1}/{maxShots}\n");

                enemyBoard.Print(hideShips: !testMode, revealAsS: testMode);

                Console.Write("\nPodaj cel (np. F7, 7F, F 7): ");
                string input = Console.ReadLine();

                if (!ParseCoordinate(input, out int r, out int c))
                {
                    Console.WriteLine("Niepoprawne współrzędne.");
                    Pause();
                    continue;
                }

                var (result, sunkName) = enemyBoard.FireAt(r, c);

                if (result != FireResult.AlreadyTried)
                    shots++;

                Console.WriteLine(result switch
                {
                    FireResult.Miss => "Pudło!",
                    FireResult.Hit => "Trafienie!",
                    FireResult.Sunk => $"{sunkName} zatopiony!",
                    FireResult.AlreadyTried => "Już tu strzelałeś.",
                    _ => ""
                });

                if (enemyBoard.AllSunk())
                {
                    Console.WriteLine("\nWygrałeś!");
                    return;
                }

                Pause();
            }

            Console.WriteLine("\nKoniec strzałów. Przegrałeś.");
            Console.WriteLine("\nPlansza końcowa:");
            enemyBoard.Print(hideShips: false, revealAsS: true);
        }

        private bool ParseCoordinate(string input, out int r, out int c)
        {
            r = c = -1;
            if (string.IsNullOrWhiteSpace(input)) return false;

            input = input.Trim().ToUpper().Replace(",", " ").Replace(";", " ");
            string letters = "";
            string digits = "";

            foreach (char ch in input)
            {
                if (char.IsLetter(ch)) letters += ch;
                if (char.IsDigit(ch)) digits += ch;
            }

            if (letters.Length != 1) return false;
            if (digits.Length == 0) return false;

            r = letters[0] - 'A';
            if (!int.TryParse(digits, out c)) return false;
            c--;

            return r >= 0 && r < 10 && c >= 0 && c < 10;
        }

        private void Pause()
        {
            Console.WriteLine("Naciśnij Enter...");
            Console.ReadLine();
        }
    }
}
