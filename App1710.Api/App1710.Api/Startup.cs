using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(App1710.Api.Startup))]
namespace App1710.Api
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
