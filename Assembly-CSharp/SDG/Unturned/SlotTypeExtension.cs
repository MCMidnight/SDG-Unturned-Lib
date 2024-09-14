using System;

namespace SDG.Unturned
{
	// Token: 0x02000496 RID: 1174
	public static class SlotTypeExtension
	{
		// Token: 0x0600248D RID: 9357 RVA: 0x00091DD5 File Offset: 0x0008FFD5
		public static bool canEquipAsPrimary(this ESlotType slotType)
		{
			return slotType == ESlotType.PRIMARY || slotType == ESlotType.SECONDARY || slotType == ESlotType.ANY;
		}

		// Token: 0x0600248E RID: 9358 RVA: 0x00091DE5 File Offset: 0x0008FFE5
		public static bool canEquipAsSecondary(this ESlotType slotType)
		{
			return slotType == ESlotType.SECONDARY || slotType == ESlotType.ANY;
		}

		// Token: 0x0600248F RID: 9359 RVA: 0x00091DF1 File Offset: 0x0008FFF1
		public static bool canEquipFromBag(this ESlotType slotType)
		{
			return slotType != ESlotType.PRIMARY && slotType != ESlotType.SECONDARY;
		}

		// Token: 0x06002490 RID: 9360 RVA: 0x00091E00 File Offset: 0x00090000
		public static bool canEquipInPage(this ESlotType slotType, byte page)
		{
			if (page == 0)
			{
				return slotType.canEquipAsPrimary();
			}
			if (page != 1)
			{
				return slotType.canEquipFromBag();
			}
			return slotType.canEquipAsSecondary();
		}
	}
}
