using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Owin;

namespace OwinHttpTracker
{
    public static class AppBuilderExtensionHelper
    {
        public static IAppBuilder UseOwinHttpTracker(this IAppBuilder builder, IHttpEventTracker tracker = null)
        {
            return builder.Use<OwinHttpTracker>(tracker);
        }
    }
}
