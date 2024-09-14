using System;
using UnityEngine;

namespace SDG.Framework.Rendering
{
	// Token: 0x0200008A RID: 138
	public class GLUtility
	{
		// Token: 0x17000081 RID: 129
		// (get) Token: 0x06000358 RID: 856 RVA: 0x0000CC38 File Offset: 0x0000AE38
		public static Material LINE_FLAT_COLOR
		{
			get
			{
				if (GLUtility._LINE_FLAT_COLOR == null)
				{
					GLUtility._LINE_FLAT_COLOR = new Material(Shader.Find("GL/LineFlatColor"));
				}
				return GLUtility._LINE_FLAT_COLOR;
			}
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x06000359 RID: 857 RVA: 0x0000CC60 File Offset: 0x0000AE60
		public static Material LINE_CHECKERED_COLOR
		{
			get
			{
				if (GLUtility._LINE_CHECKERED_COLOR == null)
				{
					GLUtility._LINE_CHECKERED_COLOR = new Material(Shader.Find("GL/LineCheckeredColor"));
				}
				return GLUtility._LINE_CHECKERED_COLOR;
			}
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x0600035A RID: 858 RVA: 0x0000CC88 File Offset: 0x0000AE88
		public static Material LINE_DEPTH_CHECKERED_COLOR
		{
			get
			{
				if (GLUtility._LINE_DEPTH_CHECKERED_COLOR == null)
				{
					GLUtility._LINE_DEPTH_CHECKERED_COLOR = new Material(Shader.Find("GL/LineDepthCheckeredColor"));
				}
				return GLUtility._LINE_DEPTH_CHECKERED_COLOR;
			}
		}

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x0600035B RID: 859 RVA: 0x0000CCB0 File Offset: 0x0000AEB0
		public static Material LINE_CHECKERED_DEPTH_CUTOFF_COLOR
		{
			get
			{
				if (GLUtility._LINE_CHECKERED_DEPTH_CUTOFF_COLOR == null)
				{
					GLUtility._LINE_CHECKERED_DEPTH_CUTOFF_COLOR = new Material(Shader.Find("GL/LineCheckeredDepthCutoffColor"));
				}
				return GLUtility._LINE_CHECKERED_DEPTH_CUTOFF_COLOR;
			}
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x0600035C RID: 860 RVA: 0x0000CCD8 File Offset: 0x0000AED8
		public static Material LINE_DEPTH_CUTOFF_COLOR
		{
			get
			{
				if (GLUtility._LINE_DEPTH_CUTOFF_COLOR == null)
				{
					GLUtility._LINE_DEPTH_CUTOFF_COLOR = new Material(Shader.Find("GL/LineDepthCutoffColor"));
				}
				return GLUtility._LINE_DEPTH_CUTOFF_COLOR;
			}
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x0600035D RID: 861 RVA: 0x0000CD00 File Offset: 0x0000AF00
		public static Material TRI_FLAT_COLOR
		{
			get
			{
				if (GLUtility._TRI_FLAT_COLOR == null)
				{
					GLUtility._TRI_FLAT_COLOR = new Material(Shader.Find("GL/TriFlatColor"));
				}
				return GLUtility._TRI_FLAT_COLOR;
			}
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x0600035E RID: 862 RVA: 0x0000CD28 File Offset: 0x0000AF28
		public static Material TRI_CHECKERED_COLOR
		{
			get
			{
				if (GLUtility._TRI_CHECKERED_COLOR == null)
				{
					GLUtility._TRI_CHECKERED_COLOR = new Material(Shader.Find("GL/TriCheckeredColor"));
				}
				return GLUtility._TRI_CHECKERED_COLOR;
			}
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x0600035F RID: 863 RVA: 0x0000CD50 File Offset: 0x0000AF50
		public static Material TRI_DEPTH_CHECKERED_COLOR
		{
			get
			{
				if (GLUtility._TRI_DEPTH_CHECKERED_COLOR == null)
				{
					GLUtility._TRI_DEPTH_CHECKERED_COLOR = new Material(Shader.Find("GL/TriDepthCheckeredColor"));
				}
				return GLUtility._TRI_DEPTH_CHECKERED_COLOR;
			}
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x06000360 RID: 864 RVA: 0x0000CD78 File Offset: 0x0000AF78
		public static Material TRI_CHECKERED_DEPTH_CUTOFF_COLOR
		{
			get
			{
				if (GLUtility._TRI_CHECKERED_DEPTH_CUTOFF_COLOR == null)
				{
					GLUtility._TRI_CHECKERED_DEPTH_CUTOFF_COLOR = new Material(Shader.Find("GL/TriCheckeredDepthCutoffColor"));
				}
				return GLUtility._TRI_CHECKERED_DEPTH_CUTOFF_COLOR;
			}
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x06000361 RID: 865 RVA: 0x0000CDA0 File Offset: 0x0000AFA0
		public static Material TRI_DEPTH_CUTOFF_COLOR
		{
			get
			{
				if (GLUtility._TRI_DEPTH_CUTOFF_COLOR == null)
				{
					GLUtility._TRI_DEPTH_CUTOFF_COLOR = new Material(Shader.Find("GL/TriDepthCutoffColor"));
				}
				return GLUtility._TRI_DEPTH_CUTOFF_COLOR;
			}
		}

		// Token: 0x06000362 RID: 866 RVA: 0x0000CDC8 File Offset: 0x0000AFC8
		public static void line(Vector3 begin, Vector3 end)
		{
			GL.Vertex(GLUtility.matrix.MultiplyPoint3x4(begin));
			GL.Vertex(GLUtility.matrix.MultiplyPoint3x4(end));
		}

		// Token: 0x06000363 RID: 867 RVA: 0x0000CDEC File Offset: 0x0000AFEC
		public static void boxSolid(Vector3 center, Vector3 size)
		{
			Vector3 vector = size / 2f;
			GL.Vertex(GLUtility.matrix.MultiplyPoint3x4(center + new Vector3(-vector.x, -vector.y, vector.z)));
			GL.Vertex(GLUtility.matrix.MultiplyPoint3x4(center + new Vector3(-vector.x, vector.y, -vector.z)));
			GL.Vertex(GLUtility.matrix.MultiplyPoint3x4(center + new Vector3(-vector.x, -vector.y, -vector.z)));
			GL.Vertex(GLUtility.matrix.MultiplyPoint3x4(center + new Vector3(-vector.x, vector.y, -vector.z)));
			GL.Vertex(GLUtility.matrix.MultiplyPoint3x4(center + new Vector3(-vector.x, -vector.y, vector.z)));
			GL.Vertex(GLUtility.matrix.MultiplyPoint3x4(center + new Vector3(-vector.x, vector.y, vector.z)));
			GL.Vertex(GLUtility.matrix.MultiplyPoint3x4(center + new Vector3(vector.x, -vector.y, -vector.z)));
			GL.Vertex(GLUtility.matrix.MultiplyPoint3x4(center + new Vector3(vector.x, vector.y, -vector.z)));
			GL.Vertex(GLUtility.matrix.MultiplyPoint3x4(center + new Vector3(vector.x, -vector.y, vector.z)));
			GL.Vertex(GLUtility.matrix.MultiplyPoint3x4(center + new Vector3(vector.x, vector.y, vector.z)));
			GL.Vertex(GLUtility.matrix.MultiplyPoint3x4(center + new Vector3(vector.x, -vector.y, vector.z)));
			GL.Vertex(GLUtility.matrix.MultiplyPoint3x4(center + new Vector3(vector.x, vector.y, -vector.z)));
			GL.Vertex(GLUtility.matrix.MultiplyPoint3x4(center + new Vector3(-vector.x, -vector.y, -vector.z)));
			GL.Vertex(GLUtility.matrix.MultiplyPoint3x4(center + new Vector3(vector.x, -vector.y, -vector.z)));
			GL.Vertex(GLUtility.matrix.MultiplyPoint3x4(center + new Vector3(-vector.x, -vector.y, vector.z)));
			GL.Vertex(GLUtility.matrix.MultiplyPoint3x4(center + new Vector3(vector.x, -vector.y, vector.z)));
			GL.Vertex(GLUtility.matrix.MultiplyPoint3x4(center + new Vector3(-vector.x, -vector.y, vector.z)));
			GL.Vertex(GLUtility.matrix.MultiplyPoint3x4(center + new Vector3(vector.x, -vector.y, -vector.z)));
			GL.Vertex(GLUtility.matrix.MultiplyPoint3x4(center + new Vector3(-vector.x, vector.y, vector.z)));
			GL.Vertex(GLUtility.matrix.MultiplyPoint3x4(center + new Vector3(vector.x, vector.y, -vector.z)));
			GL.Vertex(GLUtility.matrix.MultiplyPoint3x4(center + new Vector3(-vector.x, vector.y, -vector.z)));
			GL.Vertex(GLUtility.matrix.MultiplyPoint3x4(center + new Vector3(vector.x, vector.y, -vector.z)));
			GL.Vertex(GLUtility.matrix.MultiplyPoint3x4(center + new Vector3(-vector.x, vector.y, vector.z)));
			GL.Vertex(GLUtility.matrix.MultiplyPoint3x4(center + new Vector3(vector.x, vector.y, vector.z)));
			GL.Vertex(GLUtility.matrix.MultiplyPoint3x4(center + new Vector3(-vector.x, -vector.y, -vector.z)));
			GL.Vertex(GLUtility.matrix.MultiplyPoint3x4(center + new Vector3(-vector.x, vector.y, -vector.z)));
			GL.Vertex(GLUtility.matrix.MultiplyPoint3x4(center + new Vector3(vector.x, -vector.y, -vector.z)));
			GL.Vertex(GLUtility.matrix.MultiplyPoint3x4(center + new Vector3(vector.x, vector.y, -vector.z)));
			GL.Vertex(GLUtility.matrix.MultiplyPoint3x4(center + new Vector3(vector.x, -vector.y, -vector.z)));
			GL.Vertex(GLUtility.matrix.MultiplyPoint3x4(center + new Vector3(-vector.x, vector.y, -vector.z)));
			GL.Vertex(GLUtility.matrix.MultiplyPoint3x4(center + new Vector3(vector.x, -vector.y, vector.z)));
			GL.Vertex(GLUtility.matrix.MultiplyPoint3x4(center + new Vector3(-vector.x, vector.y, vector.z)));
			GL.Vertex(GLUtility.matrix.MultiplyPoint3x4(center + new Vector3(-vector.x, -vector.y, vector.z)));
			GL.Vertex(GLUtility.matrix.MultiplyPoint3x4(center + new Vector3(-vector.x, vector.y, vector.z)));
			GL.Vertex(GLUtility.matrix.MultiplyPoint3x4(center + new Vector3(vector.x, -vector.y, vector.z)));
			GL.Vertex(GLUtility.matrix.MultiplyPoint3x4(center + new Vector3(vector.x, vector.y, vector.z)));
		}

		// Token: 0x06000364 RID: 868 RVA: 0x0000D46C File Offset: 0x0000B66C
		public static void circle(Vector3 center, float radius, Vector3 horizontalAxis, Vector3 verticalAxis, float steps = 0f)
		{
			float num = 6.2831855f;
			float num2 = 0f;
			if (steps == 0f)
			{
				steps = Mathf.Clamp(4f * radius, 8f, 128f);
			}
			float num3 = num / steps;
			Vector3 v = GLUtility.matrix.MultiplyPoint3x4(center + horizontalAxis * radius);
			while (num2 < num)
			{
				num2 += num3;
				float f = Mathf.Min(num2, num);
				float d = Mathf.Cos(f) * radius;
				float d2 = Mathf.Sin(f) * radius;
				Vector3 vector = GLUtility.matrix.MultiplyPoint3x4(center + horizontalAxis * d + verticalAxis * d2);
				GL.Vertex(v);
				GL.Vertex(vector);
				v = vector;
			}
		}

		// Token: 0x06000365 RID: 869 RVA: 0x0000D51C File Offset: 0x0000B71C
		public static void circle(Vector3 center, float radius, Vector3 horizontalAxis, Vector3 verticalAxis, GLCircleOffsetHandler handleGLCircleOffset)
		{
			if (handleGLCircleOffset == null)
			{
				return;
			}
			float num = 6.2831855f;
			float num2 = 0f;
			float num3 = num / Mathf.Clamp(4f * radius, 8f, 128f);
			Vector3 v = GLUtility.matrix.MultiplyPoint3x4(center + horizontalAxis * radius);
			handleGLCircleOffset(ref v);
			while (num2 < num)
			{
				num2 += num3;
				float f = Mathf.Min(num2, num);
				float d = Mathf.Cos(f) * radius;
				float d2 = Mathf.Sin(f) * radius;
				Vector3 vector = GLUtility.matrix.MultiplyPoint3x4(center + horizontalAxis * d + verticalAxis * d2);
				handleGLCircleOffset(ref vector);
				GL.Vertex(v);
				GL.Vertex(vector);
				v = vector;
			}
		}

		// Token: 0x04000162 RID: 354
		protected static Material _LINE_FLAT_COLOR;

		// Token: 0x04000163 RID: 355
		protected static Material _LINE_CHECKERED_COLOR;

		// Token: 0x04000164 RID: 356
		protected static Material _LINE_DEPTH_CHECKERED_COLOR;

		// Token: 0x04000165 RID: 357
		protected static Material _LINE_CHECKERED_DEPTH_CUTOFF_COLOR;

		// Token: 0x04000166 RID: 358
		protected static Material _LINE_DEPTH_CUTOFF_COLOR;

		// Token: 0x04000167 RID: 359
		protected static Material _TRI_FLAT_COLOR;

		// Token: 0x04000168 RID: 360
		protected static Material _TRI_CHECKERED_COLOR;

		// Token: 0x04000169 RID: 361
		protected static Material _TRI_DEPTH_CHECKERED_COLOR;

		// Token: 0x0400016A RID: 362
		protected static Material _TRI_CHECKERED_DEPTH_CUTOFF_COLOR;

		// Token: 0x0400016B RID: 363
		protected static Material _TRI_DEPTH_CUTOFF_COLOR;

		// Token: 0x0400016C RID: 364
		public static Matrix4x4 matrix;
	}
}
