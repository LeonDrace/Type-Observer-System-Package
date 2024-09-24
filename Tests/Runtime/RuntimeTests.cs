using NUnit.Framework;
using UnityEngine;
using static LeonDrace.TypeObserverEventSystem.EventInvokers;

#if UNITY_EDITOR

namespace LeonDrace.TypeObserverEventSystem.Tests
{
	public class RuntimeTests
	{
		#region Event Manager

		[Test, Order(1)]
		public void CreateEventManager()
		{
			if (!EventManager.IsInitialized)
			{
				new GameObject("Event Manager").AddComponent<EventManager>();
			}

			Assert.IsNotNull(EventManager.Instance);
		}

		[Test]
		public void AddListener_ToEventManager()
		{
			bool added = EventManager.Instance.AddListener<NoArgsEvent>(TestCallback);
			Assert.IsTrue(added);
			EventManager.Instance.RemoveListener<NoArgsEvent>(TestCallback);
		}
		[Test]
		public void RemoveListener_ToEventManager()
		{
			EventManager.Instance.AddListener<NoArgsEvent>(TestCallback);
			bool removed = EventManager.Instance.RemoveListener<NoArgsEvent>(TestCallback);
			Assert.IsTrue(removed);
		}

		[Test]
		public void Invoke_EventManager()
		{
			bool invoked = false;
			EventManager.Instance.AddListener<NoArgsEvent>((x) => { invoked = true; });
			EventManager.Instance.Invoke(NoArgs);
			Assert.IsTrue(invoked);
		}
		[Test]
		public void InvokeUnsafe_EventManager()
		{
			bool invoked = false;
			EventManager.Instance.AddListener<NoArgsEvent>((x) => { invoked = true; });
			EventManager.Instance.InvokeUnsafe(NoArgs);
			Assert.IsTrue(invoked);
		}


		private void TestCallback(NoArgsEvent @event)
		{

		}

		#endregion
	}
}

#endif
