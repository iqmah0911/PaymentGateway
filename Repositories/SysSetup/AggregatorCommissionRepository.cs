using Microsoft.EntityFrameworkCore;
using PaymentGateway21052021.Areas.Dashboard.Models;
using PaymentGateway21052021.Data;
using PaymentGateway21052021.Helpers;
using PaymentGateway21052021.Models;
using PaymentGateway21052021.Repositories.SysSetup.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Repositories.SysSetup
{
    public class AggregatorCommissionRepository : Repository<EgsAggregatorCommission>, IAggregatorCommissionRepository
    {
        ApplicationDBContext _context;

        public AggregatorCommissionRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }


        public async Task<HoldDisplayAggViewModel> GetAggregatorMonthlyCommision(int aggid)
        {
            try
            {
                DateTime MyDateTime = new DateTime();
                //MyDateTime = DateTime.Now;
                var invoices = await GeneralSqlClient.RPT_AggregatorCommission(MyDateTime, DateTime.Now, aggid.ToString(), "Aggregator Monthly Commision");
                
                List<AggregatorCommsnVModel> agentsmodel = new List<AggregatorCommsnVModel>();
                double baseamounts = 0.00;
                var iquery = invoices.GroupBy(g => new//.Where(p => p.IsAggregatorSettled == false).GroupBy(g => new
                {
                    payDate = g.CommissionPeriod,
                    outlet = g.Business,
                }).Select(group => new
                {
                    business = group.Key.outlet,
                    PayDate = group.Key.payDate,
                    Amount = group.Sum(a => a.CommissionAmount),
                    //ItemCount = group.Count(),
                });

                foreach (var invoice in iquery.OrderByDescending(x => x.PayDate).Take(12))
                {
                    agentsmodel.Add(new AggregatorCommsnVModel
                    {
                        Business = invoice.business.ToString(),
                        monthname = invoice.PayDate.ToString(),
                        Amount = invoice.Amount,
                    });
                    //baseamounts += invoice.Amount;
                }

                //var result = from r in Context.EGS_AggregatorCommission.Where(x => x.AggregatorID == aggid)
                //             group r by r.DateCreated.Month
                //              into g
                //             select new AggregatorCommsnVModel() { SettlementDate = g.Key, Amount = g.Sum(p => p.SplittingRate) };

                var nmodel = new HoldDisplayAggViewModel()
                {
                    HoldAllCommsn = agentsmodel
                };
                return nmodel;


                //var user = await Context.SYS_Users
                //             .AsNoTracking()
                //             .Include(a => a.Aggregator)
                //             .Include(a => a.Role)
                //             .Where(u => u.Role.RoleID == 3 && u.IsActive == true)
                //             .ToListAsync();
                //return user;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
