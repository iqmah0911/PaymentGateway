using PaymentGateway21052021.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Repositories.EgsOperations.Interfaces
{
    public interface ITicketRepository : IRepository<EgsTickets>
    {

        Task<EgsTickets> GetTicketByTicketID(int TicketID);

        Task<EgsTickets> FetchTicketByRefNo(string RefNo);

        Task<List<EgsTickets>> FetchTicketByWalletID(int WalletID, DateTime From, DateTime To);

        Task<List<EgsTickets>> FetchTicketByReceiverID(int userID, DateTime From, DateTime To);

        Task<List<EgsTickets>> FetchTicketsTreatedByID(int userID, DateTime From, DateTime To);
        Task<List<EgsTickets>> FetchAllTicketsByDateCreated(DateTime From, DateTime To);
    }
}
