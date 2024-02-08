using PaymentGateway21052021.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Repositories.SysSetup.Interfaces
{
    public interface IRoleRepository: IRepository<SysRole>
    {
        Task<List<SysRole>> GetAllRoles();
        Task<List<SysRole>> UpgradeRoles();
        Task<SysRole> GetRole(int id);

    }

}
