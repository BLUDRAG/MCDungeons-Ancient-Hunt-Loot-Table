using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Blu.MCDAncientHunt.AncientSearch
{
	public class GearSearchDropdown : MonoBehaviour
	{
		#region Public Variables

		public float          Height;
		public TMP_InputField FilterInputField;
		public RectTransform  GearContainer;

		#endregion

		#region Private Variables

		private List<DropdownGearItem> _items = null;

		#endregion

		#region Unity Methods

		private void OnEnable()
		{
			CoroutineHelper.Instance.StartCoroutine(InitializeOnDelay());
			FilterInputField.text = "";
		}

		#endregion

		#region Unity Methods

		private IEnumerator InitializeOnDelay()
		{
			yield return new WaitForEndOfFrame();
			RectTransform rectTransform = transform as RectTransform;
			rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, Height);
			FilterInputField.Select();
		}

		public void FilterGear(string filter)
		{
			if(_items is null) _items = GearContainer.GetComponentsInChildren<DropdownGearItem>(true).ToList();
			
			foreach(DropdownGearItem item in _items)
			{
				string text = item.Text.text;
				if(string.IsNullOrEmpty(text)) continue;
				item.gameObject.SetActive(text.ToLower().Contains(filter.ToLower()));
			}
		}

		#endregion
	}
}