using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200023E RID: 574
	public static class NetIdRegistry
	{
		// Token: 0x060011C8 RID: 4552 RVA: 0x0003CF8D File Offset: 0x0003B18D
		public static NetId Claim()
		{
			return new NetId(NetIdRegistry.counter += 1U);
		}

		// Token: 0x060011C9 RID: 4553 RVA: 0x0003CFA1 File Offset: 0x0003B1A1
		public static NetId ClaimBlock(uint size)
		{
			NetId result = new NetId(NetIdRegistry.counter + 1U);
			NetIdRegistry.counter += size;
			return result;
		}

		// Token: 0x060011CA RID: 4554 RVA: 0x0003CFBC File Offset: 0x0003B1BC
		public static object Get(NetId key)
		{
			object result;
			NetIdRegistry.pairings.TryGetValue(key, ref result);
			return result;
		}

		// Token: 0x060011CB RID: 4555 RVA: 0x0003CFD8 File Offset: 0x0003B1D8
		public static T Get<T>(NetId key) where T : class
		{
			return NetIdRegistry.Get(key) as T;
		}

		// Token: 0x060011CC RID: 4556 RVA: 0x0003CFEC File Offset: 0x0003B1EC
		public static void Assign(NetId key, object value)
		{
			object obj;
			if (NetIdRegistry.pairings.TryGetValue(key, ref obj))
			{
				if (value == obj)
				{
					UnturnedLog.error(string.Format("Net id {0} was already assigned to {1}", key, value));
				}
				else
				{
					UnturnedLog.error(string.Format("Net id {0} was previously assigned to {1}, reassigning to {2}", key, obj, value));
				}
				NetIdRegistry.pairings[key] = value;
				return;
			}
			NetIdRegistry.pairings.Add(key, value);
		}

		// Token: 0x060011CD RID: 4557 RVA: 0x0003D054 File Offset: 0x0003B254
		public static void AssignTransform(NetId key, Transform value)
		{
			object obj;
			bool flag = NetIdRegistry.pairings.TryGetValue(key, ref obj);
			NetId netId;
			bool flag2 = NetIdRegistry.transformPairings.TryGetValue(value, ref netId);
			if (flag && flag2)
			{
				if (value == obj)
				{
					if (key == netId)
					{
						UnturnedLog.error(string.Format("Net id {0} and transform {1} were already assigned", key, value));
					}
					else
					{
						UnturnedLog.error(string.Format("Net id {0} was previously assigned to transform {1}, but transform was previously assigned to net id {2}, reassigning", key, value, netId));
					}
				}
				else if (key == netId)
				{
					UnturnedLog.error(string.Format("Transform {0} was previously assigned to net id {1}, but net id was previously assigned to transform {2}, reassigning", value, key, obj));
				}
				else
				{
					UnturnedLog.error(string.Format("Net id {0} was previously assigned to {1} and transform {2} was previously assigned to {3}, reassigning", new object[]
					{
						key,
						obj,
						value,
						netId
					}));
				}
				NetIdRegistry.pairings[key] = value;
				NetIdRegistry.transformPairings[value] = key;
				return;
			}
			if (flag)
			{
				if (value == obj)
				{
					UnturnedLog.error(string.Format("Net id {0} was already assigned to transform {1}", key, value));
				}
				else
				{
					UnturnedLog.error(string.Format("Net id {0} was previously assigned to {1}, reassigning to transform {2}", key, obj, value));
				}
				NetIdRegistry.pairings[key] = value;
				NetIdRegistry.transformPairings.Add(value, key);
				return;
			}
			if (flag2)
			{
				if (key == netId)
				{
					UnturnedLog.error(string.Format("Transform {0} was already assigned to net id {1}", value, key));
				}
				else
				{
					UnturnedLog.error(string.Format("Transform {0} was previously assigned to net id {1}, reassigning to net id {2}", value, netId, key));
				}
				NetIdRegistry.pairings.Add(key, value);
				NetIdRegistry.transformPairings[value] = key;
				return;
			}
			NetIdRegistry.pairings.Add(key, value);
			NetIdRegistry.transformPairings.Add(value, key);
		}

		// Token: 0x060011CE RID: 4558 RVA: 0x0003D1F2 File Offset: 0x0003B3F2
		public static bool Release(NetId key)
		{
			return NetIdRegistry.pairings.Remove(key);
		}

		// Token: 0x060011CF RID: 4559 RVA: 0x0003D1FF File Offset: 0x0003B3FF
		public static void ReleaseTransform(NetId key, Transform value)
		{
			NetIdRegistry.pairings.Remove(key);
			NetIdRegistry.transformPairings.Remove(value);
		}

		// Token: 0x060011D0 RID: 4560 RVA: 0x0003D21C File Offset: 0x0003B41C
		public static Transform GetTransform(NetId netId, string path)
		{
			Transform transform = NetIdRegistry.Get<Transform>(netId);
			if (transform != null && !string.IsNullOrEmpty(path))
			{
				transform = transform.Find(path);
			}
			return transform;
		}

		/// <summary>
		/// Get net ID only if transform is directly registered, not if transform is the child of a registered transform.
		/// </summary>
		// Token: 0x060011D1 RID: 4561 RVA: 0x0003D24C File Offset: 0x0003B44C
		public static NetId GetTransformNetId(Transform transform)
		{
			NetId invalid;
			if (transform == null || !NetIdRegistry.transformPairings.TryGetValue(transform, ref invalid))
			{
				invalid = NetId.INVALID;
			}
			return invalid;
		}

		// Token: 0x060011D2 RID: 4562 RVA: 0x0003D278 File Offset: 0x0003B478
		public static bool GetTransformNetId(Transform transform, out NetId netId, out string path)
		{
			if (transform == null)
			{
				netId = NetId.INVALID;
				path = null;
				return false;
			}
			if (NetIdRegistry.transformPairings.TryGetValue(transform, ref netId))
			{
				path = null;
				return true;
			}
			Transform parent = transform.parent;
			if (parent != null)
			{
				if (NetIdRegistry.transformPairings.TryGetValue(parent, ref netId))
				{
					path = transform.name;
					return true;
				}
				Transform parent2 = parent.parent;
				if (parent2 != null)
				{
					NetIdRegistry.pathTransforms.Clear();
					while (!NetIdRegistry.transformPairings.TryGetValue(parent2, ref netId))
					{
						NetIdRegistry.pathTransforms.Add(parent2);
						parent2 = parent2.parent;
						if (!(parent2 != null))
						{
							goto IL_12A;
						}
					}
					NetIdRegistry.pathStringBuilder.Length = 0;
					for (int i = NetIdRegistry.pathTransforms.Count - 1; i >= 0; i--)
					{
						NetIdRegistry.pathStringBuilder.Append(NetIdRegistry.pathTransforms[i].name);
						NetIdRegistry.pathStringBuilder.Append('/');
					}
					NetIdRegistry.pathStringBuilder.Append(parent.name);
					NetIdRegistry.pathStringBuilder.Append('/');
					NetIdRegistry.pathStringBuilder.Append(transform.name);
					path = NetIdRegistry.pathStringBuilder.ToString();
					return true;
				}
			}
			IL_12A:
			netId = NetId.INVALID;
			path = null;
			return false;
		}

		/// <summary>
		/// Log every registered pairing.
		/// </summary>
		// Token: 0x060011D3 RID: 4563 RVA: 0x0003D3C0 File Offset: 0x0003B5C0
		public static void Dump()
		{
			int num = 0;
			foreach (KeyValuePair<NetId, object> keyValuePair in NetIdRegistry.pairings)
			{
				num++;
			}
		}

		/// <summary>
		/// Called before loading level.
		/// </summary>
		// Token: 0x060011D4 RID: 4564 RVA: 0x0003D414 File Offset: 0x0003B614
		public static void Clear()
		{
			NetIdRegistry.counter = 0U;
			NetIdRegistry.pairings.Clear();
			NetIdRegistry.transformPairings.Clear();
		}

		// Token: 0x060011D5 RID: 4565 RVA: 0x0003D430 File Offset: 0x0003B630
		[Conditional("LOG_NET_ID")]
		private static void Log(string message)
		{
			UnturnedLog.info(message);
		}

		// Token: 0x04000578 RID: 1400
		private static List<Transform> pathTransforms = new List<Transform>(16);

		// Token: 0x04000579 RID: 1401
		private static StringBuilder pathStringBuilder = new StringBuilder(256);

		// Token: 0x0400057A RID: 1402
		private static uint counter;

		// Token: 0x0400057B RID: 1403
		private static Dictionary<NetId, object> pairings = new Dictionary<NetId, object>();

		/// <summary>
		/// Reverse pairing specifically for building net id + relative path name.
		/// </summary>
		// Token: 0x0400057C RID: 1404
		private static Dictionary<Transform, NetId> transformPairings = new Dictionary<Transform, NetId>();
	}
}
