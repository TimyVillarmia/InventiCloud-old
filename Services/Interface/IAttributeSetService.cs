using InventiCloud.Models;

namespace InventiCloud.Services.Interface
{
    public interface IAttributeSetService
    {
        Task AddAttributeSet(AttributeSet attributesets);
        Task DeleteAttributeSet(AttributeSet attributesets);

        Task<IEnumerable<AttributeSet>> GetAllAttributeSetAsync();

        Task DisposeAsync();
    }
}
