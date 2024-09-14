using System;
using System.Collections.Generic;
using SDG.Framework.Devkit;
using SDG.NetPak;
using SDG.NetTransport;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020005AA RID: 1450
	public class ZombieManager : SteamCaller
	{
		/// <summary>
		/// Exposed for Rocket transition to modules backwards compatibility.
		/// </summary>
		// Token: 0x170008A0 RID: 2208
		// (get) Token: 0x06002EE1 RID: 12001 RVA: 0x000CD6E6 File Offset: 0x000CB8E6
		public static ZombieManager instance
		{
			get
			{
				return ZombieManager.manager;
			}
		}

		// Token: 0x170008A1 RID: 2209
		// (get) Token: 0x06002EE2 RID: 12002 RVA: 0x000CD6ED File Offset: 0x000CB8ED
		public static ZombieRegion[] regions
		{
			get
			{
				return ZombieManager._regions;
			}
		}

		// Token: 0x170008A2 RID: 2210
		// (get) Token: 0x06002EE3 RID: 12003 RVA: 0x000CD6F4 File Offset: 0x000CB8F4
		public static List<Zombie> tickingZombies
		{
			get
			{
				return ZombieManager._tickingZombies;
			}
		}

		// Token: 0x170008A3 RID: 2211
		// (get) Token: 0x06002EE4 RID: 12004 RVA: 0x000CD6FB File Offset: 0x000CB8FB
		public static bool canSpareWanderer
		{
			get
			{
				return ZombieManager.wanderingCount < 8 && ZombieManager.tickingZombies.Count < 50;
			}
		}

		// Token: 0x170008A4 RID: 2212
		// (get) Token: 0x06002EE5 RID: 12005 RVA: 0x000CD715 File Offset: 0x000CB915
		public static bool waveReady
		{
			get
			{
				return ZombieManager._waveReady;
			}
		}

		// Token: 0x170008A5 RID: 2213
		// (get) Token: 0x06002EE6 RID: 12006 RVA: 0x000CD71C File Offset: 0x000CB91C
		public static int waveIndex
		{
			get
			{
				return ZombieManager._waveIndex;
			}
		}

		// Token: 0x170008A6 RID: 2214
		// (get) Token: 0x06002EE7 RID: 12007 RVA: 0x000CD723 File Offset: 0x000CB923
		public static int waveRemaining
		{
			get
			{
				return ZombieManager._waveRemaining;
			}
		}

		// Token: 0x06002EE8 RID: 12008 RVA: 0x000CD72C File Offset: 0x000CB92C
		public static void getZombiesInRadius(Vector3 center, float sqrRadius, List<Zombie> result)
		{
			if (ZombieManager.regions == null)
			{
				return;
			}
			byte b;
			if (!LevelNavigation.tryGetNavigation(center, out b))
			{
				return;
			}
			if (ZombieManager.regions[(int)b] == null || ZombieManager.regions[(int)b].zombies == null)
			{
				return;
			}
			for (int i = 0; i < ZombieManager.regions[(int)b].zombies.Count; i++)
			{
				Zombie zombie = ZombieManager.regions[(int)b].zombies[i];
				if (!(zombie == null) && (zombie.transform.position - center).sqrMagnitude < sqrRadius)
				{
					result.Add(zombie);
				}
			}
		}

		// Token: 0x06002EE9 RID: 12009 RVA: 0x000CD7BF File Offset: 0x000CB9BF
		[Obsolete]
		public void tellBeacon(CSteamID steamID, byte reference, bool hasBeacon)
		{
			ZombieManager.ReceiveBeacon(reference, hasBeacon);
		}

		// Token: 0x06002EEA RID: 12010 RVA: 0x000CD7C8 File Offset: 0x000CB9C8
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellBeacon")]
		public static void ReceiveBeacon(byte reference, bool hasBeacon)
		{
			if (ZombieManager.regions == null || (int)reference >= ZombieManager.regions.Length)
			{
				return;
			}
			if (!Provider.isServer && !ZombieManager.regions[(int)reference].isNetworked)
			{
				return;
			}
			ZombieManager.regions[(int)reference].hasBeacon = hasBeacon;
		}

		// Token: 0x06002EEB RID: 12011 RVA: 0x000CD7FF File Offset: 0x000CB9FF
		[Obsolete]
		public void tellWave(CSteamID steamID, bool newWaveReady, int newWave)
		{
			ZombieManager.ReceiveWave(newWaveReady, newWave);
		}

		// Token: 0x06002EEC RID: 12012 RVA: 0x000CD808 File Offset: 0x000CBA08
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellWave")]
		public static void ReceiveWave(bool newWaveReady, int newWave)
		{
			ZombieManager._waveReady = newWaveReady;
			ZombieManager._waveIndex = newWave;
			WaveUpdated waveUpdated = ZombieManager.onWaveUpdated;
			if (waveUpdated == null)
			{
				return;
			}
			waveUpdated(ZombieManager.waveReady, ZombieManager.waveIndex);
		}

		// Token: 0x06002EED RID: 12013 RVA: 0x000CD82F File Offset: 0x000CBA2F
		[Obsolete]
		public void askWave(CSteamID steamID)
		{
		}

		// Token: 0x06002EEE RID: 12014 RVA: 0x000CD831 File Offset: 0x000CBA31
		internal static void SendInitialGlobalState(SteamPlayer client)
		{
			if (Level.info.type == ELevelType.HORDE)
			{
				ZombieManager.SendWave.Invoke(ENetReliability.Reliable, client.transportConnection, ZombieManager.waveReady, ZombieManager.waveIndex);
			}
		}

		// Token: 0x06002EEF RID: 12015 RVA: 0x000CD85C File Offset: 0x000CBA5C
		[Obsolete]
		public void tellZombieAlive(CSteamID steamID, byte reference, ushort id, byte newType, byte newSpeciality, byte newShirt, byte newPants, byte newHat, byte newGear, Vector3 newPosition, byte newAngle)
		{
			ZombieManager.ReceiveZombieAlive(reference, id, newType, newSpeciality, newShirt, newPants, newHat, newGear, newPosition, newAngle);
		}

		// Token: 0x06002EF0 RID: 12016 RVA: 0x000CD880 File Offset: 0x000CBA80
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellZombieAlive")]
		public static void ReceiveZombieAlive(byte reference, ushort id, byte newType, byte newSpeciality, byte newShirt, byte newPants, byte newHat, byte newGear, Vector3 newPosition, byte newAngle)
		{
			if (!Provider.isServer && !ZombieManager.regions[(int)reference].isNetworked)
			{
				return;
			}
			if ((int)id >= ZombieManager.regions[(int)reference].zombies.Count)
			{
				return;
			}
			ZombieManager.regions[(int)reference].zombies[(int)id].tellAlive(newType, newSpeciality, newShirt, newPants, newHat, newGear, newPosition, newAngle);
		}

		// Token: 0x06002EF1 RID: 12017 RVA: 0x000CD8DD File Offset: 0x000CBADD
		[Obsolete]
		public void tellZombieDead(CSteamID steamID, byte reference, ushort id, Vector3 newRagdoll, byte newRagdollEffect)
		{
			ZombieManager.ReceiveZombieDead(reference, id, newRagdoll, (ERagdollEffect)newRagdollEffect);
		}

		// Token: 0x06002EF2 RID: 12018 RVA: 0x000CD8EC File Offset: 0x000CBAEC
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellZombieDead")]
		public static void ReceiveZombieDead(byte reference, ushort id, Vector3 newRagdoll, ERagdollEffect newRagdollEffect)
		{
			if (!Provider.isServer && !ZombieManager.regions[(int)reference].isNetworked)
			{
				return;
			}
			if ((int)id >= ZombieManager.regions[(int)reference].zombies.Count)
			{
				return;
			}
			ZombieManager.regions[(int)reference].zombies[(int)id].tellDead(newRagdoll, newRagdollEffect);
		}

		// Token: 0x06002EF3 RID: 12019 RVA: 0x000CD93D File Offset: 0x000CBB3D
		[Obsolete]
		public void tellZombieStates(CSteamID steamID)
		{
		}

		// Token: 0x06002EF4 RID: 12020 RVA: 0x000CD940 File Offset: 0x000CBB40
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER)]
		public static void ReceiveZombieStates(in ClientInvocationContext context)
		{
			NetPakReader reader = context.reader;
			byte b;
			SystemNetPakReaderEx.ReadUInt8(reader, ref b);
			if ((int)b >= ZombieManager.regions.Length)
			{
				return;
			}
			if (!Provider.isServer && !ZombieManager.regions[(int)b].isNetworked)
			{
				return;
			}
			uint num;
			SystemNetPakReaderEx.ReadUInt32(reader, ref num);
			if (num <= ZombieManager.seq)
			{
				return;
			}
			ZombieManager.seq = num;
			ushort num2;
			SystemNetPakReaderEx.ReadUInt16(reader, ref num2);
			if (num2 < 1)
			{
				return;
			}
			for (ushort num3 = 0; num3 < num2; num3 += 1)
			{
				ushort num4;
				SystemNetPakReaderEx.ReadUInt16(reader, ref num4);
				Vector3 newPosition;
				UnityNetPakReaderEx.ReadClampedVector3(reader, ref newPosition, 13, 7);
				float newYaw;
				SystemNetPakReaderEx.ReadDegrees(reader, ref newYaw, 8);
				if ((int)num4 < ZombieManager.regions[(int)b].zombies.Count)
				{
					ZombieManager.regions[(int)b].zombies[(int)num4].tellState(newPosition, newYaw);
				}
			}
		}

		// Token: 0x06002EF5 RID: 12021 RVA: 0x000CDA05 File Offset: 0x000CBC05
		[Obsolete]
		public void tellZombieSpeciality(CSteamID steamID, byte reference, ushort id, byte speciality)
		{
			ZombieManager.ReceiveZombieSpeciality(reference, id, (EZombieSpeciality)speciality);
		}

		// Token: 0x06002EF6 RID: 12022 RVA: 0x000CDA10 File Offset: 0x000CBC10
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellZombieSpeciality")]
		public static void ReceiveZombieSpeciality(byte reference, ushort id, EZombieSpeciality speciality)
		{
			if (!Provider.isServer && !ZombieManager.regions[(int)reference].isNetworked)
			{
				return;
			}
			if ((int)id >= ZombieManager.regions[(int)reference].zombies.Count)
			{
				return;
			}
			ZombieManager.regions[(int)reference].zombies[(int)id].tellSpeciality(speciality);
		}

		// Token: 0x06002EF7 RID: 12023 RVA: 0x000CDA60 File Offset: 0x000CBC60
		[Obsolete]
		public void askZombieThrow(CSteamID steamID, byte reference, ushort id)
		{
			ZombieManager.ReceiveZombieThrow(reference, id);
		}

		// Token: 0x06002EF8 RID: 12024 RVA: 0x000CDA6C File Offset: 0x000CBC6C
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "askZombieThrow")]
		public static void ReceiveZombieThrow(byte reference, ushort id)
		{
			if (!Provider.isServer && !ZombieManager.regions[(int)reference].isNetworked)
			{
				return;
			}
			if ((int)id >= ZombieManager.regions[(int)reference].zombies.Count)
			{
				return;
			}
			ZombieManager.regions[(int)reference].zombies[(int)id].askThrow();
		}

		// Token: 0x06002EF9 RID: 12025 RVA: 0x000CDABB File Offset: 0x000CBCBB
		[Obsolete]
		public void askZombieBoulder(CSteamID steamID, byte reference, ushort id, Vector3 origin, Vector3 direction)
		{
			ZombieManager.ReceiveZombieBoulder(reference, id, origin, direction);
		}

		// Token: 0x06002EFA RID: 12026 RVA: 0x000CDAC8 File Offset: 0x000CBCC8
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "askZombieBoulder")]
		public static void ReceiveZombieBoulder(byte reference, ushort id, Vector3 origin, Vector3 direction)
		{
			if (!Provider.isServer && !ZombieManager.regions[(int)reference].isNetworked)
			{
				return;
			}
			if ((int)id >= ZombieManager.regions[(int)reference].zombies.Count)
			{
				return;
			}
			ZombieManager.regions[(int)reference].zombies[(int)id].askBoulder(origin, direction);
		}

		// Token: 0x06002EFB RID: 12027 RVA: 0x000CDB19 File Offset: 0x000CBD19
		[Obsolete]
		public void askZombieSpit(CSteamID steamID, byte reference, ushort id)
		{
			ZombieManager.ReceiveZombieSpit(reference, id);
		}

		// Token: 0x06002EFC RID: 12028 RVA: 0x000CDB24 File Offset: 0x000CBD24
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "askZombieSpit")]
		public static void ReceiveZombieSpit(byte reference, ushort id)
		{
			if (!Provider.isServer && !ZombieManager.regions[(int)reference].isNetworked)
			{
				return;
			}
			if ((int)id >= ZombieManager.regions[(int)reference].zombies.Count)
			{
				return;
			}
			ZombieManager.regions[(int)reference].zombies[(int)id].askSpit();
		}

		// Token: 0x06002EFD RID: 12029 RVA: 0x000CDB73 File Offset: 0x000CBD73
		[Obsolete]
		public void askZombieCharge(CSteamID steamID, byte reference, ushort id)
		{
			ZombieManager.ReceiveZombieCharge(reference, id);
		}

		// Token: 0x06002EFE RID: 12030 RVA: 0x000CDB7C File Offset: 0x000CBD7C
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "askZombieCharge")]
		public static void ReceiveZombieCharge(byte reference, ushort id)
		{
			if (!Provider.isServer && !ZombieManager.regions[(int)reference].isNetworked)
			{
				return;
			}
			if ((int)id >= ZombieManager.regions[(int)reference].zombies.Count)
			{
				return;
			}
			ZombieManager.regions[(int)reference].zombies[(int)id].askCharge();
		}

		// Token: 0x06002EFF RID: 12031 RVA: 0x000CDBCB File Offset: 0x000CBDCB
		[Obsolete]
		public void askZombieStomp(CSteamID steamID, byte reference, ushort id)
		{
			ZombieManager.ReceiveZombieStomp(reference, id);
		}

		// Token: 0x06002F00 RID: 12032 RVA: 0x000CDBD4 File Offset: 0x000CBDD4
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "askZombieStomp")]
		public static void ReceiveZombieStomp(byte reference, ushort id)
		{
			if (!Provider.isServer && !ZombieManager.regions[(int)reference].isNetworked)
			{
				return;
			}
			if ((int)id >= ZombieManager.regions[(int)reference].zombies.Count)
			{
				return;
			}
			ZombieManager.regions[(int)reference].zombies[(int)id].askStomp();
		}

		// Token: 0x06002F01 RID: 12033 RVA: 0x000CDC23 File Offset: 0x000CBE23
		[Obsolete]
		public void askZombieBreath(CSteamID steamID, byte reference, ushort id)
		{
			ZombieManager.ReceiveZombieBreath(reference, id);
		}

		// Token: 0x06002F02 RID: 12034 RVA: 0x000CDC2C File Offset: 0x000CBE2C
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "askZombieBreath")]
		public static void ReceiveZombieBreath(byte reference, ushort id)
		{
			if (!Provider.isServer && !ZombieManager.regions[(int)reference].isNetworked)
			{
				return;
			}
			if ((int)id >= ZombieManager.regions[(int)reference].zombies.Count)
			{
				return;
			}
			ZombieManager.regions[(int)reference].zombies[(int)id].askBreath();
		}

		// Token: 0x06002F03 RID: 12035 RVA: 0x000CDC7B File Offset: 0x000CBE7B
		[Obsolete]
		public void askZombieAcid(CSteamID steamID, byte reference, ushort id, Vector3 origin, Vector3 direction)
		{
			ZombieManager.ReceiveZombieAcid(reference, id, origin, direction);
		}

		// Token: 0x06002F04 RID: 12036 RVA: 0x000CDC88 File Offset: 0x000CBE88
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "askZombieAcid")]
		public static void ReceiveZombieAcid(byte reference, ushort id, Vector3 origin, Vector3 direction)
		{
			if (!Provider.isServer && !ZombieManager.regions[(int)reference].isNetworked)
			{
				return;
			}
			if ((int)id >= ZombieManager.regions[(int)reference].zombies.Count)
			{
				return;
			}
			ZombieManager.regions[(int)reference].zombies[(int)id].askAcid(origin, direction);
		}

		// Token: 0x06002F05 RID: 12037 RVA: 0x000CDCD9 File Offset: 0x000CBED9
		[Obsolete]
		public void askZombieSpark(CSteamID steamID, byte reference, ushort id, Vector3 target)
		{
			ZombieManager.ReceiveZombieSpark(reference, id, target);
		}

		// Token: 0x06002F06 RID: 12038 RVA: 0x000CDCE4 File Offset: 0x000CBEE4
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "askZombieSpark")]
		public static void ReceiveZombieSpark(byte reference, ushort id, Vector3 target)
		{
			if (!Provider.isServer && !ZombieManager.regions[(int)reference].isNetworked)
			{
				return;
			}
			if ((int)id >= ZombieManager.regions[(int)reference].zombies.Count)
			{
				return;
			}
			ZombieManager.regions[(int)reference].zombies[(int)id].askSpark(target);
		}

		// Token: 0x06002F07 RID: 12039 RVA: 0x000CDD34 File Offset: 0x000CBF34
		[Obsolete]
		public void askZombieAttack(CSteamID steamID, byte reference, ushort id, byte attack)
		{
			ZombieManager.ReceiveZombieAttack(reference, id, attack);
		}

		// Token: 0x06002F08 RID: 12040 RVA: 0x000CDD40 File Offset: 0x000CBF40
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "askZombieAttack")]
		public static void ReceiveZombieAttack(byte reference, ushort id, byte attack)
		{
			if (!Provider.isServer && !ZombieManager.regions[(int)reference].isNetworked)
			{
				return;
			}
			if ((int)id >= ZombieManager.regions[(int)reference].zombies.Count)
			{
				return;
			}
			ZombieManager.regions[(int)reference].zombies[(int)id].askAttack(attack);
		}

		// Token: 0x06002F09 RID: 12041 RVA: 0x000CDD90 File Offset: 0x000CBF90
		[Obsolete]
		public void askZombieStartle(CSteamID steamID, byte reference, ushort id, byte startle)
		{
			ZombieManager.ReceiveZombieStartle(reference, id, startle);
		}

		// Token: 0x06002F0A RID: 12042 RVA: 0x000CDD9C File Offset: 0x000CBF9C
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "askZombieStartle")]
		public static void ReceiveZombieStartle(byte reference, ushort id, byte startle)
		{
			if (!Provider.isServer && !ZombieManager.regions[(int)reference].isNetworked)
			{
				return;
			}
			if ((int)id >= ZombieManager.regions[(int)reference].zombies.Count)
			{
				return;
			}
			ZombieManager.regions[(int)reference].zombies[(int)id].askStartle(startle);
		}

		// Token: 0x06002F0B RID: 12043 RVA: 0x000CDDEC File Offset: 0x000CBFEC
		[Obsolete]
		public void askZombieStun(CSteamID steamID, byte reference, ushort id, byte stun)
		{
			ZombieManager.ReceiveZombieStun(reference, id, stun);
		}

		// Token: 0x06002F0C RID: 12044 RVA: 0x000CDDF8 File Offset: 0x000CBFF8
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "askZombieStun")]
		public static void ReceiveZombieStun(byte reference, ushort id, byte stun)
		{
			if (!Provider.isServer && !ZombieManager.regions[(int)reference].isNetworked)
			{
				return;
			}
			if ((int)id >= ZombieManager.regions[(int)reference].zombies.Count)
			{
				return;
			}
			ZombieManager.regions[(int)reference].zombies[(int)id].askStun(stun);
		}

		// Token: 0x06002F0D RID: 12045 RVA: 0x000CDE48 File Offset: 0x000CC048
		[Obsolete]
		public void tellZombies(CSteamID steamID)
		{
		}

		// Token: 0x06002F0E RID: 12046 RVA: 0x000CDE4C File Offset: 0x000CC04C
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER)]
		public static void ReceiveZombies(in ClientInvocationContext context)
		{
			NetPakReader reader = context.reader;
			byte b;
			SystemNetPakReaderEx.ReadUInt8(reader, ref b);
			if (ZombieManager.regions[(int)b].isNetworked)
			{
				return;
			}
			ZombieManager.regions[(int)b].isNetworked = true;
			bool hasBeacon;
			reader.ReadBit(ref hasBeacon);
			ushort num;
			SystemNetPakReaderEx.ReadUInt16(reader, ref num);
			for (ushort num2 = 0; num2 < num; num2 += 1)
			{
				byte type;
				SystemNetPakReaderEx.ReadUInt8(reader, ref type);
				byte speciality;
				SystemNetPakReaderEx.ReadUInt8(reader, ref speciality);
				byte shirt;
				SystemNetPakReaderEx.ReadUInt8(reader, ref shirt);
				byte pants;
				SystemNetPakReaderEx.ReadUInt8(reader, ref pants);
				byte hat;
				SystemNetPakReaderEx.ReadUInt8(reader, ref hat);
				byte gear;
				SystemNetPakReaderEx.ReadUInt8(reader, ref gear);
				byte move;
				SystemNetPakReaderEx.ReadUInt8(reader, ref move);
				byte idle;
				SystemNetPakReaderEx.ReadUInt8(reader, ref idle);
				Vector3 position;
				UnityNetPakReaderEx.ReadClampedVector3(reader, ref position, 13, 7);
				float angle;
				SystemNetPakReaderEx.ReadDegrees(reader, ref angle, 8);
				bool isDead;
				reader.ReadBit(ref isDead);
				ZombieManager.manager.addZombie(b, type, speciality, shirt, pants, hat, gear, move, idle, position, angle, isDead);
			}
			ZombieManager.regions[(int)b].hasBeacon = hasBeacon;
		}

		// Token: 0x06002F0F RID: 12047 RVA: 0x000CDF43 File Offset: 0x000CC143
		[Obsolete]
		public void askZombies(CSteamID steamID, byte bound)
		{
		}

		// Token: 0x06002F10 RID: 12048 RVA: 0x000CDF48 File Offset: 0x000CC148
		private void SendZombiesToPlayer(ITransportConnection transportConnection, byte bound)
		{
			ZombieRegion region = ZombieManager.regions[(int)bound];
			ZombieManager.SendZombies.Invoke(ENetReliability.Reliable, transportConnection, delegate(NetPakWriter writer)
			{
				SystemNetPakWriterEx.WriteUInt8(writer, bound);
				writer.WriteBit(region.hasBeacon);
				SystemNetPakWriterEx.WriteUInt16(writer, (ushort)region.zombies.Count);
				ushort num = 0;
				while ((int)num < region.zombies.Count)
				{
					Zombie zombie = region.zombies[(int)num];
					SystemNetPakWriterEx.WriteUInt8(writer, zombie.type);
					SystemNetPakWriterEx.WriteUInt8(writer, (byte)zombie.speciality);
					SystemNetPakWriterEx.WriteUInt8(writer, zombie.shirt);
					SystemNetPakWriterEx.WriteUInt8(writer, zombie.pants);
					SystemNetPakWriterEx.WriteUInt8(writer, zombie.hat);
					SystemNetPakWriterEx.WriteUInt8(writer, zombie.gear);
					SystemNetPakWriterEx.WriteUInt8(writer, zombie.move);
					SystemNetPakWriterEx.WriteUInt8(writer, zombie.idle);
					UnityNetPakWriterEx.WriteClampedVector3(writer, zombie.transform.position, 13, 7);
					SystemNetPakWriterEx.WriteDegrees(writer, zombie.transform.eulerAngles.y, 8);
					writer.WriteBit(zombie.isDead);
					num += 1;
				}
			});
		}

		// Token: 0x06002F11 RID: 12049 RVA: 0x000CDF8C File Offset: 0x000CC18C
		public static void sendZombieAlive(Zombie zombie, byte newType, byte newSpeciality, byte newShirt, byte newPants, byte newHat, byte newGear, Vector3 newPosition, byte newAngle)
		{
			ZombieManager.SendZombieAlive.InvokeAndLoopback(ENetReliability.Reliable, ZombieManager.GatherRemoteClientConnections(zombie.bound), zombie.bound, zombie.id, newType, newSpeciality, newShirt, newPants, newHat, newGear, newPosition, newAngle);
			ZombieLifeUpdated onZombieLifeUpdated = ZombieManager.regions[(int)zombie.bound].onZombieLifeUpdated;
			if (onZombieLifeUpdated == null)
			{
				return;
			}
			onZombieLifeUpdated(zombie);
		}

		// Token: 0x06002F12 RID: 12050 RVA: 0x000CDFE4 File Offset: 0x000CC1E4
		public static void sendZombieDead(Zombie zombie, Vector3 newRagdoll, ERagdollEffect newRagdollEffect = ERagdollEffect.NONE)
		{
			ZombieManager.SendZombieDead.InvokeAndLoopback(ENetReliability.Reliable, ZombieManager.GatherRemoteClientConnections(zombie.bound), zombie.bound, zombie.id, newRagdoll, newRagdollEffect);
			ZombieLifeUpdated onZombieLifeUpdated = ZombieManager.regions[(int)zombie.bound].onZombieLifeUpdated;
			if (onZombieLifeUpdated == null)
			{
				return;
			}
			onZombieLifeUpdated(zombie);
		}

		// Token: 0x06002F13 RID: 12051 RVA: 0x000CE031 File Offset: 0x000CC231
		public static void sendZombieSpeciality(Zombie zombie, EZombieSpeciality speciality)
		{
			ZombieManager.SendZombieSpeciality.InvokeAndLoopback(ENetReliability.Unreliable, ZombieManager.GatherRemoteClientConnections(zombie.bound), zombie.bound, zombie.id, speciality);
		}

		// Token: 0x06002F14 RID: 12052 RVA: 0x000CE056 File Offset: 0x000CC256
		public static void sendZombieThrow(Zombie zombie)
		{
			ZombieManager.SendZombieThrow.InvokeAndLoopback(ENetReliability.Unreliable, ZombieManager.GatherRemoteClientConnections(zombie.bound), zombie.bound, zombie.id);
		}

		// Token: 0x06002F15 RID: 12053 RVA: 0x000CE07A File Offset: 0x000CC27A
		public static void sendZombieSpit(Zombie zombie)
		{
			ZombieManager.SendZombieSpit.InvokeAndLoopback(ENetReliability.Unreliable, ZombieManager.GatherRemoteClientConnections(zombie.bound), zombie.bound, zombie.id);
		}

		// Token: 0x06002F16 RID: 12054 RVA: 0x000CE09E File Offset: 0x000CC29E
		public static void sendZombieCharge(Zombie zombie)
		{
			ZombieManager.SendZombieCharge.InvokeAndLoopback(ENetReliability.Unreliable, ZombieManager.GatherRemoteClientConnections(zombie.bound), zombie.bound, zombie.id);
		}

		// Token: 0x06002F17 RID: 12055 RVA: 0x000CE0C2 File Offset: 0x000CC2C2
		public static void sendZombieStomp(Zombie zombie)
		{
			ZombieManager.SendZombieStomp.InvokeAndLoopback(ENetReliability.Unreliable, ZombieManager.GatherRemoteClientConnections(zombie.bound), zombie.bound, zombie.id);
		}

		// Token: 0x06002F18 RID: 12056 RVA: 0x000CE0E6 File Offset: 0x000CC2E6
		public static void sendZombieBreath(Zombie zombie)
		{
			ZombieManager.SendZombieBreath.InvokeAndLoopback(ENetReliability.Unreliable, ZombieManager.GatherRemoteClientConnections(zombie.bound), zombie.bound, zombie.id);
		}

		// Token: 0x06002F19 RID: 12057 RVA: 0x000CE10A File Offset: 0x000CC30A
		public static void sendZombieBoulder(Zombie zombie, Vector3 origin, Vector3 direction)
		{
			ZombieManager.SendZombieBoulder.InvokeAndLoopback(ENetReliability.Unreliable, ZombieManager.GatherRemoteClientConnections(zombie.bound), zombie.bound, zombie.id, origin, direction);
		}

		// Token: 0x06002F1A RID: 12058 RVA: 0x000CE130 File Offset: 0x000CC330
		public static void sendZombieAcid(Zombie zombie, Vector3 origin, Vector3 direction)
		{
			ZombieManager.SendZombieAcid.InvokeAndLoopback(ENetReliability.Unreliable, ZombieManager.GatherRemoteClientConnections(zombie.bound), zombie.bound, zombie.id, origin, direction);
		}

		// Token: 0x06002F1B RID: 12059 RVA: 0x000CE156 File Offset: 0x000CC356
		public static void sendZombieSpark(Zombie zombie, Vector3 target)
		{
			ZombieManager.SendZombieSpark.Invoke(ENetReliability.Unreliable, ZombieManager.GatherClientConnections(zombie.bound), zombie.bound, zombie.id, target);
		}

		// Token: 0x06002F1C RID: 12060 RVA: 0x000CE17B File Offset: 0x000CC37B
		public static void sendZombieAttack(Zombie zombie, byte attack)
		{
			ZombieManager.SendZombieAttack.InvokeAndLoopback(ENetReliability.Unreliable, ZombieManager.GatherRemoteClientConnections(zombie.bound), zombie.bound, zombie.id, attack);
		}

		// Token: 0x06002F1D RID: 12061 RVA: 0x000CE1A0 File Offset: 0x000CC3A0
		public static void sendZombieStartle(Zombie zombie, byte startle)
		{
			ZombieManager.SendZombieStartle.InvokeAndLoopback(ENetReliability.Unreliable, ZombieManager.GatherRemoteClientConnections(zombie.bound), zombie.bound, zombie.id, startle);
		}

		// Token: 0x06002F1E RID: 12062 RVA: 0x000CE1C5 File Offset: 0x000CC3C5
		public static void sendZombieStun(Zombie zombie, byte stun)
		{
			ZombieManager.SendZombieStun.InvokeAndLoopback(ENetReliability.Unreliable, ZombieManager.GatherRemoteClientConnections(zombie.bound), zombie.bound, zombie.id, stun);
		}

		// Token: 0x06002F1F RID: 12063 RVA: 0x000CE1EC File Offset: 0x000CC3EC
		public static void dropLoot(Zombie zombie)
		{
			int num;
			if (zombie.isBoss || zombie.speciality == EZombieSpeciality.BOSS_ALL)
			{
				num = Random.Range((int)Provider.modeConfigData.Zombies.Min_Boss_Drops, (int)(Provider.modeConfigData.Zombies.Max_Boss_Drops + 1U));
			}
			else if (zombie.isMega)
			{
				num = Random.Range((int)Provider.modeConfigData.Zombies.Min_Mega_Drops, (int)(Provider.modeConfigData.Zombies.Max_Mega_Drops + 1U));
			}
			else
			{
				num = Random.Range((int)Provider.modeConfigData.Zombies.Min_Drops, (int)(Provider.modeConfigData.Zombies.Max_Drops + 1U));
			}
			num = Mathf.Clamp(num, 0, 100);
			if (LevelZombies.tables[(int)zombie.type].isMega)
			{
				ZombieManager.regions[(int)zombie.bound].lastMega = Time.realtimeSinceStartup;
				ZombieManager.regions[(int)zombie.bound].hasMega = false;
			}
			if (num > 1 || Random.value < Provider.modeConfigData.Zombies.Loot_Chance)
			{
				if (LevelZombies.tables[(int)zombie.type].lootID != 0)
				{
					for (int i = 0; i < num; i++)
					{
						ushort num2 = SpawnTableTool.ResolveLegacyId(LevelZombies.tables[(int)zombie.type].lootID, EAssetType.ITEM, new Func<string>(ZombieManager.OnGetZombieLootSpawnTableErrorContext));
						if (num2 != 0)
						{
							ItemManager.dropItem(new Item(num2, EItemOrigin.WORLD), zombie.transform.position, false, true, true);
						}
					}
					return;
				}
				if ((int)LevelZombies.tables[(int)zombie.type].lootIndex < LevelItems.tables.Count)
				{
					for (int j = 0; j < num; j++)
					{
						ushort item = LevelItems.getItem(LevelZombies.tables[(int)zombie.type].lootIndex);
						if (item != 0)
						{
							ItemManager.dropItem(new Item(item, EItemOrigin.WORLD), zombie.transform.position, false, true, true);
						}
					}
				}
			}
		}

		// Token: 0x06002F20 RID: 12064 RVA: 0x000CE3C0 File Offset: 0x000CC5C0
		private static string OnGetZombieLootSpawnTableErrorContext()
		{
			return "zombie loot";
		}

		// Token: 0x06002F21 RID: 12065 RVA: 0x000CE3C8 File Offset: 0x000CC5C8
		public void addZombie(byte bound, byte type, byte speciality, byte shirt, byte pants, byte hat, byte gear, byte move, byte idle, Vector3 position, float angle, bool isDead)
		{
			Quaternion rotation = Quaternion.Euler(0f, angle, 0f);
			GameObject original = ZombieManager.dedicatedZombiePrefab;
			GameObject gameObject = Object.Instantiate<GameObject>(original, position, rotation);
			gameObject.name = "Zombie";
			Zombie component = gameObject.GetComponent<Zombie>();
			component.id = (ushort)ZombieManager.regions[(int)bound].zombies.Count;
			component.speciality = (EZombieSpeciality)speciality;
			component.bound = bound;
			component.zombieRegion = ZombieManager.regions[(int)bound];
			component.type = type;
			component.shirt = shirt;
			component.pants = pants;
			component.hat = hat;
			component.gear = gear;
			component.move = move;
			component.idle = idle;
			component.isDead = isDead;
			component.init();
			ZombieManager.regions[(int)bound].zombies.Add(component);
		}

		// Token: 0x06002F22 RID: 12066 RVA: 0x000CE498 File Offset: 0x000CC698
		public static Zombie getZombie(Vector3 point, ushort id)
		{
			byte b;
			if (!LevelNavigation.tryGetBounds(point, out b))
			{
				return null;
			}
			if ((int)id >= ZombieManager.regions[(int)b].zombies.Count)
			{
				return null;
			}
			if (ZombieManager.regions[(int)b].zombies[(int)id].isDead)
			{
				return null;
			}
			return ZombieManager.regions[(int)b].zombies[(int)id];
		}

		/// <summary>
		/// Find difficulty asset (if valid) for navigation bound index.
		/// </summary>
		// Token: 0x06002F23 RID: 12067 RVA: 0x000CE4F4 File Offset: 0x000CC6F4
		public static ZombieDifficultyAsset getDifficultyInBound(byte bound)
		{
			if ((int)bound < LevelNavigation.flagData.Count)
			{
				return LevelNavigation.flagData[(int)bound].resolveDifficulty();
			}
			return null;
		}

		// Token: 0x06002F24 RID: 12068 RVA: 0x000CE518 File Offset: 0x000CC718
		private static EZombieSpeciality generateZombieSpeciality(byte bound, ZombieTable table)
		{
			ZombieDifficultyAsset zombieDifficultyAsset = ZombieManager.getDifficultyInBound(bound);
			if (zombieDifficultyAsset == null || !zombieDifficultyAsset.Overrides_Spawn_Chance)
			{
				zombieDifficultyAsset = table.resolveDifficulty();
			}
			ZombieManager.zombieSpecialityTable.clear();
			if (zombieDifficultyAsset != null && zombieDifficultyAsset.Overrides_Spawn_Chance)
			{
				ZombieManager.zombieSpecialityTable.add(EZombieSpeciality.CRAWLER, zombieDifficultyAsset.Crawler_Chance);
				ZombieManager.zombieSpecialityTable.add(EZombieSpeciality.SPRINTER, zombieDifficultyAsset.Sprinter_Chance);
				ZombieManager.zombieSpecialityTable.add(EZombieSpeciality.FLANKER_FRIENDLY, zombieDifficultyAsset.Flanker_Chance);
				ZombieManager.zombieSpecialityTable.add(EZombieSpeciality.BURNER, zombieDifficultyAsset.Burner_Chance);
				ZombieManager.zombieSpecialityTable.add(EZombieSpeciality.ACID, zombieDifficultyAsset.Acid_Chance);
				ZombieManager.zombieSpecialityTable.add(EZombieSpeciality.BOSS_ELECTRIC, zombieDifficultyAsset.Boss_Electric_Chance);
				ZombieManager.zombieSpecialityTable.add(EZombieSpeciality.BOSS_WIND, zombieDifficultyAsset.Boss_Wind_Chance);
				ZombieManager.zombieSpecialityTable.add(EZombieSpeciality.BOSS_FIRE, zombieDifficultyAsset.Boss_Fire_Chance);
				ZombieManager.zombieSpecialityTable.add(EZombieSpeciality.SPIRIT, zombieDifficultyAsset.Spirit_Chance);
				if (Level.isLoaded && LightingManager.isNighttime)
				{
					ZombieManager.zombieSpecialityTable.add(EZombieSpeciality.DL_RED_VOLATILE, zombieDifficultyAsset.DL_Red_Volatile_Chance);
					ZombieManager.zombieSpecialityTable.add(EZombieSpeciality.DL_BLUE_VOLATILE, zombieDifficultyAsset.DL_Blue_Volatile_Chance);
				}
				ZombieManager.zombieSpecialityTable.add(EZombieSpeciality.BOSS_ELVER_STOMPER, zombieDifficultyAsset.Boss_Elver_Stomper_Chance);
				ZombieManager.zombieSpecialityTable.add(EZombieSpeciality.BOSS_KUWAIT, zombieDifficultyAsset.Boss_Kuwait_Chance);
			}
			else
			{
				ZombieManager.zombieSpecialityTable.add(EZombieSpeciality.CRAWLER, Provider.modeConfigData.Zombies.Crawler_Chance);
				ZombieManager.zombieSpecialityTable.add(EZombieSpeciality.SPRINTER, Provider.modeConfigData.Zombies.Sprinter_Chance);
				ZombieManager.zombieSpecialityTable.add(EZombieSpeciality.FLANKER_FRIENDLY, Provider.modeConfigData.Zombies.Flanker_Chance);
				ZombieManager.zombieSpecialityTable.add(EZombieSpeciality.BURNER, Provider.modeConfigData.Zombies.Burner_Chance);
				ZombieManager.zombieSpecialityTable.add(EZombieSpeciality.ACID, Provider.modeConfigData.Zombies.Acid_Chance);
				ZombieManager.zombieSpecialityTable.add(EZombieSpeciality.BOSS_ELECTRIC, Provider.modeConfigData.Zombies.Boss_Electric_Chance);
				ZombieManager.zombieSpecialityTable.add(EZombieSpeciality.BOSS_WIND, Provider.modeConfigData.Zombies.Boss_Wind_Chance);
				ZombieManager.zombieSpecialityTable.add(EZombieSpeciality.BOSS_FIRE, Provider.modeConfigData.Zombies.Boss_Fire_Chance);
				ZombieManager.zombieSpecialityTable.add(EZombieSpeciality.SPIRIT, Provider.modeConfigData.Zombies.Spirit_Chance);
				if (Level.isLoaded && LightingManager.isNighttime)
				{
					ZombieManager.zombieSpecialityTable.add(EZombieSpeciality.DL_RED_VOLATILE, Provider.modeConfigData.Zombies.DL_Red_Volatile_Chance);
					ZombieManager.zombieSpecialityTable.add(EZombieSpeciality.DL_BLUE_VOLATILE, Provider.modeConfigData.Zombies.DL_Blue_Volatile_Chance);
				}
				ZombieManager.zombieSpecialityTable.add(EZombieSpeciality.BOSS_ELVER_STOMPER, Provider.modeConfigData.Zombies.Boss_Elver_Stomper_Chance);
				ZombieManager.zombieSpecialityTable.add(EZombieSpeciality.BOSS_KUWAIT, Provider.modeConfigData.Zombies.Boss_Kuwait_Chance);
			}
			ZombieManager.zombieSpecialityTable.add(EZombieSpeciality.NORMAL, 1f - ZombieManager.zombieSpecialityTable.totalWeight);
			return ZombieManager.zombieSpecialityTable.get();
		}

		/// <summary>
		/// When zombie falls outside the map it needs a replacement spawnpoint within the same navmesh area.
		/// </summary>
		// Token: 0x06002F25 RID: 12069 RVA: 0x000CE7E0 File Offset: 0x000CC9E0
		private static ZombieSpawnpoint getReplacementSpawnpointInBound(byte bound)
		{
			if ((int)bound < LevelZombies.zombies.Length)
			{
				List<ZombieSpawnpoint> list = LevelZombies.zombies[(int)bound];
				if (list.Count > 0)
				{
					int num = Random.Range(0, list.Count);
					return list[num];
				}
				UnturnedLog.warn("Unable to replace zombie because spawns are empty in bound {0}", new object[]
				{
					bound
				});
			}
			else
			{
				UnturnedLog.warn("Unable to replace zombie because bound {0} is out of range", new object[]
				{
					bound
				});
			}
			return null;
		}

		/// <summary>
		/// Find replacement spawnpoint for a zombie and teleport it there.
		/// </summary>
		// Token: 0x06002F26 RID: 12070 RVA: 0x000CE854 File Offset: 0x000CCA54
		public static void teleportZombieBackIntoMap(Zombie zombie)
		{
			ZombieSpawnpoint replacementSpawnpointInBound = ZombieManager.getReplacementSpawnpointInBound(zombie.bound);
			if (replacementSpawnpointInBound != null)
			{
				EffectAsset effectAsset = ZombieManager.Souls_1_Ref.Find();
				if (effectAsset != null)
				{
					EffectManager.triggerEffect(new TriggerEffectParameters(effectAsset)
					{
						relevantDistance = 16f,
						position = zombie.transform.position + Vector3.up
					});
				}
				Vector3 position = replacementSpawnpointInBound.point + Vector3.up;
				zombie.transform.position = position;
			}
		}

		// Token: 0x06002F27 RID: 12071 RVA: 0x000CE8D8 File Offset: 0x000CCAD8
		public void generateZombies(byte bound)
		{
			if (LevelNavigation.bounds.Count == 0 || LevelZombies.zombies.Length == 0 || LevelNavigation.bounds.Count != LevelZombies.zombies.Length)
			{
				return;
			}
			List<ZombieSpawnpoint> list = LevelZombies.zombies[(int)bound];
			if (list.Count > 0)
			{
				ZombieRegion zombieRegion = ZombieManager.regions[(int)bound];
				zombieRegion.alive = 0;
				List<ZombieSpawnpoint> list2 = new List<ZombieSpawnpoint>();
				foreach (ZombieSpawnpoint zombieSpawnpoint in list)
				{
					if (SafezoneManager.checkPointValid(zombieSpawnpoint.point))
					{
						list2.Add(zombieSpawnpoint);
					}
				}
				int num;
				int num2;
				if (Level.info.type == ELevelType.HORDE)
				{
					num = 40;
					num2 = -1;
				}
				else
				{
					int b = Mathf.CeilToInt((float)list.Count * Provider.modeConfigData.Zombies.Spawn_Chance);
					num = Mathf.Min((int)LevelNavigation.flagData[(int)bound].maxZombies, b);
					num2 = LevelNavigation.flagData[(int)bound].maxBossZombies;
				}
				while (list2.Count > 0 && zombieRegion.zombies.Count < num)
				{
					int num3 = Random.Range(0, list2.Count);
					ZombieSpawnpoint zombieSpawnpoint2 = list2[num3];
					list2.RemoveAt(num3);
					byte type = zombieSpawnpoint2.type;
					ZombieTable zombieTable = LevelZombies.tables[(int)type];
					if (this.canRegionSpawnZombiesFromTable(zombieRegion, zombieTable))
					{
						EZombieSpeciality ezombieSpeciality = EZombieSpeciality.NORMAL;
						if (zombieTable.isMega)
						{
							zombieRegion.lastMega = Time.realtimeSinceStartup;
							zombieRegion.hasMega = true;
							ezombieSpeciality = EZombieSpeciality.MEGA;
						}
						else if (Level.info.type == ELevelType.SURVIVAL)
						{
							ezombieSpeciality = ZombieManager.generateZombieSpeciality(bound, zombieTable);
						}
						if (num2 < 0 || !ezombieSpeciality.IsBoss() || zombieRegion.aliveBossZombieCount < num2)
						{
							if (zombieRegion.hasBeacon)
							{
								BeaconManager.checkBeacon(bound).spawnRemaining();
							}
							byte shirt = byte.MaxValue;
							if (zombieTable.slots[0].table.Count > 0 && Random.value < zombieTable.slots[0].chance)
							{
								shirt = (byte)Random.Range(0, zombieTable.slots[0].table.Count);
							}
							byte pants = byte.MaxValue;
							if (zombieTable.slots[1].table.Count > 0 && Random.value < zombieTable.slots[1].chance)
							{
								pants = (byte)Random.Range(0, zombieTable.slots[1].table.Count);
							}
							byte hat = byte.MaxValue;
							if (zombieTable.slots[2].table.Count > 0 && Random.value < zombieTable.slots[2].chance)
							{
								hat = (byte)Random.Range(0, zombieTable.slots[2].table.Count);
							}
							byte gear = byte.MaxValue;
							if (zombieTable.slots[3].table.Count > 0 && Random.value < zombieTable.slots[3].chance)
							{
								gear = (byte)Random.Range(0, zombieTable.slots[3].table.Count);
							}
							byte move = (byte)Random.Range(0, 4);
							byte idle = (byte)Random.Range(0, 3);
							Vector3 vector = zombieSpawnpoint2.point;
							vector += new Vector3(0f, 0.5f, 0f);
							this.addZombie(bound, type, (byte)ezombieSpeciality, shirt, pants, hat, gear, move, idle, vector, Random.Range(0f, 360f), !LevelNavigation.flagData[(int)bound].spawnZombies || Level.info.type == ELevelType.HORDE);
						}
					}
				}
			}
		}

		// Token: 0x06002F28 RID: 12072 RVA: 0x000CEC70 File Offset: 0x000CCE70
		private bool canRegionSpawnZombiesFromTable(ZombieRegion region, ZombieTable table)
		{
			if (region.hasBeacon)
			{
				return !table.isMega;
			}
			return !table.isMega || (!region.hasMega && Time.realtimeSinceStartup - region.lastMega > 600f);
		}

		// Token: 0x06002F29 RID: 12073 RVA: 0x000CECAC File Offset: 0x000CCEAC
		public void respawnZombies()
		{
			ZombieRegion zombieRegion = ZombieManager.regions[(int)ZombieManager.respawnZombiesBound];
			if (Level.info.type == ELevelType.HORDE)
			{
				if (ZombieManager.waveRemaining > 0 || zombieRegion.alive > 0)
				{
					ZombieManager.lastWave = Time.realtimeSinceStartup;
				}
				if (ZombieManager.waveRemaining == 0)
				{
					if (zombieRegion.alive > 0)
					{
						return;
					}
					if (Time.realtimeSinceStartup - ZombieManager.lastWave <= 10f && ZombieManager.waveIndex != 0)
					{
						if (ZombieManager.waveReady)
						{
							ZombieManager._waveReady = false;
							ZombieManager.SendWave.InvokeAndLoopback(ENetReliability.Reliable, Provider.GatherRemoteClientConnections(), ZombieManager.waveReady, ZombieManager.waveIndex);
						}
						return;
					}
					if (!ZombieManager.waveReady)
					{
						ZombieManager._waveReady = true;
						ZombieManager._waveIndex++;
						ZombieManager._waveRemaining = (int)Mathf.Ceil(Mathf.Pow((float)(ZombieManager.waveIndex + 5), 1.5f));
						ZombieManager.SendWave.InvokeAndLoopback(ENetReliability.Reliable, Provider.GatherRemoteClientConnections(), ZombieManager.waveReady, ZombieManager.waveIndex);
					}
				}
			}
			if (!LevelNavigation.flagData[(int)ZombieManager.respawnZombiesBound].spawnZombies)
			{
				return;
			}
			if (zombieRegion.zombies.Count > 0)
			{
				if (zombieRegion.hasBeacon && BeaconManager.checkBeacon(ZombieManager.respawnZombiesBound).getRemaining() == 0)
				{
					return;
				}
				if ((int)zombieRegion.respawnZombieIndex >= zombieRegion.zombies.Count)
				{
					zombieRegion.respawnZombieIndex = (ushort)(zombieRegion.zombies.Count - 1);
				}
				Zombie zombie = zombieRegion.zombies[(int)zombieRegion.respawnZombieIndex];
				ZombieRegion zombieRegion2 = zombieRegion;
				zombieRegion2.respawnZombieIndex += 1;
				if ((int)zombieRegion.respawnZombieIndex >= zombieRegion.zombies.Count)
				{
					zombieRegion.respawnZombieIndex = 0;
				}
				if (!zombie.isDead)
				{
					return;
				}
				float num = Provider.modeConfigData.Zombies.Respawn_Day_Time;
				if (zombieRegion.hasBeacon)
				{
					num = Provider.modeConfigData.Zombies.Respawn_Beacon_Time;
				}
				else if (LightingManager.isFullMoon)
				{
					num = Provider.modeConfigData.Zombies.Respawn_Night_Time;
				}
				if (Time.realtimeSinceStartup - zombie.lastDead > num)
				{
					ZombieSpawnpoint zombieSpawnpoint = LevelZombies.zombies[(int)ZombieManager.respawnZombiesBound][Random.Range(0, LevelZombies.zombies[(int)ZombieManager.respawnZombiesBound].Count)];
					if (!SafezoneManager.checkPointValid(zombieSpawnpoint.point))
					{
						return;
					}
					ushort num2 = 0;
					while ((int)num2 < zombieRegion.zombies.Count)
					{
						if (!zombieRegion.zombies[(int)num2].isDead && (zombieRegion.zombies[(int)num2].transform.position - zombieSpawnpoint.point).sqrMagnitude < 4f)
						{
							return;
						}
						num2 += 1;
					}
					byte b = zombieSpawnpoint.type;
					ZombieTable zombieTable = LevelZombies.tables[(int)b];
					if (this.canRegionSpawnZombiesFromTable(zombieRegion, zombieTable))
					{
						EZombieSpeciality ezombieSpeciality = EZombieSpeciality.NORMAL;
						if (zombieRegion.hasBeacon ? (BeaconManager.checkBeacon(ZombieManager.respawnZombiesBound).getRemaining() == 1) : zombieTable.isMega)
						{
							if (!zombieTable.isMega)
							{
								byte b2 = 0;
								while ((int)b2 < LevelZombies.tables.Count)
								{
									ZombieTable zombieTable2 = LevelZombies.tables[(int)b2];
									if (zombieTable2.isMega)
									{
										b = b2;
										zombieTable = zombieTable2;
										break;
									}
									b2 += 1;
								}
							}
							zombieRegion.lastMega = Time.realtimeSinceStartup;
							zombieRegion.hasMega = true;
							ezombieSpeciality = EZombieSpeciality.MEGA;
						}
						else if (Level.info.type == ELevelType.SURVIVAL)
						{
							ezombieSpeciality = ZombieManager.generateZombieSpeciality(ZombieManager.respawnZombiesBound, zombieTable);
						}
						int maxBossZombies = LevelNavigation.flagData[(int)ZombieManager.respawnZombiesBound].maxBossZombies;
						if (maxBossZombies >= 0 && ezombieSpeciality.IsBoss() && zombieRegion.aliveBossZombieCount >= maxBossZombies)
						{
							return;
						}
						if (zombieRegion.hasBeacon)
						{
							BeaconManager.checkBeacon(ZombieManager.respawnZombiesBound).spawnRemaining();
						}
						byte shirt;
						byte pants;
						byte hat;
						byte gear;
						zombieTable.GetSpawnClothingParameters(out shirt, out pants, out hat, out gear);
						Vector3 vector = zombieSpawnpoint.point;
						vector += new Vector3(0f, 0.5f, 0f);
						zombie.sendRevive(b, (byte)ezombieSpeciality, shirt, pants, hat, gear, vector, Random.Range(0f, 360f));
						if (Level.info.type == ELevelType.HORDE)
						{
							ZombieManager._waveRemaining--;
						}
					}
				}
			}
		}

		// Token: 0x06002F2A RID: 12074 RVA: 0x000CF0A8 File Offset: 0x000CD2A8
		private void onBoundUpdated(Player player, byte oldBound, byte newBound)
		{
			if (player.channel.IsLocalPlayer && LevelNavigation.checkSafe(oldBound) && ZombieManager.regions[(int)oldBound].isNetworked)
			{
				ZombieManager.regions[(int)oldBound].destroy();
				ZombieManager.regions[(int)oldBound].isNetworked = false;
			}
			if (Provider.isServer)
			{
				if (LevelNavigation.checkSafe(oldBound) && player.movement.loadedBounds[(int)oldBound].isZombiesLoaded)
				{
					player.movement.loadedBounds[(int)oldBound].isZombiesLoaded = false;
				}
				if (LevelNavigation.checkSafe(newBound) && !player.movement.loadedBounds[(int)newBound].isZombiesLoaded)
				{
					if (player.channel.IsLocalPlayer)
					{
						this.generateZombies(newBound);
						ZombieManager.regions[(int)newBound].isNetworked = true;
					}
					else
					{
						this.SendZombiesToPlayer(player.channel.owner.transportConnection, newBound);
					}
					player.movement.loadedBounds[(int)newBound].isZombiesLoaded = true;
				}
			}
		}

		// Token: 0x06002F2B RID: 12075 RVA: 0x000CF193 File Offset: 0x000CD393
		private void onPlayerCreated(Player player)
		{
			PlayerMovement movement = player.movement;
			movement.onBoundUpdated = (PlayerBoundUpdated)Delegate.Combine(movement.onBoundUpdated, new PlayerBoundUpdated(this.onBoundUpdated));
		}

		// Token: 0x06002F2C RID: 12076 RVA: 0x000CF1BC File Offset: 0x000CD3BC
		private void onLevelLoaded(int level)
		{
			if (level > Level.BUILD_INDEX_MENU)
			{
				ZombieManager.seq = 0U;
				if (LevelNavigation.bounds == null)
				{
					return;
				}
				ZombieManager._regions = new ZombieRegion[LevelNavigation.bounds.Count];
				byte b = 0;
				while ((int)b < ZombieManager.regions.Length)
				{
					ZombieManager.regions[(int)b] = new ZombieRegion(b);
					Vector3 center = LevelNavigation.bounds[(int)b].center;
					ZombieManager.regions[(int)b].isRadioactive = VolumeManager<DeadzoneVolume, DeadzoneVolumeManager>.Get().IsNavmeshCenterInsideAnyVolume(center);
					b += 1;
				}
				ZombieManager.wanderingCount = 0;
				ZombieManager.tickIndex = 0;
				ZombieManager._tickingZombies = new List<Zombie>();
				ZombieManager.respawnZombiesBound = 0;
				ZombieManager._waveReady = false;
				ZombieManager._waveIndex = 0;
				ZombieManager._waveRemaining = 0;
				ZombieManager.onWaveUpdated = null;
				if (LevelNavigation.bounds.Count == 0 || LevelZombies.zombies.Length == 0 || LevelNavigation.bounds.Count != LevelZombies.zombies.Length)
				{
					return;
				}
				byte b2 = 0;
				while ((int)b2 < LevelNavigation.bounds.Count)
				{
					this.generateZombies(b2);
					b2 += 1;
				}
			}
			int build_INDEX_SETUP = Level.BUILD_INDEX_SETUP;
		}

		/// <summary>
		/// Kills night-only zombies at dawn. 
		/// </summary>
		// Token: 0x06002F2D RID: 12077 RVA: 0x000CF2C4 File Offset: 0x000CD4C4
		private void onDayNightUpdated(bool isDaytime)
		{
			if (!isDaytime)
			{
				return;
			}
			ZombieRegion[] regions = ZombieManager.regions;
			for (int i = 0; i < regions.Length; i++)
			{
				foreach (Zombie zombie in regions[i].zombies)
				{
					if (zombie.speciality.IsDLVolatile())
					{
						zombie.killWithFireExplosion();
					}
				}
			}
		}

		// Token: 0x06002F2E RID: 12078 RVA: 0x000CF340 File Offset: 0x000CD540
		private void onPostLevelLoaded(int level)
		{
			if (level > Level.BUILD_INDEX_MENU)
			{
				if (ZombieManager.regions == null)
				{
					return;
				}
				for (int i = 0; i < ZombieManager.regions.Length; i++)
				{
					ZombieManager.regions[i].init();
					if (Provider.isServer)
					{
						InteractableBeacon interactableBeacon = BeaconManager.checkBeacon((byte)i);
						if (interactableBeacon != null)
						{
							interactableBeacon.init(ZombieManager.regions[i].alive);
						}
						ZombieManager.regions[i].hasBeacon = (interactableBeacon != null);
					}
				}
				LightingManager.onDayNightUpdated = (DayNightUpdated)Delegate.Combine(LightingManager.onDayNightUpdated, new DayNightUpdated(this.onDayNightUpdated));
			}
		}

		// Token: 0x06002F2F RID: 12079 RVA: 0x000CF3DC File Offset: 0x000CD5DC
		private void onBeaconUpdated(byte nav, bool hasBeacon)
		{
			if (!Provider.isServer)
			{
				return;
			}
			if (ZombieManager.regions == null || (int)nav >= ZombieManager.regions.Length)
			{
				return;
			}
			if (hasBeacon)
			{
				BeaconManager.checkBeacon(nav).init(ZombieManager.regions[(int)nav].alive);
			}
			ZombieManager.SendBeacon.InvokeAndLoopback(ENetReliability.Reliable, ZombieManager.GatherRemoteClientConnections(nav), nav, hasBeacon);
		}

		// Token: 0x06002F30 RID: 12080 RVA: 0x000CF430 File Offset: 0x000CD630
		private void updateRegionsAndSendZombieStates()
		{
			byte regionIndex = 0;
			while ((int)regionIndex < ZombieManager.regions.Length)
			{
				ZombieRegion region = ZombieManager.regions[(int)regionIndex];
				region.UpdateRegion();
				if (region.updates > 0)
				{
					ZombieManager.seq += 1U;
					ZombieManager.SendZombieStates.Invoke(ENetReliability.Unreliable, ZombieManager.GatherRemoteClientConnections(regionIndex), delegate(NetPakWriter writer)
					{
						SystemNetPakWriterEx.WriteUInt8(writer, regionIndex);
						SystemNetPakWriterEx.WriteUInt32(writer, ZombieManager.seq);
						SystemNetPakWriterEx.WriteUInt16(writer, region.updates);
						foreach (Zombie zombie in region.zombies)
						{
							if (zombie.isUpdated)
							{
								zombie.isUpdated = false;
								SystemNetPakWriterEx.WriteUInt16(writer, zombie.id);
								UnityNetPakWriterEx.WriteClampedVector3(writer, zombie.transform.position, 13, 7);
								SystemNetPakWriterEx.WriteDegrees(writer, zombie.transform.eulerAngles.y, 8);
							}
						}
					});
					region.updates = 0;
				}
				byte regionIndex2 = regionIndex + 1;
				regionIndex = regionIndex2;
			}
		}

		// Token: 0x06002F31 RID: 12081 RVA: 0x000CF4F0 File Offset: 0x000CD6F0
		private void Update()
		{
			if (!Level.isLoaded)
			{
				return;
			}
			if (!Provider.isServer)
			{
				return;
			}
			if (LevelNavigation.bounds == null || LevelNavigation.bounds.Count == 0 || LevelZombies.zombies == null || LevelZombies.zombies.Length == 0 || LevelNavigation.bounds.Count != LevelZombies.zombies.Length)
			{
				return;
			}
			if (ZombieManager.regions == null || ZombieManager.tickingZombies == null)
			{
				return;
			}
			if (ZombieManager.tickIndex >= ZombieManager.tickingZombies.Count)
			{
				ZombieManager.tickIndex = 0;
			}
			int num = ZombieManager.tickIndex;
			int num2 = num + 50;
			if (num2 >= ZombieManager.tickingZombies.Count)
			{
				num2 = ZombieManager.tickingZombies.Count;
			}
			ZombieManager.tickIndex = num2;
			for (int i = num2 - 1; i >= num; i--)
			{
				Zombie zombie = ZombieManager.tickingZombies[i];
				if (zombie == null)
				{
					UnturnedLog.error("Missing zombie " + i.ToString());
				}
				else
				{
					zombie.tick();
				}
			}
			if (Time.realtimeSinceStartup - ZombieManager.lastTick > Provider.UPDATE_TIME)
			{
				ZombieManager.lastTick += Provider.UPDATE_TIME;
				if (Time.realtimeSinceStartup - ZombieManager.lastTick > Provider.UPDATE_TIME)
				{
					ZombieManager.lastTick = Time.realtimeSinceStartup;
				}
				this.updateRegionsAndSendZombieStates();
			}
			this.respawnZombies();
			ZombieManager.respawnZombiesBound += 1;
			if ((int)ZombieManager.respawnZombiesBound >= LevelZombies.zombies.Length)
			{
				ZombieManager.respawnZombiesBound = 0;
			}
		}

		// Token: 0x06002F32 RID: 12082 RVA: 0x000CF640 File Offset: 0x000CD840
		private void Start()
		{
			ZombieManager.manager = this;
			CommandLogMemoryUsage.OnExecuted = (Action<List<string>>)Delegate.Combine(CommandLogMemoryUsage.OnExecuted, new Action<List<string>>(this.OnLogMemoryUsage));
			Level.onLevelLoaded = (LevelLoaded)Delegate.Combine(Level.onLevelLoaded, new LevelLoaded(this.onLevelLoaded));
			Level.onPostLevelLoaded = (PostLevelLoaded)Delegate.Combine(Level.onPostLevelLoaded, new PostLevelLoaded(this.onPostLevelLoaded));
			Player.onPlayerCreated = (PlayerCreated)Delegate.Combine(Player.onPlayerCreated, new PlayerCreated(this.onPlayerCreated));
			BeaconManager.onBeaconUpdated = (BeaconUpdated)Delegate.Combine(BeaconManager.onBeaconUpdated, new BeaconUpdated(this.onBeaconUpdated));
		}

		// Token: 0x06002F33 RID: 12083 RVA: 0x000CF6F4 File Offset: 0x000CD8F4
		private void OnLogMemoryUsage(List<string> results)
		{
			results.Add(string.Format("Zombie regions: {0}", ZombieManager.regions.Length));
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			foreach (ZombieRegion zombieRegion in ZombieManager.regions)
			{
				int num4 = num;
				List<Zombie> zombies = zombieRegion.zombies;
				num = num4 + ((zombies != null) ? zombies.Count : 0);
				num2 += zombieRegion.alive;
				num3 += zombieRegion.aliveBossZombieCount;
			}
			results.Add(string.Format("Zombies: {0}", num));
			results.Add(string.Format("Alive zombies: {0}", num2));
			results.Add(string.Format("Alive boss zombies: {0}", num3));
			results.Add(string.Format("Ticking zombies: {0}", ZombieManager.tickingZombies.Count));
		}

		// Token: 0x06002F34 RID: 12084 RVA: 0x000CF7CC File Offset: 0x000CD9CC
		public static PooledTransportConnectionList GatherClientConnections(byte bound)
		{
			PooledTransportConnectionList pooledTransportConnectionList = TransportConnectionListPool.Get();
			foreach (SteamPlayer steamPlayer in Provider.clients)
			{
				if (steamPlayer.player != null && steamPlayer.player.movement.bound == bound)
				{
					pooledTransportConnectionList.Add(steamPlayer.transportConnection);
				}
			}
			return pooledTransportConnectionList;
		}

		// Token: 0x06002F35 RID: 12085 RVA: 0x000CF84C File Offset: 0x000CDA4C
		[Obsolete("Replaced by GatherClientConnections")]
		public static IEnumerable<ITransportConnection> EnumerateClients(byte bound)
		{
			return ZombieManager.GatherClientConnections(bound);
		}

		// Token: 0x06002F36 RID: 12086 RVA: 0x000CF854 File Offset: 0x000CDA54
		public static PooledTransportConnectionList GatherRemoteClientConnections(byte bound)
		{
			PooledTransportConnectionList pooledTransportConnectionList = TransportConnectionListPool.Get();
			foreach (SteamPlayer steamPlayer in Provider.clients)
			{
				if (steamPlayer.player != null && steamPlayer.player.movement.bound == bound)
				{
					pooledTransportConnectionList.Add(steamPlayer.transportConnection);
				}
			}
			return pooledTransportConnectionList;
		}

		// Token: 0x06002F37 RID: 12087 RVA: 0x000CF8D4 File Offset: 0x000CDAD4
		[Obsolete("Replaced by GatherRemoteClientConnections")]
		public static IEnumerable<ITransportConnection> EnumerateClients_Remote(byte bound)
		{
			return ZombieManager.GatherRemoteClientConnections(bound);
		}

		// Token: 0x0400192F RID: 6447
		public static AudioClip[] roars;

		// Token: 0x04001930 RID: 6448
		public static AudioClip[] groans;

		// Token: 0x04001931 RID: 6449
		public static AudioClip[] spits;

		// Token: 0x04001932 RID: 6450
		public static AudioClip[] dl_attacks;

		// Token: 0x04001933 RID: 6451
		public static AudioClip[] dl_deaths;

		// Token: 0x04001934 RID: 6452
		public static AudioClip[] dl_enemy_spotted;

		// Token: 0x04001935 RID: 6453
		public static AudioClip[] dl_taunt;

		// Token: 0x04001936 RID: 6454
		private static ZombieManager manager;

		// Token: 0x04001937 RID: 6455
		private static ZombieRegion[] _regions;

		// Token: 0x04001938 RID: 6456
		public static int wanderingCount;

		// Token: 0x04001939 RID: 6457
		private static int tickIndex;

		// Token: 0x0400193A RID: 6458
		private static List<Zombie> _tickingZombies;

		// Token: 0x0400193B RID: 6459
		private static byte respawnZombiesBound;

		// Token: 0x0400193C RID: 6460
		private static float lastWave;

		// Token: 0x0400193D RID: 6461
		private static bool _waveReady;

		// Token: 0x0400193E RID: 6462
		private static int _waveIndex;

		// Token: 0x0400193F RID: 6463
		private static int _waveRemaining;

		// Token: 0x04001940 RID: 6464
		private static float lastTick;

		// Token: 0x04001941 RID: 6465
		public static WaveUpdated onWaveUpdated;

		// Token: 0x04001942 RID: 6466
		private static readonly ClientStaticMethod<byte, bool> SendBeacon = ClientStaticMethod<byte, bool>.Get(new ClientStaticMethod<byte, bool>.ReceiveDelegate(ZombieManager.ReceiveBeacon));

		// Token: 0x04001943 RID: 6467
		private static readonly ClientStaticMethod<bool, int> SendWave = ClientStaticMethod<bool, int>.Get(new ClientStaticMethod<bool, int>.ReceiveDelegate(ZombieManager.ReceiveWave));

		// Token: 0x04001944 RID: 6468
		private static readonly ClientStaticMethod<byte, ushort, byte, byte, byte, byte, byte, byte, Vector3, byte> SendZombieAlive = ClientStaticMethod<byte, ushort, byte, byte, byte, byte, byte, byte, Vector3, byte>.Get(new ClientStaticMethod<byte, ushort, byte, byte, byte, byte, byte, byte, Vector3, byte>.ReceiveDelegate(ZombieManager.ReceiveZombieAlive));

		// Token: 0x04001945 RID: 6469
		private static readonly ClientStaticMethod<byte, ushort, Vector3, ERagdollEffect> SendZombieDead = ClientStaticMethod<byte, ushort, Vector3, ERagdollEffect>.Get(new ClientStaticMethod<byte, ushort, Vector3, ERagdollEffect>.ReceiveDelegate(ZombieManager.ReceiveZombieDead));

		// Token: 0x04001946 RID: 6470
		private static uint seq;

		// Token: 0x04001947 RID: 6471
		private static readonly ClientStaticMethod SendZombieStates = ClientStaticMethod.Get(new ClientStaticMethod.ReceiveDelegateWithContext(ZombieManager.ReceiveZombieStates));

		// Token: 0x04001948 RID: 6472
		private static readonly ClientStaticMethod<byte, ushort, EZombieSpeciality> SendZombieSpeciality = ClientStaticMethod<byte, ushort, EZombieSpeciality>.Get(new ClientStaticMethod<byte, ushort, EZombieSpeciality>.ReceiveDelegate(ZombieManager.ReceiveZombieSpeciality));

		// Token: 0x04001949 RID: 6473
		private static readonly ClientStaticMethod<byte, ushort> SendZombieThrow = ClientStaticMethod<byte, ushort>.Get(new ClientStaticMethod<byte, ushort>.ReceiveDelegate(ZombieManager.ReceiveZombieThrow));

		// Token: 0x0400194A RID: 6474
		private static readonly ClientStaticMethod<byte, ushort, Vector3, Vector3> SendZombieBoulder = ClientStaticMethod<byte, ushort, Vector3, Vector3>.Get(new ClientStaticMethod<byte, ushort, Vector3, Vector3>.ReceiveDelegate(ZombieManager.ReceiveZombieBoulder));

		// Token: 0x0400194B RID: 6475
		private static readonly ClientStaticMethod<byte, ushort> SendZombieSpit = ClientStaticMethod<byte, ushort>.Get(new ClientStaticMethod<byte, ushort>.ReceiveDelegate(ZombieManager.ReceiveZombieSpit));

		// Token: 0x0400194C RID: 6476
		private static readonly ClientStaticMethod<byte, ushort> SendZombieCharge = ClientStaticMethod<byte, ushort>.Get(new ClientStaticMethod<byte, ushort>.ReceiveDelegate(ZombieManager.ReceiveZombieCharge));

		// Token: 0x0400194D RID: 6477
		private static readonly ClientStaticMethod<byte, ushort> SendZombieStomp = ClientStaticMethod<byte, ushort>.Get(new ClientStaticMethod<byte, ushort>.ReceiveDelegate(ZombieManager.ReceiveZombieStomp));

		// Token: 0x0400194E RID: 6478
		private static readonly ClientStaticMethod<byte, ushort> SendZombieBreath = ClientStaticMethod<byte, ushort>.Get(new ClientStaticMethod<byte, ushort>.ReceiveDelegate(ZombieManager.ReceiveZombieBreath));

		// Token: 0x0400194F RID: 6479
		private static readonly ClientStaticMethod<byte, ushort, Vector3, Vector3> SendZombieAcid = ClientStaticMethod<byte, ushort, Vector3, Vector3>.Get(new ClientStaticMethod<byte, ushort, Vector3, Vector3>.ReceiveDelegate(ZombieManager.ReceiveZombieAcid));

		// Token: 0x04001950 RID: 6480
		private static readonly ClientStaticMethod<byte, ushort, Vector3> SendZombieSpark = ClientStaticMethod<byte, ushort, Vector3>.Get(new ClientStaticMethod<byte, ushort, Vector3>.ReceiveDelegate(ZombieManager.ReceiveZombieSpark));

		// Token: 0x04001951 RID: 6481
		private static readonly ClientStaticMethod<byte, ushort, byte> SendZombieAttack = ClientStaticMethod<byte, ushort, byte>.Get(new ClientStaticMethod<byte, ushort, byte>.ReceiveDelegate(ZombieManager.ReceiveZombieAttack));

		// Token: 0x04001952 RID: 6482
		private static readonly ClientStaticMethod<byte, ushort, byte> SendZombieStartle = ClientStaticMethod<byte, ushort, byte>.Get(new ClientStaticMethod<byte, ushort, byte>.ReceiveDelegate(ZombieManager.ReceiveZombieStartle));

		// Token: 0x04001953 RID: 6483
		private static readonly ClientStaticMethod<byte, ushort, byte> SendZombieStun = ClientStaticMethod<byte, ushort, byte>.Get(new ClientStaticMethod<byte, ushort, byte>.ReceiveDelegate(ZombieManager.ReceiveZombieStun));

		// Token: 0x04001954 RID: 6484
		private static readonly ClientStaticMethod SendZombies = ClientStaticMethod.Get(new ClientStaticMethod.ReceiveDelegateWithContext(ZombieManager.ReceiveZombies));

		// Token: 0x04001955 RID: 6485
		private static StaticResourceRef<GameObject> dedicatedZombiePrefab = new StaticResourceRef<GameObject>("Characters/Zombie_Dedicated");

		// Token: 0x04001956 RID: 6486
		private static StaticResourceRef<GameObject> serverZombiePrefab = new StaticResourceRef<GameObject>("Characters/Zombie_Server");

		// Token: 0x04001957 RID: 6487
		private static StaticResourceRef<GameObject> clientZombiePrefab = new StaticResourceRef<GameObject>("Characters/Zombie_Client");

		// Token: 0x04001958 RID: 6488
		private static ZombieManager.ZombieSpecialityWeightedRandom zombieSpecialityTable = new ZombieManager.ZombieSpecialityWeightedRandom();

		// Token: 0x04001959 RID: 6489
		internal static readonly AssetReference<EffectAsset> Souls_1_Ref = new AssetReference<EffectAsset>("c17b00f2a58646c8a9ea728f6d72e54e");

		/// <summary>
		/// Could potentially be reused generically.
		/// </summary>
		// Token: 0x0200098F RID: 2447
		private class ZombieSpecialityWeightedRandom : IComparer<ZombieManager.ZombieSpecialityWeightedRandom.Entry>
		{
			// Token: 0x17000BE7 RID: 3047
			// (get) Token: 0x06004BA6 RID: 19366 RVA: 0x001B522C File Offset: 0x001B342C
			// (set) Token: 0x06004BA7 RID: 19367 RVA: 0x001B5234 File Offset: 0x001B3434
			public float totalWeight { get; private set; }

			// Token: 0x06004BA8 RID: 19368 RVA: 0x001B523D File Offset: 0x001B343D
			public void clear()
			{
				this.entries.Clear();
				this.totalWeight = 0f;
			}

			// Token: 0x06004BA9 RID: 19369 RVA: 0x001B5258 File Offset: 0x001B3458
			public void add(EZombieSpeciality value, float weight)
			{
				weight = Mathf.Max(weight, 0f);
				ZombieManager.ZombieSpecialityWeightedRandom.Entry entry = new ZombieManager.ZombieSpecialityWeightedRandom.Entry(value, weight);
				int num = this.entries.BinarySearch(entry, this);
				if (num < 0)
				{
					num = ~num;
				}
				this.entries.Insert(num, entry);
				this.totalWeight += weight;
			}

			// Token: 0x06004BAA RID: 19370 RVA: 0x001B52AC File Offset: 0x001B34AC
			public EZombieSpeciality get()
			{
				if (this.entries.Count < 1)
				{
					return EZombieSpeciality.NONE;
				}
				float num = Random.value * this.totalWeight;
				foreach (ZombieManager.ZombieSpecialityWeightedRandom.Entry entry in this.entries)
				{
					if (num < entry.weight)
					{
						return entry.value;
					}
					num -= entry.weight;
				}
				return this.entries[0].value;
			}

			// Token: 0x06004BAB RID: 19371 RVA: 0x001B5344 File Offset: 0x001B3544
			public void log()
			{
				UnturnedLog.info("Entries: {0} Total Weight: {1}", new object[]
				{
					this.entries.Count,
					this.totalWeight
				});
				foreach (ZombieManager.ZombieSpecialityWeightedRandom.Entry entry in this.entries)
				{
					UnturnedLog.info("{0}: {1}", new object[]
					{
						entry.value,
						entry.weight
					});
				}
			}

			// Token: 0x06004BAC RID: 19372 RVA: 0x001B53F0 File Offset: 0x001B35F0
			public int Compare(ZombieManager.ZombieSpecialityWeightedRandom.Entry lhs, ZombieManager.ZombieSpecialityWeightedRandom.Entry rhs)
			{
				return -lhs.weight.CompareTo(rhs.weight);
			}

			// Token: 0x06004BAD RID: 19373 RVA: 0x001B5405 File Offset: 0x001B3605
			public ZombieSpecialityWeightedRandom()
			{
				this.entries = new List<ZombieManager.ZombieSpecialityWeightedRandom.Entry>();
				this.totalWeight = 0f;
			}

			// Token: 0x040033AE RID: 13230
			private List<ZombieManager.ZombieSpecialityWeightedRandom.Entry> entries;

			// Token: 0x02000A37 RID: 2615
			public struct Entry
			{
				// Token: 0x06004D99 RID: 19865 RVA: 0x001B9A5F File Offset: 0x001B7C5F
				public Entry(EZombieSpeciality value, float weight)
				{
					this.value = value;
					this.weight = weight;
				}

				// Token: 0x04003563 RID: 13667
				public EZombieSpeciality value;

				// Token: 0x04003564 RID: 13668
				public float weight;
			}
		}
	}
}
