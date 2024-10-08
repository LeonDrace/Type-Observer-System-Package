using NUnit.Framework;

namespace LeonDrace.ObserverEventSystem.Observables.Tests
{
	public class ObservablesTest
	{
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

		#region List

		[Test]
		public void ObservableList_Add()
		{
			var list = new ObservableList<int>();
			int called = 0;
			void StateValidator(System.Collections.Generic.IList<int> obj)
			{
				called++;
			}

			list.AnyValueChanged += StateValidator;

			list.Add(1);

			Assert.That(called, Is.EqualTo(1));
			Assert.That(list.Count, Is.EqualTo(1));
		}

		[Test]
		public void ObservableList_Remove()
		{
			var list = new ObservableList<int>();
			int called = 0;

			void StateValidator(System.Collections.Generic.IList<int> obj)
			{
				called++;
			}

			list.AnyValueChanged += StateValidator;
			list.Add(1);
			list.Remove(1);
			Assert.That(called, Is.EqualTo(2));
			Assert.That(list.Count, Is.EqualTo(0));
		}

		[Test]
		public void ObservableList_RemoveAt()
		{
			var list = new ObservableList<int>();
			int called = 0;

			void StateValidator(System.Collections.Generic.IList<int> obj)
			{
				called++;
			}

			list.AnyValueChanged += StateValidator;
			list.Add(1);
			list.RemoveAt(0);
			Assert.That(called, Is.EqualTo(2));
			Assert.That(list.Count, Is.EqualTo(0));
		}

		[Test]
		public void ObservableList_Clear()
		{
			var list = new ObservableList<int>();
			int called = 0;

			void StateValidator(System.Collections.Generic.IList<int> obj)
			{
				called++;
			}

			list.AnyValueChanged += StateValidator;
			list.Add(1);
			list.Add(2);
			list.Clear();
			Assert.That(called, Is.EqualTo(3));
			Assert.That(list.Count, Is.EqualTo(0));
		}

		[Test]
		public void ObservableList_Insert()
		{
			var list = new ObservableList<int>();
			int called = 0;

			void StateValidator(System.Collections.Generic.IList<int> obj)
			{
				called++;
			}

			list.AnyValueChanged += StateValidator;
			list.Add(1);
			list.Add(3);
			list.Insert(1, 2);

			Assert.That(called, Is.EqualTo(3));
			Assert.That(list.Count, Is.EqualTo(3));
			Assert.That(list[1], Is.EqualTo(2));
		}

		[Test]
		public void ObservableList_Contains()
		{
			var list = new ObservableList<int>();
			int called = 0;

			void StateValidator(System.Collections.Generic.IList<int> obj)
			{
				called++;
			}

			list.AnyValueChanged += StateValidator;
			list.Add(1);
			list.Add(3);

			Assert.That(called, Is.EqualTo(2));
			Assert.That(list.Count, Is.EqualTo(2));
			Assert.IsTrue(list.Contains(1));
			Assert.IsTrue(list.Contains(3));
		}

		[Test]
		public void ObservableList_IndexOf()
		{
			var list = new ObservableList<int>();
			int called = 0;

			void StateValidator(System.Collections.Generic.IList<int> obj)
			{
				called++;
			}

			list.AnyValueChanged += StateValidator;
			list.Add(1);
			list.Add(3);

			Assert.That(called, Is.EqualTo(2));
			Assert.That(list.Count, Is.EqualTo(2));
			Assert.That(list.IndexOf(3), Is.EqualTo(1));
		}

		[Test]
		public void ObservableList_Copy()
		{
			var list = new ObservableList<int>();
			int called = 0;

			void StateValidator(System.Collections.Generic.IList<int> obj)
			{
				called++;
			}

			list.AnyValueChanged += StateValidator;
			list.Add(1);
			list.Add(3);

			int[] copy = new int[list.Count];
			list.CopyTo(copy, 0);

			Assert.That(called, Is.EqualTo(2));
			Assert.That(list.Count, Is.EqualTo(2));
			Assert.That(copy.Length, Is.EqualTo(2));
			Assert.That(copy[0], Is.EqualTo(1));
			Assert.That(copy[1], Is.EqualTo(3));
		}

		#endregion
	}
}