using api.Extensions;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/portfolio")]
    [ApiController]
    public class PortfolioController : ControllerBase
    {
        private readonly UserManager<AppUser> userManager;
        private readonly IStockRepository stockRepo;
        private readonly IPortfolioRepository portfolioRepo;
        private readonly IFMPService fmpService;

        public PortfolioController(UserManager<AppUser> userManager, IStockRepository stockRepo, IPortfolioRepository portfolioRepo, IFMPService fmpService)
        {
            this.userManager = userManager;
            this.stockRepo = stockRepo;
            this.portfolioRepo = portfolioRepo;
            this.fmpService = fmpService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserPortfolio()
        {
            var username = User.GetUsername();
            var appUser = await userManager.FindByNameAsync(username);
            var userPorfolio = await portfolioRepo.GetUserPortfolio(appUser);

            return Ok(userPorfolio);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddPortfolio(string symbol)
        {
            var username = User.GetUsername();
            var appUser = await userManager.FindByNameAsync(username);
            var stock = await stockRepo.GetBySymbolAsync(symbol);

            if (stock is null)
            {
                stock = await fmpService.FindStockBySymbolAsync(symbol);

                if (stock is null)
                    return BadRequest("Stock does not exist");

                await stockRepo.CreateAsync(stock);
            }

            if (stock is null)
                return BadRequest("Stock not found");

            var userPortfolio = await portfolioRepo.GetUserPortfolio(appUser);

            if (userPortfolio.Any(e => e.Symbol.ToLower() == symbol.ToLower()))
                return BadRequest("Can not add same stock to portfolio");

            var portfolioModel = new Portfolio {
                StockId = stock.Id,
                AppUserId = appUser.Id
            };

            await portfolioRepo.CreateAsync(portfolioModel);

            return portfolioModel is null ? StatusCode(500, "Could not create") : Created();
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeletePortfolio(string symbol)
        {
            var username = User.GetUsername();
            var appUser = await userManager.FindByNameAsync(username);
            var userPortfolios = await portfolioRepo.GetUserPortfolio(appUser);

            var filteredStock = userPortfolios.Where(s => s.Symbol.ToLower() == symbol.ToLower()).ToList();

            if (filteredStock.Count() == 1)
            {
                await portfolioRepo.DeleteAsync(appUser, symbol);

                return Ok();
            }

            return BadRequest("Stock not in your portfolio");
        }
    }
}