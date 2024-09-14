using System;
using SDG.NetPak;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200023D RID: 573
	public static class NetIdPakEx
	{
		// Token: 0x060011C4 RID: 4548 RVA: 0x0003CEEB File Offset: 0x0003B0EB
		public static bool ReadNetId(this NetPakReader reader, out NetId value)
		{
			return reader.ReadBits(32, ref value.id);
		}

		// Token: 0x060011C5 RID: 4549 RVA: 0x0003CEFB File Offset: 0x0003B0FB
		public static bool WriteNetId(this NetPakWriter writer, NetId value)
		{
			return writer.WriteBits(value.id, 32);
		}

		// Token: 0x060011C6 RID: 4550 RVA: 0x0003CF0C File Offset: 0x0003B10C
		public static bool ReadTransform(this NetPakReader reader, out Transform value)
		{
			bool flag2;
			bool flag = reader.ReadBit(ref flag2);
			if (flag && flag2)
			{
				NetId netId;
				flag &= reader.ReadNetId(out netId);
				string path;
				flag &= SystemNetPakReaderEx.ReadString(reader, ref path, 7);
				value = NetIdRegistry.GetTransform(netId, path);
			}
			else
			{
				value = null;
			}
			return flag;
		}

		// Token: 0x060011C7 RID: 4551 RVA: 0x0003CF50 File Offset: 0x0003B150
		public static bool WriteTransform(this NetPakWriter writer, Transform value)
		{
			NetId value2;
			string text;
			if (NetIdRegistry.GetTransformNetId(value, out value2, out text))
			{
				return writer.WriteBit(true) && writer.WriteNetId(value2) && SystemNetPakWriterEx.WriteString(writer, text, 7);
			}
			return writer.WriteBit(false);
		}

		// Token: 0x04000577 RID: 1399
		internal const int TRANSFORM_PATH_BYTE_COUNT_BITS = 7;
	}
}
