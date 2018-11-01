using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(HoardSorter.Startup))]
namespace HoardSorter
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
