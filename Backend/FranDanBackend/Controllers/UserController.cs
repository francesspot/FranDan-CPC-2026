using FranDanBackend.DTO;
using FranDanBackend.Models;
using FranDanBackend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
namespace FranDanBackend.Controllers
{

    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService service;
        public UserController(UserService _service)
        {
            service = _service;
        }

        [HttpGet("full")]
        [Authorize]
        [ProducesResponseType(typeof(UserFullDTO), StatusCodes.Status200OK)]
        public ActionResult<UserFullDTO> getFull()
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    return BadRequest("Wrong token. No user with this id.");
                }
                int loggedInUserId = int.Parse(userIdClaim.Value);
                return Ok(service.getUserFullDTO(loggedInUserId));
            }
            catch (Exception ex)
            {
                return BadRequest($"DTO generation error: {ex.Message}");
            }
        }
        [HttpDelete("delete")]
        [Authorize]
        public IActionResult delete()
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    return BadRequest("Wrong token. No user with this id.");
                }
                int loggedInUserId = int.Parse(userIdClaim.Value);
                service.delete(loggedInUserId);
                return Ok("Your account deleted succesfully!");
            }
            catch (Exception ex)
            {
                return BadRequest($"User account deletion error: {ex.Message}");
            }
        }
        [HttpPut("edit")]
        [Authorize]
        public IActionResult edit(UserEditDTO request)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    return BadRequest("Wrong token. No user with this id.");
                }
                int loggedInUserId = int.Parse(userIdClaim.Value);
                service.edit(loggedInUserId,request);
                return Ok("Your account edited succesfully!");
            }
            catch (Exception ex)
            {
                return BadRequest($"User account edition error: {ex.Message}");
            }
        }
    }

}