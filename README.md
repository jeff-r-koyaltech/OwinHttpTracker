# OwinHttpTracker

This is a piece of middleware for monitoring requests and responses that pass through OWIN.

## Quickstart

This is typical OWIN middleware, so just wire it up in your solution. Read more here for details:
https://docs.microsoft.com/en-us/aspnet/aspnet/overview/owin-and-katana/owin-startup-class-detection

By default, you will get messages emitted through ETW.

## Extending

You can provide your own implementation of IHttpEventTracker when registering the middleware for use. This will allow you to control where the events are persisted. By default, an ETW event sink will be used. See HttpEventSource.cs for the ETW implementation.
