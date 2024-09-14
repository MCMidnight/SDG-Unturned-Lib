using System;

namespace SDG.Unturned
{
	// Token: 0x020002E4 RID: 740
	public class ItemGearAsset : ItemClothingAsset
	{
		/// <summary>
		/// If set, find a child meshrenderer with this name and change its material to the character hair material.
		/// </summary>
		// Token: 0x17000393 RID: 915
		// (get) Token: 0x06001601 RID: 5633 RVA: 0x0005177A File Offset: 0x0004F97A
		// (set) Token: 0x06001602 RID: 5634 RVA: 0x00051782 File Offset: 0x0004F982
		public string hairOverride { get; protected set; }

		// Token: 0x06001603 RID: 5635 RVA: 0x0005178B File Offset: 0x0004F98B
		public override void PopulateAsset(Bundle bundle, DatDictionary data, Local localization)
		{
			base.PopulateAsset(bundle, data, localization);
			base.hairVisible = data.ContainsKey("Hair");
			base.beardVisible = data.ContainsKey("Beard");
			this.hairOverride = data.GetString("Hair_Override", null);
		}
	}
}
