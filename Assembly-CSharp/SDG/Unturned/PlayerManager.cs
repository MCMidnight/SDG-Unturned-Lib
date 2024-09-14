using System;
using System.Collections.Generic;
using SDG.NetPak;
using SDG.NetTransport;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000585 RID: 1413
	public class PlayerManager : SteamCaller
	{
		// Token: 0x06002D41 RID: 11585 RVA: 0x000C4E9B File Offset: 0x000C309B
		[Obsolete]
		public void tellPlayerStates(CSteamID steamID)
		{
		}

		/// <summary>
		/// Whether local client is currently penalized for potentially using a lag switch. Server has an equivalent check which reduces
		/// damage dealt, whereas the clientside check stops shooting in order to prevent abuse of inbound-only lagswitches. For example,
		/// if a cheater freezes enemy positions by dropping inbound traffic while still sending movement and shooting outbound traffic.
		/// </summary>
		// Token: 0x1700088F RID: 2191
		// (get) Token: 0x06002D42 RID: 11586 RVA: 0x000C4EA0 File Offset: 0x000C30A0
		internal static bool IsClientUnderFakeLagPenalty
		{
			get
			{
				bool flag = false;
				flag |= Provider.isServer;
				flag |= !Provider.isPvP;
				flag |= (Provider.clients.Count < 2);
				return Time.realtimeSinceStartupAsDouble - PlayerManager.lastReceivePlayerStates > 2.0 && !flag;
			}
		}

		// Token: 0x06002D43 RID: 11587 RVA: 0x000C4EF0 File Offset: 0x000C30F0
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER)]
		public static void ReceivePlayerStates(in ClientInvocationContext context)
		{
			NetPakReader reader = context.reader;
			uint num;
			SystemNetPakReaderEx.ReadUInt32(reader, ref num);
			if (num <= PlayerManager.seq)
			{
				return;
			}
			PlayerManager.seq = num;
			PlayerManager.lastReceivePlayerStates = Time.realtimeSinceStartupAsDouble;
			ushort num2;
			SystemNetPakReaderEx.ReadUInt16(reader, ref num2);
			if (num2 < 1)
			{
				return;
			}
			for (ushort num3 = 0; num3 < num2; num3 += 1)
			{
				byte channel;
				SystemNetPakReaderEx.ReadUInt8(reader, ref channel);
				Vector3 newPosition;
				UnityNetPakReaderEx.ReadClampedVector3(reader, ref newPosition, 13, 7);
				byte newPitch;
				SystemNetPakReaderEx.ReadUInt8(reader, ref newPitch);
				byte newYaw;
				SystemNetPakReaderEx.ReadUInt8(reader, ref newYaw);
				SteamPlayer steamPlayer = PlayerTool.findSteamPlayerByChannel((int)channel);
				if (steamPlayer != null && !(steamPlayer.player == null) && !(steamPlayer.player.movement == null))
				{
					steamPlayer.player.movement.tellState(newPosition, newPitch, newYaw);
				}
			}
		}

		// Token: 0x06002D44 RID: 11588 RVA: 0x000C4FAF File Offset: 0x000C31AF
		private void onLevelLoaded(int level)
		{
			if (level > Level.BUILD_INDEX_SETUP)
			{
				PlayerManager.seq = 0U;
			}
		}

		// Token: 0x06002D45 RID: 11589 RVA: 0x000C4FC0 File Offset: 0x000C31C0
		private void sendPlayerStates()
		{
			PlayerManager.seq += 1U;
			for (int i = 0; i < Provider.clients.Count; i++)
			{
				SteamPlayer steamPlayer = Provider.clients[i];
				if (steamPlayer != null && !(steamPlayer.player == null))
				{
					ushort updateCount = 0;
					this.playersToSend.Clear();
					for (int j = 0; j < Provider.clients.Count; j++)
					{
						if (j != i)
						{
							SteamPlayer steamPlayer2 = Provider.clients[j];
							if (steamPlayer2 != null && !(steamPlayer2.player == null) && !(steamPlayer2.player.movement == null) && steamPlayer2.player.movement.updates != null && steamPlayer2.player.movement.updates.Count != 0)
							{
								this.playersToSend.Add(steamPlayer2);
								updateCount += (ushort)steamPlayer2.player.movement.updates.Count;
							}
						}
					}
					PlayerManager.SendPlayerStates.Invoke(ENetReliability.Unreliable, steamPlayer.transportConnection, delegate(NetPakWriter writer)
					{
						SystemNetPakWriterEx.WriteUInt32(writer, PlayerManager.seq);
						SystemNetPakWriterEx.WriteUInt16(writer, updateCount);
						foreach (SteamPlayer steamPlayer4 in this.playersToSend)
						{
							for (int l = 0; l < steamPlayer4.player.movement.updates.Count; l++)
							{
								PlayerStateUpdate playerStateUpdate = steamPlayer4.player.movement.updates[l];
								SystemNetPakWriterEx.WriteUInt8(writer, (byte)steamPlayer4.channel);
								UnityNetPakWriterEx.WriteClampedVector3(writer, playerStateUpdate.pos, 13, 7);
								SystemNetPakWriterEx.WriteUInt8(writer, playerStateUpdate.angle);
								SystemNetPakWriterEx.WriteUInt8(writer, playerStateUpdate.rot);
							}
						}
						if (writer.errors != null && Time.realtimeSinceStartup - this.lastSendOverflowWarning > 1f)
						{
							this.lastSendOverflowWarning = Time.realtimeSinceStartup;
							CommandWindow.LogWarningFormat("Error {0} writing player states. The player count ({1}) is probably too high. No this is not a bug introduced in the update, rather a warning of a previously silent bug.", new object[]
							{
								writer.errors,
								Provider.clients.Count
							});
						}
					});
				}
			}
			for (int k = 0; k < Provider.clients.Count; k++)
			{
				SteamPlayer steamPlayer3 = Provider.clients[k];
				if (steamPlayer3 != null && !(steamPlayer3.player == null) && !(steamPlayer3.player.movement == null) && steamPlayer3.player.movement.updates != null && steamPlayer3.player.movement.updates.Count != 0)
				{
					steamPlayer3.player.movement.updates.Clear();
				}
			}
		}

		// Token: 0x06002D46 RID: 11590 RVA: 0x000C519C File Offset: 0x000C339C
		private void Update()
		{
			if (!Provider.isServer || !Level.isLoaded)
			{
				return;
			}
			if (Time.realtimeSinceStartup - PlayerManager.lastTick > Provider.UPDATE_TIME)
			{
				PlayerManager.lastTick += Provider.UPDATE_TIME;
				if (Time.realtimeSinceStartup - PlayerManager.lastTick > Provider.UPDATE_TIME)
				{
					PlayerManager.lastTick = Time.realtimeSinceStartup;
				}
				this.sendPlayerStates();
			}
		}

		// Token: 0x06002D47 RID: 11591 RVA: 0x000C51FC File Offset: 0x000C33FC
		private void Start()
		{
			Level.onLevelLoaded = (LevelLoaded)Delegate.Combine(Level.onLevelLoaded, new LevelLoaded(this.onLevelLoaded));
			CommandLogMemoryUsage.OnExecuted = (Action<List<string>>)Delegate.Combine(CommandLogMemoryUsage.OnExecuted, new Action<List<string>>(this.OnLogMemoryUsage));
		}

		// Token: 0x06002D48 RID: 11592 RVA: 0x000C524C File Offset: 0x000C344C
		private void OnLogMemoryUsage(List<string> results)
		{
			string text = "Players: {0}";
			List<SteamPlayer> clients = Provider.clients;
			results.Add(string.Format(text, (clients != null) ? new int?(clients.Count) : default(int?)));
		}

		// Token: 0x04001869 RID: 6249
		[Obsolete]
		public static ushort updates;

		// Token: 0x0400186A RID: 6250
		private static float lastTick;

		// Token: 0x0400186B RID: 6251
		private static uint seq;

		// Token: 0x0400186C RID: 6252
		private static double lastReceivePlayerStates;

		// Token: 0x0400186D RID: 6253
		private static readonly ClientStaticMethod SendPlayerStates = ClientStaticMethod.Get(new ClientStaticMethod.ReceiveDelegateWithContext(PlayerManager.ReceivePlayerStates));

		// Token: 0x0400186E RID: 6254
		private List<SteamPlayer> playersToSend = new List<SteamPlayer>();

		// Token: 0x0400186F RID: 6255
		private float lastSendOverflowWarning;
	}
}
