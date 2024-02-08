using Microsoft.EntityFrameworkCore;
using PaymentGateway21052021.Data;
using PaymentGateway21052021.Models;
using PaymentGateway21052021.Repositories.EgsOperations.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Repositories.EgsOperations
{
    public class EgoleWalletTransactionsRepository : Repository<EgsEgoleWalletTransactions>, IEgoleWalletTransactionsRepository
    {
        ApplicationDBContext _context;

        public EgoleWalletTransactionsRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }


        public async Task<EgsEgoleWalletTransactions> GetTransaction(int id)
        {
            try
            {
                var TransactionDetails = await Context.EGS_EgoleWalletTransactions.Where(p => p.TransactionID == id)
                            .AsNoTracking()
                            .SingleOrDefaultAsync();

                return TransactionDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
