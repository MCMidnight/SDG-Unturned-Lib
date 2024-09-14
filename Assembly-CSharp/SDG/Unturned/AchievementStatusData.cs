using System;

namespace SDG.Unturned
{
	// Token: 0x020006EE RID: 1774
	public class AchievementStatusData
	{
		// Token: 0x06003B10 RID: 15120 RVA: 0x001145F0 File Offset: 0x001127F0
		public bool canBeGrantedByNPC(string id)
		{
			string[] npc_Achievement_IDs = this.NPC_Achievement_IDs;
			for (int i = 0; i < npc_Achievement_IDs.Length; i++)
			{
				if (string.Equals(npc_Achievement_IDs[i], id))
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Names of achievements that can be granted by NPC rewards.
		/// </summary>
		// Token: 0x040024E9 RID: 9449
		public string[] NPC_Achievement_IDs;
	}
}
