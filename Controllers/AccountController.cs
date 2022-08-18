using BlogApi.Data;
using BlogApi.Extensions;
using BlogApi.Models;
using BlogApi.Services;
using BlogApi.ViewModels;
using BlogApi.ViewModels.Accounts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureIdentity.Password;
using System.Text.RegularExpressions;

namespace BlogApi.Controllers
{
    [ApiController]
    public class AccountController : ControllerBase
    {
        [HttpPost("v1/accounts")]
        public async Task<IActionResult> Post(
                [FromBody] RegisterViewModel model,
                [FromServices] EmailService emailService,
                [FromServices] BlogApiDataContext context)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));

            var user = new User
            {
                Name = model.Name,
                Email = model.Email,
                Slug = model.Email.Replace('@', '-').Replace('.', '-')
            };

            var password = PasswordGenerator.Generate(25, true, false);
            user.PasswordHash = PasswordHasher.Hash(password);

            try
            {
                await context.Users.AddAsync(user);
                await context.SaveChangesAsync();

                emailService.Send(
                    user.Name,
                    user.Email,
                    "Curso Asp.Net",
                    $"Sua senha é {password}");

                return Ok(new ResultViewModel<dynamic>(new
                {
                    user = user.Name,
                    password
                }));
            }
            catch (DbUpdateException)
            {
                return BadRequest(new ResultViewModel<string>("05X99 - E-mail já cadastrado"));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<string>("05X99 - Erro interno do servidor"));
            }
        }
        [HttpPost("v1/accounts/login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(
            [FromBody] LoginViewModel model,
            [FromServices] BlogApiDataContext context,
            [FromServices] TokenService tokenService)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));
            try
            {

                var user = await context.Users
                        .AsNoTracking()
                        .Include(x => x.Roles)
                        .FirstOrDefaultAsync(x => x.Email == model.Email);

                if (user == null)
                    return StatusCode(401, new ResultViewModel<string>("05X101 - Usuário ou senha inválidos"));

                if (!PasswordHasher.Verify(user.PasswordHash, model.Password))
                    return StatusCode(401, new ResultViewModel<string>("05X101 - Usuário ou senha inválidos"));

                var token = tokenService.GenerateToken(user);
                return Ok(new ResultViewModel<string>(token,null));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<string>("05X99 - Erro interno do Servidor"));
            }
        }

        [Authorize]
        [HttpPost("v1/accounts/upload-image")]
        public async Task<IActionResult> UploadImage(
            [FromBody] UploadImageViewModel model,
            [FromServices] BlogApiDataContext context)
        {
            var filename = $"{Guid.NewGuid().ToString()}.jpg";
            var data = new Regex(@"^data:image\/[a-z]+;base64,").Replace(model.Base64Image, "");
            var bytes = Convert.FromBase64String(data);
            try
            {
                await System.IO.File.WriteAllBytesAsync($"wwwroot/images/{filename}", bytes);

            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResultViewModel<string>("0X05E00 - Falha interna no servidor"));
            }

            var user = await context.Users.FirstOrDefaultAsync(x => x.Email == User.Identity.Name);
            if (user == null)
                return NotFound(new ResultViewModel<User>("0X5E17 - Usuário não encontrado"));

            user.Image = $"https://localhost:7092/images/{filename}";

            try
            {
                context.Users.Update(user);
                await context.SaveChangesAsync();
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<string>("0X%E19 - Falha interna do servidor"));
            }

            return Ok(new ResultViewModel<string>("Imagem alterada com sucesso", null));

        }
    }
}
 