using System.Collections.Generic;
using Blu.MCDAncientHunt.Data;
using TMPro;
using UnityEngine;

namespace Blu.MCDAncientHunt.SmartFilter
{
	public class SmartFilterPanel : MonoBehaviour
	{
		#region Public Variables

		public TMP_Text        GearText;
		public RectTransform   SmartFilterItemContainer;
		public SmartFilterItem FilterItemTemplate;

		#endregion

		#region Custom Methods

		public void Initialize(List<AncientQuery> queries, string gear)
		{
			ClearItems();
			GearText.SetText(gear);

			foreach(AncientQuery query in queries)
			{
				SmartFilterItem item = Instantiate(FilterItemTemplate, SmartFilterItemContainer);
				item.Initialize(query);
			}
		}

		private void ClearItems()
		{
			for(int i = SmartFilterItemContainer.childCount - 1; i >= 0; i--)
			{
				Transform item = SmartFilterItemContainer.GetChild(i);
				if(item != FilterItemTemplate.transform) DestroyImmediate(item.gameObject);
			}
		}

		#endregion
	}
}