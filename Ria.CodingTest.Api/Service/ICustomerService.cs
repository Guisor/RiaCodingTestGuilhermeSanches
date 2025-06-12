using Ria.CodingTest.Api.Model;

namespace Ria.CodingTest.Api.Service
{
    public interface ICustomerService
    {
        public Task<IEnumerable<Customer>> GetAllCostumers();
        public Task InsertCostumers(IEnumerable<Customer> customer);
    }
}
