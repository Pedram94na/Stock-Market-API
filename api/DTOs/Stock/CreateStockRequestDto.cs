using System.ComponentModel.DataAnnotations;

namespace api.DTOs.Stock
{
    public class CreateStockRequestDto
    {
        [Required]
        [MaxLength(3, ErrorMessage = "Symbol can not be over 3 characters")]
        public string Symbol { get; set; } = string.Empty;

        [Required]
        [MinLength(3, ErrorMessage = "Company name can not b less than 3 characters")]
        public string CompanyName { get; set; } = string.Empty;
        
        [Required]
        [Range(0, 1000000000000000)]
        public decimal Purchase { get; set; }
        
        [Required]
        [Range(0.001, 100)]

        public decimal LastDiv { get; set; }

        [Required]
        [MaxLength(10, ErrorMessage = "Industry can not be over 10 characters")]
        public string Industry { get; set; } = string.Empty;
        
        [Required]
        [Range(0, 100000000000000)]
        public long MarketCap { get; set; }
    }
}