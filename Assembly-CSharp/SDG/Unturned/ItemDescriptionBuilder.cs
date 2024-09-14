using System;
using System.Collections.Generic;
using System.Text;

namespace SDG.Unturned
{
	// Token: 0x020002CF RID: 719
	public struct ItemDescriptionBuilder
	{
		// Token: 0x060014E9 RID: 5353 RVA: 0x0004D7A0 File Offset: 0x0004B9A0
		public void Append(string text, int sortOrder)
		{
			this.lines.Add(new ItemDescriptionLine
			{
				text = text,
				sortOrder = sortOrder
			});
		}

		/// <summary>
		/// If true, description should only be populated with contents from prior to the auto-layout UI changes.
		/// </summary>
		// Token: 0x04000864 RID: 2148
		public bool shouldRestrictToLegacyContent;

		// Token: 0x04000865 RID: 2149
		public List<ItemDescriptionLine> lines;

		/// <summary>
		/// BuildDescription implementations can use this to concatenate longer strings.
		/// </summary>
		// Token: 0x04000866 RID: 2150
		public StringBuilder stringBuilder;
	}
}
