namespace statkigra
{
    class Carrier : Ship
    {
        public Carrier()
        {
            Name = "Lotniskowiec";
            Size = 5;
        }

        public override string OnSunkMessage() => $"{Name} zatopiony (duży cel)";
    }
}
