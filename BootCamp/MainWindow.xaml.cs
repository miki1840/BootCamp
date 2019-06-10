using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BootCamp.Classes;
using BootCamp.Enums;
using BootCamp.Interfaces;
using BootCamp.Windows;

namespace BootCamp
{ 
    public partial class MainWindow : Window
    {
        private IRaportGenerator _raportGenerator;
        private List<DataBaseRow> _dataBase;

       
        private void rpinit()
        {
            Random rnd = new Random();
            _dataBase = new List<DataBaseRow>();
            _raportGenerator = new RaportGenerator(_dataBase);
        }
        private string[] _menuList =
        {
            "Ilość zamówień",
            "Ilość zamówień klienta o wskazanym identyfikatorze",
            "Łączna kwota zamówień",
            "Łączna kowta zamówień klienta o wskazanym identyfikatorze",
            "Lista wszystkich zamówień",
            "Lista zamówień dla klienta o wskazanym identyfikatorze",
            "Średnia wartość zamówienia",
            "Średnia wartość zamówienia dla klienta o wskazanym identyfikatorze",
            "Ilość zamówień pogrupowanych po nazwie",
            "Ilość zamówień pogrupowanych po nazwie dla klienta o wskazanym identyfikatorze",
            "Zamówienia w podanym przedziale cenowym"
        };
        public MainWindow()
        {
            InitializeComponent();
            ScoreLable.Content = "";
            foreach (var s in _menuList)
            {
                MenuComboBox.Items.Add(s);
            }
            HideFields();
            rpinit();
        }

        private void HideFields()
        {
            Lable1.Content = "";
            Lable2.Content = "";
            TextBox1.Text = "";
            TextBox2.Text = "";
            Lable1.Visibility = Visibility.Hidden;
            Lable2.Visibility = Visibility.Hidden;
            TextBox1.Visibility = Visibility.Hidden;
            TextBox2.Visibility = Visibility.Hidden;
        }

        private void LoadOnDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] drpoedFiles = (string[])e.Data.GetData((DataFormats.FileDrop));
                ILoadable loader;
                foreach (var file in drpoedFiles)
                {
                    if (file.EndsWith(".xml")) loader = new XMLParser();
                    else if (file.EndsWith(".json")) loader = new JASONParser();
                    else if (file.EndsWith(".csv")) loader = new CSVParser();
                    else
                    {
                        MessageBox.Show(file + " to niepoprawny plik");
                        break;
                    }

                    var loadingTask = Task<IEnumerable<DataBaseRow>>.Run(() =>
                    {
                        return (IEnumerable<DataBaseRow>)loader.load(file);
                    });
                    if (loadingTask.Result != null)
                    {
                        foreach (var dataBaseRow in loadingTask.Result)
                        {
                            if (dataBaseRow != null) _dataBase.Add(dataBaseRow);
                        }
                    }
                    
                }
            }
        }

        private void MenuComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EMainMenu chose = 0;
            chose+= MenuComboBox.SelectedIndex;
            switch (chose)
            {
                case EMainMenu.AvreageCostOfClientOrder:
                    HideFields();
                    Lable1.Visibility = Visibility.Visible;
                    Lable1.Content = "Identyfikator klienta: ";
                    TextBox1.Text = "";
                    TextBox1.Visibility = Visibility.Visible;
                    break;
                case EMainMenu.AvreageCostOfOrder:
                    HideFields();
                    break;
                case EMainMenu.ListOfClientsOrders:
                    HideFields();
                    Lable1.Visibility = Visibility.Visible;
                    Lable1.Content = "Identyfikator klienta: ";
                    TextBox1.Visibility = Visibility.Visible;
                    break;
                case EMainMenu.ListOfOrders:
                    HideFields();
                    break;
                case EMainMenu.OrdersBetwenCost:
                    Lable1.Visibility = Visibility.Visible;
                    Lable1.Content = "Minimalna wartość:";
                    TextBox1.Visibility = Visibility.Visible;
                    Lable2.Visibility = Visibility.Visible;
                    Lable2.Content = "Maksymalna Wartość";
                    TextBox2.Visibility = Visibility.Visible;
                    break;
                case EMainMenu.SumedCostOfOrders:
                    HideFields();
                    break;
                case EMainMenu.SumedCostOfClientOrders:
                    HideFields();
                    Lable1.Visibility = Visibility.Visible;
                    Lable1.Content = "Identyfikator klienta: ";
                    TextBox1.Visibility = Visibility.Visible;
                    break;
                case EMainMenu.QuantityOfOrders:
                    HideFields();
                    break;
                case EMainMenu.QuantityOdClientOrders:
                    HideFields();
                    Lable1.Visibility = Visibility.Visible;
                    Lable1.Content = "Identyfikator klienta: ";
                    TextBox1.Visibility = Visibility.Visible;
                    break;
                case EMainMenu.QuantityOfOrdersByName:
                    HideFields();
                    break;
                case EMainMenu.QuantityOfClientsOrdersByName:
                    HideFields();
                    Lable1.Visibility = Visibility.Visible;
                    Lable1.Content = "Identyfikator klienta: ";
                    TextBox1.Visibility = Visibility.Visible;
                    break;
            }
        }

        private void RaportButton_Click(object sender, RoutedEventArgs e)
        {
            if (_dataBase.Count > 0)
            {
                EMainMenu chose = 0;
                chose += MenuComboBox.SelectedIndex;
                ScoreLable.Content = "";
                ProductRaportWindow productRaportWindow;
                OrdersRaportWindow ordersRaportWindow;
                switch (chose)
                {
                    case EMainMenu.QuantityOfOrdersByName:
                        var resultQuantityOfOrdersByName = Task<List<ProductRow>>.Run(() =>
                        {
                            return _raportGenerator.QuantityOfOrdersByName();
                        });
                        productRaportWindow = new ProductRaportWindow();
                        productRaportWindow.Init(resultQuantityOfOrdersByName.Result,
                            "Ilość zamówień pogrupowanych po nazwie");
                        productRaportWindow.Show();
                        break;

                    case EMainMenu.ListOfOrders:
                        var resultListOfOrders =
                            Task<List<Order>>.Run(() => { return _raportGenerator.ListOfOrders(); });
                        ordersRaportWindow = new OrdersRaportWindow();
                        ordersRaportWindow.Init(resultListOfOrders.Result, "Łączna kwota zamówień");
                        ordersRaportWindow.Show();
                        break;

                    case EMainMenu.AvreageCostOfClientOrder:
                        if (TextBox1.Text.Length > 0)
                        {
                            string clientId = TextBox1.Text; //być może warunki 
                            var resultAvreageCostOfClientOrtder = Task<double>.Run(() =>
                            {
                                return _raportGenerator.AvreageCostOfClientsOrder(clientId);
                            });
                            ScoreLable.Content = "Średnia wartość zamówień klienta o identyfikatorze: " + clientId +
                                                 " to: " + Math.Round(resultAvreageCostOfClientOrtder.Result, 2);
                        }

                        break;

                    case EMainMenu.AvreageCostOfOrder:
                        var resultAvreageCostOfOrdre = Task<double>.Run(() =>
                        {
                            return _raportGenerator.AverageCostOfOrfder();
                        });
                        ScoreLable.Content = "Średnia wartość zamówienia wynosi: " +
                                             Math.Round(resultAvreageCostOfOrdre.Result, 2);
                        break;

                    case EMainMenu.ListOfClientsOrders:
                        if (TextBox1.Text.Length > 0)
                        {
                            string clientId = TextBox1.Text; //być może warunki 
                            var resultListOfClientsorders = Task<List<Order>>.Run(() =>
                            {
                                return _raportGenerator.ListOfClientsOrders(clientId);
                            });
                            ordersRaportWindow = new OrdersRaportWindow();
                            ordersRaportWindow.Init(resultListOfClientsorders.Result,
                                "Lista zamówień dla klienta o identyfikatorze " + clientId);
                            ordersRaportWindow.Show();
                        }

                        break;

                    case EMainMenu.QuantityOfClientsOrdersByName:
                        if (TextBox1.Text.Length > 0)
                        {
                            string clientId = TextBox1.Text; //być może warunki 
                            var resultQuantityOfClientsOrdersByName = Task<List<ProductRow>>.Run(() =>
                            {
                                return _raportGenerator.QuantiOfCleintsOrdersByName(clientId);
                            });
                            productRaportWindow=new ProductRaportWindow();
                            productRaportWindow.Init(resultQuantityOfClientsOrdersByName.Result,"Lista produktów zakupionych przez klienta o identyfikatorze: "+clientId);
                            productRaportWindow.Show();
                        }

                        break;

                    case EMainMenu.OrdersBetwenCost:
                        double lowerBound, upperBound;
                        if (double.TryParse(TextBox1.Text, out lowerBound) &&
                            double.TryParse(TextBox2.Text, out upperBound))
                        {
                            var resultOrdersBetweanCost = Task<List<Order>>.Run(() =>
                            {
                                return _raportGenerator.OrdersBetwenCost(lowerBound, upperBound);
                            });
                            ordersRaportWindow = new OrdersRaportWindow();
                            ordersRaportWindow.Init(resultOrdersBetweanCost.Result,
                                "Zamówienia na kwotę od " + lowerBound + " do " + upperBound);
                            ordersRaportWindow.Show();
                        }

                        break;

                    case EMainMenu.SumedCostOfOrders:
                        var resultSumedCostOfOrders =
                            Task<double>.Run(() => { return _raportGenerator.SumedCostOfOrdes(); });
                        ScoreLable.Content = "Łączna suma zamówień wynosi: " + Math.Round(resultSumedCostOfOrders.Result, 2);
                        break;

                    case EMainMenu.SumedCostOfClientOrders:
                        if (TextBox1.Text.Length > 0)
                        {
                            string clientId = TextBox1.Text; //być może warunki 
                            var resultSumedCostOfClientOrders = Task<double>.Run(() =>
                            {
                                return _raportGenerator.SumCostOfClientsOrders(clientId);
                            });
                            ScoreLable.Content = "Łączny koszt zamówień klienta o identyfikatorze " + clientId +
                                                 " to " +
                                                 Math.Round((double)resultSumedCostOfClientOrders.Result, 2);
                        }

                        break;
                    case EMainMenu.QuantityOdClientOrders:
                        if (TextBox1.Text.Length > 0)
                        {
                            string clientId = TextBox1.Text; //być może warunk
                            var resultQuantityOfClientOrders = Task<int>.Run(() =>
                            {
                                return _raportGenerator.QuantityOfClientOrders(clientId);
                            });
                            ScoreLable.Content = "Ilość zamówień klienta o identyfikatorze " + clientId + " to " +
                                                 resultQuantityOfClientOrders.Result;
                        }

                        break;

                    case EMainMenu.QuantityOfOrders:
                        var resultQuantityOfOrders =
                            Task<int>.Run(() => { return _raportGenerator.QuantityOfOrders(); });
                        ScoreLable.Content = "Ilość wszystkich zamówień to " + resultQuantityOfOrders.Result;
                        break;
                }
            }
            else MessageBox.Show("Baza danych jest pusta.");
        }
    }
}
