using System;

namespace SDG.Unturned
{
	/// <summary>
	/// Nelson 2024-02-06: when looking into resolving public issue #3703 I figured since there is a common behavior
	/// between InteractableObjectQuest, InteractableObjectNote, and InteractableObjectDropper (in that they all
	/// request the server to do X we may as well support a "mod hook" that works with all three.
	/// </summary>
	// Token: 0x02000464 RID: 1124
	public abstract class InteractableObjectTriggerableBase : InteractableObject
	{
		// Token: 0x14000085 RID: 133
		// (add) Token: 0x0600222E RID: 8750 RVA: 0x000849D0 File Offset: 0x00082BD0
		// (remove) Token: 0x0600222F RID: 8751 RVA: 0x00084A08 File Offset: 0x00082C08
		internal event Action OnUsedForModHooks;

		// Token: 0x06002230 RID: 8752 RVA: 0x00084A3D File Offset: 0x00082C3D
		internal void InvokeUsedEventForModHooks()
		{
			this.OnUsedForModHooks.TryInvoke("OnUsedForModHooks");
		}
	}
}
