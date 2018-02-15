using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(prototype_2.Startup))]
namespace prototype_2
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
