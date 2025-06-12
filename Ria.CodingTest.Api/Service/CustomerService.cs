using Ria.CodingTest.Api.Acessor;
using Ria.CodingTest.Api.Model;
using Ria.CodingTest.Api.Validator;

namespace Ria.CodingTest.Api.Service
{
    public class CustomerService : ICustomerService
    {
        private readonly ICostumerAcessor _customerAcessor;
        private IAddCustomerValidator _addCustomerValidator;

        public CustomerService()
        {
            this._customerAcessor = new CustomerAcessor();            
        }

        public async Task<IEnumerable<Customer>> GetAllCostumers()
        {
            return await _customerAcessor.GetAllCostumers();
        }

        public async Task InsertCostumers(IEnumerable<Customer> customer)
        {
            _addCustomerValidator = new AddCustomerValidator(customer);

            if (await this._addCustomerValidator.IsValid())
            {
                await _customerAcessor.InsertCustomer(customer);
            }

        }
    }
}
