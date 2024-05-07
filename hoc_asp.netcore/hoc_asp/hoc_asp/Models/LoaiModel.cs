using System.ComponentModel.DataAnnotations;

namespace hoc_asp.Models
{
    public class LoaiModel
    {
        [Required]
        [MaxLength(50)]
        public string TenLoai { get; set; }
    }
}
