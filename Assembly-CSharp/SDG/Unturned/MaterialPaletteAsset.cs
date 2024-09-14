using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000318 RID: 792
	public class MaterialPaletteAsset : Asset
	{
		// Token: 0x060017EE RID: 6126 RVA: 0x00058358 File Offset: 0x00056558
		public override void PopulateAsset(Bundle bundle, DatDictionary data, Local localization)
		{
			base.PopulateAsset(bundle, data, localization);
			DatList datList;
			if (data.TryGetList("Materials", out datList))
			{
				this.materials = datList.ParseListOfStructs<ContentReference<Material>>();
			}
		}

		// Token: 0x060017EF RID: 6127 RVA: 0x00058389 File Offset: 0x00056589
		public MaterialPaletteAsset()
		{
			this.materials = new List<ContentReference<Material>>();
		}

		// Token: 0x04000AD4 RID: 2772
		public List<ContentReference<Material>> materials;
	}
}
