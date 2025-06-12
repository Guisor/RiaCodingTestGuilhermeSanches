using Ria.CodingTest.Api.Model;

namespace Ria.CodingTest.Api.Acessor
{
    public interface ICostumerAcessor
    {
        public Task<IEnumerable<Customer>> GetAllCostumers();

        public Task InsertCustomer(IEnumerable<Customer> customers);

        public Task<Customer?> GetCustomer(int id);
    }
}
