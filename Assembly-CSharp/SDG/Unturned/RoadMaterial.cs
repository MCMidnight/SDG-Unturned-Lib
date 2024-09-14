using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200050B RID: 1291
	public class RoadMaterial
	{
		// Token: 0x1700082D RID: 2093
		// (get) Token: 0x06002889 RID: 10377 RVA: 0x000ACBF5 File Offset: 0x000AADF5
		public static Shader shader
		{
			get
			{
				if (RoadMaterial._shader == null)
				{
					RoadMaterial._shader = Shader.Find("Standard/Diffuse");
					if (RoadMaterial._shader == null)
					{
						UnturnedLog.error("Road Standard/Diffuse shader is missing!");
					}
				}
				return RoadMaterial._shader;
			}
		}

		// Token: 0x1700082E RID: 2094
		// (get) Token: 0x0600288A RID: 10378 RVA: 0x000ACC2F File Offset: 0x000AAE2F
		public Material material
		{
			get
			{
				return this._material;
			}
		}

		// Token: 0x0600288B RID: 10379 RVA: 0x000ACC38 File Offset: 0x000AAE38
		public RoadMaterial(Texture2D texture)
		{
			this._material = new Material(RoadMaterial.shader);
			this.material.name = "Road";
			this.material.mainTexture = texture;
			this.width = 4f;
			this.height = 1f;
			this.depth = 0.5f;
			this.offset = 0f;
			this.isConcrete = true;
		}

		// Token: 0x0400158B RID: 5515
		private static Shader _shader;

		// Token: 0x0400158C RID: 5516
		private Material _material;

		// Token: 0x0400158D RID: 5517
		public float width;

		// Token: 0x0400158E RID: 5518
		public float height;

		// Token: 0x0400158F RID: 5519
		public float depth;

		// Token: 0x04001590 RID: 5520
		public float offset;

		// Token: 0x04001591 RID: 5521
		public bool isConcrete;
	}
}
