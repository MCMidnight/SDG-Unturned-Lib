using System;
using System.Text;

namespace SDG.Unturned
{
	// Token: 0x02000345 RID: 837
	public struct NPCRewardsList
	{
		// Token: 0x0600191E RID: 6430 RVA: 0x0005A608 File Offset: 0x00058808
		public void Grant(Player player)
		{
			if (this.rewards != null && this.rewards.Length != 0)
			{
				try
				{
					foreach (INPCReward inpcreward in this.rewards)
					{
						if (inpcreward.grantDelaySeconds > 0f)
						{
							player.quests.GetOrCreateDelayedQuestRewards().GrantReward(inpcreward);
						}
						else
						{
							inpcreward.GrantReward(player);
						}
					}
				}
				catch (Exception e)
				{
					string text = "Caught exception granting NPC reward to {0}:";
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
				}
			}
		}

		// Token: 0x0600191F RID: 6431 RVA: 0x0005A6B0 File Offset: 0x000588B0
		public void Parse(DatDictionary data, Local localization, Asset assetContext, string countKey, string prefixKey)
		{
			int num = data.ParseInt32(countKey, 0);
			if (num > 0)
			{
				this.rewards = new INPCReward[num];
				NPCTool.readRewards(data, localization, prefixKey, this.rewards, assetContext);
			}
		}

		// Token: 0x06001920 RID: 6432 RVA: 0x0005A6E8 File Offset: 0x000588E8
		public void DebugDumpToStringBuilder(StringBuilder output)
		{
			string text = "{0} reward(s)";
			INPCReward[] array = this.rewards;
			output.AppendLine(string.Format(text, (array != null) ? new int?(array.Length) : default(int?)));
			if (this.rewards != null)
			{
				for (int i = 0; i < this.rewards.Length; i++)
				{
					output.AppendLine(string.Format("[{0}]: {1}", i, this.rewards[i]));
				}
			}
		}

		// Token: 0x06001921 RID: 6433 RVA: 0x0005A764 File Offset: 0x00058964
		public string DebugDumpToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			this.DebugDumpToStringBuilder(stringBuilder);
			return stringBuilder.ToString();
		}

		// Token: 0x06001922 RID: 6434 RVA: 0x0005A784 File Offset: 0x00058984
		[Obsolete("Removed shouldSend parameter")]
		public void Grant(Player player, bool shouldSend = true)
		{
			this.Grant(player);
		}

		// Token: 0x04000B3A RID: 2874
		internal INPCReward[] rewards;
	}
}
