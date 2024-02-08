//using PaymentGateway.Data;
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
    public class AgentMappingRepository : Repository<SysAgentMapping>, IAgentMappingRepository
    {

        ApplicationDBContext _context;

        public AgentMappingRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<SysAgentMapping>> FetchAgentByAggregatorID(int id)
        {
            try
            {
                var AgentDetails = await Context.SYS_AgentMapping
                          .Include(r => r.Aggregator)
                           .ThenInclude(r => r.User)
                           .Where(r => r.Aggregator.AggregatorID==id)
                           .ToListAsync();

                return AgentDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<SysAgentMapping>> FetchAllAgents()
        {
            try
            {
                var AgentDetails = await Context.SYS_AgentMapping  
                           .Include(r => r.Aggregator)
                           .ThenInclude(r => r.User)
                           .ToListAsync();

                return AgentDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<SysAgentMapping>> FetchMappedAgents()
        {
            try
            {
                var AgentDetails = await Context.SYS_AgentMapping
                           .Include(r => r.Aggregator)
                           .AsNoTracking()
                            .ToListAsync();

                return AgentDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<List<SysUsers>> GetAggregators()
        {
            try
            {
                var users = await Context.SYS_Users
                                 .Include(u => u.Role)
                                 .Include(u => u.Aggregator)
                                 .Where(u => u.IsActive == true && u.Aggregator.AggregatorID !=0 && u.Role.RoleID == 3)
                                 .ToListAsync();

                return users;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
