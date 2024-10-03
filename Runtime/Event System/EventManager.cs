using System;
using System.Reflection;

namespace LeonDrace.ObserverEventSystem
{
	public class EventManager : MonoSingleton<EventManager>
	{
		private Listeners[] m_Listeners;

		public override void Awake()
		{
			base.Awake();

			//Create one EventListener<T> where T : IEventInvoker for each event type.
			Type typedef = typeof(EventListener<>);
			//Array is actually faster than a dictionary with key System.Type.
			//Because getting the hashcode and collisions seems to be quite expensive
			//in most cases probably negligible.
			m_Listeners = new Listeners[EventBusUtil.EventTypes.Count];
			int i = 0;
			foreach (Type eventType in EventBusUtil.EventTypes)
			{
				//create the generic eventlistener of the invoker type
				Type eventListenerType = typedef.MakeGenericType(eventType);
				//create an object of the created eventlistener type
				object listener = Activator.CreateInstance(eventListenerType);
				//get the register method
				MethodInfo method = listener.GetType().GetMethod("Register");
				if (method != null)
				{
					//register to the event bus of the same invoker type
					method.Invoke(listener, null);
				}
				//add instance with event type and the create event listener object
				m_Listeners[i] = new Listeners() { eventType = eventType, listener = listener };
				i++;

#if UNITY_EDITOR
				UnityEngine.Debug.Log($"EventManager Create Listener Type: {eventType.Name}");
#endif
			}
		}

		private void OnDisable()
		{
			EventBusUtil.ClearAllBuses();
		}

		/// <summary>
		/// Add listener to the specified types <see cref="EventListener{T}"/> also see
		/// <see cref="EventListenerBase{TDelegate}.AddListener(TDelegate, bool)"/>
		/// </summary>
		/// <remarks>
		/// Option to allowDuplicates by default true to safe performance.
		/// </remarks>
		/// <param name="allowDuplicates"></param>
		/// <returns>Returns if add was successful.</returns>
		public bool AddListener<T>(System.Action<T> listener, bool allowDuplicates = true) where T : IEventInvoker
		{
			return GetEventListener<T>().AddListener(listener);
		}

		/// <summary>
		/// Remove listener from the specified types <see cref="EventListener{T}"/> also see
		/// <see cref="EventListenerBase{TDelegate}.RemoveListener(TDelegate)"/>
		/// </summary>
		/// <returns>Returns if remove was successful.</returns>
		public bool RemoveListener<T>(System.Action<T> listener) where T : IEventInvoker
		{
			return GetEventListener<T>().RemoveListener(listener);
		}

		/// <summary>
		/// Invokes the type <see cref="EventBus{T}.Invoke(T)"/>
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="event"></param>
		public void Invoke<T>(T @event) where T : IEventInvoker
		{
			EventBus<T>.Invoke(@event);
		}

		/// <summary>
		/// Invokes the type <see cref="EventBus{T}.InvokeUnsafe(T)"/>
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="event"></param>
		public void InvokeUnsafe<T>(T @event) where T : IEventInvoker
		{
			EventBus<T>.InvokeUnsafe(@event);
		}

		public void Clear<T>() where T : IEventInvoker
		{
			GetEventListener<T>().RemoveAll();
		}

		private IEventListener<T> GetEventListener<T>() where T : IEventInvoker
		{
			System.Type eventType = typeof(T);

			for (int i = 0; i < m_Listeners.Length; i++)
			{
				if (m_Listeners[i].eventType == eventType)
				{
					IEventListener<T> eventListener = (IEventListener<T>)m_Listeners[i].listener;
					return eventListener;
				}
			}

			return null;
		}

		private sealed class Listeners
		{
			public System.Type eventType;
			public object listener;
		}
	}
}