using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechTrack.Shared.Events
{
    public class MaintenanceReminderEvent
    {
        public Guid MaintenanceId { get; set; }
        public Guid EquipmentId { get; set; }
        public string EquipmentName { get; set; }
        public string MaintenanceName { get; set; }
        public DateTime ScheduledDate { get; set; }
        public Guid ResponsibleUserId { get; set; }
    }
}
