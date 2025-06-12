using Ria.CodingTest.Api.Acessor;
using Ria.CodingTest.Api.Model;

namespace Ria.CodingTest.Api.Validator
{
    public class AddCustomerValidator : IAddCustomerValidator
    {
        private IEnumerable<Customer> Request;
        private ICostumerAcessor _customerAcessor;
        public AddCustomerValidator(IEnumerable<Customer> request) 
        { 
            Request = request;
            _customerAcessor = new CustomerAcessor();
        }

        public async Task<bool> IsValid()
        {
            if (Request.Any(a => String.IsNullOrEmpty(a.FirstName) || String.IsNullOrEmpty(a.LastName) || a.Id == 0))
            {
                throw new ValidationException("All fields must be suplied");
            }

            if (Request.Any(a => a.Age < 18))
            {
                throw new ValidationException("The minimum age for a new customer must be 18 years old");
            }

            foreach (var customer in Request)
            {
                var customerResult = await _customerAcessor.GetCustomer(customer.Id);
                if (customerResult != null)
                {
                    throw new ValidationException($"The id {customer.Id} already exists in DB");
                }
            }            

            return true;
        }
    }
}
