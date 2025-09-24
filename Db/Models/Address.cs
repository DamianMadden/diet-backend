using NpgsqlTypes;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Xml.Linq;

namespace draft_ml.Db.Models
{
    public class Address
    {
        public enum AddressType
        {
            Both = 0,
            Billing = 1,
            Shipping = 2
        }

        public required Guid Id { get; set; }
        public required Guid UserId { get; set; }
        public AddressType Type { get; set; }

        public string Street1 { get; set; }
        public string Street2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
    }
}
