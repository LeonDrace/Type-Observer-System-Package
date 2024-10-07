using UnityEngine;

namespace LeonDrace.ObserverEventSystem
{
	[CreateAssetMenu(fileName = "ObserverEventData", menuName = "LeonDrace/Observer Event Data", order = 0)]
	public class ObserverEventData : ScriptableObject
	{
		[SerializeField]
		private string[] m_Assemblies = new string[]
		{
		"LeonDrace.ObserverEventSystem",
		"Assembly-CSharp",
		"Assembly-CSharp-firstpass"
		};

		public string[] Assemblies => m_Assemblies;

		/// <summary>
		/// Tries to find the asset in the resource folder.
		/// </summary>
		/// <returns></returns>
		public static ObserverEventData TryGet()
		{
			return Resources.Load<ObserverEventData>("ObserverEventData");
		}
	}
}
