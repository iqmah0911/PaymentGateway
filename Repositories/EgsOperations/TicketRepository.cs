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
    public class TicketRepository : Repository<EgsTickets>, ITicketRepository
    {
        ApplicationDBContext _context;

        public TicketRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }


        public async Task<EgsTickets> GetTicketByTicketID(int TicketID)
        {

            try
            {
                var ticketData = await Context.EGS_Tickets.Where(x => x.TicketID == TicketID).Include(r => r.Wallet).FirstOrDefaultAsync();


                return ticketData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<EgsTickets> FetchTicketByRefNo(string RefNo)
        {

            try
            {
                var ticketData = await Context.EGS_Tickets.Where(x => x.ReferenceNo == RefNo).Include(r => r.Wallet).FirstOrDefaultAsync();


                return ticketData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<EgsTickets>> FetchTicketByWalletID(int WalletID, DateTime From, DateTime To)
        {

            try
            {
                var ticketData = await Context.EGS_Tickets
                    .Where(x => x.Wallet.WalletID == WalletID && x.DateCreated.Date >= From.Date && x.DateCreated.Date <= To.Date)
                    .Include(r => r.Wallet)
                    .ToListAsync();


                return ticketData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<List<EgsTickets>> FetchTicketByReceiverID(int userID, DateTime From, DateTime To)
        {

            try
            {
                var ticketData = await Context.EGS_Tickets.Where(x => x.ReceivedBy == userID && x.DateCreated.Date >= From.Date && x.DateCreated.Date <= To.Date).Include(r => r.Wallet).ToListAsync();


                return ticketData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<List<EgsTickets>> FetchTicketsTreatedByID(int userID, DateTime From, DateTime To)
        {

            try
            {
                var ticketData = await Context.EGS_Tickets.Where(x => x.TreatedBy == userID && x.DateCreated.Date >= From.Date && x.DateCreated.Date <= To.Date).Include(r => r.Wallet).ToListAsync();


                return ticketData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public async Task<List<EgsTickets>> FetchAllTicketsByDateCreated(DateTime From, DateTime To)
        {

            try
            {
                var ticketData = await Context.EGS_Tickets.Where(x => x.DateCreated.Date >= From.Date && x.DateCreated.Date <= To.Date)
                    .Include(r => r.Wallet)
                    .ToListAsync();


                return ticketData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }





    }
}
