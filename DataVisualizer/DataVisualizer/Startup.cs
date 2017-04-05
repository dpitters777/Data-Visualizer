using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DataVisualizer.Startup))]
namespace DataVisualizer
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
