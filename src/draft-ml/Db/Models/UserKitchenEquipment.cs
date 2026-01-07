namespace draft_ml.Db.Models
{
    public class UserKitchenEquipment
    {
        public required Guid UserId { get; set; }

        public required Guid KitchenEquipmentId { get; set; }
    }
}
