using System;
using System.Collections.Generic;
using System.Text;

namespace BootCamp.Classes
{
    public class DataBaseRow
    {
        public string ClientId { get; set; }
        public long RequestId { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }

        public DataBaseRow(string cid, long reqid, string name, int quant, double price)
        {
            ClientId = cid;
            RequestId = reqid;
            Name = name;
            Quantity = quant;
            Price = price;
        }

        public void print()
        {
            Console.WriteLine("ClientId: "+ClientId+"\nRequestId: "+RequestId+"\n Name: "+Name+"\n Quantity: "+Quantity+"\nPrice: "+Price);
        }

    }
}
