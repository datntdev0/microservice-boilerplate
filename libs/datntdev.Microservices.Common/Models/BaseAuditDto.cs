namespace datntdev.Microservices.Common.Models
{
    public class BaseAuditDto<TKey> : ICreated, IUpdated where TKey : IEquatable<TKey>
    {
        public TKey Id { get; set; } = default!;
        public DateTime? CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
