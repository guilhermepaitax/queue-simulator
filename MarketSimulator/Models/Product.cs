using System;



namespace MarketSimulator.Models
{
    class Product   // Pordutos de clientes
    {
        public Product()
        {
            Random rand = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);
            this.ProductTime = rand.Next(100, 3000);
        }

        public int ProductTime { get; }
    }
}
