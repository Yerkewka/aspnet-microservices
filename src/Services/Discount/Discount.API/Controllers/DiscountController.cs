using Discount.API.Entities;
using Discount.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Discount.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class DiscountController : ControllerBase
    {
        private readonly IDiscountRepository _discountRepository;

        public DiscountController(IDiscountRepository discountRepository)
        {
            _discountRepository = discountRepository;
        }

        [HttpGet("{productName}", Name = nameof(GetDiscount))]
        [ProducesResponseType(typeof(Coupon), (int) HttpStatusCode.OK)]
        public async Task<ActionResult<Coupon>> GetDiscount(string productName)
        {
            return Ok(await _discountRepository.GetDiscount(productName));
        }

        [HttpPost]
        [ProducesResponseType(typeof(bool), (int) HttpStatusCode.Created)]
        public async Task<ActionResult<Coupon>> CreateDiscount(Coupon coupon)
        {
            await _discountRepository.CreateDiscount(coupon);
            return CreatedAtRoute(nameof(GetDiscount), new { ProductName = coupon.ProductName }, coupon);
        }

        [HttpPut]
        [ProducesResponseType(typeof(bool), (int) HttpStatusCode.OK)]
        public async Task<ActionResult<Coupon>> UpdateDiscount(Coupon coupon)
        {            
            return Ok(await _discountRepository.UpdateDiscount(coupon));
        }

        [HttpDelete("{productName}", Name = nameof(DeleteDiscount))]
        [ProducesResponseType(typeof(bool), (int) HttpStatusCode.OK)]
        public async Task<ActionResult<Coupon>> DeleteDiscount(string productName)
        {            
            return Ok(await _discountRepository.DeleteDiscount(productName));
        }
    }
}
