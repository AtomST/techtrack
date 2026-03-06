using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechTrack.Shared.Events
{
    public class IssueRegistred
    {
        public Guid EquipmentId { get; set; }
        public int StatusId { get; set; }
    }
}
