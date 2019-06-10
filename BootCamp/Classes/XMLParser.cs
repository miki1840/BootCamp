using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Media.Media3D;
using System.Xml;
using System.Xml.Serialization;
using BootCamp.Interfaces;
using BootCamp.SupportingClasses;

namespace BootCamp.Classes
{
    class XMLParser : ILoadable
    {
        private List<DataBaseRow> _loadingData= new List<DataBaseRow>();

        public IEnumerable load(string path)
        {
            StringBuilder builder = new StringBuilder();
            using (StreamReader reader = new StreamReader(path))
            {

                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    builder.Append(line);
                }
            }
            
                XmlSerializer serializer = new XmlSerializer(typeof(RootObjectForRequest), new XmlRootAttribute("requests"));
                StringReader stringReader = new StringReader(builder.ToString());
                RootObjectForRequest deserializedObjects = (RootObjectForRequest)serializer.Deserialize(stringReader);
                List<int> errorList = new List<int>();
            try
            {
                var toReturn = deserializedObjects.ConvertToDataBaseRows(out errorList);
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
                return toReturn;
            }
            catch(Exception e)
            {
                MessageBox.Show("Dane z pliku " + path + " nie zostały wczytane. Błąd:\n" + e.Message);
                return null;
            }
            
        }
    }
}
