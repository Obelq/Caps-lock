using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebsiteForAds.Startup))]
namespace WebsiteForAds
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
