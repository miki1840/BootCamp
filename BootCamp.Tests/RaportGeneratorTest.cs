using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BootCamp.Classes;
using System.Text;

namespace BootCamp.Tests
{
    [TestClass]
    public class RaportGeneratorTest
    {
        private Random rand = new Random();
        private string generateCorrectString(int length)
        {
            length--;
            StringBuilder builder = new StringBuilder();
            int size = rand.Next(length) + 1;
            for(int i=0; i<size;i++)
            {
                char t;
                do
                {
                    t =(char) rand.Next(126);
                } while ((int)t<33);

                builder.Append(t);
            }

            return builder.ToString();
        }

        private void mixtable(List<DataBaseRow> table, int iterations)
        {
            for (int i = 0; i < iterations; i++)
            {
                int a = rand.Next(table.Count);
                int b = rand.Next(table.Count);
                DataBaseRow t = table[a];
                table[a] = table[b];
                table[b] = t;
            }
        }

        [TestMethod]
        public void AvreageCostOfOrderTest()
        {
            List<DataBaseRow> dataBase = new List<DataBaseRow>();
            double sumedCost = 0;
            long reqID = 0;
            string clientID = generateCorrectString(6);
            for (int i=0; i<100;i++)
            {
                double price = (double) rand.Next(200000) / 100;
                int quantity = rand.Next(100);
                sumedCost += price * quantity;
                dataBase.Add(new DataBaseRow(clientID, reqID, generateCorrectString(255), quantity, price));
                if (rand.Next(2) == 1 && i<100-1) reqID++;
            }
            RaportGenerator raportGenerator = new RaportGenerator(dataBase);
            raportGenerator.DataBase = dataBase;
            reqID++;
            Assert.IsTrue(Math.Abs(raportGenerator.AverageCostOfOrfder() - sumedCost/reqID) < 0.01);
        }

        [TestMethod]
        public void AvreageCostOfCLientOrder()
        {
            List<DataBaseRow> dataBase = new List<DataBaseRow>();
            int numberOfCLients = rand.Next(30) + 1;
            double[] expectedAvreageOdOrders = new double[numberOfCLients];
            for (int i = 0; i < numberOfCLients; i++)
            {
                int size = rand.Next(50)+10;
                double sumedCost=0;
                long reqId = 1;
                for (int j = 0; j < size; j++)
                {
                    double price = (double)rand.Next(200000) / 100;
                    int quantity = rand.Next(100);
                    sumedCost += quantity * price;
                    dataBase.Add(new DataBaseRow("cli"+i,reqId,generateCorrectString(255),quantity,price));
                    if (rand.Next(5) == 1&& j<size-1) reqId++;
                }
                expectedAvreageOdOrders[i] = sumedCost / (double)reqId;
            }

            int numerOfAditionalRows = rand.Next(1000);
            for (int i = 0; i < numerOfAditionalRows; i++)
            {
                double price = (double)rand.Next(20000) / 100;
                int quantity = rand.Next(50);
                dataBase.Add(new DataBaseRow("z"+generateCorrectString(5),rand.Next(),generateCorrectString(255),quantity, price));
            }
            
            mixtable(dataBase, 300);
            RaportGenerator raportGenerator = new RaportGenerator(dataBase);
            for (int i = 0; i < numberOfCLients; i++)
            {
                Assert.IsTrue(Math.Abs(expectedAvreageOdOrders[i]-raportGenerator.AvreageCostOfClientsOrder("cli"+i))<1);
            }

        }

        [TestMethod]
        public void QuantityOfOrdersTest()
        {
            List<DataBaseRow> dataBase = new List<DataBaseRow>();
            long requestId = 0;
            int cliID = 0;
            int expectedQuantity = 0;
            int databaseSize = rand.Next(2000);
            for (int i = 0; i < databaseSize; i++)
            {
                double price = (double) rand.Next(200000) / 100;
                int quantity = rand.Next(100);
                if (rand.Next(4) == 1)
                {
                    requestId++;
                    expectedQuantity++;
                }
                else
                {
                    cliID++;
                    requestId = 0;
                    expectedQuantity++;
                }

                DataBaseRow toInsert =
                    new DataBaseRow("cli" + cliID, requestId, generateCorrectString(255), quantity, price);
                dataBase.Add(toInsert);
            }
            RaportGenerator raportGenerator= new RaportGenerator(dataBase);
            Assert.AreEqual(expectedQuantity, raportGenerator.QuantityOfOrders());
        }

        [TestMethod]
        public void QuantityOfClientsOrdersTest()
        {
            List<DataBaseRow> dataBase = new List<DataBaseRow>();
            int numberOfCLients = rand.Next(30) + 1;
            long[] expecteQuantityOfOrders = new long[numberOfCLients];
            for (int i = 0; i < numberOfCLients; i++)
            {
                int size = rand.Next(50)+10;
                long reqId = 1;
                for (int j = 0; j < size; j++)
                {
                    double price = (double)rand.Next(200000) / 100;
                    int quantity = rand.Next(100);
                    dataBase.Add(new DataBaseRow("cli" + i, reqId, generateCorrectString(255), quantity, price));
                    if (rand.Next(5) == 1 && j < size - 1) reqId++;
                }
                expecteQuantityOfOrders[i] = reqId;
            }

            int numerOfAditionalRows = rand.Next(1000);
            for (int i = 0; i < numerOfAditionalRows; i++)
            {
                double price = (double)rand.Next(200000) / 100;
                int quantity = rand.Next(100);
                dataBase.Add(new DataBaseRow("z" + generateCorrectString(5), rand.Next(), generateCorrectString(255), quantity, price));
            }

            mixtable(dataBase,300);
            RaportGenerator raportGenerator = new RaportGenerator(dataBase);
            for (int i = 0; i < numberOfCLients; i++)
            {
                Assert.AreEqual(expecteQuantityOfOrders[i], raportGenerator.QuantityOfClientOrders("cli" + i));
            }

        }

        [TestMethod]
       public void SumedCostOfOrdersTest()
        {
            List<DataBaseRow> dataBase = new List<DataBaseRow>();
            double expectedSumedCost = 0;
            int size = rand.Next(2000);
            for (int i = 0; i < size; i++)
            {
                double price = (double)rand.Next(200000) / 100;
                int quantity = rand.Next(100);
                dataBase.Add(new DataBaseRow(generateCorrectString(6), rand.Next(), generateCorrectString(255), quantity, price));
                expectedSumedCost += quantity * price;
            }

            RaportGenerator raportGenerator= new RaportGenerator(dataBase);
            Assert.IsTrue(Math.Abs(expectedSumedCost-raportGenerator.SumedCostOfOrdes())<0.1);
        }

       [TestMethod]
       public void SumedCostOfCLientsOrdersTest()
       {
           List<DataBaseRow> dataBase = new List<DataBaseRow>();
           int numberOfCLients = rand.Next(30) + 1;
           double[] expecteSumOfOrders = new double[numberOfCLients];
           for (int i = 0; i < numberOfCLients; i++)
           {
               int size = rand.Next(100);
               double sumedCost = 0;
               long reqId = 1;
               for (int j = 0; j < size; j++)
               {
                   double price = (double)rand.Next(200000) / 100;
                   int quantity = rand.Next(100);
                   sumedCost += quantity * price;
                   dataBase.Add(new DataBaseRow("cli" + i, reqId, generateCorrectString(255), quantity, price));
                   if (rand.Next(5) == 1 && j < size - 1) reqId++;
               }
               expecteSumOfOrders[i] = sumedCost;
           }

           int numerOfAditionalRows = rand.Next(1000);
           for (int i = 0; i < numerOfAditionalRows; i++)
           {
               double price = (double)rand.Next(200000) / 100;
               int quantity = rand.Next(100);
               dataBase.Add(new DataBaseRow("z" + generateCorrectString(5), rand.Next(), generateCorrectString(255), quantity, price));
           }

           mixtable(dataBase,300);
           RaportGenerator raportGenerator= new RaportGenerator(dataBase);
           for (int i = 0; i < numberOfCLients; i++)
           {
               Assert.IsTrue(Math.Abs(expecteSumOfOrders[i]-raportGenerator.SumCostOfClientsOrders("cli"+i))<0.01);
           }
        }

        

    }
}
