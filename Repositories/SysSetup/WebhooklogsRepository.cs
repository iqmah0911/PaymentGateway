using Microsoft.EntityFrameworkCore;
using PaymentGateway21052021.Data;
using PaymentGateway21052021.Models;
using PaymentGateway21052021.Repositories.SysSetup.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Repositories.SysSetup
{
    public class WebhooklogsRepository : Repository<EgsWebhooklogs>, IWebhooklogsRepository
    {
        private readonly ApplicationDBContext _context;

        public WebhooklogsRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }

        public async Task<EgsWebhooklogs> GetWebhookByID(int id)
        {
            var webhooklogs = await Context.EGS_Webhooklogs.Where(x => x.webHooklogsID == id)
                                                     .AsNoTracking()
                                                     .FirstOrDefaultAsync();

            return webhooklogs;
        }
    }
}
