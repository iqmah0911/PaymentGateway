//using PaymentGateway.Repositories;
using PaymentGateway21052021.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Repositories.SysSetup.Interfaces
{
    public interface IIdentificationTypeRepository : IRepository<SysIdentificationType>
    {

        Task<List<SysIdentificationType>> GetAllIdentificationType();
        List<SysIdentificationType> GetAllForDropdown();
        Task<SysIdentificationType> GetIdentificationType(int id);
        List<SysIdentificationType> GetAllForDropdownList();
    }
}
