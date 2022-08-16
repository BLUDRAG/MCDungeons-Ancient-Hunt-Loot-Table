using System.Collections;
using System.Collections.Generic;
using Blu.MCDAncientHunt.Data;
using TMPro;
using UnityEngine;

namespace Blu.MCDAncientHunt.GearFilter
{
	public class GearFilterPanel : MonoBehaviour
	{
		#region Public Variables

		public TMP_Text       TypeText;
		public RectTransform  GearContainer;
		public GearItem       GearItemTemplate;
		public TMP_InputField AreaFilter;

		#endregion

		#region Custom Methods

		public void Initialize(string type, List<Gear> gears)
		{
			TypeText.SetText(type);
			
			for(int i = GearContainer.childCount - 1; i >= 0; i--)
			{
				Transform item = GearContainer.GetChild(i);
				if(item != GearItemTemplate.transform) DestroyImmediate(item.gameObject);
			}

			foreach(Gear gear in gears)
			{
				GearItem item = Instantiate(GearItemTemplate, GearContainer);
				item.Initialize(gear);
			}
			
			GearContainer.gameObject.SetActive(false);
			CoroutineHelper.Instance.StartCoroutine(InitializeDelayed());
		}

		private IEnumerator InitializeDelayed()
		{
			yield return new WaitForEndOfFrame();
			FilterArea(AreaFilter.text);
			GearContainer.gameObject.SetActive(true);
		}

		public void FilterArea(AreaItem area)
		{
			AreaFilter.text = area.Name.text;
		}

		public void FilterArea(string filter)
		{
			foreach(GearItem item in GearContainer.GetComponentsInChildren<GearItem>(true))
			{
				List<AreaItem> areas         = item.GetAreas();
				string         loweredFilter = filter.ToLower();
				item.gameObject.SetActive(areas.Exists(area => area.Name.text.ToLower().Contains(loweredFilter)));
			}
		}

		#endregion
	}
}