using System;
using SDG.Framework.Devkit;

namespace SDG.Unturned
{
	// Token: 0x02000349 RID: 841
	public class NPCTeleportReward : INPCReward
	{
		// Token: 0x170004C2 RID: 1218
		// (get) Token: 0x06001935 RID: 6453 RVA: 0x0005A97C File Offset: 0x00058B7C
		// (set) Token: 0x06001936 RID: 6454 RVA: 0x0005A984 File Offset: 0x00058B84
		public string spawnpoint { get; protected set; }

		// Token: 0x06001937 RID: 6455 RVA: 0x0005A990 File Offset: 0x00058B90
		public override void GrantReward(Player player)
		{
			Spawnpoint spawnpoint = SpawnpointSystemV2.Get().FindSpawnpoint(this.spawnpoint);
			if (spawnpoint == null)
			{
				UnturnedLog.error("Failed to find NPC teleport reward spawnpoint: " + this.spawnpoint);
				return;
			}
			if (!player.teleportToLocation(spawnpoint.transform.position, spawnpoint.transform.rotation.eulerAngles.y))
			{
				UnturnedLog.error("Unable to reward NPC teleport because {0} was obstructed.", new object[]
				{
					this.spawnpoint
				});
			}
		}

		// Token: 0x06001938 RID: 6456 RVA: 0x0005AA11 File Offset: 0x00058C11
		public override string ToString()
		{
			if (this.grantDelaySeconds > 0f)
			{
				return string.Format("teleport to \"{0}\" after {1} s", this.spawnpoint, this.grantDelaySeconds);
			}
			return "teleport to \"" + this.spawnpoint + "\"";
		}

		// Token: 0x06001939 RID: 6457 RVA: 0x0005AA51 File Offset: 0x00058C51
		public NPCTeleportReward(string newSpawnpoint, string newText) : base(newText)
		{
			this.spawnpoint = newSpawnpoint;
		}
	}
}
