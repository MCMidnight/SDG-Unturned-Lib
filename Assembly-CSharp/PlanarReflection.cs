using System;
using SDG.Unturned;
using UnityEngine;

// Token: 0x02000009 RID: 9
public class PlanarReflection : MonoBehaviour
{
	// Token: 0x14000001 RID: 1
	// (add) Token: 0x06000014 RID: 20 RVA: 0x00002118 File Offset: 0x00000318
	// (remove) Token: 0x06000015 RID: 21 RVA: 0x0000214C File Offset: 0x0000034C
	public static event PlanarReflectionPreRenderHandler preRender;

	// Token: 0x14000002 RID: 2
	// (add) Token: 0x06000016 RID: 22 RVA: 0x00002180 File Offset: 0x00000380
	// (remove) Token: 0x06000017 RID: 23 RVA: 0x000021B4 File Offset: 0x000003B4
	public static event PlanarReflectionPostRenderHandler postRender;

	// Token: 0x06000018 RID: 24 RVA: 0x000021E7 File Offset: 0x000003E7
	public void Start()
	{
		if (this.sharedMaterial == null)
		{
			this.sharedMaterial = base.transform.GetChild(0).GetChild(0).GetComponent<Renderer>().material;
		}
	}

	// Token: 0x06000019 RID: 25 RVA: 0x0000221C File Offset: 0x0000041C
	private Camera CreateReflectionCameraFor(Camera cam)
	{
		Camera camera = new GameObject(base.gameObject.name + "Reflection" + cam.name).AddComponent<Camera>();
		camera.nearClipPlane = cam.nearClipPlane;
		camera.farClipPlane = cam.farClipPlane;
		camera.backgroundColor = this.clearColor;
		camera.clearFlags = (this.reflectSkybox ? CameraClearFlags.Skybox : CameraClearFlags.Color);
		camera.backgroundColor = Color.black;
		camera.enabled = false;
		if (!camera.targetTexture)
		{
			camera.targetTexture = this.CreateTextureFor(cam);
		}
		return camera;
	}

	// Token: 0x0600001A RID: 26 RVA: 0x000022B2 File Offset: 0x000004B2
	private RenderTexture CreateTextureFor(Camera cam)
	{
		return new RenderTexture(Mathf.RoundToInt((float)cam.pixelWidth * 0.5f), Mathf.RoundToInt((float)cam.pixelHeight * 0.5f), 16)
		{
			name = "PlanarReflection_RT"
		};
	}

	// Token: 0x0600001B RID: 27 RVA: 0x000022EA File Offset: 0x000004EA
	public void RenderHelpCameras(Camera currentCam)
	{
		if (!this.reflectionCamera)
		{
			this.reflectionCamera = this.CreateReflectionCameraFor(currentCam);
		}
		this.RenderReflectionFor(currentCam, this.reflectionCamera);
	}

	// Token: 0x0600001C RID: 28 RVA: 0x00002313 File Offset: 0x00000513
	public void LateUpdate()
	{
		this.helped = false;
	}

	// Token: 0x0600001D RID: 29 RVA: 0x0000231C File Offset: 0x0000051C
	public void WaterTileBeingRendered(Transform tr, Camera currentCam)
	{
		if (base.enabled && currentCam.CompareTag("MainCamera"))
		{
			if (this.helped)
			{
				return;
			}
			this.helped = true;
			this.RenderHelpCameras(currentCam);
			if (this.reflectionCamera != null && this.sharedMaterial != null)
			{
				this.sharedMaterial.EnableKeyword("WATER_REFLECTIVE");
				this.sharedMaterial.DisableKeyword("WATER_SIMPLE");
				this.sharedMaterial.SetTexture(this.reflectionSampler, this.reflectionCamera.targetTexture);
				return;
			}
		}
		else if (this.reflectionCamera != null && this.sharedMaterial != null)
		{
			this.sharedMaterial.DisableKeyword("WATER_REFLECTIVE");
			this.sharedMaterial.EnableKeyword("WATER_SIMPLE");
			this.sharedMaterial.SetTexture(this.reflectionSampler, null);
		}
	}

	// Token: 0x0600001E RID: 30 RVA: 0x00002408 File Offset: 0x00000608
	private void RenderReflectionFor(Camera cam, Camera reflectCamera)
	{
		if (!reflectCamera)
		{
			return;
		}
		if (this.sharedMaterial && !this.sharedMaterial.HasProperty(this.reflectionSampler))
		{
			return;
		}
		if (this.settingsUpdateIndex < GraphicsSettings.planarReflectionUpdateIndex)
		{
			this.settingsUpdateIndex = GraphicsSettings.planarReflectionUpdateIndex;
			switch (GraphicsSettings.planarReflectionQuality)
			{
			case EGraphicQuality.LOW:
				reflectCamera.cullingMask = PlanarReflection.CULLING_MASK_LOW;
				break;
			case EGraphicQuality.MEDIUM:
				reflectCamera.cullingMask = PlanarReflection.CULLING_MASK_MEDIUM;
				break;
			case EGraphicQuality.HIGH:
				reflectCamera.cullingMask = PlanarReflection.CULLING_MASK_HIGH;
				break;
			case EGraphicQuality.ULTRA:
				reflectCamera.cullingMask = PlanarReflection.CULLING_MASK_ULTRA;
				break;
			}
			reflectCamera.layerCullDistances = cam.layerCullDistances;
			reflectCamera.layerCullSpherical = cam.layerCullSpherical;
		}
		reflectCamera.fieldOfView = cam.fieldOfView;
		this.SaneCameraSettings(reflectCamera);
		GL.invertCulling = true;
		Transform transform = base.transform;
		Vector3 eulerAngles = cam.transform.eulerAngles;
		reflectCamera.transform.eulerAngles = new Vector3(-eulerAngles.x, eulerAngles.y, eulerAngles.z);
		reflectCamera.transform.position = cam.transform.position;
		Vector3 position = transform.transform.position;
		position.y = transform.position.y;
		Vector3 up = transform.transform.up;
		float w = -Vector3.Dot(up, position) - this.clipPlaneOffset;
		Vector4 plane = new Vector4(up.x, up.y, up.z, w);
		Matrix4x4 matrix4x = Matrix4x4.zero;
		matrix4x = PlanarReflection.CalculateReflectionMatrix(matrix4x, plane);
		this.oldpos = cam.transform.position;
		Vector3 position2 = matrix4x.MultiplyPoint(this.oldpos);
		reflectCamera.worldToCameraMatrix = cam.worldToCameraMatrix * matrix4x;
		Vector4 clipPlane = this.CameraSpacePlane(reflectCamera, position, up, 1f);
		reflectCamera.projectionMatrix = cam.CalculateObliqueMatrix(clipPlane);
		reflectCamera.transform.position = position2;
		Vector3 eulerAngles2 = cam.transform.eulerAngles;
		reflectCamera.transform.eulerAngles = new Vector3(-eulerAngles2.x, eulerAngles2.y, eulerAngles2.z);
		float lodBias = QualitySettings.lodBias;
		QualitySettings.lodBias = 1f;
		PlanarReflectionPreRenderHandler planarReflectionPreRenderHandler = PlanarReflection.preRender;
		if (planarReflectionPreRenderHandler != null)
		{
			planarReflectionPreRenderHandler();
		}
		reflectCamera.Render();
		PlanarReflectionPostRenderHandler planarReflectionPostRenderHandler = PlanarReflection.postRender;
		if (planarReflectionPostRenderHandler != null)
		{
			planarReflectionPostRenderHandler();
		}
		QualitySettings.lodBias = lodBias;
		GL.invertCulling = false;
	}

	// Token: 0x0600001F RID: 31 RVA: 0x00002661 File Offset: 0x00000861
	private void SaneCameraSettings(Camera helperCam)
	{
		helperCam.renderingPath = RenderingPath.Forward;
		helperCam.allowHDR = true;
	}

	// Token: 0x06000020 RID: 32 RVA: 0x00002674 File Offset: 0x00000874
	private static Matrix4x4 CalculateReflectionMatrix(Matrix4x4 reflectionMat, Vector4 plane)
	{
		reflectionMat.m00 = 1f - 2f * plane[0] * plane[0];
		reflectionMat.m01 = -2f * plane[0] * plane[1];
		reflectionMat.m02 = -2f * plane[0] * plane[2];
		reflectionMat.m03 = -2f * plane[3] * plane[0];
		reflectionMat.m10 = -2f * plane[1] * plane[0];
		reflectionMat.m11 = 1f - 2f * plane[1] * plane[1];
		reflectionMat.m12 = -2f * plane[1] * plane[2];
		reflectionMat.m13 = -2f * plane[3] * plane[1];
		reflectionMat.m20 = -2f * plane[2] * plane[0];
		reflectionMat.m21 = -2f * plane[2] * plane[1];
		reflectionMat.m22 = 1f - 2f * plane[2] * plane[2];
		reflectionMat.m23 = -2f * plane[3] * plane[2];
		reflectionMat.m30 = 0f;
		reflectionMat.m31 = 0f;
		reflectionMat.m32 = 0f;
		reflectionMat.m33 = 1f;
		return reflectionMat;
	}

	// Token: 0x06000021 RID: 33 RVA: 0x0000282C File Offset: 0x00000A2C
	private static float sgn(float a)
	{
		if (a > 0f)
		{
			return 1f;
		}
		if (a < 0f)
		{
			return -1f;
		}
		return 0f;
	}

	// Token: 0x06000022 RID: 34 RVA: 0x00002850 File Offset: 0x00000A50
	private Vector4 CameraSpacePlane(Camera cam, Vector3 pos, Vector3 normal, float sideSign)
	{
		Vector3 point = pos + normal * this.clipPlaneOffset;
		Matrix4x4 worldToCameraMatrix = cam.worldToCameraMatrix;
		Vector3 lhs = worldToCameraMatrix.MultiplyPoint(point);
		Vector3 vector = worldToCameraMatrix.MultiplyVector(normal).normalized * sideSign;
		return new Vector4(vector.x, vector.y, vector.z, -Vector3.Dot(lhs, vector));
	}

	// Token: 0x04000005 RID: 5
	private static readonly int CULLING_MASK_LOW = -2146435072;

	// Token: 0x04000006 RID: 6
	private static readonly int CULLING_MASK_MEDIUM = PlanarReflection.CULLING_MASK_LOW | 262144 | 32768 | 16384 | 268435456;

	// Token: 0x04000007 RID: 7
	private static readonly int CULLING_MASK_HIGH = PlanarReflection.CULLING_MASK_MEDIUM | 65536 | 134217728 | 67108864;

	// Token: 0x04000008 RID: 8
	private static readonly int CULLING_MASK_ULTRA = PlanarReflection.CULLING_MASK_HIGH | 1024 | 4096 | 8388608 | 16777216;

	// Token: 0x0400000B RID: 11
	public LayerMask reflectionMask;

	// Token: 0x0400000C RID: 12
	public bool reflectSkybox;

	// Token: 0x0400000D RID: 13
	public Color clearColor = Color.grey;

	// Token: 0x0400000E RID: 14
	public string reflectionSampler = "_ReflectionTex";

	// Token: 0x0400000F RID: 15
	public float clipPlaneOffset = 0.07f;

	// Token: 0x04000010 RID: 16
	private Vector3 oldpos = Vector3.zero;

	// Token: 0x04000011 RID: 17
	private Camera reflectionCamera;

	// Token: 0x04000012 RID: 18
	private bool helped;

	// Token: 0x04000013 RID: 19
	public Material sharedMaterial;

	// Token: 0x04000014 RID: 20
	private int settingsUpdateIndex = -1;
}
