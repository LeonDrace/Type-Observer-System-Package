using UnityEngine;

namespace LeonDrace.ObserverEventSystem
{
	public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
	{
		private static T s_Instance;

		public static bool IsInitialized
		{
			get
			{
				return s_Instance != null;
			}
		}

		public static T Instance
		{
			get
			{
				if (s_Instance == null)
				{
					MonoBehaviour[] instances = FindObjectsOfType<T>();
					if (instances.Length > 0)
					{
						s_Instance = (T)instances[0];
					}

					if (instances.Length > 1)
					{
						Debug.LogError($"{nameof(T)} There should never be more than 1 singleton!");
						return s_Instance;
					}

					if (s_Instance == null)
					{
						Debug.LogError($"{nameof(T)} Specified Singleton was not found!");
						return default;
					}
				}

				return s_Instance;
			}
		}

		public virtual void Awake()
		{
			//Destroy newly created instance one already exists.
			if (s_Instance is not null && s_Instance != this)
			{
				Destroy(gameObject);
#if UNITY_EDITOR
				Debug.Log("Destroyed newly created Eventmanager, one already exists.");
#endif
				return;
			}

			s_Instance = GetComponent<T>();
		}

		public virtual void OnDestroy()
		{
			s_Instance = null;
		}
	}
}