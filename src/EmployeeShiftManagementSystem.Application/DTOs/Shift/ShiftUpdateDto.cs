

namespace EmployeeShiftManagementSystem.Application.DTOs.Shift
{
    public class ShiftUpdateDto
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int EmployeeId { get; set; }
    }
}
