using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200076B RID: 1899
	public class StructureTool : MonoBehaviour
	{
		// Token: 0x06003E14 RID: 15892 RVA: 0x0012EB1C File Offset: 0x0012CD1C
		public static Transform getStructure(ushort id, byte hp)
		{
			ItemStructureAsset asset = Assets.find(EAssetType.ITEM, id) as ItemStructureAsset;
			return StructureTool.getStructure(id, hp, 0UL, 0UL, asset);
		}

		// Token: 0x06003E15 RID: 15893 RVA: 0x0012EB42 File Offset: 0x0012CD42
		private static Transform getEmptyStructure(ushort id)
		{
			Transform transform = new GameObject().transform;
			transform.name = id.ToString();
			transform.tag = "Structure";
			transform.gameObject.layer = 28;
			return transform;
		}

		// Token: 0x06003E16 RID: 15894 RVA: 0x0012EB74 File Offset: 0x0012CD74
		public static Transform getStructure(ushort id, byte hp, ulong owner, ulong group, ItemStructureAsset asset)
		{
			if (asset != null)
			{
				Transform transform;
				if (asset.structure != null)
				{
					transform = Object.Instantiate<GameObject>(asset.structure).transform;
				}
				else
				{
					transform = null;
				}
				if (transform == null)
				{
					transform = StructureTool.getEmptyStructure(id);
				}
				transform.name = id.ToString();
				if (Provider.isServer && asset.nav != null)
				{
					Transform transform2 = Object.Instantiate<GameObject>(asset.nav).transform;
					transform2.name = "Nav";
					transform2.parent = transform;
					transform2.localPosition = Vector3.zero;
					transform2.localRotation = Quaternion.identity;
				}
				if (!asset.isUnpickupable)
				{
					Interactable2HP interactable2HP = transform.gameObject.AddComponent<Interactable2HP>();
					interactable2HP.hp = hp;
					Interactable2SalvageStructure interactable2SalvageStructure = transform.gameObject.AddComponent<Interactable2SalvageStructure>();
					interactable2SalvageStructure.hp = interactable2HP;
					interactable2SalvageStructure.owner = owner;
					interactable2SalvageStructure.group = group;
					interactable2SalvageStructure.salvageDurationMultiplier = asset.salvageDurationMultiplier;
				}
				return transform;
			}
			return StructureTool.getEmptyStructure(id);
		}
	}
}
