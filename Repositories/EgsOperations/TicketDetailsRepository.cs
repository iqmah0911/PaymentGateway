using Microsoft.EntityFrameworkCore;
using PaymentGateway21052021.Data;
using PaymentGateway21052021.Models;
using PaymentGateway21052021.Repositories.EgsOperations.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Repositories.EgsOperations
{

    public class TicketDetailsRepository : Repository<EgsTicketDetails>, ITicketDetailsRepository
    {
        ApplicationDBContext _context;

        public TicketDetailsRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }



        public async Task<List<EgsTicketDetails>> FetchAllTicketsByDateCreated(DateTime From, DateTime To)
        {

            try
            {
                var ticketData = await Context.EGS_TicketDetails.Where(x => x.DateCreated >= From && x.DateCreated <= To)
                    .Include(r => r.Tickets)
                    .ToListAsync();


                return ticketData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<EgsTicketDetails> GetTicketByFeedbackID(int UserID)
        {

            try
            {
                var ticketData = await Context.EGS_TicketDetails.Where(x => x.FeedbackBy == UserID).Include(r => r.Tickets).FirstOrDefaultAsync();


                return ticketData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<EgsTicketDetails> GetDetailsByTicketID(int ID)
        {

            try
            {
                var ticketData = await Context.EGS_TicketDetails.Where(x => x.Tickets.TicketID == ID)
                    .Include(r => r.Tickets).FirstOrDefaultAsync();


                return ticketData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<EgsTicketDetails>> GetDetailsByTicketNo(string RefNo, string wallet)
        {

            try
            {
                var ticketData = new List<EgsTicketDetails>();
                if (wallet == null || wallet == string.Empty)
                {
                    ticketData = await Context.EGS_TicketDetails.Where(x => x.Tickets.ReferenceNo == RefNo)
                        .Include(r => r.Tickets)
                        .Include(td => td.Tickets.Wallet)
                        .ToListAsync();
                }
                else
                {
                    ticketData = await Context.EGS_TicketDetails.Where(x => x.Tickets.ReferenceNo == RefNo && x.IsOfficial == false)
                        .Include(r => r.Tickets)
                        .Include(td => td.Tickets.Wallet)
                        .ToListAsync();
                }



                return ticketData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<EgsTicketDetails> GetLastDetailsByTicketNo(string RefNo, string wallet)
        {

            try
            {
                var ticketData = new EgsTicketDetails();
                if (wallet == null || wallet == string.Empty)
                {
                    ticketData = await Context.EGS_TicketDetails.Where(x => x.Tickets.ReferenceNo == RefNo)
                        .Include(r => r.Tickets)
                        .Include(td => td.Tickets.Wallet)
                        .MaxAsync();
                }
                else
                {
                    ticketData = await Context.EGS_TicketDetails.Where(x => x.Tickets.ReferenceNo == RefNo && x.IsOfficial == false)
                        .Include(r => r.Tickets)
                        .Include(td => td.Tickets.Wallet)
                        .MaxAsync();
                }



                return ticketData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
