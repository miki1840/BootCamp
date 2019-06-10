using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BootCamp.Interfaces;
using System.Globalization;

namespace BootCamp.Classes
{
    class CSVParser : ILoadable
    {
        public IEnumerable load(string path)
        {
            
            using (StreamReader reader = new StreamReader(path))
            {

                string line=reader.ReadLine(); //skipping names 
                List<DataBaseRow> rows=new List<DataBaseRow>();
                List<int> errorList=new List<int>();
                int rowCounter = 1;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] paramsToConvert = line.Split(',');
                    if (paramsToConvert[0].Length > 6 || paramsToConvert[0].Contains(" "))
                    {
                        errorList.Add(rowCounter);
                        rowCounter++;
                        continue;
                    }

                    if (paramsToConvert[3].Length > 255)
                    {
                        errorList.Add(rowCounter);
                        rowCounter++;
                        continue;
                    }

                    if (!long.TryParse(paramsToConvert[1], NumberStyles.Integer, CultureInfo.CreateSpecificCulture("en-US"), out long requestId)||
                        !int.TryParse(paramsToConvert[3], NumberStyles.Integer, CultureInfo.CreateSpecificCulture("en-US"), out int quantity) ||
                        !double.TryParse(paramsToConvert[4], NumberStyles.Float, CultureInfo.CreateSpecificCulture("en-US"), out double price))
                    {
                        errorList.Add(rowCounter);
                        rowCounter++;
                        continue;
                    }
                    rows.Add(new DataBaseRow(paramsToConvert[0], requestId, paramsToConvert[2], quantity, price));
                    rowCounter++;

                }

                if (errorList.Count > 0)
                {
                    StringBuilder errorMesage = new StringBuilder();
                    errorMesage.Append("Nie udało się wczytać poniższych wierszy w pliku: " + path + "\n");
                    foreach (var error in errorList)
                    {
                        errorMesage.Append(error + "\n");
                    }

                    MessageBox.Show(errorMesage.ToString());
                }

                MessageBox.Show("Plik " + path + " został wczytany");
                return rows;
            }
            
        }
    }
}
