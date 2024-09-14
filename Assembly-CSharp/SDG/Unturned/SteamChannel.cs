using System;
using System.Collections.Generic;
using System.Reflection;
using SDG.NetTransport;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200068B RID: 1675
	public class SteamChannel : MonoBehaviour
	{
		// Token: 0x17000A02 RID: 2562
		// (get) Token: 0x0600383C RID: 14396 RVA: 0x00109D87 File Offset: 0x00107F87
		// (set) Token: 0x0600383D RID: 14397 RVA: 0x00109D8F File Offset: 0x00107F8F
		public SteamChannelMethod[] calls { get; protected set; }

		/// <summary>
		/// If true, this object is owned by a locally-controlled player.
		/// For example, some code is not run for "remote" players.
		/// Always true in singleplayer. Always false on dedicated server.
		/// </summary>
		// Token: 0x17000A03 RID: 2563
		// (get) Token: 0x0600383E RID: 14398 RVA: 0x00109D98 File Offset: 0x00107F98
		// (set) Token: 0x0600383F RID: 14399 RVA: 0x00109DA0 File Offset: 0x00107FA0
		public bool IsLocalPlayer
		{
			get
			{
				return this.isOwner;
			}
			internal set
			{
				this.isOwner = value;
			}
		}

		/// <summary>
		/// Use on server when invoking client methods on the owning player.
		/// </summary>
		// Token: 0x06003840 RID: 14400 RVA: 0x00109DA9 File Offset: 0x00107FA9
		public ITransportConnection GetOwnerTransportConnection()
		{
			SteamPlayer steamPlayer = this.owner;
			if (steamPlayer == null)
			{
				return null;
			}
			return steamPlayer.transportConnection;
		}

		// Token: 0x06003841 RID: 14401 RVA: 0x00109DBC File Offset: 0x00107FBC
		[Obsolete]
		public bool checkServer(CSteamID steamID)
		{
			return steamID == Provider.server;
		}

		// Token: 0x06003842 RID: 14402 RVA: 0x00109DC9 File Offset: 0x00107FC9
		[Obsolete]
		public bool checkOwner(CSteamID steamID)
		{
			return this.owner != null && steamID == this.owner.playerID.steamID;
		}

		/// <summary>
		/// Replacement for ESteamCall.NOT_OWNER.
		/// </summary>
		// Token: 0x06003843 RID: 14403 RVA: 0x00109DEC File Offset: 0x00107FEC
		public PooledTransportConnectionList GatherRemoteClientConnectionsExcludingOwner()
		{
			PooledTransportConnectionList pooledTransportConnectionList = TransportConnectionListPool.Get();
			foreach (SteamPlayer steamPlayer in Provider.clients)
			{
				if (steamPlayer != this.owner)
				{
					pooledTransportConnectionList.Add(steamPlayer.transportConnection);
				}
			}
			return pooledTransportConnectionList;
		}

		// Token: 0x06003844 RID: 14404 RVA: 0x00109E54 File Offset: 0x00108054
		[Obsolete("Replaced by GatherRemoteClientConnectionsExcludingOwner")]
		public IEnumerable<ITransportConnection> EnumerateClients_RemoteNotOwner()
		{
			return this.GatherRemoteClientConnectionsExcludingOwner();
		}

		// Token: 0x06003845 RID: 14405 RVA: 0x00109E5C File Offset: 0x0010805C
		public PooledTransportConnectionList GatherRemoteClientConnectionsWithinSphereExcludingOwner(Vector3 position, float radius)
		{
			PooledTransportConnectionList pooledTransportConnectionList = TransportConnectionListPool.Get();
			float num = radius * radius;
			foreach (SteamPlayer steamPlayer in Provider.clients)
			{
				if (steamPlayer != this.owner && steamPlayer.player != null && (steamPlayer.player.transform.position - position).sqrMagnitude < num)
				{
					pooledTransportConnectionList.Add(steamPlayer.transportConnection);
				}
			}
			return pooledTransportConnectionList;
		}

		// Token: 0x06003846 RID: 14406 RVA: 0x00109EF8 File Offset: 0x001080F8
		[Obsolete("Replaced by GatherRemoteClientConnectionsWithinSphereExcludingOwner")]
		public IEnumerable<ITransportConnection> EnumerateClients_RemoteNotOwnerWithinSphere(Vector3 position, float radius)
		{
			return this.GatherRemoteClientConnectionsWithinSphereExcludingOwner(position, radius);
		}

		// Token: 0x06003847 RID: 14407 RVA: 0x00109F04 File Offset: 0x00108104
		public PooledTransportConnectionList GatherOwnerAndClientConnectionsWithinSphere(Vector3 position, float radius)
		{
			PooledTransportConnectionList pooledTransportConnectionList = TransportConnectionListPool.Get();
			float num = radius * radius;
			foreach (SteamPlayer steamPlayer in Provider.clients)
			{
				if (steamPlayer == this.owner || (steamPlayer.player != null && (steamPlayer.player.transform.position - position).sqrMagnitude < num))
				{
					pooledTransportConnectionList.Add(steamPlayer.transportConnection);
				}
			}
			return pooledTransportConnectionList;
		}

		// Token: 0x06003848 RID: 14408 RVA: 0x00109FA0 File Offset: 0x001081A0
		[Obsolete("Replaced by GatherOwnerAndClientConnectionsWithinSphere")]
		public IEnumerable<ITransportConnection> EnumerateClients_WithinSphereOrOwner(Vector3 position, float radius)
		{
			return this.GatherOwnerAndClientConnectionsWithinSphere(position, radius);
		}

		/// <returns>True if the call succeeded, or false if the sender should be refused.</returns>
		// Token: 0x06003849 RID: 14409 RVA: 0x00109FAC File Offset: 0x001081AC
		[Obsolete]
		public bool receive(CSteamID steamID, byte[] packet, int offset, int size)
		{
			if (SteamChannel.onTriggerReceive != null)
			{
				if (!SteamChannel.warnedAboutTriggerReceive)
				{
					SteamChannel.warnedAboutTriggerReceive = true;
					CommandWindow.LogError("Plugin(s) using unsafe onTriggerReceive which will be deprecated soon.");
				}
				try
				{
					byte[] array = packet;
					if (Provider.useConstNetEvents)
					{
						array = new byte[offset + size];
						Array.Copy(packet, array, array.Length);
					}
					SteamChannel.onTriggerReceive(this, steamID, array, offset, size);
					if (Provider.useConstNetEvents && Provider.hasNetBufferChanged(packet, array, offset, size))
					{
						CommandWindow.LogError("Plugin(s) modified buffer during onTriggerReceive!");
					}
				}
				catch (Exception e)
				{
					UnturnedLog.warn("Plugin raised an exception from SteamChannel.onTriggerReceive:");
					UnturnedLog.exception(e);
				}
			}
			if (size < 3)
			{
				return true;
			}
			int num = (int)packet[offset + 1];
			this.buildCallArrayIfDirty();
			if (num < 0 || num >= this.calls.Length)
			{
				return true;
			}
			byte b = packet[offset];
			bool flag;
			switch (this.calls[num].attribute.validation)
			{
			case ESteamCallValidation.NONE:
				flag = true;
				break;
			case ESteamCallValidation.ONLY_FROM_SERVER:
				flag = (steamID == Provider.server);
				break;
			case ESteamCallValidation.SERVERSIDE:
				flag = Provider.isServer;
				break;
			case ESteamCallValidation.ONLY_FROM_OWNER:
				flag = (this.owner != null && steamID == this.owner.playerID.steamID);
				break;
			default:
				flag = false;
				UnturnedLog.warn("Unhandled RPC validation type on method: " + this.calls[num].method.Name);
				break;
			}
			if (!flag)
			{
				return true;
			}
			if (this.calls[num].attribute.rateLimitIndex >= 0)
			{
				string name = this.calls[num].method.Name;
				SteamPlayer steamPlayer = PlayerTool.getSteamPlayer(steamID);
				if (steamPlayer == null)
				{
					UnturnedLog.info(string.Concat(new string[]
					{
						"RPC ",
						name,
						" on channel ",
						this.id.ToString(),
						" called without player sender, so we're ignoring it"
					}));
					return true;
				}
				float realtimeSinceStartup = Time.realtimeSinceStartup;
				float num2 = steamPlayer.rpcAllowedTimes[this.calls[num].attribute.rateLimitIndex];
				if (realtimeSinceStartup < num2)
				{
					return true;
				}
				steamPlayer.rpcAllowedTimes[this.calls[num].attribute.rateLimitIndex] = realtimeSinceStartup + this.calls[num].attribute.ratelimitSeconds;
			}
			try
			{
				if (this.calls[num].types.Length != 0)
				{
					object[] objectsForLegacyRPC = SteamPacker.getObjectsForLegacyRPC(offset, 3, size, packet, this.calls[num].types, this.calls[num].typesReadOffset);
					SteamChannelMethod.EContextType contextType = this.calls[num].contextType;
					if (contextType != SteamChannelMethod.EContextType.Client)
					{
						if (contextType == SteamChannelMethod.EContextType.Server)
						{
							objectsForLegacyRPC[this.calls[num].contextParameterIndex] = ServerInvocationContext.FromSteamIDForBackwardsCompatibility(steamID);
						}
					}
					else
					{
						objectsForLegacyRPC[this.calls[num].contextParameterIndex] = default(ClientInvocationContext);
					}
					if (this.calls[num].method.IsStatic)
					{
						this.calls[num].method.Invoke(null, objectsForLegacyRPC);
					}
					else
					{
						this.calls[num].method.Invoke(this.calls[num].component, objectsForLegacyRPC);
					}
				}
				else
				{
					this.calls[num].method.Invoke(this.calls[num].component, null);
				}
			}
			catch (Exception e2)
			{
				UnturnedLog.info("Exception raised when RPC invoked {0}:", new object[]
				{
					this.calls[num].method.Name
				});
				UnturnedLog.exception(e2);
			}
			return true;
		}

		// Token: 0x0600384A RID: 14410 RVA: 0x0010A328 File Offset: 0x00108528
		[Obsolete]
		public object read(Type type)
		{
			return SteamPacker.read(type);
		}

		// Token: 0x0600384B RID: 14411 RVA: 0x0010A330 File Offset: 0x00108530
		[Obsolete]
		public object[] read(Type type_0, Type type_1, Type type_2)
		{
			return SteamPacker.read(type_0, type_1, type_2);
		}

		// Token: 0x0600384C RID: 14412 RVA: 0x0010A33A File Offset: 0x0010853A
		[Obsolete]
		public object[] read(Type type_0, Type type_1, Type type_2, Type type_3)
		{
			return SteamPacker.read(type_0, type_1, type_2, type_3);
		}

		// Token: 0x0600384D RID: 14413 RVA: 0x0010A346 File Offset: 0x00108546
		[Obsolete]
		public object[] read(Type type_0, Type type_1, Type type_2, Type type_3, Type type_4, Type type_5)
		{
			return SteamPacker.read(type_0, type_1, type_2, type_3, type_4, type_5);
		}

		// Token: 0x0600384E RID: 14414 RVA: 0x0010A356 File Offset: 0x00108556
		[Obsolete]
		public object[] read(Type type_0, Type type_1, Type type_2, Type type_3, Type type_4, Type type_5, Type type_6)
		{
			return SteamPacker.read(type_0, type_1, type_2, type_3, type_4, type_5, type_6);
		}

		// Token: 0x0600384F RID: 14415 RVA: 0x0010A368 File Offset: 0x00108568
		[Obsolete]
		public object[] read(params Type[] types)
		{
			return SteamPacker.read(types);
		}

		// Token: 0x06003850 RID: 14416 RVA: 0x0010A370 File Offset: 0x00108570
		[Obsolete]
		public void write(object objects)
		{
		}

		// Token: 0x06003851 RID: 14417 RVA: 0x0010A372 File Offset: 0x00108572
		[Obsolete]
		public void write(object object_0, object object_1, object object_2)
		{
		}

		// Token: 0x06003852 RID: 14418 RVA: 0x0010A374 File Offset: 0x00108574
		[Obsolete]
		public void write(object object_0, object object_1, object object_2, object object_3)
		{
		}

		// Token: 0x06003853 RID: 14419 RVA: 0x0010A376 File Offset: 0x00108576
		[Obsolete]
		public void write(object object_0, object object_1, object object_2, object object_3, object object_4, object object_5)
		{
		}

		// Token: 0x06003854 RID: 14420 RVA: 0x0010A378 File Offset: 0x00108578
		[Obsolete]
		public void write(object object_0, object object_1, object object_2, object object_3, object object_4, object object_5, object object_6)
		{
		}

		// Token: 0x06003855 RID: 14421 RVA: 0x0010A37A File Offset: 0x0010857A
		[Obsolete]
		public void write(params object[] objects)
		{
		}

		// Token: 0x17000A04 RID: 2564
		// (get) Token: 0x06003856 RID: 14422 RVA: 0x0010A37C File Offset: 0x0010857C
		// (set) Token: 0x06003857 RID: 14423 RVA: 0x0010A383 File Offset: 0x00108583
		[Obsolete]
		public bool longBinaryData
		{
			get
			{
				return SteamPacker.longBinaryData;
			}
			set
			{
				SteamPacker.longBinaryData = value;
			}
		}

		// Token: 0x06003858 RID: 14424 RVA: 0x0010A38B File Offset: 0x0010858B
		[Obsolete]
		public void openWrite()
		{
		}

		// Token: 0x06003859 RID: 14425 RVA: 0x0010A38D File Offset: 0x0010858D
		[Obsolete]
		public void closeWrite(string name, CSteamID steamID, ESteamPacket type)
		{
		}

		// Token: 0x0600385A RID: 14426 RVA: 0x0010A38F File Offset: 0x0010858F
		[Obsolete]
		public void closeWrite(string name, ESteamCall mode, byte bound, ESteamPacket type)
		{
		}

		// Token: 0x0600385B RID: 14427 RVA: 0x0010A391 File Offset: 0x00108591
		[Obsolete]
		public void closeWrite(string name, ESteamCall mode, byte x, byte y, byte area, ESteamPacket type)
		{
		}

		// Token: 0x0600385C RID: 14428 RVA: 0x0010A393 File Offset: 0x00108593
		[Obsolete]
		public void closeWrite(string name, ESteamCall mode, ESteamPacket type)
		{
		}

		// Token: 0x0600385D RID: 14429 RVA: 0x0010A398 File Offset: 0x00108598
		[Obsolete]
		public void send(string name, CSteamID steamID, ESteamPacket type, params object[] arguments)
		{
			int call = this.getCall(name);
			if (call == -1)
			{
				return;
			}
			int size;
			byte[] packet;
			this.getPacket(type, call, out size, out packet, arguments);
			if (this.IsLocalPlayer && steamID == Provider.client)
			{
				this.receive(Provider.client, packet, 0, size);
				return;
			}
			if (Provider.isServer && steamID == Provider.server)
			{
				this.receive(Provider.server, packet, 0, size);
				return;
			}
			Provider.send(steamID, type, packet, size, 0);
		}

		// Token: 0x0600385E RID: 14430 RVA: 0x0010A413 File Offset: 0x00108613
		[Obsolete]
		public void sendAside(string name, CSteamID steamID, ESteamPacket type, params object[] arguments)
		{
		}

		// Token: 0x0600385F RID: 14431 RVA: 0x0010A418 File Offset: 0x00108618
		[Obsolete]
		public void send(ESteamCall mode, byte bound, ESteamPacket type, int size, byte[] packet)
		{
			if (mode == ESteamCall.SERVER)
			{
				if (Provider.isServer)
				{
					this.receive(Provider.server, packet, 0, size);
					return;
				}
				throw new NotSupportedException();
			}
			else if (mode == ESteamCall.ALL)
			{
				if (!Provider.isServer)
				{
					throw new NotSupportedException();
				}
				for (int i = 0; i < Provider.clients.Count; i++)
				{
					if (Provider.clients[i].playerID.steamID != Provider.client && Provider.clients[i].player != null && Provider.clients[i].player.movement.bound == bound)
					{
						Provider.sendToClient(Provider.clients[i].transportConnection, type, packet, size);
					}
				}
				if (Provider.isServer)
				{
					this.receive(Provider.server, packet, 0, size);
					return;
				}
				this.receive(Provider.client, packet, 0, size);
				return;
			}
			else if (mode == ESteamCall.OTHERS)
			{
				if (!Provider.isServer)
				{
					throw new NotSupportedException();
				}
				for (int j = 0; j < Provider.clients.Count; j++)
				{
					if (Provider.clients[j].playerID.steamID != Provider.client && Provider.clients[j].player != null && Provider.clients[j].player.movement.bound == bound)
					{
						Provider.sendToClient(Provider.clients[j].transportConnection, type, packet, size);
					}
				}
				return;
			}
			else if (mode == ESteamCall.OWNER)
			{
				if (this.IsLocalPlayer)
				{
					this.receive(this.owner.playerID.steamID, packet, 0, size);
					return;
				}
				Provider.sendToClient(this.owner.transportConnection, type, packet, size);
				return;
			}
			else
			{
				if (mode != ESteamCall.NOT_OWNER)
				{
					if (mode == ESteamCall.CLIENTS)
					{
						for (int k = 0; k < Provider.clients.Count; k++)
						{
							if (Provider.clients[k].playerID.steamID != Provider.client && Provider.clients[k].player != null && Provider.clients[k].player.movement.bound == bound)
							{
								Provider.sendToClient(Provider.clients[k].transportConnection, type, packet, size);
							}
						}
						if (Provider.isClient)
						{
							this.receive(Provider.client, packet, 0, size);
						}
					}
					return;
				}
				if (!Provider.isServer)
				{
					throw new NotSupportedException();
				}
				for (int l = 0; l < Provider.clients.Count; l++)
				{
					if (Provider.clients[l].playerID.steamID != this.owner.playerID.steamID && Provider.clients[l].player != null && Provider.clients[l].player.movement.bound == bound)
					{
						Provider.sendToClient(Provider.clients[l].transportConnection, type, packet, size);
					}
				}
				return;
			}
		}

		// Token: 0x06003860 RID: 14432 RVA: 0x0010A73C File Offset: 0x0010893C
		[Obsolete]
		public void send(string name, ESteamCall mode, byte bound, ESteamPacket type, params object[] arguments)
		{
			int call = this.getCall(name);
			if (call == -1)
			{
				return;
			}
			int size;
			byte[] packet;
			this.getPacket(type, call, out size, out packet, arguments);
			this.send(mode, bound, type, size, packet);
		}

		// Token: 0x06003861 RID: 14433 RVA: 0x0010A774 File Offset: 0x00108974
		[Obsolete]
		public void send(ESteamCall mode, byte x, byte y, byte area, ESteamPacket type, int size, byte[] packet)
		{
			if (mode == ESteamCall.SERVER)
			{
				if (Provider.isServer)
				{
					this.receive(Provider.server, packet, 0, size);
					return;
				}
				throw new NotSupportedException();
			}
			else if (mode == ESteamCall.ALL)
			{
				if (!Provider.isServer)
				{
					throw new NotSupportedException();
				}
				for (int i = 0; i < Provider.clients.Count; i++)
				{
					if (Provider.clients[i].playerID.steamID != Provider.client && Provider.clients[i].player != null && Regions.checkArea(x, y, Provider.clients[i].player.movement.region_x, Provider.clients[i].player.movement.region_y, area))
					{
						Provider.sendToClient(Provider.clients[i].transportConnection, type, packet, size);
					}
				}
				if (Provider.isServer)
				{
					this.receive(Provider.server, packet, 0, size);
					return;
				}
				this.receive(Provider.client, packet, 0, size);
				return;
			}
			else if (mode == ESteamCall.OTHERS)
			{
				if (!Provider.isServer)
				{
					throw new NotSupportedException();
				}
				for (int j = 0; j < Provider.clients.Count; j++)
				{
					if (Provider.clients[j].playerID.steamID != Provider.client && Provider.clients[j].player != null && Regions.checkArea(x, y, Provider.clients[j].player.movement.region_x, Provider.clients[j].player.movement.region_y, area))
					{
						Provider.sendToClient(Provider.clients[j].transportConnection, type, packet, size);
					}
				}
				return;
			}
			else if (mode == ESteamCall.OWNER)
			{
				if (this.IsLocalPlayer)
				{
					this.receive(this.owner.playerID.steamID, packet, 0, size);
					return;
				}
				Provider.sendToClient(this.owner.transportConnection, type, packet, size);
				return;
			}
			else
			{
				if (mode != ESteamCall.NOT_OWNER)
				{
					if (mode == ESteamCall.CLIENTS)
					{
						for (int k = 0; k < Provider.clients.Count; k++)
						{
							if (Provider.clients[k].playerID.steamID != Provider.client && Provider.clients[k].player != null && Regions.checkArea(x, y, Provider.clients[k].player.movement.region_x, Provider.clients[k].player.movement.region_y, area))
							{
								Provider.sendToClient(Provider.clients[k].transportConnection, type, packet, size);
							}
						}
						if (Provider.isClient)
						{
							this.receive(Provider.client, packet, 0, size);
						}
					}
					return;
				}
				if (!Provider.isServer)
				{
					throw new NotSupportedException();
				}
				for (int l = 0; l < Provider.clients.Count; l++)
				{
					if (Provider.clients[l].playerID.steamID != this.owner.playerID.steamID && Provider.clients[l].player != null && Regions.checkArea(x, y, Provider.clients[l].player.movement.region_x, Provider.clients[l].player.movement.region_y, area))
					{
						Provider.sendToClient(Provider.clients[l].transportConnection, type, packet, size);
					}
				}
				return;
			}
		}

		// Token: 0x06003862 RID: 14434 RVA: 0x0010AB30 File Offset: 0x00108D30
		[Obsolete]
		public void send(string name, ESteamCall mode, byte x, byte y, byte area, ESteamPacket type, params object[] arguments)
		{
			int call = this.getCall(name);
			if (call == -1)
			{
				return;
			}
			int size;
			byte[] packet;
			this.getPacket(type, call, out size, out packet, arguments);
			this.send(mode, x, y, area, type, size, packet);
		}

		// Token: 0x06003863 RID: 14435 RVA: 0x0010AB6C File Offset: 0x00108D6C
		[Obsolete]
		public void send(ESteamCall mode, ESteamPacket type, int size, byte[] packet)
		{
			if (mode == ESteamCall.SERVER)
			{
				if (Provider.isServer)
				{
					this.receive(Provider.server, packet, 0, size);
					return;
				}
				throw new NotSupportedException();
			}
			else if (mode == ESteamCall.ALL)
			{
				if (!Provider.isServer)
				{
					throw new NotSupportedException();
				}
				for (int i = 0; i < Provider.clients.Count; i++)
				{
					if (Provider.clients[i].playerID.steamID != Provider.client)
					{
						Provider.sendToClient(Provider.clients[i].transportConnection, type, packet, size);
					}
				}
				if (Provider.isServer)
				{
					this.receive(Provider.server, packet, 0, size);
					return;
				}
				this.receive(Provider.client, packet, 0, size);
				return;
			}
			else if (mode == ESteamCall.OTHERS)
			{
				if (!Provider.isServer)
				{
					throw new NotSupportedException();
				}
				for (int j = 0; j < Provider.clients.Count; j++)
				{
					if (Provider.clients[j].playerID.steamID != Provider.client)
					{
						Provider.sendToClient(Provider.clients[j].transportConnection, type, packet, size);
					}
				}
				return;
			}
			else if (mode == ESteamCall.OWNER)
			{
				if (this.IsLocalPlayer)
				{
					this.receive(this.owner.playerID.steamID, packet, 0, size);
					return;
				}
				Provider.sendToClient(this.owner.transportConnection, type, packet, size);
				return;
			}
			else
			{
				if (mode != ESteamCall.NOT_OWNER)
				{
					if (mode == ESteamCall.CLIENTS)
					{
						for (int k = 0; k < Provider.clients.Count; k++)
						{
							if (Provider.clients[k].playerID.steamID != Provider.client)
							{
								Provider.sendToClient(Provider.clients[k].transportConnection, type, packet, size);
							}
						}
						if (Provider.isClient)
						{
							this.receive(Provider.client, packet, 0, size);
						}
					}
					return;
				}
				if (!Provider.isServer)
				{
					throw new NotSupportedException();
				}
				for (int l = 0; l < Provider.clients.Count; l++)
				{
					if (Provider.clients[l].playerID.steamID != this.owner.playerID.steamID)
					{
						Provider.sendToClient(Provider.clients[l].transportConnection, type, packet, size);
					}
				}
				return;
			}
		}

		// Token: 0x06003864 RID: 14436 RVA: 0x0010AD9C File Offset: 0x00108F9C
		[Obsolete]
		public void send(string name, ESteamCall mode, ESteamPacket type, params object[] arguments)
		{
			if (SteamChannel.onTriggerSend != null)
			{
				if (!SteamChannel.warnedAboutTriggerSend)
				{
					SteamChannel.warnedAboutTriggerSend = true;
					CommandWindow.LogError("Plugin(s) using unsafe onTriggerSend which will be deprecated soon.");
				}
				try
				{
					SteamChannel.onTriggerSend(this.owner, name, mode, type, arguments);
				}
				catch (Exception e)
				{
					UnturnedLog.warn("Plugin raised an exception from SteamChannel.onTriggerSend:");
					UnturnedLog.exception(e);
				}
			}
			int call = this.getCall(name);
			if (call == -1)
			{
				return;
			}
			int size;
			byte[] packet;
			this.getPacket(type, call, out size, out packet, arguments);
			this.send(mode, type, size, packet);
		}

		// Token: 0x06003865 RID: 14437 RVA: 0x0010AE28 File Offset: 0x00109028
		[Obsolete]
		public void send(ESteamCall mode, Vector3 point, float radius, ESteamPacket type, int size, byte[] packet)
		{
			radius *= radius;
			if (mode == ESteamCall.SERVER)
			{
				if (Provider.isServer)
				{
					this.receive(Provider.server, packet, 0, size);
					return;
				}
				throw new NotSupportedException();
			}
			else if (mode == ESteamCall.ALL)
			{
				if (!Provider.isServer)
				{
					throw new NotSupportedException();
				}
				for (int i = 0; i < Provider.clients.Count; i++)
				{
					if (Provider.clients[i].playerID.steamID != Provider.client && Provider.clients[i].player != null && (Provider.clients[i].player.transform.position - point).sqrMagnitude < radius)
					{
						Provider.sendToClient(Provider.clients[i].transportConnection, type, packet, size);
					}
				}
				if (Provider.isServer)
				{
					this.receive(Provider.server, packet, 0, size);
					return;
				}
				this.receive(Provider.client, packet, 0, size);
				return;
			}
			else if (mode == ESteamCall.OTHERS)
			{
				if (!Provider.isServer)
				{
					throw new NotSupportedException();
				}
				for (int j = 0; j < Provider.clients.Count; j++)
				{
					if (Provider.clients[j].playerID.steamID != Provider.client && Provider.clients[j].player != null && (Provider.clients[j].player.transform.position - point).sqrMagnitude < radius)
					{
						Provider.sendToClient(Provider.clients[j].transportConnection, type, packet, size);
					}
				}
				return;
			}
			else if (mode == ESteamCall.OWNER)
			{
				if (this.IsLocalPlayer)
				{
					this.receive(this.owner.playerID.steamID, packet, 0, size);
					return;
				}
				Provider.sendToClient(this.owner.transportConnection, type, packet, size);
				return;
			}
			else
			{
				if (mode != ESteamCall.NOT_OWNER)
				{
					if (mode == ESteamCall.CLIENTS)
					{
						for (int k = 0; k < Provider.clients.Count; k++)
						{
							if (Provider.clients[k].playerID.steamID != Provider.client && Provider.clients[k].player != null && (Provider.clients[k].player.transform.position - point).sqrMagnitude < radius)
							{
								Provider.sendToClient(Provider.clients[k].transportConnection, type, packet, size);
							}
						}
						if (Provider.isClient)
						{
							this.receive(Provider.client, packet, 0, size);
						}
					}
					return;
				}
				if (!Provider.isServer)
				{
					throw new NotSupportedException();
				}
				for (int l = 0; l < Provider.clients.Count; l++)
				{
					if (Provider.clients[l].playerID.steamID != this.owner.playerID.steamID && Provider.clients[l].player != null && (Provider.clients[l].player.transform.position - point).sqrMagnitude < radius)
					{
						Provider.sendToClient(Provider.clients[l].transportConnection, type, packet, size);
					}
				}
				return;
			}
		}

		// Token: 0x06003866 RID: 14438 RVA: 0x0010B1A0 File Offset: 0x001093A0
		[Obsolete]
		public void send(string name, ESteamCall mode, Vector3 point, float radius, ESteamPacket type, params object[] arguments)
		{
			int call = this.getCall(name);
			if (call == -1)
			{
				return;
			}
			int size;
			byte[] packet;
			this.getPacket(type, call, out size, out packet, arguments);
			this.send(mode, point, radius, type, size, packet);
		}

		/// <summary>
		/// Calls array needs rebuilding the next time it is used.
		/// Should be invoked when adding/removing components with RPCs.
		/// </summary>
		// Token: 0x06003867 RID: 14439 RVA: 0x0010B1D7 File Offset: 0x001093D7
		public void markDirty()
		{
			this.callArrayDirty = true;
		}

		/// <summary>
		/// Find methods with SteamCall attribute, and gather them into an array.
		/// </summary>
		// Token: 0x06003868 RID: 14440 RVA: 0x0010B1E0 File Offset: 0x001093E0
		private void buildCallArray()
		{
			SteamChannel.workingCalls.Clear();
			SteamChannel.workingComponents.Clear();
			base.GetComponents<Component>(SteamChannel.workingComponents);
			foreach (Component component in SteamChannel.workingComponents)
			{
				if ((component.hideFlags & HideFlags.NotEditable) != HideFlags.NotEditable)
				{
					MemberInfo[] array = component.GetType().GetMethods(28);
					foreach (MethodInfo methodInfo in array)
					{
						SteamCall customAttribute = CustomAttributeExtensions.GetCustomAttribute<SteamCall>(methodInfo);
						if (customAttribute != null)
						{
							string text = customAttribute.legacyName;
							if (string.IsNullOrEmpty(text))
							{
								text = methodInfo.Name;
							}
							ParameterInfo[] parameters = methodInfo.GetParameters();
							Type[] array2 = new Type[parameters.Length];
							for (int j = 0; j < parameters.Length; j++)
							{
								array2[j] = parameters[j].ParameterType;
							}
							int num = 0;
							SteamChannelMethod.EContextType contextType = SteamChannelMethod.EContextType.None;
							int contextParameterIndex = -1;
							if (num < array2.Length)
							{
								if (array2[num].GetElementType() == typeof(ClientInvocationContext))
								{
									contextParameterIndex = num;
									num++;
									contextType = SteamChannelMethod.EContextType.Client;
								}
								else if (array2[num].GetElementType() == typeof(ServerInvocationContext))
								{
									contextParameterIndex = num;
									num++;
									contextType = SteamChannelMethod.EContextType.Server;
								}
							}
							if (customAttribute.ratelimitHz > 0)
							{
								customAttribute.ratelimitSeconds = 1f / (float)customAttribute.ratelimitHz;
								ServerMethodInfo serverMethodInfo = NetReflection.GetServerMethodInfo(methodInfo.DeclaringType, methodInfo.Name);
								if (serverMethodInfo != null)
								{
									customAttribute.rateLimitIndex = serverMethodInfo.rateLimitIndex;
								}
							}
							SteamChannel.workingCalls.Add(new SteamChannelMethod(component, methodInfo, text, array2, num, contextType, contextParameterIndex, customAttribute));
						}
					}
				}
			}
			this.calls = SteamChannel.workingCalls.ToArray();
			if (this.calls.Length > 235)
			{
				CommandWindow.LogError(base.name + " approaching 255 methods!");
			}
		}

		// Token: 0x06003869 RID: 14441 RVA: 0x0010B3F4 File Offset: 0x001095F4
		private void buildCallArrayIfDirty()
		{
			if (this.callArrayDirty)
			{
				this.callArrayDirty = false;
				this.buildCallArray();
			}
		}

		// Token: 0x0600386A RID: 14442 RVA: 0x0010B40B File Offset: 0x0010960B
		public void setup()
		{
			Provider.openChannel(this);
		}

		// Token: 0x0600386B RID: 14443 RVA: 0x0010B413 File Offset: 0x00109613
		private void encodeChannelId(byte[] packet)
		{
			packet[2] = (byte)(this.id & 255);
		}

		// Token: 0x0600386C RID: 14444 RVA: 0x0010B425 File Offset: 0x00109625
		[Obsolete]
		public void getPacket(ESteamPacket type, int index, out int size, out byte[] packet)
		{
			packet = SteamPacker.closeWrite(out size);
			packet[0] = (byte)type;
			packet[1] = (byte)index;
			this.encodeChannelId(packet);
		}

		/// <summary>
		/// Encode byte array of voice data to send.
		/// </summary>
		// Token: 0x0600386D RID: 14445 RVA: 0x0010B447 File Offset: 0x00109647
		[Obsolete]
		public void encodeVoicePacket(byte callIndex, out int size, out byte[] packet, byte[] bytes, ushort length, bool usingWalkieTalkie)
		{
			size = 0;
			packet = null;
		}

		/// <summary>
		/// Decode voice parameters from byte array.
		/// </summary>
		// Token: 0x0600386E RID: 14446 RVA: 0x0010B44F File Offset: 0x0010964F
		[Obsolete]
		public void decodeVoicePacket(byte[] packet, out uint compressedSize, out bool usingWalkieTalkie)
		{
			compressedSize = 0U;
			usingWalkieTalkie = false;
		}

		// Token: 0x0600386F RID: 14447 RVA: 0x0010B457 File Offset: 0x00109657
		[Obsolete]
		public void sendVoicePacket(SteamPlayer player, byte[] packet, int packetSize)
		{
		}

		// Token: 0x06003870 RID: 14448 RVA: 0x0010B459 File Offset: 0x00109659
		[Obsolete]
		public void getPacket(ESteamPacket type, int index, out int size, out byte[] packet, params object[] arguments)
		{
			packet = SteamPacker.getBytes(3, out size, arguments);
			packet[0] = (byte)type;
			packet[1] = (byte)index;
			this.encodeChannelId(packet);
		}

		// Token: 0x06003871 RID: 14449 RVA: 0x0010B480 File Offset: 0x00109680
		[Obsolete]
		public int getCall(string name)
		{
			this.buildCallArrayIfDirty();
			for (int i = 0; i < this.calls.Length; i++)
			{
				if (this.calls[i].legacyMethodName == name)
				{
					return i;
				}
			}
			CommandWindow.LogError("Failed to find a method named: " + name);
			return -1;
		}

		// Token: 0x06003872 RID: 14450 RVA: 0x0010B4CE File Offset: 0x001096CE
		private void OnDestroy()
		{
			if (this.id != 0)
			{
				Provider.closeChannel(this);
			}
		}

		/// <summary>
		/// If changing header size remember to update PlayerManager and allocPlayerChannelId.
		/// </summary>
		// Token: 0x04002165 RID: 8549
		public const int CHANNEL_ID_HEADER_SIZE = 1;

		// Token: 0x04002166 RID: 8550
		public const int RPC_HEADER_SIZE = 2;

		// Token: 0x04002167 RID: 8551
		[Obsolete]
		public const int VOICE_HEADER_SIZE = 3;

		/// <summary>
		/// How far to shift compressed voice data.
		/// </summary>
		// Token: 0x04002168 RID: 8552
		[Obsolete]
		public const int VOICE_DATA_OFFSET = 6;

		// Token: 0x0400216A RID: 8554
		public int id;

		// Token: 0x0400216B RID: 8555
		public SteamPlayer owner;

		/// <summary>
		/// Don't use this. Originally added so that Rocketmod didn't have to inject into the game's assembly.
		/// </summary>
		// Token: 0x0400216C RID: 8556
		[Obsolete("Will be deprecated soon. Please discuss on the issue tracker and we will find an alternative.")]
		public static TriggerReceive onTriggerReceive;

		// Token: 0x0400216D RID: 8557
		private static bool warnedAboutTriggerReceive;

		/// <summary>
		/// Don't use this. Originally added so that Rocketmod didn't have to inject into the game's assembly.
		/// </summary>
		// Token: 0x0400216E RID: 8558
		[Obsolete("Will be deprecated soon. Please discuss on the issue tracker and we will find an alternative.")]
		public static TriggerSend onTriggerSend;

		// Token: 0x0400216F RID: 8559
		private static bool warnedAboutTriggerSend;

		/// <summary>
		/// Does array of RPCs need to be rebuilt?
		/// </summary>
		// Token: 0x04002170 RID: 8560
		private bool callArrayDirty = true;

		// Token: 0x04002171 RID: 8561
		private static List<SteamChannelMethod> workingCalls = new List<SteamChannelMethod>();

		// Token: 0x04002172 RID: 8562
		private static List<Component> workingComponents = new List<Component>();

		// Token: 0x04002173 RID: 8563
		[Obsolete("Renamed to IsLocalPlayer")]
		public bool isOwner;
	}
}
