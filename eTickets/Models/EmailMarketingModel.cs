using System.ComponentModel.DataAnnotations;

namespace eTickets.Models
{
    public class EmailMarketingModel
    {
        [Required(ErrorMessage = " vui lòng nhập Email")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]

        public string Email { get; set; }

        [Required(ErrorMessage = "Vui lòng nhap họ tên")]
        public string Name { get; set; }
    }
}
