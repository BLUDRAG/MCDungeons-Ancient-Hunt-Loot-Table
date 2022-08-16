using TMPro;
using UnityEngine;

namespace Blu.MCDAncientHunt.GearFilter
{
	public class AreaItem : MonoBehaviour
	{
		#region Public Variables

		public TMP_Text Name;

		#endregion

		#region Custom Methods

		public void Initialize(string area)
		{
			Name.SetText(area);
			gameObject.SetActive(true);
		}

		#endregion
	}
}