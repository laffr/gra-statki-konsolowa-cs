namespace statkigra
{
    class Cruiser : Ship
    {
        public Cruiser()
        {
            Name = "Krążownik";
            Size = 3;
        }

        public override string OnSunkMessage() => $"{Name} zatopiony";
    }
}
