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
    public class AggregatorRepository : Repository<SysAggregator>, IAggregatorRepository
    {
        ApplicationDBContext _context;
        //IAggregratorRepository
        public AggregatorRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }


        public async Task<List<SysUsers>> GetAllAggregators()
        {
            try
            {
                   var user = await Context.SYS_Users
                                .AsNoTracking()
                                .Include(a => a.Aggregator)
                                .Include(a => a.Role)
                                .Where(u => u.Role.RoleID == 3 && u.IsActive == true)                             
                                .ToListAsync();
                return user;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
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
                                .Where(u => u.Role.RoleID == 3 && u.IsActive == true && u.Aggregator.AggregatorID == Id)
                                .AsNoTracking()
                                .SingleOrDefaultAsync();

                return users;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<SysAggregator> GetAggregatorByCode(string code)
        {
            try
            {
                var users = await Context.SYS_Aggregator
                        .Include(b => b.Bank)
                        .Include(u => u.User)
                        .Where(aggre => aggre.User.Role.RoleID == 3 && aggre.IsActive == true && aggre.AggregatorCode == code)
                        .AsNoTracking()
                        .SingleOrDefaultAsync();


                return users;
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
        
        public async Task<SysUsers> GetAggregatorByAgentID(int agentid)
        {
            try
            {
                var users = await Context.SYS_Users
                        .Include(u => u.Role)
                        .Include(a => a.Aggregator)
                        //.Include(ar => ar.AggregatorRequest)
                        .Where(u => u.Role.RoleID == 3 && u.IsActive == true) //&& u.Aggregator.AggregatorCode == code)
                        .AsNoTracking()
                        .SingleOrDefaultAsync();

                return users;
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


        public async Task<SysAggregator> GetAggregatorByUserId(int Id)
        {
            try
            {
                var agguser = await Context.SYS_Aggregator 
                                .Include(r=>r.User)
                                .Include(r => r.Bank)
                                .Include(u => u.User.Role)
                                .Where(u => u.IsActive == true && u.User.UserID == Id)
                                .AsNoTracking()
                                .FirstOrDefaultAsync();

                return agguser;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<SysUsers> GetAllAgents()
        {
            try
            {
                var user = Context.SYS_Users
                                .AsNoTracking()
                                .Include(u => u.Role)
                                .Include(w => w.Wallet)
                                .Where(u => u.IsActive == true)
                                //.Where(u => u.Role.RoleID == 5 && u.IsActive == true)
                                .ToList();

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
        public async Task<EgsAggregatorRequest> GetAggregatorRequestByReqId(int requestID)
        {
            try
            {
                var pendingReq = await Context.EGS_AggregatorRequest
                    .Include(u => u.Agent)
                    .Include(arge => arge.Aggregator)
                    .Where(agr => agr.IsProcessed == false && agr.RequestID == requestID)
                    .AsNoTracking()
                    .SingleOrDefaultAsync();

                return pendingReq;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




        //public async Task<EgsInvoice> GetAggregatorCommission(int AggID)
        //{
        //    List<EgsInvoice> AggAgentsCmsn = new List<EgsInvoice>();
        //    try
        //    {
                

        //        return pendingReq;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}


    }
}
