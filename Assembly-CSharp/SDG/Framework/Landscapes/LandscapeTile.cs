using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using SDG.Framework.Debug;
using SDG.Framework.Foliage;
using SDG.Framework.IO.FormattedFiles;
using SDG.Framework.Utilities;
using SDG.Unturned;
using UnityEngine;
using UnityEngine.Rendering;

namespace SDG.Framework.Landscapes
{
	// Token: 0x020000AA RID: 170
	public class LandscapeTile : IFormattedFileReadable, IFormattedFileWritable, IFoliageSurface
	{
		// Token: 0x1700009B RID: 155
		// (get) Token: 0x0600045E RID: 1118 RVA: 0x00011A67 File Offset: 0x0000FC67
		// (set) Token: 0x0600045F RID: 1119 RVA: 0x00011A6F File Offset: 0x0000FC6F
		public GameObject gameObject { get; protected set; }

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x06000460 RID: 1120 RVA: 0x00011A78 File Offset: 0x0000FC78
		// (set) Token: 0x06000461 RID: 1121 RVA: 0x00011A80 File Offset: 0x0000FC80
		public LandscapeCoord coord
		{
			get
			{
				return this._coord;
			}
			protected set
			{
				this._coord = value;
				this.updateTransform();
			}
		}

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x06000462 RID: 1122 RVA: 0x00011A8F File Offset: 0x0000FC8F
		public Bounds localBounds
		{
			get
			{
				return new Bounds(new Vector3(Landscape.TILE_SIZE / 2f, 0f, Landscape.TILE_SIZE / 2f), new Vector3(Landscape.TILE_SIZE, Landscape.TILE_HEIGHT, Landscape.TILE_SIZE));
			}
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x06000463 RID: 1123 RVA: 0x00011ACC File Offset: 0x0000FCCC
		public Bounds worldBounds
		{
			get
			{
				Bounds localBounds = this.localBounds;
				localBounds.center += new Vector3((float)this.coord.x * Landscape.TILE_SIZE, 0f, (float)this.coord.y * Landscape.TILE_SIZE);
				return localBounds;
			}
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x06000464 RID: 1124 RVA: 0x00011B21 File Offset: 0x0000FD21
		// (set) Token: 0x06000465 RID: 1125 RVA: 0x00011B29 File Offset: 0x0000FD29
		public InspectableList<AssetReference<LandscapeMaterialAsset>> materials { get; protected set; }

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x06000466 RID: 1126 RVA: 0x00011B32 File Offset: 0x0000FD32
		// (set) Token: 0x06000467 RID: 1127 RVA: 0x00011B3A File Offset: 0x0000FD3A
		public TerrainData data { get; protected set; }

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x06000468 RID: 1128 RVA: 0x00011B43 File Offset: 0x0000FD43
		// (set) Token: 0x06000469 RID: 1129 RVA: 0x00011B4B File Offset: 0x0000FD4B
		public Terrain terrain { get; protected set; }

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x0600046A RID: 1130 RVA: 0x00011B54 File Offset: 0x0000FD54
		// (set) Token: 0x0600046B RID: 1131 RVA: 0x00011B5C File Offset: 0x0000FD5C
		public TerrainCollider collider { get; protected set; }

		// Token: 0x0600046C RID: 1132 RVA: 0x00011B65 File Offset: 0x0000FD65
		public void SetHeightsDelayLOD()
		{
			this.data.SetHeightsDelayLOD(0, 0, this.heightmap);
			if (this.dataWithoutHoles != null)
			{
				this.dataWithoutHoles.SetHeightsDelayLOD(0, 0, this.heightmap);
			}
		}

		// Token: 0x0600046D RID: 1133 RVA: 0x00011B9B File Offset: 0x0000FD9B
		public void SyncHeightmap()
		{
			this.data.SyncHeightmap();
			if (this.dataWithoutHoles != null)
			{
				this.dataWithoutHoles.SyncHeightmap();
			}
		}

		// Token: 0x0600046E RID: 1134 RVA: 0x00011BC4 File Offset: 0x0000FDC4
		public virtual void read(IFormattedFileReader reader)
		{
			reader = reader.readObject();
			this.coord = reader.readValue<LandscapeCoord>("Coord");
			int num = reader.readArrayLength("Materials");
			for (int i = 0; i < num; i++)
			{
				AssetReference<LandscapeMaterialAsset> assetReference = reader.readValue<AssetReference<LandscapeMaterialAsset>>(i);
				if (assetReference.isValid)
				{
					if (Level.shouldUseHolidayRedirects)
					{
						LandscapeMaterialAsset landscapeMaterialAsset = assetReference.Find();
						if (landscapeMaterialAsset != null)
						{
							AssetReference<LandscapeMaterialAsset> holidayRedirect = landscapeMaterialAsset.getHolidayRedirect();
							if (holidayRedirect.isValid)
							{
								assetReference = holidayRedirect;
							}
						}
					}
					if (assetReference.Find() == null)
					{
						ClientAssetIntegrity.ServerAddKnownMissingAsset(assetReference.GUID, string.Format("Landscape tile (x: {0} y: {1} layer: {2})", this.coord.x, this.coord.y, i));
					}
				}
				this.materials[i] = assetReference;
			}
			this.updatePrototypes();
			this.readHeightmaps();
			this.readSplatmaps();
			this.ReadHoles();
		}

		// Token: 0x0600046F RID: 1135 RVA: 0x00011CA9 File Offset: 0x0000FEA9
		public virtual void readHeightmaps()
		{
			this.readHeightmap("_Source", this.heightmap);
			this.SetHeightsDelayLOD();
		}

		// Token: 0x06000470 RID: 1136 RVA: 0x00011CC4 File Offset: 0x0000FEC4
		protected virtual void readHeightmap(string suffix, float[,] heightmap)
		{
			string[] array = new string[6];
			array[0] = "Tile_";
			int num = 1;
			LandscapeCoord coord = this.coord;
			array[num] = coord.x.ToString(CultureInfo.InvariantCulture);
			array[2] = "_";
			int num2 = 3;
			coord = this.coord;
			array[num2] = coord.y.ToString(CultureInfo.InvariantCulture);
			array[4] = suffix;
			array[5] = ".heightmap";
			string text = string.Concat(array);
			string text2 = Level.info.path + "/Landscape/Heightmaps/" + text;
			if (!File.Exists(text2))
			{
				UnturnedLog.warn("LandscapeTile missing heightmap file: {0}", new object[]
				{
					text2
				});
				return;
			}
			using (FileStream fileStream = new FileStream(text2, 3, 1, 3))
			{
				using (SHA1Stream sha1Stream = new SHA1Stream(fileStream))
				{
					for (int i = 0; i < Landscape.HEIGHTMAP_RESOLUTION; i++)
					{
						for (int j = 0; j < Landscape.HEIGHTMAP_RESOLUTION; j++)
						{
							float num3 = (float)((ushort)(sha1Stream.ReadByte() << 8 | sha1Stream.ReadByte())) / 65535f;
							heightmap[i, j] = num3;
						}
					}
					Level.includeHash(text, sha1Stream.Hash);
				}
			}
		}

		// Token: 0x06000471 RID: 1137 RVA: 0x00011E04 File Offset: 0x00010004
		public virtual void readSplatmaps()
		{
			this.readSplatmap("_Source", this.splatmap);
		}

		// Token: 0x06000472 RID: 1138 RVA: 0x00011E18 File Offset: 0x00010018
		protected virtual void readSplatmap(string suffix, float[,,] splatmap)
		{
			string[] array = new string[6];
			array[0] = "Tile_";
			int num = 1;
			LandscapeCoord coord = this.coord;
			array[num] = coord.x.ToString(CultureInfo.InvariantCulture);
			array[2] = "_";
			int num2 = 3;
			coord = this.coord;
			array[num2] = coord.y.ToString(CultureInfo.InvariantCulture);
			array[4] = suffix;
			array[5] = ".splatmap";
			string text = string.Concat(array);
			string text2 = Level.info.path + "/Landscape/Splatmaps/" + text;
			if (!File.Exists(text2))
			{
				UnturnedLog.warn("LandscapeTile missing splatmap file: {0}", new object[]
				{
					text2
				});
				return;
			}
			using (FileStream fileStream = new FileStream(text2, 3, 1, 3))
			{
				using (SHA1Stream sha1Stream = new SHA1Stream(fileStream))
				{
					for (int i = 0; i < Landscape.SPLATMAP_RESOLUTION; i++)
					{
						for (int j = 0; j < Landscape.SPLATMAP_RESOLUTION; j++)
						{
							for (int k = 0; k < Landscape.SPLATMAP_LAYERS; k++)
							{
								float num3 = (float)((byte)sha1Stream.ReadByte()) / 255f;
								splatmap[i, j, k] = num3;
							}
						}
					}
					Level.includeHash(text, sha1Stream.Hash);
				}
			}
		}

		// Token: 0x06000473 RID: 1139 RVA: 0x00011F64 File Offset: 0x00010164
		private void ReadHoles()
		{
			string[] array = new string[5];
			array[0] = "Tile_";
			int num = 1;
			LandscapeCoord coord = this.coord;
			array[num] = coord.x.ToString(CultureInfo.InvariantCulture);
			array[2] = "_";
			int num2 = 3;
			coord = this.coord;
			array[num2] = coord.y.ToString(CultureInfo.InvariantCulture);
			array[4] = ".bin";
			string text = string.Concat(array);
			string text2 = Level.info.path + "/Landscape/Holes/" + text;
			if (!File.Exists(text2))
			{
				return;
			}
			using (FileStream fileStream = new FileStream(text2, 3, 1, 3))
			{
				using (SHA1Stream sha1Stream = new SHA1Stream(fileStream))
				{
					sha1Stream.ReadByte();
					for (int i = 0; i < 256; i++)
					{
						for (int j = 0; j < 256; j += 8)
						{
							byte b = (byte)sha1Stream.ReadByte();
							for (int k = 0; k < 8; k++)
							{
								this.holes[i, j + k] = (((int)b & 1 << k) > 0);
							}
						}
					}
					Level.includeHash(text, sha1Stream.Hash);
				}
			}
			this.data.SetHoles(0, 0, this.holes);
			this.hasAnyHolesData = true;
		}

		// Token: 0x06000474 RID: 1140 RVA: 0x000120C0 File Offset: 0x000102C0
		public virtual void write(IFormattedFileWriter writer)
		{
			writer.beginObject();
			writer.writeValue<LandscapeCoord>("Coord", this.coord);
			writer.beginArray("Materials");
			for (int i = 0; i < Landscape.SPLATMAP_LAYERS; i++)
			{
				writer.writeValue<AssetReference<LandscapeMaterialAsset>>(this.materials[i]);
			}
			writer.endArray();
			writer.endObject();
			this.writeHeightmaps();
			this.writeSplatmaps();
			if (this.hasAnyHolesData)
			{
				this.WriteHoles();
			}
		}

		// Token: 0x06000475 RID: 1141 RVA: 0x00012137 File Offset: 0x00010337
		public virtual void writeHeightmaps()
		{
			this.writeHeightmap("_Source", this.heightmap);
		}

		// Token: 0x06000476 RID: 1142 RVA: 0x0001214C File Offset: 0x0001034C
		protected virtual void writeHeightmap(string suffix, float[,] heightmap)
		{
			string[] array = new string[7];
			array[0] = Level.info.path;
			array[1] = "/Landscape/Heightmaps/Tile_";
			int num = 2;
			LandscapeCoord coord = this.coord;
			array[num] = coord.x.ToString(CultureInfo.InvariantCulture);
			array[3] = "_";
			int num2 = 4;
			coord = this.coord;
			array[num2] = coord.y.ToString(CultureInfo.InvariantCulture);
			array[5] = suffix;
			array[6] = ".heightmap";
			string text = string.Concat(array);
			string directoryName = Path.GetDirectoryName(text);
			if (!Directory.Exists(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}
			using (FileStream fileStream = new FileStream(text, 4, 2, 3))
			{
				for (int i = 0; i < Landscape.HEIGHTMAP_RESOLUTION; i++)
				{
					for (int j = 0; j < Landscape.HEIGHTMAP_RESOLUTION; j++)
					{
						ushort num3 = (ushort)Mathf.RoundToInt(heightmap[i, j] * 65535f);
						fileStream.WriteByte((byte)(num3 >> 8 & 255));
						fileStream.WriteByte((byte)(num3 & 255));
					}
				}
			}
		}

		// Token: 0x06000477 RID: 1143 RVA: 0x0001225C File Offset: 0x0001045C
		public virtual void writeSplatmaps()
		{
			this.writeSplatmap("_Source", this.splatmap);
		}

		// Token: 0x06000478 RID: 1144 RVA: 0x00012270 File Offset: 0x00010470
		protected virtual void writeSplatmap(string suffix, float[,,] splatmap)
		{
			string[] array = new string[7];
			array[0] = Level.info.path;
			array[1] = "/Landscape/Splatmaps/Tile_";
			int num = 2;
			LandscapeCoord coord = this.coord;
			array[num] = coord.x.ToString(CultureInfo.InvariantCulture);
			array[3] = "_";
			int num2 = 4;
			coord = this.coord;
			array[num2] = coord.y.ToString(CultureInfo.InvariantCulture);
			array[5] = suffix;
			array[6] = ".splatmap";
			string text = string.Concat(array);
			string directoryName = Path.GetDirectoryName(text);
			if (!Directory.Exists(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}
			using (FileStream fileStream = new FileStream(text, 4, 2, 3))
			{
				for (int i = 0; i < Landscape.SPLATMAP_RESOLUTION; i++)
				{
					for (int j = 0; j < Landscape.SPLATMAP_RESOLUTION; j++)
					{
						for (int k = 0; k < Landscape.SPLATMAP_LAYERS; k++)
						{
							byte b = (byte)Mathf.RoundToInt(splatmap[i, j, k] * 255f);
							fileStream.WriteByte(b);
						}
					}
				}
			}
		}

		// Token: 0x06000479 RID: 1145 RVA: 0x0001237C File Offset: 0x0001057C
		private void WriteHoles()
		{
			string[] array = new string[6];
			array[0] = Level.info.path;
			array[1] = "/Landscape/Holes/Tile_";
			int num = 2;
			LandscapeCoord coord = this.coord;
			array[num] = coord.x.ToString(CultureInfo.InvariantCulture);
			array[3] = "_";
			int num2 = 4;
			coord = this.coord;
			array[num2] = coord.y.ToString(CultureInfo.InvariantCulture);
			array[5] = ".bin";
			string text = string.Concat(array);
			string directoryName = Path.GetDirectoryName(text);
			if (!Directory.Exists(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}
			using (FileStream fileStream = new FileStream(text, 4, 2, 3))
			{
				fileStream.WriteByte(1);
				for (int i = 0; i < 256; i++)
				{
					for (int j = 0; j < 256; j += 8)
					{
						byte b = this.holes[i, j] ? 1 : 0;
						for (int k = 1; k < 8; k++)
						{
							b |= (this.holes[i, j + k] ? ((byte)(1 << k)) : 0);
						}
						fileStream.WriteByte(b);
					}
				}
			}
		}

		/// <summary>
		/// Call this when done changing material references to grab their textures and pass them to the terrain renderer.
		/// </summary>
		// Token: 0x0600047A RID: 1146 RVA: 0x000124A8 File Offset: 0x000106A8
		public void updatePrototypes()
		{
		}

		// Token: 0x0600047B RID: 1147 RVA: 0x000124B8 File Offset: 0x000106B8
		protected void updateTransform()
		{
			this.gameObject.transform.position = new Vector3((float)this.coord.x * Landscape.TILE_SIZE, -Landscape.TILE_HEIGHT / 2f, (float)this.coord.y * Landscape.TILE_SIZE);
		}

		// Token: 0x0600047C RID: 1148 RVA: 0x0001250C File Offset: 0x0001070C
		public void convertLegacyHeightmap()
		{
			for (int i = 0; i < Landscape.HEIGHTMAP_RESOLUTION; i++)
			{
				for (int j = 0; j < Landscape.HEIGHTMAP_RESOLUTION; j++)
				{
					HeightmapCoord heightmapCoord = new HeightmapCoord(i, j);
					float num = LevelGround.getConversionHeight(Landscape.getWorldPosition(this.coord, heightmapCoord, this.heightmap[i, j]));
					num /= Landscape.TILE_HEIGHT;
					num += 0.5f;
					this.heightmap[i, j] = num;
				}
			}
			this.data.SetHeights(0, 0, this.heightmap);
			if (this.dataWithoutHoles != null)
			{
				this.dataWithoutHoles.SetHeights(0, 0, this.heightmap);
			}
		}

		// Token: 0x0600047D RID: 1149 RVA: 0x000125B4 File Offset: 0x000107B4
		public void convertLegacySplatmap()
		{
			for (int i = 0; i < Landscape.SPLATMAP_RESOLUTION; i++)
			{
				for (int j = 0; j < Landscape.SPLATMAP_RESOLUTION; j++)
				{
					SplatmapCoord splatmapCoord = new SplatmapCoord(i, j);
					Vector3 worldPosition = Landscape.getWorldPosition(this.coord, splatmapCoord);
					for (int k = 0; k < Landscape.SPLATMAP_LAYERS; k++)
					{
						float conversionWeight = LevelGround.getConversionWeight(worldPosition, k);
						this.splatmap[i, j, k] = conversionWeight;
					}
				}
			}
		}

		// Token: 0x0600047E RID: 1150 RVA: 0x00012628 File Offset: 0x00010828
		public void resetHeightmap()
		{
			for (int i = 0; i < Landscape.HEIGHTMAP_RESOLUTION; i++)
			{
				for (int j = 0; j < Landscape.HEIGHTMAP_RESOLUTION; j++)
				{
					this.heightmap[i, j] = 0.5f;
				}
			}
			Landscape.reconcileNeighbors(this);
			this.SyncHeightmap();
		}

		// Token: 0x0600047F RID: 1151 RVA: 0x00012674 File Offset: 0x00010874
		public void resetSplatmap()
		{
			for (int i = 0; i < Landscape.SPLATMAP_RESOLUTION; i++)
			{
				for (int j = 0; j < Landscape.SPLATMAP_RESOLUTION; j++)
				{
					this.splatmap[i, j, 0] = 1f;
					for (int k = 1; k < Landscape.SPLATMAP_LAYERS; k++)
					{
						this.splatmap[i, j, k] = 0f;
					}
				}
			}
			this.data.SetAlphamaps(0, 0, this.splatmap);
		}

		// Token: 0x06000480 RID: 1152 RVA: 0x000126EC File Offset: 0x000108EC
		public void normalizeSplatmap()
		{
			for (int i = 0; i < Landscape.SPLATMAP_RESOLUTION; i++)
			{
				for (int j = 0; j < Landscape.SPLATMAP_RESOLUTION; j++)
				{
					float num = 0f;
					for (int k = 0; k < Landscape.SPLATMAP_LAYERS; k++)
					{
						num += this.splatmap[i, j, k];
					}
					for (int l = 0; l < Landscape.SPLATMAP_LAYERS; l++)
					{
						this.splatmap[i, j, l] /= num;
					}
				}
			}
			this.data.SetAlphamaps(0, 0, this.splatmap);
		}

		// Token: 0x06000481 RID: 1153 RVA: 0x0001277C File Offset: 0x0001097C
		public void applyGraphicsSettings()
		{
		}

		// Token: 0x06000482 RID: 1154 RVA: 0x0001278C File Offset: 0x0001098C
		public FoliageBounds getFoliageSurfaceBounds()
		{
			return new FoliageBounds(new FoliageCoord(this.coord.x * Landscape.TILE_SIZE_INT / FoliageSystem.TILE_SIZE_INT, this.coord.y * Landscape.TILE_SIZE_INT / FoliageSystem.TILE_SIZE_INT), new FoliageCoord((this.coord.x + 1) * Landscape.TILE_SIZE_INT / FoliageSystem.TILE_SIZE_INT - 1, (this.coord.y + 1) * Landscape.TILE_SIZE_INT / FoliageSystem.TILE_SIZE_INT - 1));
		}

		// Token: 0x06000483 RID: 1155 RVA: 0x0001280C File Offset: 0x00010A0C
		public bool getFoliageSurfaceInfo(Vector3 position, out Vector3 surfacePosition, out Vector3 surfaceNormal)
		{
			surfacePosition = position;
			surfacePosition.y = this.terrain.SampleHeight(position) - Landscape.TILE_HEIGHT / 2f;
			surfaceNormal = this.data.GetInterpolatedNormal((position.x - (float)this.coord.x * Landscape.TILE_SIZE) / Landscape.TILE_SIZE, (position.z - (float)this.coord.y * Landscape.TILE_SIZE) / Landscape.TILE_SIZE);
			return !Landscape.IsPointInsideHole(surfacePosition);
		}

		// Token: 0x06000484 RID: 1156 RVA: 0x0001289C File Offset: 0x00010A9C
		public void bakeFoliageSurface(FoliageBakeSettings bakeSettings, FoliageTile foliageTile)
		{
			int num = (foliageTile.coord.y * FoliageSystem.TILE_SIZE_INT - this.coord.y * Landscape.TILE_SIZE_INT) / FoliageSystem.TILE_SIZE_INT * FoliageSystem.SPLATMAP_RESOLUTION_PER_TILE;
			int num2 = num + FoliageSystem.SPLATMAP_RESOLUTION_PER_TILE;
			int num3 = (foliageTile.coord.x * FoliageSystem.TILE_SIZE_INT - this.coord.x * Landscape.TILE_SIZE_INT) / FoliageSystem.TILE_SIZE_INT * FoliageSystem.SPLATMAP_RESOLUTION_PER_TILE;
			int num4 = num3 + FoliageSystem.SPLATMAP_RESOLUTION_PER_TILE;
			for (int i = num; i < num2; i++)
			{
				for (int j = num3; j < num4; j++)
				{
					SplatmapCoord splatmapCoord = new SplatmapCoord(i, j);
					float num5 = (float)this.coord.x * Landscape.TILE_SIZE + (float)splatmapCoord.y * Landscape.SPLATMAP_WORLD_UNIT;
					float num6 = (float)this.coord.y * Landscape.TILE_SIZE + (float)splatmapCoord.x * Landscape.SPLATMAP_WORLD_UNIT;
					Bounds bounds = default(Bounds);
					bounds.min = new Vector3(num5, 0f, num6);
					bounds.max = new Vector3(num5 + Landscape.SPLATMAP_WORLD_UNIT, 0f, num6 + Landscape.SPLATMAP_WORLD_UNIT);
					for (int k = 0; k < Landscape.SPLATMAP_LAYERS; k++)
					{
						float num7 = this.splatmap[i, j, k];
						if (num7 >= 0.01f)
						{
							LandscapeMaterialAsset landscapeMaterialAsset = Assets.find<LandscapeMaterialAsset>(this.materials[k]);
							if (landscapeMaterialAsset != null)
							{
								FoliageInfoCollectionAsset foliageInfoCollectionAsset = Assets.find<FoliageInfoCollectionAsset>(landscapeMaterialAsset.foliage);
								if (foliageInfoCollectionAsset != null)
								{
									foliageInfoCollectionAsset.bakeFoliage(bakeSettings, this, bounds, num7);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06000485 RID: 1157 RVA: 0x00012A33 File Offset: 0x00010C33
		protected virtual void handleMaterialsInspectorChanged(IInspectableList list)
		{
			this.updatePrototypes();
		}

		// Token: 0x06000486 RID: 1158 RVA: 0x00012A3B File Offset: 0x00010C3B
		public virtual void enable()
		{
			FoliageSystem.addSurface(this);
		}

		// Token: 0x06000487 RID: 1159 RVA: 0x00012A43 File Offset: 0x00010C43
		public virtual void disable()
		{
			FoliageSystem.removeSurface(this);
		}

		// Token: 0x06000488 RID: 1160 RVA: 0x00012A4C File Offset: 0x00010C4C
		public LandscapeTile(LandscapeCoord newCoord)
		{
			this.gameObject = new GameObject();
			this.gameObject.tag = "Ground";
			this.gameObject.layer = 20;
			this.gameObject.transform.rotation = MathUtility.IDENTITY_QUATERNION;
			this.gameObject.transform.localScale = Vector3.one;
			this.coord = newCoord;
			this.heightmap = new float[Landscape.HEIGHTMAP_RESOLUTION, Landscape.HEIGHTMAP_RESOLUTION];
			this.splatmap = new float[Landscape.SPLATMAP_RESOLUTION, Landscape.SPLATMAP_RESOLUTION, Landscape.SPLATMAP_LAYERS];
			this.holes = new bool[256, 256];
			for (int i = 0; i < Landscape.HEIGHTMAP_RESOLUTION; i++)
			{
				for (int j = 0; j < Landscape.HEIGHTMAP_RESOLUTION; j++)
				{
					this.heightmap[i, j] = 0.5f;
				}
			}
			for (int k = 0; k < Landscape.SPLATMAP_RESOLUTION; k++)
			{
				for (int l = 0; l < Landscape.SPLATMAP_RESOLUTION; l++)
				{
					this.splatmap[k, l, 0] = 1f;
				}
			}
			for (int m = 0; m < 256; m++)
			{
				for (int n = 0; n < 256; n++)
				{
					this.holes[m, n] = true;
				}
			}
			this.materials = new InspectableList<AssetReference<LandscapeMaterialAsset>>(Landscape.SPLATMAP_LAYERS);
			this.materials.Add(LandscapeTile.DEFAULT_MATERIAL);
			for (int num = 1; num < Landscape.SPLATMAP_LAYERS; num++)
			{
				this.materials.Add(AssetReference<LandscapeMaterialAsset>.invalid);
			}
			this.materials.canInspectorAdd = false;
			this.materials.canInspectorRemove = false;
			this.materials.inspectorChanged += this.handleMaterialsInspectorChanged;
			this.data = new TerrainData();
			this.data.heightmapResolution = Landscape.HEIGHTMAP_RESOLUTION;
			this.data.alphamapResolution = Landscape.SPLATMAP_RESOLUTION;
			this.data.baseMapResolution = Landscape.BASEMAP_RESOLUTION;
			this.data.size = new Vector3(Landscape.TILE_SIZE, Landscape.TILE_HEIGHT, Landscape.TILE_SIZE);
			this.data.SetHeightsDelayLOD(0, 0, this.heightmap);
			this.data.wavingGrassTint = Color.white;
			this.terrain = this.gameObject.AddComponent<Terrain>();
			this.terrain.drawInstanced = SystemInfo.supportsInstancing;
			this.terrain.terrainData = this.data;
			this.terrain.heightmapPixelError = 200f;
			this.terrain.reflectionProbeUsage = ReflectionProbeUsage.Off;
			this.terrain.shadowCastingMode = ShadowCastingMode.Off;
			this.terrain.drawHeightmap = false;
			this.terrain.drawTreesAndFoliage = false;
			this.terrain.collectDetailPatches = false;
			this.terrain.allowAutoConnect = false;
			this.terrain.groupingID = 1;
			this.terrain.Flush();
			if (Level.isEditor)
			{
				this.dataWithoutHoles = new TerrainData();
				this.dataWithoutHoles.heightmapResolution = this.data.heightmapResolution;
				this.dataWithoutHoles.size = this.data.size;
				this.dataWithoutHoles.SetHeightsDelayLOD(0, 0, this.heightmap);
			}
			this.collider = this.gameObject.AddComponent<TerrainCollider>();
			this.collider.terrainData = ((Level.isEditor && Landscape.DisableHoleColliders) ? this.dataWithoutHoles : this.data);
		}

		// Token: 0x06000489 RID: 1161 RVA: 0x00012DB4 File Offset: 0x00010FB4
		[Conditional("UNITY_EDITOR")]
		[Conditional("DEVELOPMENT_BUILD")]
		private void UpdateNames()
		{
			this.gameObject.name = string.Format("Terrain ({0}, {1})", this.coord.x, this.coord.y);
			this.data.name = string.Format("x: {0} y: {1}", this.coord.x, this.coord.y);
			if (this.dataWithoutHoles != null)
			{
				this.dataWithoutHoles.name = this.data.name + " (without holes)";
			}
		}

		// Token: 0x040001D2 RID: 466
		protected LandscapeCoord _coord;

		// Token: 0x040001D3 RID: 467
		public float[,] heightmap;

		// Token: 0x040001D4 RID: 468
		public float[,,] splatmap;

		/// <summary>
		/// True is solid and false is empty.
		/// </summary>
		// Token: 0x040001D5 RID: 469
		public bool[,] holes;

		/// <summary>
		/// Marked true when level editor or legacy hole volumes modify hole data.
		/// Defaults to false in which case holes do not need to be saved.
		///
		/// Initially this was not going to be marked by hole volumes because they can re-generate the holes, but saving
		/// hole volume cuts is helpful when upgrading to remove hole volumes from a map.
		/// </summary>
		// Token: 0x040001D6 RID: 470
		public bool hasAnyHolesData;

		// Token: 0x040001D8 RID: 472
		private TerrainLayer[] terrainLayers;

		/// <summary>
		/// Heightmap-only data used in level editor. Refer to Landscape.DisableHoleColliders for explanation.
		/// </summary>
		// Token: 0x040001DA RID: 474
		public TerrainData dataWithoutHoles;

		// Token: 0x040001DD RID: 477
		private static AssetReference<LandscapeMaterialAsset> DEFAULT_MATERIAL = new AssetReference<LandscapeMaterialAsset>("498ca625072d443a876b2a4f11896018");
	}
}
