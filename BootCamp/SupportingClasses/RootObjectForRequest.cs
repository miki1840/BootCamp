using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using BootCamp.Classes;

namespace BootCamp.SupportingClasses
{
    [XmlRoot(ElementName = "requests")]
    public class RootObjectForRequest
    {
        [XmlElement(ElementName = "request")]
        public List<Request> requests { get; set; }

        public IEnumerable<DataBaseRow> ConvertToDataBaseRows(out List<int> errorList)
        {
            errorList = new List<int>();
            List<DataBaseRow> rows = new List<DataBaseRow>();
            DataBaseRow curr;
            for (int i = 0; i < requests.Count; i++)
            {
                curr = requests[i].toDataBaseRow();
                if (curr != null) rows.Add(curr);
                else errorList.Add(i + 1);
            }

            return rows;
        }
    }
}
