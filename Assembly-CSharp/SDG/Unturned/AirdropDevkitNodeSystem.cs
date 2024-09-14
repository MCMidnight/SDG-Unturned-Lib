using System;
using System.Collections.Generic;
using SDG.Framework.Devkit;
using SDG.Framework.Utilities;
using UnityEngine;
using UnityEngine.Profiling;

namespace SDG.Unturned
{
	// Token: 0x02000158 RID: 344
	public class AirdropDevkitNodeSystem : TempNodeSystemBase
	{
		// Token: 0x0600089D RID: 2205 RVA: 0x0001E3C5 File Offset: 0x0001C5C5
		public static AirdropDevkitNodeSystem Get()
		{
			return AirdropDevkitNodeSystem.instance;
		}

		// Token: 0x0600089E RID: 2206 RVA: 0x0001E3CC File Offset: 0x0001C5CC
		public IReadOnlyList<AirdropDevkitNode> GetAllNodes()
		{
			return this.allNodes;
		}

		// Token: 0x0600089F RID: 2207 RVA: 0x0001E3D4 File Offset: 0x0001C5D4
		internal override Type GetComponentType()
		{
			return typeof(AirdropDevkitNode);
		}

		// Token: 0x060008A0 RID: 2208 RVA: 0x0001E3E0 File Offset: 0x0001C5E0
		internal override IEnumerable<GameObject> EnumerateGameObjects()
		{
			foreach (AirdropDevkitNode airdropDevkitNode in this.allNodes)
			{
				yield return airdropDevkitNode.gameObject;
			}
			List<AirdropDevkitNode>.Enumerator enumerator = default(List<AirdropDevkitNode>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x060008A1 RID: 2209 RVA: 0x0001E3F0 File Offset: 0x0001C5F0
		internal void AddNode(AirdropDevkitNode node)
		{
			this.allNodes.Add(node);
		}

		// Token: 0x060008A2 RID: 2210 RVA: 0x0001E3FE File Offset: 0x0001C5FE
		internal void RemoveNode(AirdropDevkitNode node)
		{
			this.allNodes.RemoveFast(node);
		}

		// Token: 0x060008A3 RID: 2211 RVA: 0x0001E40D File Offset: 0x0001C60D
		internal AirdropDevkitNodeSystem()
		{
			AirdropDevkitNodeSystem.instance = this;
			this.allNodes = new List<AirdropDevkitNode>();
			this.gizmoUpdateSampler = CustomSampler.Create("AirdropDevkitNodeSystem.UpdateGizmos", false);
			TimeUtility.updated += this.OnUpdateGizmos;
		}

		// Token: 0x060008A4 RID: 2212 RVA: 0x0001E448 File Offset: 0x0001C648
		private void OnUpdateGizmos()
		{
			if (!SpawnpointSystemV2.Get().IsVisible || !Level.isEditor)
			{
				return;
			}
			foreach (AirdropDevkitNode airdropDevkitNode in this.allNodes)
			{
				Color color = airdropDevkitNode.isSelected ? Color.yellow : Color.red;
				RuntimeGizmos.Get().Arrow(airdropDevkitNode.transform.position + airdropDevkitNode.transform.up * 32f, -airdropDevkitNode.transform.up, 32f, color, 0f, EGizmoLayer.World);
			}
		}

		// Token: 0x0400034D RID: 845
		private static AirdropDevkitNodeSystem instance;

		// Token: 0x0400034E RID: 846
		private List<AirdropDevkitNode> allNodes;

		// Token: 0x0400034F RID: 847
		private CustomSampler gizmoUpdateSampler;
	}
}
