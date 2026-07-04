using FranDanBackend.DTO;
using FranDanBackend.Models;
using FranDanBackend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Emit;
using System.Threading.Tasks;
using System.Security.Claims;

namespace FranDanBackend.Controllers
{
    [Route("api/plan")]
    [ApiController]
    public class PlanController : ControllerBase
    {
        private readonly PlanService service;

        public PlanController(PlanService _service)
        {
            service = _service;
        }

        [HttpPost("create")]
        [Authorize]
        public IActionResult create([FromBody] PlanCreateDTO request)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    return BadRequest("Wrong token. No user with this id.");
                }
                int loggedInUserId = int.Parse(userIdClaim.Value);
                service.create(loggedInUserId, request);
                return Ok("Plan created succesfully!");
            }
            catch (Exception ex)
            {
                return BadRequest($"Plan creation error: {ex.Message}");
            }
        }

        [HttpPut("edit")]
        [Authorize]
        public IActionResult edit([FromBody] PlanEditDTO request)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    return BadRequest("Wrong token. No user with this id.");
                }
                int loggedInUserId = int.Parse(userIdClaim.Value);
                service.edit(loggedInUserId, request);
                return Ok("Plan edited succesfully!");
            }
            catch (Exception ex)
            {
                return BadRequest($"Plan edition error: {ex.Message}");
            }
        }

        [HttpGet("full")]
        [Authorize]
        public IActionResult getFull(int request)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    return BadRequest("Wrong token. No user with this id.");
                }
                int loggedInUserId = int.Parse(userIdClaim.Value);

                var fullPlan = service.fullPlan(loggedInUserId, new PlanIdDTO { id = request });
                return Ok(fullPlan);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("No plan found"))
                {
                    return Ok(Array.Empty<object>());
                }

                return BadRequest($"Plan retrieval error: {ex.Message}");
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
                service.delete(loggedInUserId, new PlanIdDTO { id = request });
                return Ok("Plan deleted succesfully!");
            }
            catch (Exception ex)
            {
                return BadRequest($"Plan deletion error: {ex.Message}");
            }
        }
    }
}