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
    public class StateRepository : Repository<SysState>, IStateRepository
    {
        ApplicationDBContext _context;

        public StateRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<SysState>> GetAllStates()
        {
            try
            {
                var allStates = await Context.SYS_States
                           // .AsNoTracking()
                            .ToListAsync();

                return allStates;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<SysState> GetStateByID(int id)
        {
            try
            {
                var State = await Context.SYS_States.Where(x=>x.StateID == id)
                            .AsNoTracking()
                            .FirstOrDefaultAsync();

                return State;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<SysState> GetAllForDropdown()
        {
            return Context.SYS_States.ToList();
        }
    }
}
