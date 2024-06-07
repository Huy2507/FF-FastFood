using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FF_Fastfood.Startup))]
namespace FF_Fastfood
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
