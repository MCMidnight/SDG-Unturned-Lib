using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000546 RID: 1350
	// (Invoke) Token: 0x06002ADA RID: 10970
	public delegate void ServerSendingChatMessageHandler(ref string text, ref Color color, SteamPlayer fromPlayer, SteamPlayer toPlayer, EChatMode mode, ref string iconURL, ref bool useRichTextFormatting);
}
