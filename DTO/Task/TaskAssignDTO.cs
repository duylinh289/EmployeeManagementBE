namespace EmployeeManagementBE.DTO.Task
{
    public class TaskAssignDTO
    {
        public int Id { get; set; }
        public string TaskName { get; set; } = string.Empty;
        public string TaskDescription { get; set; } = string.Empty;
        public Guid Assignee { get; set; }
        public string? AssigneeName { get; set; } = string.Empty;
        public string Reporter { get; set; } = string.Empty;
    }
}
