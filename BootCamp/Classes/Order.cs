using System;
using System.Collections.Generic;
using System.Text;

namespace BootCamp.Classes
{
    public class Order
    {
        public string ClientId { get; set; }
        public long RequestId { get; set; }
        public double Price { get; set; }

        public Order()
        {
            Price = 0;
        }

        public Order(string clientId, long requestId)
        {
            ClientId = clientId;
            RequestId = requestId;
        }

        public void print()
        {
            Console.WriteLine("Id Klienta: "+ClientId+"\nId Zapytania: "+RequestId+"\nCena: "+Price);
        }

    }
}
