using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebMarket.Contracts;
using WebMarket.DataAccess.Models;
using WebMarket.Services;

namespace WebMarket.Controllers
{
    public class ShoppingCartController : MyController<ShoppingCartElement>
    {
        private readonly ShoppingCartService _cartService;
        public ShoppingCartController(ShoppingCartService cartService) : base(cartService)
        {
            _cartService = cartService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddElement([FromBody] ShoppingCartElementInfo elementInfo)
        {
            var id = HttpContext.User.FindFirstValue(ClaimTypes.Actor);
            if (!int.TryParse(id, out int userId))
                return BadRequest("Bad token");
            var element = await _cartService.Create(new ShoppingCartElement()
            {
                UserId = userId,
                ProductId = elementInfo.ProductId,
                ProductAmount = elementInfo.Amount
            });
            return Created();

        }
        //Return product amount
        [HttpPatch]
        public async Task<IActionResult> UpdateAmount([FromBody] CartElementAmount cartElementAmount)
        {
            int affected = await _cartService.UpdateAmount(cartElementAmount.ElementId, cartElementAmount.Amount);
            return Ok();
        }
    }
}
