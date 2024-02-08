using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace PaymentGateway21052021.Areas.Identity.MenuConfiguration
{

    public class Config
    {
        public string ApiBaseUrl { get; set; } = null;
        public string NppaState { get; set; } = null;
        public List<Menu> Menu { get; set; } = null;
    }
    public class Menu
    {
        public string Icon { get; set; } = null;
        public string Name { get; set; } = null;
        public List<Link> Links { get; set; }

        public Menu()
        {
            Links = new List<Link>();
        }
    }

    public class Link
    {
        public string Area { get; set; } = null;
        public string Controller { get; set; } = null;
        public List<string> RoleNames { get; set; } = null;
        public Action Action { get; set; } = null;
    }

    public class Action
    {
        public string DisplayName { get; set; } = null;
        public string ActionName { get; set; } = null;
        public string IconName { get; set; } = null;
    }



    #region "Configuration for Administrator"
    public class ConfigAdministrator
    {
        public List<MenuAdm> MenuAdm { get; set; } = null;
    }

    public class MenuAdm
    {
        public string Icon { get; set; } = null;
        public string Name { get; set; } = null;
        public List<LinkAdm> LinksAdm { get; set; }

        public MenuAdm()
        {
            LinksAdm = new List<LinkAdm>();
        }
    }

    public class LinkAdm
    {
        public string Area { get; set; } = null;
        public string Controller { get; set; } = null;
        public List<string> RoleNames { get; set; } = null;
        public ActionAdm Action { get; set; } = null;
    }

    public class ActionAdm
    {
        public string DisplayName { get; set; } = null;
        public string ActionName { get; set; } = null;
        public string IconName { get; set; } = null;
    }
    #endregion
}

