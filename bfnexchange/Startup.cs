using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(bfnexchange.Startup))]
namespace bfnexchange
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
