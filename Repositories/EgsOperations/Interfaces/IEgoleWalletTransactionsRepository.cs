﻿using PaymentGateway21052021.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Repositories.EgsOperations.Interfaces
{ 
     public interface IEgoleWalletTransactionsRepository : IRepository<EgsEgoleWalletTransactions>
    {
        Task<EgsEgoleWalletTransactions> GetTransaction(int id);
    }
}
