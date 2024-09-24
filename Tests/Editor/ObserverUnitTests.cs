using NUnit.Framework;
using static LeonDrace.TypeObserverEventSystem.EventInvokers;

namespace LeonDrace.TypeObserverEventSystem.Tests
{
	public class ObserverUnitTests
	{
		#region Event Listener

		[Test]
		public void Add_Ten_Listeners_Benchmark()
		{
			int amount = 10;
			EventListener eventListener = new EventListener();
			for (int i = 0; i < amount; i++)
			{
				eventListener.AddListener(() => { });
			}
		}

		[Test]
		public void Add_Ten_Thousand_Listeners_Benchmark()
		{
			int amount = 10_000;
			EventListener eventListener = new EventListener();
			for (int i = 0; i < amount; i++)
			{
				eventListener.AddListener(() => { });
			}
		}

		[Test]
		public void AddListener_Once_Benchmark()
		{
			EventListener eventListener = new EventListener();
			eventListener.AddListener(TestCallback);
		}

		[Test]
		public void AddListener_ToEventListener()
		{
			EventListener eventListener = new EventListener();
			bool added = eventListener.AddListener(TestCallback);
			Assert.IsTrue(added);
		}
		[Test]
		public void RemoveListener_ToEventListenert()
		{
			EventListener eventListener = new EventListener();
			eventListener.AddListener(TestCallback);
			bool removed = eventListener.RemoveListener(TestCallback);
			Assert.IsTrue(removed);
		}
		[Test]
		public void EventListener_Count_1()
		{
			EventListener eventListener = new EventListener();
			eventListener.AddListener(TestCallback);
			Assert.IsTrue(eventListener.Count == 1);
		}
		[Test]
		public void EventListenerCount_10()
		{
			EventListener eventListener = new EventListener();
			for (int i = 0; i < 10; i++)
			{
				eventListener.AddListener(TestCallback);
			}
			Assert.IsTrue(eventListener.Count == 10);
		}
		[Test]
		public void EventListener_Contains()
		{
			EventListener eventListener = new EventListener();
			eventListener.AddListener(TestCallback);
			Assert.IsTrue(eventListener.Contains(TestCallback));
		}
		[Test]
		public void EventListener_Clear()
		{
			EventListener eventListener = new EventListener();
			eventListener.AddListener(TestCallback);
			eventListener.RemoveAll();
			Assert.IsTrue(eventListener.Count == 0);
		}
		[Test]
		public void EventListenerAny_Invoke_1()
		{
			EventListenerAny<bool> eventListener = new EventListenerAny<bool>();
			bool invoked = false;
			eventListener.AddListener((x) => { invoked = true; });
			eventListener.Invoke(true);
			Assert.IsTrue(invoked);
		}

		private void TestCallback()
		{

		}

		#endregion

		#region Event Bus
		[Test]
		public void Register_UnRegister_To_EventBus()
		{
			EventListener<NoArgsEvent> listener = new EventListener<NoArgsEvent>(TestItemCallback);
			EventBus<NoArgsEvent>.Register(listener);
			bool added = EventBus<NoArgsEvent>.Contains(listener);
			Assert.IsTrue(added);
			EventBus<NoArgsEvent>.Unregister(listener);
			bool removed = EventBus<NoArgsEvent>.Contains(listener);
			Assert.IsTrue(!removed);
		}
		[Test]
		public void Invoke_EventBus()
		{
			EventListener<NoArgsEvent> listener = new EventListener<NoArgsEvent>(TestItemCallback);
			EventBus<NoArgsEvent>.Register(listener);
			EventBus<NoArgsEvent>.Invoke(NoArgs);
			EventBus<NoArgsEvent>.Unregister(listener);
		}

		private void TestItemCallback(NoArgsEvent @event)
		{
			Assert.IsTrue(true);
		}

		#endregion

		#region Observed<>

		[Test]
		public void Observed_Default_And_Change()
		{
			Observable<float> observed = new Observable<float>(0, Observed_Default_And_Change_Test_Listener);
			Assert.IsTrue(observed.Value == 0);
			observed.Value = 0.5f;
		}

		private void Observed_Default_And_Change_Test_Listener(float percentage)
		{
			Assert.IsTrue(percentage == 0.5f);
		}

		[Test]
		public void Observed_Non_Default_At_Start()
		{
			Observable<float> observed = new Observable<float>(0.5f, Observed_Non_Default_At_Start_Test_Listener);
		}

		private void Observed_Non_Default_At_Start_Test_Listener(float percentage)
		{
			Assert.IsTrue(percentage == 0.5f);
		}

		[Test]
		public void UnityObserved_Callback()
		{
			UnityObservable<int> observed = new UnityObservable<int>(0, UnityObserved_Callback_Test_Listener);
			Assert.IsTrue(observed.Value == 0);
			observed.Value = 4;
		}

		private void UnityObserved_Callback_Test_Listener(int progress)
		{
			Assert.IsTrue(progress == 4);
		}

		#endregion
	}
}

