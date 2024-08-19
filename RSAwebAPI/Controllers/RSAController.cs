using Microsoft.AspNetCore.Mvc;
using RSAwebAPI.Models;
using RSAwebAPI.Services;

namespace RSAwebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RSAController : ControllerBase
    {
        private readonly RsaServices _rsaService;

        public RSAController()
        {
            _rsaService = new RsaServices();
        }

        [HttpGet("GenerateKeys")]
        public IActionResult GenerateKeys()
        {
            _rsaService.GenerateKeys(out string publicKey, out string privateKey);
            return Ok(new { PublicKey = publicKey, PrivateKey = privateKey });
        }

        [HttpPost("Encrypt")]
        public IActionResult Encrypt([FromBody] RsaRequest request)
        {
            if (string.IsNullOrEmpty(request.Text) || string.IsNullOrEmpty(request.Key))
            {
                return BadRequest("Text and PublicKey are required");
            }

            byte[] encryptedData = _rsaService.Encrypt(request.Text, request.Key);
            return Ok(Convert.ToBase64String(encryptedData));
        }

        [HttpPost("Decrypt")]
        public IActionResult Decrypt([FromBody] RsaRequest request)
        {
            if (string.IsNullOrEmpty(request.Text) || string.IsNullOrEmpty(request.Key))
            {
                return BadRequest("Encrypted text and PrivateKey are required");
            }

            byte[] encryptedData = Convert.FromBase64String(request.Text);
            string decryptedText = _rsaService.Decrypt(encryptedData, request.Key);
            return Ok(decryptedText);
        }
    }
}
