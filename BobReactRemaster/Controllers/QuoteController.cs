using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BobReactRemaster.Auth;
using BobReactRemaster.Data;
using BobReactRemaster.Data.Models.Stream;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace BobReactRemaster.Controllers
{
    [ApiController]
    [Route("Quotes")]
    public class QuoteController : Controller
    {
        private readonly ApplicationDbContext _context;
        public QuoteController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("GetStreamQuotes")]
        [Authorize(Policy = Policies.User)]
        public IActionResult GetStreamQuotes(int StreamID)
        {

            var Quotes = _context.Quotes.AsQueryable().Where(x => x.LiveStreamID == StreamID);
            if (Quotes.Any())
            {
                List<dynamic> data = new List<dynamic>();
                foreach (Quote quote in Quotes)
                {
                    data.Add(new { ID = quote.Id, Date = quote.Created.ToString("dd.MM.yyyy HH:mm"), Content = quote.Text });
                }
                return Ok(data);
            }
            return NotFound();
        }
        [HttpGet]
        [Route("DeleteQuote")]
        [Authorize(Policy = Policies.User)]
        public IActionResult DeleteQuote(int QuoteID)
        {

            var Quote = _context.Quotes.AsQueryable().First(x => x.Id == QuoteID);
            if (Quote != null)
            {
                _context.Quotes.Remove(Quote);
                _context.SaveChanges();
                return Ok();
            }
            return NotFound();
        }
    }
}
