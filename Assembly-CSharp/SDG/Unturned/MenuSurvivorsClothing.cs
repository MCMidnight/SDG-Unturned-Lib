using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020005BD RID: 1469
	public class MenuSurvivorsClothing : MonoBehaviour
	{
		// Token: 0x06002FB0 RID: 12208 RVA: 0x000D2738 File Offset: 0x000D0938
		private void onClickedMouse()
		{
			RaycastHit raycastHit;
			Physics.Raycast(MainCamera.instance.ScreenPointToRay(Input.mousePosition), out raycastHit, 64f, RayMasks.CLOTHING_INTERACT);
			if (raycastHit.collider == null)
			{
				return;
			}
			Transform transform = raycastHit.collider.transform;
			if (transform.CompareTag("Player"))
			{
				ELimb limb = DamageTool.getLimb(transform);
				if (limb == ELimb.LEFT_FOOT || limb == ELimb.LEFT_LEG || limb == ELimb.RIGHT_FOOT || limb == ELimb.RIGHT_LEG)
				{
					if (Characters.active.packagePants != 0UL)
					{
						Characters.ToggleEquipItemByInstanceId(Characters.active.packagePants);
					}
				}
				else if ((limb == ELimb.LEFT_HAND || limb == ELimb.LEFT_ARM || limb == ELimb.RIGHT_HAND || limb == ELimb.RIGHT_ARM || limb == ELimb.SPINE) && Characters.active.packageShirt != 0UL)
				{
					Characters.ToggleEquipItemByInstanceId(Characters.active.packageShirt);
				}
			}
			else if (transform.CompareTag("Enemy"))
			{
				if (transform.name == "Hat")
				{
					if (Characters.active.packageHat != 0UL)
					{
						Characters.ToggleEquipItemByInstanceId(Characters.active.packageHat);
					}
				}
				else if (transform.name == "Glasses")
				{
					if (Characters.active.packageGlasses != 0UL)
					{
						Characters.ToggleEquipItemByInstanceId(Characters.active.packageGlasses);
					}
				}
				else if (transform.name == "Mask")
				{
					if (Characters.active.packageMask != 0UL)
					{
						Characters.ToggleEquipItemByInstanceId(Characters.active.packageMask);
					}
				}
				else if (transform.name == "Vest")
				{
					if (Characters.active.packageVest != 0UL)
					{
						Characters.ToggleEquipItemByInstanceId(Characters.active.packageVest);
					}
				}
				else if (transform.name == "Backpack" && Characters.active.packageBackpack != 0UL)
				{
					Characters.ToggleEquipItemByInstanceId(Characters.active.packageBackpack);
				}
			}
			if (MenuSurvivorsClothingItemUI.active)
			{
				MenuSurvivorsClothingItemUI.viewItem();
			}
		}

		// Token: 0x06002FB1 RID: 12209 RVA: 0x000D2915 File Offset: 0x000D0B15
		private void Update()
		{
			if (!MenuSurvivorsClothingUI.active && !MenuSurvivorsClothingItemUI.active)
			{
				return;
			}
			if (Input.GetMouseButtonUp(0) && Glazier.Get().ShouldGameProcessInput)
			{
				this.onClickedMouse();
			}
		}
	}
}
