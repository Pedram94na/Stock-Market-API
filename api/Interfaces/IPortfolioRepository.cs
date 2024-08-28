using api.Models;

namespace api.Interfaces
{
    public interface IPortfolioRepository
    {
        Task<List<Stock>> GetUserPortfolio(AppUser user);
        Task<Portfolio> CreateAsync(Portfolio portfolio);
        Task<Portfolio> DeleteAsync(AppUser appUser, string symbol);
    }
}