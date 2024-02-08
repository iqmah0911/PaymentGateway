using PaymentGateway21052021.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Areas.SysSetup.Models
{
    public class DocumentType
    {
        public string DocumentTypeName { get; set; }

    }

    public class DocumentParamsModel : BasicObjectsViewModel
    {
        public int DocumentID { get; set; }

        public string DocumentName { get; set; }

        public string DocumentType { get; set; }

        public int UserTypeID { get; set; }
        public string RoleName { get; set; }
        public int RoleID { get; set; }
    }

    public class DocumentsViewModel
    {
        public int documentID { get; set; }
        public string documentName { get; set; }
        public string documentType { get; set; }
        public int RoleID { get; set; }
        public string RoleName { get; set; }
        public DateTime dateCreated { get; set; }
        public int createdBy { get; set; }
    }

    public class DocumentsResponse
    {
        public string status { get; set; }
        public string message { get; set; }
        public DocumentsViewModel document { get; set; }
        public List<DocumentsViewModel> documents { get; set; }
    }
    public class HoldDocumentDisplayViewModel
    {
        public Pager Pager { get; set; }
        public IEnumerable<DocumentsViewModel> HoldAllDocument { get; internal set; }
    }
}
