using UnityEngine;

namespace Blu.MCDAncientHunt
{
	public class CoroutineHelper : MonoBehaviour
	{
		#region Public Variables

		public static CoroutineHelper Instance;

		#endregion

		#region Unity Methods

		private void Awake()
		{
			Instance = this;
		}

		#endregion
	}
}