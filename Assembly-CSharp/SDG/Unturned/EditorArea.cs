using System;
using SDG.Framework.Devkit;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020003FC RID: 1020
	public class EditorArea : MonoBehaviour
	{
		// Token: 0x14000079 RID: 121
		// (add) Token: 0x06001E21 RID: 7713 RVA: 0x0006DA18 File Offset: 0x0006BC18
		// (remove) Token: 0x06001E22 RID: 7714 RVA: 0x0006DA4C File Offset: 0x0006BC4C
		public static event EditorAreaRegisteredHandler registered;

		// Token: 0x17000620 RID: 1568
		// (get) Token: 0x06001E23 RID: 7715 RVA: 0x0006DA7F File Offset: 0x0006BC7F
		// (set) Token: 0x06001E24 RID: 7716 RVA: 0x0006DA86 File Offset: 0x0006BC86
		public static EditorArea instance { get; protected set; }

		// Token: 0x17000621 RID: 1569
		// (get) Token: 0x06001E25 RID: 7717 RVA: 0x0006DA8E File Offset: 0x0006BC8E
		public byte region_x
		{
			get
			{
				return this._region_x;
			}
		}

		// Token: 0x17000622 RID: 1570
		// (get) Token: 0x06001E26 RID: 7718 RVA: 0x0006DA96 File Offset: 0x0006BC96
		public byte region_y
		{
			get
			{
				return this._region_y;
			}
		}

		// Token: 0x17000623 RID: 1571
		// (get) Token: 0x06001E27 RID: 7719 RVA: 0x0006DA9E File Offset: 0x0006BC9E
		public byte bound
		{
			get
			{
				return this._bound;
			}
		}

		// Token: 0x17000624 RID: 1572
		// (get) Token: 0x06001E28 RID: 7720 RVA: 0x0006DAA6 File Offset: 0x0006BCA6
		// (set) Token: 0x06001E29 RID: 7721 RVA: 0x0006DAAE File Offset: 0x0006BCAE
		public IAmbianceNode effectNode { get; private set; }

		// Token: 0x06001E2A RID: 7722 RVA: 0x0006DAB7 File Offset: 0x0006BCB7
		protected void triggerRegistered()
		{
			EditorAreaRegisteredHandler editorAreaRegisteredHandler = EditorArea.registered;
			if (editorAreaRegisteredHandler == null)
			{
				return;
			}
			editorAreaRegisteredHandler(this);
		}

		// Token: 0x06001E2B RID: 7723 RVA: 0x0006DACC File Offset: 0x0006BCCC
		private void Update()
		{
			byte b;
			byte b2;
			if (Regions.tryGetCoordinate(base.transform.position, out b, out b2) && (b != this.region_x || b2 != this.region_y))
			{
				byte region_x = this.region_x;
				byte region_y = this.region_y;
				this._region_x = b;
				this._region_y = b2;
				EditorRegionUpdated editorRegionUpdated = this.onRegionUpdated;
				if (editorRegionUpdated != null)
				{
					editorRegionUpdated(region_x, region_y, b, b2);
				}
			}
			byte b3;
			LevelNavigation.tryGetBounds(base.transform.position, out b3);
			if (b3 != this.bound)
			{
				byte bound = this.bound;
				this._bound = b3;
				EditorBoundUpdated editorBoundUpdated = this.onBoundUpdated;
				if (editorBoundUpdated != null)
				{
					editorBoundUpdated(bound, b3);
				}
			}
			this.effectNode = VolumeManager<AmbianceVolume, AmbianceVolumeManager>.Get().GetFirstOverlappingVolume(base.transform.position);
			LevelLighting.updateLocal(MainCamera.instance.transform.position, 0f, this.effectNode);
		}

		// Token: 0x06001E2C RID: 7724 RVA: 0x0006DBAD File Offset: 0x0006BDAD
		private void Start()
		{
			this._region_x = byte.MaxValue;
			this._region_y = byte.MaxValue;
			this._bound = byte.MaxValue;
			EditorArea.instance = this;
			LevelLighting.updateLighting();
			this.triggerRegistered();
		}

		// Token: 0x04000E6E RID: 3694
		public EditorRegionUpdated onRegionUpdated;

		// Token: 0x04000E6F RID: 3695
		public EditorBoundUpdated onBoundUpdated;

		// Token: 0x04000E70 RID: 3696
		private byte _region_x;

		// Token: 0x04000E71 RID: 3697
		private byte _region_y;

		// Token: 0x04000E72 RID: 3698
		private byte _bound;
	}
}
