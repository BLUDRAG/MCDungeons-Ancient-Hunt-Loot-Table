using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Blu.MCDAncientHunt.AncientSearch;
using Blu.MCDAncientHunt.Data;
using TMPro;
using UnityEngine;

namespace Blu.MCDAncientHunt.SmartFilter
{
	public class SmartFilterItem : MonoBehaviour
	{
		#region Public Variables

		public AncientQuery        Query;
		public TMP_Text            AncientName;
		public SmartGearFilterItem FilterItemTemplate;
		public RuneItem            RuneItemTemplate;

		#endregion

		#region Custom Methods

		public void Initialize(AncientQuery query)
		{
			Query = query;
			AncientName.SetText(Query.Ancient.Mob);

			List<Data.SmartFilter> filters = new();
			AddGearToSmartFilters(ref filters, GearTypes.Weapons,   Query.Weapons);
			AddGearToSmartFilters(ref filters, GearTypes.Armor,     Query.Armor);
			AddGearToSmartFilters(ref filters, GearTypes.Bows,      Query.Bows);
			AddGearToSmartFilters(ref filters, GearTypes.Artifacts, Query.Artifacts);
			filters.Sort((first, second) => second.GetWeight().CompareTo(first.GetWeight()));

			foreach((string Rune, int Amount) runeSet in Query.Ancient.RuneSet)
			{
				RuneItem runeItem = Instantiate(RuneItemTemplate, RuneItemTemplate.transform.parent);
				runeItem.Initialize(runeSet);
			}

			foreach(Data.SmartFilter filter in filters)
			{
				SmartGearFilterItem item = Instantiate(FilterItemTemplate, FilterItemTemplate.transform.parent);
				item.Initialize(filter);
			}

			CoroutineHelper.Instance.StartCoroutine(InitializeOnDelay());
		}

		private IEnumerator InitializeOnDelay()
		{
			yield return new WaitForEndOfFrame();
			gameObject.SetActive(true);
		}

		private void AddGearToSmartFilters(ref List<Data.SmartFilter> filters, GearTypes type, List<Gear> gears)
		{
			foreach(Gear gear in gears)
			{
				foreach(string location in gear.AllLocations)
				{
					Data.SmartFilter filter = filters.FirstOrDefault(filter => filter.Location.Equals(location));

					if(string.IsNullOrEmpty(filter.Location))
					{
						filter = new Data.SmartFilter(location);
						filters.Add(filter);
					}

					switch(type)
					{
						case GearTypes.Weapons:
							filter.Weapons.Add(gear);
							break;
						case GearTypes.Armor:
							filter.Armor.Add(gear);
							break;
						case GearTypes.Bows:
							filter.Bows.Add(gear);
							break;
						case GearTypes.Artifacts:
							filter.Artifacts.Add(gear);
							break;
					}
				}
			}
		}

		#endregion
	}
}