using PaymentGateway21052021.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Areas.EgsOperations.Models
{
    public class EgsAggregatorRequestViewModel
    {
        public int AggregatorID { get; set; }
        public string AggregatorCode { get; set; }
        public string Company { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int RoleID { get; set; }
        public string RoleName { get; set; }
        public string Bank { get; set; }
        public string AccountNumber { get; set; }
    }

    public class RequestAggregatorParams
    {
        public int RequestID { get; set; }
        public int AgentID { get; set; }
        public int AggregatorID { get; set; }
        public DateTime DateCreated { get; set; }
    }

    public class ProcessRequestAggregatorParams : RequestAggregatorParams
    {
        public string Comment { get; set; }
        public bool isProcessed { get; set; }
        public int ApprovedBy { get; set; }
        public DateTime DateApproved { get; set; }
        public int RejectedBy { get; set; }
        public DateTime DateRejected { get; set; }
    }

    public class AgentRequestViewModel
    {
        public int RequestID { get; set; }
         
        public int UserID { get; set; }
        public string UserName { get; set; }

        public int AggregatorId { get; set; }
        public string AggregatorName { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsProcessed { get; set; }
        public int ProcessedBy { get; set; }
        public int ApprovedBy { get; set; }
        public DateTime DateApproved { get; set; } 
        public int RejectedBy { get; set; }
        public DateTime DateRejected { get; set; }
        public int ApprovedBy2 { get; set; }
        public DateTime DateApprovedBy2 { get; set; }
        public int RejectedBy2 { get; set; }
        public DateTime DateRejectedBy2 { get; set; }
        public string Comment { get; set; }
        public string AgentCompany { get; set; }
        public string AggregatorCompany { get; set; }

    }

    public class PostAgentRequestViewModel
    {
        public int RequestID { get; set; }
        public int UserID { get; set; } 
        public int AggregatorID { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsProcessed { get; set; }
        public int ApprovedBy { get; set; }
        public DateTime DateApproved { get; set; } 
        public int RejectedBy { get; set; }
        public DateTime DateRejected { get; set; }
        public int ApprovedBy2 { get; set; }
        public DateTime DateApprovedBy2 { get; set; }
        public int RejectedBy2 { get; set; }
        public DateTime DateRejectedBy2 { get; set; }
        public string Comment { get; set; } 

    }




    public class HoldAgentRequestViewModel
    {
        public IEnumerable<AgentRequestViewModel> HoldAllRequests { get; set; }
        public Pager Pager { get; set; }
    }


    public class PostAgentResponse
    {
        public string status { get; set; }
        public string message { get; set; }
    }

   


    public class Aggregatorrequest
    {
        public object comment { get; set; }
        public bool isProcessed { get; set; }
        public int approvedBy { get; set; }
        public DateTime dateApproved { get; set; }
        public int rejectedBy { get; set; }
        public DateTime dateRejected { get; set; }
        public int requestID { get; set; }
        public int agentID { get; set; }
        public int aggregatorID { get; set; }
        public DateTime dateCreated { get; set; }
    }

    public class PAgentResponse
    {
        public string status { get; set; }
        public string message { get; set; }
        public List<Aggregatorrequest> aggregatorrequest { get; set; }
    }

















    public class AggregatorViewModel
    {
        public int AggregatorID { get; set; }
        public string AggregatorCode { get; set; }
        public string Company { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int RoleID { get; set; }
        public string RoleName { get; set; }
        public string Bank { get; set; }
        public string AccountNumber { get; set; }

    }
    public class AggregatorRequestResponse
    {
        public string status { get; set; }
        public string message { get; set; }
        public List<AggregatorViewModel> aggregators { get; set; }
    }

    public class HoldAggregatorsDisplayViewModel
    {
        public int aggID { get; set; }
        public Pager Pager { get; set; }
        public IEnumerable<AggregatorViewModel> HoldsAggregatorInfo { get; internal set; }
    }






}
