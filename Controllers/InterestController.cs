using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewsAggregator.Data;
using NewsAggregator.Dtos;
using NewsAggregator.Models;
using System.Security.Claims;

namespace NewsAggregator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class InterestController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public InterestController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Interest([FromBody] InterestDto interest)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Unauthorized();
            }
            if (interest == null)
            {
                return BadRequest("InterestDto cannot be null.");
            }
            var userInterest = await _context.Interests.FirstOrDefaultAsync(x => x.UserId == int.Parse(userId));
            if (userInterest == null)
            {
                userInterest = new Interests
                {
                    InterestName = interest.InterestName,
                    UserId = int.Parse(userId)
                };
                await _context.Interests.AddAsync(userInterest);
            }
            else
            {
                userInterest.InterestName = interest.InterestName;
            }
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
