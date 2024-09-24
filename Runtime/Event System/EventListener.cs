using System;

namespace LeonDrace.TypeObserverEventSystem
{
	internal interface IEventListener
	{
		/// <summary>
		/// Add listener. 
		/// <see cref="EventListenerBase{TDelegate}.AddListener(TDelegate, bool)"/>
		/// </summary>
		/// <remarks>
		/// Option to allowDuplicates by default true to safe performance.
		/// </remarks>
		/// <param name="allowDuplicates"></param>
		/// <returns>Returns if add was successful.</returns>
		public bool AddListener(Action onEvent, bool allowDuplicates = true);
		/// <summary>
		/// Remove listener. 
		/// <see cref="EventListenerBase{TDelegate}.RemoveListener(TDelegate)"/>
		/// </summary>
		/// <returns>Returns if removal was successful.</returns>
		public bool RemoveListener(Action onEvent);
		/// <summary>
		/// Remove all listeners. 
		/// <see cref="EventListenerBase{TDelegate}.RemoveAll"/>
		/// </summary>
		/// <returns>Returns if removal was successful.</returns>
		public void RemoveAll();
		/// <summary>
		/// Will invoke <see cref="EventListener.Invoke"/> all non-null 
		/// instances otherwise removes them.
		/// </summary>
		public void Invoke();
		/// <summary>
		/// Faster Invoke on <see cref="EventListener.InvokeUnsafe"/> but expecting proper 
		/// add/remove handling that none of the listeners is ever null.
		/// </summary>
		public void InvokeUnsafe();
	}

	internal interface IEventListener<T>
	{
		/// <summary>
		/// Register <see cref="EventListener{T}"/> to <see cref="EventBus{T}"/>. 
		/// The <see cref="EventBus{T}"/> auto initializes through <see cref="EventBusUtil"/> 
		/// and manages all different event listeners of this type and on invoke will dispatch
		/// all listeners in <see cref="EventListenerBase{TDelegate}"/>.
		/// </summary>
		public void Register();
		/// <summary>
		/// Remove event listener from <see cref="EventBus{T}"/>.
		/// </summary>
		public void Unregister();
		/// <summary>
		/// Add listener. 
		/// <see cref="EventListenerBase{TDelegate}.AddListener(TDelegate, bool)"/>
		/// </summary>
		/// <remarks>
		/// Option to allowDuplicates by default true to safe performance.
		/// </remarks>
		/// <param name="allowDuplicates"></param>
		/// <returns>Returns if add was successful.</returns>
		public bool AddListener(Action<T> onEvent, bool allowDuplicates = true);
		/// <summary>
		/// Remove listener. 
		/// <see cref="EventListenerBase{TDelegate}.RemoveListener(TDelegate)"/>
		/// </summary>
		/// <returns>Returns if removal was successful.</returns>
		public bool RemoveListener(Action<T> onEvent);
		/// <summary>
		/// Remove all listeners. 
		/// <see cref="EventListenerBase{TDelegate}.RemoveAll"/>
		/// </summary>
		/// <returns>Returns if removal was successful.</returns>
		public void RemoveAll();
		/// <summary>
		/// Will invoke <see cref="EventListener{T}.Invoke(T)"/> all non-null 
		/// instances otherwise removes them.
		/// </summary>
		/// <param name="event"></param>
		public void Invoke(T @event);
		/// <summary>
		/// Faster Invoke on <see cref="EventListener{T}.InvokeUnsafe(T)"/> but expecting proper 
		/// add/remove handling that none of the listeners is ever null.
		/// </summary>
		/// <param name="event"></param>
		public void InvokeUnsafe(T @event);
	}

	/// <summary>
	/// An <see cref="EventListener"/> that takes no arguments.
	/// Is not part of the <see cref="EventBus{T}"/> or <see cref="EventManager"/> eco system
	/// thus has no Register/Unregister, has to be self-managed. 
	/// </summary>
	/// <remarks>
	/// Implements the <see cref="IEventListener"/> interface.
	/// </remarks>
	public class EventListener : EventListenerBase<Action>, IEventListener
	{
		public EventListener() { }
		public EventListener(Action listener) => AddListener(listener);
		public EventListener(Action listener, bool allowDuplicates = true) => AddListener(listener, allowDuplicates);

		public void Invoke()
		{
			for (uint i = m_Count; i > 0; --i)
			{
				if (i > m_Count) throw s_IndexOutOfRangeException;

				if (m_Listeners[i - 1] != null)
				{
					m_Listeners[i - 1]();
				}
				else
				{
					RemoveAt(i - 1);
				}
			}
		}
		public void InvokeUnsafe()
		{
			for (uint i = m_Count; i > 0; --i)
			{
				m_Listeners[i - 1]();
			}
		}
	}

	/// <summary>
	/// <see cref="EventListenerAny{T}"/> accepts any type which is useful 
	/// for creating a fast listener for distinct value changes as <see cref="Observable{T}.onValueChanged"/>.
	/// Is not part of the <see cref="EventBus{T}"/> or <see cref="EventManager"/> eco system
	/// thus has no Register/Unregister, has to be self-managed.
	/// </summary>
	public class EventListenerAny<T> : EventListenerBase<Action<T>>
	{
		public EventListenerAny() { }
		public EventListenerAny(Action<T> listener) => AddListener(listener);
		public EventListenerAny(Action<T> listener, bool allowDuplicates = true) => AddListener(listener, allowDuplicates);

		public void Invoke(T @event)
		{
			for (uint i = m_Count; i > 0; --i)
			{
				if (i > m_Count) throw s_IndexOutOfRangeException;

				if (m_Listeners[i - 1] != null)
				{
					m_Listeners[i - 1](@event);
				}
				else
				{
					RemoveAt(i - 1);
				}
			}
		}
		public void InvokeUnsafe(T @event)
		{
			for (uint i = m_Count; i > 0; --i)
			{
				m_Listeners[i - 1](@event);
			}
		}
	}

	/// <summary>
	/// The standard event listener.
	/// Expects an IEventInvoker as T which implements <see cref="EventListenerBase{TDelegate}"/>
	/// and creates an <see cref="Action{T}"/>
	/// </summary>
	/// <remarks>
	/// Implements the <see cref="IEventListener{T}"/> interface.
	/// </remarks>
	/// <typeparam name="T"></typeparam>
	public class EventListener<T> : EventListenerBase<Action<T>>, IEventListener<T> where T : IEventInvoker
	{
		public EventListener() { }
		public EventListener(Action<T> listener) => AddListener(listener);
		public EventListener(Action<T> listener, bool allowDuplicates = true) => AddListener(listener, allowDuplicates);

		public void Register() => EventBus<T>.Register(this);
		public void Unregister() => EventBus<T>.Unregister(this);
		public void Invoke(T @event)
		{
			for (uint i = m_Count; i > 0; --i)
			{
				if (i > m_Count) throw s_IndexOutOfRangeException;

				if (m_Listeners[i - 1] != null)
				{
					m_Listeners[i - 1](@event);
				}
				else
				{
					RemoveAt(i - 1);
				}
			}
		}
		public void InvokeUnsafe(T @event)
		{
			for (uint i = m_Count; i > 0; --i)
			{
				m_Listeners[i - 1](@event);
			}
		}
	}
}
