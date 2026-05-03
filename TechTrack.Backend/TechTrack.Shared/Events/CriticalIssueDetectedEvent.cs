using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechTrack.Shared.Events
{
    public class CriticalIssueDetectedEvent
    {
        public Guid EquipmentId { get; set; }
        public string EquipmentName { get; set; }
        public Guid ResponsibleUserId { get; set; }
        public string IssueName { get; set; }
        public string IssueDescription { get; set; }
    }
}
