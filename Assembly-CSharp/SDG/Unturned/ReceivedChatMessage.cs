using System;
using UnityEngine;

namespace SDG.Unturned
{
	/// <summary>
	/// Created when a chat entry is received from the server for display in the UI.
	/// </summary>
	// Token: 0x02000589 RID: 1417
	public struct ReceivedChatMessage
	{
		/// <summary>
		/// How many seconds ago this message was locally received from the server.
		/// </summary>
		// Token: 0x17000890 RID: 2192
		// (get) Token: 0x06002D54 RID: 11604 RVA: 0x000C53B5 File Offset: 0x000C35B5
		public float age
		{
			get
			{
				return Time.time - this.receivedTimestamp;
			}
		}

		// Token: 0x06002D55 RID: 11605 RVA: 0x000C53C3 File Offset: 0x000C35C3
		public ReceivedChatMessage(SteamPlayer speaker, string iconURL, EChatMode newMode, Color newColor, bool newRich, string newContents)
		{
			this.speaker = speaker;
			this.iconURL = iconURL;
			this.mode = newMode;
			this.color = newColor;
			this.useRichTextFormatting = newRich;
			this.contents = newContents;
			this.receivedTimestamp = Time.time;
		}

		/// <summary>
		/// Player who sent the message, or null if it was a plugin broadcast.
		/// Used to retrieve player avatar.
		/// </summary>
		// Token: 0x04001873 RID: 6259
		public SteamPlayer speaker;

		/// <summary>
		/// Web address of a 32x32 .png to use rather than a platform avatar.
		/// Only used if not null/empty.
		/// </summary>
		// Token: 0x04001874 RID: 6260
		public string iconURL;

		/// <summary>
		/// How the message was sent through global, local or group.
		/// Mostly deprecated because that status isn't formatted into texts anymore.
		/// </summary>
		// Token: 0x04001875 RID: 6261
		public EChatMode mode;

		/// <summary>
		/// Default font color to use unless overridden by rich text formatting.
		/// </summary>
		// Token: 0x04001876 RID: 6262
		public Color color;

		/// <summary>
		/// Whether this entry should enable rich text formatting.
		/// False by default because players abuse font size and ugly colors.
		/// </summary>
		// Token: 0x04001877 RID: 6263
		public bool useRichTextFormatting;

		/// <summary>
		/// Text to display for this message.
		/// </summary>
		// Token: 0x04001878 RID: 6264
		public string contents;

		/// <summary>
		/// When the entry was locally received from the server.
		/// </summary>
		// Token: 0x04001879 RID: 6265
		public float receivedTimestamp;
	}
}
