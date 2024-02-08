//using PaymentGateway.Repositories;
using Microsoft.EntityFrameworkCore;
using PaymentGateway21052021.Data;
using PaymentGateway21052021.Models;
using PaymentGateway21052021.Repositories.SysSetup.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Repositories.SysSetup
{
    public class TransactionTypeRepository : Repository<SysTransactionType>, ITransactionTypeRepository
    {
        ApplicationDBContext _context;
       
        public TransactionTypeRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }


        public async Task<SysTransactionType> GetTransactionTypeByID(int id)
        {
            try
            {
                var Transactiontype = await Context.SYS_TransactionType
                    .Where(p => p.TransactionTypeID == id).FirstOrDefaultAsync();

                return Transactiontype;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




    }
}
