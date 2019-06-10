using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using BootCamp.Classes;
using System.Globalization;

namespace BootCamp.SupportingClasses
{
    [XmlRoot(ElementName = "request")]
    public class Request
    {
        [XmlElement(ElementName = "clientId")]
        public string clientId { get; set; }
        [XmlElement(ElementName = "requestId")]
        public string requestId { get; set; }
        [XmlElement(ElementName = "name")]
        public string name { get; set; }
        [XmlElement(ElementName = "quantity")]
        public string quantity { get; set; }
        [XmlElement(ElementName = "price")]
        public string price { get; set; }

        public DataBaseRow toDataBaseRow()
        {

            if (clientId == null || requestId == null || name == null || quantity == null || price == null)
                return null;
            if (clientId.Equals("") || name.Equals("")) return null;
            if (clientId.Length > 6 || clientId.Contains(" ")) return null;
            if (name.Length > 255) return null;
            if (!double.TryParse(price, NumberStyles.Float, CultureInfo.CreateSpecificCulture("en-US"), out double priceAsDouble) ||
                !long.TryParse(requestId, NumberStyles.Integer, CultureInfo.CreateSpecificCulture("en-US"), out long requestIdAsLong) ||
                !int.TryParse(quantity, NumberStyles.Integer, CultureInfo.CreateSpecificCulture("en-US"), out int quantityAsInt)) return null;
            return new DataBaseRow(clientId,requestIdAsLong, name, quantityAsInt, priceAsDouble);
        }
    }
}
