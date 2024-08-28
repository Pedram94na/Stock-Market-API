using api.DTOs.Comment;
using api.Extensions;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/comment")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository commentRepo;
        private readonly IStockRepository stockRepo;
        private readonly UserManager<AppUser> userManager;
        private readonly IFMPService fmpService;

        public CommentController(ICommentRepository commentRepo, IStockRepository stockRepo, UserManager<AppUser> userManager, IFMPService fmpService)
        {
            this.commentRepo = commentRepo;
            this.stockRepo = stockRepo;
            this.userManager = userManager;
            this.fmpService = fmpService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var comments = await commentRepo.GetAllAsync();
            var commentDto = comments.Select(c => c.ToCommentDto());
            
            return Ok(commentDto);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
                
            var comment = await commentRepo.GetByIdAsync(id);

            return comment is null ? NotFound("Comment not found") : Ok(comment);
        }

        [HttpPost]
        [Route("{symbol:alpha}")]
        public async Task<IActionResult> Create([FromRoute] string symbol, CreateCommentRequestDto commentDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var stock = await stockRepo.GetBySymbolAsync(symbol);

            if (stock is null)
            {
                stock = await fmpService.FindStockBySymbolAsync(symbol);

                if (stock is null)
                    return BadRequest("Stock does not exist");

                await stockRepo.CreateAsync(stock);
            }

            var username = User.GetUsername();
            var appUser = await userManager.FindByNameAsync(username);

            var commentModel = commentDto.ToCommentFromCreateDto(stock.Id);
            commentModel.AppUserId = appUser.Id;
            await commentRepo.CreateAsync(commentModel);

            return CreatedAtAction(nameof(GetById), new { id = commentModel.Id }, commentModel.ToCommentDto());
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCommentRequestDto commentDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
                
            var comment = await commentRepo.UpdateAsync(id, commentDto.ToCommentFromUpdateDto());

            return comment is null ? NotFound("Comment not found") : Ok(comment.ToCommentDto());
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
                
            var comment = await commentRepo.DeleteAsync(id);

            return comment is null ? NotFound("Comment not found") : Ok(comment);
        }
    }
}