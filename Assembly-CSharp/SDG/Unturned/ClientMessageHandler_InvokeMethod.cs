using System;
using SDG.NetPak;

namespace SDG.Unturned
{
	// Token: 0x0200026B RID: 619
	internal static class ClientMessageHandler_InvokeMethod
	{
		// Token: 0x0600126B RID: 4715 RVA: 0x0003FA88 File Offset: 0x0003DC88
		internal static void ReadMessage(NetPakReader reader)
		{
			uint num;
			if (!reader.ReadBits(NetReflection.clientMethodsBitCount, ref num))
			{
				UnturnedLog.warn("unable to read method index");
				return;
			}
			if (num >= NetReflection.clientMethodsLength)
			{
				UnturnedLog.warn("out of bounds method index ({0}/{1})", new object[]
				{
					num,
					NetReflection.clientMethodsLength
				});
				return;
			}
			ClientMethodInfo clientMethodInfo = NetReflection.clientMethods[(int)num];
			ClientInvocationContext clientInvocationContext = new ClientInvocationContext(ClientInvocationContext.EOrigin.Remote, reader, clientMethodInfo);
			try
			{
				clientMethodInfo.readMethod(clientInvocationContext);
			}
			catch (Exception e)
			{
				UnturnedLog.exception(e, "Exception invoking {0} from server:", new object[]
				{
					clientMethodInfo
				});
			}
		}
	}
}
