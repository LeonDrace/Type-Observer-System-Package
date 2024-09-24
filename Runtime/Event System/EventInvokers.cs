namespace LeonDrace.TypeObserverEventSystem
{
	/// <summary>
	/// Event signature interface to transfer data to inoked listeners.
	/// </summary>
	/// <remarks>
	/// Used to create <see cref="EventBus{T}"/> through <see cref="EventBusUtil.Initialize"/> and 
	/// <see cref="EventListener{T}"/> in <see cref="EventManager"/>.
	/// </remarks>
	public interface IEventInvoker { }

	public partial class EventInvokers
	{
		public static NoArgsEvent NoArgs => new();

		public class NoArgsEvent : IEventInvoker { }
	}
}
