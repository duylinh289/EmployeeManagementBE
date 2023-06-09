using Microsoft.CodeAnalysis.Completion;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementBE.DTO.SignalR
{
    public class Message
    {
        [Required]
        public string From { get; set; }
        public string To { get; set; }
        [Required]
        public string Content { get; set; }
        public string Time { get; set; } = DateTime.Now.ToString("HH:mm:ss tt");
    }
}
