using System.ComponentModel.DataAnnotations.Schema;

namespace draft_ml.Db.Models
{
    public class UserTagExclusion
    {
        public required Guid UserId { get; set; }
        public required Guid TagId { get; set; }

        [ForeignKey("UserId")]
        public User? User { get; set; }

        [ForeignKey("TagId")]
        public Tag? Tag { get; set; }
    }
}
