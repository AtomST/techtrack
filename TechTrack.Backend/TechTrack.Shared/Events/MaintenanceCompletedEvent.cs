using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechTrack.Shared.Events
{
    public class MaintenanceCompletedEvent
    {
        public Guid MaintenanceId { get; set; }
        public Guid EquipmentId { get; set; }
        public string EquipmentName { get; set; }
        public string MaintenanceName { get; set; }
        public string MaintenanceTypeName { get; set; }
        public DateTime CompletedAt { get; set; }
        public Guid? ResponsibleUserId { get; set; }
        public Guid CompletedByUserId { get; set; }
        public Guid? ScheduledByUserId { get; set; }
    }
}
