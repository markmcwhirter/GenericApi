namespace GenericApi.Repository;


public class CustomerRepository : ICustomerRepository
{
    public readonly IDbContextFactory<NorthwindContext> _contextFactory;

    public CustomerRepository(IDbContextFactory<NorthwindContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }
    public async Task<List<Customer>> GetAllAsync()
    {
        using var dbContext = await _contextFactory.CreateDbContextAsync();
        return await dbContext.Customers.ToListAsync();
    }
}
