using System;
using System.Collections;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000616 RID: 1558
	public class PlayerDelayedQuestRewardsComponent : MonoBehaviour
	{
		// Token: 0x06003207 RID: 12807 RVA: 0x000DEAF2 File Offset: 0x000DCCF2
		internal void GrantReward(INPCReward reward)
		{
			base.StartCoroutine(this.GrantRewardCoroutine(reward));
		}

		// Token: 0x06003208 RID: 12808 RVA: 0x000DEB02 File Offset: 0x000DCD02
		private IEnumerator GrantRewardCoroutine(INPCReward reward)
		{
			yield return new WaitForSeconds(reward.grantDelaySeconds);
			try
			{
				reward.GrantReward(this.player);
				yield break;
			}
			catch (Exception e)
			{
				string text = "Caught exception granting delayed NPC reward to {0}:";
				Player player = this.player;
				object obj;
				if (player == null)
				{
					obj = null;
				}
				else
				{
					SteamChannel channel = player.channel;
					if (channel == null)
					{
						obj = null;
					}
					else
					{
						SteamPlayer owner = channel.owner;
						obj = ((owner != null) ? owner.playerID : null);
					}
				}
				UnturnedLog.exception(e, string.Format(text, obj));
				yield break;
			}
			yield break;
		}

		// Token: 0x04001C6D RID: 7277
		public Player player;
	}
}
