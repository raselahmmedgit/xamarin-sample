using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xam.Plugin;
using Xamarin.Forms;

namespace App1780
{
	public partial class MainPage : ContentPage
	{
        public SampleViewModel ViewModel => SampleViewModel.Instance;
        //public StackLayout MainLayout;
        //public Button ShowPopup;
        public PopupMenu Popup;

        public MainPage()
		{
			InitializeComponent();
            Title = "Title";

            Popup = new PopupMenu()
            {
                BindingContext = ViewModel
            };

            Popup.SetBinding(PopupMenu.ItemsSourceProperty, "ListItems");
        }

        private void btnShowMenu_Clicked(object sender, EventArgs e)
        {
            try
            {
                Navigation.PushAsync(new MenuPage());
                //Popup?.ShowPopup(sender as View);
                //DisplayAlert(string.Empty, "Show Menu", "Ok");
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
                var pageContent = PageContent;
                Popup?.ShowPopup(pageContent as View);
                //DisplayAlert(string.Empty, "Toolbar Item", "Ok");
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
