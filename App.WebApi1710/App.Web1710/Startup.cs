using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(App.Web1710.Startup))]
namespace App.Web1710
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
