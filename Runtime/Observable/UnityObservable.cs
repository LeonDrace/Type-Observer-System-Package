using UnityEngine;
using UnityEngine.Events;

namespace LeonDrace.ObserverEventSystem.Observables
{
	[System.Serializable]
	public class UnityObservable<T>
	{
		[SerializeField]
		private T value;
		[SerializeField]
		private UnityEvent<T> onValueChanged;

		public T Value
		{
			get => value;
			set => Set(value);
		}

		public UnityObservable()
		{
			Value = default;
		}

		public UnityObservable(T value, UnityAction<T> callback = null)
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

		public void AddListener(UnityAction<T> callback)
		{
			if (callback == null) return;

			if (onValueChanged == null) onValueChanged = new UnityEvent<T>();

			onValueChanged.AddListener(callback);
		}

		public void RemoveListener(UnityAction<T> callback)
		{
			if (callback == null || onValueChanged == null) return;

			onValueChanged.RemoveListener(callback);
		}

		public void RemoveAllListeners()
		{
			if (onValueChanged == null) return;

			onValueChanged.RemoveAllListeners();
		}

		public void Dispose()
		{
			RemoveAllListeners();
			onValueChanged = null;
			value = default;
		}
	}
}


