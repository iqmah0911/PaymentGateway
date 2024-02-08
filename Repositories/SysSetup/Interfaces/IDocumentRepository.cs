using PaymentGateway21052021.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Repositories.SysSetup.Interfaces
{
    public interface IDocumentRepository : IRepository<SysDocument>
    {
        Task<List<SysDocument>> GetAllDocuments();

        Task<SysDocument> GetDocumentById(int id);

        Task<List<SysDocument>> GetDocumentByRole(int id);
        Task<SysDocument> GetDocumentByName(string name);
        Task<SysDocument> GetByRoleID(int id);


    }

}
