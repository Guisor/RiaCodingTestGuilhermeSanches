using Ria.CodingTest.Api.Model;
using System.Text.Json;

namespace Ria.CodingTest.Api.Acessor
{
    public class CustomerAcessor : ICostumerAcessor
    {
        private string DBPath = @"DB\customers.json";
        public async Task<IEnumerable<Customer>> GetAllCostumers()
        {
            var customers = await GeAllCustomersFromDb();

            return customers ?? new List<Customer>();
        }

        public async Task<Customer?> GetCustomer(int id)
        {
            var customers = await GeAllCustomersFromDb();

            return customers.Find(c => c.Id == id);
        }

        public async Task InsertCustomer(IEnumerable<Customer> customers)
        {
            var customersSortedToInsert = await InsertCustomerByLastName(customers);

            string updatedJson = JsonSerializer.Serialize(customersSortedToInsert, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            await File.WriteAllTextAsync(DBPath, updatedJson);
        }

        private async Task<IEnumerable<Customer>> InsertCustomerByLastName(IEnumerable<Customer> customers)
        {
            List<Customer> existingCustomers = await GeAllCustomersFromDb();            

            foreach (var newCustomer in customers)
            {
                bool inserted = false;

                for (int i = 0; i < existingCustomers.Count; i++)
                {
                    // Comparação alfabética dos sobrenomes
                    if (string.Compare(newCustomer.LastName, existingCustomers[i].LastName, StringComparison.OrdinalIgnoreCase) < 0)
                    {
                        existingCustomers.Insert(i, newCustomer);
                        inserted = true;
                        break;
                    }
                }

                // Se não inseriu em nenhum lugar, adiciona no final
                if (!inserted)
                {
                    existingCustomers.Add(newCustomer);
                }
            }

            return existingCustomers;
        }

        private async Task<List<Customer>> GeAllCustomersFromDb()
        {
            if (File.Exists(DBPath))
            {
                string existingJson = await File.ReadAllTextAsync(DBPath);
                var loaded = JsonSerializer.Deserialize<List<Customer>>(existingJson);
                if (loaded != null)
                    return loaded;
            }

            return new List<Customer> { };
        }
    }
}
