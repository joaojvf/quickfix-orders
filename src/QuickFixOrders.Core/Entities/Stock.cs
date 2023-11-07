namespace QuickFixOrders.Core.Entities;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Stock
{
    // [Key]
    // [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string Symbol { get; set; } = null!;

    public decimal FinancialExposition { get; set; }

    public decimal FinancialExpositionLimit { get; set; }
}