using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTrack.Shared.Auth;

namespace TechTrack.Shared.Events
{
    public class CriticalIssueDetectedEvent
    {
        public Guid EquipmentId { get; set; }
        public string EquipmentName { get; set; }
        public Guid? ResponsibleUserId { get; set; }
        public Guid? ManagerId { get; set; }
        public Guid? DepartmentHeadId { get; set; }
        public string IssueName { get; set; }
        public string IssueDescription { get; set; }
    }
}
