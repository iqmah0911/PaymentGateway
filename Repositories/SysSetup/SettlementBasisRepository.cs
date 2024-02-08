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
    public class SettlementBasisRepository : Repository<EgsSettlementBasis>, ISettlementBasisRepository
    {
        ApplicationDBContext _context;

        public SettlementBasisRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }


        public async Task<EgsSettlementBasis> GetSettlementByID(int id)
        {
            try
            {
                var settlementDetails = await Context.EGS_SettlementBasis.Where(p => p.SettlementBasisID == id)
                            //.AsNoTracking()
                            .SingleOrDefaultAsync();

                return settlementDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<EgsSettlementBasis> GetSettlementBasisByProductID(int id)
        {
            try
            {
                var settlementDetails = await Context.EGS_SettlementBasis.Where(p => p.ProductID == id)
                            //.AsNoTracking()
                            .FirstOrDefaultAsync();

                return settlementDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<EgsSettlementBasis> GetSettlementBasisByProductItemID(int productid,int merchantid, int itemid)
        {
            try
            {
                var settlementDetails = await Context.EGS_SettlementBasis.Where(p => p.ProductItemID == itemid && p.ProductID == productid && p.MerchantID == merchantid)
                            //.AsNoTracking()
                            .FirstOrDefaultAsync();

                return settlementDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //public async Task<EgsSettlementBasis> FetchMerchantMappingByMerchantID(int id)
        //{
        //    try
        //    {
        //        var MerchantMappingDetails = await Context.SYS_MerchantMapping.Where(p => p.MerchantMappingID == id)
        //                    //.AsNoTracking()
        //                    .SingleOrDefaultAsync();

        //        return MerchantMappingDetails;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}










    }
}
