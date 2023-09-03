using BlogProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using BlogProject.DataAccess;
using Newtonsoft.Json;

namespace InternalPWebGateway.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private ISqlDataAccess _db;
        private IConfiguration _configuration;
        public AuthController(IConfiguration configuration, ISqlDataAccess db)
        {
            _configuration = configuration;
            _db = db;
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("/login")]
        public async Task<ActionResult> Login([FromBody] LoginUser userLogin)
        {
            var user = await Authenticate(userLogin);
            if (user != null)
            {
                var token = GenerateToken(user);
                return Ok(token);
            }
            return NotFound("user not found");
        }

        // To generate token
        private string GenerateToken(UserModel user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim("Username",(string)user.Username),
            };
            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        //to authenticate the user creds
        private async Task<UserModel> Authenticate(LoginUser userLogin)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                var apiUrl = $"https://localhost:7138/GetUserAuthentication?username={userLogin.Username}&password={userLogin.Password}";
                var response = await httpClient.GetAsync(apiUrl);
                var responseContent = (await response.Content.ReadAsStringAsync());

                var userAuthData = JsonConvert.DeserializeObject<UserModel>(responseContent);
                UserModel model = new UserModel();

                if (userAuthData != null)
                {
                    if (userAuthData.Username == userLogin.Username && userAuthData.Password == userLogin.Password)
                    {
                        model = userAuthData;
                    }
                    else
                    {
                        model = null;
                    }
                }else
                {
                    model = null;
                }
                return model;
            }
        }

        [HttpGet]
        [Route("/GetUserAuthentication")]
        public async Task<UserModel> GetUserAuth(string username, string password)
        {
            try
            {
                string SP = @"select blog.get_all_users_role(@username ,@password);";
                UserModel user = (await _db.LoadDataRefCursor<UserModel, dynamic>(SP, new { username, password })).FirstOrDefault();
                return user;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
