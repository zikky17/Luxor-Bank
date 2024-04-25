using BankApp.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceLibrary.Data;
using ServiceLibrary.Interfaces;
using ServiceLibrary.Services;

namespace BankAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeController : ControllerBase
    {
        private readonly ApplicationDbContext _context = DatabaseService.GetDbContext();

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<CustomerViewModel>> GetOne(int id)
        {
            var customer = _context.Customers.Find(id);

            if (customer == null)
            {
                return BadRequest("Customer not found");
            }
            return Ok(customer);
        }

    }
}
