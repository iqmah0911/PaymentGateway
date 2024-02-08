using PaymentGateway21052021.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Repositories.SysSetup.Interfaces
{
    public interface IUserKYCInfoRepository: IRepository<SysUserKycInfo>
    {
        Task<SysUserKycInfo> GetUserKYCInfo(int id);
        Task<SysUserKycInfo> GetUserKYCInfoByUserID(int id);
        Task<SysUserKycInfo> GetIdentificationTypeById(int id);
        Task<List<SysUserKycInfo>> GetAllUserKYCInfo();
    }
}
