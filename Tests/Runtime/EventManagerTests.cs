using NUnit.Framework;
using UnityEngine;

#if UNITY_EDITOR

namespace LeonDrace.ObserverEventSystem.Tests
{
	public class EventManagerTests
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
			EventManager.Instance.Invoke(new NoArgsEvent());
			Assert.IsTrue(invoked);
		}
		[Test]
		public void InvokeUnsafe_EventManager()
		{
			bool invoked = false;
			EventManager.Instance.AddListener<NoArgsEvent>((x) => { invoked = true; });
			EventManager.Instance.InvokeUnsafe(new NoArgsEvent());
			Assert.IsTrue(invoked);
		}


		private void TestCallback(NoArgsEvent @event)
		{

		}

		#endregion
	}
}

#endif
