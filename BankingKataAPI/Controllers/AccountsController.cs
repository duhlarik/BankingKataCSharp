using System;
using System.Linq;
using BankingKataAPI.Models;
using BankingKataAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace BankingKataAPI.Controllers
{
    [Route("api/[controller]")]
    public class AccountsController : Controller
    {
        private readonly AccountsService _accountsService;

        public AccountsController(AccountsService accountsService){
            _accountsService = accountsService;
        }

        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            return Ok(_accountsService.FindAccount(id));
        }

        [HttpPost("")]
        public IActionResult Post([FromBody] Money[] monies)
        {
            var accounts = monies.Select(m => _accountsService.OpenNewAccount(m)).ToArray();
            return Ok(accounts);
        }
    }
}
