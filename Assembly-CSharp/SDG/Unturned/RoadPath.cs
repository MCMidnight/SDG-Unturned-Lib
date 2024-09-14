using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000509 RID: 1289
	public class RoadPath
	{
		// Token: 0x06002860 RID: 10336 RVA: 0x000AA83F File Offset: 0x000A8A3F
		public void highlightVertex()
		{
			this.meshRenderer.material.color = Color.red;
		}

		// Token: 0x06002861 RID: 10337 RVA: 0x000AA856 File Offset: 0x000A8A56
		public void unhighlightVertex()
		{
			this.meshRenderer.material.color = Color.white;
		}

		// Token: 0x06002862 RID: 10338 RVA: 0x000AA86D File Offset: 0x000A8A6D
		public void highlightTangent(int index)
		{
			this.meshRenderers[index].material.color = Color.red;
			this.lineRenderers[index].material.color = Color.red;
		}

		// Token: 0x06002863 RID: 10339 RVA: 0x000AA8A0 File Offset: 0x000A8AA0
		public void unhighlightTangent(int index)
		{
			Color color;
			if (index == 0)
			{
				color = Color.yellow;
			}
			else
			{
				color = Color.blue;
			}
			this.meshRenderers[index].material.color = color;
			this.lineRenderers[index].material.color = color;
		}

		// Token: 0x06002864 RID: 10340 RVA: 0x000AA8E4 File Offset: 0x000A8AE4
		public void setTangent(int index, Vector3 tangent)
		{
			this.tangents[index].localPosition = tangent;
			this.lineRenderers[index].SetPosition(1, -tangent);
		}

		// Token: 0x06002865 RID: 10341 RVA: 0x000AA908 File Offset: 0x000A8B08
		public void remove()
		{
			Object.Destroy(this.vertex.gameObject);
		}

		// Token: 0x06002866 RID: 10342 RVA: 0x000AA91C File Offset: 0x000A8B1C
		public RoadPath(Transform vertex)
		{
			this.vertex = vertex;
			this.meshRenderer = vertex.GetComponent<MeshRenderer>();
			this.tangents = new Transform[2];
			this.tangents[0] = vertex.Find("Tangent_0");
			this.tangents[1] = vertex.Find("Tangent_1");
			this.meshRenderers = new MeshRenderer[2];
			this.meshRenderers[0] = this.tangents[0].GetComponent<MeshRenderer>();
			this.meshRenderers[1] = this.tangents[1].GetComponent<MeshRenderer>();
			this.lineRenderers = new LineRenderer[2];
			this.lineRenderers[0] = this.tangents[0].GetComponent<LineRenderer>();
			this.lineRenderers[1] = this.tangents[1].GetComponent<LineRenderer>();
			this.unhighlightVertex();
			this.unhighlightTangent(0);
			this.unhighlightTangent(1);
		}

		// Token: 0x0400157B RID: 5499
		public Transform vertex;

		// Token: 0x0400157C RID: 5500
		private MeshRenderer meshRenderer;

		// Token: 0x0400157D RID: 5501
		public Transform[] tangents;

		// Token: 0x0400157E RID: 5502
		private MeshRenderer[] meshRenderers;

		// Token: 0x0400157F RID: 5503
		private LineRenderer[] lineRenderers;
	}
}
