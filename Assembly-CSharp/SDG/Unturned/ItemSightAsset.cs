using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020002FF RID: 767
	public class ItemSightAsset : ItemCaliberAsset
	{
		// Token: 0x17000415 RID: 1045
		// (get) Token: 0x0600171B RID: 5915 RVA: 0x00054AB8 File Offset: 0x00052CB8
		public GameObject sight
		{
			get
			{
				return this._sight;
			}
		}

		// Token: 0x17000416 RID: 1046
		// (get) Token: 0x0600171C RID: 5916 RVA: 0x00054AC0 File Offset: 0x00052CC0
		public ELightingVision vision
		{
			get
			{
				return this._vision;
			}
		}

		/// <summary>
		/// Factor e.g. 2 is a 2x multiplier.
		/// Prior to 2022-04-11 this was the target field of view. (90/fov)
		/// </summary>
		// Token: 0x17000417 RID: 1047
		// (get) Token: 0x0600171D RID: 5917 RVA: 0x00054AC8 File Offset: 0x00052CC8
		// (set) Token: 0x0600171E RID: 5918 RVA: 0x00054AD0 File Offset: 0x00052CD0
		public float zoom { get; private set; }

		/// <summary>
		/// Zoom factor used in third-person view.
		/// </summary>
		// Token: 0x17000418 RID: 1048
		// (get) Token: 0x0600171F RID: 5919 RVA: 0x00054AD9 File Offset: 0x00052CD9
		// (set) Token: 0x06001720 RID: 5920 RVA: 0x00054AE1 File Offset: 0x00052CE1
		public float thirdPersonZoomFactor { get; private set; }

		// Token: 0x17000419 RID: 1049
		// (get) Token: 0x06001721 RID: 5921 RVA: 0x00054AEA File Offset: 0x00052CEA
		public bool isHolographic
		{
			get
			{
				return this._isHolographic;
			}
		}

		// Token: 0x06001722 RID: 5922 RVA: 0x00054AF4 File Offset: 0x00052CF4
		public override void BuildDescription(ItemDescriptionBuilder builder, Item itemInstance)
		{
			base.BuildDescription(builder, itemInstance);
			if (builder.shouldRestrictToLegacyContent)
			{
				return;
			}
			if (this.zoom != 1f)
			{
				builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_ZoomFactor", this.zoom), 10000);
			}
			if (this.thirdPersonZoomFactor != 1.25f)
			{
				builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_ThirdPersonZoomFactor", this.thirdPersonZoomFactor), 10001);
			}
		}

		// Token: 0x06001723 RID: 5923 RVA: 0x00054B78 File Offset: 0x00052D78
		public override void PopulateAsset(Bundle bundle, DatDictionary data, Local localization)
		{
			base.PopulateAsset(bundle, data, localization);
			this._sight = base.loadRequiredAsset<GameObject>(bundle, "Sight");
			if (data.ContainsKey("Vision"))
			{
				this._vision = (ELightingVision)Enum.Parse(typeof(ELightingVision), data.GetString("Vision", null), true);
				if (this.vision == ELightingVision.CIVILIAN)
				{
					this.nightvisionColor = data.LegacyParseColor32RGB("Nightvision_Color", LevelLighting.NIGHTVISION_CIVILIAN);
					this.nightvisionFogIntensity = data.ParseFloat("Nightvision_Fog_Intensity", 0.5f);
				}
				else if (this.vision == ELightingVision.MILITARY)
				{
					this.nightvisionColor = data.LegacyParseColor32RGB("Nightvision_Color", LevelLighting.NIGHTVISION_MILITARY);
					this.nightvisionFogIntensity = data.ParseFloat("Nightvision_Fog_Intensity", 0.25f);
				}
			}
			else
			{
				this._vision = ELightingVision.NONE;
			}
			this.zoom = Mathf.Max(1f, data.ParseFloat("Zoom", 0f));
			this.thirdPersonZoomFactor = Mathf.Max(1f, data.ParseFloat("ThirdPerson_Zoom", 1.25f));
			this.shouldZoomUsingEyes = data.ParseBool("Zoom_Using_Eyes", false);
			this.shouldOffsetScopeOverlayByOneTexel = data.ParseBool("Offset_Scope_Overlay_By_One_Texel", false);
			this._isHolographic = data.ContainsKey("Holographic");
			this.distanceMarkers = data.ParseListOfStructs<ItemSightAsset.DistanceMarker>("DistanceMarkers");
		}

		// Token: 0x04000A29 RID: 2601
		protected GameObject _sight;

		// Token: 0x04000A2A RID: 2602
		private ELightingVision _vision;

		// Token: 0x04000A2B RID: 2603
		public Color nightvisionColor;

		// Token: 0x04000A2C RID: 2604
		public float nightvisionFogIntensity;

		// Token: 0x04000A2F RID: 2607
		private bool _isHolographic;

		/// <summary>
		/// Whether main camera field of view should zoom without scope camera / scope overlay.
		/// </summary>
		// Token: 0x04000A30 RID: 2608
		public bool shouldZoomUsingEyes;

		/// <summary>
		/// If true, scale scope overly by 1 texel to keep "middle" pixel centered.
		/// </summary>
		// Token: 0x04000A31 RID: 2609
		public bool shouldOffsetScopeOverlayByOneTexel;

		// Token: 0x04000A32 RID: 2610
		public List<ItemSightAsset.DistanceMarker> distanceMarkers;

		// Token: 0x02000920 RID: 2336
		public struct DistanceMarker : IDatParseable
		{
			// Token: 0x06004A79 RID: 19065 RVA: 0x001B1758 File Offset: 0x001AF958
			public bool TryParse(IDatNode node)
			{
				DatDictionary datDictionary = node as DatDictionary;
				if (datDictionary == null)
				{
					return false;
				}
				if (!datDictionary.TryParseFloat("Distance", out this.distance))
				{
					return false;
				}
				this.lineOffset = datDictionary.ParseFloat("LineOffset", 0f);
				this.lineWidth = datDictionary.ParseFloat("LineWidth", 0.05f);
				this.side = datDictionary.ParseEnum<ItemSightAsset.DistanceMarker.ESide>("Side", ItemSightAsset.DistanceMarker.ESide.Right);
				this.hasLabel = datDictionary.ParseBool("HasLabel", true);
				this.color = datDictionary.ParseColor32RGB("Color", default(Color32));
				return true;
			}

			// Token: 0x04003260 RID: 12896
			public float distance;

			/// <summary>
			/// [0, 1] local distance from center to start of line.
			/// </summary>
			// Token: 0x04003261 RID: 12897
			public float lineOffset;

			/// <summary>
			/// [0, 1] local width of horizontal line.
			/// </summary>
			// Token: 0x04003262 RID: 12898
			public float lineWidth;

			/// <summary>
			/// Whether line/number are on left or right side of the center line.
			/// </summary>
			// Token: 0x04003263 RID: 12899
			public ItemSightAsset.DistanceMarker.ESide side;

			/// <summary>
			/// If true, text label for distance is visible.
			/// </summary>
			// Token: 0x04003264 RID: 12900
			public bool hasLabel;

			// Token: 0x04003265 RID: 12901
			public Color32 color;

			// Token: 0x02000A35 RID: 2613
			public enum ESide
			{
				// Token: 0x0400355C RID: 13660
				Left,
				// Token: 0x0400355D RID: 13661
				Right
			}
		}
	}
}
