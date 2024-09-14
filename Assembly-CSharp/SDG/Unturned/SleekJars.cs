using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000721 RID: 1825
	public class SleekJars : SleekWrapper
	{
		// Token: 0x06003C42 RID: 15426 RVA: 0x0011BAFC File Offset: 0x00119CFC
		private void onClickedButton(SleekItem item)
		{
			int num = base.FindIndexOfChild(item);
			if (num != -1)
			{
				ClickedJar clickedJar = this.onClickedJar;
				if (clickedJar == null)
				{
					return;
				}
				clickedJar(this, num);
			}
		}

		// Token: 0x06003C43 RID: 15427 RVA: 0x0011BB28 File Offset: 0x00119D28
		public SleekJars(float radius, List<InventorySearch> search, float startAngle = 0f)
		{
			float num = 6.2831855f / (float)search.Count;
			for (int i = 0; i < search.Count; i++)
			{
				ItemJar jar = search[i].jar;
				if (jar.GetAsset() != null)
				{
					SleekItem sleekItem = new SleekItem(jar);
					sleekItem.PositionOffset_X = (float)((int)(Mathf.Cos(startAngle + num * (float)i) * radius)) - sleekItem.SizeOffset_X / 2f;
					sleekItem.PositionOffset_Y = (float)((int)(Mathf.Sin(startAngle + num * (float)i) * radius)) - sleekItem.SizeOffset_Y / 2f;
					sleekItem.PositionScale_X = 0.5f;
					sleekItem.PositionScale_Y = 0.5f;
					sleekItem.onClickedItem = new ClickedItem(this.onClickedButton);
					sleekItem.onDraggedItem = new DraggedItem(this.onClickedButton);
					base.AddChild(sleekItem);
				}
			}
		}

		// Token: 0x040025A9 RID: 9641
		public ClickedJar onClickedJar;
	}
}
