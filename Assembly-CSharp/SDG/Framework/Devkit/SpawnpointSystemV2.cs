using System;
using System.Collections.Generic;
using SDG.Framework.Utilities;
using SDG.Unturned;
using UnityEngine;

namespace SDG.Framework.Devkit
{
	// Token: 0x0200012A RID: 298
	public class SpawnpointSystemV2 : TempNodeSystemBase
	{
		// Token: 0x0600079B RID: 1947 RVA: 0x0001BB93 File Offset: 0x00019D93
		public static SpawnpointSystemV2 Get()
		{
			return SpawnpointSystemV2.instance;
		}

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x0600079C RID: 1948 RVA: 0x0001BB9A File Offset: 0x00019D9A
		// (set) Token: 0x0600079D RID: 1949 RVA: 0x0001BBA4 File Offset: 0x00019DA4
		public bool IsVisible
		{
			get
			{
				return this._isVisible;
			}
			set
			{
				if (this._isVisible != value)
				{
					this._isVisible = value;
					ConvenientSavedata.get().write("Visibility_Spawnpoints", value);
					if (Level.isEditor)
					{
						foreach (AirdropDevkitNode airdropDevkitNode in AirdropDevkitNodeSystem.Get().GetAllNodes())
						{
							airdropDevkitNode.UpdateEditorVisibility();
						}
						foreach (LocationDevkitNode locationDevkitNode in LocationDevkitNodeSystem.Get().GetAllNodes())
						{
							locationDevkitNode.UpdateEditorVisibility();
						}
						foreach (Spawnpoint spawnpoint in this.spawnpoints)
						{
							spawnpoint.UpdateEditorVisibility();
						}
					}
				}
			}
		}

		// Token: 0x0600079E RID: 1950 RVA: 0x0001BC9C File Offset: 0x00019E9C
		public IReadOnlyList<Spawnpoint> GetAllSpawnpoints()
		{
			return this.spawnpoints;
		}

		// Token: 0x0600079F RID: 1951 RVA: 0x0001BCA4 File Offset: 0x00019EA4
		public Spawnpoint FindSpawnpoint(string id)
		{
			if (string.IsNullOrEmpty(id))
			{
				return null;
			}
			return this.spawnpoints.Find((Spawnpoint x) => x.id.Equals(id, 3));
		}

		// Token: 0x060007A0 RID: 1952 RVA: 0x0001BCE4 File Offset: 0x00019EE4
		internal override Type GetComponentType()
		{
			return typeof(Spawnpoint);
		}

		// Token: 0x060007A1 RID: 1953 RVA: 0x0001BCF0 File Offset: 0x00019EF0
		internal override IEnumerable<GameObject> EnumerateGameObjects()
		{
			foreach (Spawnpoint spawnpoint in this.spawnpoints)
			{
				yield return spawnpoint.gameObject;
			}
			List<Spawnpoint>.Enumerator enumerator = default(List<Spawnpoint>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x060007A2 RID: 1954 RVA: 0x0001BD00 File Offset: 0x00019F00
		internal void AddSpawnpoint(Spawnpoint spawnpoint)
		{
			this.spawnpoints.Add(spawnpoint);
		}

		// Token: 0x060007A3 RID: 1955 RVA: 0x0001BD0E File Offset: 0x00019F0E
		internal void RemoveSpawnpoint(Spawnpoint spawnpoint)
		{
			this.spawnpoints.RemoveFast(spawnpoint);
		}

		// Token: 0x060007A4 RID: 1956 RVA: 0x0001BD20 File Offset: 0x00019F20
		internal SpawnpointSystemV2()
		{
			SpawnpointSystemV2.instance = this;
			this.spawnpoints = new List<Spawnpoint>();
			TimeUtility.updated += this.OnUpdateGizmos;
			bool isVisible;
			if (ConvenientSavedata.get().read("Visibility_Nodes", out isVisible))
			{
				this._isVisible = isVisible;
				return;
			}
			this._isVisible = true;
		}

		// Token: 0x060007A5 RID: 1957 RVA: 0x0001BD78 File Offset: 0x00019F78
		private void OnUpdateGizmos()
		{
			if (!this._isVisible || !Level.isEditor)
			{
				return;
			}
			foreach (Spawnpoint spawnpoint in this.spawnpoints)
			{
				Color color = spawnpoint.isSelected ? Color.yellow : Color.red;
				Matrix4x4 localToWorldMatrix = spawnpoint.transform.localToWorldMatrix;
				RuntimeGizmos.Get().Line(localToWorldMatrix.MultiplyPoint3x4(new Vector3(-0.5f, 0f, 0f)), localToWorldMatrix.MultiplyPoint3x4(new Vector3(0.5f, 0f, 0f)), color, 0f, EGizmoLayer.World);
				RuntimeGizmos.Get().Line(localToWorldMatrix.MultiplyPoint3x4(new Vector3(0f, -0.5f, 0f)), localToWorldMatrix.MultiplyPoint3x4(new Vector3(0f, 0.5f, 0f)), color, 0f, EGizmoLayer.World);
				RuntimeGizmos.Get().ArrowFromTo(localToWorldMatrix.MultiplyPoint3x4(new Vector3(0f, 0f, -0.5f)), localToWorldMatrix.MultiplyPoint3x4(new Vector3(0f, 0f, 1f)), color, 0f, EGizmoLayer.World);
			}
		}

		// Token: 0x040002C7 RID: 711
		private bool _isVisible;

		// Token: 0x040002C8 RID: 712
		private static SpawnpointSystemV2 instance;

		// Token: 0x040002C9 RID: 713
		internal List<Spawnpoint> spawnpoints;
	}
}
