//using PaymentGateway.Data;
using Microsoft.EntityFrameworkCore;
using PaymentGateway21052021.Data;
using PaymentGateway21052021.Models;
using PaymentGateway21052021.Repositories.SysSetup.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Repositories
{

    public class MerchantMappingRepository : Repository<SysMerchantMapping>, IMerchantMappingRepository
    {
        ApplicationDBContext _context;

        public MerchantMappingRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<SysMerchantMapping>> FetchSubMerchantByMerchantID(int id)
        {
            try
            {
                var SubMerchantDetails = await Context.SYS_MerchantMapping  //
                           //.AsNoTracking()
                           .Include(r => r.Merchant)
                           .ThenInclude(r => r.Users)
                           .Where(r => r.Merchant.MerchantID == id)
                           .ToListAsync();

                return SubMerchantDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<SysMerchantMapping>> FetchAllSubMerchants()
        {
            try
            {
                var SubMerchantDetails = await Context.SYS_MerchantMapping  //.AsNoTracking()
                           .Include(r => r.Merchant)
                           .ThenInclude(r => r.Users)
                           .ToListAsync();

                return SubMerchantDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<SysMerchantMapping>> FetchMappedSubMerchants()
        {
            try
            {
                var SubMerchantDetails = await Context.SYS_MerchantMapping  //.AsNoTracking()
                           .Include(r => r.Merchant)
                           .AsNoTracking()
                            .ToListAsync();

                return SubMerchantDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<List<SysUsers>> GetMerchants()
        {
            try
            {
                var users = await Context.SYS_Users
                                 .Include(u => u.Role)
                                 .Include(u => u.Merchant)
                                 .Where(u => u.IsActive == true && u.Merchant.MerchantID != 0 && u.Role.RoleID == 4)// 4 = merchantroleid
                                 .ToListAsync();

                return users;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}