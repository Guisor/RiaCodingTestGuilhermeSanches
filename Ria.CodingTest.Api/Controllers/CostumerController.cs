using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ria.CodingTest.Api.Model;
using Ria.CodingTest.Api.Service;
using System.ComponentModel.DataAnnotations;

namespace Ria.CodingTest.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CostumerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CostumerController()
        {
            _customerService = new CustomerService();
        }

        [HttpGet]
        public async Task<IEnumerable<Customer>> Get()
        {
            return await _customerService.GetAllCostumers();
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] IEnumerable<Customer> request)
        {
            try
            {
                await _customerService.InsertCostumers(request);
                return Ok("Clientes inseridos com sucesso.");
            }
            catch (ValidationException ex)
            {
                return StatusCode(403, $"Validation error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao inserir clientes: {ex.Message}");
            }
        }
    }
}
