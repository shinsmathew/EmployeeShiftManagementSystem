

namespace EmployeeShiftManagementSystem.Application.DTOs.Shift
{
    public class ShiftCreateDto
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int EmployeeId { get; set; }
    }
}
