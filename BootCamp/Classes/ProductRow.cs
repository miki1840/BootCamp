using System;
using System.Collections.Generic;
using System.Text;

namespace BootCamp.Classes
{
    class ProductRow
    {
        public string Name { get; set; }
        public int Quantity { get; set; }

        public ProductRow()
        {
            Quantity = 0;
        }

        public ProductRow(string name)
        {
            Name = name;
            Quantity = 0;
        }

        public void print()
        {
            Console.WriteLine("Nazwa: "+Name+" Ilość: "+Quantity);
        }
    }
}
