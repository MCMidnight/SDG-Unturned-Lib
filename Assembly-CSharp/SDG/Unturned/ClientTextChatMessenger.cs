using System;
using UnityEngine;

namespace SDG.Unturned
{
	/// <summary>
	/// Allows Unity events to send text chat messages from the client, for example to execute commands.
	/// </summary>
	// Token: 0x020005C2 RID: 1474
	[AddComponentMenu("Unturned/Client Text Chat Messenger")]
	public class ClientTextChatMessenger : MonoBehaviour
	{
		// Token: 0x06002FD3 RID: 12243 RVA: 0x000D382C File Offset: 0x000D1A2C
		private EChatMode getChatMode()
		{
			ClientTextChatMessenger.EChannel channel = this.Channel;
			if (channel == ClientTextChatMessenger.EChannel.Global || channel != ClientTextChatMessenger.EChannel.Local)
			{
				return EChatMode.GLOBAL;
			}
			return EChatMode.LOCAL;
		}

		// Token: 0x06002FD4 RID: 12244 RVA: 0x000D384A File Offset: 0x000D1A4A
		public void SendTextChatMessage(string text)
		{
		}

		// Token: 0x06002FD5 RID: 12245 RVA: 0x000D384C File Offset: 0x000D1A4C
		public void SendDefaultTextChatMessage()
		{
			this.SendTextChatMessage(this.DefaultText);
		}

		/// <summary>
		/// Text to use when SendDefaultTextChatMessage is invoked.
		/// </summary>
		// Token: 0x040019D0 RID: 6608
		public string DefaultText;

		/// <summary>
		/// Chat mode to send request in.
		/// </summary>
		// Token: 0x040019D1 RID: 6609
		public ClientTextChatMessenger.EChannel Channel;

		// Token: 0x02000996 RID: 2454
		public enum EChannel
		{
			/// <summary>
			/// All players on the server will see the message.
			/// </summary>
			// Token: 0x040033BF RID: 13247
			Global,
			/// <summary>
			/// Only nearby players will see the message.
			/// </summary>
			// Token: 0x040033C0 RID: 13248
			Local
		}
	}
}
