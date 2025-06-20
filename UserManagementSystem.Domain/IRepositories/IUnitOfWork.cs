using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagementSystem.Domain.IRepositories
{
    public interface IUnitOfWork
    {
        IUserRepository Users { get; }
        IPermissionRepository Permissions { get; }
        Task<int> CompleteAsync();
        void Dispose();
    }
}
