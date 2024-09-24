using UnityEngine;

namespace LeonDrace.TypeObserverEventSystem
{
	[System.Serializable]
	public class Observable<T>
	{
		[SerializeField]
		private T value;

		private EventListenerAny<T> onValueChanged = new EventListenerAny<T>();

		public T Value
		{
			get => value;
			set => Set(value);
		}

		public Observable()
		{
			Value = default;
		}

		public Observable(T value, System.Action<T> callback = null)
		{
			AddListener(callback);
			Value = value;
		}

		private void Set(T value)
		{
			if (Equals(this.value, value)) return;

			this.value = value;
			Invoke();
		}

		public void Invoke()
		{
			onValueChanged?.Invoke(value);
		}

		public void AddListener(System.Action<T> callback)
		{
			if (callback == null) return;

			onValueChanged.AddListener(callback);
		}

		public void RemoveListener(System.Action<T> callback)
		{
			if (callback == null || onValueChanged == null) return;

			onValueChanged.RemoveListener(callback);
		}

		public void RemoveAllListeners()
		{
			if (onValueChanged == null) return;

			onValueChanged.RemoveAll();
		}

		public void Dispose()
		{
			onValueChanged = new EventListenerAny<T>();
			value = default;
		}
	}
}