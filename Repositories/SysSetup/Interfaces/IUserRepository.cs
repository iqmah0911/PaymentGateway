using PaymentGateway21052021.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Repositories.SysSetup.Interfaces
{
    public interface IUserRepository : IRepository<ApplicationUser>
    {
        Task UpdateUser(SysUsers entity);
        Task UpdateUserAsync(SysUsers entity);
        Task<SysUsers> GetSysUsers(string Id);

        Task<SysUsers> GetAdminUser(string Id);

        Task<SysUsers> GetUserByPhone(string phone);
        Task<bool> ApiLogin(string phone, string password);
        Task<SysUsers> GetUserByEmail(string email);
        Task<List<SysUsers>> GetAllActiveUsers();
        Task<SysUsers> GetUserByEmailAndPhoneNumber(string email, string phoneNumber);
        Task<List<SysUsers>> GetUserByWalletAccountNumber(string code);
        Task<SysUsers> GetUserProfile(int userID);
        Task<List<SysUsers>> GetLSysUserByRole(int roleId);
        Task<SysUsers> GetUserByInvoice(int userID);
        Task<bool> CheckExistingUser(string phoneNumber, string email);
        Task<SysUsers> GetExistingUser(string email, string mobileNumber);
        Task<List<SysUsers>> GetMerchants();
        Task<List<EgsMerchant>> GetAllMerchants();
        Task<SysUsers> GetMerchantsById(int Id);
        Task<List<SysUsers>> GetMerchantsExcludingId(int Id);
        Task<List<SysUsers>> listSelected(int[] IDs);
        Task<List<SysUsers>> FetchMerchantMappingByMerchantID(int Id);
        Task<SysUsers> GetMerchantSysUsers(int Id);
        Task<bool> CheckExistingUserWithEmailAPI(string email);
        Task<SysUsers> GetUserByEmailAPI(string email);
        Task<SysUsers> GetUserByChangePassword(SysUsers usercheck);
        Task<SysUsers> GetSubMerchantSysUsers(int Id);
        Task<SysUsers> UpdatingSubMerchantSysUsers(int Id);
        Task<List<SysUsers>> GetAllAgents();
        Task<SysUsers> GetAgentById(int Id);
        Task<IEnumerable<SysUsers>> GetMerchantsToday(DateTime startPeriod, DateTime endPeriod);
        Task<IEnumerable<SysUsers>> GetAgentsToday(DateTime startPeriod, DateTime endPeriod);
        Task<List<SysUsers>> GetActiveAgents();
        Task<List<SysUsers>> GetInActiveAgents();
        Task<List<SysUsers>> GetActiveMerchants();
        Task<List<SysUsers>> GetInActiveMerchants();
        Task<List<SysUsers>> GetMerchantUsers();
        Task<SysUsers> GetUserLoginByEmailAPI(string email); 
        Task<List<EgsUpgradeAccount>> GetPendingUpgradeRequest();
        Task<EgsUpgradeAccount> GetUpgradeRequestDetails(int reqId);
        Task<EgsWallet> GetExistingUserBVN(string BVN);
        Task<SysUsers> GetSysUserByRole(int roleId);
        Task<SysUsers> GetUserByStateID(int id);
        Task<SysUsers> GetUserByID(int id);
        Task<List<SysUsers>> GetUserActiveSubAgents();
        Task<List<SysUsers>> GetSpecialSubAgents();
        Task<List<SysUsers>> GetSpecialAgents();
        Task<List<SysUsers>> GetActiveAgentsNew();

        Task<List<SysUsers>> GetAllCourtevilleAgents();

        Task<List<SysUsers>> GetAllSystemUsers();
    }
}
