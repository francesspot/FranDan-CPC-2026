using FranDanBackend.DTO;
using FranDanBackend.Models;
using FranDanBackend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Emit;
using System.Threading.Tasks;
using System.Security.Claims;
namespace FranDanBackend.Controllers
{

    [Route("api/friend")]
    [ApiController]
    public class FriendController : ControllerBase
    {
        private readonly FriendService service;
        public FriendController(FriendService _service)
        {
            service = _service;
        }

        [HttpPost("invite")]
        [Authorize]
        public IActionResult invite([FromBody] UserFindDTO request)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    return BadRequest("Wrong token. No user with this id.");
                }
                int loggedInUserId = int.Parse(userIdClaim.Value);
                service.inviteFriend(loggedInUserId, request);
                return Ok("Friend invited succesfully!");
            }
            catch (Exception ex)
            {
                return BadRequest($"Inviting friend error: {ex.Message}");
            }
        }
        [HttpPatch("accept")]
        [Authorize]
        public IActionResult accept(int request)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    return BadRequest("Wrong token. No user with this id.");
                }
                int loggedInUserId = int.Parse(userIdClaim.Value);
                service.acceptFriend(loggedInUserId, new UserIdDTO { id = request });
                return Ok("Friend invite accepted succesfully!");
            }
            catch (Exception ex)
            {
                return BadRequest($"Accepting friend error: {ex.Message}");
            }
        }
        [HttpPatch("reject")]
        [Authorize]
        public IActionResult reject(int request)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    return BadRequest("Wrong token. No user with this id.");
                }
                int loggedInUserId = int.Parse(userIdClaim.Value);
                service.rejectFriend(loggedInUserId, new UserIdDTO { id = request });
                return Ok("Friend invite rejected succesfully! This user is from now on black list.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Rejecting friend error: {ex.Message}");
            }
        }
        [HttpDelete("delete")]
        [Authorize]
        public IActionResult delete(int request)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    return BadRequest("Wrong token. No user with this id.");
                }
                int loggedInUserId = int.Parse(userIdClaim.Value);
                service.deleteFriend(loggedInUserId, new UserIdDTO { id = request });
                return Ok("Friend removed succesfully! This user is from now on black list.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Removing friend error: {ex.Message}");
            }
        }
    }
}