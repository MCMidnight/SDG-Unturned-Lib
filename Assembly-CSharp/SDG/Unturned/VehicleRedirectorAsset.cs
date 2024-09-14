using System;
using UnityEngine;

namespace SDG.Unturned
{
	/// <summary>
	/// When loaded or spawned as a vehicle, creates a different vehicle instead.
	/// For example, Off_Roader_Orange has ID 4. When that ID is loaded/spawned the new combined Off_Roader vehicle is
	/// used instead. Can also optionally apply a paint color, allowing saves to be converted without losing colors.
	/// </summary>
	// Token: 0x0200037E RID: 894
	public class VehicleRedirectorAsset : Asset
	{
		/// <summary>
		/// Redirectors are in the Vehicle category so that legacy vehicle IDs point at the redirector.
		/// </summary>
		// Token: 0x170005B8 RID: 1464
		// (get) Token: 0x06001BB5 RID: 7093 RVA: 0x00063441 File Offset: 0x00061641
		public override EAssetType assetCategory
		{
			get
			{
				return EAssetType.VEHICLE;
			}
		}

		/// <summary>
		/// Vehicle to use when attempting to load or spawn this asset.
		/// </summary>
		// Token: 0x170005B9 RID: 1465
		// (get) Token: 0x06001BB6 RID: 7094 RVA: 0x00063444 File Offset: 0x00061644
		// (set) Token: 0x06001BB7 RID: 7095 RVA: 0x0006344C File Offset: 0x0006164C
		public AssetReference<VehicleAsset> TargetVehicle { get; protected set; }

		/// <summary>
		/// If set, overrides the default random paint color when loading a vehicle from a save file.
		/// Used to preserve colors of vehicles in existing saves.
		/// </summary>
		// Token: 0x170005BA RID: 1466
		// (get) Token: 0x06001BB8 RID: 7096 RVA: 0x00063455 File Offset: 0x00061655
		// (set) Token: 0x06001BB9 RID: 7097 RVA: 0x0006345D File Offset: 0x0006165D
		public Color32? LoadPaintColor { get; protected set; }

		/// <summary>
		/// If set, overrides the default random paint color when spawning a new vehicle.
		/// Optionally used to preserve colors of vehicles in spawn tables.
		/// </summary>
		// Token: 0x170005BB RID: 1467
		// (get) Token: 0x06001BBA RID: 7098 RVA: 0x00063466 File Offset: 0x00061666
		// (set) Token: 0x06001BBB RID: 7099 RVA: 0x0006346E File Offset: 0x0006166E
		public Color32? SpawnPaintColor { get; protected set; }

		// Token: 0x06001BBC RID: 7100 RVA: 0x00063478 File Offset: 0x00061678
		public override void PopulateAsset(Bundle bundle, DatDictionary data, Local localization)
		{
			base.PopulateAsset(bundle, data, localization);
			this.TargetVehicle = data.readAssetReference("TargetVehicle");
			Color32 color;
			if (data.TryParseColor32RGB("LoadPaintColor", out color))
			{
				this.LoadPaintColor = new Color32?(color);
			}
			Color32 color2;
			if (data.TryParseColor32RGB("SpawnPaintColor", out color2))
			{
				this.SpawnPaintColor = new Color32?(color2);
			}
		}
	}
}
