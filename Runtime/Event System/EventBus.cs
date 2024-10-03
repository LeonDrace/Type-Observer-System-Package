using System.Collections.Generic;

namespace LeonDrace.ObserverEventSystem
{
	public static class EventBus<T> where T : IEventInvoker
	{
		private static readonly HashSet<IEventListener<T>> s_EventListeners = new HashSet<IEventListener<T>>();

		public static void Register(EventListener<T> listener) => s_EventListeners.Add(listener);
		public static void Unregister(EventListener<T> listener) => s_EventListeners.Remove(listener);

		public static bool Contains(EventListener<T> listener)
		{
			return s_EventListeners.Contains(listener);
		}

		public static void Invoke(T @event)
		{
			foreach (IEventListener<T> listener in s_EventListeners)
			{
				listener.Invoke(@event);
			}
		}

		public static void InvokeUnsafe(T @event)
		{
			foreach (IEventListener<T> listener in s_EventListeners)
			{
				listener.InvokeUnsafe(@event);
			}
		}

		public static void Clear()
		{
			s_EventListeners.Clear();
		}
	}
}