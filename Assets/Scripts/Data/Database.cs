using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

namespace Blu.MCDAncientHunt.Data
{
	public class Database : ScriptableObject
	{
		#region Public Variables

		public bool LoadButton;

		public List<Rune> Runes = new ();
		public TextAsset  LootTablesData;
		public TextAsset  WeaponsData;
		public TextAsset  ArmorData;
		public TextAsset  BowsData;
		public TextAsset  ArtifactsData;
		
		public List<LootTable> LootTables = new();
		public List<Gear>      Weapons    = new();
		public List<Gear>      Armor      = new();
		public List<Gear>      Bows       = new();
		public List<Gear>      Artifacts  = new();

		#endregion

		#region Private Variables

		private List<(GearTypes type, List<Gear> gear)> _gears = null;

		#endregion
		
		#region Custom Methods

		public void LoadData()
		{
			LootTables = JsonConvert.DeserializeObject<List<LootTable>>(LootTablesData.text);
			Weapons    = JsonConvert.DeserializeObject<List<Gear>>(WeaponsData.text);
			Armor      = JsonConvert.DeserializeObject<List<Gear>>(ArmorData.text);
			Bows       = JsonConvert.DeserializeObject<List<Gear>>(BowsData.text);
			Artifacts  = JsonConvert.DeserializeObject<List<Gear>>(ArtifactsData.text);

			EnsureGearList();

			for(int i = 0, condition = LootTables.Count; i < condition; i++)
			{
				LootTable table = LootTables[i];
				table.AllLoot = table.Loot.Split(',').ToList();

				List<(string Rune, int Amount)> runeSet = new();
				if(!string.IsNullOrEmpty(table.C)) runeSet.Add((nameof(table.C), int.Parse(table.C)));
				if(!string.IsNullOrEmpty(table.R)) runeSet.Add((nameof(table.R), int.Parse(table.R)));
				if(!string.IsNullOrEmpty(table.I)) runeSet.Add((nameof(table.I), int.Parse(table.I)));
				if(!string.IsNullOrEmpty(table.A)) runeSet.Add((nameof(table.A), int.Parse(table.A)));
				if(!string.IsNullOrEmpty(table.Q)) runeSet.Add((nameof(table.Q), int.Parse(table.Q)));
				if(!string.IsNullOrEmpty(table.U)) runeSet.Add((nameof(table.U), int.Parse(table.U)));
				if(!string.IsNullOrEmpty(table.S)) runeSet.Add((nameof(table.S), int.Parse(table.S)));
				if(!string.IsNullOrEmpty(table.T)) runeSet.Add((nameof(table.T), int.Parse(table.T)));
				if(!string.IsNullOrEmpty(table.P)) runeSet.Add((nameof(table.P), int.Parse(table.P)));
				table.RuneSet = runeSet;
				
				LootTables[i] = table;
			}

			foreach((GearTypes type, List<Gear> gear) gear in _gears)
			{
				for(int i = 0, condition = gear.gear.Count; i < condition; i++)
				{
					Gear data = gear.gear[i];
					data.AllLocations = gear.gear[i].Locations.Split(',').ToList();
					data.Type         = gear.type;

					data.Runes = new();
					if(data.C.Equals("X")) data.Runes.Add(nameof(data.C));
					if(data.R.Equals("X")) data.Runes.Add(nameof(data.R));
					if(data.I.Equals("X")) data.Runes.Add(nameof(data.I));
					if(data.A.Equals("X")) data.Runes.Add(nameof(data.A));
					if(data.Q.Equals("X")) data.Runes.Add(nameof(data.Q));
					if(data.U.Equals("X")) data.Runes.Add(nameof(data.U));
					if(data.S.Equals("X")) data.Runes.Add(nameof(data.S));
					if(data.T.Equals("X")) data.Runes.Add(nameof(data.T));
					if(data.P.Equals("X")) data.Runes.Add(nameof(data.P));

					gear.gear[i] = data;
				}
			}
		}

		private void EnsureGearList()
		{
			if(_gears is null)
			{
				_gears = new()
				        {
					       (GearTypes.Weapons, Weapons), 
					       (GearTypes.Armor, Armor),
					       (GearTypes.Bows, Bows),
					       (GearTypes.Artifacts, Artifacts)
				        };
			}
		}

		public List<AncientQuery> QueryGear(string item)
		{
			EnsureGearList();
			Gear gear = default;

			foreach((GearTypes type, List<Gear> gear) list in _gears)
			{
				foreach(Gear _gear in list.gear)
				{
					if(_gear.Item.Equals(item))
					{
						gear = _gear;
						goto queryEnd;
					}
				}
			}

			queryEnd:
			List<AncientQuery> ancientQueries = new();
			if(gear.Item is null) return ancientQueries;
			
			IEnumerable<LootTable> ancients = LootTables.Where(table => table.AllLoot.Exists(loot => loot.Equals(gear.Item)));

			foreach(LootTable ancient in ancients)
			{
				AncientQuery query = new()
				                     {
					                     Ancient   = ancient,
					                     Weapons   = new List<Gear>(),
					                     Armor     = new List<Gear>(),
					                     Bows      = new List<Gear>(),
					                     Artifacts = new List<Gear>(),
				                     };
				ancientQueries.Add(query);

				foreach((string Rune, int Amount) runeSet in ancient.RuneSet)
				{
					foreach((GearTypes type, List<Gear> gear) gearList in _gears)
					{
						switch(gearList.type)
						{
							case GearTypes.Weapons:
								AddRangeUnique(query.Weapons, gearList.gear.Where(_gear => _gear.Runes.Contains(runeSet.Rune)));
								break;
							case GearTypes.Armor:
								AddRangeUnique(query.Armor, gearList.gear.Where(_gear => _gear.Runes.Contains(runeSet.Rune)));
								break;
							case GearTypes.Bows:
								AddRangeUnique(query.Bows, gearList.gear.Where(_gear => _gear.Runes.Contains(runeSet.Rune)));
								break;
							case GearTypes.Artifacts:
								AddRangeUnique(query.Artifacts, gearList.gear.Where(_gear => _gear.Runes.Contains(runeSet.Rune)));
								break;
						}
					}
				}
				
				query.Weapons.Sort((gear1, gear2) => string.Compare(gear1.Item, gear2.Item, StringComparison.Ordinal));
				query.Armor.Sort((gear1, gear2) => string.Compare(gear1.Item, gear2.Item, StringComparison.Ordinal));
				query.Bows.Sort((gear1, gear2) => string.Compare(gear1.Item, gear2.Item, StringComparison.Ordinal));
				query.Artifacts.Sort((gear1, gear2) => string.Compare(gear1.Item, gear2.Item, StringComparison.Ordinal));
			}

			return ancientQueries;
		}

		private void AddUnique<T>(IList<T> list, T item)
		{
			if(!list.Contains(item))
			{
				list.Add(item);
			}
		}

		private void AddRangeUnique<T>(IList<T> list, IEnumerable<T> items)
		{
			foreach(var item in items)
			{
				AddUnique(list, item);
			}
		}

		#endregion
	}
}