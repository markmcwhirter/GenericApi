namespace GenericApi.Repository;

using GenericApi.Models;

public interface ICustomerRepository
{
    Task<List<Customer>> GetAllAsync();

}
