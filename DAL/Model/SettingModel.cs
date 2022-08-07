using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DAL.Model
{

    public class SettingModel
    {
        public int Id { get; set; }

        public bool WebSiteIsActive { get; set; }

        public SiteTheme Theme { get; set; }

        public bool English { get; set; }

        public SettingModel()
        {
            WebSiteIsActive = true;
            English = false;
            Theme = SiteTheme.LIGHT;
        }

        public enum SiteTheme
        {
            DARK,
            LIGHT
        }


    }
}
