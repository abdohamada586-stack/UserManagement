using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using UserManagement.Domain.Entities;
using UserManagement.Service.DTOs;
using UserManagement.Service.Interfaces;

namespace UserManagement.Service.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IConfiguration _configuration;
        private readonly IJwtService _jwtService;

        public UserService(
            UserManager<ApplicationUser> userManager,
            IDepartmentRepository departmentRepository,
            IConfiguration configuration, IJwtService jwtService)
        {
            _userManager = userManager;
            _departmentRepository = departmentRepository;
            _configuration = configuration;
            _jwtService = jwtService;
        }

        public async Task<bool> RegisterAsync(RegisterDto dto)
        {
            var department = await _departmentRepository.GetByIdAsync(dto.DepartmentId);

            if (department == null)
                return false;

            var user = new ApplicationUser
            {
                UserName = dto.Email,
                Email = dto.Email,
                FullName = $"{dto.FirstName.Trim()} {dto.LastName.Trim()}",
                DepartmentId = dto.DepartmentId,
                IsActive = true
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            return result.Succeeded;
        }

        public async Task<string?> LoginAsync(LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);

            if (user == null || !user.IsActive)
                return null;

            var passwordValid =
                await _userManager.CheckPasswordAsync(user, dto.Password);

            if (!passwordValid)
                return null;

            return await _jwtService.GenerateTokenAsync(
                user.Id,
                user.UserName ?? user.Email ?? "",
                user.Email ?? "");
        }

        public async Task<UserDetailsDto?> GetUserAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
                return null;

            var department =
                await _departmentRepository.GetByIdAsync(user.DepartmentId);

            var names = user.FullName.Split(
                ' ', 2, StringSplitOptions.RemoveEmptyEntries);

            return new UserDetailsDto
            {
                Id = user.Id,
                FirstName = names.Length > 0 ? names[0] : "",
                LastName = names.Length > 1 ? names[1] : "",
                Email = user.Email ?? "",
                DepartmentId = user.DepartmentId,
                DepartmentName = department?.Name ?? "",
                IsActive = user.IsActive
            };
        }

        public async Task<bool> UpdateUserAsync(string id, UpdateUserDto dto)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
                return false;

            var department =
                await _departmentRepository.GetByIdAsync(dto.DepartmentId);

            if (department == null)
                return false;

            user.FullName = $"{dto.FirstName.Trim()} {dto.LastName.Trim()}";
            user.DepartmentId = dto.DepartmentId;

            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }

        public async Task<bool> SetUserStatusAsync(string id, bool isActive)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
                return false;

            user.IsActive = isActive;

            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }
    }
}