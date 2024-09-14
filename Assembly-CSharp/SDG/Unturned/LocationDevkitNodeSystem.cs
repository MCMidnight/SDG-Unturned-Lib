using System;
using System.Collections.Generic;
using SDG.Framework.Devkit;
using SDG.Framework.Utilities;
using UnityEngine;
using UnityEngine.Profiling;

namespace SDG.Unturned
{
	// Token: 0x0200015A RID: 346
	public class LocationDevkitNodeSystem : TempNodeSystemBase
	{
		// Token: 0x060008AD RID: 2221 RVA: 0x0001E629 File Offset: 0x0001C829
		public static LocationDevkitNodeSystem Get()
		{
			return LocationDevkitNodeSystem.instance;
		}

		// Token: 0x060008AE RID: 2222 RVA: 0x0001E630 File Offset: 0x0001C830
		public IReadOnlyList<LocationDevkitNode> GetAllNodes()
		{
			return this.allNodes;
		}

		// Token: 0x060008AF RID: 2223 RVA: 0x0001E638 File Offset: 0x0001C838
		public LocationDevkitNode FindByName(string id)
		{
			foreach (LocationDevkitNode locationDevkitNode in this.allNodes)
			{
				if (string.Equals(locationDevkitNode.locationName, id, 3))
				{
					return locationDevkitNode;
				}
			}
			return null;
		}

		// Token: 0x060008B0 RID: 2224 RVA: 0x0001E69C File Offset: 0x0001C89C
		internal override Type GetComponentType()
		{
			return typeof(LocationDevkitNode);
		}

		// Token: 0x060008B1 RID: 2225 RVA: 0x0001E6A8 File Offset: 0x0001C8A8
		internal override IEnumerable<GameObject> EnumerateGameObjects()
		{
			foreach (LocationDevkitNode locationDevkitNode in this.allNodes)
			{
				yield return locationDevkitNode.gameObject;
			}
			List<LocationDevkitNode>.Enumerator enumerator = default(List<LocationDevkitNode>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x060008B2 RID: 2226 RVA: 0x0001E6B8 File Offset: 0x0001C8B8
		internal void AddNode(LocationDevkitNode node)
		{
			this.allNodes.Add(node);
		}

		// Token: 0x060008B3 RID: 2227 RVA: 0x0001E6C6 File Offset: 0x0001C8C6
		internal void RemoveNode(LocationDevkitNode node)
		{
			this.allNodes.RemoveFast(node);
		}

		// Token: 0x060008B4 RID: 2228 RVA: 0x0001E6D5 File Offset: 0x0001C8D5
		internal LocationDevkitNodeSystem()
		{
			LocationDevkitNodeSystem.instance = this;
			this.allNodes = new List<LocationDevkitNode>();
			this.gizmoUpdateSampler = CustomSampler.Create("LocationDevkitNodeSystem.UpdateGizmos", false);
			TimeUtility.updated += this.OnUpdateGizmos;
		}

		// Token: 0x060008B5 RID: 2229 RVA: 0x0001E710 File Offset: 0x0001C910
		private void OnUpdateGizmos()
		{
			if (!SpawnpointSystemV2.Get().IsVisible || !Level.isEditor)
			{
				return;
			}
			foreach (LocationDevkitNode locationDevkitNode in this.allNodes)
			{
				Color color = locationDevkitNode.isSelected ? Color.yellow : Color.red;
				RuntimeGizmos.Get().Cube(locationDevkitNode.transform.position, locationDevkitNode.transform.rotation, 1.5f, color, 0f, EGizmoLayer.World);
			}
		}

		// Token: 0x04000353 RID: 851
		private static LocationDevkitNodeSystem instance;

		// Token: 0x04000354 RID: 852
		private List<LocationDevkitNode> allNodes;

		// Token: 0x04000355 RID: 853
		private CustomSampler gizmoUpdateSampler;
	}
}
