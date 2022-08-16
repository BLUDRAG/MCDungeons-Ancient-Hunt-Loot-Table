using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Blu.MCDAncientHunt.AncientSearch
{
	public class RuneItem : MonoBehaviour
	{
		#region Public Variables

		public Image    Rune;
		public TMP_Text Amount;

		#endregion

		#region Custom Methods

		public void Initialize((string Rune, int Amount) runeSet)
		{
			Rune.sprite = Resources.Load<Sprite>(runeSet.Rune);
			Amount.SetText($"{runeSet.Amount}");
			gameObject.SetActive(true);
		}

		#endregion
	}
}