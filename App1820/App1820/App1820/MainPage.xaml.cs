using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App1820
{
    public partial class MainPage : MasterDetailPage
    {
        public List<MasterPageItem> menuList { get; set; }

        public MainPage()
        {
            InitializeComponent();

            menuList = new List<MasterPageItem>();

            // Creating our pages for menu navigation
            // Here you can define title for item, 
            // icon on the left side, and page that you want to open after selection
            var pageHome = new MasterPageItem() { Title = "Home", Icon = "home_icon.png", TargetType = typeof(HomePage) };
            var pageSchedule = new MasterPageItem() { Title = "Schedule", Icon = "schedule_icon.png", TargetType = typeof(SchedulePage) };
            var pagePhysician = new MasterPageItem() { Title = "Physician", Icon = "physician_icon.png", TargetType = typeof(PhysicianPage) };
            var pageLocation = new MasterPageItem() { Title = "Location", Icon = "location_icon.png", TargetType = typeof(LocationPage) };
            var pageResources = new MasterPageItem() { Title = "Resources", Icon = "resources_icon.png", TargetType = typeof(ResourcesPage) };
            var pageOthers = new MasterPageItem() { Title = "Other Procedure", Icon = "otherprocedure_icon.png", TargetType = typeof(OtherProcedurePage) };
            var pageChangePassword = new MasterPageItem() { Title = "Change Password", Icon = "settings_icon.png", TargetType = typeof(ChangePasswordPage) };
            var pageSignOut = new MasterPageItem() { Title = "Sign Out", Icon = "settings_icon.png", TargetType = typeof(SignOutPage) };

            // Adding menu items to menuList
            menuList.Add(pageHome);
            menuList.Add(pageSchedule);
            menuList.Add(pagePhysician);
            menuList.Add(pageLocation);
            menuList.Add(pageResources);
            menuList.Add(pageOthers);
            menuList.Add(pageChangePassword);
            menuList.Add(pageSignOut);

            // Setting our list to be ItemSource for ListView in MainPage.xaml
            navigationDrawerList.ItemsSource = menuList;

            // Initial navigation, this can be used for our home page
            Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(HomePage)));
        }

        // Event for Menu Item selection, here we are going to handle navigation based
        // on user selection in menu ListView
        private void OnMenuItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

            var item = (MasterPageItem)e.SelectedItem;
            Type page = item.TargetType;

            string pageName = page?.Name;

            if (pageName == "HomePage")
            {
            }
            else if (pageName == "SchedulePage")
            {
            }
            else if (pageName == "PhysicianPage")
            {
            }
            else if (pageName == "LocationPage")
            {
            }
            else if (pageName == "ResourcesPage")
            {
            }
            else if (pageName == "ChangePasswordPage")
            {
            }
            else if (pageName == "SignOutPage")
            {
            }

            Detail = new NavigationPage((Page)Activator.CreateInstance(page));

            //var loadPage = (Page)Activator.CreateInstance(page);
            //Detail = loadPage;

            IsPresented = false;
        }
    }
}
