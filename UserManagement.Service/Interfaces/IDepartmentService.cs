using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Domain.Entities;

namespace UserManagement.Service.Interfaces
{
    public interface IDepartmentService
    {
        Task<List<Department>> GetAllAsync();
    }
}