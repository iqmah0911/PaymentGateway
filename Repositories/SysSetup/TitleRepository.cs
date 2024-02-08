//using PaymentGateway.Repositories;
using PaymentGateway21052021.Data;
using PaymentGateway21052021.Models;
using PaymentGateway21052021.Repositories.SysSetup.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Repositories.SysSetup
{
    public class TitleRepository : Repository<SysTitle>, ITitleRepository
    {
        //ApplicationDBContext _context;

        public TitleRepository(ApplicationDBContext context) : base(context)
        {
            //_context = context;

        }



        public List<SysTitle> GetAllForDropdown()
        {
            return Context.Set<SysTitle>().ToList();
        }
    
    }
}
