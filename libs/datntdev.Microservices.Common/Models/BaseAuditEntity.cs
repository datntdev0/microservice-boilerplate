namespace datntdev.Microservices.Common.Models
{
    public class BaseAuditEntity : IAuditEntity
    {
        public bool IsDeleted { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
