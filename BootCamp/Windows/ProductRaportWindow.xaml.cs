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
    /// <summary>
    /// Logika interakcji dla klasy ProductRaportWindow.xaml
    /// </summary>
    public partial class ProductRaportWindow : Window
    {
        private ObservableCollection<ProductRow> _data;
        public ProductRaportWindow()
        {
            InitializeComponent();
            
        }

        public void Init(IEnumerable Data, string title)
        {
            var dataToConvert = ((IEnumerable<ProductRow>)Data).ToList();
            _data = new ObservableCollection<ProductRow>();
            foreach (var row in dataToConvert)
            {
                _data.Add(row);
            }

            TitleLable.Content = title;
            RaportDataGrid.ItemsSource = _data;
        }
    }
}
