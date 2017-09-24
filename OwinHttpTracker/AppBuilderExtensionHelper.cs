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
