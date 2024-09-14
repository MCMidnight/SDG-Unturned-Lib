using System;
using UnityEngine;
using UnityEngine.Events;
using Unturned.UnityEx;

namespace SDG.Unturned
{
	/// <summary>
	/// Can be added to any GameObject to receive text chat events.
	/// </summary>
	// Token: 0x020005D5 RID: 1493
	[AddComponentMenu("Unturned/Text Chat Event Hook")]
	public class TextChatEventHook : MonoBehaviour
	{
		// Token: 0x06003012 RID: 12306 RVA: 0x000D4390 File Offset: 0x000D2590
		protected bool passesModeFilter(EChatMode mode)
		{
			switch (this.ModeFilter)
			{
			default:
				return true;
			case TextChatEventHook.EModeFilter.Global:
				return mode == EChatMode.GLOBAL;
			case TextChatEventHook.EModeFilter.Local:
				return mode == EChatMode.LOCAL;
			}
		}

		// Token: 0x06003013 RID: 12307 RVA: 0x000D43C4 File Offset: 0x000D25C4
		protected bool passesPositionFilter(Vector3 playerPosition)
		{
			return this.SqrDetectionRadius < 0.01f || (playerPosition - base.transform.position).sqrMagnitude < this.SqrDetectionRadius;
		}

		// Token: 0x06003014 RID: 12308 RVA: 0x000D4404 File Offset: 0x000D2604
		protected bool passesPhraseFilter(string text)
		{
			switch (this.PhraseFilter)
			{
			case TextChatEventHook.EPhraseFilter.StartsWith:
				return text.StartsWith(this.Phrase, 3);
			case TextChatEventHook.EPhraseFilter.EndsWith:
				return text.EndsWith(this.Phrase, 3);
			}
			return text.IndexOf(this.Phrase, 3) >= 0;
		}

		// Token: 0x06003015 RID: 12309 RVA: 0x000D445C File Offset: 0x000D265C
		protected void onChatted(SteamPlayer player, EChatMode mode, ref Color chatted, ref bool isRich, string text, ref bool isVisible)
		{
			if (player == null || player.player == null || player.player.transform == null)
			{
				return;
			}
			if (!this.passesModeFilter(mode))
			{
				return;
			}
			if (!this.passesPositionFilter(player.player.transform.position))
			{
				return;
			}
			if (!this.passesPhraseFilter(text))
			{
				return;
			}
			this.OnTriggered.TryInvoke(this);
		}

		// Token: 0x06003016 RID: 12310 RVA: 0x000D44C8 File Offset: 0x000D26C8
		protected void OnEnable()
		{
			if (!Provider.isServer)
			{
				return;
			}
			if (string.IsNullOrWhiteSpace(this.Phrase))
			{
				UnturnedLog.warn("{0} phrase is empty", new object[]
				{
					base.name
				});
				return;
			}
			if (!this.isListening)
			{
				ChatManager.onChatted = (Chatted)Delegate.Combine(ChatManager.onChatted, new Chatted(this.onChatted));
				this.isListening = true;
			}
		}

		// Token: 0x06003017 RID: 12311 RVA: 0x000D4533 File Offset: 0x000D2733
		protected void OnDisable()
		{
			if (this.isListening)
			{
				ChatManager.onChatted = (Chatted)Delegate.Remove(ChatManager.onChatted, new Chatted(this.onChatted));
				this.isListening = false;
			}
		}

		/// <summary>
		/// Filter to apply to message type.
		/// </summary>
		// Token: 0x04001A0D RID: 6669
		public TextChatEventHook.EModeFilter ModeFilter;

		/// <summary>
		/// Sphere radius (squared) around this transform to detect player messages.
		/// e.g. 16 is 4 meters
		/// </summary>
		// Token: 0x04001A0E RID: 6670
		public float SqrDetectionRadius;

		/// <summary>
		/// Substring to search for in message.
		/// </summary>
		// Token: 0x04001A0F RID: 6671
		public string Phrase;

		/// <summary>
		/// Filter to apply to message text.
		/// </summary>
		// Token: 0x04001A10 RID: 6672
		public TextChatEventHook.EPhraseFilter PhraseFilter;

		/// <summary>
		/// Invoked when a player message passes the filters.
		/// </summary>
		// Token: 0x04001A11 RID: 6673
		public UnityEvent OnTriggered;

		// Token: 0x04001A12 RID: 6674
		private bool isListening;

		// Token: 0x0200099A RID: 2458
		public enum EModeFilter
		{
			/// <summary>
			/// Message can be in any chat channel.
			/// </summary>
			// Token: 0x040033CB RID: 13259
			Any,
			/// <summary>
			/// Message must be in Global channel.
			/// </summary>
			// Token: 0x040033CC RID: 13260
			Global,
			/// <summary>
			/// Message must be in Local channel.
			/// </summary>
			// Token: 0x040033CD RID: 13261
			Local
		}

		// Token: 0x0200099B RID: 2459
		public enum EPhraseFilter
		{
			/// <summary>
			/// Message must start with phrase text.
			/// </summary>
			// Token: 0x040033CF RID: 13263
			StartsWith,
			/// <summary>
			/// Message must contain phrase text.
			/// </summary>
			// Token: 0x040033D0 RID: 13264
			Contains,
			/// <summary>
			/// Message must end with phrase text.
			/// </summary>
			// Token: 0x040033D1 RID: 13265
			EndsWith
		}
	}
}
