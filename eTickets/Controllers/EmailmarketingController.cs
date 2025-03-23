using eTickets.Models;
using eTickets.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
namespace eTickets.Controllers
{
    public class EmailmarketingController : Controller
    {
        private readonly EmailService _emailService;

        public EmailmarketingController(EmailService emailService)
        {
            _emailService = emailService;
        }
        public IActionResult SendMarketingEmail()
        {
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> SendMarketingEmail(EmailMarketingModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            string templatPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Templates/EmailTemplate.html");
            string link = "https://your-marketing-link.com";
            string emailContent = await _emailService.LoadEmailTemplateAsync(templatPath, model.Name, link);
            await _emailService.SendEmailAsync(model.Email, "Ưu đãi đặc biệt cho ban!", emailContent);

            ViewBag.Message = "Email đã gửi thành công!";
            return View();
        }
    }
}
