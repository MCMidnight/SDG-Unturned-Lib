using System;
using System.Collections.Generic;
using SDG.Framework.Rendering;
using UnityEngine;
using UnityEngine.Rendering;

namespace SDG.Unturned
{
	/// <summary>
	/// In-game debug drawing utility similar to Unity's editor Gizmos.
	/// </summary>
	// Token: 0x02000817 RID: 2071
	public class RuntimeGizmos : MonoBehaviour
	{
		// Token: 0x060046AB RID: 18091 RVA: 0x001A53D4 File Offset: 0x001A35D4
		public static RuntimeGizmos Get()
		{
			if (RuntimeGizmos.instance == null)
			{
				GameObject gameObject = new GameObject("GizmoSingleton");
				Object.DontDestroyOnLoad(gameObject);
				gameObject.hideFlags = HideFlags.DontSave;
				RuntimeGizmos.instance = gameObject.AddComponent<RuntimeGizmos>();
				RuntimeGizmos.instance.materialLayers = new Material[2];
				RuntimeGizmos.instance.materialLayers[0] = GLUtility.LINE_DEPTH_CHECKERED_COLOR;
				RuntimeGizmos.instance.materialLayers[1] = GLUtility.LINE_FLAT_COLOR;
			}
			return RuntimeGizmos.instance;
		}

		// Token: 0x060046AC RID: 18092 RVA: 0x001A5447 File Offset: 0x001A3647
		public void Box(Vector3 center, Vector3 size, Color color, float lifespan = 0f, EGizmoLayer layer = EGizmoLayer.World)
		{
			this.boxLayers[(int)layer].Add(new RuntimeGizmos.BoxData(Matrix4x4.Translate(center), Vector3.zero, size, color, lifespan));
		}

		// Token: 0x060046AD RID: 18093 RVA: 0x001A546B File Offset: 0x001A366B
		public void Box(Vector3 center, Quaternion rotation, Vector3 size, Color color, float lifespan = 0f, EGizmoLayer layer = EGizmoLayer.World)
		{
			this.boxLayers[(int)layer].Add(new RuntimeGizmos.BoxData(Matrix4x4.TRS(center, rotation, Vector3.one), Vector3.zero, size, color, lifespan));
		}

		// Token: 0x060046AE RID: 18094 RVA: 0x001A5496 File Offset: 0x001A3696
		public void Box(Matrix4x4 matrix, Vector3 size, Color color, float lifespan = 0f, EGizmoLayer layer = EGizmoLayer.World)
		{
			this.boxLayers[(int)layer].Add(new RuntimeGizmos.BoxData(matrix, Vector3.zero, size, color, lifespan));
		}

		/// <param name="center">Local space relative to matrix.</param>
		// Token: 0x060046AF RID: 18095 RVA: 0x001A54B5 File Offset: 0x001A36B5
		public void Box(Matrix4x4 matrix, Vector3 center, Vector3 size, Color color, float lifespan = 0f, EGizmoLayer layer = EGizmoLayer.World)
		{
			this.boxLayers[(int)layer].Add(new RuntimeGizmos.BoxData(matrix, center, size, color, lifespan));
		}

		// Token: 0x060046B0 RID: 18096 RVA: 0x001A54D1 File Offset: 0x001A36D1
		public void Cube(Vector3 center, float size, Color color, float lifespan = 0f, EGizmoLayer layer = EGizmoLayer.World)
		{
			this.Box(center, new Vector3(size, size, size), color, lifespan, EGizmoLayer.World);
		}

		// Token: 0x060046B1 RID: 18097 RVA: 0x001A54E6 File Offset: 0x001A36E6
		public void Cube(Vector3 center, Quaternion rotation, float size, Color color, float lifespan = 0f, EGizmoLayer layer = EGizmoLayer.World)
		{
			this.Box(center, rotation, new Vector3(size, size, size), color, lifespan, EGizmoLayer.World);
		}

		// Token: 0x060046B2 RID: 18098 RVA: 0x001A54FD File Offset: 0x001A36FD
		public void Line(Vector3 begin, Vector3 end, Color color, float lifespan = 0f, EGizmoLayer layer = EGizmoLayer.World)
		{
			this.lineLayers[(int)layer].Add(new RuntimeGizmos.LineData(begin, end, color, lifespan));
		}

		// Token: 0x060046B3 RID: 18099 RVA: 0x001A5518 File Offset: 0x001A3718
		public void LineToward(Vector3 begin, Vector3 end, float length, Color color, float lifespan = 0f, EGizmoLayer layer = EGizmoLayer.World)
		{
			Vector3 normalized = (end - begin).normalized;
			this.lineLayers[(int)layer].Add(new RuntimeGizmos.LineData(begin, begin + normalized * length, color, lifespan));
		}

		// Token: 0x060046B4 RID: 18100 RVA: 0x001A555C File Offset: 0x001A375C
		public void Arrow(Vector3 origin, Vector3 direction, float length, Color color, float lifespan = 0f, EGizmoLayer layer = EGizmoLayer.World)
		{
			Vector3 rhs;
			if (MainCamera.instance != null)
			{
				rhs = (origin - MainCamera.instance.transform.position).normalized;
			}
			else
			{
				rhs = Vector3.up;
			}
			Vector3 b = Vector3.Cross(direction, rhs).normalized * 0.1f * length;
			Vector3 a = origin + direction * 0.75f * length;
			Vector3 end = origin + direction * length;
			this.lineLayers[(int)layer].Add(new RuntimeGizmos.LineData(origin, end, color, lifespan));
			this.lineLayers[(int)layer].Add(new RuntimeGizmos.LineData(a - b, end, color, lifespan));
			this.lineLayers[(int)layer].Add(new RuntimeGizmos.LineData(a + b, end, color, lifespan));
		}

		// Token: 0x060046B5 RID: 18101 RVA: 0x001A563C File Offset: 0x001A383C
		public void ArrowFromTo(Vector3 begin, Vector3 end, Color color, float lifespan = 0f, EGizmoLayer layer = EGizmoLayer.World)
		{
			Vector3 a = end - begin;
			float magnitude = a.magnitude;
			if (magnitude > 0.001f)
			{
				Vector3 direction = a / magnitude;
				this.Arrow(begin, direction, magnitude, color, lifespan, layer);
			}
		}

		// Token: 0x060046B6 RID: 18102 RVA: 0x001A5677 File Offset: 0x001A3877
		public void Capsule(Vector3 begin, Vector3 end, float radius, Color color, float lifespan = 0f, EGizmoLayer layer = EGizmoLayer.World)
		{
			this.capsuleLayers[(int)layer].Add(new RuntimeGizmos.CapsuleData(begin, end, color, lifespan, radius));
		}

		// Token: 0x060046B7 RID: 18103 RVA: 0x001A5693 File Offset: 0x001A3893
		public void Sphere(Vector3 center, float radius, Color color, float lifespan = 0f, EGizmoLayer layer = EGizmoLayer.World)
		{
			this.sphereLayers[(int)layer].Add(new RuntimeGizmos.SphereData(Matrix4x4.Translate(center), Vector3.zero, color, lifespan, radius));
		}

		// Token: 0x060046B8 RID: 18104 RVA: 0x001A56B7 File Offset: 0x001A38B7
		public void Sphere(Matrix4x4 matrix, float radius, Color color, float lifespan = 0f, EGizmoLayer layer = EGizmoLayer.World)
		{
			this.sphereLayers[(int)layer].Add(new RuntimeGizmos.SphereData(matrix, Vector3.zero, color, lifespan, radius));
		}

		// Token: 0x060046B9 RID: 18105 RVA: 0x001A56D6 File Offset: 0x001A38D6
		public void Circle(Vector3 center, Vector3 axisU, Vector3 axisV, float radius, Color color, float lifespan = 0f, int resolution = 0, EGizmoLayer layer = EGizmoLayer.World)
		{
			this.circleLayers[(int)layer].Add(new RuntimeGizmos.CircleData(center, axisU, axisV, color, lifespan, radius, resolution));
		}

		// Token: 0x060046BA RID: 18106 RVA: 0x001A56F6 File Offset: 0x001A38F6
		public void Raycast(Ray ray, float maxDistance, RaycastHit hit, Color rayColor, Color hitColor, float lifespan = 0f, EGizmoLayer layer = EGizmoLayer.World)
		{
			this.Linecast(ray.origin, ray.origin + ray.direction * maxDistance, hit, rayColor, hitColor, lifespan, EGizmoLayer.World);
		}

		// Token: 0x060046BB RID: 18107 RVA: 0x001A5728 File Offset: 0x001A3928
		public void Linecast(Vector3 start, Vector3 end, RaycastHit hit, Color rayColor, Color hitColor, float lifespan = 0f, EGizmoLayer layer = EGizmoLayer.World)
		{
			if (hit.collider == null)
			{
				this.Line(start, end, rayColor, lifespan, EGizmoLayer.World);
				return;
			}
			this.Line(start, hit.point, hitColor, lifespan, EGizmoLayer.World);
			this.Line(hit.point, end, rayColor, lifespan, EGizmoLayer.World);
		}

		// Token: 0x060046BC RID: 18108 RVA: 0x001A5778 File Offset: 0x001A3978
		public void Spherecast(Ray ray, float radius, float maxDistance, Color color, float lifespan = 0f, EGizmoLayer layer = EGizmoLayer.World)
		{
			this.Capsule(ray.origin, ray.origin + ray.direction * maxDistance, radius, color, lifespan, EGizmoLayer.World);
		}

		// Token: 0x060046BD RID: 18109 RVA: 0x001A57A8 File Offset: 0x001A39A8
		public void Spherecast(Ray ray, float radius, float maxDistance, RaycastHit hit, Color rayColor, Color hitColor, float lifespan = 0f, EGizmoLayer layer = EGizmoLayer.World)
		{
			if (hit.collider == null)
			{
				this.Capsule(ray.origin, ray.origin + ray.direction * maxDistance, radius, rayColor, lifespan, EGizmoLayer.World);
				return;
			}
			Vector3 vector = ray.origin + ray.direction * hit.distance;
			this.Capsule(ray.origin, vector, radius, hitColor, lifespan, EGizmoLayer.World);
			this.Capsule(vector, ray.origin + ray.direction * maxDistance, radius, rayColor, lifespan, EGizmoLayer.World);
		}

		// Token: 0x060046BE RID: 18110 RVA: 0x001A584B File Offset: 0x001A3A4B
		public void Label(Vector3 position, string content, float lifespan = 0f, EGizmoLayer layer = EGizmoLayer.World)
		{
			if (string.IsNullOrEmpty(content))
			{
				return;
			}
			this.labelsToRender.Add(new RuntimeGizmos.LabelData(position, content, Color.white, lifespan));
		}

		// Token: 0x060046BF RID: 18111 RVA: 0x001A586E File Offset: 0x001A3A6E
		public void Label(Vector3 position, string content, Color color, float lifespan = 0f, EGizmoLayer layer = EGizmoLayer.World)
		{
			if (string.IsNullOrEmpty(content))
			{
				return;
			}
			this.labelsToRender.Add(new RuntimeGizmos.LabelData(position, content, color, lifespan));
		}

		/// <summary>
		/// Wireframe grid on the XZ plane.
		/// </summary>
		// Token: 0x060046C0 RID: 18112 RVA: 0x001A5890 File Offset: 0x001A3A90
		public void GridXZ(Vector3 center, float size, int cells, Color color, float lifespan = 0f, EGizmoLayer layer = EGizmoLayer.World)
		{
			Vector3 a = center - new Vector3(size * 0.5f, 0f, size * 0.5f);
			float num = size / (float)cells;
			for (int i = 0; i <= cells; i++)
			{
				float num2 = num * (float)i;
				this.Line(a + new Vector3(num2, 0f, 0f), a + new Vector3(num2, 0f, size), color, lifespan, EGizmoLayer.World);
				this.Line(a + new Vector3(0f, 0f, num2), a + new Vector3(size, 0f, num2), color, lifespan, EGizmoLayer.World);
			}
		}

		// Token: 0x17000BA1 RID: 2977
		// (get) Token: 0x060046C1 RID: 18113 RVA: 0x001A5938 File Offset: 0x001A3B38
		public bool HasQueuedElements
		{
			get
			{
				if (RuntimeGizmos.clUseLineRenderers.value)
				{
					return false;
				}
				bool flag = false;
				for (int i = 0; i < 2; i++)
				{
					flag |= (this.boxLayers[i].Count > 0);
					flag |= (this.lineLayers[i].Count > 0);
					flag |= (this.capsuleLayers[i].Count > 0);
					flag |= (this.sphereLayers[i].Count > 0);
					flag |= (this.circleLayers[i].Count > 0);
				}
				return flag;
			}
		}

		// Token: 0x060046C2 RID: 18114 RVA: 0x001A59C4 File Offset: 0x001A3BC4
		public void Render()
		{
			this.renderTime = Time.time;
			Camera camera = MainCamera.instance;
			if (camera != null)
			{
				this.mainCameraPosition = camera.transform.position;
				this.cullDistance = camera.farClipPlane;
				this.sqrCullDistance = this.cullDistance * this.cullDistance;
			}
			else
			{
				this.mainCameraPosition = Vector3.zero;
				this.cullDistance = 0f;
				this.sqrCullDistance = 0f;
			}
			for (int i = 0; i < 2; i++)
			{
				this.materialLayers[i].SetPass(0);
				this.RenderBoxes(this.boxLayers[i]);
				this.RenderLines(this.lineLayers[i]);
				this.RenderCapsules(this.capsuleLayers[i]);
				this.RenderSpheres(this.sphereLayers[i]);
				this.RenderCircles(this.circleLayers[i]);
			}
		}

		// Token: 0x060046C3 RID: 18115 RVA: 0x001A5AA0 File Offset: 0x001A3CA0
		private void RenderBoxes(List<RuntimeGizmos.BoxData> boxesToRender)
		{
			GL.Begin(1);
			for (int i = boxesToRender.Count - 1; i >= 0; i--)
			{
				RuntimeGizmos.BoxData boxData = boxesToRender[i];
				GL.Color(boxData.color);
				Vector3 extents = boxData.extents;
				GL.Vertex(boxData.matrix.MultiplyPoint3x4(boxData.localCenter + new Vector3(-extents.x, -extents.y, -extents.z)));
				GL.Vertex(boxData.matrix.MultiplyPoint3x4(boxData.localCenter + new Vector3(extents.x, -extents.y, -extents.z)));
				GL.Vertex(boxData.matrix.MultiplyPoint3x4(boxData.localCenter + new Vector3(-extents.x, -extents.y, -extents.z)));
				GL.Vertex(boxData.matrix.MultiplyPoint3x4(boxData.localCenter + new Vector3(-extents.x, -extents.y, extents.z)));
				GL.Vertex(boxData.matrix.MultiplyPoint3x4(boxData.localCenter + new Vector3(-extents.x, -extents.y, extents.z)));
				GL.Vertex(boxData.matrix.MultiplyPoint3x4(boxData.localCenter + new Vector3(extents.x, -extents.y, extents.z)));
				GL.Vertex(boxData.matrix.MultiplyPoint3x4(boxData.localCenter + new Vector3(extents.x, -extents.y, -extents.z)));
				GL.Vertex(boxData.matrix.MultiplyPoint3x4(boxData.localCenter + new Vector3(extents.x, -extents.y, extents.z)));
				GL.Vertex(boxData.matrix.MultiplyPoint3x4(boxData.localCenter + new Vector3(-extents.x, -extents.y, -extents.z)));
				GL.Vertex(boxData.matrix.MultiplyPoint3x4(boxData.localCenter + new Vector3(-extents.x, extents.y, -extents.z)));
				GL.Vertex(boxData.matrix.MultiplyPoint3x4(boxData.localCenter + new Vector3(extents.x, -extents.y, -extents.z)));
				GL.Vertex(boxData.matrix.MultiplyPoint3x4(boxData.localCenter + new Vector3(extents.x, extents.y, -extents.z)));
				GL.Vertex(boxData.matrix.MultiplyPoint3x4(boxData.localCenter + new Vector3(-extents.x, -extents.y, extents.z)));
				GL.Vertex(boxData.matrix.MultiplyPoint3x4(boxData.localCenter + new Vector3(-extents.x, extents.y, extents.z)));
				GL.Vertex(boxData.matrix.MultiplyPoint3x4(boxData.localCenter + new Vector3(extents.x, -extents.y, extents.z)));
				GL.Vertex(boxData.matrix.MultiplyPoint3x4(boxData.localCenter + new Vector3(extents.x, extents.y, extents.z)));
				GL.Vertex(boxData.matrix.MultiplyPoint3x4(boxData.localCenter + new Vector3(-extents.x, extents.y, -extents.z)));
				GL.Vertex(boxData.matrix.MultiplyPoint3x4(boxData.localCenter + new Vector3(extents.x, extents.y, -extents.z)));
				GL.Vertex(boxData.matrix.MultiplyPoint3x4(boxData.localCenter + new Vector3(-extents.x, extents.y, -extents.z)));
				GL.Vertex(boxData.matrix.MultiplyPoint3x4(boxData.localCenter + new Vector3(-extents.x, extents.y, extents.z)));
				GL.Vertex(boxData.matrix.MultiplyPoint3x4(boxData.localCenter + new Vector3(-extents.x, extents.y, extents.z)));
				GL.Vertex(boxData.matrix.MultiplyPoint3x4(boxData.localCenter + new Vector3(extents.x, extents.y, extents.z)));
				GL.Vertex(boxData.matrix.MultiplyPoint3x4(boxData.localCenter + new Vector3(extents.x, extents.y, -extents.z)));
				GL.Vertex(boxData.matrix.MultiplyPoint3x4(boxData.localCenter + new Vector3(extents.x, extents.y, extents.z)));
				if (this.renderTime >= boxData.expireAfter)
				{
					boxesToRender.RemoveAtFast(i);
				}
			}
			GL.End();
		}

		// Token: 0x060046C4 RID: 18116 RVA: 0x001A5FEC File Offset: 0x001A41EC
		private void RenderBoxesUsingLineRenderers(List<RuntimeGizmos.BoxData> boxesToRender)
		{
			for (int i = boxesToRender.Count - 1; i >= 0; i--)
			{
				RuntimeGizmos.BoxData boxData = boxesToRender[i];
				Vector3 extents = boxData.extents;
				this.DrawLineUsingLineRenderer(boxData.matrix.MultiplyPoint3x4(boxData.localCenter + new Vector3(-extents.x, -extents.y, -extents.z)), boxData.matrix.MultiplyPoint3x4(boxData.localCenter + new Vector3(extents.x, -extents.y, -extents.z)), boxData.color);
				this.DrawLineUsingLineRenderer(boxData.matrix.MultiplyPoint3x4(boxData.localCenter + new Vector3(-extents.x, -extents.y, -extents.z)), boxData.matrix.MultiplyPoint3x4(boxData.localCenter + new Vector3(-extents.x, -extents.y, extents.z)), boxData.color);
				this.DrawLineUsingLineRenderer(boxData.matrix.MultiplyPoint3x4(boxData.localCenter + new Vector3(-extents.x, -extents.y, extents.z)), boxData.matrix.MultiplyPoint3x4(boxData.localCenter + new Vector3(extents.x, -extents.y, extents.z)), boxData.color);
				this.DrawLineUsingLineRenderer(boxData.matrix.MultiplyPoint3x4(boxData.localCenter + new Vector3(extents.x, -extents.y, -extents.z)), boxData.matrix.MultiplyPoint3x4(boxData.localCenter + new Vector3(extents.x, -extents.y, extents.z)), boxData.color);
				this.DrawLineUsingLineRenderer(boxData.matrix.MultiplyPoint3x4(boxData.localCenter + new Vector3(-extents.x, -extents.y, -extents.z)), boxData.matrix.MultiplyPoint3x4(boxData.localCenter + new Vector3(-extents.x, extents.y, -extents.z)), boxData.color);
				this.DrawLineUsingLineRenderer(boxData.matrix.MultiplyPoint3x4(boxData.localCenter + new Vector3(extents.x, -extents.y, -extents.z)), boxData.matrix.MultiplyPoint3x4(boxData.localCenter + new Vector3(extents.x, extents.y, -extents.z)), boxData.color);
				this.DrawLineUsingLineRenderer(boxData.matrix.MultiplyPoint3x4(boxData.localCenter + new Vector3(-extents.x, -extents.y, extents.z)), boxData.matrix.MultiplyPoint3x4(boxData.localCenter + new Vector3(-extents.x, extents.y, extents.z)), boxData.color);
				this.DrawLineUsingLineRenderer(boxData.matrix.MultiplyPoint3x4(boxData.localCenter + new Vector3(extents.x, -extents.y, extents.z)), boxData.matrix.MultiplyPoint3x4(boxData.localCenter + new Vector3(extents.x, extents.y, extents.z)), boxData.color);
				this.DrawLineUsingLineRenderer(boxData.matrix.MultiplyPoint3x4(boxData.localCenter + new Vector3(-extents.x, extents.y, -extents.z)), boxData.matrix.MultiplyPoint3x4(boxData.localCenter + new Vector3(extents.x, extents.y, -extents.z)), boxData.color);
				this.DrawLineUsingLineRenderer(boxData.matrix.MultiplyPoint3x4(boxData.localCenter + new Vector3(-extents.x, extents.y, -extents.z)), boxData.matrix.MultiplyPoint3x4(boxData.localCenter + new Vector3(-extents.x, extents.y, extents.z)), boxData.color);
				this.DrawLineUsingLineRenderer(boxData.matrix.MultiplyPoint3x4(boxData.localCenter + new Vector3(-extents.x, extents.y, extents.z)), boxData.matrix.MultiplyPoint3x4(boxData.localCenter + new Vector3(extents.x, extents.y, extents.z)), boxData.color);
				this.DrawLineUsingLineRenderer(boxData.matrix.MultiplyPoint3x4(boxData.localCenter + new Vector3(extents.x, extents.y, -extents.z)), boxData.matrix.MultiplyPoint3x4(boxData.localCenter + new Vector3(extents.x, extents.y, extents.z)), boxData.color);
				if (this.renderTime >= boxData.expireAfter)
				{
					boxesToRender.RemoveAtFast(i);
				}
			}
		}

		// Token: 0x060046C5 RID: 18117 RVA: 0x001A653C File Offset: 0x001A473C
		private void RenderLines(List<RuntimeGizmos.LineData> linesToRender)
		{
			GL.Begin(1);
			for (int i = linesToRender.Count - 1; i >= 0; i--)
			{
				RuntimeGizmos.LineData lineData = linesToRender[i];
				GL.Color(lineData.color);
				GL.Vertex(lineData.begin);
				GL.Vertex(lineData.end);
				if (this.renderTime >= lineData.expireAfter)
				{
					linesToRender.RemoveAtFast(i);
				}
			}
			GL.End();
		}

		// Token: 0x060046C6 RID: 18118 RVA: 0x001A65A8 File Offset: 0x001A47A8
		private void RenderLinesUsingLineRenderer(List<RuntimeGizmos.LineData> linesToRender)
		{
			for (int i = linesToRender.Count - 1; i >= 0; i--)
			{
				RuntimeGizmos.LineData lineData = linesToRender[i];
				this.DrawLineUsingLineRenderer(lineData.begin, lineData.end, lineData.color);
				if (this.renderTime >= lineData.expireAfter)
				{
					linesToRender.RemoveAtFast(i);
				}
			}
		}

		// Token: 0x060046C7 RID: 18119 RVA: 0x001A6600 File Offset: 0x001A4800
		private void RenderCapsules(List<RuntimeGizmos.CapsuleData> capsulesToRender)
		{
			for (int i = capsulesToRender.Count - 1; i >= 0; i--)
			{
				RuntimeGizmos.CapsuleData capsuleData = capsulesToRender[i];
				Vector3 vector = capsuleData.end - capsuleData.begin;
				vector.Normalize();
				Vector3 vector2 = Vector3.Cross(vector, (Mathf.Abs(Vector3.Dot(vector, Vector3.up)) > 0.95f) ? Vector3.forward : Vector3.up);
				Vector3 vector3 = Vector3.Cross(vector, vector2);
				int num = Mathf.Clamp(Mathf.RoundToInt(8f * capsuleData.radius), 8, 64);
				this.RenderCircle(capsuleData.begin, vector2, vector3, capsuleData.radius, num, capsuleData.color);
				this.RenderCircle(capsuleData.end, vector2, vector3, capsuleData.radius, num, capsuleData.color);
				int resolution = num / 2;
				this.RenderSemicircle(capsuleData.begin, vector2, -vector, capsuleData.radius, resolution, capsuleData.color);
				this.RenderSemicircle(capsuleData.begin, vector3, -vector, capsuleData.radius, resolution, capsuleData.color);
				this.RenderSemicircle(capsuleData.end, vector2, vector, capsuleData.radius, resolution, capsuleData.color);
				this.RenderSemicircle(capsuleData.end, vector3, vector, capsuleData.radius, resolution, capsuleData.color);
				GL.Begin(1);
				GL.Color(capsuleData.color);
				GL.Vertex(capsuleData.begin + vector2 * capsuleData.radius);
				GL.Vertex(capsuleData.end + vector2 * capsuleData.radius);
				GL.Vertex(capsuleData.begin - vector2 * capsuleData.radius);
				GL.Vertex(capsuleData.end - vector2 * capsuleData.radius);
				GL.Vertex(capsuleData.begin + vector3 * capsuleData.radius);
				GL.Vertex(capsuleData.end + vector3 * capsuleData.radius);
				GL.Vertex(capsuleData.begin - vector3 * capsuleData.radius);
				GL.Vertex(capsuleData.end - vector3 * capsuleData.radius);
				GL.End();
				if (this.renderTime >= capsuleData.expireAfter)
				{
					capsulesToRender.RemoveAtFast(i);
				}
			}
		}

		// Token: 0x060046C8 RID: 18120 RVA: 0x001A6860 File Offset: 0x001A4A60
		private void RenderCapsulesUsingLineRenderers(List<RuntimeGizmos.CapsuleData> capsulesToRender)
		{
			for (int i = capsulesToRender.Count - 1; i >= 0; i--)
			{
				RuntimeGizmos.CapsuleData capsuleData = capsulesToRender[i];
				Vector3 vector = capsuleData.end - capsuleData.begin;
				vector.Normalize();
				Vector3 vector2 = Vector3.Cross(vector, (Mathf.Abs(Vector3.Dot(vector, Vector3.up)) > 0.95f) ? Vector3.forward : Vector3.up);
				Vector3 vector3 = Vector3.Cross(vector, vector2);
				int num = Mathf.Clamp(Mathf.RoundToInt(8f * capsuleData.radius), 8, 64);
				this.DrawCircleUsingLineRenderer(capsuleData.begin, vector2, vector3, capsuleData.radius, num, capsuleData.color);
				this.DrawCircleUsingLineRenderer(capsuleData.end, vector2, vector3, capsuleData.radius, num, capsuleData.color);
				int resolution = num / 2;
				this.DrawSemicircleUsingLineRenderer(capsuleData.begin, vector2, -vector, capsuleData.radius, resolution, capsuleData.color);
				this.DrawSemicircleUsingLineRenderer(capsuleData.begin, vector3, -vector, capsuleData.radius, resolution, capsuleData.color);
				this.DrawSemicircleUsingLineRenderer(capsuleData.end, vector2, vector, capsuleData.radius, resolution, capsuleData.color);
				this.DrawSemicircleUsingLineRenderer(capsuleData.end, vector3, vector, capsuleData.radius, resolution, capsuleData.color);
				this.DrawLineUsingLineRenderer(capsuleData.begin + vector2 * capsuleData.radius, capsuleData.end + vector2 * capsuleData.radius, capsuleData.color);
				this.DrawLineUsingLineRenderer(capsuleData.begin - vector2 * capsuleData.radius, capsuleData.end - vector2 * capsuleData.radius, capsuleData.color);
				this.DrawLineUsingLineRenderer(capsuleData.begin + vector3 * capsuleData.radius, capsuleData.end + vector3 * capsuleData.radius, capsuleData.color);
				this.DrawLineUsingLineRenderer(capsuleData.begin - vector3 * capsuleData.radius, capsuleData.end - vector3 * capsuleData.radius, capsuleData.color);
				if (this.renderTime >= capsuleData.expireAfter)
				{
					capsulesToRender.RemoveAtFast(i);
				}
			}
		}

		// Token: 0x060046C9 RID: 18121 RVA: 0x001A6AB4 File Offset: 0x001A4CB4
		private void RenderSpheres(List<RuntimeGizmos.SphereData> spheresToRender)
		{
			for (int i = spheresToRender.Count - 1; i >= 0; i--)
			{
				RuntimeGizmos.SphereData sphereData = spheresToRender[i];
				Vector3 vector = sphereData.matrix.MultiplyPoint3x4(sphereData.localCenter);
				float sqrMagnitude = (vector - this.mainCameraPosition).sqrMagnitude;
				float num = sphereData.localRadius * sphereData.localRadius;
				if (sqrMagnitude - num < this.sqrCullDistance)
				{
					Vector3 axisU = sphereData.matrix.MultiplyVector(Vector3.up);
					Vector3 axisV = sphereData.matrix.MultiplyVector(Vector3.forward);
					Vector3 vector2 = sphereData.matrix.MultiplyVector(Vector3.right);
					this.RenderCircle(vector, axisU, vector2, sphereData.localRadius, sphereData.circleResolution, sphereData.color);
					this.RenderCircle(vector, axisU, axisV, sphereData.localRadius, sphereData.circleResolution, sphereData.color);
					this.RenderCircle(vector, vector2, axisV, sphereData.localRadius, sphereData.circleResolution, sphereData.color);
				}
				if (this.renderTime >= sphereData.expireAfter)
				{
					spheresToRender.RemoveAtFast(i);
				}
			}
		}

		// Token: 0x060046CA RID: 18122 RVA: 0x001A6BCC File Offset: 0x001A4DCC
		private void RenderSpheresUsingLineRenderers(List<RuntimeGizmos.SphereData> spheresToRender)
		{
			for (int i = spheresToRender.Count - 1; i >= 0; i--)
			{
				RuntimeGizmos.SphereData sphereData = spheresToRender[i];
				Vector3 vector = sphereData.matrix.MultiplyPoint3x4(sphereData.localCenter);
				float sqrMagnitude = (vector - this.mainCameraPosition).sqrMagnitude;
				float num = sphereData.localRadius * sphereData.localRadius;
				if (sqrMagnitude - num < this.sqrCullDistance)
				{
					Vector3 axisU = sphereData.matrix.MultiplyVector(Vector3.up);
					Vector3 axisV = sphereData.matrix.MultiplyVector(Vector3.forward);
					Vector3 vector2 = sphereData.matrix.MultiplyVector(Vector3.right);
					this.DrawCircleUsingLineRenderer(vector, axisU, vector2, sphereData.localRadius, sphereData.circleResolution, sphereData.color);
					this.DrawCircleUsingLineRenderer(vector, axisU, axisV, sphereData.localRadius, sphereData.circleResolution, sphereData.color);
					this.DrawCircleUsingLineRenderer(vector, vector2, axisV, sphereData.localRadius, sphereData.circleResolution, sphereData.color);
				}
				if (this.renderTime >= sphereData.expireAfter)
				{
					spheresToRender.RemoveAtFast(i);
				}
			}
		}

		// Token: 0x060046CB RID: 18123 RVA: 0x001A6CE4 File Offset: 0x001A4EE4
		private void RenderCircles(List<RuntimeGizmos.CircleData> circlesToRender)
		{
			for (int i = circlesToRender.Count - 1; i >= 0; i--)
			{
				RuntimeGizmos.CircleData circleData = circlesToRender[i];
				float sqrMagnitude = (circleData.center - this.mainCameraPosition).sqrMagnitude;
				float num = circleData.radius * circleData.radius;
				if (sqrMagnitude - num < this.sqrCullDistance)
				{
					this.RenderCircle(circleData.center, circleData.axisU, circleData.axisV, circleData.radius, circleData.resolution, circleData.color);
				}
				if (this.renderTime >= circleData.expireAfter)
				{
					circlesToRender.RemoveAtFast(i);
				}
			}
		}

		// Token: 0x060046CC RID: 18124 RVA: 0x001A6D7C File Offset: 0x001A4F7C
		private void RenderCirclesUsingLineRenderers(List<RuntimeGizmos.CircleData> circlesToRender)
		{
			for (int i = circlesToRender.Count - 1; i >= 0; i--)
			{
				RuntimeGizmos.CircleData circleData = circlesToRender[i];
				float sqrMagnitude = (circleData.center - this.mainCameraPosition).sqrMagnitude;
				float num = circleData.radius * circleData.radius;
				if (sqrMagnitude - num < this.sqrCullDistance)
				{
					this.DrawCircleUsingLineRenderer(circleData.center, circleData.axisU, circleData.axisV, circleData.radius, circleData.resolution, circleData.color);
				}
				if (this.renderTime >= circleData.expireAfter)
				{
					circlesToRender.RemoveAtFast(i);
				}
			}
		}

		// Token: 0x060046CD RID: 18125 RVA: 0x001A6E14 File Offset: 0x001A5014
		private void RenderCircle(Vector3 center, Vector3 axisU, Vector3 axisV, float radius, int resolution, Color color)
		{
			float num = 6.2831855f / (float)resolution;
			Vector3 v = center + axisU * radius;
			GL.Begin(2);
			GL.Color(color);
			GL.Vertex(v);
			for (int i = 1; i < resolution; i++)
			{
				float f = (float)i * num;
				float d = Mathf.Cos(f) * radius;
				float d2 = Mathf.Sin(f) * radius;
				GL.Vertex(center + axisU * d + axisV * d2);
			}
			GL.Vertex(v);
			GL.End();
		}

		// Token: 0x060046CE RID: 18126 RVA: 0x001A6E9C File Offset: 0x001A509C
		private void RenderSemicircle(Vector3 center, Vector3 axisU, Vector3 axisV, float radius, int resolution, Color color)
		{
			float num = 3.1415927f / (float)resolution;
			GL.Begin(2);
			GL.Color(color);
			GL.Vertex(center + axisU * radius);
			for (int i = 1; i < resolution; i++)
			{
				float f = (float)i * num;
				float d = Mathf.Cos(f) * radius;
				float d2 = Mathf.Sin(f) * radius;
				GL.Vertex(center + axisU * d + axisV * d2);
			}
			GL.Vertex(center - axisU * radius);
			GL.End();
		}

		// Token: 0x060046CF RID: 18127 RVA: 0x001A6F30 File Offset: 0x001A5130
		private void DrawSemicircleUsingLineRenderer(Vector3 center, Vector3 axisU, Vector3 axisV, float radius, int resolution, Color color)
		{
			float num = 3.1415927f / (float)resolution;
			LineRenderer lineRenderer = this.ClaimLineRenderer();
			lineRenderer.positionCount = resolution + 1;
			lineRenderer.loop = false;
			lineRenderer.startColor = color;
			lineRenderer.endColor = color;
			Vector3 linePosition = center + axisU * radius;
			AnimationCurve animationCurve = this.ClaimCurveWithKeyCount(resolution + 1);
			animationCurve.MoveKey(0, new Keyframe(0f, this.CalculateLineRendererWidth(linePosition)));
			lineRenderer.SetPosition(0, center + axisU * radius);
			for (int i = 1; i < resolution; i++)
			{
				float f = (float)i * num;
				float d = Mathf.Cos(f) * radius;
				float d2 = Mathf.Sin(f) * radius;
				Vector3 vector = center + axisU * d + axisV * d2;
				animationCurve.MoveKey(i, new Keyframe((float)i / (float)resolution, this.CalculateLineRendererWidth(vector)));
				lineRenderer.SetPosition(i, vector);
			}
			Vector3 vector2 = center - axisU * radius;
			animationCurve.MoveKey(resolution, new Keyframe(1f, this.CalculateLineRendererWidth(vector2)));
			lineRenderer.SetPosition(resolution, vector2);
			lineRenderer.widthCurve = animationCurve;
		}

		// Token: 0x060046D0 RID: 18128 RVA: 0x001A7064 File Offset: 0x001A5264
		private LineRenderer ClaimLineRenderer()
		{
			LineRenderer lineRenderer = null;
			while (this.lineRendererPool.Count > 0 && lineRenderer == null)
			{
				lineRenderer = this.lineRendererPool.GetAndRemoveTail<LineRenderer>();
			}
			if (lineRenderer != null)
			{
				lineRenderer.enabled = true;
			}
			else
			{
				lineRenderer = new GameObject("Runtime Gizmo LineRenderer").AddComponent<LineRenderer>();
				lineRenderer.sharedMaterial = this.lineRendererSharedMaterial;
				lineRenderer.shadowCastingMode = ShadowCastingMode.Off;
				lineRenderer.numCapVertices = 1;
			}
			lineRenderer.gameObject.layer = this.lineRendererLayer;
			this.activeLineRenderers.Add(lineRenderer);
			return lineRenderer;
		}

		// Token: 0x060046D1 RID: 18129 RVA: 0x001A70F4 File Offset: 0x001A52F4
		private AnimationCurve ClaimCurveWithKeyCount(int count)
		{
			List<AnimationCurve> list;
			if (!this.animationCurvePool.TryGetValue(count, ref list))
			{
				list = new List<AnimationCurve>();
				this.animationCurvePool.Add(count, list);
			}
			AnimationCurve result;
			if (list.Count > 0)
			{
				result = list.GetAndRemoveTail<AnimationCurve>();
			}
			else
			{
				Keyframe[] array = new Keyframe[count];
				for (int i = 0; i < count; i++)
				{
					array[i] = new Keyframe((float)i / (float)(count - 1), 10f);
				}
				result = new AnimationCurve(array);
			}
			return result;
		}

		// Token: 0x060046D2 RID: 18130 RVA: 0x001A716C File Offset: 0x001A536C
		private float CalculateLineRendererWidth(Vector3 linePosition)
		{
			return (linePosition - this.mainCameraPosition).magnitude * 0.005f;
		}

		// Token: 0x060046D3 RID: 18131 RVA: 0x001A7194 File Offset: 0x001A5394
		private void DrawLineUsingLineRenderer(Vector3 begin, Vector3 end, Color color)
		{
			LineRenderer lineRenderer = this.ClaimLineRenderer();
			lineRenderer.positionCount = 2;
			lineRenderer.loop = false;
			lineRenderer.SetPosition(0, begin);
			lineRenderer.SetPosition(1, end);
			lineRenderer.startColor = color;
			lineRenderer.endColor = color;
			AnimationCurve animationCurve = this.ClaimCurveWithKeyCount(2);
			animationCurve.MoveKey(0, new Keyframe(0f, this.CalculateLineRendererWidth(begin)));
			animationCurve.MoveKey(1, new Keyframe(1f, this.CalculateLineRendererWidth(end)));
			lineRenderer.widthCurve = animationCurve;
		}

		// Token: 0x060046D4 RID: 18132 RVA: 0x001A7214 File Offset: 0x001A5414
		private void DrawCircleUsingLineRenderer(Vector3 center, Vector3 axisU, Vector3 axisV, float radius, int resolution, Color color)
		{
			float num = 6.2831855f / (float)resolution;
			Vector3 normalized = Vector3.Cross(axisU, axisV).normalized;
			LineRenderer lineRenderer = this.ClaimLineRenderer();
			lineRenderer.positionCount = resolution;
			lineRenderer.loop = true;
			lineRenderer.startColor = color;
			lineRenderer.endColor = color;
			Vector3 vector = center + axisU * radius;
			AnimationCurve animationCurve = this.ClaimCurveWithKeyCount(resolution);
			animationCurve.MoveKey(0, new Keyframe(0f, this.CalculateLineRendererWidth(vector)));
			lineRenderer.SetPosition(0, vector);
			for (int i = 1; i < resolution; i++)
			{
				float f = (float)i * num;
				float d = Mathf.Cos(f) * radius;
				float d2 = Mathf.Sin(f) * radius;
				Vector3 vector2 = center + axisU * d + axisV * d2;
				lineRenderer.SetPosition(i, vector2);
				animationCurve.MoveKey(i, new Keyframe((float)i / (float)(resolution - 1), this.CalculateLineRendererWidth(vector2)));
			}
			lineRenderer.widthCurve = animationCurve;
		}

		// Token: 0x060046D5 RID: 18133 RVA: 0x001A7314 File Offset: 0x001A5514
		private void OnGUI()
		{
			if (Event.current.type != 7)
			{
				return;
			}
			Camera camera = MainCamera.instance;
			if (camera == null)
			{
				return;
			}
			Color color = GUI.color;
			float num = (float)camera.pixelWidth;
			float num2 = (float)camera.pixelHeight;
			float time = Time.time;
			for (int i = this.labelsToRender.Count - 1; i >= 0; i--)
			{
				RuntimeGizmos.LabelData labelData = this.labelsToRender[i];
				Vector3 vector = camera.WorldToViewportPoint(labelData.position);
				if (vector.z > 0f)
				{
					Vector2 vector2 = new Vector2(vector.x * num, (1f - vector.y) * num2);
					Rect rect = new Rect(vector2.x - 100f, vector2.y - 100f, 200f, 200f);
					GUI.skin.label.alignment = 4;
					GUI.color = Color.black;
					GUI.Label(rect, labelData.content);
					rect.position -= Vector2.one;
					GUI.color = labelData.color;
					GUI.Label(rect, labelData.content);
				}
				if (time >= labelData.expireAfter)
				{
					this.labelsToRender.RemoveAtFast(i);
				}
			}
			GUI.color = color;
		}

		// Token: 0x060046D6 RID: 18134 RVA: 0x001A746E File Offset: 0x001A566E
		private void OnEnable()
		{
			base.useGUILayout = false;
			this.lineRendererSharedMaterial = new Material(Shader.Find("Sprites/Default"));
			CommandLogMemoryUsage.OnExecuted = (Action<List<string>>)Delegate.Combine(CommandLogMemoryUsage.OnExecuted, new Action<List<string>>(this.OnLogMemoryUsage));
		}

		// Token: 0x060046D7 RID: 18135 RVA: 0x001A74AC File Offset: 0x001A56AC
		private void OnDisable()
		{
			CommandLogMemoryUsage.OnExecuted = (Action<List<string>>)Delegate.Remove(CommandLogMemoryUsage.OnExecuted, new Action<List<string>>(this.OnLogMemoryUsage));
		}

		/// <summary>
		/// LateUpdate so that the most up-to-date gizmos and main camera position are used.
		/// </summary>
		// Token: 0x060046D8 RID: 18136 RVA: 0x001A74D0 File Offset: 0x001A56D0
		private void LateUpdate()
		{
			if (!RuntimeGizmos.clUseLineRenderers.value)
			{
				return;
			}
			this.renderTime = Time.time;
			Camera camera = MainCamera.instance;
			if (camera != null)
			{
				this.mainCameraPosition = camera.transform.position;
				this.cullDistance = camera.farClipPlane;
				this.sqrCullDistance = this.cullDistance * this.cullDistance;
				if (this.lineRendererForegroundCamera == null)
				{
					GameObject gameObject = new GameObject("Runtime Gizmo Camera");
					this.lineRendererForegroundCamera = gameObject.AddComponent<Camera>();
					this.lineRendererForegroundCamera.clearFlags = CameraClearFlags.Depth;
					this.lineRendererForegroundCamera.cullingMask = 4;
					this.lineRendererForegroundCamera.depth = 10f;
					this.lineRendererForegroundCamera.nearClipPlane = camera.nearClipPlane;
					this.lineRendererForegroundCamera.farClipPlane = camera.farClipPlane;
				}
				this.lineRendererForegroundCamera.transform.SetPositionAndRotation(this.mainCameraPosition, camera.transform.rotation);
				this.lineRendererForegroundCamera.projectionMatrix = camera.projectionMatrix;
			}
			else
			{
				this.mainCameraPosition = Vector3.zero;
				this.cullDistance = 0f;
				this.sqrCullDistance = 0f;
			}
			foreach (LineRenderer lineRenderer in this.activeLineRenderers)
			{
				if (!(lineRenderer == null))
				{
					lineRenderer.enabled = false;
					this.lineRendererPool.Add(lineRenderer);
					AnimationCurve widthCurve = lineRenderer.widthCurve;
					List<AnimationCurve> list;
					if (this.animationCurvePool.TryGetValue(widthCurve.length, ref list))
					{
						list.Add(widthCurve);
					}
				}
			}
			this.activeLineRenderers.Clear();
			for (int i = 0; i < 2; i++)
			{
				this.lineRendererLayer = this.lineRendererLayers[i];
				this.RenderBoxesUsingLineRenderers(this.boxLayers[i]);
				this.RenderLinesUsingLineRenderer(this.lineLayers[i]);
				this.RenderCapsulesUsingLineRenderers(this.capsuleLayers[i]);
				this.RenderSpheresUsingLineRenderers(this.sphereLayers[i]);
				this.RenderCirclesUsingLineRenderers(this.circleLayers[i]);
			}
		}

		// Token: 0x060046D9 RID: 18137 RVA: 0x001A76F4 File Offset: 0x001A58F4
		private RuntimeGizmos()
		{
			this.boxLayers = new List<RuntimeGizmos.BoxData>[2];
			this.lineLayers = new List<RuntimeGizmos.LineData>[2];
			this.capsuleLayers = new List<RuntimeGizmos.CapsuleData>[2];
			this.sphereLayers = new List<RuntimeGizmos.SphereData>[2];
			this.circleLayers = new List<RuntimeGizmos.CircleData>[2];
			for (int i = 0; i < 2; i++)
			{
				this.boxLayers[i] = new List<RuntimeGizmos.BoxData>();
				this.lineLayers[i] = new List<RuntimeGizmos.LineData>();
				this.capsuleLayers[i] = new List<RuntimeGizmos.CapsuleData>();
				this.sphereLayers[i] = new List<RuntimeGizmos.SphereData>();
				this.circleLayers[i] = new List<RuntimeGizmos.CircleData>();
			}
			this.lineRendererLayers = new int[2];
			this.lineRendererLayers[0] = 18;
			this.lineRendererLayers[1] = 2;
		}

		// Token: 0x060046DA RID: 18138 RVA: 0x001A77DC File Offset: 0x001A59DC
		private void OnLogMemoryUsage(List<string> results)
		{
			results.Add(string.Format("Runtime gizmos line renderer pool size: {0}", this.lineRendererPool.Count));
			results.Add(string.Format("Runtime gizmos animation curve pool size: {0}", this.animationCurvePool.Count));
			results.Add(string.Format("Runtime gizmos active line renderers: {0}", this.activeLineRenderers.Count));
			results.Add(string.Format("Runtime gizmos pending labels: {0}", this.labelsToRender.Count));
			int num = 0;
			foreach (List<RuntimeGizmos.LineData> list in this.lineLayers)
			{
				num += list.Count;
			}
			results.Add(string.Format("Runtime gizmos pending lines: {0}", num));
			int num2 = 0;
			foreach (List<RuntimeGizmos.SphereData> list2 in this.sphereLayers)
			{
				num2 += list2.Count;
			}
			results.Add(string.Format("Runtime gizmos pending spheres: {0}", num2));
			int num3 = 0;
			foreach (List<RuntimeGizmos.CircleData> list3 in this.circleLayers)
			{
				num3 += list3.Count;
			}
			results.Add(string.Format("Runtime gizmos pending circles: {0}", num3));
			int num4 = 0;
			foreach (List<RuntimeGizmos.CapsuleData> list4 in this.capsuleLayers)
			{
				num4 += list4.Count;
			}
			results.Add(string.Format("Runtime gizmos pending capsules: {0}", num4));
			int num5 = 0;
			foreach (List<RuntimeGizmos.BoxData> list5 in this.boxLayers)
			{
				num5 += list5.Count;
			}
			results.Add(string.Format("Runtime gizmos pending boxes: {0}", num5));
		}

		// Token: 0x0400302B RID: 12331
		private List<RuntimeGizmos.BoxData>[] boxLayers;

		// Token: 0x0400302C RID: 12332
		private List<RuntimeGizmos.LineData>[] lineLayers;

		// Token: 0x0400302D RID: 12333
		private List<RuntimeGizmos.CapsuleData>[] capsuleLayers;

		// Token: 0x0400302E RID: 12334
		private List<RuntimeGizmos.SphereData>[] sphereLayers;

		// Token: 0x0400302F RID: 12335
		private List<RuntimeGizmos.CircleData>[] circleLayers;

		// Token: 0x04003030 RID: 12336
		private List<RuntimeGizmos.LabelData> labelsToRender = new List<RuntimeGizmos.LabelData>();

		// Token: 0x04003031 RID: 12337
		private float renderTime;

		// Token: 0x04003032 RID: 12338
		private float cullDistance;

		// Token: 0x04003033 RID: 12339
		private float sqrCullDistance;

		// Token: 0x04003034 RID: 12340
		private Material[] materialLayers;

		// Token: 0x04003035 RID: 12341
		private int[] lineRendererLayers;

		// Token: 0x04003036 RID: 12342
		private int lineRendererLayer;

		// Token: 0x04003037 RID: 12343
		private List<LineRenderer> lineRendererPool = new List<LineRenderer>();

		// Token: 0x04003038 RID: 12344
		private Dictionary<int, List<AnimationCurve>> animationCurvePool = new Dictionary<int, List<AnimationCurve>>();

		// Token: 0x04003039 RID: 12345
		private List<LineRenderer> activeLineRenderers = new List<LineRenderer>();

		// Token: 0x0400303A RID: 12346
		private Material lineRendererSharedMaterial;

		// Token: 0x0400303B RID: 12347
		private Vector3 mainCameraPosition;

		// Token: 0x0400303C RID: 12348
		private Camera lineRendererForegroundCamera;

		// Token: 0x0400303D RID: 12349
		private static RuntimeGizmos instance;

		// Token: 0x0400303E RID: 12350
		private const int LAYER_COUNT = 2;

		// Token: 0x0400303F RID: 12351
		private const float CIRCLE_RESOLUTION_MULTIPLIER = 8f;

		// Token: 0x04003040 RID: 12352
		private const int MIN_CIRCLE_RESOLUTION = 8;

		// Token: 0x04003041 RID: 12353
		private const int MAX_CIRCLE_RESOLUTION = 64;

		// Token: 0x04003042 RID: 12354
		private static CommandLineFlag clUseLineRenderers = new CommandLineFlag(false, "-FallbackGizmos");

		// Token: 0x02000A23 RID: 2595
		private struct BoxData
		{
			// Token: 0x06004D90 RID: 19856 RVA: 0x001B986B File Offset: 0x001B7A6B
			public BoxData(Matrix4x4 matrix, Vector3 localCenter, Vector3 size, Color color, float lifespan)
			{
				this.matrix = matrix;
				this.localCenter = localCenter;
				this.size = size;
				this.extents = size * 0.5f;
				this.color = color;
				this.expireAfter = Time.time + lifespan;
			}

			// Token: 0x0400352A RID: 13610
			public Matrix4x4 matrix;

			/// <summary>
			/// Center relative to matrix.
			/// </summary>
			// Token: 0x0400352B RID: 13611
			public Vector3 localCenter;

			// Token: 0x0400352C RID: 13612
			public Vector3 size;

			// Token: 0x0400352D RID: 13613
			public Vector3 extents;

			// Token: 0x0400352E RID: 13614
			public Color color;

			// Token: 0x0400352F RID: 13615
			public float expireAfter;
		}

		// Token: 0x02000A24 RID: 2596
		private struct LineData
		{
			// Token: 0x06004D91 RID: 19857 RVA: 0x001B98A9 File Offset: 0x001B7AA9
			public LineData(Vector3 begin, Vector3 end, Color color, float lifespan)
			{
				this.begin = begin;
				this.end = end;
				this.color = color;
				this.expireAfter = Time.time + lifespan;
			}

			// Token: 0x04003530 RID: 13616
			public Vector3 begin;

			// Token: 0x04003531 RID: 13617
			public Vector3 end;

			// Token: 0x04003532 RID: 13618
			public Color color;

			// Token: 0x04003533 RID: 13619
			public float expireAfter;
		}

		// Token: 0x02000A25 RID: 2597
		private struct CapsuleData
		{
			// Token: 0x06004D92 RID: 19858 RVA: 0x001B98CE File Offset: 0x001B7ACE
			public CapsuleData(Vector3 begin, Vector3 end, Color color, float lifespan, float radius)
			{
				this.begin = begin;
				this.end = end;
				this.color = color;
				this.expireAfter = Time.time + lifespan;
				this.radius = radius;
			}

			// Token: 0x04003534 RID: 13620
			public Vector3 begin;

			// Token: 0x04003535 RID: 13621
			public Vector3 end;

			// Token: 0x04003536 RID: 13622
			public Color color;

			// Token: 0x04003537 RID: 13623
			public float expireAfter;

			// Token: 0x04003538 RID: 13624
			public float radius;
		}

		// Token: 0x02000A26 RID: 2598
		private struct SphereData
		{
			// Token: 0x06004D93 RID: 19859 RVA: 0x001B98FC File Offset: 0x001B7AFC
			public SphereData(Matrix4x4 matrix, Vector3 localCenter, Color color, float lifespan, float localRadius)
			{
				this.matrix = matrix;
				this.localCenter = localCenter;
				this.color = color;
				this.expireAfter = Time.time + lifespan;
				this.localRadius = localRadius;
				float max = matrix.lossyScale.GetAbs().GetMax();
				this.circleResolution = Mathf.Clamp(Mathf.RoundToInt(8f * localRadius * max), 8, 64);
			}

			// Token: 0x04003539 RID: 13625
			public Matrix4x4 matrix;

			/// <summary>
			/// Center relative to matrix.
			/// </summary>
			// Token: 0x0400353A RID: 13626
			public Vector3 localCenter;

			// Token: 0x0400353B RID: 13627
			public Color color;

			// Token: 0x0400353C RID: 13628
			public float expireAfter;

			// Token: 0x0400353D RID: 13629
			public float localRadius;

			// Token: 0x0400353E RID: 13630
			public int circleResolution;
		}

		// Token: 0x02000A27 RID: 2599
		private struct CircleData
		{
			// Token: 0x06004D94 RID: 19860 RVA: 0x001B9964 File Offset: 0x001B7B64
			public CircleData(Vector3 center, Vector3 axisU, Vector3 axisV, Color color, float lifespan, float radius, int resolution)
			{
				this.center = center;
				this.axisU = axisU;
				this.axisV = axisV;
				this.color = color;
				this.expireAfter = Time.time + lifespan;
				this.radius = radius;
				this.resolution = ((resolution > 0) ? resolution : Mathf.Clamp(Mathf.RoundToInt(8f * radius), 8, 64));
			}

			// Token: 0x0400353F RID: 13631
			public Vector3 center;

			// Token: 0x04003540 RID: 13632
			public Vector3 axisU;

			// Token: 0x04003541 RID: 13633
			public Vector3 axisV;

			// Token: 0x04003542 RID: 13634
			public Color color;

			// Token: 0x04003543 RID: 13635
			public float expireAfter;

			// Token: 0x04003544 RID: 13636
			public float radius;

			// Token: 0x04003545 RID: 13637
			public int resolution;
		}

		// Token: 0x02000A28 RID: 2600
		private struct LabelData
		{
			// Token: 0x06004D95 RID: 19861 RVA: 0x001B99C8 File Offset: 0x001B7BC8
			public LabelData(Vector3 position, string content, Color color, float lifespan)
			{
				this.position = position;
				this.content = content;
				this.color = color;
				this.expireAfter = Time.time + lifespan;
			}

			// Token: 0x04003546 RID: 13638
			public Vector3 position;

			// Token: 0x04003547 RID: 13639
			public string content;

			// Token: 0x04003548 RID: 13640
			public Color color;

			// Token: 0x04003549 RID: 13641
			public float expireAfter;
		}
	}
}
