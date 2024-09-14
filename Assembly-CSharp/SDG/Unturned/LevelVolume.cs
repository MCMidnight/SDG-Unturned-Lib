using System;
using SDG.Framework.Devkit;
using SDG.Framework.Devkit.Interactable;
using SDG.Framework.IO.FormattedFiles;
using UnityEngine;
using UnityEngine.Rendering;

namespace SDG.Unturned
{
	// Token: 0x02000519 RID: 1305
	public class LevelVolume<TVolume, TManager> : VolumeBase, IDevkitInteractableBeginSelectionHandler, IDevkitInteractableEndSelectionHandler, ITransformedHandler where TVolume : LevelVolume<TVolume, TManager> where TManager : VolumeManager<TVolume, TManager>
	{
		// Token: 0x060028BE RID: 10430 RVA: 0x000AD8C6 File Offset: 0x000ABAC6
		public override ISleekElement CreateMenu()
		{
			if ((this.supportsBoxShape && this.supportsSphereShape) || this.supportsFalloff)
			{
				return new LevelVolume<TVolume, TManager>.Menu(this);
			}
			return null;
		}

		// Token: 0x17000837 RID: 2103
		// (get) Token: 0x060028BF RID: 10431 RVA: 0x000AD8E8 File Offset: 0x000ABAE8
		// (set) Token: 0x060028C0 RID: 10432 RVA: 0x000AD8F0 File Offset: 0x000ABAF0
		public virtual ELevelVolumeShape Shape
		{
			get
			{
				return this._shape;
			}
			set
			{
				if (this._shape != value)
				{
					this._shape = value;
					if (this.volumeCollider != null)
					{
						bool enabled = this.volumeCollider.enabled;
						bool isTrigger = this.volumeCollider.isTrigger;
						Object.Destroy(this.volumeCollider);
						if (value != ELevelVolumeShape.Box)
						{
							if (value == ELevelVolumeShape.Sphere)
							{
								this.volumeCollider = base.gameObject.AddComponent<SphereCollider>();
							}
						}
						else
						{
							this.volumeCollider = base.gameObject.AddComponent<BoxCollider>();
						}
						this.volumeCollider.enabled = enabled;
						this.volumeCollider.isTrigger = isTrigger;
					}
					if (this.editorMeshFilter != null)
					{
						this.SyncEditorMeshToShape();
					}
					if (value == ELevelVolumeShape.Sphere)
					{
						float max = base.transform.localScale.GetAbs().GetMax();
						base.transform.localScale = new Vector3(max, max, max);
					}
				}
			}
		}

		// Token: 0x060028C1 RID: 10433 RVA: 0x000AD9C7 File Offset: 0x000ABBC7
		public virtual void beginSelection(InteractionData data)
		{
			this.isSelected = true;
		}

		// Token: 0x060028C2 RID: 10434 RVA: 0x000AD9D0 File Offset: 0x000ABBD0
		public virtual void endSelection(InteractionData data)
		{
			this.isSelected = false;
		}

		// Token: 0x060028C3 RID: 10435 RVA: 0x000AD9DC File Offset: 0x000ABBDC
		public void OnTransformed(Vector3 oldPosition, Quaternion oldRotation, Vector3 oldLocalScale, Vector3 newPosition, Quaternion newRotation, Vector3 newLocalScale, bool modifyRotation, bool modifyScale)
		{
			if (!newPosition.IsNearlyEqual(base.transform.position, 0.001f))
			{
				base.transform.position = newPosition;
			}
			if (modifyRotation)
			{
				base.transform.SetRotation_RoundIfNearlyAxisAligned(newRotation, 0.05f);
			}
			if (modifyScale)
			{
				if (this.Shape == ELevelVolumeShape.Sphere)
				{
					if (newLocalScale.sqrMagnitude > oldLocalScale.sqrMagnitude)
					{
						float max = newLocalScale.GetAbs().GetMax();
						newLocalScale = new Vector3(max, max, max);
					}
					else
					{
						float min = newLocalScale.GetAbs().GetMin();
						newLocalScale = new Vector3(min, min, min);
					}
				}
				base.transform.SetLocalScale_RoundIfNearlyEqualToOne(newLocalScale, 0.001f);
			}
		}

		// Token: 0x060028C4 RID: 10436 RVA: 0x000ADA84 File Offset: 0x000ABC84
		public bool IsPositionInsideVolume(Vector3 position)
		{
			ELevelVolumeShape shape = this._shape;
			if (shape == ELevelVolumeShape.Box)
			{
				Vector3 vector = base.transform.InverseTransformPoint(position);
				return Mathf.Abs(vector.x) < 0.5f && Mathf.Abs(vector.y) < 0.5f && Mathf.Abs(vector.z) < 0.5f;
			}
			if (shape != ELevelVolumeShape.Sphere)
			{
				throw new NotImplementedException();
			}
			float sphereRadius = this.GetSphereRadius();
			float num = sphereRadius * sphereRadius;
			return (position - base.transform.position).sqrMagnitude < num;
		}

		/// <summary>
		/// Alpha is 0.0 outside volume and 1.0 inside inner volume.
		/// </summary>
		// Token: 0x060028C5 RID: 10437 RVA: 0x000ADB14 File Offset: 0x000ABD14
		public bool IsPositionInsideVolumeWithAlpha(Vector3 position, out float alpha)
		{
			if (this.falloffDistance < 0.0001f)
			{
				alpha = 1f;
				return this.IsPositionInsideVolume(position);
			}
			ELevelVolumeShape shape = this._shape;
			if (shape != ELevelVolumeShape.Box)
			{
				if (shape != ELevelVolumeShape.Sphere)
				{
					throw new NotImplementedException();
				}
				float sphereRadius = this.GetSphereRadius();
				float num = sphereRadius * sphereRadius;
				float sqrMagnitude = (position - base.transform.position).sqrMagnitude;
				if (sqrMagnitude < num)
				{
					float value = Mathf.Sqrt(sqrMagnitude);
					float b = Mathf.Max(0f, sphereRadius - this.falloffDistance);
					alpha = Mathf.InverseLerp(sphereRadius, b, value);
					return true;
				}
				alpha = 0f;
				return false;
			}
			else
			{
				Vector3 abs = base.transform.InverseTransformPoint(position).GetAbs();
				if (abs.x < 0.5f && abs.y < 0.5f && abs.z < 0.5f)
				{
					Vector3 a = new Vector3(0.5f, 0.5f, 0.5f);
					Vector3 localInnerBoxExtents = this.GetLocalInnerBoxExtents();
					Vector3 vector = MathfEx.InverseLerp(a, localInnerBoxExtents, abs);
					alpha = vector.GetMin();
					return true;
				}
				alpha = 0f;
				return false;
			}
		}

		/// <summary>
		/// World space size of the box.
		/// </summary>
		// Token: 0x060028C6 RID: 10438 RVA: 0x000ADC28 File Offset: 0x000ABE28
		public Vector3 GetBoxSize()
		{
			return base.transform.localScale.GetAbs();
		}

		/// <summary>
		/// Half the world space size of the box.
		/// </summary>
		// Token: 0x060028C7 RID: 10439 RVA: 0x000ADC3A File Offset: 0x000ABE3A
		public Vector3 GetBoxExtents()
		{
			return base.transform.localScale.GetAbs() * 0.5f;
		}

		/// <summary>
		/// World space size of inner falloff box when falloffDistance is non-zero.
		/// For example a 24x12x6 box with a falloff of 4 has an inner box sized 16x4x0.
		/// </summary>
		// Token: 0x060028C8 RID: 10440 RVA: 0x000ADC58 File Offset: 0x000ABE58
		public Vector3 GetInnerBoxSize()
		{
			Vector3 abs = base.transform.localScale.GetAbs();
			abs.x = Mathf.Max(0f, abs.x - this.falloffDistance * 2f);
			abs.y = Mathf.Max(0f, abs.y - this.falloffDistance * 2f);
			abs.z = Mathf.Max(0f, abs.z - this.falloffDistance * 2f);
			return abs;
		}

		/// <summary>
		/// World space extents of inner falloff box when falloffDistance is non-zero.
		/// </summary>
		// Token: 0x060028C9 RID: 10441 RVA: 0x000ADCE4 File Offset: 0x000ABEE4
		public Vector3 GetInnerBoxExtents()
		{
			Vector3 vector = base.transform.localScale.GetAbs() * 0.5f;
			vector.x = Mathf.Max(0f, vector.x - this.falloffDistance);
			vector.y = Mathf.Max(0f, vector.y - this.falloffDistance);
			vector.z = Mathf.Max(0f, vector.z - this.falloffDistance);
			return vector;
		}

		/// <summary>
		/// Local space size of inner falloff box when falloffDistance is non-zero.
		/// </summary>
		// Token: 0x060028CA RID: 10442 RVA: 0x000ADD68 File Offset: 0x000ABF68
		public Vector3 GetLocalInnerBoxSize()
		{
			Vector3 abs = base.transform.localScale.GetAbs();
			return new Vector3(Mathf.Max(0f, abs.x - this.falloffDistance * 2f) / abs.x, Mathf.Max(0f, abs.y - this.falloffDistance * 2f) / abs.y, Mathf.Max(0f, abs.z - this.falloffDistance * 2f) / abs.z);
		}

		/// <summary>
		/// Local space extents of inner falloff box when falloffDistance is non-zero.
		/// </summary>
		// Token: 0x060028CB RID: 10443 RVA: 0x000ADDF8 File Offset: 0x000ABFF8
		public Vector3 GetLocalInnerBoxExtents()
		{
			Vector3 abs = base.transform.localScale.GetAbs();
			return new Vector3(Mathf.Max(0f, abs.x * 0.5f - this.falloffDistance) / abs.x, Mathf.Max(0f, abs.y * 0.5f - this.falloffDistance) / abs.y, Mathf.Max(0f, abs.z * 0.5f - this.falloffDistance) / abs.z);
		}

		/// <summary>
		/// World space radius of the sphere.
		/// </summary>
		// Token: 0x060028CC RID: 10444 RVA: 0x000ADE87 File Offset: 0x000AC087
		public float GetSphereRadius()
		{
			return base.transform.localScale.GetAbs().GetMax() * 0.5f;
		}

		/// <summary>
		/// Local space radius of the sphere.
		/// </summary>
		// Token: 0x060028CD RID: 10445 RVA: 0x000ADEA4 File Offset: 0x000AC0A4
		public float GetLocalSphereRadius()
		{
			return 0.5f;
		}

		/// <summary>
		/// World space radius of inner falloff sphere when falloffDistance is non-zero.
		/// </summary>
		// Token: 0x060028CE RID: 10446 RVA: 0x000ADEAC File Offset: 0x000AC0AC
		public float GetWorldSpaceInnerSphereRadius()
		{
			float num = base.transform.localScale.GetAbs().GetMax() * 0.5f;
			return Mathf.Max(0f, num - this.falloffDistance);
		}

		/// <summary>
		/// Local space radius of inner falloff sphere when falloffDistance is non-zero.
		/// </summary>
		// Token: 0x060028CF RID: 10447 RVA: 0x000ADEE8 File Offset: 0x000AC0E8
		public float GetLocalInnerSphereRadius()
		{
			float max = base.transform.localScale.GetAbs().GetMax();
			float num = max * 0.5f;
			return Mathf.Max(0f, num - this.falloffDistance) / max;
		}

		// Token: 0x060028D0 RID: 10448 RVA: 0x000ADF28 File Offset: 0x000AC128
		public void SetSphereRadius(float radius)
		{
			float num = radius * 2f;
			base.transform.localScale = new Vector3(num, num, num);
		}

		/// <summary>
		/// Useful for code which previously depended on creating the Unity collider to calculate bounding box.
		/// </summary>
		// Token: 0x060028D1 RID: 10449 RVA: 0x000ADF50 File Offset: 0x000AC150
		public Bounds CalculateWorldBounds()
		{
			Bounds result = new Bounds(base.transform.position, Vector3.zero);
			Matrix4x4 localToWorldMatrix = base.transform.localToWorldMatrix;
			result.Encapsulate(localToWorldMatrix.MultiplyPoint3x4(new Vector3(-0.5f, -0.5f, -0.5f)));
			result.Encapsulate(localToWorldMatrix.MultiplyPoint3x4(new Vector3(-0.5f, -0.5f, 0.5f)));
			result.Encapsulate(localToWorldMatrix.MultiplyPoint3x4(new Vector3(-0.5f, 0.5f, -0.5f)));
			result.Encapsulate(localToWorldMatrix.MultiplyPoint3x4(new Vector3(-0.5f, 0.5f, 0.5f)));
			result.Encapsulate(localToWorldMatrix.MultiplyPoint3x4(new Vector3(0.5f, -0.5f, -0.5f)));
			result.Encapsulate(localToWorldMatrix.MultiplyPoint3x4(new Vector3(0.5f, -0.5f, 0.5f)));
			result.Encapsulate(localToWorldMatrix.MultiplyPoint3x4(new Vector3(0.5f, 0.5f, -0.5f)));
			result.Encapsulate(localToWorldMatrix.MultiplyPoint3x4(new Vector3(0.5f, 0.5f, 0.5f)));
			return result;
		}

		// Token: 0x060028D2 RID: 10450 RVA: 0x000AE091 File Offset: 0x000AC291
		public Bounds CalculateLocalBounds()
		{
			return new Bounds(Vector3.zero, base.transform.localScale.GetAbs());
		}

		/// <summary>
		/// Called in the level editor during registraion and when visibility is changed.
		/// </summary>
		// Token: 0x060028D3 RID: 10451 RVA: 0x000AE0AD File Offset: 0x000AC2AD
		public virtual void UpdateEditorVisibility(ELevelVolumeVisibility visibility)
		{
			this.volumeCollider.enabled = (visibility > ELevelVolumeVisibility.Hidden);
			this.editorGameObject.SetActive(visibility == ELevelVolumeVisibility.Solid);
		}

		// Token: 0x060028D4 RID: 10452 RVA: 0x000AE0CD File Offset: 0x000AC2CD
		protected virtual void OnEnable()
		{
			LevelHierarchy.addItem(this);
			VolumeManager<TVolume, TManager>.Get().AddVolume((TVolume)((object)this));
		}

		// Token: 0x060028D5 RID: 10453 RVA: 0x000AE0EA File Offset: 0x000AC2EA
		protected virtual void OnDisable()
		{
			VolumeManager<TVolume, TManager>.Get().RemoveVolume((TVolume)((object)this));
			LevelHierarchy.removeItem(this);
		}

		// Token: 0x060028D6 RID: 10454 RVA: 0x000AE108 File Offset: 0x000AC308
		protected override void readHierarchyItem(IFormattedFileReader reader)
		{
			base.readHierarchyItem(reader);
			if (reader.containsKey("Shape"))
			{
				this.Shape = reader.readValue<ELevelVolumeShape>("Shape");
			}
			else
			{
				this.Shape = ELevelVolumeShape.Box;
			}
			if (this.supportsFalloff && reader.containsKey("Falloff"))
			{
				this.falloffDistance = reader.readValue<float>("Falloff");
			}
		}

		// Token: 0x060028D7 RID: 10455 RVA: 0x000AE169 File Offset: 0x000AC369
		protected override void writeHierarchyItem(IFormattedFileWriter writer)
		{
			base.writeHierarchyItem(writer);
			writer.writeValue<ELevelVolumeShape>("Shape", this.Shape);
			if (this.supportsFalloff)
			{
				writer.writeValue<float>("Falloff", this.falloffDistance);
			}
		}

		// Token: 0x060028D8 RID: 10456 RVA: 0x000AE19C File Offset: 0x000AC39C
		protected virtual void Awake()
		{
			base.gameObject.layer = 30;
			if (this._shape == ELevelVolumeShape.Box && !this.supportsBoxShape)
			{
				this._shape = ELevelVolumeShape.Sphere;
			}
			else if (this._shape == ELevelVolumeShape.Sphere && !this.supportsSphereShape)
			{
				this._shape = ELevelVolumeShape.Box;
			}
			bool flag = this.forceShouldAddCollider || Level.isEditor;
			if (this.volumeCollider == null)
			{
				Collider component = base.GetComponent<Collider>();
				if (component != null)
				{
					bool flag2 = false;
					if (component is BoxCollider)
					{
						if (this.supportsBoxShape)
						{
							this._shape = ELevelVolumeShape.Box;
							flag2 = flag;
						}
					}
					else if (component is SphereCollider && this.supportsSphereShape)
					{
						this._shape = ELevelVolumeShape.Sphere;
						flag2 = flag;
					}
					if (flag2)
					{
						this.volumeCollider = component;
						this.volumeCollider.isTrigger = true;
					}
					else
					{
						Object.Destroy(component);
					}
				}
			}
			if (flag && this.volumeCollider == null)
			{
				ELevelVolumeShape shape = this._shape;
				if (shape != ELevelVolumeShape.Box)
				{
					if (shape == ELevelVolumeShape.Sphere)
					{
						this.volumeCollider = base.gameObject.AddComponent<SphereCollider>();
					}
				}
				else
				{
					this.volumeCollider = base.gameObject.AddComponent<BoxCollider>();
				}
				this.volumeCollider.isTrigger = true;
			}
			if (Level.isEditor && this.editorGameObject == null)
			{
				this.editorGameObject = new GameObject("EditorPreview");
				this.editorGameObject.transform.SetParent(base.transform, false);
				this.editorGameObject.layer = 18;
				this.editorMeshFilter = this.editorGameObject.AddComponent<MeshFilter>();
				this.SyncEditorMeshToShape();
				this.editorMeshRenderer = this.editorGameObject.AddComponent<MeshRenderer>();
				this.editorMeshRenderer.shadowCastingMode = ShadowCastingMode.Off;
				this.editorMeshRenderer.sharedMaterial = VolumeManager<TVolume, TManager>.Get().solidMaterial;
			}
		}

		// Token: 0x060028D9 RID: 10457 RVA: 0x000AE358 File Offset: 0x000AC558
		protected virtual void Start()
		{
			NetId netIdFromInstanceId = base.GetNetIdFromInstanceId();
			if (!netIdFromInstanceId.IsNull())
			{
				NetIdRegistry.Assign(netIdFromInstanceId, this);
			}
		}

		// Token: 0x060028DA RID: 10458 RVA: 0x000AE37C File Offset: 0x000AC57C
		protected virtual void OnDestroy()
		{
			NetId netIdFromInstanceId = base.GetNetIdFromInstanceId();
			if (!netIdFromInstanceId.IsNull())
			{
				NetIdRegistry.Release(netIdFromInstanceId);
			}
		}

		// Token: 0x060028DB RID: 10459 RVA: 0x000AE3A0 File Offset: 0x000AC5A0
		protected void AppendBaseMenu(ISleekElement childMenu)
		{
			if ((this.supportsBoxShape && this.supportsSphereShape) || this.supportsFalloff)
			{
				LevelVolume<TVolume, TManager>.Menu menu = new LevelVolume<TVolume, TManager>.Menu(this);
				menu.PositionScale_Y = 1f;
				menu.PositionOffset_Y = -menu.SizeOffset_Y;
				childMenu.SizeOffset_Y += menu.SizeOffset_Y + 10f;
				childMenu.AddChild(menu);
			}
		}

		// Token: 0x060028DC RID: 10460 RVA: 0x000AE404 File Offset: 0x000AC604
		internal TManager GetVolumeManager()
		{
			return VolumeManager<TVolume, TManager>.Get();
		}

		// Token: 0x060028DD RID: 10461 RVA: 0x000AE40C File Offset: 0x000AC60C
		private void SyncEditorMeshToShape()
		{
			ELevelVolumeShape shape = this._shape;
			if (shape == ELevelVolumeShape.Box)
			{
				this.editorMeshFilter.sharedMesh = LevelVolume<TVolume, TManager>.GetCubeMesh();
				return;
			}
			if (shape != ELevelVolumeShape.Sphere)
			{
				this.editorMeshFilter.sharedMesh = null;
				return;
			}
			this.editorMeshFilter.sharedMesh = LevelVolume<TVolume, TManager>.GetSphereMesh();
		}

		// Token: 0x060028DE RID: 10462 RVA: 0x000AE457 File Offset: 0x000AC657
		private static Mesh GetCubeMesh()
		{
			if (LevelVolume<TVolume, TManager>._cubeMesh == null)
			{
				LevelVolume<TVolume, TManager>._cubeMesh = Resources.Load<GameObject>("Shapes/TwoSidedUnitCube").GetComponent<MeshFilter>().sharedMesh;
			}
			return LevelVolume<TVolume, TManager>._cubeMesh;
		}

		// Token: 0x060028DF RID: 10463 RVA: 0x000AE484 File Offset: 0x000AC684
		private static Mesh GetSphereMesh()
		{
			if (LevelVolume<TVolume, TManager>._sphereMesh == null)
			{
				LevelVolume<TVolume, TManager>._sphereMesh = Resources.Load<GameObject>("Shapes/TwoSidedOneDiameterSphere").GetComponent<MeshFilter>().sharedMesh;
			}
			return LevelVolume<TVolume, TManager>._sphereMesh;
		}

		// Token: 0x040015AC RID: 5548
		[SerializeField]
		private ELevelVolumeShape _shape;

		/// <summary>
		/// Distance inward from edge before intensity reaches 100%.
		/// </summary>
		// Token: 0x040015AD RID: 5549
		public float falloffDistance;

		// Token: 0x040015AE RID: 5550
		internal bool isSelected;

		// Token: 0x040015AF RID: 5551
		[SerializeField]
		internal Collider volumeCollider;

		/// <summary>
		/// Editor-only solid/opaque child mesh renderer object.
		/// </summary>
		// Token: 0x040015B0 RID: 5552
		[SerializeField]
		protected GameObject editorGameObject;

		// Token: 0x040015B1 RID: 5553
		[SerializeField]
		protected MeshFilter editorMeshFilter;

		// Token: 0x040015B2 RID: 5554
		[SerializeField]
		protected MeshRenderer editorMeshRenderer;

		/// <summary>
		/// If true during Awake the collider component will be added.
		/// Otherwise only in the level editor. Some volume types like water use the collider in gameplay,
		/// whereas most only need the collider for general-purpose selection in the level editor.
		/// </summary>
		// Token: 0x040015B3 RID: 5555
		protected bool forceShouldAddCollider;

		// Token: 0x040015B4 RID: 5556
		protected bool supportsBoxShape = true;

		// Token: 0x040015B5 RID: 5557
		protected bool supportsSphereShape = true;

		// Token: 0x040015B6 RID: 5558
		protected bool supportsFalloff;

		// Token: 0x040015B7 RID: 5559
		private static Mesh _cubeMesh;

		// Token: 0x040015B8 RID: 5560
		private static Mesh _sphereMesh;

		// Token: 0x02000961 RID: 2401
		private class Menu : SleekWrapper
		{
			// Token: 0x06004B25 RID: 19237 RVA: 0x001B381C File Offset: 0x001B1A1C
			public Menu(LevelVolume<TVolume, TManager> volume)
			{
				this.volume = volume;
				base.SizeOffset_X = 400f;
				float num = 0f;
				if (volume.supportsBoxShape && volume.supportsSphereShape)
				{
					this.shapeButton = new SleekButtonStateEnum<ELevelVolumeShape>();
					this.shapeButton.PositionOffset_Y = num;
					this.shapeButton.SizeOffset_X = 200f;
					this.shapeButton.SizeOffset_Y = 30f;
					this.shapeButton.SetEnum(volume.Shape);
					this.shapeButton.AddLabel("Shape", 1);
					SleekButtonStateEnum<ELevelVolumeShape> sleekButtonStateEnum = this.shapeButton;
					sleekButtonStateEnum.OnSwappedEnum = (Action<SleekButtonStateEnum<ELevelVolumeShape>, ELevelVolumeShape>)Delegate.Combine(sleekButtonStateEnum.OnSwappedEnum, new Action<SleekButtonStateEnum<ELevelVolumeShape>, ELevelVolumeShape>(this.OnShapeChanged));
					base.AddChild(this.shapeButton);
					num += this.shapeButton.SizeOffset_Y + 10f;
				}
				if (volume.supportsFalloff)
				{
					this.falloffField = Glazier.Get().CreateFloat32Field();
					this.falloffField.PositionOffset_Y = num;
					this.falloffField.SizeOffset_X = 200f;
					this.falloffField.SizeOffset_Y = 30f;
					this.falloffField.Value = volume.falloffDistance;
					this.falloffField.AddLabel("Falloff", 1);
					this.falloffField.OnValueChanged += new TypedSingle(this.OnFalloffTyped);
					base.AddChild(this.falloffField);
					num += this.falloffField.SizeOffset_Y + 10f;
				}
				base.SizeOffset_Y = num - 10f;
				this.prevShape = volume.Shape;
			}

			// Token: 0x06004B26 RID: 19238 RVA: 0x001B39B8 File Offset: 0x001B1BB8
			public override void OnUpdate()
			{
				ELevelVolumeShape shape = this.volume.Shape;
				if (this.prevShape != shape)
				{
					this.prevShape = shape;
					this.shapeButton.SetEnum(shape);
				}
			}

			// Token: 0x06004B27 RID: 19239 RVA: 0x001B39F0 File Offset: 0x001B1BF0
			private void OnShapeChanged(SleekButtonStateEnum<ELevelVolumeShape> button, ELevelVolumeShape state)
			{
				this.prevShape = state;
				using (new ScopedObjectUndo(this.volume))
				{
					this.volume.Shape = state;
				}
			}

			// Token: 0x06004B28 RID: 19240 RVA: 0x001B3A38 File Offset: 0x001B1C38
			private void OnFalloffTyped(ISleekFloat32Field field, float state)
			{
				this.volume.falloffDistance = state;
			}

			// Token: 0x0400333D RID: 13117
			private ELevelVolumeShape prevShape;

			// Token: 0x0400333E RID: 13118
			private SleekButtonStateEnum<ELevelVolumeShape> shapeButton;

			// Token: 0x0400333F RID: 13119
			private ISleekFloat32Field falloffField;

			// Token: 0x04003340 RID: 13120
			private LevelVolume<TVolume, TManager> volume;
		}
	}
}
