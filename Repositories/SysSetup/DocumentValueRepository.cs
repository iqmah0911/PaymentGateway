using Microsoft.EntityFrameworkCore;
using PaymentGateway21052021.Data;
using PaymentGateway21052021.Models;
using PaymentGateway21052021.Repositories.EgsOperations.Interfaces;
using PaymentGateway21052021.Repositories.SysSetup.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Repositories.SysSetup
{
    public class DocumentValueRepository : Repository<EgsDocumentValue>, IDocumentValueRepository
    {
        ApplicationDBContext _context;

        public DocumentValueRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }


        public async Task<List<EgsDocumentValue>> GetDocumentsbykycID(int id)
        {
            try
            {
                var userdocuments = await Context.EGS_DocumentValue.Where(x=>x.KycInfo.UserKYCID == id) 
                    .Include(r=>r.Document)
                    .Include(r=>r.KycInfo)
                            .ToListAsync();

                return userdocuments;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



    }
}
