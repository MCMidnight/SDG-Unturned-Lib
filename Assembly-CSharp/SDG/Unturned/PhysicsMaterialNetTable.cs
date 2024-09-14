using System;
using System.Collections.Generic;
using SDG.NetPak;
using SDG.NetTransport;

namespace SDG.Unturned
{
	/// <summary>
	/// String table specifically for Unity physics material names.
	/// Implemented so that tires can more efficiently replicate which ground material they are touching.
	/// </summary>
	// Token: 0x0200035D RID: 861
	public static class PhysicsMaterialNetTable
	{
		/// <summary>
		/// Get an ID that can be used to reference a physics material name over the network. If given material name
		/// isn't supported (e.g., not registered in a PhysicsMaterialAsset or over max material limit) returns
		/// <see cref="F:SDG.Unturned.PhysicsMaterialNetId.NULL" /> instead.
		/// </summary>
		// Token: 0x06001A02 RID: 6658 RVA: 0x0005D838 File Offset: 0x0005BA38
		public static PhysicsMaterialNetId GetNetId(string materialName)
		{
			if (string.IsNullOrEmpty(materialName))
			{
				return PhysicsMaterialNetId.NULL;
			}
			uint id;
			if (PhysicsMaterialNetTable.nameToId.TryGetValue(materialName, ref id))
			{
				return new PhysicsMaterialNetId(id);
			}
			return PhysicsMaterialNetId.NULL;
		}

		/// <summary>
		/// Get name of a physics material from network ID. Returns null if ID is null, e.g., if the sent name wasn't
		/// registered or was over the max material limit.
		/// </summary>
		// Token: 0x06001A03 RID: 6659 RVA: 0x0005D870 File Offset: 0x0005BA70
		public static string GetMaterialName(PhysicsMaterialNetId netId)
		{
			string result;
			if (netId.id > 0U && PhysicsMaterialNetTable.idToName.TryGetValue(netId.id, ref result))
			{
				return result;
			}
			return null;
		}

		/// <summary>
		/// Called when resetting network state.
		/// </summary>
		// Token: 0x06001A04 RID: 6660 RVA: 0x0005D89D File Offset: 0x0005BA9D
		internal static void Clear()
		{
			PhysicsMaterialNetTable.nameToId.Clear();
			PhysicsMaterialNetTable.idToName.Clear();
		}

		/// <summary>
		/// Called on server and singleplayer before loading level.
		/// </summary>
		// Token: 0x06001A05 RID: 6661 RVA: 0x0005D8B4 File Offset: 0x0005BAB4
		internal static void ServerPopulateTable()
		{
			uint num = 1U;
			foreach (KeyValuePair<Guid, PhysicsMaterialAsset> keyValuePair in PhysicMaterialCustomData.GetAssets())
			{
				PhysicsMaterialAsset value = keyValuePair.Value;
				if (value.physicMaterialNames != null && value.physicMaterialNames.Length >= 1)
				{
					uint num2 = num;
					num += 1U;
					bool flag = false;
					foreach (string text in value.physicMaterialNames)
					{
						if (PhysicsMaterialNetTable.nameToId.ContainsKey(text))
						{
							UnturnedLog.warn("Multiple physics material assets contain Unity name \"" + text + "\"");
						}
						else
						{
							PhysicsMaterialNetTable.nameToId[text] = num2;
							if (!flag)
							{
								PhysicsMaterialNetTable.idToName[num2] = text;
								flag = true;
							}
						}
					}
				}
			}
			PhysicsMaterialNetTable.idBitCount = NetPakConst.CountBits(num);
			UnturnedLog.info(string.Format("Server registered {0} Unity physics material names with {1} unique IDs ({2} bits)", PhysicsMaterialNetTable.nameToId.Count, num - 1U, PhysicsMaterialNetTable.idBitCount));
		}

		// Token: 0x06001A06 RID: 6662 RVA: 0x0005D9D8 File Offset: 0x0005BBD8
		internal static void Send(ITransportConnection transportConnection)
		{
			PhysicsMaterialNetTable.SendMappings.Invoke(ENetReliability.Reliable, transportConnection, delegate(NetPakWriter writer)
			{
				SystemNetPakWriterEx.WriteUInt8(writer, (byte)PhysicsMaterialNetTable.nameToId.Count);
				SystemNetPakWriterEx.WriteUInt8(writer, (byte)PhysicsMaterialNetTable.idBitCount);
				foreach (KeyValuePair<string, uint> keyValuePair in PhysicsMaterialNetTable.nameToId)
				{
					string key = keyValuePair.Key;
					uint value = keyValuePair.Value;
					SystemNetPakWriterEx.WriteString(writer, key, 6);
					writer.WriteBits(value, PhysicsMaterialNetTable.idBitCount);
				}
			});
		}

		// Token: 0x06001A07 RID: 6663 RVA: 0x0005DA08 File Offset: 0x0005BC08
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER)]
		public static void ReceiveMappings(in ClientInvocationContext context)
		{
			NetPakReader reader = context.reader;
			byte b;
			SystemNetPakReaderEx.ReadUInt8(reader, ref b);
			byte b2;
			SystemNetPakReaderEx.ReadUInt8(reader, ref b2);
			PhysicsMaterialNetTable.idBitCount = (int)b2;
			for (int i = 0; i < (int)b; i++)
			{
				string text;
				SystemNetPakReaderEx.ReadString(reader, ref text, 6);
				uint num;
				reader.ReadBits(PhysicsMaterialNetTable.idBitCount, ref num);
				PhysicsMaterialNetTable.nameToId[text] = num;
				PhysicsMaterialNetTable.idToName[num] = text;
			}
			UnturnedLog.info(string.Format("Client received {0} Unity physics material names ({1} bits)", PhysicsMaterialNetTable.nameToId.Count, PhysicsMaterialNetTable.idBitCount));
		}

		// Token: 0x04000BE3 RID: 3043
		private static readonly ClientStaticMethod SendMappings = ClientStaticMethod.Get(new ClientStaticMethod.ReceiveDelegateWithContext(PhysicsMaterialNetTable.ReceiveMappings));

		/// <summary>
		/// Number of bits needed to replicate PhysicsMaterialNetId.
		/// </summary>
		// Token: 0x04000BE4 RID: 3044
		internal static int idBitCount;

		// Token: 0x04000BE5 RID: 3045
		private static Dictionary<string, uint> nameToId = new Dictionary<string, uint>();

		// Token: 0x04000BE6 RID: 3046
		private static Dictionary<uint, string> idToName = new Dictionary<uint, string>();
	}
}
