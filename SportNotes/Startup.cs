using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SportNotes.Startup))]
namespace SportNotes
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
