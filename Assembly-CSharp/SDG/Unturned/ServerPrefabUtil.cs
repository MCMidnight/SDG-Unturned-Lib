using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Unturned.SystemEx;

namespace SDG.Unturned
{
	/// <summary>
	/// Helpers on the dedicated server to optimize client prefabs for server usage.
	/// </summary>
	// Token: 0x02000364 RID: 868
	internal static class ServerPrefabUtil
	{
		/// <summary>
		/// Optimize client prefab for server usage.
		/// </summary>
		// Token: 0x06001A3C RID: 6716 RVA: 0x0005E4B4 File Offset: 0x0005C6B4
		public static void RemoveClientComponents(GameObject gameObject)
		{
			gameObject.GetComponentsInChildren<Component>(true, ServerPrefabUtil.workingComponents);
			ListEx.RemoveSwap<Component>(ServerPrefabUtil.workingComponents, delegate(Component component)
			{
				if (component == null)
				{
					return true;
				}
				if (ServerPrefabUtil.typesToRemove.Contains(component.GetType()))
				{
					return false;
				}
				Animation animation = component as Animation;
				if (animation != null)
				{
					animation.cullingType = 0;
				}
				return true;
			});
			ServerPrefabUtil.workingComponents.Sort(delegate(Component lhs, Component rhs)
			{
				if (!(lhs is TextMeshPro) && !(lhs is TextMesh) && !(lhs is LODGroupAdditionalData))
				{
					return 0;
				}
				return -1;
			});
			foreach (Component obj in ServerPrefabUtil.workingComponents)
			{
				Object.DestroyImmediate(obj, true);
			}
			ServerPrefabUtil.workingComponents.Clear();
		}

		// Token: 0x06001A3D RID: 6717 RVA: 0x0005E56C File Offset: 0x0005C76C
		static ServerPrefabUtil()
		{
			HashSet<Type> hashSet = new HashSet<Type>();
			hashSet.Add(typeof(LODGroup));
			hashSet.Add(typeof(LODGroupAdditionalData));
			hashSet.Add(typeof(MeshFilter));
			hashSet.Add(typeof(Cloth));
			hashSet.Add(typeof(TextMesh));
			hashSet.Add(typeof(TextMeshPro));
			hashSet.Add(typeof(TextMeshProUGUI));
			hashSet.Add(typeof(WindZone));
			hashSet.Add(typeof(LensFlare));
			hashSet.Add(typeof(Projector));
			hashSet.Add(typeof(Camera));
			hashSet.Add(typeof(Skybox));
			hashSet.Add(typeof(FlareLayer));
			hashSet.Add(typeof(Light));
			hashSet.Add(typeof(LightProbeGroup));
			hashSet.Add(typeof(LightProbeProxyVolume));
			hashSet.Add(typeof(ReflectionProbe));
			hashSet.Add(typeof(Tree));
			hashSet.Add(typeof(CanvasRenderer));
			hashSet.Add(typeof(Button));
			hashSet.Add(typeof(CanvasScaler));
			hashSet.Add(typeof(Dropdown));
			hashSet.Add(typeof(Graphic));
			hashSet.Add(typeof(GridLayoutGroup));
			hashSet.Add(typeof(HorizontalLayoutGroup));
			hashSet.Add(typeof(Image));
			hashSet.Add(typeof(InputField));
			hashSet.Add(typeof(LayoutElement));
			hashSet.Add(typeof(LayoutGroup));
			hashSet.Add(typeof(Mask));
			hashSet.Add(typeof(MaskableGraphic));
			hashSet.Add(typeof(RawImage));
			hashSet.Add(typeof(RectMask2D));
			hashSet.Add(typeof(Scrollbar));
			hashSet.Add(typeof(ScrollRect));
			hashSet.Add(typeof(Slider));
			hashSet.Add(typeof(Text));
			hashSet.Add(typeof(Toggle));
			hashSet.Add(typeof(ToggleGroup));
			hashSet.Add(typeof(VerticalLayoutGroup));
			hashSet.Add(typeof(AudioChorusFilter));
			hashSet.Add(typeof(AudioDistortionFilter));
			hashSet.Add(typeof(AudioEchoFilter));
			hashSet.Add(typeof(AudioHighPassFilter));
			hashSet.Add(typeof(AudioListener));
			hashSet.Add(typeof(AudioLowPassFilter));
			hashSet.Add(typeof(AudioReverbFilter));
			hashSet.Add(typeof(AudioReverbZone));
			hashSet.Add(typeof(AudioSource));
			hashSet.Add(typeof(Renderer));
			hashSet.Add(typeof(BillboardRenderer));
			hashSet.Add(typeof(LineRenderer));
			hashSet.Add(typeof(MeshRenderer));
			hashSet.Add(typeof(ParticleSystemRenderer));
			hashSet.Add(typeof(SkinnedMeshRenderer));
			hashSet.Add(typeof(SpriteMask));
			hashSet.Add(typeof(SpriteRenderer));
			hashSet.Add(typeof(TrailRenderer));
			ServerPrefabUtil.typesToRemove = hashSet;
		}

		// Token: 0x04000C12 RID: 3090
		private static List<Component> workingComponents = new List<Component>();

		// Token: 0x04000C13 RID: 3091
		private static HashSet<Type> typesToRemove;
	}
}
