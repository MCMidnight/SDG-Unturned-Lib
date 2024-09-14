using System;
using System.Collections.Generic;
using SDG.Framework.Devkit;
using SDG.Framework.Utilities;
using UnityEngine;
using UnityEngine.Profiling;

namespace SDG.Unturned
{
	// Token: 0x0200051C RID: 1308
	public class VolumeManager<TVolume, TManager> : VolumeManagerBase where TVolume : LevelVolume<TVolume, TManager> where TManager : VolumeManager<TVolume, TManager>
	{
		// Token: 0x060028EA RID: 10474 RVA: 0x000AE4FD File Offset: 0x000AC6FD
		public static TManager Get()
		{
			return VolumeManager<TVolume, TManager>.instance;
		}

		// Token: 0x060028EB RID: 10475 RVA: 0x000AE504 File Offset: 0x000AC704
		public IReadOnlyList<TVolume> GetAllVolumes()
		{
			return this.allVolumes;
		}

		// Token: 0x060028EC RID: 10476 RVA: 0x000AE50C File Offset: 0x000AC70C
		internal List<TVolume> InternalGetAllVolumes()
		{
			return this.allVolumes;
		}

		// Token: 0x060028ED RID: 10477 RVA: 0x000AE514 File Offset: 0x000AC714
		public TVolume GetRandomVolumeOrNull()
		{
			return this.allVolumes.RandomOrDefault<TVolume>();
		}

		// Token: 0x1700083A RID: 2106
		// (get) Token: 0x060028EE RID: 10478 RVA: 0x000AE521 File Offset: 0x000AC721
		// (set) Token: 0x060028EF RID: 10479 RVA: 0x000AE52C File Offset: 0x000AC72C
		public override ELevelVolumeVisibility Visibility
		{
			get
			{
				return this.visibility;
			}
			set
			{
				if (this.visibility != value)
				{
					this.visibility = value;
					ConvenientSavedata.get().write("Visibility_" + typeof(TVolume).Name, (long)value);
					if (Level.isEditor)
					{
						foreach (TVolume tvolume in this.allVolumes)
						{
							tvolume.UpdateEditorVisibility(this.visibility);
						}
					}
				}
			}
		}

		// Token: 0x060028F0 RID: 10480 RVA: 0x000AE5C4 File Offset: 0x000AC7C4
		public void ForceUpdateEditorVisibility()
		{
			foreach (TVolume tvolume in this.allVolumes)
			{
				tvolume.UpdateEditorVisibility(this.visibility);
			}
		}

		// Token: 0x060028F1 RID: 10481 RVA: 0x000AE620 File Offset: 0x000AC820
		public TVolume GetFirstOverlappingVolume(Vector3 position)
		{
			foreach (TVolume tvolume in this.allVolumes)
			{
				if (tvolume.IsPositionInsideVolume(position))
				{
					return tvolume;
				}
			}
			return default(TVolume);
		}

		// Token: 0x060028F2 RID: 10482 RVA: 0x000AE68C File Offset: 0x000AC88C
		public bool IsPositionInsideAnyVolume(Vector3 position)
		{
			return this.GetFirstOverlappingVolume(position) != null;
		}

		// Token: 0x060028F3 RID: 10483 RVA: 0x000AE6A0 File Offset: 0x000AC8A0
		public override bool Raycast(Ray ray, out RaycastHit hitInfo, float maxDistance)
		{
			TVolume tvolume;
			return this.Raycast(ray, out hitInfo, out tvolume, maxDistance);
		}

		// Token: 0x060028F4 RID: 10484 RVA: 0x000AE6B8 File Offset: 0x000AC8B8
		public override void InstantiateVolume(Vector3 position, Quaternion rotation, Vector3 scale)
		{
			if (this.allowInstantiation)
			{
				if (this.Visibility == ELevelVolumeVisibility.Hidden)
				{
					this.Visibility = ELevelVolumeVisibility.Wireframe;
				}
				DevkitTypeFactory.instantiate(typeof(TVolume), position, rotation, scale);
			}
		}

		// Token: 0x060028F5 RID: 10485 RVA: 0x000AE6E3 File Offset: 0x000AC8E3
		public override IEnumerable<VolumeBase> EnumerateAllVolumes()
		{
			return this.allVolumes;
		}

		// Token: 0x060028F6 RID: 10486 RVA: 0x000AE6EC File Offset: 0x000AC8EC
		public bool Raycast(Ray ray, out RaycastHit hitInfo, out TVolume hitVolume, float maxDistance)
		{
			hitInfo = default(RaycastHit);
			hitVolume = default(TVolume);
			float num = maxDistance + 10f;
			foreach (TVolume tvolume in this.allVolumes)
			{
				RaycastHit raycastHit;
				if (tvolume.volumeCollider.Raycast(ray, out raycastHit, maxDistance) && raycastHit.distance < num)
				{
					hitVolume = tvolume;
					num = raycastHit.distance;
					hitInfo = raycastHit;
				}
			}
			return hitVolume != null;
		}

		// Token: 0x060028F7 RID: 10487 RVA: 0x000AE798 File Offset: 0x000AC998
		public virtual void AddVolume(TVolume volume)
		{
			if (Level.isEditor)
			{
				volume.UpdateEditorVisibility(this.visibility);
			}
			this.allVolumes.Add(volume);
		}

		// Token: 0x060028F8 RID: 10488 RVA: 0x000AE7BE File Offset: 0x000AC9BE
		public virtual void RemoveVolume(TVolume volume)
		{
			this.allVolumes.RemoveFast(volume);
		}

		// Token: 0x060028F9 RID: 10489 RVA: 0x000AE7D0 File Offset: 0x000AC9D0
		public VolumeManager()
		{
			VolumeManager<TVolume, TManager>.instance = (TManager)((object)this);
			VolumeManagerBase.allManagers.Add(this);
			base.FriendlyName = typeof(TVolume).Name;
			this.allVolumes = new List<TVolume>();
			this.solidMaterial = new Material(VolumeManager<TVolume, TManager>.solidShader);
			this.solidMaterial.hideFlags = HideFlags.HideAndDontSave;
			this.gizmoUpdateSampler = CustomSampler.Create(base.GetType().Name + ".UpdateGizmos", false);
			long num;
			if (ConvenientSavedata.get().read("Visibility_" + typeof(TVolume).Name, out num))
			{
				this.visibility = (ELevelVolumeVisibility)num;
			}
			else
			{
				this.visibility = this.DefaultVisibility;
			}
			TimeUtility.updated += this.PrivateOnUpdateGizmos;
		}

		// Token: 0x060028FA RID: 10490 RVA: 0x000AE8AC File Offset: 0x000ACAAC
		protected virtual void OnUpdateGizmos(RuntimeGizmos runtimeGizmos)
		{
			foreach (TVolume tvolume in this.allVolumes)
			{
				Color color = tvolume.isSelected ? Color.yellow : this.debugColor;
				ELevelVolumeShape shape = tvolume.Shape;
				if (shape != ELevelVolumeShape.Box)
				{
					if (shape == ELevelVolumeShape.Sphere)
					{
						RuntimeGizmos.Get().Sphere(tvolume.transform.localToWorldMatrix, 0.5f, color, 0f, EGizmoLayer.World);
					}
				}
				else
				{
					RuntimeGizmos.Get().Box(tvolume.transform.localToWorldMatrix, Vector3.one, color, 0f, EGizmoLayer.World);
				}
			}
			if (this.supportsFalloff)
			{
				foreach (TVolume tvolume2 in this.allVolumes)
				{
					if (tvolume2.falloffDistance >= 0.0001f)
					{
						Color color2 = tvolume2.isSelected ? Color.yellow : this.debugColor;
						color2.a *= 0.25f;
						Matrix4x4 localToWorldMatrix = tvolume2.transform.localToWorldMatrix;
						ELevelVolumeShape shape = tvolume2.Shape;
						if (shape != ELevelVolumeShape.Box)
						{
							if (shape == ELevelVolumeShape.Sphere)
							{
								float num = 0.5f;
								float localInnerSphereRadius = tvolume2.GetLocalInnerSphereRadius();
								RuntimeGizmos.Get().Sphere(localToWorldMatrix, localInnerSphereRadius, color2, 0f, EGizmoLayer.World);
								RuntimeGizmos.Get().Line(localToWorldMatrix.MultiplyPoint3x4(new Vector3(localInnerSphereRadius, 0f, 0f)), localToWorldMatrix.MultiplyPoint3x4(new Vector3(num, 0f, 0f)), color2, 0f, EGizmoLayer.World);
								RuntimeGizmos.Get().Line(localToWorldMatrix.MultiplyPoint3x4(new Vector3(-localInnerSphereRadius, 0f, 0f)), localToWorldMatrix.MultiplyPoint3x4(new Vector3(-num, 0f, 0f)), color2, 0f, EGizmoLayer.World);
								RuntimeGizmos.Get().Line(localToWorldMatrix.MultiplyPoint3x4(new Vector3(0f, localInnerSphereRadius, 0f)), localToWorldMatrix.MultiplyPoint3x4(new Vector3(0f, num, 0f)), color2, 0f, EGizmoLayer.World);
								RuntimeGizmos.Get().Line(localToWorldMatrix.MultiplyPoint3x4(new Vector3(0f, -localInnerSphereRadius, 0f)), localToWorldMatrix.MultiplyPoint3x4(new Vector3(0f, -num, 0f)), color2, 0f, EGizmoLayer.World);
								RuntimeGizmos.Get().Line(localToWorldMatrix.MultiplyPoint3x4(new Vector3(0f, 0f, localInnerSphereRadius)), localToWorldMatrix.MultiplyPoint3x4(new Vector3(0f, 0f, num)), color2, 0f, EGizmoLayer.World);
								RuntimeGizmos.Get().Line(localToWorldMatrix.MultiplyPoint3x4(new Vector3(0f, 0f, -localInnerSphereRadius)), localToWorldMatrix.MultiplyPoint3x4(new Vector3(0f, 0f, -num)), color2, 0f, EGizmoLayer.World);
							}
						}
						else
						{
							Vector3 localInnerBoxSize = tvolume2.GetLocalInnerBoxSize();
							Vector3 vector = new Vector3(0.5f, 0.5f, 0.5f);
							Vector3 vector2 = localInnerBoxSize * 0.5f;
							RuntimeGizmos.Get().Box(localToWorldMatrix, localInnerBoxSize, color2, 0f, EGizmoLayer.World);
							RuntimeGizmos.Get().Line(localToWorldMatrix.MultiplyPoint3x4(new Vector3(vector2.x, vector2.y, vector2.z)), localToWorldMatrix.MultiplyPoint3x4(new Vector3(vector.x, vector.y, vector.z)), color2, 0f, EGizmoLayer.World);
							RuntimeGizmos.Get().Line(localToWorldMatrix.MultiplyPoint3x4(new Vector3(-vector2.x, vector2.y, vector2.z)), localToWorldMatrix.MultiplyPoint3x4(new Vector3(-vector.x, vector.y, vector.z)), color2, 0f, EGizmoLayer.World);
							RuntimeGizmos.Get().Line(localToWorldMatrix.MultiplyPoint3x4(new Vector3(vector2.x, vector2.y, -vector2.z)), localToWorldMatrix.MultiplyPoint3x4(new Vector3(vector.x, vector.y, -vector.z)), color2, 0f, EGizmoLayer.World);
							RuntimeGizmos.Get().Line(localToWorldMatrix.MultiplyPoint3x4(new Vector3(-vector2.x, vector2.y, -vector2.z)), localToWorldMatrix.MultiplyPoint3x4(new Vector3(-vector.x, vector.y, -vector.z)), color2, 0f, EGizmoLayer.World);
							RuntimeGizmos.Get().Line(localToWorldMatrix.MultiplyPoint3x4(new Vector3(vector2.x, -vector2.y, vector2.z)), localToWorldMatrix.MultiplyPoint3x4(new Vector3(vector.x, -vector.y, vector.z)), color2, 0f, EGizmoLayer.World);
							RuntimeGizmos.Get().Line(localToWorldMatrix.MultiplyPoint3x4(new Vector3(-vector2.x, -vector2.y, vector2.z)), localToWorldMatrix.MultiplyPoint3x4(new Vector3(-vector.x, -vector.y, vector.z)), color2, 0f, EGizmoLayer.World);
							RuntimeGizmos.Get().Line(localToWorldMatrix.MultiplyPoint3x4(new Vector3(vector2.x, -vector2.y, -vector2.z)), localToWorldMatrix.MultiplyPoint3x4(new Vector3(vector.x, -vector.y, -vector.z)), color2, 0f, EGizmoLayer.World);
							RuntimeGizmos.Get().Line(localToWorldMatrix.MultiplyPoint3x4(new Vector3(-vector2.x, -vector2.y, -vector2.z)), localToWorldMatrix.MultiplyPoint3x4(new Vector3(-vector.x, -vector.y, -vector.z)), color2, 0f, EGizmoLayer.World);
						}
					}
				}
			}
		}

		// Token: 0x060028FB RID: 10491 RVA: 0x000AEF18 File Offset: 0x000AD118
		protected void SetDebugColor(Color debugColor)
		{
			this.debugColor = debugColor;
			this.solidMaterial.SetColor("_Color", debugColor);
		}

		// Token: 0x1700083B RID: 2107
		// (get) Token: 0x060028FC RID: 10492 RVA: 0x000AEF32 File Offset: 0x000AD132
		protected virtual ELevelVolumeVisibility DefaultVisibility
		{
			get
			{
				return ELevelVolumeVisibility.Wireframe;
			}
		}

		// Token: 0x060028FD RID: 10493 RVA: 0x000AEF35 File Offset: 0x000AD135
		private void PrivateOnUpdateGizmos()
		{
			if (this.visibility == ELevelVolumeVisibility.Hidden || !Level.isEditor)
			{
				return;
			}
			this.OnUpdateGizmos(RuntimeGizmos.Get());
		}

		// Token: 0x040015C0 RID: 5568
		internal Color debugColor;

		// Token: 0x040015C1 RID: 5569
		internal Material solidMaterial;

		// Token: 0x040015C2 RID: 5570
		private ELevelVolumeVisibility visibility;

		// Token: 0x040015C3 RID: 5571
		private CustomSampler gizmoUpdateSampler;

		/// <summary>
		/// Should calling InstantiateVolume create a new volume?
		/// False for deprecated (landscape hole volume) types.
		/// </summary>
		// Token: 0x040015C4 RID: 5572
		protected bool allowInstantiation = true;

		// Token: 0x040015C5 RID: 5573
		protected bool supportsFalloff;

		// Token: 0x040015C6 RID: 5574
		protected List<TVolume> allVolumes;

		// Token: 0x040015C7 RID: 5575
		private static TManager instance;

		// Token: 0x040015C8 RID: 5576
		private static Shader solidShader = Shader.Find("Standard");
	}
}
