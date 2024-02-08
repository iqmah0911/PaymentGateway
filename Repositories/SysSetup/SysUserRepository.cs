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
    public class SysUserRepository:Repository<SysUsers>, ISysUserRepository
    {
        ApplicationDBContext _context;

        public SysUserRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }

        public void UpdateUser(SysUsers entity)
        {
            try
            {
                Context.Entry(entity).State = EntityState.Modified;
            }
            catch (Exception ex)
            {

            }
        }

        public async Task<SysUsers> GetUserProfile(int id)
        {
            try
            {
                var userProfileInfo = await Context.SYS_Users
                    .Include(p => p.Title)
                    .Where(p => p.UserID == id)
                            .SingleOrDefaultAsync();

                return userProfileInfo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<SysUsers> GetEgoleUserToUpdate(int Id)
        {
            try
            {
                return await _context.SYS_Users
                                        .Include(r => r.Role)
                                        .FirstOrDefaultAsync(x => x.UserID == Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public async Task<SysUserKycInfo> GetUserKYC(int id)
        //{
        //    try
        //    {
        //        var userKYCInfo = await Context.SYS_UserKYCInfo
        //            .Include(p => p.Bank)
        //            .Where(p => p.UserKYCID == id)
        //                    .SingleOrDefaultAsync();

        //        return userKYCInfo;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public async Task<SysUsers> GetUserToBeIssuedhWallet(SysUsers userDet)
        {
            try
            {
                var user = await Context.SYS_Users
                                .AsNoTracking()
                                .Where(u => u.UserID == userDet.UserID)
                                .SingleOrDefaultAsync();

                return user;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<SysUsers> GetUserToBeIssuedhWalletX(int userID)
        {
            try
            {
                var user = await Context.SYS_Users
                                .AsNoTracking()
                                .Include(u => u.Wallet)
                                .Where(u => u.UserID == userID)
                                .FirstOrDefaultAsync();

                return user;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
