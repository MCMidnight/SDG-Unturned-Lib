using System;
using System.Collections.Generic;
using SDG.NetPak;

namespace SDG.Unturned
{
	/// <summary>
	/// When a client method is called on a target object that does not exist yet this class is responsible for
	/// deferring the invocation until the instance does exist. For example until finished async loading.
	/// </summary>
	// Token: 0x02000241 RID: 577
	public static class NetInvocationDeferralRegistry
	{
		/// <summary>
		/// Called by generated methods when target object does not exist. If target object has been marked deferred
		/// then the method will be invoked after it exists.
		/// </summary>
		// Token: 0x060011DB RID: 4571 RVA: 0x0003D46C File Offset: 0x0003B66C
		public static void Defer(NetId key, in ClientInvocationContext context, NetInvokeDeferred callback)
		{
			List<NetInvocationDeferralRegistry.DeferredInvocation> list;
			if (NetInvocationDeferralRegistry.deferrals.TryGetValue(key, ref list))
			{
				NetPakReader reader = context.reader;
				NetInvocationDeferralRegistry.DeferredInvocation deferredInvocation = default(NetInvocationDeferralRegistry.DeferredInvocation);
				deferredInvocation.netId = key;
				deferredInvocation.buffer = new byte[reader.RemainingSegmentLength];
				if (reader.SaveState(ref deferredInvocation.scratch, ref deferredInvocation.scratchBitCount, deferredInvocation.buffer))
				{
					deferredInvocation.methodInfo = context.clientMethodInfo;
					deferredInvocation.callback = callback;
					list.Add(deferredInvocation);
				}
			}
		}

		/// <summary>
		/// Add list of deferred invocations for key. Otherwise messages will be discarded assuming it was canceled.
		/// </summary>
		// Token: 0x060011DC RID: 4572 RVA: 0x0003D4EC File Offset: 0x0003B6EC
		public static void MarkDeferred(NetId key, uint blockSize = 1U)
		{
			List<NetInvocationDeferralRegistry.DeferredInvocation> list;
			if (!NetInvocationDeferralRegistry.deferrals.TryGetValue(key, ref list))
			{
				if (NetInvocationDeferralRegistry.pool.Count > 0)
				{
					list = NetInvocationDeferralRegistry.pool.GetAndRemoveTail<List<NetInvocationDeferralRegistry.DeferredInvocation>>();
				}
				else
				{
					list = new List<NetInvocationDeferralRegistry.DeferredInvocation>();
				}
				for (uint num = 0U; num < blockSize; num += 1U)
				{
					NetInvocationDeferralRegistry.deferrals.Add(key + num, list);
				}
			}
		}

		/// <summary>
		/// Remove pending invocations.
		/// </summary>
		// Token: 0x060011DD RID: 4573 RVA: 0x0003D548 File Offset: 0x0003B748
		public static void Cancel(NetId key, uint blockSize = 1U)
		{
			List<NetInvocationDeferralRegistry.DeferredInvocation> list;
			if (NetInvocationDeferralRegistry.deferrals.TryGetValue(key, ref list))
			{
				for (uint num = 0U; num < blockSize; num += 1U)
				{
					NetInvocationDeferralRegistry.deferrals.Remove(key + num);
				}
				list.Clear();
				NetInvocationDeferralRegistry.pool.Add(list);
			}
		}

		// Token: 0x060011DE RID: 4574 RVA: 0x0003D594 File Offset: 0x0003B794
		private static void Invoke(object voidNetObj, NetInvocationDeferralRegistry.DeferredInvocation invocation)
		{
			NetPakReader invokableReader = NetMessages.GetInvokableReader();
			invokableReader.LoadState(invocation.scratch, invocation.scratchBitCount, invocation.buffer, invocation.buffer.Length);
			ClientInvocationContext clientInvocationContext = new ClientInvocationContext(ClientInvocationContext.EOrigin.Deferred, invokableReader, invocation.methodInfo);
			try
			{
				invocation.callback(voidNetObj, clientInvocationContext);
			}
			catch (Exception e)
			{
				UnturnedLog.exception(e, "Exception invoking {0} deferred:", new object[]
				{
					invocation.methodInfo
				});
			}
		}

		/// <summary>
		/// Invoke all deferred calls.
		/// </summary>
		// Token: 0x060011DF RID: 4575 RVA: 0x0003D614 File Offset: 0x0003B814
		public static void Invoke(NetId key, uint blockSize = 1U)
		{
			List<NetInvocationDeferralRegistry.DeferredInvocation> list;
			if (NetInvocationDeferralRegistry.deferrals.TryGetValue(key, ref list))
			{
				for (uint num = 0U; num < blockSize; num += 1U)
				{
					NetInvocationDeferralRegistry.deferrals.Remove(key + num);
				}
				foreach (NetInvocationDeferralRegistry.DeferredInvocation deferredInvocation in list)
				{
					object obj = NetIdRegistry.Get(deferredInvocation.netId);
					if (obj == null)
					{
						break;
					}
					NetInvocationDeferralRegistry.Invoke(obj, deferredInvocation);
				}
				list.Clear();
				NetInvocationDeferralRegistry.pool.Add(list);
			}
		}

		/// <summary>
		/// Called before loading level.
		/// </summary>
		// Token: 0x060011E0 RID: 4576 RVA: 0x0003D6B4 File Offset: 0x0003B8B4
		internal static void Clear()
		{
			NetInvocationDeferralRegistry.deferrals.Clear();
			NetInvocationDeferralRegistry.pool.Clear();
		}

		// Token: 0x04000580 RID: 1408
		private static Dictionary<NetId, List<NetInvocationDeferralRegistry.DeferredInvocation>> deferrals = new Dictionary<NetId, List<NetInvocationDeferralRegistry.DeferredInvocation>>();

		// Token: 0x04000581 RID: 1409
		private static List<List<NetInvocationDeferralRegistry.DeferredInvocation>> pool = new List<List<NetInvocationDeferralRegistry.DeferredInvocation>>();

		// Token: 0x020008CF RID: 2255
		private struct DeferredInvocation
		{
			/// <summary>
			/// Invocations are grouped by net id block to ensure order is preserved between related objects. 
			/// </summary>
			// Token: 0x040031E5 RID: 12773
			public NetId netId;

			// Token: 0x040031E6 RID: 12774
			public uint scratch;

			// Token: 0x040031E7 RID: 12775
			public int scratchBitCount;

			// Token: 0x040031E8 RID: 12776
			public byte[] buffer;

			// Token: 0x040031E9 RID: 12777
			public ClientMethodInfo methodInfo;

			/// <summary>
			/// Not a member of ClientMethodInfo because it does not need to be looked up using reflection.
			/// </summary>
			// Token: 0x040031EA RID: 12778
			public NetInvokeDeferred callback;
		}
	}
}
