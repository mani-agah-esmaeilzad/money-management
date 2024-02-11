using NPOI.HPSF;
using System.ComponentModel.DataAnnotations;

namespace MoneyManagement.Models
{
    public class Cards
    {
        [Key]
        public Guid CardId { get; set; }
        public string CardNumber { get; set; }
    }
}
