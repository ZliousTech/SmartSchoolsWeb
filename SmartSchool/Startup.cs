using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(SmartSchool.Startup))]

namespace SmartSchool
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}
