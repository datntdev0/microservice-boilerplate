namespace datntdev.Microservices.Common.Models
{
    public class BaseAuditEntity<TKey> : IAuditEntity where TKey : IEquatable<TKey>
    {
        public TKey Id { get; set; } = default!;
        public bool IsDeleted { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
