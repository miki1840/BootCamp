using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BootCamp.Interfaces;
using BootCamp.SupportingClasses;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BootCamp.Classes
{
    class JASONParser : ILoadable
    {
        
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
            JObject jObject= JObject.Parse(builder.ToString());
            try
            {
                var deserializedObjects = JsonConvert.DeserializeObject<RootObjectForRequest>(builder.ToString());
                List<int> listOfErrors;
                var toReturn = deserializedObjects.ConvertToDataBaseRows(out listOfErrors);
                if (listOfErrors.Count > 0)
                {
                    StringBuilder errorMesage = new StringBuilder();
                    errorMesage.Append("Nie udało się wczytać poniższych wierszy w pliku: " + path + "\n");
                    foreach (var error in listOfErrors)
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
