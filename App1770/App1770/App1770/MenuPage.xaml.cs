using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App1770
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MenuPage : ContentPage
	{
		public MenuPage ()
		{
			InitializeComponent ();

            Title = "Title";
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

        private void btnShowMenu_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (ContentMenu.IsVisible)
                {
                    ContentMenu.IsVisible = false;
                }
                else {
                    ContentMenu.IsVisible = true;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void PageContent_Focused(object sender, FocusEventArgs e)
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

        private void PageContent_Unfocused(object sender, FocusEventArgs e)
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
    }
}