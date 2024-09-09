namespace GenericApi.Features.Customers.GetAllCustomers;

public class GetAllCustomers : EndpointWithoutRequest<GetAllCustomersResponse>
{
    private readonly ICustomerRepository _customerRepository;
    public GetAllCustomers(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public override void Configure()
    {
        Post("/api/customer/getall");
        Version(1);
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        List<Customer> customerList = await _customerRepository.GetAllAsync();

        Response.Customers.AddRange(from e in customerList
                                    select new Customer
                                    {
                                        CompanyName = e.CompanyName,
                                        CustomerId = e.CustomerId,
                                        ContactName = e.ContactName,
                                        ContactTitle = e.ContactTitle,
                                        Address = e.Address,
                                        City = e.City,
                                        Region = e.Region,
                                        PostalCode = e.PostalCode,
                                        Country = e.Country,
                                        Phone = e.Phone,
                                        Fax = e.Fax
                                    });

        await SendAsync(Response, cancellation: ct);
    }
}
