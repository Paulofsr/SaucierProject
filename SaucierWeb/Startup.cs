using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SaucierWeb.Startup))]
namespace SaucierWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
