using PaymentGateway21052021.Models;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Repositories.SysSetup.Interfaces
{
    public interface IWebhooklogsRepository : IRepository<EgsWebhooklogs>
    {
        Task<EgsWebhooklogs> GetWebhookByID(int id);
    }
}
