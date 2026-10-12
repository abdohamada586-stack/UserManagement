using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserManagement.Domain.Entities;
using UserManagement.Infrastructure.Responses;
using UserManagement.Persistence.Context;

namespace UserManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DepartmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var departments = await _context.Departments
                .AsNoTracking()
                .ToListAsync();

            return Ok(new ApiResponse<List<Department>>
            {
                Success = true,
                Message = "Departments retrieved successfully.",
                Data = departments
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create(Department department)
        {
            if (string.IsNullOrWhiteSpace(department.Name))
                return BadRequest(new ApiResponse<string>
                {
                    Success = false,
                    Message = "Department name is required."
                });

            department.Id = 0;

            _context.Departments.Add(department);
            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<Department>
            {
                Success = true,
                Message = "Department created successfully.",
                Data = department
            });
        }
    }
}