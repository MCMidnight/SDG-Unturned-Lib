using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000450 RID: 1104
	public class InteractableDoorHinge : Interactable
	{
		// Token: 0x0600213C RID: 8508 RVA: 0x0008055C File Offset: 0x0007E75C
		public override bool checkUseable()
		{
			return this.door.checkToggle(Provider.client, Player.player.quests.groupID);
		}

		// Token: 0x0600213D RID: 8509 RVA: 0x0008057D File Offset: 0x0007E77D
		public override void use()
		{
			this.door.ClientToggle();
		}

		// Token: 0x0600213E RID: 8510 RVA: 0x0008058A File Offset: 0x0007E78A
		public override bool checkHint(out EPlayerMessage message, out string text, out Color color)
		{
			if (this.checkUseable())
			{
				if (this.door.isOpen)
				{
					message = EPlayerMessage.DOOR_CLOSE;
				}
				else
				{
					message = EPlayerMessage.DOOR_OPEN;
				}
			}
			else
			{
				message = EPlayerMessage.LOCKED;
			}
			text = "";
			color = Color.white;
			return true;
		}

		// Token: 0x04001057 RID: 4183
		public InteractableDoor door;
	}
}
