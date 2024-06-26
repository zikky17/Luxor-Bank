﻿using BankApp.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using ServiceLibrary.Data;
using ServiceLibrary.Services;
using ServiceLibrary.ViewModels;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace BankAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Account : ControllerBase
    {
        private readonly ApplicationDbContext _context = DatabaseService.GetDbContext();

        // GET TRANSACTIONS ///////////////////////////////////////////////////////
        /// <summary>
        /// Get one or many transactions from the database
        /// </summary>
        /// <remarks>
        /// LIMIT: Choose how many transactions you want to get<br/>
        /// OFFSET: Choose how many transactions you want to skip
        /// </remarks>
        /// <returns>
        /// One customers transactions
        /// </returns>
        /// <response code="200">
        /// Successfully returned transactions for your customer
        /// </response>


        [HttpGet]
        [Route("{id}/{limit}/{offset}")]
        public async Task<ActionResult<AccountViewModel>> GetOne(int id, int limit, int offset)
        {
            var transactions = await _context.Dispositions
                .Include(d => d.Account)
                .ThenInclude(a => a.Transactions)
                .Where(d => d.AccountId == id)
                .SelectMany(d => d.Account.Transactions)
                .Take(limit)
                .Skip(offset)
                .Select(t => new TransactionViewModel
                {
                    AccountId = t.AccountId,
                    Date = t.Date,
                    Operation = t.Operation,
                    Type = t.Type,
                    Amount = t.Amount
                })
                .ToListAsync();

            if (transactions == null || transactions.Count == 0)
            {
                return BadRequest("Account not found or no transactions available");
            }

            return Ok(transactions);
        }

    }
}

