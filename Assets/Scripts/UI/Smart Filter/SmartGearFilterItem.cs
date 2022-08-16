using System.Collections.Generic;
using Blu.MCDAncientHunt.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Blu.MCDAncientHunt.SmartFilter
{
	public class SmartGearFilterItem : MonoBehaviour
	{
		#region Public Variables

		public Data.SmartFilter Filter;
		public TMP_Text         AreaName;
		public Image            Background;
		public SmartGearItem    WeaponGearTemplate;
		public SmartGearItem    ArmorGearTemplate;
		public SmartGearItem    BowGearTemplate;
		public SmartGearItem    ArtifactGearTemplate;

		#endregion

		#region Custom Methods

		public void Initialize(Data.SmartFilter filter)
		{
			Filter = filter;
			AreaName.SetText(Filter.Location);
			if(filter.GetWeight() >= 1f) Background.color = Color.green;
			CreateSmartGearItems(WeaponGearTemplate,   Filter.Weapons);
			CreateSmartGearItems(ArmorGearTemplate,    Filter.Armor);
			CreateSmartGearItems(BowGearTemplate,      Filter.Bows);
			CreateSmartGearItems(ArtifactGearTemplate, Filter.Artifacts);
			gameObject.SetActive(true);
		}

		private void CreateSmartGearItems(SmartGearItem template, List<Gear> gears)
		{
			foreach(Gear gear in gears)
			{
				SmartGearItem item = Instantiate(template, template.transform.parent);
				item.Initialize(gear);
			}
		}

		#endregion
	}
}