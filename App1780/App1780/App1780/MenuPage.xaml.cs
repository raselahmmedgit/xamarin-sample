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
	public partial class MenuPage : ContentPage
	{
		public MenuPage ()
		{
			InitializeComponent ();

            Title = "Title";
        }

        private void btnShowMenu_Clicked(object sender, EventArgs e)
        {
            try
            {
                Navigation.PushAsync(new OtherPage());
                //Popup?.ShowPopup(sender as View);
                //DisplayAlert(string.Empty, "Show Menu", "Ok");
            }
            catch (Exception ex)
            {
                throw;
            }
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

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (ContentMenu.IsVisible)
                {
                    ContentMenu.IsVisible = false;
                }
                else
                {
                    ContentMenu.IsVisible = true;
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
                //DisplayAlert(string.Empty, "PageContentTapGestureRecognizer_Tapped", "Ok");
                if (ContentMenu.IsVisible)
                {
                    ContentMenu.IsVisible = false;
                }
                else
                {
                    ContentMenu.IsVisible = true;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}