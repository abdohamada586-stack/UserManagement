using Microsoft.AspNetCore.Mvc;
using UserManagement.Infrastructure.Responses;
using UserManagement.Service.DTOs;
using UserManagement.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace UserManagement.Controllers
{  
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var result = await _userService.RegisterAsync(dto);

            if (!result)
                return BadRequest(new ApiResponse<string>
                {
                    Success = false,
                    Message = "Registration failed. Check the department ID and user data."
                });

            return Ok(new ApiResponse<string>
            {
                Success = true,
                Message = "User registered successfully.",
                Data = null
            });
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var token = await _userService.LoginAsync(dto);

            if (token == null)
                return Unauthorized(new ApiResponse<string>
                {
                    Success = false,
                    Message = "Invalid credentials or inactive user."
                });

            return Ok(new ApiResponse<string>
            {
                Success = true,
                Message = "Login successful.",
                Data = token
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(string id)
        {
            var user = await _userService.GetUserAsync(id);

            if (user == null)
                return NotFound(new ApiResponse<UserDetailsDto>
                {
                    Success = false,
                    Message = "User not found."
                });

            return Ok(new ApiResponse<UserDetailsDto>
            {
                Success = true,
                Message = "User retrieved successfully.",
                Data = user
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(
            string id, UpdateUserDto dto)
        {
            var result = await _userService.UpdateUserAsync(id, dto);

            if (!result)
                return BadRequest(new ApiResponse<string>
                {
                    Success = false,
                    Message = "Update failed. Check the user ID and department ID."
                });

            return Ok(new ApiResponse<string>
            {
                Success = true,
                Message = "User updated successfully."
            });
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> SetUserStatus(
            string id, [FromQuery] bool isActive)
        {
            var result = await _userService.SetUserStatusAsync(id, isActive);

            if (!result)
                return NotFound(new ApiResponse<string>
                {
                    Success = false,
                    Message = "User not found or status update failed."
                });

            return Ok(new ApiResponse<string>
            {
                Success = true,
                Message = isActive
                    ? "User activated successfully."
                    : "User deactivated successfully."
            });
        }
    }
}