namespace LeonDrace.ObserverEventSystem
{
	/// <summary>
	/// Event signature interface to transfer data to inoked listeners.
	/// </summary>
	/// <remarks>
	/// Used to create <see cref="EventBus{T}"/> through <see cref="EventBusUtil.Initialize"/> and 
	/// <see cref="EventListener{T}"/> in <see cref="EventManager"/>.
	/// </remarks>
	public interface IEventInvoker { }

	public class NoArgsEvent : IEventInvoker { }
}
