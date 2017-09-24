namespace OwinHttpTracker
{
    public interface IHttpEventTracker
    {
        void EmitEvent(HttpEvent httpEvent);
    }
}
