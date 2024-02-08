using Microsoft.AspNetCore.Identity;
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
    public class UserRepository : Repository<ApplicationUser>, IUserRepository
    {
        private readonly SignInManager<ApplicationUser> _signInManager;

        ApplicationDBContext _context;

        public UserRepository(SignInManager<ApplicationUser> signInManager, ApplicationDBContext context) : base(context)
        {
            _context = context;
            _signInManager = signInManager;
        }

        public async Task UpdateUserAsync(SysUsers entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
            await Context.SaveChangesAsync();


        }
        public async Task UpdateUser(SysUsers entity)
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

        public async Task<SysUsers> GetSysUsers(string Id)
        {
            try
            {
                return await _context.SYS_Users
                                        .Include(r => r.ResidentialState)
                                        .Include(r => r.Role)
                                        .Include(r => r.Wallet)
                                        .Include(r=>r.UserType)
                                        .FirstOrDefaultAsync(x => x.UserID == Convert.ToInt32(Id));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<SysUsers> GetAdminUser(string Id)
        {
            try
            {
                return await _context.SYS_Users
                                        .Include(r => r.ResidentialState)
                                        .Include(r => r.Role)
                                       // .Include(r => r.Wallet)
                                        .FirstOrDefaultAsync(x => x.UserID == Convert.ToInt32(Id));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<SysUsers> GetMerchantSysUsers(int Id)
        {
            try
            {
                return await _context.SYS_Users
                                        .Include(r => r.Role)
                                        .Include(r => r.Wallet)
                                        .Include(r => r.Merchant)
                                        .FirstOrDefaultAsync(x => x.UserID == Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<SysUsers> GetSubMerchantSysUsers(int Id)
        {
            try
            {
                return await _context.SYS_Users
                                        .Include(r => r.Role)
                                        .Include(r => r.Wallet)
                                        .Include(r => r.Merchant)
                                        .FirstOrDefaultAsync(x => x.UserID == Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<SysUsers> UpdatingSubMerchantSysUsers(int Id)
        {
            try
            {
                return await _context.SYS_Users
                                        .Include(r => r.Merchant)
                                        .FirstOrDefaultAsync(x => x.UserID == Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<SysUsers> GetUserByPhone(string phone)
        {
            try
            {
                var user = await Context.SYS_Users
                                .AsNoTracking()
                                .Include(r => r.Wallet)
                                .Include(u => u.Role)
                                .Include(u => u.ResidentialState)
                                .Where(u => u.PhoneNumber == phone)
                                .FirstOrDefaultAsync();

                return user;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<bool> ApiLogin(string phone, string password)
        {

            var user2 = await Context.SYS_Users.Where(x=>x.PhoneNumber==phone).FirstOrDefaultAsync();
            if (user2 != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user2.Email, password, false, lockoutOnFailure: true);

                if (result.Succeeded)
                {
                    return true;
                }
            }

            return false;
        }


        public async Task<SysUsers> GetUserByEmail(string email)
        {
            try
            {
                var user = await Context.SYS_Users
                                .AsNoTracking()
                                .Include(r => r.Wallet)
                                //.Include(r=>r.Wallet.User)
                                .Include(u => u.Role)
                                .Include(u => u.ResidentialState)
                                .Where(u => u.Email == email && u.IsActive == true)
                                .FirstOrDefaultAsync();

                return user;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<SysUsers> GetUserLoginByEmailAPI(string email)
        {
            try
            {
                var user = await Context.SYS_Users
                                .AsNoTracking()
                                .Include(u => u.Role)
                                .Include(s => s.Merchant)
                                .Include(u => u.Wallet)
                                .Where(u => u.Email == email && u.IsActive == true)
                                .FirstOrDefaultAsync();

                return user;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<SysUsers> GetUserByEmailAPI(string email)
        {
            try
            {
                var user = await Context.SYS_Users
                                .AsNoTracking()
                                .Include(u => u.Role)
                                .Where(u => u.Email == email)
                                .FirstOrDefaultAsync();

                return user;
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
                                 .Where(u => u.Merchant.MerchantID != 0 && u.Role.RoleID == 4)// 4 = merchantroleid
                                 .ToListAsync();

                return users;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<EgsMerchant>> GetAllMerchants()
        {
            try
            {
                var merchantusers = await Context.EGS_Merchant
                                 //.Include(u => u.Role)
                                 .ToListAsync();

                return merchantusers;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<SysUsers>> GetMerchantsToday(DateTime startPeriod, DateTime endPeriod)
        {
            try
            {
                var merchantusers = await Context.SYS_Users
                                 .Include(mer => mer.Merchant)
                                 .Include(u => u.Role)
                                 .Where(u => u.IsActive == true && u.Merchant.MerchantID != 0 && u.Role.RoleID == 4
                                 && u.Merchant.DateCreated.Date == startPeriod.Date.Date)
                                 .ToListAsync();

                return merchantusers;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<SysUsers>> GetAgentsToday(DateTime startPeriod, DateTime endPeriod)
        {
            try
            {
                var merchantusers = await Context.SYS_Users
                                 .Include(u => u.Role)
                                 .Where(u => u.IsActive == true && u.Role.RoleID == 5 && u.DateCreated.Date == startPeriod.Date.Date)
                                 .ToListAsync();

                return merchantusers;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<SysUsers>> GetAllCourtevilleAgents()
        {
            //&& x.UserID > 1073
            try
            {
                var agentUsers = await Context.SYS_Users
                                 .Include(u => u.Role)
                                 .Include(u => u.Wallet)
                                 .Where(x => x.IsActive == true && x.Role.RoleID == 5 && 
                                    (x.CompanyName.Contains("corporate head office") || x.CompanyName.Contains("courteville"))
                                 )
                                 .ToListAsync();

                return agentUsers;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<List<SysUsers>> GetAllAgents()
        {
            //&& x.UserID > 1073
            try
            {
                var agentUsers = await Context.SYS_Users
                                 .Include(u => u.Role)
                                 .Include(u => u.Wallet)
                                 .Where(x => x.IsActive == true && x.Wallet.WalletID > 0 && x.Role.RoleID!=4 && x.Role.RoleID != 2 && x.Role.RoleID != 12
                                 && x.Role.RoleID != 13 && x.Role.RoleID != 11)//x.Role.RoleID == 5   1073)
                                 .ToListAsync();

                return agentUsers;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<SysUsers>> GetActiveAgentsNew()
        {
            try
            {
                var agentUsers = await Context.SYS_Users
                                 .Include(u => u.Role)
                                 .Include(u => u.Wallet)
                                 .Where(x => (x.Role.RoleID == 5 || x.Role.RoleID == 3) && x.IsActive == true)
                                 .ToListAsync();

                return agentUsers;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<List<SysUsers>> GetActiveAgents()
        {
            try
            {
                var agentUsers = await Context.SYS_Users
                                 .Include(u => u.Role)
                                 .Include(u => u.Wallet)
                                 .Include(u => u.ResidentialState)
                                 .Include(u => u.UserType)
                                 .Where(x => (x.Role.RoleID == 5 || x.Role.RoleID == 3 || x.Role.RoleID == 1) && x.IsActive == true && x.Role.RoleID != 13 && x.Role.RoleID != 12
                                && x.Role.RoleID != 11 && x.Role.RoleID != 4 && x.Role.RoleID != 2 && x.Role.RoleID != 6 && x.Wallet.WalletID != 0)
                                 .ToListAsync();

                return agentUsers;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<SysUsers>> GetInActiveAgents()
        {
            try
            {
                var agentUsers = await Context.SYS_Users
                                 .Include(u => u.Role)
                                 .Where(x => x.Role.RoleID == 5 && x.IsActive == false)
                                 .ToListAsync();

                return agentUsers;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<SysUsers>> GetActiveMerchants()
        {
            try
            {
                var merchantusers = await Context.SYS_Users
                                 .Include(mer => mer.Merchant)
                                 .Include(u => u.Role)
                                 .Where(u => u.IsActive == true && u.Role.RoleID == 4)
                                 .ToListAsync();

                return merchantusers;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<SysUsers>> GetMerchantUsers()
        {
            try
            {
                var merchantusers = await Context.SYS_Users
                                 .Include(mer => mer.Merchant)
                                 .Include(u => u.Role)
                                 .Where(u => u.Merchant.MerchantID != 0 && u.Role.RoleID == 4)
                                 .ToListAsync();

                return merchantusers;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<SysUsers>> GetInActiveMerchants()
        {
            try
            {
                var merchantusers = await Context.SYS_Users
                                 .Include(mer => mer.Merchant)
                                 .Include(u => u.Role)
                                 .Where(u => u.IsActive == false && u.Role.RoleID == 4)
                                 .ToListAsync();

                return merchantusers;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<SysUsers> GetAgentById(int Id)
        {
            try
            {
                var users = await Context.SYS_Users
                                 .Include(r => r.Role)
                                 .Include(u => u.Wallet)
                                 .Where(u => (u.Role.RoleID == 5 || u.Role.RoleID == 3) && u.IsActive == true && u.UserID == Id)
                                 .FirstOrDefaultAsync();

                return users;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<List<SysUsers>> listSelected(int[] IDs)
        {
            try
            {
                //int[] myInts = IDs.Select(Int32.Parse).ToArray();
                var selected = await Context.SYS_Users
                    .Where(a => IDs.Contains(a.UserID))
                    .ToListAsync();

                return selected;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            //var result = from Q in Context.SYS_Users
            //             where myInts.Contains(Q.UserID)
            //             orderby Q.CompanyName
            //             select Q;
        }

        public async Task<List<SysUsers>> GetMerchantsExcludingId(int Id)
        {
            try
            {
                var users = await Context.SYS_Users
                                 //.Include(u => u.Role)
                                 .Where(u => u.Role.RoleID == 4 && u.IsActive == true && u.UserID != Id)
                                 .ToListAsync();

                return users;
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
                                 .Where(u => u.Role.RoleID == 4 && u.UserID == Id)
                                 .SingleOrDefaultAsync();

                return users;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<List<SysUsers>> FetchMerchantMappingByMerchantID(int id)
        {
            try
            {
                var MerchantMappingDetails = await Context.SYS_Users.Where(p => p.Merchant.MerchantID == id)
                           //.AsNoTracking()
                           .Include(r => r.Merchant)
                           .ToListAsync();

                return MerchantMappingDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<SysUsers> GetUserByEmailAndPhoneNumber(string email, string phoneNumber)
        {
            try
            {
                var user = await Context.SYS_Users
                                .AsNoTracking()
                                .Include(u => u.Role)
                                .Where(u => (u.Email == email || u.PhoneNumber == phoneNumber) && u.IsActive == true)
                                .FirstOrDefaultAsync();

                return user;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public async Task<SysUsers> GetUserByChangePassword(SysUsers usercheck)
        {
            try
            {
                var user = await Context.SYS_Users
                                .AsNoTracking()
                                .Include(u => u.Role)
                                .Where(u => (u.Email == usercheck.Email) && u.IsActive == true)
                                .FirstOrDefaultAsync();

                return user;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<SysUsers> GetSysUsersAPI(string Id)
        {
            try
            {
                var ApplicationUser = await _context.Users.FirstOrDefaultAsync(x => x.Id == Id);
                return await _context.SYS_Users
                                        //.Include(r => r.UserState)
                                        //.Include(c => c.ApplicationUser)
                                        //.Include(b => b.Branch)
                                        //.Include(r => r.Role)
                                        //.Include(u => u.Company)
                                        //.Include(u => u.Regions)
                                        .FirstOrDefaultAsync(x => x.UserID == Convert.ToInt32(ApplicationUser.UserID));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<SysUsers>> GetAllActiveUsers()
        {
            try
            {
                var users = await Context.SYS_Users
                               // .Include(r => r.ResidentialState)
                                .Include(x=>x.ResidentialState)
                                //.Include(v=>v.Lga)
                                .Include(f => f.Role)
                                //.Include(v => v.KYCID) 
                                //.Include(v => v.UserType)
                                .Include(v => v.Wallet)
                                .Where(u => !string.IsNullOrEmpty(u.FirstName) && u.UserID>= 1083 && (u.PhoneNumber!="" && u.PhoneNumber.Length>11))
                                .ToListAsync();

                return users;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<SysUsers> GetUserProfile(int userID)
        {
            try
            {
                var user = await Context.SYS_Users
                                .AsNoTracking()
                                .Include(u => u.Role)
                                .Include(r => r.Wallet)
                                .Where(u => u.UserID == userID)
                                .FirstOrDefaultAsync();

                return user;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<SysUsers> GetUserByInvoice(int userID)
        {
            try
            {
                var user = await Context.SYS_Users
                                .AsNoTracking()
                                .Where(u => u.UserID == userID)
                                .FirstOrDefaultAsync();

                return user;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<SysUsers> GetExistingUser(string email, string mobileNumber)
        {
            try
            {
                var user = await Context.SYS_Users
                                .AsNoTracking()
                                .Where(u => (u.Email == email || u.PhoneNumber == mobileNumber) && u.IsSignUp == false)
                                .FirstOrDefaultAsync();

                return user;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<EgsWallet> GetExistingUserBVN(string BVN)
        {
            try
            {
                var user = await Context.EGS_Wallet
                                .Where(u => (u.BVN == BVN)).FirstOrDefaultAsync();

                return user;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<bool> CheckExistingUser(string phoneNumber, string email)
        {
            try
            {
                var user = await Context.SYS_Users
                                .AsNoTracking()
                                .Where(u => (u.PhoneNumber == phoneNumber || u.Email == email) && u.IsSignUp == false)
                                .ToListAsync();
                if (user.Count > 0)
                {
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> CheckExistingUserWithEmailAPI(string email)
        {
            try
            {
                var user = await Context.SYS_Users
                                .AsNoTracking()
                                .Where(u => u.Email == email)
                                .ToListAsync();
                if (user.Count > 0)
                {
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<EgsUpgradeAccount>> GetPendingUpgradeRequest()
        {
            try
            {
                var users = await Context.EGS_UpgradeAccount
                                .Include(u => u.User)
                                //.Include(rl => rl.RoleRequest)
                                .Where(u => u.User.IsActive == true && u.IsProcessed == false)
                                .AsNoTracking()
                                .ToListAsync();

                return users;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<EgsUpgradeAccount> GetUpgradeRequestDetails(int reqId)
        {
            try
            {
                var users = await Context.EGS_UpgradeAccount
                                .Include(u => u.User)
                                //.Include(rl => rl.RoleRequest)
                                .Where(u => u.User.IsActive == true && u.IsProcessed == false && u.UpgradeAccountID == reqId)
                                .AsNoTracking()
                                .SingleOrDefaultAsync();

                return users;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<SysUsers> GetSysUserByRole(int roleId)
        {
            try
            {
                var userrole = await Context.SYS_Users
                                .Include(rl => rl.Role)
                                .Where(u => u.Role.RoleID == roleId)
                                .AsNoTracking()
                                .SingleOrDefaultAsync();

                return userrole;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<SysUsers>> GetLSysUserByRole(int roleId)
        {
            try
            {
                var userrole = await Context.SYS_Users
                                .Include(rl => rl.Role)
                                .Include(r => r.Wallet)
                                .Where(u => u.Role.RoleID == roleId)
                                .AsNoTracking()
                                .ToListAsync();

                return userrole;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }






        public async Task<SysUsers> GetUserByStateID(int id)
        {
            try
            {
                var stateuser = await Context.SYS_Users.Where(u => u.UserID == id)
                            .Include(u => u.ResidentialState)
                            .AsNoTracking()
                            .SingleOrDefaultAsync();

                return stateuser;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<SysUsers> GetUserByID(int id)
        {
            try
            {
                var stateuser = await Context.SYS_Users.Where(u => u.UserID == id)
                             .Include(u => u.ResidentialState)
                             .Include(u => u.Wallet)
                            .AsNoTracking()
                            .FirstOrDefaultAsync();

                return stateuser;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<SysUsers>> GetUserActiveSubAgents()
        {
            try
            {
                var users = await Context.SYS_Users
                                 .Include(u => u.Merchant)
                                 .Where(u => u.Role.RoleID == 9 && u.IsActive == true && u.BankAccountCode != null)
                                 .ToListAsync();

                return users;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<SysUsers>> GetSpecialSubAgents()
        {
            try
            {
                var agentUsers = await Context.SYS_Users
                                 .Include(u => u.Role)
                                 .Include(u => u.Wallet)
                                 .Where(x => x.IsActive == true && x.UserID > 1073 && x.BankAccountCode != null && x.IsSpecial == true)//x.Role.RoleID == 5)
                                 .ToListAsync();

                return agentUsers;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<SysUsers>> GetSpecialAgents()
        {
            try
            {
                var agentUsers = await Context.SYS_Users
                                 .Include(u => u.Role)
                                 .Include(u => u.Wallet)
                                 .Where(x => x.IsActive == true && x.UserID > 1073 && x.IsSpecial == true && x.Role.RoleID == 5)
                                 .ToListAsync();

                return agentUsers;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<SysUsers>> GetUserByWalletAccountNumber(string code)
        {
            try
            {
                var agentUsers = await Context.SYS_Users
                                 .Include(u => u.Role)
                                 .Include(u => u.Wallet)
                                 .Where(x => x.IsActive == true && x.UserID > 1073 && x.BankAccountCode == code)
                                 .ToListAsync();

                return agentUsers;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<SysUsers>> GetAllSystemUsers()
        {
            try
            {
                var users = await Context.SYS_Users
                                .Include(x => x.ResidentialState)
                                // .Include(v => v.Lga)
                                .Include(f => f.Role)
                                // .Include(v => v.UserType)
                                // .Include(w => w.AccountType)
                                //.Include(v => v.Wallet)
                                .Where(u => !string.IsNullOrEmpty(u.FirstName) && (u.PhoneNumber != ""))
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
