using System;
using SDG.NetPak;

namespace SDG.Unturned
{
	// Token: 0x02000269 RID: 617
	internal static class ClientMessageHandler_BattlEye
	{
		// Token: 0x06001267 RID: 4711 RVA: 0x0003F67C File Offset: 0x0003D87C
		internal unsafe static void ReadMessage(NetPakReader reader)
		{
			if (Provider.battlEyeClientHandle != IntPtr.Zero && Provider.battlEyeClientRunData != null && Provider.battlEyeClientRunData.pfnReceivedPacket != null)
			{
				uint num;
				reader.ReadBits(Provider.battlEyeBufferSize.bitCount, ref num);
				byte[] array;
				int num2;
				if (num > 0U && reader.ReadBytesPtr((int)num, ref array, ref num2))
				{
					byte[] array2;
					byte* ptr;
					if ((array2 = array) == null || array2.Length == 0)
					{
						ptr = null;
					}
					else
					{
						ptr = &array2[0];
					}
					IntPtr intPtr;
					intPtr..ctor((void*)(ptr + num2));
					Provider.battlEyeClientRunData.pfnReceivedPacket.Invoke(intPtr, (int)num);
					array2 = null;
				}
			}
		}
	}
}
