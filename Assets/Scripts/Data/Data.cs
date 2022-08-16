using System;
using System.Collections.Generic;
using UnityEngine;

namespace Blu.MCDAncientHunt.Data
{
	[Serializable]
	public struct LootTable
	{
		public string                          Mob;
		public string                          Loot;
		public string                          C;
		public string                          R;
		public string                          I;
		public string                          A;
		public string                          Q;
		public string                          U;
		public string                          S;
		public string                          T;
		public string                          P;
		public List<string>                    AllLoot;
		public List<(string Rune, int Amount)> RuneSet;
	}
	
	[Serializable]
	public struct Gear
	{
		public string       Item;
		public GearTypes    Type;
		public string       C;
		public string       R;
		public string       I;
		public string       A;
		public string       Q;
		public string       U;
		public string       S;
		public string       T;
		public string       P;
		public List<string> Runes;
		public string       Locations;
		public List<string> AllLocations;
	}

	[Serializable]
	public struct Rune
	{
		public string Name;
		public Sprite Image;
	}

	[Serializable]
	public struct AncientQuery
	{
		public LootTable  Ancient;
		public List<Gear> Weapons;
		public List<Gear> Armor;
		public List<Gear> Bows;
		public List<Gear> Artifacts;
	}

	[Serializable]
	public struct SmartFilter
	{
		public string     Location;
		public List<Gear> Weapons;
		public List<Gear> Armor;
		public List<Gear> Bows;
		public List<Gear> Artifacts;

		public SmartFilter(string location)
		{
			Location         = location;
			Weapons          = new List<Gear>();
			Armor            = new List<Gear>();
			Bows             = new List<Gear>();
			Artifacts        = new List<Gear>();
		}

		public float GetWeight()
		{
			int weaponsWeight   = Weapons.Count   > 0 ? 1 : 0;
			int armorWeight     = Armor.Count     > 0 ? 1 : 0;
			int bowsWeight      = Bows.Count      > 0 ? 1 : 0;
			int artifactsWeight = Artifacts.Count > 0 ? 1 : 0;
			
			return (weaponsWeight + armorWeight + bowsWeight + artifactsWeight) / 4f;
		}
	}

	public enum GearTypes
	{
		Weapons   = 0,
		Armor     = 1,
		Bows      = 2,
		Artifacts = 3
	}
}