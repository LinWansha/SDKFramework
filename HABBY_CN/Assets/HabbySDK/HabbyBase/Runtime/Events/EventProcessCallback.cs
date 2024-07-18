using System;

namespace Habby.Events
{
    public class EventProcessCallback
    {
        public static Action<EventHandler> OnAdded = delegate { };
        public static Action<EventHandler> OnRemoved = delegate { };
        public static Action<String> OnMissing = delegate { };
        public static Action<EventHandler, object[]> OnDispatched = delegate { };
        public static Action<EventHandler,object[], Exception> OnError = (eventHandler, args, exception) => Console.WriteLine(exception.ToString());
    }
}