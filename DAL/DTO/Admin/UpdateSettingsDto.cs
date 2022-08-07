using DAL.Model;

namespace DAL.DTO.Admin
{
    public class UpdateSettingsDto
    {
        public bool WebSiteIsActive { get; set; }

        public SettingModel.SiteTheme Theme { get; set; }

        public bool English { get; set; }
    }
}
