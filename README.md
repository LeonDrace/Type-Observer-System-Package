# Type Observer System Package

An observer event system with an Event Manager which auto creates all existing events and is easy to use.
Also provides a low level api to self-manage events.

### How To Install:
Unity Package Manager - add package from git URL: 

```sh
https://github.com/LeonDrace/Type-Observer-System-Package.git
```

Alternatively specific version: 

```sh
https://github.com/LeonDrace/Type-Observer-System-Package.git#v0.10.0
```

### How To Use:

Namespace:
```sh
using LeonDrace.ObserverEventSystem;
```

### Event Markers/Invokers

* Create your own events which only need to implement the IEventInvoker type as shown below with the HealthEvent.

* You can use structs too.

* By default it will search all assemblies and find all events automatically.

* But to improve search time you can create a scriptable object through the right click context menu: LeonDrace/ObserverEventData.

* Put that scriptable object into the resource folder. (There is no Addressables support yet, in that case remove it and default back to auto search).

* Add the assembly names in which your Events are a part of.

* By default it contains the package assembly for the NoArgsEvent and the default assemblies when no assembly definition is used.

### Example:

Extended the events by a health event providing the new health value.

* Health Event
```sh
public class HealthEvent : IEventInvoker
{
	public float m_Health;
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

For collections there is an Observable list that invokes on any element changes.

Namespace:
```sh
using LeonDrace.ObserverEventSystem.Observables;
```
```sh
Observable<T>
UnityObservable<T>
ObservableList<T>
```
