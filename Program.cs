using System;

namespace statkigra
{
    class Program
    {
        static void Main()
        {
            Console.Write("Tryb testowy? (T/N): ");
            bool testMode = Console.ReadLine().Trim().ToUpper() == "T";

            Game g = new Game(30, testMode);
            g.Run();
        }
    }
}
