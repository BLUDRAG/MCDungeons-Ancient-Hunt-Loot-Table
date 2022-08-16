using System.Collections.Generic;
using System.Linq;
using Blu.MCDAncientHunt.Data;
using TMPro;
using UnityEngine;

namespace Blu.MCDAncientHunt.GearFilter
{
	public class GearItem : MonoBehaviour
	{
		#region Public Variables

		public Gear          Gear;
		public TMP_Text      Name;
		public RectTransform AreasPanel;
		public AreaItem      AreaItemTemplate;

		#endregion

		#region Private Variables

		private List<AreaItem> _areas = null;

		#endregion

		#region Custom Methods

		public void Initialize(Gear gear)
		{
			Gear = gear;
			Name.SetText(Gear.Item);

			foreach(string location in Gear.AllLocations)
			{
				AreaItem item = Instantiate(AreaItemTemplate, AreasPanel);
				item.Initialize(location);
			}
			
			gameObject.SetActive(true);
		}

		public List<AreaItem> GetAreas()
		{
			if(_areas is null) _areas = AreasPanel.GetComponentsInChildren<AreaItem>(true).Where(area => !string.IsNullOrEmpty(area.Name.text)).ToList();
			return _areas;
		}

		#endregion
	}
}