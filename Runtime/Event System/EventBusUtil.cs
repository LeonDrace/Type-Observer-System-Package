using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace LeonDrace.ObserverEventSystem
{
	public static class EventBusUtil
	{
		public static IReadOnlyList<Type> EventTypes { get; set; }
		public static IReadOnlyList<Type> EventBusTypes { get; set; }

#if UNITY_EDITOR
		public static PlayModeStateChange PlayModeState { get; set; }

		[InitializeOnLoadMethod]
		public static void InitializeEditor()
		{
			EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
			EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
		}

		static void OnPlayModeStateChanged(PlayModeStateChange state)
		{
			PlayModeState = state;
			if (state == PlayModeStateChange.ExitingPlayMode)
			{
				ClearAllBuses();
			}
		}
#endif

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		public static void Initialize()
		{
			var eventData = ObserverEventData.TryGet();

			EventTypes = eventData != null ?
				GetTypeFromAssembly(typeof(IEventInvoker), eventData.Assemblies) : GetTypeFromAllAssembly(typeof(IEventInvoker));
			EventBusTypes = InitializeAllBuses();
		}

		static List<Type> InitializeAllBuses()
		{
			List<Type> eventBusTypes = new List<Type>();

			Type typedef = typeof(EventBus<>);
			foreach (Type eventType in EventTypes)
			{
				Type busType = typedef.MakeGenericType(eventType);
				eventBusTypes.Add(busType);
#if UNITY_EDITOR
				Debug.Log($"Initialized EventBus<{eventType.Name}>");
#endif
			}

			return eventBusTypes;
		}

		public static void ClearAllBuses()
		{
#if UNITY_EDITOR
			Debug.Log("Clearing all event buses...");
#endif
			for (int i = 0; i < EventBusTypes.Count; i++)
			{
				Type busType = EventBusTypes[i];
				MethodInfo clearMethod = busType.GetMethod("Clear", BindingFlags.Static | BindingFlags.Public);

				clearMethod?.Invoke(null, null);
			}
		}

		public static List<Type> GetTypeFromAssembly(Type interfaceType, params string[] assemblyNames)
		{
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

			List<Type> filteredTypes = new List<Type>();

			for (int i = 0; i < assemblies.Length; i++)
			{
				foreach (var assemblyName in assemblyNames)
				{
					if (assemblies[i].GetName().Name == assemblyName)
					{
						AddTypesFromAssembly(assemblies[i].GetTypes(), interfaceType, filteredTypes);
						break;
					}
				}
			}

			return filteredTypes;
		}

		public static List<Type> GetTypeFromAllAssembly(Type interfaceType)
		{
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

			List<Type> filteredTypes = new List<Type>();

			foreach (Assembly assembly in assemblies)
			{
				AddTypesFromAssembly(assembly.GetTypes(), interfaceType, filteredTypes);
			}

			return filteredTypes;
		}

		private static void AddTypesFromAssembly(Type[] assemblyTypes, Type interfaceType, ICollection<Type> results)
		{
			if (assemblyTypes == null) return;

			for (int i = 0; i < assemblyTypes.Length; i++)
			{
				Type type = assemblyTypes[i];
				if (type != interfaceType && interfaceType.IsAssignableFrom(type))
				{
					results.Add(type);
				}
			}
		}
	}
}
