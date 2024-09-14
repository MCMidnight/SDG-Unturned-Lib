using System;
using System.Collections.Generic;
using System.Text;
using SDG.NetTransport;
using Steamworks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Unturned.SystemEx;

namespace SDG.Unturned
{
	// Token: 0x0200046C RID: 1132
	public class InteractableSign : Interactable
	{
		// Token: 0x170006C2 RID: 1730
		// (get) Token: 0x06002295 RID: 8853 RVA: 0x00087651 File Offset: 0x00085851
		public CSteamID owner
		{
			get
			{
				return this._owner;
			}
		}

		// Token: 0x170006C3 RID: 1731
		// (get) Token: 0x06002296 RID: 8854 RVA: 0x00087659 File Offset: 0x00085859
		public CSteamID group
		{
			get
			{
				return this._group;
			}
		}

		/// <summary>
		/// Actual unfiltered text.
		/// Kept because plugins might be referencing, and game should use directly once state byte array is refactored.
		/// </summary>
		// Token: 0x170006C4 RID: 1732
		// (get) Token: 0x06002297 RID: 8855 RVA: 0x00087661 File Offset: 0x00085861
		// (set) Token: 0x06002298 RID: 8856 RVA: 0x00087669 File Offset: 0x00085869
		public string text { get; private set; }

		/// <summary>
		/// If profanity filter is enabled this filtered text is displayed on the 3D sign and in the note UI.
		/// Null or empty on the dedicated server.
		/// </summary>
		// Token: 0x170006C5 RID: 1733
		// (get) Token: 0x06002299 RID: 8857 RVA: 0x00087672 File Offset: 0x00085872
		// (set) Token: 0x0600229A RID: 8858 RVA: 0x0008767A File Offset: 0x0008587A
		public string DisplayText { get; private set; }

		// Token: 0x0600229B RID: 8859 RVA: 0x00087683 File Offset: 0x00085883
		public bool checkUpdate(CSteamID enemyPlayer, CSteamID enemyGroup)
		{
			bool isServer = Provider.isServer;
			return !this.isLocked || enemyPlayer == this.owner || (this.group != CSteamID.Nil && enemyGroup == this.group);
		}

		// Token: 0x170006C6 RID: 1734
		// (get) Token: 0x0600229C RID: 8860 RVA: 0x000876C3 File Offset: 0x000858C3
		public bool hasMesh
		{
			get
			{
				return this.label_0 != null || this.label_1 != null || (this.tmpComponents != null && this.tmpComponents.Count > 0);
			}
		}

		// Token: 0x0600229D RID: 8861 RVA: 0x000876FB File Offset: 0x000858FB
		public string trimText(string text)
		{
			return text.Trim();
		}

		// Token: 0x0600229E RID: 8862 RVA: 0x00087703 File Offset: 0x00085903
		public bool isTextValid(string text)
		{
			if (Encoding.UTF8.GetByteCount(text) > 230)
			{
				return false;
			}
			if (this.hasMesh)
			{
				if (!RichTextUtil.isTextValidForSign(text))
				{
					return false;
				}
				if (StringExtension.CountNewlines(text) > 8)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600229F RID: 8863 RVA: 0x00087738 File Offset: 0x00085938
		public void updateText(string newText)
		{
			this.text = newText;
		}

		// Token: 0x060022A0 RID: 8864 RVA: 0x0008774C File Offset: 0x0008594C
		public override void updateState(Asset asset, byte[] state)
		{
			this.isLocked = ((ItemBarricadeAsset)asset).isLocked;
			this._owner = new CSteamID(BitConverter.ToUInt64(state, 0));
			this._group = new CSteamID(BitConverter.ToUInt64(state, 8));
			byte b = state[16];
			if (b > 0)
			{
				string @string = Encoding.UTF8.GetString(state, 17, (int)b);
				this.updateText(@string);
				return;
			}
			this.updateText(string.Empty);
		}

		// Token: 0x060022A1 RID: 8865 RVA: 0x000877BB File Offset: 0x000859BB
		public override bool checkUseable()
		{
			return this.checkUpdate(Provider.client, Player.player.quests.groupID) && !PlayerUI.window.showCursor;
		}

		// Token: 0x060022A2 RID: 8866 RVA: 0x000877E8 File Offset: 0x000859E8
		public override void use()
		{
			PlayerBarricadeSignUI.open(this);
			PlayerLifeUI.close();
		}

		// Token: 0x060022A3 RID: 8867 RVA: 0x000877F5 File Offset: 0x000859F5
		public override bool checkHint(out EPlayerMessage message, out string text, out Color color)
		{
			if (this.checkUseable())
			{
				message = EPlayerMessage.USE;
			}
			else
			{
				message = EPlayerMessage.LOCKED;
			}
			text = "";
			color = Color.white;
			return !PlayerUI.window.showCursor;
		}

		// Token: 0x060022A4 RID: 8868 RVA: 0x00087828 File Offset: 0x00085A28
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, deferMode = ENetInvocationDeferMode.Queue)]
		public void ReceiveChangeText(string newText)
		{
			this.updateText(newText);
		}

		// Token: 0x060022A5 RID: 8869 RVA: 0x00087831 File Offset: 0x00085A31
		public void ClientSetText(string newText)
		{
			InteractableSign.SendChangeTextRequest.Invoke(base.GetNetId(), ENetReliability.Unreliable, newText);
		}

		// Token: 0x060022A6 RID: 8870 RVA: 0x00087848 File Offset: 0x00085A48
		[SteamCall(ESteamCallValidation.SERVERSIDE, ratelimitHz = 2)]
		public void ReceiveChangeTextRequest(in ServerInvocationContext context, string newText)
		{
			Player player = context.GetPlayer();
			if (player == null)
			{
				return;
			}
			if (player.life.isDead)
			{
				return;
			}
			if ((base.transform.position - player.transform.position).sqrMagnitude > 400f)
			{
				return;
			}
			if (this.checkUpdate(player.channel.owner.playerID.steamID, player.quests.groupID))
			{
				string text = this.trimText(newText);
				if (!this.isTextValid(text))
				{
					return;
				}
				bool flag = true;
				ModifySignRequestHandler onModifySignRequested = BarricadeManager.onModifySignRequested;
				if (onModifySignRequested != null)
				{
					onModifySignRequested(player.channel.owner.playerID.steamID, this, ref text, ref flag);
				}
				if (!flag)
				{
					return;
				}
				byte x;
				byte y;
				ushort plant;
				BarricadeRegion region;
				if (BarricadeManager.tryGetRegion(base.transform, out x, out y, out plant, out region))
				{
					BarricadeManager.ServerSetSignTextInternal(this, region, x, y, plant, text);
				}
			}
		}

		// Token: 0x0400111E RID: 4382
		private CSteamID _owner;

		// Token: 0x0400111F RID: 4383
		private CSteamID _group;

		// Token: 0x04001122 RID: 4386
		private bool isLocked;

		// Token: 0x04001123 RID: 4387
		private bool hasInitializedTextComponents;

		/// <summary>
		/// Legacy uGUI text on canvas.
		/// </summary>
		// Token: 0x04001124 RID: 4388
		private Text label_0;

		/// <summary>
		/// Legacy uGUI text on canvas.
		/// </summary>
		// Token: 0x04001125 RID: 4389
		private Text label_1;

		// Token: 0x04001126 RID: 4390
		private List<TextMeshPro> tmpComponents;

		// Token: 0x04001127 RID: 4391
		internal static readonly ClientInstanceMethod<string> SendChangeText = ClientInstanceMethod<string>.Get(typeof(InteractableSign), "ReceiveChangeText");

		// Token: 0x04001128 RID: 4392
		private static readonly ServerInstanceMethod<string> SendChangeTextRequest = ServerInstanceMethod<string>.Get(typeof(InteractableSign), "ReceiveChangeTextRequest");
	}
}
