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
    public class UserKYCInfoRepository : Repository<SysUserKycInfo>, IUserKYCInfoRepository
    {
        ApplicationDBContext _context;

        public UserKYCInfoRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }
        public async Task<List<SysUserKycInfo>> GetAllUserKYCInfo()
        {
            try
            {
                var getUserKycInfo = await Context.SYS_UserKYCInfo
                            .AsNoTracking()
                            .ToListAsync();

                return getUserKycInfo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<SysUserKycInfo> GetUserKYCInfo(int id)
        {
            try
            {
                var getUserKycInfo = await Context.SYS_UserKYCInfo
                    .Include(p => p.Bank)
                    .Where(p => p.UserKYCID == id)
                            .SingleOrDefaultAsync();

                return getUserKycInfo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<SysUserKycInfo> GetUserKYCInfoByUserID(int id)
        {
            try
            {
                var UserKycInfo = await Context.SYS_UserKYCInfo
                    .Include(p => p.Bank)
                    .Where(p => p.Createdby == id)
                            .FirstOrDefaultAsync();

                return UserKycInfo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<SysUserKycInfo> GetIdentificationTypeById(int id) 
        {
            try
            {
                var kycInfo = await Context.SYS_UserKYCInfo.Where(p => p.UserKYCID == id)
                            .Include(x => x.IdentificationType)
                            .AsNoTracking()
                            .SingleOrDefaultAsync();

                return kycInfo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
   

}
