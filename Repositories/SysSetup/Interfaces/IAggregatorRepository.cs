using PaymentGateway21052021.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Repositories.SysSetup.Interfaces
{
    public interface IAggregatorRepository : IRepository<SysAggregator>
    {
        Task<List<SysUsers>> GetAllAggregators();
        Task<SysUsers> GetAggregatorById(int Id);
        Task<SysAggregator> GetAggregatorFromTblById(int Id);
        Task<SysAggregator> GetAggregatorByCode(string code);
        Task<List<SysUsers>> GetAgentsByAggregatorID(int aggreID);
        Task<SysUsers> GetAggregatorByAgentID(int agentid);
        List<SysUsers> GetAllAgents();
        Task<SysUsers> GetAgentById(int Id);
        Task<EgsAggregatorRequest> GetAggregatorRequestByReqId(int requestID);
        Task<List<EgsAggregatorRequest>> GetPendingRequestByAggregator(int aggregatorID);
        Task<SysAggregator> GetAggregatorByUserId(int Id);
    }
}
