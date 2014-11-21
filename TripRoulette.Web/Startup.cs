using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TripRoulette.Web.Startup))]
namespace TripRoulette.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
