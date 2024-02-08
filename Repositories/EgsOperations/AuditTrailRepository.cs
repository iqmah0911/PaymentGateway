using PaymentGateway21052021.Data;
using PaymentGateway21052021.Models;
using PaymentGateway21052021.Repositories.EgsOperations.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Repositories.EgsOperations
{
    public class AuditTrailRepository : Repository<EgsAuditTrail>, IAuditTrailRepository
    {
        ApplicationDBContext _context;

        public AuditTrailRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }

    }
}
