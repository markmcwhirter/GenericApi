//namespace GenericApi.Features.Customers.GetAllCustomers;


//public class CustomerMapper : Mapper<GetAllCustomersRequest, GetAllCustomersResponse, Customer>
//{
//    //public override Customer ToEntity(Request r) => new()
//    //{
//    //    Id = r.Id,
//    //    DateOfBirth = DateOnly.Parse(r.BirthDay),
//    //    FullName = $"{r.FirstName} {r.LastName}"
//    //};

//    public  GetAllCustomersResponse FromEntity(List<Customer> customerList)
//    {
//        var response = new GetAllCustomersResponse();
//        response.Customers.AddRange(from e in customerList
//                                    select new Customer
//                                    {
//                                        CompanyName = e.CompanyName,
//                                        CustomerId = e.CustomerId,
//                                        ContactName = e.ContactName,
//                                        ContactTitle = e.ContactTitle,
//                                        Address = e.Address,
//                                        City = e.City,
//                                        Region = e.Region,
//                                        PostalCode = e.PostalCode,
//                                        Country = e.Country,
//                                        Phone = e.Phone,
//                                        Fax = e.Fax
//                                    });
//        return response;
//    }

//}