using Blu.MCDAncientHunt.Data;
using Blu.MCDAncientHunt.GearFilter;
using TMPro;
using UnityEngine;

namespace Blu.MCDAncientHunt.AncientSearch
{
	public class AncientItem : MonoBehaviour
	{
		#region Public Variables

		public AncientQuery    Query;
		public GearFilterPanel GearFilterPanel;
		public TMP_Text        Name;
		public RectTransform   RunesPanel;
		public RuneItem        RuneItemTemplate;
		public TMP_Text        WeaponsText;
		public TMP_Text        ArmorText;
		public TMP_Text        BowsText;
		public TMP_Text        ArtifactsText;

		#endregion

		#region Custom Methods

		public void Initialize(AncientQuery query)
		{
			Query = query;
			Name.SetText(Query.Ancient.Mob);

			foreach((string Rune, int Amount) runeSet in Query.Ancient.RuneSet)
			{
				RuneItem runeItem = Instantiate(RuneItemTemplate, RunesPanel);
				runeItem.Initialize(runeSet);
			}
			
			WeaponsText.SetText($"Weapons ({Query.Weapons.Count})");
			ArmorText.SetText($"Armor ({Query.Armor.Count})");
			BowsText.SetText($"Bows ({Query.Bows.Count})");
			ArtifactsText.SetText($"Artifacts ({Query.Artifacts.Count})");
			gameObject.SetActive(true);
		}

		public void ShowGear(int type)
		{
			switch((GearTypes)type)
			{
				case GearTypes.Weapons:
					GearFilterPanel.Initialize(nameof(GearTypes.Weapons), Query.Weapons);
					break;
				case GearTypes.Armor:
					GearFilterPanel.Initialize(nameof(GearTypes.Armor), Query.Armor);
					break;
				case GearTypes.Bows:
					GearFilterPanel.Initialize(nameof(GearTypes.Bows), Query.Bows);
					break;
				case GearTypes.Artifacts:
					GearFilterPanel.Initialize(nameof(GearTypes.Artifacts), Query.Artifacts);
					break;
			}
		}

		#endregion
	}
}