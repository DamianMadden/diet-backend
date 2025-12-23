using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Xml.Linq;
using NpgsqlTypes;

namespace draft_ml.Db.Models
{
    public class IngestionRecord
    {
        public required Guid Id { get; set; }
        public required string Source { get; set; }
        public required DateTimeOffset Timestamp { get; set; }
        public required ulong Count { get; set; }
    }
}
