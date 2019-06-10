using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using BootCamp.Interfaces;

namespace BootCamp.Classes
{
     public class RaportGenerator : IRaportGenerator
    {
        public List<DataBaseRow> DataBase{ get; set; }

        public RaportGenerator(List<DataBaseRow> db)
        {
            DataBase = db;
        }
        public double AverageCostOfOrfder()
        {
            var sum = DataBase.Sum(x => x.Price * x.Quantity);
            var quantityOfOrders = DataBase.GroupBy(x => new {x.ClientId, x.RequestId}).Count();
            return sum / quantityOfOrders;
        }

        public double AvreageCostOfClientsOrder(string clientId)
        {
            var sum = DataBase.Where(x => x.ClientId.Equals(clientId)).Sum(y => y.Price * y.Quantity);
            var quantityOfOrders = DataBase.Where(x => x.ClientId.Equals(clientId)).GroupBy(y=>y.RequestId).Count();
            
            return sum/quantityOfOrders;

        }

        public IEnumerable ListOfClientsOrders(string clientId)
        {
            var sortedList = DataBase.Where(x => x.ClientId.Equals(clientId)).OrderBy(y => y.RequestId).ToList();
            if (sortedList.Count > 0)
            {
                List<Order> clientsOrders = new List<Order>();
                long lastRequestId = sortedList[0].RequestId;
                Order insertingOrder = new Order(sortedList[0].ClientId, sortedList[0].RequestId);
                foreach (DataBaseRow row in sortedList)
                {
                    if (row.RequestId != lastRequestId)
                    {
                        clientsOrders.Add(insertingOrder);
                        insertingOrder = new Order(row.ClientId, row.RequestId);
                        lastRequestId = row.RequestId;
                    }

                    insertingOrder.Price += row.Price * row.Quantity;
                }

                if (insertingOrder.Price > 0) clientsOrders.Add(insertingOrder);
                return clientsOrders;
            }
            else return null;

        }

        public IEnumerable ListOfOrders()
        {
            var sortedList = DataBase.OrderBy(x => x.ClientId).ThenBy(y => y.RequestId).ToList();
            if (sortedList.Count > 0)
            {
                List<Order> clientsOrders = new List<Order>();
                long lastRequestId = sortedList[0].RequestId;
                string lastClientId = sortedList[0].ClientId;
                Order insertingOrder = new Order(sortedList[0].ClientId, sortedList[0].RequestId);
                foreach (DataBaseRow row in sortedList)
                {
                    if (row.RequestId != lastRequestId|| !row.ClientId.Equals(lastClientId))
                    {
                        clientsOrders.Add(insertingOrder);
                        insertingOrder = new Order(row.ClientId, row.RequestId);
                        lastRequestId = row.RequestId;
                        lastClientId = row.ClientId;
                    }

                    insertingOrder.Price += row.Price * row.Quantity;
                }

                if (insertingOrder.Price > 0) clientsOrders.Add(insertingOrder);
                return clientsOrders;
            }
            else return null;
        }

        public IEnumerable OrdersBetwenCost(double lowerBand, double upperBand)
        {
            var ordersList = ((IEnumerable<Order>) ListOfOrders()).ToList();
            List<Order> selectedOrders=new List<Order>();
            foreach (var order in ordersList)
            {
                if (order.Price >= lowerBand && order.Price <= upperBand)
                {
                    selectedOrders.Add(order);
                }
            }

            return selectedOrders;
        }

        public IEnumerable QuantiOfCleintsOrdersByName(string clientId)
        {

            List<DataBaseRow> sortedList = DataBase.Where(x=> x.ClientId.Equals(clientId)).OrderBy(x => x.Name).ToList();

            if (sortedList.Count > 0)
            {
                List<ProductRow> productList = new List<ProductRow>();
                string lastName = sortedList[0].Name;
                ProductRow insertingRow = new ProductRow(lastName);
                foreach (DataBaseRow row in sortedList)
                {
                    if (!row.Name.Equals(lastName))
                    {
                        productList.Add(insertingRow);
                        lastName = row.Name;
                        insertingRow = new ProductRow(lastName);
                    }

                    insertingRow.Quantity += row.Quantity;
                }
                if (insertingRow.Quantity > 0) productList.Add(insertingRow);
                return productList;
            }
            else return null;
        }

        public int QuantityOfClientOrders(string clientId)
        {
            return DataBase.Where(x => x.ClientId.Equals(clientId)).GroupBy(y => y.RequestId).Count();
        }

        public int QuantityOfOrders()
        {
            return DataBase.GroupBy(x => new {x.ClientId, x.RequestId}).Count();
        }

        public IEnumerable QuantityOfOrdersByName()
        {
            List<DataBaseRow> sortedList = DataBase.OrderBy(x => x.Name).ToList();

            if (sortedList.Count > 0)
            {
                List<ProductRow> productList = new List<ProductRow>();
                string lastName = sortedList[0].Name;
                ProductRow insertingRow = new ProductRow(lastName);
                foreach (DataBaseRow row in sortedList)
                {
                    if (!row.Name.Equals(lastName))
                    {
                        productList.Add(insertingRow);
                        lastName = row.Name;
                        insertingRow=new ProductRow(lastName);
                    }

                    insertingRow.Quantity += row.Quantity;
                }
                if (insertingRow.Quantity>0)productList.Add(insertingRow);
                return productList;
            }
            else return null;
            
        }

        public double SumCostOfClientsOrders(string clientId)
        {
            return DataBase.Where(x => x.ClientId.Equals(clientId)).Sum(y => y.Price * y.Quantity);
        }

        public double SumedCostOfOrdes()
        {
            return DataBase.Sum(x => x.Price * x.Quantity);
        }
    }
}
