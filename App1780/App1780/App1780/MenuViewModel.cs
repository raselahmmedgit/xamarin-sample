using System;
using System.Collections.Generic;
using System.Text;

namespace App1780
{
    public class MenuViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }

        public List<MenuViewModel> LoadMainPageMenu()
        {
            var menuViewModel = new List<MenuViewModel>()
                            {
                                new MenuViewModel() { Id = Convert.ToInt32(MenuEnum.Home), Name = "Home", Image = "settings_icon.png" },
                                new MenuViewModel() { Id = Convert.ToInt32(MenuEnum.ChangePassword), Name = "Change Password", Image = "settings_icon.png" },
                                new MenuViewModel() { Id = Convert.ToInt32(MenuEnum.SignOut), Name = "Sign Out", Image = "settings_icon.png" }
                            };

            return menuViewModel;
        }

        public List<MenuViewModel> LoadMenu()
        {
            var menuViewModel = new List<MenuViewModel>()
                            {
                                new MenuViewModel() { Id = Convert.ToInt32(MenuEnum.Home), Name = "Home", Image = "settings_icon.png" },
                                new MenuViewModel() { Id = Convert.ToInt32(MenuEnum.Schedule), Name = "Schedule", Image = "schedule_icon.png" },
                                new MenuViewModel() { Id = Convert.ToInt32(MenuEnum.Physician), Name = "Physician", Image = "physician_icon.png" },
                                new MenuViewModel() { Id = Convert.ToInt32(MenuEnum.Location), Name = "Location", Image = "location_icon.png" },
                                new MenuViewModel() { Id = Convert.ToInt32(MenuEnum.Resources), Name = "Resources", Image = "resources_icon.png" },
                                new MenuViewModel() { Id = Convert.ToInt32(MenuEnum.ChangePassword), Name = "Change Password", Image = "settings_icon.png" },
                                new MenuViewModel() { Id = Convert.ToInt32(MenuEnum.SignOut), Name = "Sign Out", Image = "settings_icon.png" }
                            };

            return menuViewModel;
        }
    }
}
