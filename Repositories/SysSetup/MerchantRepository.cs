//using PaymentGateway.Repositories;
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
    public class MerchantRepository : Repository<EgsMerchant>, IMerchantRepository
    {
        ApplicationDBContext _context;

        public MerchantRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }

        public List<EgsMerchant> GetAllForDropdown()
        {
            return Context.EGS_Merchant.ToList();
        }
        public async Task UpdateMerchant(EgsMerchant entity)
        {
            try
            {
                //Context.Entry(entity).State = EntityState.Modified;
                //Context.SaveChanges();


                _context.Entry(entity).State = EntityState.Modified;
               await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

            }
        }
        public async Task UpdateMerchantAsync(SysUsers entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
            await Context.SaveChangesAsync();


        }

        public List<SysUsers> GetAllMerchantsExcludingid(int merchantid)
        {
            try
            {
                var user = Context.SYS_Users
                                .AsNoTracking()
                                .Include(u => u.Merchant)
                                .Include(u => u.Role)
                                .Where(u => u.Role.RoleID == 4 && u.IsActive == true && u.Merchant.MerchantID != merchantid)
                                .ToList();

                return user;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SysUsers> GetAllMerchants()
        {
            try
            {
                var user = Context.SYS_Users
                                .AsNoTracking()
                                .Include(u => u.Merchant)
                                .Include(u => u.Role)
                                .Where(u => u.Role.RoleID == 4 && u.IsActive == true)
                                .ToList();

                return user;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<List<SysUsers>> GetAllTMerchants()
        {
            try
            {
                var user = await Context.SYS_Users
                                .AsNoTracking()
                                .Include(u => u.Merchant)
                                .Include(u => u.Role)
                                .Where(u => u.Role.RoleID == 4 && u.IsActive == true)
                                .ToListAsync();

                return user;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<SysUsers> GetMerchantByCode(string merchantcode)
        {
            try
            {
                var user = await Context.SYS_Users
                                .AsNoTracking()
                                .Include(u => u.Merchant)
                                .Include(u => u.Role)
                                .Where(u => u.Merchant.MerchantCode == merchantcode && u.IsActive == true && u.Role.RoleID == 4)
                                .FirstOrDefaultAsync();

                return user;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<SysUsers> GetMerchantsById(int Id)
        {
            try
            {
                var users = await Context.SYS_Users
                                 .Include(u => u.Merchant)
                                  .Include(u => u.ResidentialState)
                                 .Where(u => u.Role.RoleID == 4 && u.IsActive == true && u.Merchant.MerchantID == Id)
                                 .FirstOrDefaultAsync();

                return users;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //FetchMerchantMappingByMerchantID


        public async Task<EgsMerchant> GetMerchantsFromId(int Id)
        {
            try
            {
                var merchantuser = await Context.EGS_Merchant 
                                 .Where(u => u.MerchantID == Id)
                                 .FirstOrDefaultAsync();

                return merchantuser;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
