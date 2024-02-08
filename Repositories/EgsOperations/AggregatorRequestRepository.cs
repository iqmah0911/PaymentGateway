using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaymentGateway21052021.Areas.EgsOperations.Models;
using PaymentGateway21052021.Data;
using PaymentGateway21052021.Models;
using PaymentGateway21052021.Repositories.EgsOperations.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Repositories.EgsOperations
{

    public class AggregatorRequestRepository : Repository<EgsAggregatorRequest>, IAggregatorRequestRepository
    {
        ApplicationDBContext _context;

        public AggregatorRequestRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<SysUsers>> GetAllAggregators()
        {
            try
            {
                var user = await Context.SYS_Users
                                .Include(u => u.Role)
                                .Include(a => a.Aggregator)
                                .Where(u => u.Role.RoleID == 3 && u.IsActive == true)
                                .AsNoTracking()
                                .ToListAsync();
                return user;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<EgsAggregatorRequest>> GetAllAggregatorRequest()
        {
            try
            {
                var users = await Context.EGS_AggregatorRequest
                        .AsNoTracking()
                        .ToListAsync();

                return users;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<EgsAggregatorRequest>> GetPendingAggregatorRequest()
        {
            try
            {
                var users = await Context.EGS_AggregatorRequest
                         .Include(r => r.Agent)
                         .Include(r => r.Agent.Role)
                         .Include(u => u.Aggregator)
                        .Where(u => u.IsProcessed == false)
                        .AsNoTracking()
                        .ToListAsync();

                return users;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<EgsAggregatorRequest>> GetProcessedAggregatorRequest(int aggregID)
        {
            try
            {
                List<EgsAggregatorRequest> Aggre = new List<EgsAggregatorRequest>();

                if (aggregID != 0)
                {
                    Aggre = await Context.EGS_AggregatorRequest
                        .Include(r => r.Agent)
                        .Include(w => w.Agent.Wallet)
                        .Include(r => r.Aggregator)
                        .Where(u => u.IsProcessed == true && u.Aggregator.AggregatorID == aggregID)
                        .AsNoTracking()
                        .ToListAsync();
                }
                else
                {
                    Aggre = await Context.EGS_AggregatorRequest
                        .Include(r => r.Agent)
                        .Include(w => w.Agent.Wallet)
                        .Include(r => r.Aggregator)
                        .Where(u => u.IsProcessed == true)
                        .AsNoTracking()
                        .ToListAsync();
                }
                return Aggre;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public async Task<List<EgsAggregatorRequest>> GetAggregatorRequestByAggregatorID(int aggregID)
        {
            try
            {
                var users = await Context.EGS_AggregatorRequest
                        .Include(arg => arg.Aggregator)
                        .Include(ag=>ag.Agent)
                        .Where(u => u.Aggregator.AggregatorID == aggregID && u.IsProcessed==false)
                        .AsNoTracking()
                        .ToListAsync();

                return users;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<EgsAggregatorRequest>> GetAggregatorRequestAcknowledged()
        {
            try
            {
                var users = await Context.EGS_AggregatorRequest
                        .Include(arg => arg.Aggregator)
                        .Include(ag => ag.Agent)
                        .Where(u => u.ApprovedBy2 == 0 && u.ApprovedBy != 0 && u.RejectedBy == 0)
                        .AsNoTracking()
                        .ToListAsync();

                return users;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<EgsAggregatorRequest> GetAggregatorRequestAcknowledgedByReqId(int requestID)
        {
            try
            {
                var pendingReq = await Context.EGS_AggregatorRequest
                    .Include(u => u.Agent)
                    .Include(arge => arge.Aggregator)
                    .Where(agr => agr.ApprovedBy2 == 0 && agr.ApprovedBy != 0 && agr.RejectedBy == 0 && agr.RequestID == requestID)
                    .AsNoTracking()
                    .FirstOrDefaultAsync();

                return pendingReq;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /***This following methods were supposed to be at the Aggregator Repository but due to issues of not being able to access it..
        was moved to this repo **/

        public async Task<SysAggregator> GetAggregatorFromTblById(int Id)
        {
            try
            {

                var users = await Context.SYS_Aggregator
                                .Include(u => u.User)
                                .Where(u => u.IsActive == true && u.AggregatorID == Id)
                                .AsNoTracking()
                                .SingleOrDefaultAsync();

                return users;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<SysUsers> GetAggregatorById(int Id)
        {
            try
            {
                var users = await Context.SYS_Users
                                .Include(u => u.Role)
                                .Include(a => a.Aggregator)
                                .Where(u => u.Role.RoleID == 3 && u.IsActive == true && u.UserID == Id)
                                .AsNoTracking()
                                .SingleOrDefaultAsync();

                return users;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<SysAggregator>> GetAggregatorByCode(string code)
        {
            try
            {      //aggre.User.Role.RoleID == 3 &&
                var users = await Context.SYS_Aggregator
                        .Include(u => u.User)
                        .Include(r => r.User.Role)
                        .Include(b => b.Bank)
                        .Where(aggre => aggre.IsActive == true && aggre.AggregatorCode == code)
                        .AsNoTracking()
                        .ToListAsync();
                        //.SingleOrDefaultAsync();


                return users;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<SysAggregator> GetAggregatorByUserId(int Id)
        {
            try
            {
                var agguser = await Context.SYS_Aggregator
                                .Include(r => r.User)
                                .Include(r=>r.Bank)
                                .Include(u=>u.User.Role)
                                .Where(u => u.IsActive == true && u.User.UserID == Id)
                                .AsNoTracking()
                                .SingleOrDefaultAsync();

                return agguser;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<EgsAggregatorRequest> GetUserTiedtoAggregator(int Id)
        {
            try
            {
                var agentcheck = await Context.EGS_AggregatorRequest  
                    .Include(a=>a.Agent)
                                .Where(u => u.IsProcessed==true && u.ApprovedBy!=0 && u.Agent.UserID==Id)
                                .AsNoTracking()
                                .FirstOrDefaultAsync();

                return agentcheck;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<SysUsers>> GetAgentsByAggregatorID(int aggreID)
        {
            try
            {
                var users = await Context.SYS_Users
                        .Include(u => u.Role)
                        .Include(a => a.Aggregator)
                        //.Include(ar => ar.AggregatorRequest)
                        .Where(u => u.Role.RoleID == 3 && u.IsActive == true) //&& u.Aggregator.AggregatorCode == code)
                        .AsNoTracking()
                        .ToListAsync();

                return users;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        
        public async Task<EgsAggregatorRequest> GetAggregatorByAgentID(int agentid)
        {
            try
            {
                var detailsReq = await Context.EGS_AggregatorRequest
                    .Include(u => u.Agent)
                    .Include(arge => arge.Aggregator)
                    .Where(agr => agr.Agent.UserID == agentid)
                    .AsNoTracking()
                    .SingleOrDefaultAsync();


                return detailsReq;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<SysUsers> GetAgentById(int Id)
        {
            try
            {
                var users = await Context.SYS_Users
                                .Include(u => u.Role)
                                .Include(w => w.Wallet)
                                .Where(u => u.Role.RoleID == 5 && u.IsActive == true && u.UserID == Id)
                                .AsNoTracking()
                                .SingleOrDefaultAsync();

                return users;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<SysUsers>> GetAllAgents()
        {
            try
            {
                var user = await Context.SYS_Users
                                .AsNoTracking()
                                .Include(u => u.Role)
                                .Include(w => w.Wallet)
                                .Where(u => u.Role.RoleID == 5 && u.IsActive == true)
                                .ToListAsync();

                return user;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<EgsAggregatorRequest>> GetPendingRequestByAggregator(int aggregatorID)
        {
            try
            {
                var pendingReq = await Context.EGS_AggregatorRequest
                    .Include(u => u.Agent)
                    .Include(arge => arge.Aggregator)
                    .Where(agr => agr.IsProcessed == false && agr.Aggregator.AggregatorID == aggregatorID)
                    .AsNoTracking()
                    .ToListAsync();

                return pendingReq;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<List<EgsAggregatorRequest>> GetAgentstiedtoAggregator(int aggregatorID)
        {
            List<EgsAggregatorRequest> aggagents = new List<EgsAggregatorRequest>();
            try
            {
                aggagents = await Context.EGS_AggregatorRequest
                    .Include(u => u.Agent)
                    .Include(arge => arge.Aggregator)
                    .Where(agr =>   agr.Aggregator.AggregatorID == aggregatorID)
                    .AsNoTracking()
                    .ToListAsync();

                return aggagents;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<EgsAggregatorRequest> GetAggregatorRequestByReqId(int requestID)
        {
            try
            {
                var pendingReq = await Context.EGS_AggregatorRequest
                    .Include(u => u.Agent)
                    .Include(arge => arge.Aggregator)
                    .Where(agr => agr.IsProcessed == false && agr.RequestID == requestID)
                    .AsNoTracking()
                    .FirstOrDefaultAsync();

                return pendingReq;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<EgsAggregatorRequest> GetProcessedRequestByReqId(int requestID)
        {
            try
            {
                var pendingReq = await Context.EGS_AggregatorRequest
                    .Include(u => u.Agent)
                    .Include(arge => arge.Aggregator)
                    .Where(agr => agr.IsProcessed == true && agr.RequestID == requestID)
                    .AsNoTracking()
                    .SingleOrDefaultAsync();

                return pendingReq;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task RemoveRequest(EgsAggregatorRequest entity)
        {
            try
            { 
                _context.Entry(entity).State = EntityState.Deleted;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

            }
        }


        public async Task<List<EgsAggregatorRequest>> GetAggregatorAgents(int aggregID)
        {
            try
            {
                List<EgsAggregatorRequest> Aggre = new List<EgsAggregatorRequest>();

                if (aggregID != 0)
                {
                    Aggre = await Context.EGS_AggregatorRequest
                        .Include(r => r.Agent)
                        .Include(w => w.Agent.Wallet)
                        .Include(r => r.Aggregator)
                        .Where(u => u.IsProcessed == true && u.Aggregator.AggregatorID == aggregID)
                        .AsNoTracking()
                        .ToListAsync();
                }
                return Aggre;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
