using PaymentGateway21052021.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Repositories.SysSetup.Interfaces
{
   public interface ISysUserRepository: IRepository<SysUsers>
    {
        void UpdateUser(SysUsers entity);
        Task<SysUsers> GetUserToBeIssuedhWallet(SysUsers userDet);

        Task<SysUsers> GetUserToBeIssuedhWalletX(int userID);
        Task<SysUsers> GetUserProfile(int id);
        Task<SysUsers> GetEgoleUserToUpdate(int Id);
        //Task<SysUserKycInfo> GetUserKYCInfo(int id);
    }
}
