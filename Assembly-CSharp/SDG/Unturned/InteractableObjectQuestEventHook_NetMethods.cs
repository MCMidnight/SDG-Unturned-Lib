using System;
using SDG.NetPak;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020001E0 RID: 480
	[NetInvokableGeneratedClass(typeof(InteractableObjectQuestEventHook))]
	public static class InteractableObjectQuestEventHook_NetMethods
	{
		// Token: 0x06000E94 RID: 3732 RVA: 0x00032AA0 File Offset: 0x00030CA0
		[NetInvokableGeneratedMethod("ReceiveUsedNotification", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveUsedNotification_Read(in ClientInvocationContext context)
		{
			Transform eventHookTransform;
			context.reader.ReadTransform(out eventHookTransform);
			InteractableObjectQuestEventHook.ReceiveUsedNotification(eventHookTransform);
		}

		// Token: 0x06000E95 RID: 3733 RVA: 0x00032AC1 File Offset: 0x00030CC1
		[NetInvokableGeneratedMethod("ReceiveUsedNotification", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveUsedNotification_Write(NetPakWriter writer, Transform eventHookTransform)
		{
			writer.WriteTransform(eventHookTransform);
		}
	}
}
