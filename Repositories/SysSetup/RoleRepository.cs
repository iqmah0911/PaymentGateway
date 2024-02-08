using Microsoft.EntityFrameworkCore;
using PaymentGateway21052021.Data;
using PaymentGateway21052021.Models;
using PaymentGateway21052021.Repositories.SysSetup.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Repositories.SysSetup
{
    public class RoleRepository : Repository<SysRole>, IRoleRepository
    {
        ApplicationDBContext _context;

        public RoleRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<SysRole>> GetAllRoles()
        {
            try
            {
                var allRole = await Context.SYS_Role
                            .AsNoTracking()
                            .ToListAsync();

                return allRole;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<SysRole>> UpgradeRoles()
        {
            try
            {
                var allRole = await Context.SYS_Role
                            .Where(u => u.RoleID == 3 || u.RoleID == 5)
                            .AsNoTracking()
                            .ToListAsync();

                return allRole;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<SysRole> GetRole(int id)
        {
            try
            {
                var idRole = await Context.SYS_Role
                            .Where(u => u.RoleID == id)
                            .Include(u=>u.Users) 
                            .AsNoTracking()
                             .FirstOrDefaultAsync();
                            //.SingleOrDefaultAsync();

                return idRole;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
