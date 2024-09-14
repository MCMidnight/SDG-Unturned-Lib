using System;
using SDG.NetPak;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000214 RID: 532
	[NetInvokableGeneratedClass(typeof(UseableGun))]
	public static class UseableGun_NetMethods
	{
		// Token: 0x0600104C RID: 4172 RVA: 0x00038CAC File Offset: 0x00036EAC
		[NetInvokableGeneratedMethod("ReceiveChangeFiremode", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveChangeFiremode_Read(in ServerInvocationContext context)
		{
			NetPakReader reader = context.reader;
			NetId key;
			if (!reader.ReadNetId(out key))
			{
				return;
			}
			object obj = NetIdRegistry.Get(key);
			if (obj == null)
			{
				return;
			}
			UseableGun useableGun = obj as UseableGun;
			if (useableGun == null)
			{
				return;
			}
			if (!context.IsOwnerOf(useableGun.channel))
			{
				context.Kick(string.Format("not owner of {0}", useableGun));
				return;
			}
			EFiremode newFiremode;
			reader.ReadEnum(out newFiremode);
			useableGun.ReceiveChangeFiremode(newFiremode);
		}

		// Token: 0x0600104D RID: 4173 RVA: 0x00038D18 File Offset: 0x00036F18
		[NetInvokableGeneratedMethod("ReceiveChangeFiremode", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveChangeFiremode_Write(NetPakWriter writer, EFiremode newFiremode)
		{
			writer.WriteEnum(newFiremode);
		}

		// Token: 0x0600104E RID: 4174 RVA: 0x00038D24 File Offset: 0x00036F24
		[NetInvokableGeneratedMethod("ReceivePlayProject", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceivePlayProject_Read(in ClientInvocationContext context)
		{
			NetPakReader reader = context.reader;
			NetId key;
			if (!reader.ReadNetId(out key))
			{
				return;
			}
			object obj = NetIdRegistry.Get(key);
			if (obj == null)
			{
				return;
			}
			UseableGun useableGun = obj as UseableGun;
			if (useableGun == null)
			{
				return;
			}
			Vector3 origin;
			UnityNetPakReaderEx.ReadClampedVector3(reader, ref origin, 13, 7);
			Vector3 direction;
			UnityNetPakReaderEx.ReadNormalVector3(reader, ref direction, 9);
			ushort barrelId;
			SystemNetPakReaderEx.ReadUInt16(reader, ref barrelId);
			ushort magazineId;
			SystemNetPakReaderEx.ReadUInt16(reader, ref magazineId);
			useableGun.ReceivePlayProject(origin, direction, barrelId, magazineId);
		}

		// Token: 0x0600104F RID: 4175 RVA: 0x00038D96 File Offset: 0x00036F96
		[NetInvokableGeneratedMethod("ReceivePlayProject", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceivePlayProject_Write(NetPakWriter writer, Vector3 origin, Vector3 direction, ushort barrelId, ushort magazineId)
		{
			UnityNetPakWriterEx.WriteClampedVector3(writer, origin, 13, 7);
			UnityNetPakWriterEx.WriteNormalVector3(writer, direction, 9);
			SystemNetPakWriterEx.WriteUInt16(writer, barrelId);
			SystemNetPakWriterEx.WriteUInt16(writer, magazineId);
		}

		// Token: 0x06001050 RID: 4176 RVA: 0x00038DC0 File Offset: 0x00036FC0
		[NetInvokableGeneratedMethod("ReceivePlayShoot", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceivePlayShoot_Read(in ClientInvocationContext context)
		{
			NetId key;
			if (!context.reader.ReadNetId(out key))
			{
				return;
			}
			object obj = NetIdRegistry.Get(key);
			if (obj == null)
			{
				return;
			}
			UseableGun useableGun = obj as UseableGun;
			if (useableGun == null)
			{
				return;
			}
			useableGun.ReceivePlayShoot();
		}

		// Token: 0x06001051 RID: 4177 RVA: 0x00038DFF File Offset: 0x00036FFF
		[NetInvokableGeneratedMethod("ReceivePlayShoot", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceivePlayShoot_Write(NetPakWriter writer)
		{
		}

		// Token: 0x06001052 RID: 4178 RVA: 0x00038E04 File Offset: 0x00037004
		[NetInvokableGeneratedMethod("ReceiveAttachSight", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveAttachSight_Read(in ServerInvocationContext context)
		{
			NetPakReader reader = context.reader;
			NetId key;
			if (!reader.ReadNetId(out key))
			{
				return;
			}
			object obj = NetIdRegistry.Get(key);
			if (obj == null)
			{
				return;
			}
			UseableGun useableGun = obj as UseableGun;
			if (useableGun == null)
			{
				return;
			}
			if (!context.IsOwnerOf(useableGun.channel))
			{
				context.Kick(string.Format("not owner of {0}", useableGun));
				return;
			}
			byte page;
			SystemNetPakReaderEx.ReadUInt8(reader, ref page);
			byte x;
			SystemNetPakReaderEx.ReadUInt8(reader, ref x);
			byte y;
			SystemNetPakReaderEx.ReadUInt8(reader, ref y);
			byte b;
			SystemNetPakReaderEx.ReadUInt8(reader, ref b);
			byte[] array = new byte[(int)b];
			reader.ReadBytes(array);
			useableGun.ReceiveAttachSight(page, x, y, array);
		}

		// Token: 0x06001053 RID: 4179 RVA: 0x00038EA4 File Offset: 0x000370A4
		[NetInvokableGeneratedMethod("ReceiveAttachSight", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveAttachSight_Write(NetPakWriter writer, byte page, byte x, byte y, byte[] hash)
		{
			SystemNetPakWriterEx.WriteUInt8(writer, page);
			SystemNetPakWriterEx.WriteUInt8(writer, x);
			SystemNetPakWriterEx.WriteUInt8(writer, y);
			byte b = (byte)hash.Length;
			SystemNetPakWriterEx.WriteUInt8(writer, b);
			writer.WriteBytes(hash, (int)b);
		}

		// Token: 0x06001054 RID: 4180 RVA: 0x00038EE4 File Offset: 0x000370E4
		[NetInvokableGeneratedMethod("ReceiveAttachTactical", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveAttachTactical_Read(in ServerInvocationContext context)
		{
			NetPakReader reader = context.reader;
			NetId key;
			if (!reader.ReadNetId(out key))
			{
				return;
			}
			object obj = NetIdRegistry.Get(key);
			if (obj == null)
			{
				return;
			}
			UseableGun useableGun = obj as UseableGun;
			if (useableGun == null)
			{
				return;
			}
			if (!context.IsOwnerOf(useableGun.channel))
			{
				context.Kick(string.Format("not owner of {0}", useableGun));
				return;
			}
			byte page;
			SystemNetPakReaderEx.ReadUInt8(reader, ref page);
			byte x;
			SystemNetPakReaderEx.ReadUInt8(reader, ref x);
			byte y;
			SystemNetPakReaderEx.ReadUInt8(reader, ref y);
			byte b;
			SystemNetPakReaderEx.ReadUInt8(reader, ref b);
			byte[] array = new byte[(int)b];
			reader.ReadBytes(array);
			useableGun.ReceiveAttachTactical(page, x, y, array);
		}

		// Token: 0x06001055 RID: 4181 RVA: 0x00038F84 File Offset: 0x00037184
		[NetInvokableGeneratedMethod("ReceiveAttachTactical", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveAttachTactical_Write(NetPakWriter writer, byte page, byte x, byte y, byte[] hash)
		{
			SystemNetPakWriterEx.WriteUInt8(writer, page);
			SystemNetPakWriterEx.WriteUInt8(writer, x);
			SystemNetPakWriterEx.WriteUInt8(writer, y);
			byte b = (byte)hash.Length;
			SystemNetPakWriterEx.WriteUInt8(writer, b);
			writer.WriteBytes(hash, (int)b);
		}

		// Token: 0x06001056 RID: 4182 RVA: 0x00038FC4 File Offset: 0x000371C4
		[NetInvokableGeneratedMethod("ReceiveAttachGrip", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveAttachGrip_Read(in ServerInvocationContext context)
		{
			NetPakReader reader = context.reader;
			NetId key;
			if (!reader.ReadNetId(out key))
			{
				return;
			}
			object obj = NetIdRegistry.Get(key);
			if (obj == null)
			{
				return;
			}
			UseableGun useableGun = obj as UseableGun;
			if (useableGun == null)
			{
				return;
			}
			if (!context.IsOwnerOf(useableGun.channel))
			{
				context.Kick(string.Format("not owner of {0}", useableGun));
				return;
			}
			byte page;
			SystemNetPakReaderEx.ReadUInt8(reader, ref page);
			byte x;
			SystemNetPakReaderEx.ReadUInt8(reader, ref x);
			byte y;
			SystemNetPakReaderEx.ReadUInt8(reader, ref y);
			byte b;
			SystemNetPakReaderEx.ReadUInt8(reader, ref b);
			byte[] array = new byte[(int)b];
			reader.ReadBytes(array);
			useableGun.ReceiveAttachGrip(page, x, y, array);
		}

		// Token: 0x06001057 RID: 4183 RVA: 0x00039064 File Offset: 0x00037264
		[NetInvokableGeneratedMethod("ReceiveAttachGrip", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveAttachGrip_Write(NetPakWriter writer, byte page, byte x, byte y, byte[] hash)
		{
			SystemNetPakWriterEx.WriteUInt8(writer, page);
			SystemNetPakWriterEx.WriteUInt8(writer, x);
			SystemNetPakWriterEx.WriteUInt8(writer, y);
			byte b = (byte)hash.Length;
			SystemNetPakWriterEx.WriteUInt8(writer, b);
			writer.WriteBytes(hash, (int)b);
		}

		// Token: 0x06001058 RID: 4184 RVA: 0x000390A4 File Offset: 0x000372A4
		[NetInvokableGeneratedMethod("ReceiveAttachBarrel", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveAttachBarrel_Read(in ServerInvocationContext context)
		{
			NetPakReader reader = context.reader;
			NetId key;
			if (!reader.ReadNetId(out key))
			{
				return;
			}
			object obj = NetIdRegistry.Get(key);
			if (obj == null)
			{
				return;
			}
			UseableGun useableGun = obj as UseableGun;
			if (useableGun == null)
			{
				return;
			}
			if (!context.IsOwnerOf(useableGun.channel))
			{
				context.Kick(string.Format("not owner of {0}", useableGun));
				return;
			}
			byte page;
			SystemNetPakReaderEx.ReadUInt8(reader, ref page);
			byte x;
			SystemNetPakReaderEx.ReadUInt8(reader, ref x);
			byte y;
			SystemNetPakReaderEx.ReadUInt8(reader, ref y);
			byte b;
			SystemNetPakReaderEx.ReadUInt8(reader, ref b);
			byte[] array = new byte[(int)b];
			reader.ReadBytes(array);
			useableGun.ReceiveAttachBarrel(page, x, y, array);
		}

		// Token: 0x06001059 RID: 4185 RVA: 0x00039144 File Offset: 0x00037344
		[NetInvokableGeneratedMethod("ReceiveAttachBarrel", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveAttachBarrel_Write(NetPakWriter writer, byte page, byte x, byte y, byte[] hash)
		{
			SystemNetPakWriterEx.WriteUInt8(writer, page);
			SystemNetPakWriterEx.WriteUInt8(writer, x);
			SystemNetPakWriterEx.WriteUInt8(writer, y);
			byte b = (byte)hash.Length;
			SystemNetPakWriterEx.WriteUInt8(writer, b);
			writer.WriteBytes(hash, (int)b);
		}

		// Token: 0x0600105A RID: 4186 RVA: 0x00039184 File Offset: 0x00037384
		[NetInvokableGeneratedMethod("ReceiveAttachMagazine", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveAttachMagazine_Read(in ServerInvocationContext context)
		{
			NetPakReader reader = context.reader;
			NetId key;
			if (!reader.ReadNetId(out key))
			{
				return;
			}
			object obj = NetIdRegistry.Get(key);
			if (obj == null)
			{
				return;
			}
			UseableGun useableGun = obj as UseableGun;
			if (useableGun == null)
			{
				return;
			}
			if (!context.IsOwnerOf(useableGun.channel))
			{
				context.Kick(string.Format("not owner of {0}", useableGun));
				return;
			}
			byte page;
			SystemNetPakReaderEx.ReadUInt8(reader, ref page);
			byte x;
			SystemNetPakReaderEx.ReadUInt8(reader, ref x);
			byte y;
			SystemNetPakReaderEx.ReadUInt8(reader, ref y);
			byte b;
			SystemNetPakReaderEx.ReadUInt8(reader, ref b);
			byte[] array = new byte[(int)b];
			reader.ReadBytes(array);
			useableGun.ReceiveAttachMagazine(context, page, x, y, array);
		}

		// Token: 0x0600105B RID: 4187 RVA: 0x00039224 File Offset: 0x00037424
		[NetInvokableGeneratedMethod("ReceiveAttachMagazine", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveAttachMagazine_Write(NetPakWriter writer, byte page, byte x, byte y, byte[] hash)
		{
			SystemNetPakWriterEx.WriteUInt8(writer, page);
			SystemNetPakWriterEx.WriteUInt8(writer, x);
			SystemNetPakWriterEx.WriteUInt8(writer, y);
			byte b = (byte)hash.Length;
			SystemNetPakWriterEx.WriteUInt8(writer, b);
			writer.WriteBytes(hash, (int)b);
		}

		// Token: 0x0600105C RID: 4188 RVA: 0x00039264 File Offset: 0x00037464
		[NetInvokableGeneratedMethod("ReceivePlayReload", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceivePlayReload_Read(in ClientInvocationContext context)
		{
			NetPakReader reader = context.reader;
			NetId key;
			if (!reader.ReadNetId(out key))
			{
				return;
			}
			object obj = NetIdRegistry.Get(key);
			if (obj == null)
			{
				return;
			}
			UseableGun useableGun = obj as UseableGun;
			if (useableGun == null)
			{
				return;
			}
			bool newHammer;
			reader.ReadBit(ref newHammer);
			useableGun.ReceivePlayReload(newHammer);
		}

		// Token: 0x0600105D RID: 4189 RVA: 0x000392B0 File Offset: 0x000374B0
		[NetInvokableGeneratedMethod("ReceivePlayReload", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceivePlayReload_Write(NetPakWriter writer, bool newHammer)
		{
			writer.WriteBit(newHammer);
		}

		// Token: 0x0600105E RID: 4190 RVA: 0x000392BC File Offset: 0x000374BC
		[NetInvokableGeneratedMethod("ReceivePlayChamberJammed", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceivePlayChamberJammed_Read(in ClientInvocationContext context)
		{
			NetPakReader reader = context.reader;
			NetId key;
			if (!reader.ReadNetId(out key))
			{
				return;
			}
			object obj = NetIdRegistry.Get(key);
			if (obj == null)
			{
				return;
			}
			UseableGun useableGun = obj as UseableGun;
			if (useableGun == null)
			{
				return;
			}
			byte correctedAmmo;
			SystemNetPakReaderEx.ReadUInt8(reader, ref correctedAmmo);
			useableGun.ReceivePlayChamberJammed(correctedAmmo);
		}

		// Token: 0x0600105F RID: 4191 RVA: 0x00039308 File Offset: 0x00037508
		[NetInvokableGeneratedMethod("ReceivePlayChamberJammed", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceivePlayChamberJammed_Write(NetPakWriter writer, byte correctedAmmo)
		{
			SystemNetPakWriterEx.WriteUInt8(writer, correctedAmmo);
		}

		// Token: 0x06001060 RID: 4192 RVA: 0x00039314 File Offset: 0x00037514
		[NetInvokableGeneratedMethod("ReceivePlayAimStart", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceivePlayAimStart_Read(in ClientInvocationContext context)
		{
			NetId key;
			if (!context.reader.ReadNetId(out key))
			{
				return;
			}
			object obj = NetIdRegistry.Get(key);
			if (obj == null)
			{
				return;
			}
			UseableGun useableGun = obj as UseableGun;
			if (useableGun == null)
			{
				return;
			}
			useableGun.ReceivePlayAimStart();
		}

		// Token: 0x06001061 RID: 4193 RVA: 0x00039353 File Offset: 0x00037553
		[NetInvokableGeneratedMethod("ReceivePlayAimStart", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceivePlayAimStart_Write(NetPakWriter writer)
		{
		}

		// Token: 0x06001062 RID: 4194 RVA: 0x00039358 File Offset: 0x00037558
		[NetInvokableGeneratedMethod("ReceivePlayAimStop", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceivePlayAimStop_Read(in ClientInvocationContext context)
		{
			NetId key;
			if (!context.reader.ReadNetId(out key))
			{
				return;
			}
			object obj = NetIdRegistry.Get(key);
			if (obj == null)
			{
				return;
			}
			UseableGun useableGun = obj as UseableGun;
			if (useableGun == null)
			{
				return;
			}
			useableGun.ReceivePlayAimStop();
		}

		// Token: 0x06001063 RID: 4195 RVA: 0x00039397 File Offset: 0x00037597
		[NetInvokableGeneratedMethod("ReceivePlayAimStop", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceivePlayAimStop_Write(NetPakWriter writer)
		{
		}
	}
}
