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
    public class UpgradeAccountRepository : Repository<EgsUpgradeAccount>, IUpgradeAccountRepository
    {
        ApplicationDBContext _context;

        public UpgradeAccountRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }


        public async Task<EgsUpgradeAccount> GetRequestByUserId(int id)
        {
            try
            {
                var requestDetails = await Context.EGS_UpgradeAccount
                            .Where(p => p.User.UserID == id && p.IsProcessed==false && p.RejectedBy==0) 
                            .SingleOrDefaultAsync();

                return requestDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<EgsUpgradeAccount> GetRequestById(int id)
        {
            try
            {
                var requestDetails = await Context.EGS_UpgradeAccount.Where(p => p.UpgradeAccountID == id)
                    .Include(r=>r.User)
                    //.Include(r => r.RoleRequest)
                    .SingleOrDefaultAsync();

                return requestDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




        public async Task<IEnumerable<EgsUpgradeAccount>> GetAllRequests()
        {
            //IEnumerable<EgsUpgradeAccount>  
            try
            {
                // var request = await Context.EGS_UpgradeAccount.Where(x=>x.IsProcessed!=false && x.RoleRequest.RoleID==3)
                var request = await Context.EGS_UpgradeAccount.Where(x => x.IsProcessed == false ) 
                    .Include(x => x.User)
                     //.Include(x=>x.RoleRequest)
                    .ToListAsync();

                return request;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public async Task<IEnumerable<EgsUpgradeAccount>> GetAllAgentRequest()
        //{
        //    //IEnumerable<EgsUpgradeAccount>  
        //    try
        //    {
        //        // var request = await Context.EGS_UpgradeAccount.Where(x => x.IsProcessed != false && x.RoleRequest.RoleID == 5)
        //        var request = await Context.EGS_UpgradeAccount.Where(x => x.IsProcessed != false && x.RoleRequestID == 5) 
        //                 .Include(x => x.User)
        //             //.Include(x => x.RoleRequest)
        //            .ToListAsync();

        //        return request;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}



    }
}
