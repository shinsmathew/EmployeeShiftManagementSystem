using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeShiftManagementSystem.Core.Entities
{
    public class Shift : BaseEntity
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool IsDeleted { get; set; } 
        public int EmployeeId { get; set; }
        public virtual Employee Employee { get; set; } = null!;
    }
}
