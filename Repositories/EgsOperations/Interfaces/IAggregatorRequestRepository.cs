using PaymentGateway21052021.Areas.EgsOperations.Models;
using PaymentGateway21052021.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Repositories.EgsOperations.Interfaces
{
    public interface IAggregatorRequestRepository : IRepository<EgsAggregatorRequest>
    {
        Task<List<SysUsers>> GetAllAggregators();
        Task<List<EgsAggregatorRequest>> GetAllAggregatorRequest();
        Task<List<EgsAggregatorRequest>> GetPendingAggregatorRequest(); 
        //Task<List<EgsAggregatorRequest>> GetProcessedAggregatorRequest();
        Task<List<EgsAggregatorRequest>> GetAggregatorRequestByAggregatorID(int aggregID);
        Task<List<EgsAggregatorRequest>> GetAggregatorRequestAcknowledged();
        Task<EgsAggregatorRequest> GetAggregatorRequestAcknowledgedByReqId(int requestID);
        Task<List<EgsAggregatorRequest>> GetProcessedAggregatorRequest(int aggregID);
        //For other repos
        Task<SysAggregator> GetAggregatorFromTblById(int Id);
        Task<List<EgsAggregatorRequest>> GetAgentstiedtoAggregator(int aggregatorID);
        Task<SysUsers> GetAggregatorById(int Id);
        //Task<SysAggregator> GetAggregatorByCode(string code);
        Task<List<SysAggregator>> GetAggregatorByCode(string code);
        Task<List<SysUsers>> GetAgentsByAggregatorID(int aggreID);
        // Task<SysUsers> GetAggregatorByAgentID(int agentid);
        Task<EgsAggregatorRequest> GetAggregatorByAgentID(int agentid);


        Task<SysUsers> GetAgentById(int Id);
        Task<List<SysUsers>> GetAllAgents();
        Task<List<EgsAggregatorRequest>> GetPendingRequestByAggregator(int aggregatorID);
        Task<EgsAggregatorRequest> GetAggregatorRequestByReqId(int requestID);
        Task<EgsAggregatorRequest> GetProcessedRequestByReqId(int requestID);
        Task<SysAggregator> GetAggregatorByUserId(int Id);
        Task<EgsAggregatorRequest> GetUserTiedtoAggregator(int Id);
        Task RemoveRequest(EgsAggregatorRequest entity);
        Task<List<EgsAggregatorRequest>> GetAggregatorAgents(int aggregID);

    }
}
