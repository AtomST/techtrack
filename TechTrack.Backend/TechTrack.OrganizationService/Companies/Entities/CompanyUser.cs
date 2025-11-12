namespace TechTrack.OrganizationService.Companies.Entities
{
    public class CompanyUser
    {
        public Guid CompanyId { get; set; }
        public Guid UserId { get; set; }
        public DateTime JoinedAt { get; set; }
        
        public Company? Company { get; set; }
    }
}
