using PaymentGateway21052021.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Repositories.EgsOperations.Interfaces
{
    public interface ITicketDetailsRepository : IRepository<EgsTicketDetails>
    {

        Task<List<EgsTicketDetails>> FetchAllTicketsByDateCreated(DateTime From, DateTime To);

        Task<EgsTicketDetails> GetTicketByFeedbackID(int UserID);

        Task<EgsTicketDetails> GetDetailsByTicketID(int ID);
        //Task<List<EgsTicketDetails>> GetDetailsByTicketNo(string RefNo);
        Task<List<EgsTicketDetails>> GetDetailsByTicketNo(string RefNo, string wallet);
        Task<EgsTicketDetails> GetLastDetailsByTicketNo(string RefNo, string wallet);
    }
}
