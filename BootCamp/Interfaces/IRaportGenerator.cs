using System;
using System.Collections;
using System.Collections.Generic;

namespace BootCamp.Interfaces
{
    public interface IRaportGenerator
    {
        int QuantityOfOrders();
        int QuantityOfClientOrders(string clientId);
        double SumedCostOfOrdes();
        double SumCostOfClientsOrders(string clientId);
        IEnumerable ListOfOrders();
        IEnumerable ListOfClientsOrders(string clientId);
        double AverageCostOfOrfder();
        double AvreageCostOfClientsOrder(string clientId);
        IEnumerable OrdersBetwenCost(double lowerBand, double upperBand);
        IEnumerable QuantityOfOrdersByName();
        IEnumerable QuantiOfCleintsOrdersByName(string clientId);
    }
}