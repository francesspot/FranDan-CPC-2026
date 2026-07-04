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

    [Route("api/participation")]
    [ApiController]
    public class ParticipationController : ControllerBase
    {
        private readonly PlanService service;
        public ParticipationController(PlanService _service)
        {
            service = _service;
        }

        [HttpPost("add")]
        [Authorize]
        public IActionResult add([FromBody] PlanParticipantDTO request)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    return BadRequest("Wrong token. No user with this id.");
                }
                int loggedInUserId = int.Parse(userIdClaim.Value);
                service.addParticipant(loggedInUserId, request);
                return Ok("Participant added succesfully!");
            }
            catch (Exception ex)
            {
                return BadRequest($"Participant addition error: {ex.Message}");
            }
        }
        [HttpDelete("delete")]
        [Authorize]
        public IActionResult delete([FromBody] PlanActionDTO request)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    return BadRequest("Wrong token. No user with this id.");
                }
                int loggedInUserId = int.Parse(userIdClaim.Value);
                service.removeParticipant(loggedInUserId, request);
                return Ok("Participant removed succesfully!");
            }
            catch (Exception ex)
            {
                return BadRequest($"Participant removal error: {ex.Message}");
            }
        }
        [HttpPatch("accept")]
        [Authorize]
        public IActionResult accept([FromBody] PlanIdDTO request)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    return BadRequest("Wrong token. No user with this id.");
                }
                int loggedInUserId = int.Parse(userIdClaim.Value);
                service.acceptInvitation(loggedInUserId, request);
                return Ok("Plan invitation accepted succesfully!");
            }
            catch (Exception ex)
            {
                return BadRequest($"Plan invitation acteptation error: {ex.Message}");
            }
        }
        [HttpPatch("reject")]
        [Authorize]
        public IActionResult rejec([FromBody] PlanIdDTO request)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    return BadRequest("Wrong token. No user with this id.");
                }
                int loggedInUserId = int.Parse(userIdClaim.Value);
                service.rejectInvitation(loggedInUserId, request);
                return Ok("Plan invitation rejected succesfully!");
            }
            catch (Exception ex)
            {
                return BadRequest($"Plan invitation rejection error: {ex.Message}");
            }
        }
        [HttpPatch("set-admin")]
        [Authorize]
        public IActionResult setAdmin([FromBody] PlanParticipantDTO request)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    return BadRequest("Wrong token. No user with this id.");
                }
                int loggedInUserId = int.Parse(userIdClaim.Value);
                service.setAdmin(loggedInUserId, request);
                return Ok("Admin state changed succesfully!");
            }
            catch (Exception ex)
            {
                return BadRequest($"Admin state change error: {ex.Message}");
            }
        }
    }
}