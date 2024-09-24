# Type Observer System Package

An observer event system with an Event Manager which auto creates all existing events and is easy to use.
Also provides a low level api to self-manage events.

### How To Install:
Unity Package Manager Import URL: https://github.com/LeonDrace/Type-Observer-System-Package.git

### How To Use:
First extend the partial class EventInvokers with your own events.

* Event Markers/Invokers
```sh
public partial class EventInvokers
{
	public static NoArgsEvent NoArgs => new();

	public class NoArgsEvent : IEventInvoker { }
}
```
You can use structs too.

### Example:

Extended the events by a health event providing the new health value.

* Health Event
```sh
 public partial class EventInvokers
{
	public class HealthEvent : IEventInvoker
	{
		public float m_Health;
	}
}
```

* Create an observer
```sh
public void HealthObserver(EventInvokers.HealthEvent value)
{
	//Do something with the new health value.
}
```

* Add the observer
```sh
public void AddObserver()
{
	EventManager.Instance.AddListener<EventInvokers.HealthEvent>(HealthObserver);
}
```

* Invoke the event
```sh
public void InvokeHealthEvent()
{
	EventManager.Instance.Invoke(new EventInvokers.HealthEvent() { m_Health = 1 });
}
```

### Observables
There is an Observable<T> and UnityObservable<T> class that you can use as well to create observable fields of any kind.
They do support Inspector updates too.
