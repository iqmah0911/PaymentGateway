using PaymentGateway21052021.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace PaymentGateway21052021.Repositories.SysSetup.Interfaces
{
    public interface IAgentMappingRepository  : IRepository<SysAgentMapping>
    {

        Task<List<SysAgentMapping>> FetchAgentByAggregatorID(int id);
        Task<List<SysAgentMapping>> FetchAllAgents();
        Task<List<SysUsers>> GetAggregators();
        Task<List<SysAgentMapping>> FetchMappedAgents();
    }
}
