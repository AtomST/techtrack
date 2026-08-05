using System.ComponentModel.DataAnnotations;

namespace TechTrack.OrganizationService.UserProjections.Entities
{
    public class UserProjection
    {
        [Key]
        public Guid UserId { get; set; }
        public string FullName { get; set; }
    }
}
