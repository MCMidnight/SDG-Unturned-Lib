using System;
using System.Collections.Generic;
using Pathfinding;
using SDG.Framework.Landscapes;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020004C4 RID: 1220
	public class Flag
	{
		// Token: 0x17000771 RID: 1905
		// (get) Token: 0x0600255B RID: 9563 RVA: 0x000947CD File Offset: 0x000929CD
		public Vector3 point
		{
			get
			{
				return this._point;
			}
		}

		// Token: 0x17000772 RID: 1906
		// (get) Token: 0x0600255C RID: 9564 RVA: 0x000947D5 File Offset: 0x000929D5
		public Transform model
		{
			get
			{
				return this._model;
			}
		}

		// Token: 0x17000773 RID: 1907
		// (get) Token: 0x0600255D RID: 9565 RVA: 0x000947DD File Offset: 0x000929DD
		public LineRenderer area
		{
			get
			{
				return this._area;
			}
		}

		// Token: 0x17000774 RID: 1908
		// (get) Token: 0x0600255E RID: 9566 RVA: 0x000947E5 File Offset: 0x000929E5
		public LineRenderer bounds
		{
			get
			{
				return this._bounds;
			}
		}

		// Token: 0x17000775 RID: 1909
		// (get) Token: 0x0600255F RID: 9567 RVA: 0x000947ED File Offset: 0x000929ED
		public RecastGraph graph
		{
			get
			{
				return this._graph;
			}
		}

		// Token: 0x17000776 RID: 1910
		// (get) Token: 0x06002560 RID: 9568 RVA: 0x000947F5 File Offset: 0x000929F5
		// (set) Token: 0x06002561 RID: 9569 RVA: 0x000947FD File Offset: 0x000929FD
		public FlagData data { get; private set; }

		// Token: 0x06002562 RID: 9570 RVA: 0x00094806 File Offset: 0x00092A06
		public void move(Vector3 newPoint)
		{
			this._point = newPoint;
			this.model.position = this.point;
			this.navmesh.transform.position = Vector3.zero;
		}

		// Token: 0x06002563 RID: 9571 RVA: 0x00094835 File Offset: 0x00092A35
		public void setEnabled(bool isEnabled)
		{
			this.model.gameObject.SetActive(isEnabled);
		}

		// Token: 0x06002564 RID: 9572 RVA: 0x00094848 File Offset: 0x00092A48
		public void buildMesh()
		{
			float num = Flag.MIN_SIZE + this.width * (Flag.MAX_SIZE - Flag.MIN_SIZE);
			float num2 = Flag.MIN_SIZE + this.height * (Flag.MAX_SIZE - Flag.MIN_SIZE);
			this.area.SetPosition(0, new Vector3(-num / 2f, 0f, -num2 / 2f));
			this.area.SetPosition(1, new Vector3(num / 2f, 0f, -num2 / 2f));
			this.area.SetPosition(2, new Vector3(num / 2f, 0f, num2 / 2f));
			this.area.SetPosition(3, new Vector3(-num / 2f, 0f, num2 / 2f));
			this.area.SetPosition(4, new Vector3(-num / 2f, 0f, -num2 / 2f));
			num += LevelNavigation.BOUNDS_SIZE.x;
			num2 += LevelNavigation.BOUNDS_SIZE.z;
			this.bounds.SetPosition(0, new Vector3(-num / 2f, 0f, -num2 / 2f));
			this.bounds.SetPosition(1, new Vector3(num / 2f, 0f, -num2 / 2f));
			this.bounds.SetPosition(2, new Vector3(num / 2f, 0f, num2 / 2f));
			this.bounds.SetPosition(3, new Vector3(-num / 2f, 0f, num2 / 2f));
			this.bounds.SetPosition(4, new Vector3(-num / 2f, 0f, -num2 / 2f));
		}

		// Token: 0x06002565 RID: 9573 RVA: 0x00094A15 File Offset: 0x00092C15
		public void remove()
		{
			AstarPath.active.astarData.RemoveGraph(this.graph);
			Object.Destroy(this.model.gameObject);
		}

		// Token: 0x06002566 RID: 9574 RVA: 0x00094A40 File Offset: 0x00092C40
		public void bakeNavigation()
		{
			LevelObjects.ImmediatelySyncRegionalVisibility();
			float x = Flag.MIN_SIZE + this.width * (Flag.MAX_SIZE - Flag.MIN_SIZE);
			float z = Flag.MIN_SIZE + this.height * (Flag.MAX_SIZE - Flag.MIN_SIZE);
			if (Level.info.configData.Use_Legacy_Water && LevelLighting.seaLevel < 0.99f && !Level.info.configData.Allow_Underwater_Features)
			{
				this.graph.forcedBoundsCenter = new Vector3(this.point.x, LevelLighting.seaLevel * Level.TERRAIN + (Level.TERRAIN - LevelLighting.seaLevel * Level.TERRAIN) / 2f - 0.625f, this.point.z);
				this.graph.forcedBoundsSize = new Vector3(x, Level.TERRAIN - LevelLighting.seaLevel * Level.TERRAIN + 1.25f, z);
			}
			else
			{
				this.graph.forcedBoundsCenter = new Vector3(this.point.x, 0f, this.point.z);
				this.graph.forcedBoundsSize = new Vector3(x, Landscape.TILE_HEIGHT, z);
			}
			AstarPath.active.ScanSpecific(this.graph);
			LevelNavigation.updateBounds();
		}

		// Token: 0x06002567 RID: 9575 RVA: 0x00094B8C File Offset: 0x00092D8C
		private void updateNavmesh()
		{
			if (Level.isEditor && this.graph != null)
			{
				List<Vector3> list = new List<Vector3>();
				List<int> list2 = new List<int>();
				List<Vector2> list3 = new List<Vector2>();
				RecastGraph.NavmeshTile[] tiles = this.graph.GetTiles();
				int num = 0;
				if (tiles == null)
				{
					return;
				}
				foreach (RecastGraph.NavmeshTile navmeshTile in tiles)
				{
					for (int j = 0; j < navmeshTile.verts.Length; j++)
					{
						Vector3 vector = (Vector3)navmeshTile.verts[j];
						vector.y += 0.1f;
						list.Add(vector);
						list3.Add(new Vector2(vector.x, vector.z));
					}
					for (int k = 0; k < navmeshTile.tris.Length; k++)
					{
						list2.Add(navmeshTile.tris[k] + num);
					}
					num += navmeshTile.verts.Length;
				}
				Mesh mesh = new Mesh();
				mesh.name = "Navmesh";
				mesh.vertices = list.ToArray();
				mesh.triangles = list2.ToArray();
				mesh.normals = new Vector3[list.Count];
				mesh.uv = list3.ToArray();
				this.navmesh.transform.position = Vector3.zero;
				this.navmesh.sharedMesh = mesh;
			}
		}

		// Token: 0x06002568 RID: 9576 RVA: 0x00094CF6 File Offset: 0x00092EF6
		private void OnGraphPostScan(NavGraph updated)
		{
			if (updated == this.graph)
			{
				this.needsNavigationSave = true;
				this.updateNavmesh();
			}
		}

		// Token: 0x06002569 RID: 9577 RVA: 0x00094D0E File Offset: 0x00092F0E
		private void setupGraph()
		{
			AstarPath.OnGraphPostScan = (OnGraphDelegate)Delegate.Combine(AstarPath.OnGraphPostScan, new OnGraphDelegate(this.OnGraphPostScan));
		}

		// Token: 0x0600256A RID: 9578 RVA: 0x00094D30 File Offset: 0x00092F30
		public Flag(Vector3 newPoint, RecastGraph newGraph, FlagData newData)
		{
			this._point = newPoint;
			this._model = ((GameObject)Object.Instantiate(Resources.Load("Edit/Flag"))).transform;
			this.model.name = "Flag";
			this.model.position = this.point;
			this._area = this.model.Find("Area").GetComponent<LineRenderer>();
			this._bounds = this.model.Find("Bounds").GetComponent<LineRenderer>();
			this.navmesh = this.model.Find("Navmesh").GetComponent<MeshFilter>();
			this.width = 0f;
			this.height = 0f;
			this._graph = newGraph;
			this.data = newData;
			this.setupGraph();
			this.buildMesh();
		}

		// Token: 0x0600256B RID: 9579 RVA: 0x00094E0C File Offset: 0x0009300C
		public Flag(Vector3 newPoint, float newWidth, float newHeight, RecastGraph newGraph, FlagData newData)
		{
			this._point = newPoint;
			this._model = ((GameObject)Object.Instantiate(Resources.Load("Edit/Flag"))).transform;
			this.model.name = "Flag";
			this.model.position = this.point;
			this._area = this.model.Find("Area").GetComponent<LineRenderer>();
			this._bounds = this.model.Find("Bounds").GetComponent<LineRenderer>();
			this.navmesh = this.model.Find("Navmesh").GetComponent<MeshFilter>();
			this.width = newWidth;
			this.height = newHeight;
			this._graph = newGraph;
			this.data = newData;
			this.setupGraph();
			this.buildMesh();
			this.updateNavmesh();
		}

		// Token: 0x04001325 RID: 4901
		public static readonly float MIN_SIZE = 32f;

		// Token: 0x04001326 RID: 4902
		public static readonly float MAX_SIZE = 1024f;

		// Token: 0x04001327 RID: 4903
		public float width;

		// Token: 0x04001328 RID: 4904
		public float height;

		// Token: 0x04001329 RID: 4905
		private Vector3 _point;

		// Token: 0x0400132A RID: 4906
		private Transform _model;

		// Token: 0x0400132B RID: 4907
		private MeshFilter navmesh;

		// Token: 0x0400132C RID: 4908
		private LineRenderer _area;

		// Token: 0x0400132D RID: 4909
		private LineRenderer _bounds;

		// Token: 0x0400132E RID: 4910
		private RecastGraph _graph;

		// Token: 0x04001330 RID: 4912
		public bool needsNavigationSave;
	}
}
