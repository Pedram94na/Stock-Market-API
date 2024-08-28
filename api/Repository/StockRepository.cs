using api.Data;
using api.DTOs.Stock;
using api.Interfaces;
using api.Models;
using api.Utils;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class StockRepository : IStockRepository
    {
        private readonly ApplicationDBContext context;

        public StockRepository(ApplicationDBContext context)
        {
            this.context = context;
        }

        public async Task<Stock> CreateAsync(Stock stockModel)
        {
            await context.Stocks.AddAsync(stockModel);
            await context.SaveChangesAsync();

            return stockModel;
        }

        public async Task<Stock?> DeleteAsync(int id)
        {
            var stockModel = await context.Stocks.FirstOrDefaultAsync(s => s.Id == id);

            if (stockModel is null)
                return null;

            context.Stocks.Remove(stockModel);
            await context.SaveChangesAsync();

            return stockModel;
        }

        public async Task<List<Stock>> GetAllAsync(QueryObject query)
        {
            var stocks = context.Stocks.Include(c => c.Comments).ThenInclude(c => c.AppUser).AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.Symbol))
                stocks = stocks.Where(s => s.Symbol.Contains(query.Symbol));

            if (!string.IsNullOrWhiteSpace(query.CompanyName))
                stocks = stocks.Where(s => s.CompanyName.Contains(query.CompanyName));

            if (!string.IsNullOrEmpty(query.SortBy) &&
                query.SortBy.Equals("Symbol", StringComparison.OrdinalIgnoreCase))
                {
                    stocks = query.IsDescending ? stocks.OrderByDescending(s => s.Symbol): stocks.OrderBy(s => s.Symbol);
                }

            var skipNumber = (query.PageNumber - 1) * query.PageSize;

            return await stocks.Skip(skipNumber).Take(query.PageSize).ToListAsync();
        }

        public async Task<Stock?> GetByIdAsync(int id)
        {
            return await context.Stocks.Include(s => s.Comments).FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<Stock?> GetBySymbolAsync(string symbol)
        {
            return await context.Stocks.FirstOrDefaultAsync(s => s.Symbol == symbol);
        }

        public async Task<Stock?> UpdateAsync(int id, UpdateStockRequestDto stockDto)
        {
            var existingStock = await context.Stocks.FirstOrDefaultAsync(s => s.Id == id);

            if (existingStock is null)
                return null;

            existingStock.Symbol = stockDto.Symbol;
            existingStock.CompanyName = stockDto.CompanyName;
            existingStock.Purchase = stockDto.Purchase;
            existingStock.LastDiv = stockDto.LastDiv;
            existingStock.Industry = stockDto.Industry;
            existingStock.MarketCap = stockDto.MarketCap;

            await context.SaveChangesAsync();

            return existingStock;
        }

        public async Task<bool> StockExists(int id)
        {
            return await context.Stocks.AnyAsync(s => s.Id == id);
        }
    }
}