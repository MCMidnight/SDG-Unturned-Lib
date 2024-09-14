using System;
using System.Collections.Generic;
using SDG.Provider;
using UnityEngine;
using UnityEngine.Rendering;

namespace SDG.Unturned
{
	// Token: 0x0200075A RID: 1882
	public class ItemTool : MonoBehaviour
	{
		// Token: 0x06003D83 RID: 15747 RVA: 0x001273B0 File Offset: 0x001255B0
		public static string filterRarityRichText(string desc)
		{
			if (!string.IsNullOrEmpty(desc))
			{
				desc = desc.Replace("color=common", "color=#ffffff");
				desc = desc.Replace("color=gold", "color=#d2bf22");
				desc = desc.Replace("color=uncommon", "color=#1f871f");
				desc = desc.Replace("color=rare", "color=#4b64fa");
				desc = desc.Replace("color=epic", "color=#964bfa");
				desc = desc.Replace("color=legendary", "color=#c832fa");
				desc = desc.Replace("color=mythical", "color=#fa3219");
				desc = desc.Replace("color=red", "color=#bf1f1f");
				desc = desc.Replace("color=green", "color=#1f871f");
				desc = desc.Replace("color=blue", "color=#3298c8");
				desc = desc.Replace("color=orange", "color=#ab8019");
				desc = desc.Replace("color=yellow", "color=#dcb413");
				desc = desc.Replace("color=purple", "color=#6a466d");
			}
			return desc;
		}

		// Token: 0x06003D84 RID: 15748 RVA: 0x001274B4 File Offset: 0x001256B4
		public static Color getRarityColorHighlight(EItemRarity rarity)
		{
			switch (rarity)
			{
			case EItemRarity.COMMON:
				return ItemTool.RARITY_COMMON_HIGHLIGHT;
			case EItemRarity.UNCOMMON:
				return ItemTool.RARITY_UNCOMMON_HIGHLIGHT;
			case EItemRarity.RARE:
				return ItemTool.RARITY_RARE_HIGHLIGHT;
			case EItemRarity.EPIC:
				return ItemTool.RARITY_EPIC_HIGHLIGHT;
			case EItemRarity.LEGENDARY:
				return ItemTool.RARITY_LEGENDARY_HIGHLIGHT;
			case EItemRarity.MYTHICAL:
				return ItemTool.RARITY_MYTHICAL_HIGHLIGHT;
			default:
				return Color.white;
			}
		}

		// Token: 0x06003D85 RID: 15749 RVA: 0x0012750C File Offset: 0x0012570C
		public static Color getRarityColorUI(EItemRarity rarity)
		{
			switch (rarity)
			{
			case EItemRarity.COMMON:
				return ItemTool.RARITY_COMMON_UI;
			case EItemRarity.UNCOMMON:
				return ItemTool.RARITY_UNCOMMON_UI;
			case EItemRarity.RARE:
				return ItemTool.RARITY_RARE_UI;
			case EItemRarity.EPIC:
				return ItemTool.RARITY_EPIC_UI;
			case EItemRarity.LEGENDARY:
				return ItemTool.RARITY_LEGENDARY_UI;
			case EItemRarity.MYTHICAL:
				return ItemTool.RARITY_MYTHICAL_UI;
			default:
				return Color.white;
			}
		}

		// Token: 0x06003D86 RID: 15750 RVA: 0x00127562 File Offset: 0x00125762
		public static Color getQualityColor(float quality)
		{
			if (quality < 0.5f)
			{
				return Color.Lerp(Palette.COLOR_R, Palette.COLOR_Y, quality * 2f);
			}
			return Color.Lerp(Palette.COLOR_Y, Palette.COLOR_G, (quality - 0.5f) * 2f);
		}

		// Token: 0x06003D87 RID: 15751 RVA: 0x001275A0 File Offset: 0x001257A0
		public static void ApplyMythicalEffectToMultipleTransforms(Transform[] bones, MythicalEffectController[] systems, ushort mythicID, EEffectType type)
		{
			if (bones == null || systems == null)
			{
				return;
			}
			if (mythicID == 0)
			{
				for (int i = 0; i < bones.Length; i++)
				{
					systems[i] = null;
				}
				return;
			}
			MythicAsset mythicAsset = Assets.find(EAssetType.MYTHIC, mythicID) as MythicAsset;
			if (mythicAsset == null)
			{
				for (int j = 0; j < bones.Length; j++)
				{
					systems[j] = null;
				}
				return;
			}
			for (int k = 0; k < bones.Length; k++)
			{
				systems[k] = ItemTool.ApplyMythicalEffect(bones[k], mythicAsset, type);
			}
		}

		// Token: 0x06003D88 RID: 15752 RVA: 0x00127608 File Offset: 0x00125808
		public static MythicalEffectController ApplyMythicalEffect(Transform parent, ushort mythicID, EEffectType type)
		{
			if (mythicID == 0)
			{
				return null;
			}
			if (parent == null)
			{
				return null;
			}
			MythicAsset mythicAsset = Assets.find(EAssetType.MYTHIC, mythicID) as MythicAsset;
			if (mythicAsset == null)
			{
				return null;
			}
			return ItemTool.ApplyMythicalEffect(parent, mythicAsset, type);
		}

		// Token: 0x06003D89 RID: 15753 RVA: 0x00127640 File Offset: 0x00125840
		private static MythicalEffectController ApplyMythicalEffect(Transform parent, MythicAsset mythicAsset, EEffectType type)
		{
			if (mythicAsset == null)
			{
				return null;
			}
			if (parent == null)
			{
				return null;
			}
			GameObject gameObject;
			switch (type)
			{
			case EEffectType.AREA:
				gameObject = mythicAsset.systemArea;
				break;
			case EEffectType.HOOK:
				gameObject = mythicAsset.systemHook;
				break;
			case EEffectType.THIRD:
				gameObject = mythicAsset.systemThird;
				break;
			case EEffectType.FIRST:
				gameObject = mythicAsset.systemFirst;
				break;
			default:
				return null;
			}
			if (gameObject == null)
			{
				return null;
			}
			Transform transform = parent.Find("Effect");
			MythicalEffectController mythicalEffectController = ((transform != null) ? transform : parent).gameObject.AddComponent<MythicalEffectController>();
			mythicalEffectController.systemPrefab = gameObject;
			return mythicalEffectController;
		}

		// Token: 0x06003D8A RID: 15754 RVA: 0x001276D4 File Offset: 0x001258D4
		public static bool tryForceGiveItem(Player player, ushort id, byte amount)
		{
			ItemAsset itemAsset = Assets.find(EAssetType.ITEM, id) as ItemAsset;
			if (itemAsset == null || itemAsset.isPro)
			{
				return false;
			}
			for (int i = 0; i < (int)amount; i++)
			{
				Item item = new Item(id, EItemOrigin.ADMIN);
				player.inventory.forceAddItem(item, true);
			}
			return true;
		}

		// Token: 0x06003D8B RID: 15755 RVA: 0x00127720 File Offset: 0x00125920
		public static bool checkUseable(byte page, ushort id)
		{
			ItemAsset itemAsset = Assets.find(EAssetType.ITEM, id) as ItemAsset;
			return itemAsset != null && itemAsset.canPlayerEquip && itemAsset.slot.canEquipInPage(page);
		}

		/// <summary>
		/// No longer used in vanilla. Kept in case plugins are using it.
		/// </summary>
		// Token: 0x06003D8C RID: 15756 RVA: 0x00127758 File Offset: 0x00125958
		public static Transform getItem(ushort id, ushort skin, byte quality, byte[] state, bool viewmodel, GetStatTrackerValueHandler statTrackerCallback)
		{
			ItemAsset itemAsset = Assets.find(EAssetType.ITEM, id) as ItemAsset;
			return ItemTool.getItem(id, skin, quality, state, viewmodel, itemAsset, statTrackerCallback);
		}

		/// <summary>
		/// No longer used in vanilla. Kept in case plugins are using it.
		/// </summary>
		// Token: 0x06003D8D RID: 15757 RVA: 0x00127780 File Offset: 0x00125980
		public static Transform getItem(ushort id, ushort skin, byte quality, byte[] state, bool viewmodel, ItemAsset itemAsset, List<Mesh> outTempMeshes, out Material tempMaterial, GetStatTrackerValueHandler statTrackerCallback)
		{
			SkinAsset skinAsset = Assets.find(EAssetType.SKIN, skin) as SkinAsset;
			return ItemTool.InstantiateItem(quality, state, viewmodel, itemAsset, skinAsset, false, outTempMeshes, out tempMaterial, statTrackerCallback, null);
		}

		/// <summary>
		/// No longer used in vanilla. Kept in case plugins are using it.
		/// </summary>
		// Token: 0x06003D8E RID: 15758 RVA: 0x001277B0 File Offset: 0x001259B0
		public static Transform getItem(ushort id, ushort skin, byte quality, byte[] state, bool viewmodel, ItemAsset itemAsset, bool shouldDestroyColliders, List<Mesh> outTempMeshes, out Material tempMaterial, GetStatTrackerValueHandler statTrackerCallback)
		{
			SkinAsset skinAsset = Assets.find(EAssetType.SKIN, skin) as SkinAsset;
			return ItemTool.InstantiateItem(quality, state, viewmodel, itemAsset, skinAsset, shouldDestroyColliders, outTempMeshes, out tempMaterial, statTrackerCallback, null);
		}

		// Token: 0x06003D8F RID: 15759 RVA: 0x001277E0 File Offset: 0x001259E0
		public static Transform getItem(ushort id, ushort skin, byte quality, byte[] state, bool viewmodel, ItemAsset itemAsset, GetStatTrackerValueHandler statTrackerCallback)
		{
			SkinAsset skinAsset = Assets.find(EAssetType.SKIN, skin) as SkinAsset;
			Material material;
			return ItemTool.InstantiateItem(quality, state, viewmodel, itemAsset, skinAsset, false, null, out material, statTrackerCallback, null);
		}

		// Token: 0x06003D90 RID: 15760 RVA: 0x00127810 File Offset: 0x00125A10
		public static Transform getItem(ushort id, ushort skin, byte quality, byte[] state, bool viewmodel, ItemAsset itemAsset, bool shouldDestroyColliders, GetStatTrackerValueHandler statTrackerCallback)
		{
			SkinAsset skinAsset = Assets.find(EAssetType.SKIN, skin) as SkinAsset;
			Material material;
			return ItemTool.InstantiateItem(quality, state, viewmodel, itemAsset, skinAsset, shouldDestroyColliders, null, out material, statTrackerCallback, null);
		}

		// Token: 0x06003D91 RID: 15761 RVA: 0x00127840 File Offset: 0x00125A40
		public static Transform getItem(ushort id, ushort skin, byte quality, byte[] state, bool viewmodel, ItemAsset itemAsset, SkinAsset skinAsset, GetStatTrackerValueHandler statTrackerCallback)
		{
			Material material;
			return ItemTool.InstantiateItem(quality, state, viewmodel, itemAsset, skinAsset, false, null, out material, statTrackerCallback, null);
		}

		// Token: 0x06003D92 RID: 15762 RVA: 0x00127864 File Offset: 0x00125A64
		public static Transform getItem(ushort id, ushort skin, byte quality, byte[] state, bool viewmodel, ItemAsset itemAsset, SkinAsset skinAsset, List<Mesh> outTempMeshes, out Material tempMaterial, GetStatTrackerValueHandler statTrackerCallback)
		{
			return ItemTool.InstantiateItem(quality, state, viewmodel, itemAsset, skinAsset, false, outTempMeshes, out tempMaterial, statTrackerCallback, null);
		}

		// Token: 0x06003D93 RID: 15763 RVA: 0x00127888 File Offset: 0x00125A88
		internal static Transform getItem(byte quality, byte[] state, bool viewmodel, ItemAsset itemAsset, SkinAsset skinAsset, List<Mesh> outTempMeshes, out Material tempMaterial, GetStatTrackerValueHandler statTrackerCallback, GameObject prefabOverride = null)
		{
			return ItemTool.InstantiateItem(quality, state, viewmodel, itemAsset, skinAsset, false, outTempMeshes, out tempMaterial, statTrackerCallback, prefabOverride);
		}

		// Token: 0x06003D94 RID: 15764 RVA: 0x001278AC File Offset: 0x00125AAC
		[Obsolete("Removed id and skin parameters because itemAsset and skinAsset are required")]
		internal static Transform InstantiateItem(ushort id, ushort skin, byte quality, byte[] state, bool viewmodel, ItemAsset itemAsset, SkinAsset skinAsset, bool shouldDestroyColliders, List<Mesh> outTempMeshes, out Material tempMaterial, GetStatTrackerValueHandler statTrackerCallback, GameObject prefabOverride = null)
		{
			return ItemTool.InstantiateItem(quality, state, viewmodel, itemAsset, skinAsset, shouldDestroyColliders, outTempMeshes, out tempMaterial, statTrackerCallback, prefabOverride);
		}

		/// <summary>
		/// Actual internal implementation.
		/// </summary>
		// Token: 0x06003D95 RID: 15765 RVA: 0x001278D0 File Offset: 0x00125AD0
		internal static Transform InstantiateItem(byte quality, byte[] state, bool viewmodel, ItemAsset itemAsset, SkinAsset skinAsset, bool shouldDestroyColliders, List<Mesh> outTempMeshes, out Material tempMaterial, GetStatTrackerValueHandler statTrackerCallback, GameObject prefabOverride = null)
		{
			tempMaterial = null;
			GameObject gameObject = prefabOverride;
			if (itemAsset != null && gameObject == null)
			{
				gameObject = itemAsset.item;
			}
			if (gameObject == null)
			{
				Transform transform = new GameObject().transform;
				transform.name = itemAsset.instantiatedItemName;
				if (viewmodel)
				{
					transform.tag = "Viewmodel";
					transform.gameObject.layer = 11;
				}
				else
				{
					transform.tag = "Item";
					transform.gameObject.layer = 13;
				}
				return transform;
			}
			Transform transform2 = Object.Instantiate<GameObject>(gameObject).transform;
			transform2.name = itemAsset.instantiatedItemName;
			if (shouldDestroyColliders && itemAsset.shouldDestroyItemColliders)
			{
				PrefabUtil.DestroyCollidersInChildren(transform2.gameObject, true);
			}
			if (viewmodel)
			{
				Layerer.viewmodel(transform2);
			}
			if (skinAsset != null)
			{
				if (skinAsset.overrideMeshes != null && skinAsset.overrideMeshes.Count > 0)
				{
					HighlighterTool.remesh(transform2, skinAsset.overrideMeshes, outTempMeshes);
				}
				else if (outTempMeshes != null)
				{
					outTempMeshes.Clear();
				}
				if (skinAsset.primarySkin != null)
				{
					if (skinAsset.isPattern)
					{
						Material material = Object.Instantiate<Material>(skinAsset.primarySkin);
						itemAsset.applySkinBaseTextures(material);
						skinAsset.SetMaterialProperties(material);
						HighlighterTool.rematerialize(transform2, material, out tempMaterial);
						transform2.gameObject.AddComponent<DestroyMaterialOnDestroy>().instantiatedMaterial = material;
					}
					else
					{
						HighlighterTool.rematerialize(transform2, skinAsset.primarySkin, out tempMaterial);
					}
				}
			}
			else if (outTempMeshes != null)
			{
				outTempMeshes.Clear();
			}
			if (itemAsset.type == EItemType.GUN)
			{
				Attachments attachments = transform2.gameObject.AddComponent<Attachments>();
				attachments.isSkinned = true;
				attachments.shouldDestroyColliders = shouldDestroyColliders;
				attachments.updateGun((ItemGunAsset)itemAsset, skinAsset);
				attachments.updateAttachments(state, viewmodel);
			}
			return transform2;
		}

		// Token: 0x06003D96 RID: 15766 RVA: 0x00127A68 File Offset: 0x00125C68
		public static Texture2D getCard(Transform item, Transform hook_0, Transform hook_1, int width, int height, float size, float range)
		{
			if (item == null)
			{
				return null;
			}
			item.position = new Vector3(-256f, -256f, 0f);
			RenderTexture temporary = RenderTexture.GetTemporary(width, height, 16, RenderTextureFormat.ARGB32, RenderTextureReadWrite.sRGB);
			temporary.name = "Card_Render";
			RenderTexture.active = temporary;
			ItemTool.tool.cameraComponent.targetTexture = temporary;
			ItemTool.tool.cameraComponent.orthographicSize = size;
			Texture2D texture2D = new Texture2D(width * 2, height, TextureFormat.ARGB32, false, false);
			texture2D.name = "Card_Atlas";
			texture2D.filterMode = FilterMode.Point;
			texture2D.wrapMode = TextureWrapMode.Clamp;
			bool fog = RenderSettings.fog;
			AmbientMode ambientMode = RenderSettings.ambientMode;
			Color ambientSkyColor = RenderSettings.ambientSkyColor;
			Color ambientEquatorColor = RenderSettings.ambientEquatorColor;
			Color ambientGroundColor = RenderSettings.ambientGroundColor;
			RenderSettings.fog = false;
			RenderSettings.ambientMode = AmbientMode.Trilight;
			RenderSettings.ambientSkyColor = Color.white;
			RenderSettings.ambientEquatorColor = Color.white;
			RenderSettings.ambientGroundColor = Color.white;
			if (Provider.isConnected)
			{
				LevelLighting.setEnabled(false);
			}
			ItemTool.tool.cameraComponent.cullingMask = 16384;
			ItemTool.tool.cameraComponent.farClipPlane = range;
			ItemTool.tool.transform.position = hook_0.position;
			ItemTool.tool.transform.rotation = hook_0.rotation;
			ItemTool.tool.cameraComponent.clearFlags = CameraClearFlags.Color;
			ItemTool.tool.cameraComponent.Render();
			texture2D.ReadPixels(new Rect(0f, 0f, (float)width, (float)height), 0, 0);
			ItemTool.tool.transform.position = hook_1.position;
			ItemTool.tool.transform.rotation = hook_1.rotation;
			ItemTool.tool.cameraComponent.Render();
			texture2D.ReadPixels(new Rect(0f, 0f, (float)width, (float)height), width, 0);
			if (Provider.isConnected)
			{
				LevelLighting.setEnabled(true);
			}
			RenderSettings.fog = fog;
			RenderSettings.ambientMode = ambientMode;
			RenderSettings.ambientSkyColor = ambientSkyColor;
			RenderSettings.ambientEquatorColor = ambientEquatorColor;
			RenderSettings.ambientGroundColor = ambientGroundColor;
			item.position = new Vector3(0f, -256f, -256f);
			Object.Destroy(item.gameObject);
			for (int i = 0; i < texture2D.width; i++)
			{
				for (int j = 0; j < texture2D.height; j++)
				{
					Color32 color = texture2D.GetPixel(i, j);
					if (color.r == 255 && color.g == 255 && color.b == 255)
					{
						color.a = 0;
					}
					else
					{
						color.a = byte.MaxValue;
					}
					texture2D.SetPixel(i, j, color);
				}
			}
			texture2D.Apply();
			RenderTexture.ReleaseTemporary(temporary);
			return texture2D;
		}

		// Token: 0x06003D97 RID: 15767 RVA: 0x00127D24 File Offset: 0x00125F24
		public static void getIcon(ushort id, byte quality, byte[] state, ItemIconReady callback)
		{
			ItemAsset itemAsset = Assets.find(EAssetType.ITEM, id) as ItemAsset;
			ItemTool.getIcon(id, quality, state, itemAsset, callback);
		}

		// Token: 0x06003D98 RID: 15768 RVA: 0x00127D48 File Offset: 0x00125F48
		public static void getIcon(ushort id, byte quality, byte[] state, ItemAsset itemAsset, ItemIconReady callback)
		{
			ItemTool.getIcon(id, quality, state, itemAsset, (int)(itemAsset.size_x * 50), (int)(itemAsset.size_y * 50), callback);
		}

		// Token: 0x06003D99 RID: 15769 RVA: 0x00127D68 File Offset: 0x00125F68
		public static void getIcon(ushort id, byte quality, byte[] state, ItemAsset itemAsset, int x, int y, ItemIconReady callback)
		{
			ushort num = 0;
			SkinAsset skinAsset = null;
			string empty = string.Empty;
			string empty2 = string.Empty;
			if (Player.player != null)
			{
				bool flag = itemAsset != null && itemAsset.sharedSkinLookupID != itemAsset.id;
				ushort itemID = flag ? itemAsset.sharedSkinLookupID : id;
				int num2;
				if (Player.player.channel.owner.getItemSkinItemDefID(itemID, out num2) && num2 != 0)
				{
					if (!flag || itemAsset.SharedSkinShouldApplyVisuals)
					{
						num = Provider.provider.economyService.getInventorySkinID(num2);
						skinAsset = (Assets.find(EAssetType.SKIN, num) as SkinAsset);
					}
					Player.player.channel.owner.getTagsAndDynamicPropsForItem(num2, out empty, out empty2);
				}
			}
			ItemTool.getIcon(id, num, quality, state, itemAsset, skinAsset, empty, empty2, x, y, false, false, callback);
		}

		// Token: 0x06003D9A RID: 15770 RVA: 0x00127E38 File Offset: 0x00126038
		public static void getIcon(ushort id, ushort skin, byte quality, byte[] state, ItemAsset itemAsset, SkinAsset skinAsset, string tags, string dynamic_props, int x, int y, bool scale, bool readableOnCPU, ItemIconReady callback)
		{
			if (itemAsset == null)
			{
				itemAsset = (Assets.find(EAssetType.ITEM, id) as ItemAsset);
				if (itemAsset == null)
				{
					UnturnedLog.warn(string.Format("getIcon called with null item, unable to find by legacy id {0}", id));
					return;
				}
				UnturnedLog.warn(string.Format("getIcon called with null item, found \"{0}\" by legacy id {1}", itemAsset.name, id));
			}
			bool flag = state.Length == 0 && skinAsset == null && x == (int)(itemAsset.size_x * 50) && y == (int)(itemAsset.size_y * 50) && !readableOnCPU;
			if (flag)
			{
				Texture2D texture2D;
				if (ItemTool.iconCache.TryGetValue(itemAsset, ref texture2D))
				{
					if (texture2D != null)
					{
						callback(texture2D);
						return;
					}
					ItemTool.iconCache.Remove(itemAsset);
				}
				foreach (ItemIconInfo itemIconInfo in ItemTool.icons)
				{
					if (itemIconInfo.isEligibleForCaching && itemIconInfo.itemAsset == itemAsset)
					{
						ItemIconInfo itemIconInfo2 = itemIconInfo;
						itemIconInfo2.callback = (ItemIconReady)Delegate.Combine(itemIconInfo2.callback, callback);
						return;
					}
				}
			}
			ItemIconInfo itemIconInfo3 = new ItemIconInfo();
			itemIconInfo3.id = itemAsset.id;
			itemIconInfo3.skin = ((skinAsset != null) ? skinAsset.id : 0);
			itemIconInfo3.quality = quality;
			itemIconInfo3.state = state;
			itemIconInfo3.itemAsset = itemAsset;
			itemIconInfo3.skinAsset = skinAsset;
			itemIconInfo3.tags = tags;
			itemIconInfo3.dynamic_props = dynamic_props;
			itemIconInfo3.x = x;
			itemIconInfo3.y = y;
			itemIconInfo3.scale = scale;
			itemIconInfo3.readableOnCPU = readableOnCPU;
			itemIconInfo3.isEligibleForCaching = flag;
			itemIconInfo3.callback = callback;
			ItemTool.icons.Enqueue(itemIconInfo3);
		}

		// Token: 0x06003D9B RID: 15771 RVA: 0x00127FF4 File Offset: 0x001261F4
		public static Texture2D captureIcon(ushort id, ushort skin, Transform model, Transform icon, int width, int height, float orthoSize, bool readableOnCPU)
		{
			ItemTool.tool.transform.position = icon.position;
			ItemTool.tool.transform.rotation = icon.rotation;
			int antiAliasing = GraphicsSettings.IsItemIconAntiAliasingEnabled ? 4 : 1;
			RenderTexture temporary = RenderTexture.GetTemporary(width, height, 16, RenderTextureFormat.ARGB32, RenderTextureReadWrite.sRGB, antiAliasing);
			temporary.name = "Render_" + id.ToString() + "_" + skin.ToString();
			RenderTexture.active = temporary;
			ItemTool.tool.cameraComponent.targetTexture = temporary;
			ItemTool.tool.cameraComponent.orthographicSize = orthoSize;
			bool fog = RenderSettings.fog;
			AmbientMode ambientMode = RenderSettings.ambientMode;
			Color ambientSkyColor = RenderSettings.ambientSkyColor;
			Color ambientEquatorColor = RenderSettings.ambientEquatorColor;
			Color ambientGroundColor = RenderSettings.ambientGroundColor;
			RenderSettings.fog = false;
			RenderSettings.ambientMode = AmbientMode.Trilight;
			RenderSettings.ambientSkyColor = Color.white;
			RenderSettings.ambientEquatorColor = Color.white;
			RenderSettings.ambientGroundColor = Color.white;
			if (Provider.isConnected)
			{
				LevelLighting.setEnabled(false);
			}
			ItemTool.tool.lightComponent.enabled = true;
			GL.Clear(true, true, ColorEx.BlackZeroAlpha);
			ItemTool.tool.cameraComponent.cullingMask = 67313664;
			ItemTool.tool.cameraComponent.farClipPlane = 16f;
			ItemTool.tool.cameraComponent.clearFlags = CameraClearFlags.Nothing;
			ItemTool.tool.cameraComponent.Render();
			ItemTool.tool.lightComponent.enabled = false;
			if (Provider.isConnected)
			{
				LevelLighting.setEnabled(true);
			}
			RenderSettings.fog = fog;
			RenderSettings.ambientMode = ambientMode;
			RenderSettings.ambientSkyColor = ambientSkyColor;
			RenderSettings.ambientEquatorColor = ambientEquatorColor;
			RenderSettings.ambientGroundColor = ambientGroundColor;
			model.position = new Vector3(0f, -256f, -256f);
			Object.Destroy(model.gameObject);
			Texture2D texture2D = new Texture2D(width, height, TextureFormat.ARGB32, false);
			texture2D.name = "Icon_" + id.ToString() + "_" + skin.ToString();
			texture2D.filterMode = FilterMode.Point;
			texture2D.ReadPixels(new Rect(0f, 0f, (float)width, (float)height), 0, 0);
			texture2D.Apply(false, !readableOnCPU);
			RenderTexture.ReleaseTemporary(temporary);
			return texture2D;
		}

		// Token: 0x06003D9C RID: 15772 RVA: 0x00128210 File Offset: 0x00126410
		private bool getIconStatTrackerValue(out EStatTrackerType type, out int kills)
		{
			DynamicEconDetails dynamicEconDetails = new DynamicEconDetails(this.currentIconTags, this.currentIconDynamicProps);
			return dynamicEconDetails.getStatTrackerValue(out type, out kills);
		}

		/// <summary>
		/// World to local bounds only works well for axis-aligned icons.
		/// </summary>
		// Token: 0x06003D9D RID: 15773 RVA: 0x0012823C File Offset: 0x0012643C
		private bool IsTransformAxisAligned(Transform cameraTransform)
		{
			Vector3 eulerAngles = cameraTransform.localRotation.eulerAngles;
			eulerAngles.x = Mathf.Abs(eulerAngles.x) % 90f;
			eulerAngles.y = Mathf.Abs(eulerAngles.y) % 90f;
			eulerAngles.z = Mathf.Abs(eulerAngles.z) % 90f;
			return (eulerAngles.x < 1f || eulerAngles.x > 89f) && (eulerAngles.y < 1f || eulerAngles.y > 89f) && (eulerAngles.z < 1f || eulerAngles.z > 89f);
		}

		/// <summary>
		/// Unity's Camera.orthographicSize is half the height of the viewing volume. Width is calculated from aspect ratio.
		/// </summary>
		// Token: 0x06003D9E RID: 15774 RVA: 0x001282F4 File Offset: 0x001264F4
		private float CalculateOrthographicSize(ItemAsset assetContext, GameObject modelGameObject, Transform cameraTransform, int renderWidth, int renderHeight)
		{
			this.renderers.Clear();
			modelGameObject.GetComponentsInChildren<Renderer>(false, this.renderers);
			Bounds bounds = default(Bounds);
			bool flag = false;
			foreach (Renderer renderer in this.renderers)
			{
				if (renderer is MeshRenderer || renderer is SkinnedMeshRenderer)
				{
					if (flag)
					{
						bounds.Encapsulate(renderer.bounds);
					}
					else
					{
						flag = true;
						bounds = renderer.bounds;
					}
				}
			}
			if (!flag)
			{
				return 1f;
			}
			Vector3 extents = bounds.extents;
			if (extents.ContainsInfinity() || extents.ContainsNaN() || extents.IsNearlyZero(0.001f))
			{
				Assets.reportError(assetContext, "has invalid icon world extent {0}", extents);
				return 1f;
			}
			Bounds bounds2 = new Bounds(cameraTransform.InverseTransformVector(extents), Vector3.zero);
			bounds2.Encapsulate(cameraTransform.InverseTransformVector(-extents));
			bounds2.Encapsulate(cameraTransform.InverseTransformVector(new Vector3(-extents.x, extents.y, extents.z)));
			bounds2.Encapsulate(cameraTransform.InverseTransformVector(new Vector3(extents.x, -extents.y, extents.z)));
			bounds2.Encapsulate(cameraTransform.InverseTransformVector(new Vector3(extents.x, extents.y, -extents.z)));
			bounds2.Encapsulate(cameraTransform.InverseTransformVector(new Vector3(-extents.x, -extents.y, extents.z)));
			bounds2.Encapsulate(cameraTransform.InverseTransformVector(new Vector3(-extents.x, extents.y, -extents.z)));
			bounds2.Encapsulate(cameraTransform.InverseTransformVector(new Vector3(extents.x, -extents.y, -extents.z)));
			Vector3 extents2 = bounds2.extents;
			if (extents2.ContainsInfinity() || extents2.ContainsNaN() || extents2.IsNearlyZero(0.001f))
			{
				Assets.reportError(assetContext, "has invalid icon local extent {0} Maybe camera scale {1} is wrong?", extents2, cameraTransform.localScale);
				return 1f;
			}
			float num = Mathf.Abs(extents2.x);
			float num2 = Mathf.Abs(extents2.y);
			float num3 = Mathf.Abs(extents2.z);
			float nearClipPlane = this.cameraComponent.nearClipPlane;
			cameraTransform.position = bounds.center - cameraTransform.forward * (num3 + 0.02f + nearClipPlane);
			if (assetContext.iconCameraOrthographicSize > 0f && !this.IsTransformAxisAligned(cameraTransform))
			{
				return assetContext.iconCameraOrthographicSize;
			}
			num *= (float)(renderWidth + 16) / (float)renderWidth;
			num2 *= (float)(renderHeight + 16) / (float)renderHeight;
			float num4 = (float)renderWidth / (float)renderHeight;
			float num5 = num / num2;
			float num6 = (num5 > num4) ? (num5 / num4) : 1f;
			return num2 * num6;
		}

		// Token: 0x06003D9F RID: 15775 RVA: 0x001285F0 File Offset: 0x001267F0
		private void Update()
		{
			if (!(this.pendingItem == null))
			{
				ItemIconInfo itemIconInfo = this.pendingInfo;
				Transform transform = this.pendingItem;
				this.pendingInfo = null;
				this.pendingItem = null;
				Transform transform2;
				if (itemIconInfo.scale && itemIconInfo.skinAsset != null)
				{
					transform2 = transform.Find("Icon2");
					if (transform2 == null)
					{
						transform.position = new Vector3(0f, -256f, -256f);
						Object.Destroy(transform.gameObject);
						Assets.reportError(itemIconInfo.itemAsset, "missing 'Icon2' Transform");
						return;
					}
				}
				else
				{
					transform2 = transform.Find("Icon");
					if (transform2 == null)
					{
						transform.position = new Vector3(0f, -256f, -256f);
						Object.Destroy(transform.gameObject);
						Assets.reportError(itemIconInfo.itemAsset, "missing 'Icon' Transform");
						return;
					}
				}
				float orthoSize;
				if (itemIconInfo.scale && itemIconInfo.skinAsset != null)
				{
					orthoSize = itemIconInfo.itemAsset.econIconCameraOrthographicSize;
				}
				else if (itemIconInfo.itemAsset.isEligibleForAutomaticIconMeasurements)
				{
					orthoSize = this.CalculateOrthographicSize(itemIconInfo.itemAsset, transform.gameObject, transform2, itemIconInfo.x, itemIconInfo.y);
				}
				else
				{
					orthoSize = itemIconInfo.itemAsset.iconCameraOrthographicSize;
				}
				ushort id = itemIconInfo.itemAsset.id;
				SkinAsset skinAsset = itemIconInfo.skinAsset;
				Texture2D texture2D = ItemTool.captureIcon(id, (skinAsset != null) ? skinAsset.id : 0, transform, transform2, itemIconInfo.x, itemIconInfo.y, orthoSize, itemIconInfo.readableOnCPU);
				if (itemIconInfo.callback != null)
				{
					try
					{
						itemIconInfo.callback(texture2D);
					}
					catch (Exception e)
					{
						UnturnedLog.exception(e, "Caught exception during item icon capture callback:");
					}
				}
				if (itemIconInfo.isEligibleForCaching && !ItemTool.iconCache.ContainsKey(itemIconInfo.itemAsset))
				{
					ItemTool.iconCache.Add(itemIconInfo.itemAsset, texture2D);
				}
				return;
			}
			if (ItemTool.icons == null || ItemTool.icons.Count == 0)
			{
				return;
			}
			this.pendingInfo = ItemTool.icons.Dequeue();
			if (this.pendingInfo == null)
			{
				return;
			}
			if (this.pendingInfo.itemAsset == null)
			{
				return;
			}
			this.currentIconTags = this.pendingInfo.tags;
			this.currentIconDynamicProps = this.pendingInfo.dynamic_props;
			ushort id2 = this.pendingInfo.itemAsset.id;
			SkinAsset skinAsset2 = this.pendingInfo.skinAsset;
			this.pendingItem = ItemTool.getItem(id2, (skinAsset2 != null) ? skinAsset2.id : 0, this.pendingInfo.quality, this.pendingInfo.state, false, this.pendingInfo.itemAsset, this.pendingInfo.skinAsset, new GetStatTrackerValueHandler(this.getIconStatTrackerValue));
			this.pendingItem.position = new Vector3(-256f, -256f, 0f);
		}

		// Token: 0x06003DA0 RID: 15776 RVA: 0x001288B0 File Offset: 0x00126AB0
		private void Start()
		{
			ItemTool.tool = this;
			this.cameraComponent = base.GetComponent<Camera>();
			this.lightComponent = base.GetComponent<Light>();
			ItemTool.icons = new Queue<ItemIconInfo>();
		}

		// Token: 0x040026C5 RID: 9925
		private static readonly Color RARITY_COMMON_HIGHLIGHT = Color.white;

		// Token: 0x040026C6 RID: 9926
		private static readonly Color RARITY_COMMON_UI = Color.white;

		// Token: 0x040026C7 RID: 9927
		private static readonly Color RARITY_UNCOMMON_HIGHLIGHT = Color.green;

		// Token: 0x040026C8 RID: 9928
		private static readonly Color RARITY_UNCOMMON_UI = new Color(0.12156863f, 0.5294118f, 0.12156863f);

		// Token: 0x040026C9 RID: 9929
		private static readonly Color RARITY_RARE_HIGHLIGHT = Color.blue;

		// Token: 0x040026CA RID: 9930
		private static readonly Color RARITY_RARE_UI = new Color(0.29411766f, 0.39215687f, 0.98039216f);

		// Token: 0x040026CB RID: 9931
		private static readonly Color RARITY_EPIC_HIGHLIGHT = new Color(0.33f, 0f, 1f);

		// Token: 0x040026CC RID: 9932
		private static readonly Color RARITY_EPIC_UI = new Color(0.5882353f, 0.29411766f, 0.98039216f);

		// Token: 0x040026CD RID: 9933
		private static readonly Color RARITY_LEGENDARY_HIGHLIGHT = Color.magenta;

		// Token: 0x040026CE RID: 9934
		private static readonly Color RARITY_LEGENDARY_UI = new Color(0.78431374f, 0.19607843f, 0.98039216f);

		// Token: 0x040026CF RID: 9935
		private static readonly Color RARITY_MYTHICAL_HIGHLIGHT = Color.red;

		// Token: 0x040026D0 RID: 9936
		private static readonly Color RARITY_MYTHICAL_UI = new Color(0.98039216f, 0.19607843f, 0.09803922f);

		// Token: 0x040026D1 RID: 9937
		private static Queue<ItemIconInfo> icons;

		// Token: 0x040026D2 RID: 9938
		private string currentIconTags;

		// Token: 0x040026D3 RID: 9939
		private string currentIconDynamicProps;

		// Token: 0x040026D4 RID: 9940
		private List<Renderer> renderers = new List<Renderer>();

		// Token: 0x040026D5 RID: 9941
		private Camera cameraComponent;

		// Token: 0x040026D6 RID: 9942
		private Light lightComponent;

		// Token: 0x040026D7 RID: 9943
		private Transform pendingItem;

		// Token: 0x040026D8 RID: 9944
		private ItemIconInfo pendingInfo;

		// Token: 0x040026D9 RID: 9945
		private static ItemTool tool;

		// Token: 0x040026DA RID: 9946
		private static Dictionary<ItemAsset, Texture2D> iconCache = new Dictionary<ItemAsset, Texture2D>();
	}
}
