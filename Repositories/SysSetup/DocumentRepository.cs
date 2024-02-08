using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PaymentGateway21052021.Data;
using PaymentGateway21052021.Models;
using PaymentGateway21052021.Repositories.SysSetup.Interfaces;

namespace PaymentGateway21052021.Repositories.SysSetup
{
    public class DocumentRepository : Repository<SysDocument>, IDocumentRepository
    {

        ApplicationDBContext _context;

        public DocumentRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }

        public async Task<SysDocument> GetDocumentById(int id)
        {
            try
            {
                var DocumentDetails = await Context.SYS_Document.Where(p => p.DocumentID == id)
                            //.AsNoTracking()
                            .Include(u => u.Role)
                            .SingleOrDefaultAsync();

                return DocumentDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<SysDocument>> GetAllDocuments()
        {
            try
            {
                var allDocument = await Context.SYS_Document
                            .Include(u => u.Role)
                            .AsNoTracking()
                            .ToListAsync();

                return allDocument;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<SysDocument>> GetDocumentByRole(int id)
        {
            try
            {
                var documentsList = await Context.SYS_Document.Where(p => p.Role.RoleID == id)
                            .Include(u => u.Role)
                            .AsNoTracking()
                            .ToListAsync();

                return documentsList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<SysDocument> GetByRoleID(int id)
        {
            try
            {
                var documentsList = await Context.SYS_Document.Where(p => p.DocumentID == id)
                            .Include(u => u.Role)
                            .AsNoTracking()
                            .SingleOrDefaultAsync();

                return documentsList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<SysDocument> GetDocumentByName(string name)
        {
            try
            {
                var documentsDetails = await Context.SYS_Document.Where(p => p.DocumentName== name) 
                            .AsNoTracking()
                            .SingleOrDefaultAsync();

                return documentsDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


     
         








    }
}
