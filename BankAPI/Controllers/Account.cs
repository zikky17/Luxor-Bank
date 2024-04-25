using BankApp.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using ServiceLibrary.Data;
using ServiceLibrary.Services;

namespace BankAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Account : ControllerBase
    {
        private readonly ApplicationDbContext _context = DatabaseService.GetDbContext();

        [HttpGet]
        [Route("{id}/{limit}/{offset}")]
        public async Task<ActionResult<AccountViewModel>> GetOne(int id, int limit, int offset)
        {
            var transactions = _context.Dispositions
            .Include(d => d.Account)
                .Where(d => d.AccountId == id)
                .Select(d => new AccountViewModel
                {
                    AccountId = d.AccountId,
                    Created = d.Account.Created,
                    Balance = d.Account.Balance,
                    Transactions = d.Account.Transactions.ToList()
                })
                .Skip(limit)
                .Take(offset);

            if (transactions == null)
            {
                return BadRequest("Account not found");
            }
            return Ok(transactions);
        }
    }
}
