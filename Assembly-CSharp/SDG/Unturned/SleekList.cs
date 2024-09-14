using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000726 RID: 1830
	public class SleekList<T> : SleekWrapper where T : class
	{
		/// <summary>
		/// Kind of hacky... Used by player list for group connections.
		/// </summary>
		// Token: 0x17000B2D RID: 2861
		// (get) Token: 0x06003C52 RID: 15442 RVA: 0x0011C16C File Offset: 0x0011A36C
		// (set) Token: 0x06003C53 RID: 15443 RVA: 0x0011C174 File Offset: 0x0011A374
		public int IndexOfCreateElementItem { get; private set; }

		// Token: 0x17000B2E RID: 2862
		// (get) Token: 0x06003C54 RID: 15444 RVA: 0x0011C17D File Offset: 0x0011A37D
		public int ElementCount
		{
			get
			{
				return this.visibleEntries.Count;
			}
		}

		// Token: 0x06003C55 RID: 15445 RVA: 0x0011C18A File Offset: 0x0011A38A
		public ISleekElement GetElement(int index)
		{
			return this.visibleEntries[index].element;
		}

		// Token: 0x06003C56 RID: 15446 RVA: 0x0011C19D File Offset: 0x0011A39D
		public void SetData(List<T> data)
		{
			this.data = data;
			this.NotifyDataChanged();
		}

		// Token: 0x06003C57 RID: 15447 RVA: 0x0011C1AC File Offset: 0x0011A3AC
		public void NotifyDataChanged()
		{
			int num = this.data.Count * this.itemHeight;
			if (this.data.Count > 1)
			{
				num += (this.data.Count - 1) * this.itemPadding;
			}
			this.scrollView.ContentSizeOffset = new Vector2(0f, (float)num);
			this.UpdateVisibleRange();
		}

		// Token: 0x06003C58 RID: 15448 RVA: 0x0011C20E File Offset: 0x0011A40E
		public void ForceRebuildElements()
		{
			this.scrollView.RemoveAllChildren();
			this.visibleEntries.Clear();
			this.NotifyDataChanged();
		}

		// Token: 0x06003C59 RID: 15449 RVA: 0x0011C22C File Offset: 0x0011A42C
		public override void OnUpdate()
		{
			if (this.data.Count > 0)
			{
				int num = this.CalculateVisibleItemsCount();
				if (this.oldVisibleItemsCount != num)
				{
					this.oldVisibleItemsCount = num;
					this.UpdateVisibleRange();
				}
			}
		}

		// Token: 0x06003C5A RID: 15450 RVA: 0x0011C264 File Offset: 0x0011A464
		public SleekList()
		{
			this.scrollView = Glazier.Get().CreateScrollView();
			this.scrollView.SizeScale_X = 1f;
			this.scrollView.SizeScale_Y = 1f;
			this.scrollView.ScaleContentToWidth = true;
			this.scrollView.OnNormalizedValueChanged += new Action<Vector2>(this.onValueChanged);
			base.AddChild(this.scrollView);
		}

		// Token: 0x06003C5B RID: 15451 RVA: 0x0011C2E4 File Offset: 0x0011A4E4
		private int IndexOfItemWithinRange(T item, int minIndex, int maxIndex)
		{
			for (int i = minIndex; i <= maxIndex; i++)
			{
				if (this.data[i] == item)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06003C5C RID: 15452 RVA: 0x0011C31C File Offset: 0x0011A51C
		private bool HasElementForItem(T item)
		{
			using (List<SleekList<T>.VisibleEntry>.Enumerator enumerator = this.visibleEntries.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.item == item)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06003C5D RID: 15453 RVA: 0x0011C380 File Offset: 0x0011A580
		private void UpdateVisibleRange(float normalizedValue)
		{
			if (this.data.Count == 0 || this.onCreateElement == null)
			{
				this.scrollView.RemoveAllChildren();
				this.visibleEntries.Clear();
				return;
			}
			int num = this.CalculateVisibleItemsCount();
			this.oldVisibleItemsCount = num;
			int num2 = Mathf.Max(0, Mathf.FloorToInt(normalizedValue * (float)(this.data.Count - num)));
			int num3 = Mathf.Min(this.data.Count - 1, num2 + num);
			for (int i = this.visibleEntries.Count - 1; i >= 0; i--)
			{
				SleekList<T>.VisibleEntry visibleEntry = this.visibleEntries[i];
				int num4 = this.IndexOfItemWithinRange(visibleEntry.item, num2, num3);
				if (num4 == -1)
				{
					this.scrollView.RemoveChild(visibleEntry.element);
					this.visibleEntries.RemoveAtFast(i);
				}
				else
				{
					visibleEntry.element.PositionOffset_Y = (float)(num4 * (this.itemHeight + this.itemPadding));
				}
			}
			for (int j = num2; j <= num3; j++)
			{
				T item = this.data[j];
				if (!this.HasElementForItem(item))
				{
					this.IndexOfCreateElementItem = j;
					ISleekElement sleekElement = this.onCreateElement(item);
					sleekElement.SizeOffset_Y = (float)this.itemHeight;
					sleekElement.SizeScale_X = 1f;
					sleekElement.PositionOffset_Y = (float)(j * (this.itemHeight + this.itemPadding));
					this.scrollView.AddChild(sleekElement);
					this.visibleEntries.Add(new SleekList<T>.VisibleEntry(item, sleekElement));
				}
			}
		}

		// Token: 0x06003C5E RID: 15454 RVA: 0x0011C50B File Offset: 0x0011A70B
		private void UpdateVisibleRange()
		{
			this.UpdateVisibleRange(this.scrollView.NormalizedVerticalPosition);
		}

		// Token: 0x06003C5F RID: 15455 RVA: 0x0011C51E File Offset: 0x0011A71E
		private int CalculateVisibleItemsCount()
		{
			return Mathf.CeilToInt(this.scrollView.NormalizedViewportHeight * (float)this.data.Count);
		}

		// Token: 0x06003C60 RID: 15456 RVA: 0x0011C53D File Offset: 0x0011A73D
		private void onValueChanged(Vector2 value)
		{
			this.UpdateVisibleRange(value.y);
		}

		// Token: 0x17000B2F RID: 2863
		// (get) Token: 0x06003C61 RID: 15457 RVA: 0x0011C54B File Offset: 0x0011A74B
		// (set) Token: 0x06003C62 RID: 15458 RVA: 0x0011C553 File Offset: 0x0011A753
		public ISleekScrollView scrollView { get; private set; }

		// Token: 0x040025B1 RID: 9649
		public int itemHeight;

		// Token: 0x040025B2 RID: 9650
		public int itemPadding;

		// Token: 0x040025B3 RID: 9651
		public SleekList<T>.CreateElement onCreateElement;

		// Token: 0x040025B6 RID: 9654
		private List<T> data;

		// Token: 0x040025B7 RID: 9655
		private List<SleekList<T>.VisibleEntry> visibleEntries = new List<SleekList<T>.VisibleEntry>();

		// Token: 0x040025B8 RID: 9656
		private int oldVisibleItemsCount;

		// Token: 0x020009EF RID: 2543
		// (Invoke) Token: 0x06004CEE RID: 19694
		public delegate ISleekElement CreateElement(T item);

		// Token: 0x020009F0 RID: 2544
		private struct VisibleEntry
		{
			// Token: 0x06004CF1 RID: 19697 RVA: 0x001B832C File Offset: 0x001B652C
			public VisibleEntry(T item, ISleekElement element)
			{
				this.item = item;
				this.element = element;
			}

			// Token: 0x040034B8 RID: 13496
			public T item;

			// Token: 0x040034B9 RID: 13497
			public ISleekElement element;
		}
	}
}
