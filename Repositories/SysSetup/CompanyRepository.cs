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
    public class CompanyRepository : Repository<SysCompany>, ICompanyRepository
    {
        ApplicationDBContext _context;

        public CompanyRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }

        public async Task<SysCompany> GetCompanyDetail(int id)
        {
            try
            {
                var companyDetails = await Context.SYS_Company.Where(p => p.CompanyID == id)
                            //.Include(a => a.GLAAccount)
                            .AsNoTracking()
                            .SingleOrDefaultAsync();

                return companyDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<SysCompany>> GetCompanys()
        {
            try
            {
                var companyDetails = await Context.SYS_Company
                            //.Include(a => a.GLAAccount)
                            .AsNoTracking()
                            .ToListAsync();

                return companyDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
