using Chat.Domain.Entities;

namespace Chat.Domain.Shared.Models;

public class PaginatorResponse<TEntity>(IEnumerable<TEntity> collection, MetaResponse meta)
    where TEntity : BaseEntity
{
    public IEnumerable<TEntity> Collection { get; set; } = collection;
    public MetaResponse Meta { get; set; } = meta;
}
