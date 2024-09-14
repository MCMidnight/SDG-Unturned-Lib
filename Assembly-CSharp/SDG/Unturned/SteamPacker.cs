using System;
using Steamworks;

namespace SDG.Unturned
{
	// Token: 0x02000690 RID: 1680
	public class SteamPacker
	{
		// Token: 0x17000A13 RID: 2579
		// (get) Token: 0x0600388E RID: 14478 RVA: 0x0010B6A9 File Offset: 0x001098A9
		// (set) Token: 0x0600388F RID: 14479 RVA: 0x0010B6B5 File Offset: 0x001098B5
		[Obsolete]
		public static bool longBinaryData
		{
			get
			{
				return SteamPacker.luggageBlock.longBinaryData;
			}
			set
			{
				SteamPacker.luggageBlock.longBinaryData = value;
			}
		}

		// Token: 0x06003890 RID: 14480 RVA: 0x0010B6C2 File Offset: 0x001098C2
		[Obsolete]
		public static object read(Type type)
		{
			return SteamPacker.luggageBlock.read(type);
		}

		// Token: 0x06003891 RID: 14481 RVA: 0x0010B6CF File Offset: 0x001098CF
		[Obsolete]
		public static object[] read(Type type_0, Type type_1, Type type_2)
		{
			return SteamPacker.luggageBlock.read(type_0, type_1, type_2);
		}

		// Token: 0x06003892 RID: 14482 RVA: 0x0010B6DE File Offset: 0x001098DE
		[Obsolete]
		public static object[] read(Type type_0, Type type_1, Type type_2, Type type_3)
		{
			return SteamPacker.luggageBlock.read(type_0, type_1, type_2, type_3);
		}

		// Token: 0x06003893 RID: 14483 RVA: 0x0010B6EE File Offset: 0x001098EE
		[Obsolete]
		public static object[] read(Type type_0, Type type_1, Type type_2, Type type_3, Type type_4, Type type_5)
		{
			return SteamPacker.luggageBlock.read(type_0, type_1, type_2, type_3, type_4, type_5);
		}

		// Token: 0x06003894 RID: 14484 RVA: 0x0010B702 File Offset: 0x00109902
		[Obsolete]
		public static object[] read(Type type_0, Type type_1, Type type_2, Type type_3, Type type_4, Type type_5, Type type_6)
		{
			return SteamPacker.luggageBlock.read(type_0, type_1, type_2, type_3, type_4, type_5, type_6);
		}

		// Token: 0x06003895 RID: 14485 RVA: 0x0010B718 File Offset: 0x00109918
		[Obsolete]
		public static object[] read(params Type[] types)
		{
			return SteamPacker.luggageBlock.read(types);
		}

		// Token: 0x06003896 RID: 14486 RVA: 0x0010B725 File Offset: 0x00109925
		[Obsolete]
		public static void openRead(int prefix, byte[] bytes)
		{
			SteamPacker.openRead(prefix, bytes.Length, bytes);
		}

		// Token: 0x06003897 RID: 14487 RVA: 0x0010B731 File Offset: 0x00109931
		[Obsolete]
		public static void openRead(int prefix, int size, byte[] bytes)
		{
			SteamPacker.luggageBlock.resetForRead(prefix, bytes, size);
		}

		// Token: 0x06003898 RID: 14488 RVA: 0x0010B740 File Offset: 0x00109940
		[Obsolete]
		public static void closeRead()
		{
		}

		// Token: 0x06003899 RID: 14489 RVA: 0x0010B742 File Offset: 0x00109942
		[Obsolete]
		public static void write(object objects)
		{
			SteamPacker.luggageBlock.write(objects);
		}

		// Token: 0x0600389A RID: 14490 RVA: 0x0010B74F File Offset: 0x0010994F
		[Obsolete]
		public static void write(object object_0, object object_1)
		{
			SteamPacker.luggageBlock.write(object_0, object_1);
		}

		// Token: 0x0600389B RID: 14491 RVA: 0x0010B75D File Offset: 0x0010995D
		[Obsolete]
		public static void write(object object_0, object object_1, object object_2)
		{
			SteamPacker.luggageBlock.write(object_0, object_1, object_2);
		}

		// Token: 0x0600389C RID: 14492 RVA: 0x0010B76C File Offset: 0x0010996C
		[Obsolete]
		public static void write(object object_0, object object_1, object object_2, object object_3)
		{
			SteamPacker.luggageBlock.write(object_0, object_1, object_2, object_3);
		}

		// Token: 0x0600389D RID: 14493 RVA: 0x0010B77C File Offset: 0x0010997C
		[Obsolete]
		public static void write(object object_0, object object_1, object object_2, object object_3, object object_4, object object_5)
		{
			SteamPacker.luggageBlock.write(object_0, object_1, object_2, object_3, object_4, object_5);
		}

		// Token: 0x0600389E RID: 14494 RVA: 0x0010B790 File Offset: 0x00109990
		[Obsolete]
		public static void write(object object_0, object object_1, object object_2, object object_3, object object_4, object object_5, object object_6)
		{
			SteamPacker.luggageBlock.write(object_0, object_1, object_2, object_3, object_4, object_5, object_6);
		}

		// Token: 0x0600389F RID: 14495 RVA: 0x0010B7A6 File Offset: 0x001099A6
		[Obsolete]
		public static void write(params object[] objects)
		{
			SteamPacker.luggageBlock.write(objects);
		}

		// Token: 0x060038A0 RID: 14496 RVA: 0x0010B7B3 File Offset: 0x001099B3
		[Obsolete]
		public static void openWrite(int prefix)
		{
			SteamPacker.luggageBlock.resetForWrite(prefix);
		}

		// Token: 0x060038A1 RID: 14497 RVA: 0x0010B7C0 File Offset: 0x001099C0
		[Obsolete]
		public static byte[] closeWrite(out int size)
		{
			return SteamPacker.luggageBlock.getBytes(out size);
		}

		// Token: 0x060038A2 RID: 14498 RVA: 0x0010B7CD File Offset: 0x001099CD
		public static byte[] getBytes(int prefix, out int size, params object[] objects)
		{
			SteamPacker.luggageBlock.resetForWrite(prefix);
			SteamPacker.luggageBlock.write(objects);
			return SteamPacker.luggageBlock.getBytes(out size);
		}

		// Token: 0x060038A3 RID: 14499 RVA: 0x0010B7F0 File Offset: 0x001099F0
		[Obsolete]
		public static object[] getObjects(CSteamID steamID, int offset, int prefix, byte[] bytes, params Type[] types)
		{
			return SteamPacker.getObjects(steamID, offset, prefix, bytes.Length, bytes, types);
		}

		// Token: 0x060038A4 RID: 14500 RVA: 0x0010B800 File Offset: 0x00109A00
		[Obsolete]
		public static object[] getObjects(CSteamID steamID, int offset, int prefix, int size, byte[] bytes, params Type[] types)
		{
			SteamPacker.luggageBlock.resetForRead(offset + prefix, bytes, size);
			if (types[0].GetElementType() == typeof(ClientInvocationContext))
			{
				object[] array = SteamPacker.luggageBlock.read(1, types);
				array[0] = default(ClientInvocationContext);
				return array;
			}
			if (types[0].GetElementType() == typeof(ServerInvocationContext))
			{
				object[] array2 = SteamPacker.luggageBlock.read(1, types);
				ServerInvocationContext serverInvocationContext = ServerInvocationContext.FromSteamIDForBackwardsCompatibility(steamID);
				array2[0] = serverInvocationContext;
				return array2;
			}
			return SteamPacker.luggageBlock.read(types);
		}

		// Token: 0x060038A5 RID: 14501 RVA: 0x0010B89A File Offset: 0x00109A9A
		internal static object[] getObjectsForLegacyRPC(int offset, int prefix, int size, byte[] bytes, Type[] types, int typesOffset)
		{
			SteamPacker.luggageBlock.resetForRead(offset + prefix, bytes, size);
			return SteamPacker.luggageBlock.readForLegacyRPC(typesOffset, types);
		}

		// Token: 0x04002185 RID: 8581
		[Obsolete]
		public static Block block = new Block();

		/// <summary>
		/// Temporary replacement for static block member because plugins might depend on it.
		/// </summary>
		// Token: 0x04002186 RID: 8582
		private static NetPakBlockImplementation luggageBlock = new NetPakBlockImplementation();
	}
}
