using System;
using System.Collections.Generic;
using SDG.Framework.Devkit;
using SDG.Framework.Devkit.Tools;
using SDG.Framework.Devkit.Transactions;
using SDG.Framework.Landscapes;
using SDG.Framework.Rendering;
using SDG.Framework.Utilities;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200041B RID: 1051
	public class TerrainEditor : IDevkitTool
	{
		// Token: 0x17000656 RID: 1622
		// (get) Token: 0x06001EE3 RID: 7907 RVA: 0x00073805 File Offset: 0x00071A05
		// (set) Token: 0x06001EE4 RID: 7908 RVA: 0x0007380C File Offset: 0x00071A0C
		public static TerrainEditor.EDevkitLandscapeToolMode toolMode
		{
			get
			{
				return TerrainEditor._toolMode;
			}
			set
			{
				if (TerrainEditor.toolMode == value)
				{
					return;
				}
				TerrainEditor.EDevkitLandscapeToolMode toolMode = TerrainEditor.toolMode;
				TerrainEditor._toolMode = value;
				TerrainEditor.DevkitLandscapeToolModeChangedHandler devkitLandscapeToolModeChangedHandler = TerrainEditor.toolModeChanged;
				if (devkitLandscapeToolModeChangedHandler == null)
				{
					return;
				}
				devkitLandscapeToolModeChangedHandler(toolMode, TerrainEditor.toolMode);
			}
		}

		// Token: 0x1400007A RID: 122
		// (add) Token: 0x06001EE5 RID: 7909 RVA: 0x00073844 File Offset: 0x00071A44
		// (remove) Token: 0x06001EE6 RID: 7910 RVA: 0x00073878 File Offset: 0x00071A78
		public static event TerrainEditor.DevkitLandscapeToolModeChangedHandler toolModeChanged;

		// Token: 0x17000657 RID: 1623
		// (get) Token: 0x06001EE7 RID: 7911 RVA: 0x000738AB File Offset: 0x00071AAB
		// (set) Token: 0x06001EE8 RID: 7912 RVA: 0x000738B4 File Offset: 0x00071AB4
		public static LandscapeTile selectedTile
		{
			get
			{
				return TerrainEditor._selectedTile;
			}
			set
			{
				if (TerrainEditor.selectedTile == value)
				{
					return;
				}
				LandscapeTile selectedTile = TerrainEditor.selectedTile;
				TerrainEditor._selectedTile = value;
				TerrainEditor.DevkitLandscapeToolSelectedTileChangedHandler devkitLandscapeToolSelectedTileChangedHandler = TerrainEditor.selectedTileChanged;
				if (devkitLandscapeToolSelectedTileChangedHandler == null)
				{
					return;
				}
				devkitLandscapeToolSelectedTileChangedHandler(selectedTile, TerrainEditor.selectedTile);
			}
		}

		// Token: 0x1400007B RID: 123
		// (add) Token: 0x06001EE9 RID: 7913 RVA: 0x000738EC File Offset: 0x00071AEC
		// (remove) Token: 0x06001EEA RID: 7914 RVA: 0x00073920 File Offset: 0x00071B20
		public static event TerrainEditor.DevkitLandscapeToolSelectedTileChangedHandler selectedTileChanged;

		// Token: 0x17000658 RID: 1624
		// (get) Token: 0x06001EEB RID: 7915 RVA: 0x00073953 File Offset: 0x00071B53
		public virtual float heightmapAdjustSensitivity
		{
			get
			{
				return DevkitLandscapeToolHeightmapOptions.adjustSensitivity;
			}
		}

		// Token: 0x17000659 RID: 1625
		// (get) Token: 0x06001EEC RID: 7916 RVA: 0x0007395A File Offset: 0x00071B5A
		public virtual float heightmapFlattenSensitivity
		{
			get
			{
				return DevkitLandscapeToolHeightmapOptions.flattenSensitivity;
			}
		}

		// Token: 0x1700065A RID: 1626
		// (get) Token: 0x06001EED RID: 7917 RVA: 0x00073961 File Offset: 0x00071B61
		// (set) Token: 0x06001EEE RID: 7918 RVA: 0x0007396D File Offset: 0x00071B6D
		public virtual float heightmapBrushRadius
		{
			get
			{
				return DevkitLandscapeToolHeightmapOptions.instance.brushRadius;
			}
			set
			{
				DevkitLandscapeToolHeightmapOptions.instance.brushRadius = value;
			}
		}

		// Token: 0x1700065B RID: 1627
		// (get) Token: 0x06001EEF RID: 7919 RVA: 0x0007397A File Offset: 0x00071B7A
		// (set) Token: 0x06001EF0 RID: 7920 RVA: 0x00073986 File Offset: 0x00071B86
		public virtual float heightmapBrushFalloff
		{
			get
			{
				return DevkitLandscapeToolHeightmapOptions.instance.brushFalloff;
			}
			set
			{
				DevkitLandscapeToolHeightmapOptions.instance.brushFalloff = value;
			}
		}

		// Token: 0x1700065C RID: 1628
		// (get) Token: 0x06001EF1 RID: 7921 RVA: 0x00073994 File Offset: 0x00071B94
		// (set) Token: 0x06001EF2 RID: 7922 RVA: 0x000739D0 File Offset: 0x00071BD0
		public virtual float heightmapBrushStrength
		{
			get
			{
				TerrainEditor.EDevkitLandscapeToolHeightmapMode edevkitLandscapeToolHeightmapMode = TerrainEditor.heightmapMode;
				if (edevkitLandscapeToolHeightmapMode == TerrainEditor.EDevkitLandscapeToolHeightmapMode.FLATTEN)
				{
					return DevkitLandscapeToolHeightmapOptions.instance.flattenStrength;
				}
				if (edevkitLandscapeToolHeightmapMode != TerrainEditor.EDevkitLandscapeToolHeightmapMode.SMOOTH)
				{
					return DevkitLandscapeToolHeightmapOptions.instance.brushStrength;
				}
				return DevkitLandscapeToolHeightmapOptions.instance.smoothStrength;
			}
			set
			{
				TerrainEditor.EDevkitLandscapeToolHeightmapMode edevkitLandscapeToolHeightmapMode = TerrainEditor.heightmapMode;
				if (edevkitLandscapeToolHeightmapMode == TerrainEditor.EDevkitLandscapeToolHeightmapMode.FLATTEN)
				{
					DevkitLandscapeToolHeightmapOptions.instance.flattenStrength = value;
					return;
				}
				if (edevkitLandscapeToolHeightmapMode != TerrainEditor.EDevkitLandscapeToolHeightmapMode.SMOOTH)
				{
					DevkitLandscapeToolHeightmapOptions.instance.brushStrength = value;
					return;
				}
				DevkitLandscapeToolHeightmapOptions.instance.smoothStrength = value;
			}
		}

		// Token: 0x1700065D RID: 1629
		// (get) Token: 0x06001EF3 RID: 7923 RVA: 0x00073A0E File Offset: 0x00071C0E
		// (set) Token: 0x06001EF4 RID: 7924 RVA: 0x00073A1A File Offset: 0x00071C1A
		public virtual float heightmapFlattenTarget
		{
			get
			{
				return DevkitLandscapeToolHeightmapOptions.instance.flattenTarget;
			}
			set
			{
				DevkitLandscapeToolHeightmapOptions.instance.flattenTarget = value;
			}
		}

		// Token: 0x1700065E RID: 1630
		// (get) Token: 0x06001EF5 RID: 7925 RVA: 0x00073A27 File Offset: 0x00071C27
		// (set) Token: 0x06001EF6 RID: 7926 RVA: 0x00073A33 File Offset: 0x00071C33
		public virtual uint heightmapMaxPreviewSamples
		{
			get
			{
				return DevkitLandscapeToolHeightmapOptions.instance.maxPreviewSamples;
			}
			set
			{
				DevkitLandscapeToolHeightmapOptions.instance.maxPreviewSamples = value;
			}
		}

		// Token: 0x1700065F RID: 1631
		// (get) Token: 0x06001EF7 RID: 7927 RVA: 0x00073A40 File Offset: 0x00071C40
		public virtual float splatmapPaintSensitivity
		{
			get
			{
				return DevkitLandscapeToolSplatmapOptions.paintSensitivity;
			}
		}

		// Token: 0x17000660 RID: 1632
		// (get) Token: 0x06001EF8 RID: 7928 RVA: 0x00073A47 File Offset: 0x00071C47
		// (set) Token: 0x06001EF9 RID: 7929 RVA: 0x00073A70 File Offset: 0x00071C70
		public virtual float splatmapBrushRadius
		{
			get
			{
				if (TerrainEditor.splatmapMode == TerrainEditor.EDevkitLandscapeToolSplatmapMode.CUT)
				{
					return Mathf.Min(32f, DevkitLandscapeToolSplatmapOptions.instance.brushRadius);
				}
				return DevkitLandscapeToolSplatmapOptions.instance.brushRadius;
			}
			set
			{
				DevkitLandscapeToolSplatmapOptions.instance.brushRadius = value;
			}
		}

		// Token: 0x17000661 RID: 1633
		// (get) Token: 0x06001EFA RID: 7930 RVA: 0x00073A7D File Offset: 0x00071C7D
		// (set) Token: 0x06001EFB RID: 7931 RVA: 0x00073A89 File Offset: 0x00071C89
		public virtual float splatmapBrushFalloff
		{
			get
			{
				return DevkitLandscapeToolSplatmapOptions.instance.brushFalloff;
			}
			set
			{
				DevkitLandscapeToolSplatmapOptions.instance.brushFalloff = value;
			}
		}

		// Token: 0x17000662 RID: 1634
		// (get) Token: 0x06001EFC RID: 7932 RVA: 0x00073A98 File Offset: 0x00071C98
		// (set) Token: 0x06001EFD RID: 7933 RVA: 0x00073AD4 File Offset: 0x00071CD4
		public virtual float splatmapBrushStrength
		{
			get
			{
				TerrainEditor.EDevkitLandscapeToolSplatmapMode edevkitLandscapeToolSplatmapMode = TerrainEditor.splatmapMode;
				if (edevkitLandscapeToolSplatmapMode == TerrainEditor.EDevkitLandscapeToolSplatmapMode.AUTO)
				{
					return DevkitLandscapeToolSplatmapOptions.instance.autoStrength;
				}
				if (edevkitLandscapeToolSplatmapMode != TerrainEditor.EDevkitLandscapeToolSplatmapMode.SMOOTH)
				{
					return DevkitLandscapeToolSplatmapOptions.instance.brushStrength;
				}
				return DevkitLandscapeToolSplatmapOptions.instance.smoothStrength;
			}
			set
			{
				TerrainEditor.EDevkitLandscapeToolSplatmapMode edevkitLandscapeToolSplatmapMode = TerrainEditor.splatmapMode;
				if (edevkitLandscapeToolSplatmapMode == TerrainEditor.EDevkitLandscapeToolSplatmapMode.AUTO)
				{
					DevkitLandscapeToolSplatmapOptions.instance.autoStrength = value;
					return;
				}
				if (edevkitLandscapeToolSplatmapMode != TerrainEditor.EDevkitLandscapeToolSplatmapMode.SMOOTH)
				{
					DevkitLandscapeToolSplatmapOptions.instance.brushStrength = value;
					return;
				}
				DevkitLandscapeToolSplatmapOptions.instance.smoothStrength = value;
			}
		}

		// Token: 0x17000663 RID: 1635
		// (get) Token: 0x06001EFE RID: 7934 RVA: 0x00073B12 File Offset: 0x00071D12
		// (set) Token: 0x06001EFF RID: 7935 RVA: 0x00073B1E File Offset: 0x00071D1E
		public virtual bool splatmapUseWeightTarget
		{
			get
			{
				return DevkitLandscapeToolSplatmapOptions.instance.useWeightTarget;
			}
			set
			{
				DevkitLandscapeToolSplatmapOptions.instance.useWeightTarget = value;
			}
		}

		// Token: 0x17000664 RID: 1636
		// (get) Token: 0x06001F00 RID: 7936 RVA: 0x00073B2B File Offset: 0x00071D2B
		// (set) Token: 0x06001F01 RID: 7937 RVA: 0x00073B37 File Offset: 0x00071D37
		public virtual float splatmapWeightTarget
		{
			get
			{
				return DevkitLandscapeToolSplatmapOptions.instance.weightTarget;
			}
			set
			{
				DevkitLandscapeToolSplatmapOptions.instance.weightTarget = value;
			}
		}

		// Token: 0x17000665 RID: 1637
		// (get) Token: 0x06001F02 RID: 7938 RVA: 0x00073B44 File Offset: 0x00071D44
		// (set) Token: 0x06001F03 RID: 7939 RVA: 0x00073B50 File Offset: 0x00071D50
		public virtual uint splatmapMaxPreviewSamples
		{
			get
			{
				return DevkitLandscapeToolSplatmapOptions.instance.maxPreviewSamples;
			}
			set
			{
				DevkitLandscapeToolSplatmapOptions.instance.maxPreviewSamples = value;
			}
		}

		// Token: 0x17000666 RID: 1638
		// (get) Token: 0x06001F04 RID: 7940 RVA: 0x00073B5D File Offset: 0x00071D5D
		// (set) Token: 0x06001F05 RID: 7941 RVA: 0x00073B64 File Offset: 0x00071D64
		public static AssetReference<LandscapeMaterialAsset> splatmapMaterialTarget
		{
			get
			{
				return TerrainEditor._splatmapMaterialTarget;
			}
			set
			{
				TerrainEditor._splatmapMaterialTarget = value;
				TerrainEditor.splatmapMaterialTargetAsset = Assets.find<LandscapeMaterialAsset>(TerrainEditor.splatmapMaterialTarget);
			}
		}

		// Token: 0x17000667 RID: 1639
		// (get) Token: 0x06001F06 RID: 7942 RVA: 0x00073B7B File Offset: 0x00071D7B
		// (set) Token: 0x06001F07 RID: 7943 RVA: 0x00073B91 File Offset: 0x00071D91
		protected virtual float brushRadius
		{
			get
			{
				if (TerrainEditor.toolMode == TerrainEditor.EDevkitLandscapeToolMode.HEIGHTMAP)
				{
					return this.heightmapBrushRadius;
				}
				return this.splatmapBrushRadius;
			}
			set
			{
				if (TerrainEditor.toolMode == TerrainEditor.EDevkitLandscapeToolMode.HEIGHTMAP)
				{
					this.heightmapBrushRadius = value;
					return;
				}
				this.splatmapBrushRadius = value;
			}
		}

		// Token: 0x17000668 RID: 1640
		// (get) Token: 0x06001F08 RID: 7944 RVA: 0x00073BA9 File Offset: 0x00071DA9
		// (set) Token: 0x06001F09 RID: 7945 RVA: 0x00073BBF File Offset: 0x00071DBF
		protected virtual float brushFalloff
		{
			get
			{
				if (TerrainEditor.toolMode == TerrainEditor.EDevkitLandscapeToolMode.HEIGHTMAP)
				{
					return this.heightmapBrushFalloff;
				}
				return this.splatmapBrushFalloff;
			}
			set
			{
				if (TerrainEditor.toolMode == TerrainEditor.EDevkitLandscapeToolMode.HEIGHTMAP)
				{
					this.heightmapBrushFalloff = value;
					return;
				}
				this.splatmapBrushFalloff = value;
			}
		}

		// Token: 0x17000669 RID: 1641
		// (get) Token: 0x06001F0A RID: 7946 RVA: 0x00073BD7 File Offset: 0x00071DD7
		// (set) Token: 0x06001F0B RID: 7947 RVA: 0x00073BED File Offset: 0x00071DED
		protected virtual float brushStrength
		{
			get
			{
				if (TerrainEditor.toolMode == TerrainEditor.EDevkitLandscapeToolMode.HEIGHTMAP)
				{
					return this.heightmapBrushStrength;
				}
				return this.splatmapBrushStrength;
			}
			set
			{
				if (TerrainEditor.toolMode == TerrainEditor.EDevkitLandscapeToolMode.HEIGHTMAP)
				{
					this.heightmapBrushStrength = value;
					return;
				}
				this.splatmapBrushStrength = value;
			}
		}

		// Token: 0x1700066A RID: 1642
		// (get) Token: 0x06001F0C RID: 7948 RVA: 0x00073C05 File Offset: 0x00071E05
		// (set) Token: 0x06001F0D RID: 7949 RVA: 0x00073C1B File Offset: 0x00071E1B
		protected virtual uint maxPreviewSamples
		{
			get
			{
				if (TerrainEditor.toolMode == TerrainEditor.EDevkitLandscapeToolMode.HEIGHTMAP)
				{
					return this.heightmapMaxPreviewSamples;
				}
				return this.splatmapMaxPreviewSamples;
			}
			set
			{
				if (TerrainEditor.toolMode == TerrainEditor.EDevkitLandscapeToolMode.HEIGHTMAP)
				{
					this.heightmapMaxPreviewSamples = value;
					return;
				}
				this.splatmapMaxPreviewSamples = value;
			}
		}

		// Token: 0x1700066B RID: 1643
		// (get) Token: 0x06001F0E RID: 7950 RVA: 0x00073C33 File Offset: 0x00071E33
		protected virtual bool isChangingBrush
		{
			get
			{
				return this.isChangingBrushRadius || this.isChangingBrushFalloff || this.isChangingBrushStrength || this.isChangingWeightTarget;
			}
		}

		// Token: 0x06001F0F RID: 7951 RVA: 0x00073C55 File Offset: 0x00071E55
		protected virtual void beginChangeHotkeyTransaction()
		{
			DevkitTransactionUtility.beginGenericTransaction();
			DevkitTransactionUtility.recordObjectDelta(DevkitLandscapeToolHeightmapOptions.instance);
			DevkitTransactionUtility.recordObjectDelta(DevkitLandscapeToolSplatmapOptions.instance);
		}

		// Token: 0x06001F10 RID: 7952 RVA: 0x00073C70 File Offset: 0x00071E70
		protected virtual void endChangeHotkeyTransaction()
		{
			DevkitTransactionUtility.endGenericTransaction();
		}

		// Token: 0x06001F11 RID: 7953 RVA: 0x00073C78 File Offset: 0x00071E78
		public virtual void update()
		{
			Ray ray = EditorInteract.ray;
			Plane plane = default(Plane);
			plane.SetNormalAndPosition(Vector3.up, Vector3.zero);
			float num;
			this.isPointerOnTilePlane = plane.Raycast(ray, out num);
			this.tilePlanePosition = ray.origin + ray.direction * num;
			this.pointerTileCoord = new LandscapeCoord(this.tilePlanePosition);
			this.isTileVisible = this.isPointerOnTilePlane;
			this.previewSamples.Clear();
			RaycastHit raycastHit;
			this.isPointerOnLandscape = Physics.Raycast(ray, out raycastHit, 8192f, -2146435072);
			this.pointerWorldPosition = raycastHit.point;
			if (!EditorInteract.isFlying && Glazier.Get().ShouldGameProcessInput)
			{
				if (InputEx.GetKeyDown(KeyCode.B))
				{
					this.isChangingBrushRadius = true;
					this.beginChangeHotkeyTransaction();
				}
				if (InputEx.GetKeyDown(KeyCode.F))
				{
					this.isChangingBrushFalloff = true;
					this.beginChangeHotkeyTransaction();
				}
				if (InputEx.GetKeyDown(KeyCode.V))
				{
					this.isChangingBrushStrength = true;
					this.beginChangeHotkeyTransaction();
				}
				if (InputEx.GetKeyDown(KeyCode.G))
				{
					this.isChangingWeightTarget = true;
					this.beginChangeHotkeyTransaction();
				}
			}
			if (InputEx.GetKeyUp(KeyCode.B))
			{
				this.isChangingBrushRadius = false;
				this.endChangeHotkeyTransaction();
			}
			if (InputEx.GetKeyUp(KeyCode.F))
			{
				this.isChangingBrushFalloff = false;
				this.endChangeHotkeyTransaction();
			}
			if (InputEx.GetKeyUp(KeyCode.V))
			{
				this.isChangingBrushStrength = false;
				this.endChangeHotkeyTransaction();
			}
			if (InputEx.GetKeyUp(KeyCode.G))
			{
				this.isChangingWeightTarget = false;
				this.endChangeHotkeyTransaction();
			}
			if (this.isChangingBrush)
			{
				Plane plane2 = default(Plane);
				plane2.SetNormalAndPosition(Vector3.up, this.brushWorldPosition);
				float d;
				plane2.Raycast(ray, out d);
				this.changePlanePosition = ray.origin + ray.direction * d;
				if (this.isChangingBrushRadius)
				{
					this.brushRadius = (this.changePlanePosition - this.brushWorldPosition).magnitude;
				}
				if (this.isChangingBrushFalloff)
				{
					this.brushFalloff = Mathf.Clamp01((this.changePlanePosition - this.brushWorldPosition).magnitude / this.brushRadius);
				}
				if (this.isChangingBrushStrength)
				{
					this.brushStrength = (this.changePlanePosition - this.brushWorldPosition).magnitude / this.brushRadius;
				}
				if (this.isChangingWeightTarget)
				{
					this.splatmapWeightTarget = Mathf.Clamp01((this.changePlanePosition - this.brushWorldPosition).magnitude / this.brushRadius);
				}
			}
			else
			{
				this.brushWorldPosition = this.pointerWorldPosition;
				if (TerrainEditor.toolMode == TerrainEditor.EDevkitLandscapeToolMode.HEIGHTMAP && TerrainEditor.heightmapMode == TerrainEditor.EDevkitLandscapeToolHeightmapMode.FLATTEN)
				{
					Plane plane3 = default(Plane);
					plane3.SetNormalAndPosition(Vector3.up, new Vector3(0f, this.heightmapFlattenTarget, 0f));
					float d2;
					if (plane3.Raycast(ray, out d2))
					{
						this.flattenPlanePosition = ray.origin + ray.direction * d2;
						this.brushWorldPosition = this.flattenPlanePosition;
						if (!this.isPointerOnLandscape)
						{
							this.isPointerOnLandscape = Landscape.isPointerInTile(this.brushWorldPosition);
						}
					}
					else
					{
						this.flattenPlanePosition = new Vector3(this.brushWorldPosition.x, this.heightmapFlattenTarget, this.brushWorldPosition.z);
					}
				}
			}
			this.isBrushVisible = (this.isPointerOnLandscape || this.isChangingBrush);
			if (!EditorInteract.isFlying && Glazier.Get().ShouldGameProcessInput)
			{
				if (TerrainEditor.toolMode == TerrainEditor.EDevkitLandscapeToolMode.TILE)
				{
					if (InputEx.GetKeyDown(KeyCode.Mouse0))
					{
						if (this.isPointerOnTilePlane)
						{
							LandscapeTile tile = Landscape.getTile(this.pointerTileCoord);
							if (tile == null)
							{
								if (num < 4096f)
								{
									LandscapeTile landscapeTile = Landscape.addTile(this.pointerTileCoord);
									if (landscapeTile != null)
									{
										landscapeTile.readHeightmaps();
										landscapeTile.readSplatmaps();
										landscapeTile.updatePrototypes();
										Landscape.linkNeighbors();
										Landscape.reconcileNeighbors(landscapeTile);
										Landscape.applyLOD();
										LevelHierarchy.MarkDirty();
									}
									TerrainEditor.selectedTile = landscapeTile;
								}
								else
								{
									TerrainEditor.selectedTile = null;
								}
							}
							else if (TerrainEditor.selectedTile != null && TerrainEditor.selectedTile.coord == this.pointerTileCoord)
							{
								TerrainEditor.selectedTile = null;
							}
							else
							{
								TerrainEditor.selectedTile = tile;
							}
						}
						else
						{
							TerrainEditor.selectedTile = null;
						}
					}
					if (InputEx.GetKeyDown(KeyCode.Delete) && TerrainEditor.selectedTile != null)
					{
						Landscape.removeTile(TerrainEditor.selectedTile.coord);
						TerrainEditor.selectedTile = null;
						LevelHierarchy.MarkDirty();
					}
				}
				else if (TerrainEditor.toolMode == TerrainEditor.EDevkitLandscapeToolMode.HEIGHTMAP)
				{
					if (InputEx.GetKeyDown(KeyCode.Q))
					{
						TerrainEditor.heightmapMode = TerrainEditor.EDevkitLandscapeToolHeightmapMode.ADJUST;
					}
					if (InputEx.GetKeyDown(KeyCode.W))
					{
						TerrainEditor.heightmapMode = TerrainEditor.EDevkitLandscapeToolHeightmapMode.FLATTEN;
					}
					if (InputEx.GetKeyDown(KeyCode.E))
					{
						TerrainEditor.heightmapMode = TerrainEditor.EDevkitLandscapeToolHeightmapMode.SMOOTH;
					}
					if (InputEx.GetKeyDown(KeyCode.R))
					{
						TerrainEditor.heightmapMode = TerrainEditor.EDevkitLandscapeToolHeightmapMode.RAMP;
					}
					if (TerrainEditor.heightmapMode == TerrainEditor.EDevkitLandscapeToolHeightmapMode.FLATTEN)
					{
						if (InputEx.GetKeyDown(KeyCode.LeftAlt))
						{
							this.isSamplingFlattenTarget = true;
						}
						if (InputEx.GetKeyUp(KeyCode.Mouse0) && this.isSamplingFlattenTarget)
						{
							RaycastHit raycastHit2;
							if (Physics.Raycast(ray, out raycastHit2, 8192f))
							{
								this.heightmapFlattenTarget = raycastHit2.point.y;
							}
							this.isSamplingFlattenTarget = false;
						}
					}
					if (!this.isSamplingFlattenTarget && this.isPointerOnLandscape)
					{
						if (TerrainEditor.heightmapMode == TerrainEditor.EDevkitLandscapeToolHeightmapMode.RAMP)
						{
							if (InputEx.GetKeyDown(KeyCode.Mouse0))
							{
								this.heightmapRampBeginPosition = this.pointerWorldPosition;
								this.isSamplingRampPositions = true;
								DevkitTransactionManager.beginTransaction("Heightmap");
								Landscape.clearHeightmapTransactions();
							}
							if (InputEx.GetKeyDown(KeyCode.R))
							{
								this.isSamplingRampPositions = false;
							}
							this.heightmapRampEndPosition = this.pointerWorldPosition;
							if (this.isSamplingRampPositions && new Vector2(this.heightmapRampBeginPosition.x - this.heightmapRampEndPosition.x, this.heightmapRampBeginPosition.z - this.heightmapRampEndPosition.z).magnitude > 1f)
							{
								Vector3 vector = new Vector3(Mathf.Min(this.heightmapRampBeginPosition.x, this.heightmapRampEndPosition.x), Mathf.Min(this.heightmapRampBeginPosition.y, this.heightmapRampEndPosition.y), Mathf.Min(this.heightmapRampBeginPosition.z, this.heightmapRampEndPosition.z));
								Vector3 vector2 = new Vector3(Mathf.Max(this.heightmapRampBeginPosition.x, this.heightmapRampEndPosition.x), Mathf.Max(this.heightmapRampBeginPosition.y, this.heightmapRampEndPosition.y), Mathf.Max(this.heightmapRampBeginPosition.z, this.heightmapRampEndPosition.z));
								vector.x -= this.heightmapBrushRadius;
								vector.z -= this.heightmapBrushRadius;
								vector2.x += this.heightmapBrushRadius;
								vector2.z += this.heightmapBrushRadius;
								Landscape.getHeightmapVertices(new Bounds((vector + vector2) / 2f, vector2 - vector), new Landscape.LandscapeGetHeightmapVerticesHandler(this.handleHeightmapGetVerticesRamp));
							}
						}
						else
						{
							if (InputEx.GetKeyDown(KeyCode.Mouse0))
							{
								DevkitTransactionManager.beginTransaction("Heightmap");
								Landscape.clearHeightmapTransactions();
							}
							Bounds bounds = new Bounds(this.brushWorldPosition, new Vector3(this.heightmapBrushRadius * 2f, 0f, this.heightmapBrushRadius * 2f));
							Landscape.getHeightmapVertices(bounds, new Landscape.LandscapeGetHeightmapVerticesHandler(this.handleHeightmapGetVerticesBrush));
							if (InputEx.GetKey(KeyCode.Mouse0))
							{
								if (TerrainEditor.heightmapMode == TerrainEditor.EDevkitLandscapeToolHeightmapMode.ADJUST)
								{
									Landscape.writeHeightmap(bounds, new Landscape.LandscapeWriteHeightmapHandler(this.handleHeightmapWriteAdjust));
								}
								else if (TerrainEditor.heightmapMode == TerrainEditor.EDevkitLandscapeToolHeightmapMode.FLATTEN)
								{
									bounds.center = this.flattenPlanePosition;
									Landscape.writeHeightmap(bounds, new Landscape.LandscapeWriteHeightmapHandler(this.handleHeightmapWriteFlatten));
								}
								else if (TerrainEditor.heightmapMode == TerrainEditor.EDevkitLandscapeToolHeightmapMode.SMOOTH)
								{
									if (DevkitLandscapeToolHeightmapOptions.instance.smoothMethod == EDevkitLandscapeToolHeightmapSmoothMethod.BRUSH_AVERAGE)
									{
										this.heightmapSmoothSampleCount = 0;
										this.heightmapSmoothSampleAverage = 0f;
										Landscape.readHeightmap(bounds, new Landscape.LandscapeReadHeightmapHandler(this.HandleHeightmapReadBrushAverage));
										if (this.heightmapSmoothSampleCount > 0)
										{
											this.heightmapSmoothTarget = this.heightmapSmoothSampleAverage / (float)this.heightmapSmoothSampleCount;
										}
										else
										{
											this.heightmapSmoothTarget = 0f;
										}
									}
									else if (DevkitLandscapeToolHeightmapOptions.instance.smoothMethod == EDevkitLandscapeToolHeightmapSmoothMethod.PIXEL_AVERAGE)
									{
										Bounds worldBounds = bounds;
										worldBounds.Expand(Landscape.HEIGHTMAP_WORLD_UNIT * 2f);
										Landscape.readHeightmap(worldBounds, new Landscape.LandscapeReadHeightmapHandler(this.HandleHeightmapReadPixelSmooth));
									}
									Landscape.writeHeightmap(bounds, new Landscape.LandscapeWriteHeightmapHandler(this.handleHeightmapWriteSmooth));
									if (DevkitLandscapeToolHeightmapOptions.instance.smoothMethod == EDevkitLandscapeToolHeightmapSmoothMethod.PIXEL_AVERAGE)
									{
										this.ReleaseHeightmapPixelSmoothBuffer();
									}
								}
							}
						}
					}
				}
				else if (TerrainEditor.toolMode == TerrainEditor.EDevkitLandscapeToolMode.SPLATMAP)
				{
					if (InputEx.GetKeyDown(KeyCode.Q))
					{
						TerrainEditor.splatmapMode = TerrainEditor.EDevkitLandscapeToolSplatmapMode.PAINT;
					}
					if (InputEx.GetKeyDown(KeyCode.W))
					{
						TerrainEditor.splatmapMode = TerrainEditor.EDevkitLandscapeToolSplatmapMode.AUTO;
					}
					if (InputEx.GetKeyDown(KeyCode.E))
					{
						TerrainEditor.splatmapMode = TerrainEditor.EDevkitLandscapeToolSplatmapMode.SMOOTH;
					}
					if (InputEx.GetKeyDown(KeyCode.R))
					{
						TerrainEditor.splatmapMode = TerrainEditor.EDevkitLandscapeToolSplatmapMode.CUT;
					}
					if (InputEx.GetKeyDown(KeyCode.LeftAlt))
					{
						this.isSamplingLayer = true;
					}
					if (InputEx.GetKeyUp(KeyCode.Mouse0) && this.isSamplingLayer)
					{
						AssetReference<LandscapeMaterialAsset> splatmapMaterialTarget;
						if (this.isPointerOnLandscape && Landscape.getSplatmapMaterial(raycastHit.point, out splatmapMaterialTarget))
						{
							TerrainEditor.splatmapMaterialTarget = splatmapMaterialTarget;
						}
						this.isSamplingLayer = false;
					}
					if (!this.isSamplingLayer && this.isPointerOnLandscape)
					{
						if (InputEx.GetKeyDown(KeyCode.Mouse0))
						{
							DevkitTransactionManager.beginTransaction("Splatmap");
							Landscape.clearSplatmapTransactions();
							Landscape.clearHoleTransactions();
						}
						Bounds bounds2 = new Bounds(this.brushWorldPosition, new Vector3(this.splatmapBrushRadius * 2f, 0f, this.splatmapBrushRadius * 2f));
						if (DevkitLandscapeToolSplatmapOptions.instance.previewMethod == EDevkitLandscapeToolSplatmapPreviewMethod.BRUSH_ALPHA)
						{
							Landscape.getSplatmapVertices(bounds2, new Landscape.LandscapeGetSplatmapVerticesHandler(this.handleSplatmapGetVerticesBrush));
						}
						else if (DevkitLandscapeToolSplatmapOptions.instance.previewMethod == EDevkitLandscapeToolSplatmapPreviewMethod.WEIGHT)
						{
							Landscape.readSplatmap(bounds2, new Landscape.LandscapeReadSplatmapHandler(this.handleSplatmapReadWeights));
						}
						if (InputEx.GetKey(KeyCode.Mouse0))
						{
							if (TerrainEditor.splatmapMode == TerrainEditor.EDevkitLandscapeToolSplatmapMode.PAINT)
							{
								Landscape.writeSplatmap(bounds2, new Landscape.LandscapeWriteSplatmapHandler(this.handleSplatmapWritePaint));
							}
							else if (TerrainEditor.splatmapMode == TerrainEditor.EDevkitLandscapeToolSplatmapMode.AUTO)
							{
								Landscape.writeSplatmap(bounds2, new Landscape.LandscapeWriteSplatmapHandler(this.handleSplatmapWriteAuto));
							}
							else if (TerrainEditor.splatmapMode == TerrainEditor.EDevkitLandscapeToolSplatmapMode.SMOOTH)
							{
								if (DevkitLandscapeToolSplatmapOptions.instance.smoothMethod == EDevkitLandscapeToolSplatmapSmoothMethod.BRUSH_AVERAGE)
								{
									this.splatmapSmoothSampleCount = 0;
									this.splatmapSmoothSampleAverage.Clear();
									Landscape.readSplatmap(bounds2, new Landscape.LandscapeReadSplatmapHandler(this.handleSplatmapReadBrushAverage));
								}
								else if (DevkitLandscapeToolSplatmapOptions.instance.smoothMethod == EDevkitLandscapeToolSplatmapSmoothMethod.PIXEL_AVERAGE)
								{
									Bounds worldBounds2 = bounds2;
									worldBounds2.Expand(Landscape.SPLATMAP_WORLD_UNIT * 2f);
									Landscape.readSplatmap(worldBounds2, new Landscape.LandscapeReadSplatmapHandler(this.HandleSplatmapReadPixelSmooth));
								}
								Landscape.writeSplatmap(bounds2, new Landscape.LandscapeWriteSplatmapHandler(this.handleSplatmapWriteSmooth));
								if (DevkitLandscapeToolSplatmapOptions.instance.smoothMethod == EDevkitLandscapeToolSplatmapSmoothMethod.PIXEL_AVERAGE)
								{
									this.ReleaseSplatmapPixelSmoothBuffer();
								}
							}
							else if (TerrainEditor.splatmapMode == TerrainEditor.EDevkitLandscapeToolSplatmapMode.CUT)
							{
								Landscape.writeHoles(bounds2, new Landscape.LandscapeWriteHolesHandler(this.handleSplatmapWriteCut));
							}
						}
					}
				}
			}
			if (InputEx.GetKeyUp(KeyCode.LeftAlt))
			{
				if (this.isSamplingFlattenTarget)
				{
					this.isSamplingFlattenTarget = false;
				}
				if (this.isSamplingLayer)
				{
					this.isSamplingLayer = false;
				}
			}
			if (InputEx.GetKeyUp(KeyCode.Mouse0))
			{
				if (this.isSamplingRampPositions)
				{
					if (this.isPointerOnLandscape && new Vector2(this.heightmapRampBeginPosition.x - this.heightmapRampEndPosition.x, this.heightmapRampBeginPosition.z - this.heightmapRampEndPosition.z).magnitude > 1f)
					{
						Vector3 vector3 = new Vector3(Mathf.Min(this.heightmapRampBeginPosition.x, this.heightmapRampEndPosition.x), Mathf.Min(this.heightmapRampBeginPosition.y, this.heightmapRampEndPosition.y), Mathf.Min(this.heightmapRampBeginPosition.z, this.heightmapRampEndPosition.z));
						Vector3 vector4 = new Vector3(Mathf.Max(this.heightmapRampBeginPosition.x, this.heightmapRampEndPosition.x), Mathf.Max(this.heightmapRampBeginPosition.y, this.heightmapRampEndPosition.y), Mathf.Max(this.heightmapRampBeginPosition.z, this.heightmapRampEndPosition.z));
						vector3.x -= this.heightmapBrushRadius;
						vector3.z -= this.heightmapBrushRadius;
						vector4.x += this.heightmapBrushRadius;
						vector4.z += this.heightmapBrushRadius;
						Landscape.writeHeightmap(new Bounds((vector3 + vector4) / 2f, vector4 - vector3), new Landscape.LandscapeWriteHeightmapHandler(this.handleHeightmapWriteRamp));
					}
					this.isSamplingRampPositions = false;
				}
				DevkitTransactionManager.endTransaction();
				if (TerrainEditor.toolMode == TerrainEditor.EDevkitLandscapeToolMode.HEIGHTMAP)
				{
					Landscape.applyLOD();
				}
			}
		}

		// Token: 0x06001F12 RID: 7954 RVA: 0x000748FE File Offset: 0x00072AFE
		public virtual void equip()
		{
			GLRenderer.render += this.handleGLRender;
			Landscape.DisableHoleColliders = true;
		}

		// Token: 0x06001F13 RID: 7955 RVA: 0x00074917 File Offset: 0x00072B17
		public virtual void dequip()
		{
			GLRenderer.render -= this.handleGLRender;
			Landscape.DisableHoleColliders = false;
		}

		/// <summary>
		/// Get brush strength multiplier where strength decreases past falloff. Use this method so that different falloffs e.g. linear, curved can be added.
		/// </summary>
		/// <param name="normalizedDistance">Percentage of <see cref="P:SDG.Unturned.TerrainEditor.brushRadius" />.</param>
		// Token: 0x06001F14 RID: 7956 RVA: 0x00074930 File Offset: 0x00072B30
		protected float getBrushAlpha(float normalizedDistance)
		{
			if (normalizedDistance <= this.brushFalloff || this.brushFalloff >= 1f)
			{
				return 1f;
			}
			return (1f - normalizedDistance) / (1f - this.brushFalloff);
		}

		// Token: 0x06001F15 RID: 7957 RVA: 0x00074964 File Offset: 0x00072B64
		protected void HandleHeightmapReadBrushAverage(LandscapeCoord tileCoord, HeightmapCoord heightmapCoord, Vector3 worldPosition, float currentHeight)
		{
			if (new Vector2(worldPosition.x - this.brushWorldPosition.x, worldPosition.z - this.brushWorldPosition.z).magnitude / this.heightmapBrushRadius > 1f)
			{
				return;
			}
			this.heightmapSmoothSampleCount++;
			this.heightmapSmoothSampleAverage += currentHeight;
		}

		// Token: 0x06001F16 RID: 7958 RVA: 0x000749D0 File Offset: 0x00072BD0
		protected void HandleHeightmapReadPixelSmooth(LandscapeCoord tileCoord, HeightmapCoord heightmapCoord, Vector3 worldPosition, float currentHeight)
		{
			float[,] array;
			if (!this.heightmapPixelSmoothBuffer.TryGetValue(tileCoord, ref array))
			{
				array = LandscapeHeightmapCopyPool.claim();
				this.heightmapPixelSmoothBuffer.Add(tileCoord, array);
			}
			array[heightmapCoord.x, heightmapCoord.y] = currentHeight;
		}

		// Token: 0x06001F17 RID: 7959 RVA: 0x00074A14 File Offset: 0x00072C14
		private void ReleaseHeightmapPixelSmoothBuffer()
		{
			foreach (KeyValuePair<LandscapeCoord, float[,]> keyValuePair in this.heightmapPixelSmoothBuffer)
			{
				LandscapeHeightmapCopyPool.release(keyValuePair.Value);
			}
			this.heightmapPixelSmoothBuffer.Clear();
		}

		// Token: 0x06001F18 RID: 7960 RVA: 0x00074A78 File Offset: 0x00072C78
		protected void HandleSplatmapReadPixelSmooth(LandscapeCoord tileCoord, SplatmapCoord splatmapCoord, Vector3 worldPosition, float[] currentWeights)
		{
			float[,,] array;
			if (!this.splatmapPixelSmoothBuffer.TryGetValue(tileCoord, ref array))
			{
				array = LandscapeSplatmapCopyPool.claim();
				this.splatmapPixelSmoothBuffer.Add(tileCoord, array);
			}
			for (int i = 0; i < Landscape.SPLATMAP_LAYERS; i++)
			{
				array[splatmapCoord.x, splatmapCoord.y, i] = currentWeights[i];
			}
		}

		// Token: 0x06001F19 RID: 7961 RVA: 0x00074AD0 File Offset: 0x00072CD0
		private void ReleaseSplatmapPixelSmoothBuffer()
		{
			foreach (KeyValuePair<LandscapeCoord, float[,,]> keyValuePair in this.splatmapPixelSmoothBuffer)
			{
				LandscapeSplatmapCopyPool.release(keyValuePair.Value);
			}
			this.splatmapPixelSmoothBuffer.Clear();
		}

		// Token: 0x06001F1A RID: 7962 RVA: 0x00074B34 File Offset: 0x00072D34
		protected void handleHeightmapGetVerticesBrush(LandscapeCoord tileCoord, HeightmapCoord heightmapCoord, Vector3 worldPosition)
		{
			float num = new Vector2(worldPosition.x - this.brushWorldPosition.x, worldPosition.z - this.brushWorldPosition.z).magnitude / this.heightmapBrushRadius;
			if (num > 1f)
			{
				return;
			}
			float brushAlpha = this.getBrushAlpha(num);
			this.previewSamples.Add(new LandscapePreviewSample(worldPosition, brushAlpha));
		}

		// Token: 0x06001F1B RID: 7963 RVA: 0x00074BA0 File Offset: 0x00072DA0
		protected void handleHeightmapGetVerticesRamp(LandscapeCoord tileCoord, HeightmapCoord heightmapCoord, Vector3 worldPosition)
		{
			Vector2 a = new Vector2(this.heightmapRampEndPosition.x - this.heightmapRampBeginPosition.x, this.heightmapRampEndPosition.z - this.heightmapRampBeginPosition.z);
			float magnitude = a.magnitude;
			Vector2 vector = a / magnitude;
			Vector2 rhs = vector.Cross();
			Vector2 a2 = new Vector2(worldPosition.x - this.heightmapRampBeginPosition.x, worldPosition.z - this.heightmapRampBeginPosition.z);
			float magnitude2 = a2.magnitude;
			Vector2 lhs = a2 / magnitude2;
			float num = Vector2.Dot(lhs, vector);
			if (num < 0f)
			{
				return;
			}
			if (magnitude2 * num / magnitude > 1f)
			{
				return;
			}
			float num2 = Vector2.Dot(lhs, rhs);
			float num3 = Mathf.Abs(magnitude2 * num2 / this.heightmapBrushRadius);
			if (num3 > 1f)
			{
				return;
			}
			float brushAlpha = this.getBrushAlpha(num3);
			this.previewSamples.Add(new LandscapePreviewSample(worldPosition, brushAlpha));
		}

		// Token: 0x06001F1C RID: 7964 RVA: 0x00074CA0 File Offset: 0x00072EA0
		protected void handleSplatmapGetVerticesBrush(LandscapeCoord tileCoord, SplatmapCoord splatmapCoord, Vector3 worldPosition)
		{
			float num = new Vector2(worldPosition.x - this.brushWorldPosition.x, worldPosition.z - this.brushWorldPosition.z).magnitude / this.splatmapBrushRadius;
			if (num > 1f)
			{
				return;
			}
			float brushAlpha = this.getBrushAlpha(num);
			this.previewSamples.Add(new LandscapePreviewSample(worldPosition, brushAlpha));
		}

		// Token: 0x06001F1D RID: 7965 RVA: 0x00074D0C File Offset: 0x00072F0C
		protected float handleHeightmapWriteAdjust(LandscapeCoord tileCoord, HeightmapCoord heightmapCoord, Vector3 worldPosition, float currentHeight)
		{
			float num = new Vector2(worldPosition.x - this.brushWorldPosition.x, worldPosition.z - this.brushWorldPosition.z).magnitude / this.heightmapBrushRadius;
			if (num > 1f)
			{
				return currentHeight;
			}
			float brushAlpha = this.getBrushAlpha(num);
			float num2 = Time.deltaTime * this.heightmapBrushStrength * brushAlpha;
			num2 *= this.heightmapAdjustSensitivity;
			if (InputEx.GetKey(KeyCode.LeftShift))
			{
				num2 = -num2;
			}
			currentHeight += num2;
			return currentHeight;
		}

		// Token: 0x06001F1E RID: 7966 RVA: 0x00074D94 File Offset: 0x00072F94
		protected float handleHeightmapWriteFlatten(LandscapeCoord tileCoord, HeightmapCoord heightmapCoord, Vector3 worldPosition, float currentHeight)
		{
			float num = new Vector2(worldPosition.x - this.brushWorldPosition.x, worldPosition.z - this.brushWorldPosition.z).magnitude / this.heightmapBrushRadius;
			if (num > 1f)
			{
				return currentHeight;
			}
			float brushAlpha = this.getBrushAlpha(num);
			float num2 = (this.heightmapFlattenTarget + Landscape.TILE_HEIGHT / 2f) / Landscape.TILE_HEIGHT;
			EDevkitLandscapeToolHeightmapFlattenMethod flattenMethod = DevkitLandscapeToolHeightmapOptions.instance.flattenMethod;
			if (flattenMethod != EDevkitLandscapeToolHeightmapFlattenMethod.MIN)
			{
				if (flattenMethod == EDevkitLandscapeToolHeightmapFlattenMethod.MAX)
				{
					num2 = Mathf.Max(num2, currentHeight);
				}
			}
			else
			{
				num2 = Mathf.Min(num2, currentHeight);
			}
			float num3 = num2 - currentHeight;
			float num4 = Time.deltaTime * this.heightmapBrushStrength * brushAlpha;
			num3 = Mathf.Clamp(num3, -num4, num4);
			num3 *= this.heightmapFlattenSensitivity;
			currentHeight += num3;
			return currentHeight;
		}

		// Token: 0x06001F1F RID: 7967 RVA: 0x00074E68 File Offset: 0x00073068
		private void SampleHeightPixelSmooth(Vector3 worldPosition, ref int sampleCount, ref float sampleAverage)
		{
			LandscapeCoord landscapeCoord = new LandscapeCoord(worldPosition);
			float[,] array;
			if (this.heightmapPixelSmoothBuffer.TryGetValue(landscapeCoord, ref array))
			{
				HeightmapCoord heightmapCoord = new HeightmapCoord(landscapeCoord, worldPosition);
				sampleCount++;
				sampleAverage += array[heightmapCoord.x, heightmapCoord.y];
			}
		}

		// Token: 0x06001F20 RID: 7968 RVA: 0x00074EB4 File Offset: 0x000730B4
		protected float handleHeightmapWriteSmooth(LandscapeCoord tileCoord, HeightmapCoord heightmapCoord, Vector3 worldPosition, float currentHeight)
		{
			float num = new Vector2(worldPosition.x - this.brushWorldPosition.x, worldPosition.z - this.brushWorldPosition.z).magnitude / this.heightmapBrushRadius;
			if (num > 1f)
			{
				return currentHeight;
			}
			float brushAlpha = this.getBrushAlpha(num);
			if (DevkitLandscapeToolHeightmapOptions.instance.smoothMethod == EDevkitLandscapeToolHeightmapSmoothMethod.PIXEL_AVERAGE)
			{
				this.heightmapSmoothSampleCount = 0;
				this.heightmapSmoothSampleAverage = 0f;
				this.SampleHeightPixelSmooth(worldPosition + new Vector3(Landscape.HEIGHTMAP_WORLD_UNIT, 0f, 0f), ref this.heightmapSmoothSampleCount, ref this.heightmapSmoothSampleAverage);
				this.SampleHeightPixelSmooth(worldPosition + new Vector3(-Landscape.HEIGHTMAP_WORLD_UNIT, 0f, 0f), ref this.heightmapSmoothSampleCount, ref this.heightmapSmoothSampleAverage);
				this.SampleHeightPixelSmooth(worldPosition + new Vector3(0f, 0f, Landscape.HEIGHTMAP_WORLD_UNIT), ref this.heightmapSmoothSampleCount, ref this.heightmapSmoothSampleAverage);
				this.SampleHeightPixelSmooth(worldPosition + new Vector3(0f, 0f, -Landscape.HEIGHTMAP_WORLD_UNIT), ref this.heightmapSmoothSampleCount, ref this.heightmapSmoothSampleAverage);
				if (this.heightmapSmoothSampleCount > 0)
				{
					this.heightmapSmoothTarget = this.heightmapSmoothSampleAverage / (float)this.heightmapSmoothSampleCount;
				}
				else
				{
					this.heightmapSmoothTarget = currentHeight;
				}
			}
			currentHeight = Mathf.Lerp(currentHeight, this.heightmapSmoothTarget, Time.deltaTime * this.heightmapBrushStrength * brushAlpha);
			return currentHeight;
		}

		// Token: 0x06001F21 RID: 7969 RVA: 0x00075028 File Offset: 0x00073228
		protected float handleHeightmapWriteRamp(LandscapeCoord tileCoord, HeightmapCoord heightmapCoord, Vector3 worldPosition, float currentHeight)
		{
			Vector2 a = new Vector2(this.heightmapRampEndPosition.x - this.heightmapRampBeginPosition.x, this.heightmapRampEndPosition.z - this.heightmapRampBeginPosition.z);
			float magnitude = a.magnitude;
			Vector2 vector = a / magnitude;
			Vector2 rhs = vector.Cross();
			Vector2 a2 = new Vector2(worldPosition.x - this.heightmapRampBeginPosition.x, worldPosition.z - this.heightmapRampBeginPosition.z);
			float magnitude2 = a2.magnitude;
			Vector2 lhs = a2 / magnitude2;
			float num = Vector2.Dot(lhs, vector);
			if (num < 0f)
			{
				return currentHeight;
			}
			float num2 = magnitude2 * num / magnitude;
			if (num2 > 1f)
			{
				return currentHeight;
			}
			float num3 = Vector2.Dot(lhs, rhs);
			float num4 = Mathf.Abs(magnitude2 * num3 / this.heightmapBrushRadius);
			if (num4 > 1f)
			{
				return currentHeight;
			}
			float brushAlpha = this.getBrushAlpha(num4);
			float a3 = (this.heightmapRampBeginPosition.y + Landscape.TILE_HEIGHT / 2f) / Landscape.TILE_HEIGHT;
			float b = (this.heightmapRampEndPosition.y + Landscape.TILE_HEIGHT / 2f) / Landscape.TILE_HEIGHT;
			currentHeight = Mathf.Lerp(currentHeight, Mathf.Lerp(a3, b, num2), brushAlpha);
			return Mathf.Clamp01(currentHeight);
		}

		// Token: 0x06001F22 RID: 7970 RVA: 0x0007517C File Offset: 0x0007337C
		protected void handleSplatmapReadBrushAverage(LandscapeCoord tileCoord, SplatmapCoord splatmapCoord, Vector3 worldPosition, float[] currentWeights)
		{
			LandscapeTile tile = Landscape.getTile(tileCoord);
			if (tile.materials == null)
			{
				return;
			}
			if (new Vector2(worldPosition.x - this.brushWorldPosition.x, worldPosition.z - this.brushWorldPosition.z).magnitude / this.splatmapBrushRadius > 1f)
			{
				return;
			}
			for (int i = 0; i < Landscape.SPLATMAP_LAYERS; i++)
			{
				AssetReference<LandscapeMaterialAsset> assetReference = tile.materials[i];
				if (assetReference.isValid)
				{
					if (!this.splatmapSmoothSampleAverage.ContainsKey(assetReference))
					{
						this.splatmapSmoothSampleAverage.Add(assetReference, 0f);
					}
					Dictionary<AssetReference<LandscapeMaterialAsset>, float> dictionary = this.splatmapSmoothSampleAverage;
					AssetReference<LandscapeMaterialAsset> assetReference2 = assetReference;
					dictionary[assetReference2] += currentWeights[i];
					this.splatmapSmoothSampleCount++;
				}
			}
		}

		// Token: 0x06001F23 RID: 7971 RVA: 0x00075250 File Offset: 0x00073450
		protected void handleSplatmapReadWeights(LandscapeCoord tileCoord, SplatmapCoord splatmapCoord, Vector3 worldPosition, float[] currentWeights)
		{
			LandscapeTile tile = Landscape.getTile(tileCoord);
			if (tile.materials == null)
			{
				return;
			}
			if (new Vector2(worldPosition.x - this.brushWorldPosition.x, worldPosition.z - this.brushWorldPosition.z).magnitude / this.splatmapBrushRadius > 1f)
			{
				return;
			}
			int splatmapTargetMaterialLayerIndex = this.getSplatmapTargetMaterialLayerIndex(tile, TerrainEditor.splatmapMaterialTarget);
			float newWeight;
			if (splatmapTargetMaterialLayerIndex == -1)
			{
				newWeight = 0f;
			}
			else
			{
				newWeight = currentWeights[splatmapTargetMaterialLayerIndex];
			}
			this.previewSamples.Add(new LandscapePreviewSample(worldPosition, newWeight));
		}

		// Token: 0x06001F24 RID: 7972 RVA: 0x000752E0 File Offset: 0x000734E0
		protected int getSplatmapTargetMaterialLayerIndex(LandscapeTile tile, AssetReference<LandscapeMaterialAsset> targetMaterial)
		{
			if (!targetMaterial.isValid)
			{
				return -1;
			}
			int num = -1;
			for (int i = 0; i < Landscape.SPLATMAP_LAYERS; i++)
			{
				if (tile.materials[i] == targetMaterial)
				{
					num = i;
					break;
				}
			}
			if (num == -1)
			{
				for (int j = 0; j < Landscape.SPLATMAP_LAYERS; j++)
				{
					if (!tile.materials[j].isValid)
					{
						tile.materials[j] = targetMaterial;
						tile.updatePrototypes();
						num = j;
						break;
					}
				}
			}
			return num;
		}

		// Token: 0x06001F25 RID: 7973 RVA: 0x00075364 File Offset: 0x00073564
		protected void blendSplatmapWeights(float[] currentWeights, int targetMaterialLayer, float targetWeight, float speed)
		{
			int splatmapHighestWeightLayerIndex = Landscape.getSplatmapHighestWeightLayerIndex(currentWeights, targetMaterialLayer);
			for (int i = 0; i < Landscape.SPLATMAP_LAYERS; i++)
			{
				float num;
				if (i == targetMaterialLayer)
				{
					num = targetWeight;
				}
				else if (i == splatmapHighestWeightLayerIndex)
				{
					num = 1f - targetWeight;
				}
				else
				{
					num = 0f;
				}
				float num2 = num - currentWeights[i];
				num2 *= speed;
				currentWeights[i] += num2;
			}
		}

		// Token: 0x06001F26 RID: 7974 RVA: 0x000753BC File Offset: 0x000735BC
		protected void handleSplatmapWritePaint(LandscapeCoord tileCoord, SplatmapCoord splatmapCoord, Vector3 worldPosition, float[] currentWeights)
		{
			LandscapeTile tile = Landscape.getTile(tileCoord);
			if (tile.materials == null)
			{
				return;
			}
			int splatmapTargetMaterialLayerIndex = this.getSplatmapTargetMaterialLayerIndex(tile, TerrainEditor.splatmapMaterialTarget);
			if (splatmapTargetMaterialLayerIndex == -1)
			{
				return;
			}
			float num = new Vector2(worldPosition.x - this.brushWorldPosition.x, worldPosition.z - this.brushWorldPosition.z).magnitude / this.splatmapBrushRadius;
			if (num > 1f)
			{
				return;
			}
			bool flag = InputEx.GetKey(KeyCode.LeftControl) || this.splatmapUseWeightTarget;
			float targetWeight = 0.5f;
			if (DevkitLandscapeToolSplatmapOptions.instance.useAutoFoundation || DevkitLandscapeToolSplatmapOptions.instance.useAutoSlope)
			{
				bool flag2 = false;
				if (DevkitLandscapeToolSplatmapOptions.instance.useAutoFoundation)
				{
					int num2 = Physics.SphereCastNonAlloc(worldPosition + new Vector3(0f, TerrainEditor.splatmapMaterialTargetAsset.autoRayLength, 0f), DevkitLandscapeToolSplatmapOptions.instance.autoRayRadius, Vector3.down, TerrainEditor.FOUNDATION_HITS, DevkitLandscapeToolSplatmapOptions.instance.autoRayLength, (int)DevkitLandscapeToolSplatmapOptions.instance.autoRayMask, QueryTriggerInteraction.Ignore);
					if (num2 > 0)
					{
						bool flag3 = false;
						for (int i = 0; i < num2; i++)
						{
							RaycastHit raycastHit = TerrainEditor.FOUNDATION_HITS[i];
							ObjectAsset asset = LevelObjects.getAsset(raycastHit.transform);
							if (asset == null)
							{
								flag3 = true;
								break;
							}
							if (!asset.isSnowshoe)
							{
								flag3 = true;
								break;
							}
						}
						if (flag3)
						{
							targetWeight = (flag ? this.splatmapWeightTarget : 1f);
							flag2 = true;
						}
					}
				}
				Vector3 to;
				if (!flag2 && DevkitLandscapeToolSplatmapOptions.instance.useAutoSlope && Landscape.getNormal(worldPosition, out to))
				{
					float num3 = Vector3.Angle(Vector3.up, to);
					if (num3 >= DevkitLandscapeToolSplatmapOptions.instance.autoMinAngleBegin && num3 <= DevkitLandscapeToolSplatmapOptions.instance.autoMaxAngleEnd)
					{
						if (num3 < DevkitLandscapeToolSplatmapOptions.instance.autoMinAngleEnd)
						{
							targetWeight = Mathf.InverseLerp(DevkitLandscapeToolSplatmapOptions.instance.autoMinAngleBegin, DevkitLandscapeToolSplatmapOptions.instance.autoMinAngleEnd, num3);
						}
						else if (num3 > DevkitLandscapeToolSplatmapOptions.instance.autoMaxAngleBegin)
						{
							targetWeight = 1f - Mathf.InverseLerp(DevkitLandscapeToolSplatmapOptions.instance.autoMaxAngleBegin, DevkitLandscapeToolSplatmapOptions.instance.autoMaxAngleEnd, num3);
						}
						else
						{
							targetWeight = 1f;
						}
						flag2 = true;
					}
				}
				if (!flag2)
				{
					return;
				}
			}
			else if (flag)
			{
				targetWeight = this.splatmapWeightTarget;
			}
			else if (InputEx.GetKey(KeyCode.LeftShift))
			{
				targetWeight = 0f;
			}
			else
			{
				targetWeight = 1f;
			}
			float brushAlpha = this.getBrushAlpha(num);
			float speed = Time.deltaTime * this.splatmapBrushStrength * brushAlpha * this.splatmapPaintSensitivity;
			this.blendSplatmapWeights(currentWeights, splatmapTargetMaterialLayerIndex, targetWeight, speed);
		}

		// Token: 0x06001F27 RID: 7975 RVA: 0x00075640 File Offset: 0x00073840
		protected void handleSplatmapWriteAuto(LandscapeCoord tileCoord, SplatmapCoord splatmapCoord, Vector3 worldPosition, float[] currentWeights)
		{
			if (TerrainEditor.splatmapMaterialTargetAsset == null)
			{
				return;
			}
			LandscapeTile tile = Landscape.getTile(tileCoord);
			if (tile.materials == null)
			{
				return;
			}
			int splatmapTargetMaterialLayerIndex = this.getSplatmapTargetMaterialLayerIndex(tile, TerrainEditor.splatmapMaterialTarget);
			if (splatmapTargetMaterialLayerIndex == -1)
			{
				return;
			}
			float num = new Vector2(worldPosition.x - this.brushWorldPosition.x, worldPosition.z - this.brushWorldPosition.z).magnitude / this.splatmapBrushRadius;
			if (num > 1f)
			{
				return;
			}
			float targetWeight = 1f;
			bool flag = false;
			if (TerrainEditor.splatmapMaterialTargetAsset.useAutoFoundation)
			{
				int num2 = Physics.SphereCastNonAlloc(worldPosition + new Vector3(0f, TerrainEditor.splatmapMaterialTargetAsset.autoRayLength, 0f), TerrainEditor.splatmapMaterialTargetAsset.autoRayRadius, Vector3.down, TerrainEditor.FOUNDATION_HITS, TerrainEditor.splatmapMaterialTargetAsset.autoRayLength, (int)TerrainEditor.splatmapMaterialTargetAsset.autoRayMask, QueryTriggerInteraction.Ignore);
				if (num2 > 0)
				{
					bool flag2 = false;
					for (int i = 0; i < num2; i++)
					{
						RaycastHit raycastHit = TerrainEditor.FOUNDATION_HITS[i];
						ObjectAsset asset = LevelObjects.getAsset(raycastHit.transform);
						if (asset == null)
						{
							flag2 = true;
							break;
						}
						if (!asset.isSnowshoe)
						{
							flag2 = true;
							break;
						}
					}
					if (flag2)
					{
						targetWeight = 1f;
						flag = true;
					}
				}
			}
			Vector3 to;
			if (!flag && TerrainEditor.splatmapMaterialTargetAsset.useAutoSlope && Landscape.getNormal(worldPosition, out to))
			{
				float num3 = Vector3.Angle(Vector3.up, to);
				if (num3 >= TerrainEditor.splatmapMaterialTargetAsset.autoMinAngleBegin && num3 <= TerrainEditor.splatmapMaterialTargetAsset.autoMaxAngleEnd)
				{
					if (num3 < TerrainEditor.splatmapMaterialTargetAsset.autoMinAngleEnd)
					{
						targetWeight = Mathf.InverseLerp(TerrainEditor.splatmapMaterialTargetAsset.autoMinAngleBegin, TerrainEditor.splatmapMaterialTargetAsset.autoMinAngleEnd, num3);
					}
					else if (num3 > TerrainEditor.splatmapMaterialTargetAsset.autoMaxAngleBegin)
					{
						targetWeight = 1f - Mathf.InverseLerp(TerrainEditor.splatmapMaterialTargetAsset.autoMaxAngleBegin, TerrainEditor.splatmapMaterialTargetAsset.autoMaxAngleEnd, num3);
					}
					flag = true;
				}
			}
			if (!flag)
			{
				return;
			}
			float brushAlpha = this.getBrushAlpha(num);
			float speed = Time.deltaTime * this.splatmapBrushStrength * brushAlpha * this.splatmapPaintSensitivity;
			this.blendSplatmapWeights(currentWeights, splatmapTargetMaterialLayerIndex, targetWeight, speed);
		}

		// Token: 0x06001F28 RID: 7976 RVA: 0x00075858 File Offset: 0x00073A58
		private void SampleSplatmapPixelSmooth(LandscapeCoord tileCoord, SplatmapCoord splatmapCoord)
		{
			float[,,] array;
			if (this.splatmapPixelSmoothBuffer.TryGetValue(tileCoord, ref array))
			{
				LandscapeTile tile = Landscape.getTile(tileCoord);
				if (tile != null && tile.materials != null)
				{
					for (int i = 0; i < Landscape.SPLATMAP_LAYERS; i++)
					{
						AssetReference<LandscapeMaterialAsset> assetReference = tile.materials[i];
						if (assetReference.isValid)
						{
							if (!this.splatmapSmoothSampleAverage.ContainsKey(assetReference))
							{
								this.splatmapSmoothSampleAverage.Add(assetReference, 0f);
							}
							Dictionary<AssetReference<LandscapeMaterialAsset>, float> dictionary = this.splatmapSmoothSampleAverage;
							AssetReference<LandscapeMaterialAsset> assetReference2 = assetReference;
							dictionary[assetReference2] += array[splatmapCoord.x, splatmapCoord.y, i];
							this.splatmapSmoothSampleCount++;
						}
					}
				}
			}
		}

		// Token: 0x06001F29 RID: 7977 RVA: 0x00075914 File Offset: 0x00073B14
		protected void handleSplatmapWriteSmooth(LandscapeCoord tileCoord, SplatmapCoord splatmapCoord, Vector3 worldPosition, float[] currentWeights)
		{
			float num = new Vector2(worldPosition.x - this.brushWorldPosition.x, worldPosition.z - this.brushWorldPosition.z).magnitude / this.splatmapBrushRadius;
			if (num > 1f)
			{
				return;
			}
			if (DevkitLandscapeToolSplatmapOptions.instance.smoothMethod == EDevkitLandscapeToolSplatmapSmoothMethod.PIXEL_AVERAGE)
			{
				this.splatmapSmoothSampleCount = 0;
				this.splatmapSmoothSampleAverage.Clear();
				LandscapeCoord tileCoord2 = tileCoord;
				SplatmapCoord splatmapCoord2 = new SplatmapCoord(splatmapCoord.x, splatmapCoord.y - 1);
				LandscapeUtility.cleanSplatmapCoord(ref tileCoord2, ref splatmapCoord2);
				this.SampleSplatmapPixelSmooth(tileCoord2, splatmapCoord2);
				tileCoord2 = tileCoord;
				splatmapCoord2 = new SplatmapCoord(splatmapCoord.x + 1, splatmapCoord.y);
				LandscapeUtility.cleanSplatmapCoord(ref tileCoord2, ref splatmapCoord2);
				this.SampleSplatmapPixelSmooth(tileCoord2, splatmapCoord2);
				tileCoord2 = tileCoord;
				splatmapCoord2 = new SplatmapCoord(splatmapCoord.x, splatmapCoord.y + 1);
				LandscapeUtility.cleanSplatmapCoord(ref tileCoord2, ref splatmapCoord2);
				this.SampleSplatmapPixelSmooth(tileCoord2, splatmapCoord2);
				tileCoord2 = tileCoord;
				splatmapCoord2 = new SplatmapCoord(splatmapCoord.x - 1, splatmapCoord.y);
				LandscapeUtility.cleanSplatmapCoord(ref tileCoord2, ref splatmapCoord2);
				this.SampleSplatmapPixelSmooth(tileCoord2, splatmapCoord2);
			}
			if (this.splatmapSmoothSampleCount <= 0)
			{
				return;
			}
			LandscapeTile tile = Landscape.getTile(tileCoord);
			if (tile.materials == null)
			{
				return;
			}
			float brushAlpha = this.getBrushAlpha(num);
			float num2 = Time.deltaTime * this.splatmapBrushStrength * brushAlpha;
			float num3 = 0f;
			for (int i = 0; i < Landscape.SPLATMAP_LAYERS; i++)
			{
				if (this.splatmapSmoothSampleAverage.ContainsKey(tile.materials[i]))
				{
					num3 += this.splatmapSmoothSampleAverage[tile.materials[i]] / (float)this.splatmapSmoothSampleCount;
				}
			}
			num3 = 1f / num3;
			for (int j = 0; j < Landscape.SPLATMAP_LAYERS; j++)
			{
				float num4;
				if (this.splatmapSmoothSampleAverage.ContainsKey(tile.materials[j]))
				{
					num4 = this.splatmapSmoothSampleAverage[tile.materials[j]] / (float)this.splatmapSmoothSampleCount * num3;
				}
				else
				{
					num4 = 0f;
				}
				float num5 = num4 - currentWeights[j];
				num5 *= num2;
				currentWeights[j] += num5;
			}
		}

		// Token: 0x06001F2A RID: 7978 RVA: 0x00075B44 File Offset: 0x00073D44
		protected bool handleSplatmapWriteCut(Vector3 worldPosition, bool currentlyVisible)
		{
			if (new Vector2(worldPosition.x - this.brushWorldPosition.x, worldPosition.z - this.brushWorldPosition.z).magnitude / this.splatmapBrushRadius > 1f)
			{
				return currentlyVisible;
			}
			return InputEx.GetKey(KeyCode.LeftShift);
		}

		// Token: 0x06001F2B RID: 7979 RVA: 0x00075B9C File Offset: 0x00073D9C
		protected void handleGLCircleOffset(ref Vector3 position)
		{
			Landscape.getWorldHeight(position, out position.y);
		}

		// Token: 0x06001F2C RID: 7980 RVA: 0x00075BB0 File Offset: 0x00073DB0
		protected void handleGLRender()
		{
			GLUtility.matrix = MathUtility.IDENTITY_MATRIX;
			if (TerrainEditor.toolMode == TerrainEditor.EDevkitLandscapeToolMode.TILE)
			{
				GLUtility.LINE_FLAT_COLOR.SetPass(0);
				GL.Begin(1);
				if (TerrainEditor.selectedTile != null && TerrainEditor.selectedTile.coord != this.pointerTileCoord)
				{
					GL.Color(Color.yellow);
					GLUtility.line(new Vector3((float)TerrainEditor.selectedTile.coord.x * Landscape.TILE_SIZE, 0f, (float)TerrainEditor.selectedTile.coord.y * Landscape.TILE_SIZE), new Vector3((float)(TerrainEditor.selectedTile.coord.x + 1) * Landscape.TILE_SIZE, 0f, (float)TerrainEditor.selectedTile.coord.y * Landscape.TILE_SIZE));
					GLUtility.line(new Vector3((float)TerrainEditor.selectedTile.coord.x * Landscape.TILE_SIZE, 0f, (float)TerrainEditor.selectedTile.coord.y * Landscape.TILE_SIZE), new Vector3((float)TerrainEditor.selectedTile.coord.x * Landscape.TILE_SIZE, 0f, (float)(TerrainEditor.selectedTile.coord.y + 1) * Landscape.TILE_SIZE));
					GLUtility.line(new Vector3((float)(TerrainEditor.selectedTile.coord.x + 1) * Landscape.TILE_SIZE, 0f, (float)(TerrainEditor.selectedTile.coord.y + 1) * Landscape.TILE_SIZE), new Vector3((float)(TerrainEditor.selectedTile.coord.x + 1) * Landscape.TILE_SIZE, 0f, (float)TerrainEditor.selectedTile.coord.y * Landscape.TILE_SIZE));
					GLUtility.line(new Vector3((float)(TerrainEditor.selectedTile.coord.x + 1) * Landscape.TILE_SIZE, 0f, (float)(TerrainEditor.selectedTile.coord.y + 1) * Landscape.TILE_SIZE), new Vector3((float)TerrainEditor.selectedTile.coord.x * Landscape.TILE_SIZE, 0f, (float)(TerrainEditor.selectedTile.coord.y + 1) * Landscape.TILE_SIZE));
				}
				if (this.isTileVisible && Glazier.Get().ShouldGameProcessInput)
				{
					GL.Color((Landscape.getTile(this.pointerTileCoord) == null) ? Color.green : ((TerrainEditor.selectedTile != null && TerrainEditor.selectedTile.coord == this.pointerTileCoord) ? Color.red : Color.white));
					GLUtility.line(new Vector3((float)this.pointerTileCoord.x * Landscape.TILE_SIZE, 0f, (float)this.pointerTileCoord.y * Landscape.TILE_SIZE), new Vector3((float)(this.pointerTileCoord.x + 1) * Landscape.TILE_SIZE, 0f, (float)this.pointerTileCoord.y * Landscape.TILE_SIZE));
					GLUtility.line(new Vector3((float)this.pointerTileCoord.x * Landscape.TILE_SIZE, 0f, (float)this.pointerTileCoord.y * Landscape.TILE_SIZE), new Vector3((float)this.pointerTileCoord.x * Landscape.TILE_SIZE, 0f, (float)(this.pointerTileCoord.y + 1) * Landscape.TILE_SIZE));
					GLUtility.line(new Vector3((float)(this.pointerTileCoord.x + 1) * Landscape.TILE_SIZE, 0f, (float)(this.pointerTileCoord.y + 1) * Landscape.TILE_SIZE), new Vector3((float)(this.pointerTileCoord.x + 1) * Landscape.TILE_SIZE, 0f, (float)this.pointerTileCoord.y * Landscape.TILE_SIZE));
					GLUtility.line(new Vector3((float)(this.pointerTileCoord.x + 1) * Landscape.TILE_SIZE, 0f, (float)(this.pointerTileCoord.y + 1) * Landscape.TILE_SIZE), new Vector3((float)this.pointerTileCoord.x * Landscape.TILE_SIZE, 0f, (float)(this.pointerTileCoord.y + 1) * Landscape.TILE_SIZE));
				}
				GL.End();
				return;
			}
			if (this.isBrushVisible && Glazier.Get().ShouldGameProcessInput)
			{
				if ((long)this.previewSamples.Count <= (long)((ulong)this.maxPreviewSamples))
				{
					GLUtility.LINE_FLAT_COLOR.SetPass(0);
					GL.Begin(4);
					float num = Mathf.Lerp(0.1f, 1f, this.brushRadius / 256f);
					Vector3 size = new Vector3(num, num, num);
					foreach (LandscapePreviewSample landscapePreviewSample in this.previewSamples)
					{
						GL.Color(Color.Lerp(Color.red, Color.green, landscapePreviewSample.weight));
						GLUtility.boxSolid(landscapePreviewSample.position, size);
					}
					GL.End();
				}
				GLUtility.LINE_FLAT_COLOR.SetPass(0);
				GL.Begin(1);
				if (TerrainEditor.toolMode == TerrainEditor.EDevkitLandscapeToolMode.HEIGHTMAP && TerrainEditor.heightmapMode == TerrainEditor.EDevkitLandscapeToolHeightmapMode.RAMP)
				{
					if (this.isSamplingRampPositions)
					{
						Vector3 normalized = (this.heightmapRampEndPosition - this.heightmapRampBeginPosition).normalized;
						Vector3 a = Vector3.Cross(Vector3.up, normalized);
						GL.Color(new Color(0.5f, 0.5f, 0f, 0.5f));
						GLUtility.line(this.heightmapRampBeginPosition - a * this.brushRadius, this.heightmapRampEndPosition - a * this.brushRadius);
						GLUtility.line(this.heightmapRampBeginPosition + a * this.brushRadius, this.heightmapRampEndPosition + a * this.brushRadius);
						GL.Color(Color.yellow);
						GLUtility.line(this.heightmapRampBeginPosition - a * this.brushRadius * this.heightmapBrushFalloff, this.heightmapRampEndPosition - a * this.brushRadius * this.heightmapBrushFalloff);
						GLUtility.line(this.heightmapRampBeginPosition + a * this.brushRadius * this.heightmapBrushFalloff, this.heightmapRampEndPosition + a * this.brushRadius * this.heightmapBrushFalloff);
					}
					else if (this.isChangingBrushRadius || this.isChangingBrushFalloff)
					{
						Vector3 normalized2 = (this.pointerWorldPosition - this.brushWorldPosition).normalized;
						Vector3 b = Vector3.Cross(Vector3.up, normalized2);
						GL.Color(new Color(0.5f, 0.5f, 0f, 0.5f));
						GLUtility.line(this.brushWorldPosition - normalized2 * this.brushRadius - b, this.brushWorldPosition - normalized2 * this.brushRadius + b);
						GLUtility.line(this.brushWorldPosition + normalized2 * this.brushRadius - b, this.brushWorldPosition + normalized2 * this.brushRadius + b);
						GL.Color(Color.yellow);
						GLUtility.line(this.brushWorldPosition - normalized2 * this.brushRadius * this.heightmapBrushFalloff - b, this.brushWorldPosition - normalized2 * this.brushRadius * this.heightmapBrushFalloff + b);
						GLUtility.line(this.brushWorldPosition + normalized2 * this.brushRadius * this.heightmapBrushFalloff - b, this.brushWorldPosition + normalized2 * this.brushRadius * this.heightmapBrushFalloff + b);
					}
				}
				else
				{
					Color color;
					if (this.isChangingBrushStrength)
					{
						color = Color.Lerp(Color.red, Color.green, this.brushStrength);
					}
					else if (this.isChangingWeightTarget)
					{
						color = Color.Lerp(Color.red, Color.green, this.splatmapWeightTarget);
					}
					else
					{
						color = Color.yellow;
					}
					bool flag = TerrainEditor.toolMode != TerrainEditor.EDevkitLandscapeToolMode.SPLATMAP || TerrainEditor.splatmapMode != TerrainEditor.EDevkitLandscapeToolSplatmapMode.CUT;
					GL.Color(flag ? (color / 2f) : color);
					GLUtility.circle(this.brushWorldPosition, this.brushRadius, new Vector3(1f, 0f, 0f), new Vector3(0f, 0f, 1f), new GLCircleOffsetHandler(this.handleGLCircleOffset));
					if (TerrainEditor.toolMode == TerrainEditor.EDevkitLandscapeToolMode.HEIGHTMAP && TerrainEditor.heightmapMode == TerrainEditor.EDevkitLandscapeToolHeightmapMode.FLATTEN)
					{
						GLUtility.circle(this.flattenPlanePosition, this.brushRadius, new Vector3(1f, 0f, 0f), new Vector3(0f, 0f, 1f), 0f);
					}
					if (flag)
					{
						GL.Color(color);
						GLUtility.circle(this.brushWorldPosition, this.brushRadius * this.brushFalloff, new Vector3(1f, 0f, 0f), new Vector3(0f, 0f, 1f), new GLCircleOffsetHandler(this.handleGLCircleOffset));
						if (TerrainEditor.toolMode == TerrainEditor.EDevkitLandscapeToolMode.HEIGHTMAP && TerrainEditor.heightmapMode == TerrainEditor.EDevkitLandscapeToolHeightmapMode.FLATTEN)
						{
							GLUtility.circle(this.flattenPlanePosition, this.brushRadius * this.brushFalloff, new Vector3(1f, 0f, 0f), new Vector3(0f, 0f, 1f), 0f);
						}
					}
				}
				GL.End();
			}
		}

		// Token: 0x04000F49 RID: 3913
		private static readonly RaycastHit[] FOUNDATION_HITS = new RaycastHit[4];

		// Token: 0x04000F4A RID: 3914
		protected static TerrainEditor.EDevkitLandscapeToolMode _toolMode;

		// Token: 0x04000F4C RID: 3916
		protected static LandscapeTile _selectedTile;

		// Token: 0x04000F4E RID: 3918
		public static TerrainEditor.EDevkitLandscapeToolHeightmapMode heightmapMode;

		// Token: 0x04000F4F RID: 3919
		public static TerrainEditor.EDevkitLandscapeToolSplatmapMode splatmapMode;

		// Token: 0x04000F50 RID: 3920
		protected static LandscapeMaterialAsset splatmapMaterialTargetAsset;

		// Token: 0x04000F51 RID: 3921
		protected static AssetReference<LandscapeMaterialAsset> _splatmapMaterialTarget;

		// Token: 0x04000F52 RID: 3922
		protected int heightmapSmoothSampleCount;

		// Token: 0x04000F53 RID: 3923
		protected float heightmapSmoothSampleAverage;

		// Token: 0x04000F54 RID: 3924
		protected float heightmapSmoothTarget;

		// Token: 0x04000F55 RID: 3925
		protected int splatmapSmoothSampleCount;

		// Token: 0x04000F56 RID: 3926
		protected Dictionary<AssetReference<LandscapeMaterialAsset>, float> splatmapSmoothSampleAverage = new Dictionary<AssetReference<LandscapeMaterialAsset>, float>();

		// Token: 0x04000F57 RID: 3927
		protected Vector3 heightmapRampBeginPosition;

		// Token: 0x04000F58 RID: 3928
		protected Vector3 heightmapRampEndPosition;

		// Token: 0x04000F59 RID: 3929
		protected Vector3 tilePlanePosition;

		// Token: 0x04000F5A RID: 3930
		protected Vector3 pointerWorldPosition;

		// Token: 0x04000F5B RID: 3931
		protected Vector3 brushWorldPosition;

		// Token: 0x04000F5C RID: 3932
		protected Vector3 changePlanePosition;

		// Token: 0x04000F5D RID: 3933
		protected Vector3 flattenPlanePosition;

		/// <summary>
		/// Whether the pointer is currently in a spot that can be painted.
		/// </summary>
		// Token: 0x04000F5E RID: 3934
		protected bool isPointerOnLandscape;

		// Token: 0x04000F5F RID: 3935
		protected bool isPointerOnTilePlane;

		// Token: 0x04000F60 RID: 3936
		protected bool isBrushVisible;

		// Token: 0x04000F61 RID: 3937
		protected bool isTileVisible;

		// Token: 0x04000F62 RID: 3938
		protected LandscapeCoord pointerTileCoord;

		// Token: 0x04000F63 RID: 3939
		protected List<LandscapePreviewSample> previewSamples = new List<LandscapePreviewSample>();

		// Token: 0x04000F64 RID: 3940
		protected bool isChangingBrushRadius;

		// Token: 0x04000F65 RID: 3941
		protected bool isChangingBrushFalloff;

		// Token: 0x04000F66 RID: 3942
		protected bool isChangingBrushStrength;

		// Token: 0x04000F67 RID: 3943
		protected bool isChangingWeightTarget;

		// Token: 0x04000F68 RID: 3944
		protected bool isSamplingFlattenTarget;

		// Token: 0x04000F69 RID: 3945
		protected bool isSamplingRampPositions;

		// Token: 0x04000F6A RID: 3946
		protected bool isSamplingLayer;

		// Token: 0x04000F6B RID: 3947
		private Dictionary<LandscapeCoord, float[,]> heightmapPixelSmoothBuffer = new Dictionary<LandscapeCoord, float[,]>();

		// Token: 0x04000F6C RID: 3948
		private Dictionary<LandscapeCoord, float[,,]> splatmapPixelSmoothBuffer = new Dictionary<LandscapeCoord, float[,,]>();

		// Token: 0x02000938 RID: 2360
		public enum EDevkitLandscapeToolMode
		{
			// Token: 0x040032BD RID: 12989
			HEIGHTMAP,
			// Token: 0x040032BE RID: 12990
			SPLATMAP,
			// Token: 0x040032BF RID: 12991
			TILE
		}

		// Token: 0x02000939 RID: 2361
		public enum EDevkitLandscapeToolHeightmapMode
		{
			// Token: 0x040032C1 RID: 12993
			ADJUST,
			// Token: 0x040032C2 RID: 12994
			FLATTEN,
			// Token: 0x040032C3 RID: 12995
			SMOOTH,
			// Token: 0x040032C4 RID: 12996
			RAMP
		}

		// Token: 0x0200093A RID: 2362
		public enum EDevkitLandscapeToolSplatmapMode
		{
			// Token: 0x040032C6 RID: 12998
			PAINT,
			// Token: 0x040032C7 RID: 12999
			AUTO,
			// Token: 0x040032C8 RID: 13000
			SMOOTH,
			// Token: 0x040032C9 RID: 13001
			CUT
		}

		// Token: 0x0200093B RID: 2363
		// (Invoke) Token: 0x06004AA8 RID: 19112
		public delegate void DevkitLandscapeToolModeChangedHandler(TerrainEditor.EDevkitLandscapeToolMode oldMode, TerrainEditor.EDevkitLandscapeToolMode newMode);

		// Token: 0x0200093C RID: 2364
		// (Invoke) Token: 0x06004AAC RID: 19116
		public delegate void DevkitLandscapeToolSelectedTileChangedHandler(LandscapeTile oldSelectedTile, LandscapeTile newSelectedTile);
	}
}
