using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechTrack.Shared.Events
{
    public record EquipmentAddedEvent
    {
        public Guid EquipmentId { get; init; }
        public Guid DepartmentId { get; init; }
        public Guid CompanyId { get; init; }
        public string Name { get; init; }
        public Guid? ResponsibleUserId { get; init; }
    }
}
