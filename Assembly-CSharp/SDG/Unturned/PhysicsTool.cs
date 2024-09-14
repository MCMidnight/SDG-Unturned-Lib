using System;
using SDG.Framework.Landscapes;
using SDG.Framework.Water;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000763 RID: 1891
	public class PhysicsTool
	{
		// Token: 0x06003DD1 RID: 15825 RVA: 0x0012B284 File Offset: 0x00129484
		[Obsolete("Intended for backwards compatibility")]
		public static string GetNameOfLegacyMaterial(EPhysicsMaterial material)
		{
			switch (material)
			{
			default:
				return null;
			case EPhysicsMaterial.CLOTH_DYNAMIC:
			case EPhysicsMaterial.CLOTH_STATIC:
				return "Cloth";
			case EPhysicsMaterial.TILE_DYNAMIC:
			case EPhysicsMaterial.TILE_STATIC:
				return "Tile";
			case EPhysicsMaterial.CONCRETE_DYNAMIC:
			case EPhysicsMaterial.CONCRETE_STATIC:
				return "Concrete";
			case EPhysicsMaterial.FLESH_DYNAMIC:
				return "Flesh";
			case EPhysicsMaterial.GRAVEL_DYNAMIC:
			case EPhysicsMaterial.GRAVEL_STATIC:
				return "Gravel";
			case EPhysicsMaterial.METAL_DYNAMIC:
			case EPhysicsMaterial.METAL_STATIC:
			case EPhysicsMaterial.METAL_SLIP:
				return "Metal";
			case EPhysicsMaterial.WOOD_DYNAMIC:
			case EPhysicsMaterial.WOOD_STATIC:
				return "Wood";
			case EPhysicsMaterial.FOLIAGE_STATIC:
			case EPhysicsMaterial.FOLIAGE_DYNAMIC:
				return "Foliage";
			case EPhysicsMaterial.SNOW_STATIC:
				return "Snow";
			case EPhysicsMaterial.ICE_STATIC:
				return "Ice";
			case EPhysicsMaterial.WATER_STATIC:
				return "Water";
			case EPhysicsMaterial.ALIEN_DYNAMIC:
				return "Alien";
			case EPhysicsMaterial.SAND_STATIC:
				return "Sand";
			}
		}

		// Token: 0x06003DD2 RID: 15826 RVA: 0x0012B340 File Offset: 0x00129540
		public static string GetTerrainMaterialName(Vector3 position)
		{
			AssetReference<LandscapeMaterialAsset> reference;
			if (Landscape.getSplatmapMaterial(position, out reference))
			{
				LandscapeMaterialAsset landscapeMaterialAsset = Assets.find<LandscapeMaterialAsset>(reference);
				if (landscapeMaterialAsset != null)
				{
					return landscapeMaterialAsset.physicsMaterialName;
				}
			}
			return null;
		}

		// Token: 0x06003DD3 RID: 15827 RVA: 0x0012B36C File Offset: 0x0012956C
		[Obsolete("Replaced by GetTerrainMaterialName")]
		public static EPhysicsMaterial checkMaterial(Vector3 point)
		{
			AssetReference<LandscapeMaterialAsset> reference;
			if (Landscape.getSplatmapMaterial(point, out reference))
			{
				LandscapeMaterialAsset landscapeMaterialAsset = Assets.find<LandscapeMaterialAsset>(reference);
				if (landscapeMaterialAsset != null)
				{
					return landscapeMaterialAsset.physicsMaterial;
				}
			}
			return EPhysicsMaterial.NONE;
		}

		// Token: 0x06003DD4 RID: 15828 RVA: 0x0012B398 File Offset: 0x00129598
		[Obsolete("Network attachment removes the need for distinction between dynamic and static materials")]
		public static bool isMaterialDynamic(EPhysicsMaterial material)
		{
			switch (material)
			{
			case EPhysicsMaterial.CLOTH_DYNAMIC:
				return true;
			case EPhysicsMaterial.TILE_DYNAMIC:
				return true;
			case EPhysicsMaterial.CONCRETE_DYNAMIC:
				return true;
			case EPhysicsMaterial.FLESH_DYNAMIC:
				return true;
			case EPhysicsMaterial.GRAVEL_DYNAMIC:
				return true;
			case EPhysicsMaterial.METAL_DYNAMIC:
				return true;
			case EPhysicsMaterial.WOOD_DYNAMIC:
				return true;
			}
			return false;
		}

		/// <summary>
		/// Get legacy enum corresponding to Unity physics material object name.
		/// Moved from obsolete <cref>checkMaterial</cref> method.
		/// </summary>
		// Token: 0x06003DD5 RID: 15829 RVA: 0x0012B3F4 File Offset: 0x001295F4
		[Obsolete("Intended for backwards compatibility")]
		public static EPhysicsMaterial GetLegacyMaterialByName(string name)
		{
			uint num = <PrivateImplementationDetails>.ComputeStringHash(name);
			if (num <= 2550614696U)
			{
				if (num <= 1010759330U)
				{
					if (num <= 118068405U)
					{
						if (num <= 78706450U)
						{
							if (num != 51878123U)
							{
								if (num == 78706450U)
								{
									if (name == "Snow")
									{
										return EPhysicsMaterial.SNOW_STATIC;
									}
								}
							}
							else if (name == "Wood_Static")
							{
								return EPhysicsMaterial.WOOD_STATIC;
							}
						}
						else if (num != 81868168U)
						{
							if (num == 118068405U)
							{
								if (name == "Sand_Dynamic")
								{
									return EPhysicsMaterial.SAND_STATIC;
								}
							}
						}
						else if (name == "Wood")
						{
							return EPhysicsMaterial.WOOD_STATIC;
						}
					}
					else if (num <= 487873524U)
					{
						if (num != 210636655U)
						{
							if (num == 487873524U)
							{
								if (name == "Flesh_Static")
								{
									return EPhysicsMaterial.FLESH_DYNAMIC;
								}
							}
						}
						else if (name == "Metal_Static")
						{
							return EPhysicsMaterial.METAL_STATIC;
						}
					}
					else if (num != 685078495U)
					{
						if (num != 911566717U)
						{
							if (num == 1010759330U)
							{
								if (name == "Metal_Dynamic")
								{
									return EPhysicsMaterial.METAL_DYNAMIC;
								}
							}
						}
						else if (name == "Snow_Static")
						{
							return EPhysicsMaterial.SNOW_STATIC;
						}
					}
					else if (name == "Tile_Dynamic")
					{
						return EPhysicsMaterial.TILE_DYNAMIC;
					}
				}
				else if (num <= 1715042677U)
				{
					if (num <= 1044434307U)
					{
						if (num != 1011963518U)
						{
							if (num == 1044434307U)
							{
								if (name == "Sand")
								{
									return EPhysicsMaterial.SAND_STATIC;
								}
							}
						}
						else if (name == "Wood_Dynamic")
						{
							return EPhysicsMaterial.WOOD_DYNAMIC;
						}
					}
					else if (num != 1299728523U)
					{
						if (num == 1715042677U)
						{
							if (name == "Gravel_Static")
							{
								return EPhysicsMaterial.GRAVEL_STATIC;
							}
						}
					}
					else if (name == "Foliage_Static")
					{
						return EPhysicsMaterial.FOLIAGE_STATIC;
					}
				}
				else if (num <= 1886027574U)
				{
					if (num != 1819043923U)
					{
						if (num == 1886027574U)
						{
							if (name == "Water_Dynamic")
							{
								return EPhysicsMaterial.WATER_STATIC;
							}
						}
					}
					else if (name == "Ice_Static")
					{
						return EPhysicsMaterial.ICE_STATIC;
					}
				}
				else if (num != 2326475139U)
				{
					if (num != 2391610136U)
					{
						if (num == 2550614696U)
						{
							if (name == "Foliage")
							{
								return EPhysicsMaterial.FOLIAGE_STATIC;
							}
						}
					}
					else if (name == "Concrete_Dynamic")
					{
						return EPhysicsMaterial.CONCRETE_DYNAMIC;
					}
				}
				else if (name == "Water_Static")
				{
					return EPhysicsMaterial.WATER_STATIC;
				}
			}
			else if (num <= 3240848276U)
			{
				if (num <= 2840670588U)
				{
					if (num <= 2570630558U)
					{
						if (num != 2553495518U)
						{
							if (num == 2570630558U)
							{
								if (name == "Foliage_Dynamic")
								{
									return EPhysicsMaterial.FOLIAGE_DYNAMIC;
								}
							}
						}
						else if (name == "Concrete")
						{
							return EPhysicsMaterial.CONCRETE_STATIC;
						}
					}
					else if (num != 2793729766U)
					{
						if (num == 2840670588U)
						{
							if (name == "Metal")
							{
								return EPhysicsMaterial.METAL_STATIC;
							}
						}
					}
					else if (name == "Ice_Dynamic")
					{
						return EPhysicsMaterial.ICE_STATIC;
					}
				}
				else if (num <= 2897181954U)
				{
					if (num != 2872292845U)
					{
						if (num == 2897181954U)
						{
							if (name == "Cloth_Static")
							{
								return EPhysicsMaterial.CLOTH_STATIC;
							}
						}
					}
					else if (name == "Flesh")
					{
						return EPhysicsMaterial.FLESH_DYNAMIC;
					}
				}
				else if (num != 2990136679U)
				{
					if (num != 2995012523U)
					{
						if (num == 3240848276U)
						{
							if (name == "Gravel_Dynamic")
							{
								return EPhysicsMaterial.GRAVEL_DYNAMIC;
							}
						}
					}
					else if (name == "Cloth")
					{
						return EPhysicsMaterial.CLOTH_STATIC;
					}
				}
				else if (name == "Metal_Slip")
				{
					return EPhysicsMaterial.METAL_SLIP;
				}
			}
			else if (num <= 3880682275U)
			{
				if (num <= 3593819824U)
				{
					if (num != 3580699424U)
					{
						if (num == 3593819824U)
						{
							if (name == "Water")
							{
								return EPhysicsMaterial.WATER_STATIC;
							}
						}
					}
					else if (name == "Ice")
					{
						return EPhysicsMaterial.ICE_STATIC;
					}
				}
				else if (num != 3690703481U)
				{
					if (num != 3705351136U)
					{
						if (num == 3880682275U)
						{
							if (name == "Flesh_Dynamic")
							{
								return EPhysicsMaterial.FLESH_DYNAMIC;
							}
						}
					}
					else if (name == "Tile_Static")
					{
						return EPhysicsMaterial.TILE_STATIC;
					}
				}
				else if (name == "Concrete_Static")
				{
					return EPhysicsMaterial.CONCRETE_STATIC;
				}
			}
			else if (num <= 3979223562U)
			{
				if (num != 3951150012U)
				{
					if (num == 3979223562U)
					{
						if (name == "Sand_Static")
						{
							return EPhysicsMaterial.SAND_STATIC;
						}
					}
				}
				else if (name == "Snow_Dynamic")
				{
					return EPhysicsMaterial.SNOW_STATIC;
				}
			}
			else if (num != 4091621865U)
			{
				if (num != 4176637722U)
				{
					if (num == 4247685421U)
					{
						if (name == "Cloth_Dynamic")
						{
							return EPhysicsMaterial.CLOTH_DYNAMIC;
						}
					}
				}
				else if (name == "Gravel")
				{
					return EPhysicsMaterial.GRAVEL_STATIC;
				}
			}
			else if (name == "Tile")
			{
				return EPhysicsMaterial.TILE_STATIC;
			}
			return EPhysicsMaterial.NONE;
		}

		// Token: 0x06003DD6 RID: 15830 RVA: 0x0012B9C7 File Offset: 0x00129BC7
		[Obsolete("Replaced by GetMaterialName")]
		public static EPhysicsMaterial checkMaterial(Collider collider)
		{
			if (collider.sharedMaterial == null)
			{
				return EPhysicsMaterial.NONE;
			}
			return PhysicsTool.GetLegacyMaterialByName(collider.sharedMaterial.name);
		}

		// Token: 0x06003DD7 RID: 15831 RVA: 0x0012B9EC File Offset: 0x00129BEC
		public static string GetMaterialName(Vector3 point, Transform transform, Collider collider)
		{
			if (WaterUtility.isPointUnderwater(point))
			{
				return "Water_Static";
			}
			if (transform != null && transform.CompareTag("Ground"))
			{
				return PhysicsTool.GetTerrainMaterialName(point);
			}
			if (collider == null)
			{
				return null;
			}
			PhysicMaterial sharedMaterial = collider.sharedMaterial;
			if (sharedMaterial == null)
			{
				return null;
			}
			return sharedMaterial.name;
		}

		// Token: 0x06003DD8 RID: 15832 RVA: 0x0012BA3A File Offset: 0x00129C3A
		public static string GetMaterialName(RaycastHit hit)
		{
			return PhysicsTool.GetMaterialName(hit.point, hit.transform, hit.collider);
		}

		// Token: 0x06003DD9 RID: 15833 RVA: 0x0012BA56 File Offset: 0x00129C56
		public static string GetMaterialName(WheelHit hit)
		{
			Vector3 point = hit.point;
			Collider collider = hit.collider;
			return PhysicsTool.GetMaterialName(point, (collider != null) ? collider.transform : null, hit.collider);
		}

		// Token: 0x040026DF RID: 9951
		internal const int NAME_LENGTH_BITS = 6;
	}
}
