using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Blabla.Startup))]
namespace Blabla
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}
