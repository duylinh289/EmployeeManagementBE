namespace EmployeeManagementBE.Data
{
    public class Students_RankDTO
    {
        public Guid? StudentCode { get; set; }
        public string? StudentName { get; set; } = string.Empty;
        public string? CreateBy { get; set; } = string.Empty;
        public DateTime? CreatedOn { get; set; }
        public string? ModifiedBy { get; set; } = string.Empty;
        public DateTime? ModifiedOn { get; set; }
        public string? ClassName { get; set; }
        public double? AvgScore { get; set; }
        public string? Ranked { get; set; }

    }
}
