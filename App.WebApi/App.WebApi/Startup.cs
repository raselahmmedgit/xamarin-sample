using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(App.WebApi.Startup))]
namespace App.WebApi
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
