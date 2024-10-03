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
	}
}