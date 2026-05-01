using System.ComponentModel.DataAnnotations;

namespace TechTrack.NotificationService.Projections.Entities
{
    public class UserContact
    {
        [Key]
        public Guid UserId { get; set; }
        public string Email { get; set; }
    }
}
