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
    public class IdentificationTypeRepository : Repository<SysIdentificationType>, IIdentificationTypeRepository
    {
        ApplicationDBContext _context;

        public IdentificationTypeRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }
        //IdentificationType



        public async Task<SysIdentificationType> GetIdentificationType(int id)
        {
            try
            {
                var IdentificationTypeDetails = await Context.SYS_IdentificationType.Where(p => p.IdentificationTypeID == id)
                            //.AsNoTracking()
                            .SingleOrDefaultAsync();

                return IdentificationTypeDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<SysIdentificationType>> GetAllIdentificationType()
        {
            try
            {
                var allIdentificationType = await Context.SYS_IdentificationType
                            .AsNoTracking()
                            .ToListAsync();

                return allIdentificationType;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public List<SysIdentificationType> GetAllForDropdown()
        {
            return Context.SYS_IdentificationType.ToList();
        }


        public List<SysIdentificationType> GetAllForDropdownList()
        {
            return Context.SYS_IdentificationType.ToList();
        }

    }
}
