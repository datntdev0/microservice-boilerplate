﻿namespace datntdev.Microservices.Common.Models
{
    public class FullAuditEntity<TKey> : BaseAuditEntity<TKey>, IDeleteEntity where TKey : IEquatable<TKey>
    {
        public bool IsDeleted { get; set; }
    }

    public class BaseAuditEntity<TKey> : IAuditCreatedEntity, IAuditUpdatedEntity where TKey : IEquatable<TKey>
    {
        public TKey Id { get; set; } = default!;
        public DateTime? CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public string ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();
    }
}