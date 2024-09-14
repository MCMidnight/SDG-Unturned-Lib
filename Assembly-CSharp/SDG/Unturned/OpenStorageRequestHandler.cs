using System;
using Steamworks;

namespace SDG.Unturned
{
	// Token: 0x0200053C RID: 1340
	// (Invoke) Token: 0x060029F1 RID: 10737
	public delegate void OpenStorageRequestHandler(CSteamID instigator, InteractableStorage storage, ref bool shouldAllow);
}
