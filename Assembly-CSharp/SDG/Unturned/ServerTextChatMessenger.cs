using System;
using UnityEngine;

namespace SDG.Unturned
{
	/// <summary>
	/// Allows Unity events to broadcast text chat messages from the server.
	/// </summary>
	// Token: 0x020005D4 RID: 1492
	[AddComponentMenu("Unturned/Server Text Chat Messenger")]
	public class ServerTextChatMessenger : MonoBehaviour
	{
		// Token: 0x0600300D RID: 12301 RVA: 0x000D4334 File Offset: 0x000D2534
		public void SendTextChatMessage(string text)
		{
			if (Provider.isServer)
			{
				ChatManager.serverSendMessage_UnityEvent(text, this.DefaultColor, this.IconURL, this.UseRichTextFormatting, this);
			}
		}

		// Token: 0x0600300E RID: 12302 RVA: 0x000D4356 File Offset: 0x000D2556
		public void SendDefaultTextChatMessage()
		{
			this.SendTextChatMessage(this.DefaultText);
		}

		// Token: 0x0600300F RID: 12303 RVA: 0x000D4364 File Offset: 0x000D2564
		public void ExecuteTextChatCommand(string command)
		{
			Commander.execute_UnityEvent(command, this);
		}

		// Token: 0x06003010 RID: 12304 RVA: 0x000D436D File Offset: 0x000D256D
		public void ExecuteDefaultTextChatCommand()
		{
			this.ExecuteTextChatCommand(this.DefaultText);
		}

		/// <summary>
		/// Text to use when SendDefaultTextChatMessage is invoked.
		/// </summary>
		// Token: 0x04001A09 RID: 6665
		public string DefaultText;

		/// <summary>
		/// URL of a png or jpg image file to show next to the message.
		/// </summary>
		// Token: 0x04001A0A RID: 6666
		public string IconURL;

		/// <summary>
		/// Text color when rich text does not override with color tags.
		/// </summary>
		// Token: 0x04001A0B RID: 6667
		public Color DefaultColor = Color.white;

		/// <summary>
		/// Should rich text tags be parsed?
		/// e.g. bold, italic, color
		/// </summary>
		// Token: 0x04001A0C RID: 6668
		public bool UseRichTextFormatting;
	}
}
