using System;
using UnityEngine;

// Token: 0x02000006 RID: 6
public class MeshContainer
{
	// Token: 0x0600000A RID: 10 RVA: 0x000020CD File Offset: 0x000002CD
	public MeshContainer(Mesh m)
	{
		this.mesh = m;
		this.vertices = m.vertices;
		this.normals = m.normals;
	}

	// Token: 0x0600000B RID: 11 RVA: 0x000020F4 File Offset: 0x000002F4
	public void Update()
	{
		this.mesh.vertices = this.vertices;
		this.mesh.normals = this.normals;
	}

	// Token: 0x04000002 RID: 2
	public Mesh mesh;

	// Token: 0x04000003 RID: 3
	public Vector3[] vertices;

	// Token: 0x04000004 RID: 4
	public Vector3[] normals;
}
