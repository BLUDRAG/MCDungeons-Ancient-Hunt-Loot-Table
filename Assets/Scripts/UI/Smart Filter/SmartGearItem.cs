using Blu.MCDAncientHunt.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Blu.MCDAncientHunt.SmartFilter
{
	public class SmartGearItem : MonoBehaviour
	{
		#region Public Variables

		public Gear     Gear;
		public TMP_Text Name;
		public Image    RuneTemplate;

		#endregion

		#region Custom Methods

		public void Initialize(Gear gear)
		{
			Gear = gear;
			Name.SetText(Gear.Item);

			foreach(string rune in gear.Runes)
			{
				Image item = Instantiate(RuneTemplate, RuneTemplate.transform.parent);
				item.sprite = Resources.Load<Sprite>(rune);
				item.gameObject.SetActive(true);
			}
			
			gameObject.SetActive(true);
		}

		#endregion
	}
}