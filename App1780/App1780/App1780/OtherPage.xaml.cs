using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App1780
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OtherPage : ContentPage
    {
        public OtherPage()
        {
            InitializeComponent();
            Title = "Title";

            var menuViewModel = new MenuViewModel();
            ContentMenuListView.ItemsSource = menuViewModel.LoadMenu();

        }

        private void btnMenu1_Clicked(object sender, EventArgs e)
        {
            try
            {
                DisplayAlert(string.Empty, "Menu One", "Ok");
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void btnMenu2_Clicked(object sender, EventArgs e)
        {
            try
            {
                DisplayAlert(string.Empty, "Menu Two", "Ok");
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void btnMenu3_Clicked(object sender, EventArgs e)
        {
            try
            {
                DisplayAlert(string.Empty, "Menu Three", "Ok");
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void btnMenu4_Clicked(object sender, EventArgs e)
        {
            try
            {
                DisplayAlert(string.Empty, "Menu Four", "Ok");
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void btnMenu5_Clicked(object sender, EventArgs e)
        {
            try
            {
                DisplayAlert(string.Empty, "Menu Five", "Ok");
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void btnMenu6_Clicked(object sender, EventArgs e)
        {
            try
            {
                DisplayAlert(string.Empty, "Menu Six", "Ok");
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void btnMenu7_Clicked(object sender, EventArgs e)
        {
            try
            {
                DisplayAlert(string.Empty, "Menu Seven", "Ok");
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            try
            {
                var toolbarItem = sender as ToolbarItem;

                if (ContentMenu.IsVisible)
                {
                    ContentMenu.IsVisible = false;
                    ContentBody.IsVisible = true;
                    toolbarItem.Icon = "menu_open_icon.png";
                }
                else
                {
                    ContentMenu.IsVisible = true;
                    ContentBody.IsVisible = false;
                    toolbarItem.Icon = "menu_close_icon.png";
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void PageContentTapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            try
            {
                var toolbarItem = MenuToolbarItem as ToolbarItem;

                //DisplayAlert(string.Empty, "PageContentTapGestureRecognizer_Tapped", "Ok");
                if (ContentMenu.IsVisible)
                {
                    ContentMenu.IsVisible = false;
                    ContentBody.IsVisible = true;
                    toolbarItem.Icon = "menu_open_icon.png";
                }
                else
                {
                    ContentMenu.IsVisible = true;
                    ContentBody.IsVisible = false;
                    toolbarItem.Icon = "menu_close_icon.png";
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void ViewCell_Tapped(object sender, EventArgs e)
        {
            try
            {
                var viewCell = sender as ViewCell;
                var menuId = Convert.ToInt32(viewCell.ClassId);

                if (Convert.ToInt32(MenuEnum.Home) == menuId)
                {
                    DisplayAlert(string.Empty, menuId.ToString(), "Ok");
                }
                else if (Convert.ToInt32(MenuEnum.Schedule) == menuId)
                {
                    DisplayAlert(string.Empty, menuId.ToString(), "Ok");
                }
                else if (Convert.ToInt32(MenuEnum.Physician) == menuId)
                {
                    DisplayAlert(string.Empty, menuId.ToString(), "Ok");
                }
                else if (Convert.ToInt32(MenuEnum.Location) == menuId)
                {
                    DisplayAlert(string.Empty, menuId.ToString(), "Ok");
                }
                else if (Convert.ToInt32(MenuEnum.Resources) == menuId)
                {
                    DisplayAlert(string.Empty, menuId.ToString(), "Ok");
                }
                else if (Convert.ToInt32(MenuEnum.ChangePassword) == menuId)
                {
                    DisplayAlert(string.Empty, menuId.ToString(), "Ok");
                }
                else if (Convert.ToInt32(MenuEnum.SignOut) == menuId)
                {
                    DisplayAlert(string.Empty, menuId.ToString(), "Ok");
                }
                
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}