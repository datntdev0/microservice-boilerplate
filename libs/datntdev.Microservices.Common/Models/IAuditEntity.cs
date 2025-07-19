namespace datntdev.Microservices.Common.Models
{
    public interface IAuditCreatedEntity
    {
        public DateTime? CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
    }

    public interface IAuditUpdatedEntity
    {
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
    }

    public interface IDeleteEntity
    {
        public bool IsDeleted { get; set; }
    }

    public interface IAuditEntity : IDeleteEntity, IAuditCreatedEntity, IAuditUpdatedEntity
    {
    }
}
