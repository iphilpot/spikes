namespace Aggregator.Functions.Entities
{
    using System.Threading.Tasks;
    using Aggregator.Events;

    public interface IStoreEntity
    {
        Task<Item> GetItem(string itemId);
        void Aggregate(InventoryEvent inventoryEvent);
    }
}