using PaymentGateway21052021.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Areas.SysSetup.Models
{
    public class SysRoleViewModel
    {
    }

    public class CreateRoleViewModel
    {
        public int RoleID { get; set; }
        public string RoleName { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }
        public int CreatedBy { get; set; }
    }

    public class UpdateRoleViewModel
    {
        public int RoleID { get; set; }
        public string RoleName { get; set; }
        public DateTime DateCreated { get; set; }
    }

    public class DisplayRoleViewModel
    {
        public int RoleID { get; set; }
        public string RoleName { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }
        public Pager Pager { get; set; }
        public IEnumerable<object> Items { get; set; }
    }

    public class RoleView
    {
        public int RoleID { get; set; }
        public string RoleName { get; set; }
    }

    public class RoleList
    {
        public List<RoleView> Role { get; set; }
    }

    public class HoldDisplayRoleViewModel
    {
        public IEnumerable<DisplayRoleViewModel> HoldAllRoles { get; set; }
        public Pager Pager { get; set; }
    }
}
