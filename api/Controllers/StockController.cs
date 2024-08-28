using api.DTOs.Stock;
using api.Interfaces;
using api.Mappers;
using api.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/stock")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly IStockRepository stockRepo;

        public StockController(IStockRepository stockRepo)
        {
            this.stockRepo = stockRepo;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll([FromQuery] QueryObject query)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
                
            var stocks = await stockRepo.GetAllAsync(query);
            var stockDto = stocks.Select(s => s.ToStockDto()).ToList();

            return Ok(stockDto);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
                
            var stock = await stockRepo.GetByIdAsync(id);
            
            return stock is null ? NotFound("Stock not found") : Ok(stock.ToStockDto());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStockRequestDto stockDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
                
            var stockModel = stockDto.ToStockFromCreateDto();
            await stockRepo.CreateAsync(stockModel);

            return CreatedAtAction(nameof(GetById), new { id = stockModel.Id },stockModel.ToStockDto());
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDto stockDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
                
            var stockModel = await stockRepo.UpdateAsync(id, stockDto);

            return stockModel is null ? NotFound("Stock not found") : Ok(stockModel.ToStockDto());
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
                
            var stockModel = await stockRepo.DeleteAsync(id);

            return stockModel is null ? NotFound("Stock not found") : NoContent();
        }
    }
}