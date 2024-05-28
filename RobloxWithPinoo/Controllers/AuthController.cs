using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using RobloxWithPinoo.Entity.Dtos.AuthDtos;
using RobloxWithPinoo.Services.AuthService;

namespace RobloxWithPinoo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterDto model)
        {
            var result = await _authService.RegisterUserAsync(model);

            if (result.Succeeded)
            {
                //var message = new MimeMessage();
                //message.From.Add(new MailboxAddress("Pinoo Robotics", "mehmetgulcudeveloper@gmail.com"));
                //message.To.Add(new MailboxAddress("User", model.Email));
                //message.Subject = "Pinoo Roblox'a Hoşgeldiniz !";
                //message.Body = new TextPart("plain") { Text = "Pinoo Robotics'e Hoşgeldiniz." };

                //using (var client = new SmtpClient())
                //{
                //    client.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                //    client.Authenticate("mehmetgulcudeveloper@gmail.com", "ldfmzxxthxfzhmtb");
                //    client.Send(message);
                //    client.Disconnect(true);
                //}

                return Ok("Kullanıcı başarıyla kaydoldu.");
            }
            else
            {
                return BadRequest("Kayıt olmadı");
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] LoginDto model)
        {
            var token = await _authService.LoginUserAsync(model);

            if (token != null)
            {
                return Ok(new { Token = token });
            }
            else
            {
                return Unauthorized("Geçersiz e-posta veya şifre.");
            }
        }
    }
}
