namespace statkigra
{
    class Destroyer : Ship
    {
        public Destroyer()
        {
            Name = "Niszczyciel";
            Size = 2;
        }

        public override string OnSunkMessage() => $"{Name} zatopiony";
    }
}
