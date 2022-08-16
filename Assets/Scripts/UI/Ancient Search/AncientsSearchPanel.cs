using System.Collections.Generic;
using System.Linq;
using Blu.MCDAncientHunt.Data;
using Blu.MCDAncientHunt.SmartFilter;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Blu.MCDAncientHunt.AncientSearch
{
	public class AncientsSearchPanel : MonoBehaviour
	{
		#region Public Variables

		public Database         Database;
		public RectTransform    AncientItemsPanel;
		public AncientItem      AncientItemTemplate;
		public TMP_Dropdown     SearchDropdown;
		public Button           SmartFilterButton;
		public SmartFilterPanel SmartFilter;

		#endregion

		#region Private Variables

		private List<AncientQuery> _currentQueries = new();
		private string             _currentGear;

		#endregion

		#region Unity Methods

		private void Start()
		{
			Database.LoadData();

			List<string> options = new();
			options.Add("Select Gear");
			foreach(Gear gear in GetFilteredGear(Database.Weapons)) options.Add($"Weapon/{gear.Item}");
			foreach(Gear gear in GetFilteredGear(Database.Armor)) options.Add($"Armor/{gear.Item}");
			foreach(Gear gear in GetFilteredGear(Database.Bows)) options.Add($"Bow/{gear.Item}");
			foreach(Gear gear in GetFilteredGear(Database.Artifacts)) options.Add($"Artifact/{gear.Item}");

			SearchDropdown.ClearOptions();
			SearchDropdown.AddOptions(options);
			SearchDropdown.captionText.SetText("Select Gear");
		}

		#endregion
		
		#region Custom Methods

		public void SearchForAncients()
		{
			ClearItems();
			string queryText = SearchDropdown.options[SearchDropdown.value].text;
			_currentGear = queryText.Substring(queryText.IndexOf('/') + 1);
			_currentQueries.AddRange(Database.QueryGear(_currentGear));
			SmartFilterButton.interactable = _currentQueries.Count > 0;

			foreach(AncientQuery query in _currentQueries)
			{
				AncientItem item = Instantiate(AncientItemTemplate, AncientItemsPanel);
				item.Initialize(query);
			}
		}

		public void SmartFilterCurrentAncients()
		{
			SmartFilter.Initialize(_currentQueries, _currentGear);
		}

		private void ClearItems()
		{
			_currentQueries.Clear();
			for(int i = AncientItemsPanel.childCount - 1; i >= 0; i--)
			{
				Transform item = AncientItemsPanel.GetChild(i);
				if(item != AncientItemTemplate.transform) DestroyImmediate(item.gameObject);
			}
		}

		private IEnumerable<Gear> GetFilteredGear(IEnumerable<Gear> gears)
		{
			return gears.Where(gear => Database.LootTables.Exists(table => table.AllLoot.Contains(gear.Item)));
		}

		#endregion
	}
}