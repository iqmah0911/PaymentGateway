using PaymentGateway21052021.Data;
using PaymentGateway21052021.Models;
using PaymentGateway21052021.Repositories.SysSetup.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Repositories.SysSetup
{
    public class RequestResponseLogRepository : Repository<SysRequestResponseLog>, IRequestResponseLogRepository
    {
        ApplicationDBContext _context;

        public RequestResponseLogRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }

      
    }
}
