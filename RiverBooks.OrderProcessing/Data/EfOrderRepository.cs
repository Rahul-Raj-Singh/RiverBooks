using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace RiverBooks.OrderProcessing.Data;

internal interface IOrderRepository
{
    Task<List<Order>> GetAllOrdersAsync();
    void AddOrder(Order order);
    Task SaveChangesAsync();
}

internal class EfOrderRepository(OrderProcessingDbContext dbContext) : IOrderRepository
{
    public Task<List<Order>> GetAllOrdersAsync()
    {
        return dbContext.Orders
            .Include(x => x.OrderItems)
            .ToListAsync();
    }

    public void AddOrder(Order order)
    {
        dbContext.Orders.Add(order);
    }

    public async Task SaveChangesAsync()
    {
        await dbContext.SaveChangesAsync();
    }
}