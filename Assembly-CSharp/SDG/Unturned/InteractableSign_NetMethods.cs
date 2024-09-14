using System;
using SDG.NetPak;

namespace SDG.Unturned
{
	// Token: 0x020001E7 RID: 487
	[NetInvokableGeneratedClass(typeof(InteractableSign))]
	public static class InteractableSign_NetMethods
	{
		// Token: 0x06000EAF RID: 3759 RVA: 0x00032FA4 File Offset: 0x000311A4
		private static void ReceiveChangeText_DeferredRead(object voidNetObj, in ClientInvocationContext context)
		{
			InteractableSign interactableSign = voidNetObj as InteractableSign;
			if (interactableSign == null)
			{
				return;
			}
			string newText;
			SystemNetPakReaderEx.ReadString(context.reader, ref newText, 11);
			interactableSign.ReceiveChangeText(newText);
		}

		// Token: 0x06000EB0 RID: 3760 RVA: 0x00032FDC File Offset: 0x000311DC
		[NetInvokableGeneratedMethod("ReceiveChangeText", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveChangeText_Read(in ClientInvocationContext context)
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
				NetInvocationDeferralRegistry.Defer(key, context, new NetInvokeDeferred(InteractableSign_NetMethods.ReceiveChangeText_DeferredRead));
				return;
			}
			InteractableSign interactableSign = obj as InteractableSign;
			if (interactableSign == null)
			{
				return;
			}
			string newText;
			SystemNetPakReaderEx.ReadString(reader, ref newText, 11);
			interactableSign.ReceiveChangeText(newText);
		}

		// Token: 0x06000EB1 RID: 3761 RVA: 0x0003303D File Offset: 0x0003123D
		[NetInvokableGeneratedMethod("ReceiveChangeText", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveChangeText_Write(NetPakWriter writer, string newText)
		{
			SystemNetPakWriterEx.WriteString(writer, newText, 11);
		}

		// Token: 0x06000EB2 RID: 3762 RVA: 0x0003304C File Offset: 0x0003124C
		[NetInvokableGeneratedMethod("ReceiveChangeTextRequest", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveChangeTextRequest_Read(in ServerInvocationContext context)
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
			InteractableSign interactableSign = obj as InteractableSign;
			if (interactableSign == null)
			{
				return;
			}
			string newText;
			SystemNetPakReaderEx.ReadString(reader, ref newText, 11);
			interactableSign.ReceiveChangeTextRequest(context, newText);
		}

		// Token: 0x06000EB3 RID: 3763 RVA: 0x0003309B File Offset: 0x0003129B
		[NetInvokableGeneratedMethod("ReceiveChangeTextRequest", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveChangeTextRequest_Write(NetPakWriter writer, string newText)
		{
			SystemNetPakWriterEx.WriteString(writer, newText, 11);
		}
	}
}
