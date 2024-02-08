using PaymentGateway21052021.Data;
using PaymentGateway21052021.Models;
using PaymentGateway21052021.Repositories.EgsOperations.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Repositories.EgsOperations
{
    public class InvoiceModeRepository : Repository<EgsInvoiceMode>, IInvoiceModeRepository
    {
        ApplicationDBContext _context;

        public InvoiceModeRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }

        public List<EgsInvoiceMode> GetAllForDropdown()
        {
            return Context.EGS_InvoiceMode.ToList();
        }
    }
}
