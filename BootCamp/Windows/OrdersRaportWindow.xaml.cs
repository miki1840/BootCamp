using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BootCamp.Classes;

namespace BootCamp.Windows
{

    public partial class OrdersRaportWindow : Window
    {
        private ObservableCollection<Order> _data;
        public OrdersRaportWindow()
        {
            InitializeComponent();
           
        }

        public void Init(IEnumerable data, string title)
        {
            TitleLable.Content = title;
            var tempList = ((IEnumerable<Order>)data).ToList();
            _data = new ObservableCollection<Order>();
            foreach (var order in tempList)
            {
                _data.Add(order);
            }

            RaportDataGrid.ItemsSource = _data;
        }
    }
}
