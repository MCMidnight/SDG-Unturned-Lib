using System;
using System.Collections.Generic;
using SDG.Framework.Devkit;
using SDG.Framework.Devkit.Tools;
using SDG.Framework.Devkit.Transactions;
using SDG.Framework.Foliage;
using SDG.Framework.Rendering;
using SDG.Framework.Utilities;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000413 RID: 1043
	internal class FoliageEditor : IDevkitTool
	{
		// Token: 0x1700064B RID: 1611
		// (get) Token: 0x06001EA4 RID: 7844 RVA: 0x00071314 File Offset: 0x0006F514
		// (set) Token: 0x06001EA5 RID: 7845 RVA: 0x00071320 File Offset: 0x0006F520
		public float brushRadius
		{
			get
			{
				return DevkitFoliageToolOptions.instance.brushRadius;
			}
			set
			{
				DevkitFoliageToolOptions.instance.brushRadius = value;
			}
		}

		// Token: 0x1700064C RID: 1612
		// (get) Token: 0x06001EA6 RID: 7846 RVA: 0x0007132D File Offset: 0x0006F52D
		// (set) Token: 0x06001EA7 RID: 7847 RVA: 0x00071339 File Offset: 0x0006F539
		public float brushFalloff
		{
			get
			{
				return DevkitFoliageToolOptions.instance.brushFalloff;
			}
			set
			{
				DevkitFoliageToolOptions.instance.brushFalloff = value;
			}
		}

		// Token: 0x1700064D RID: 1613
		// (get) Token: 0x06001EA8 RID: 7848 RVA: 0x00071346 File Offset: 0x0006F546
		// (set) Token: 0x06001EA9 RID: 7849 RVA: 0x00071352 File Offset: 0x0006F552
		public float brushStrength
		{
			get
			{
				return DevkitFoliageToolOptions.instance.brushStrength;
			}
			set
			{
				DevkitFoliageToolOptions.instance.brushStrength = value;
			}
		}

		// Token: 0x1700064E RID: 1614
		// (get) Token: 0x06001EAA RID: 7850 RVA: 0x0007135F File Offset: 0x0006F55F
		// (set) Token: 0x06001EAB RID: 7851 RVA: 0x0007136B File Offset: 0x0006F56B
		public uint maxPreviewSamples
		{
			get
			{
				return DevkitFoliageToolOptions.instance.maxPreviewSamples;
			}
			set
			{
				DevkitFoliageToolOptions.instance.maxPreviewSamples = value;
			}
		}

		// Token: 0x1700064F RID: 1615
		// (get) Token: 0x06001EAC RID: 7852 RVA: 0x00071378 File Offset: 0x0006F578
		private bool isChangingBrush
		{
			get
			{
				return this.isChangingBrushRadius || this.isChangingBrushFalloff || this.isChangingBrushStrength;
			}
		}

		// Token: 0x06001EAD RID: 7853 RVA: 0x00071392 File Offset: 0x0006F592
		private void beginChangeHotkeyTransaction()
		{
			DevkitTransactionUtility.beginGenericTransaction();
			DevkitTransactionUtility.recordObjectDelta(DevkitFoliageToolOptions.instance);
		}

		// Token: 0x06001EAE RID: 7854 RVA: 0x000713A3 File Offset: 0x0006F5A3
		private void endChangeHotkeyTransaction()
		{
			DevkitTransactionUtility.endGenericTransaction();
		}

		// Token: 0x06001EAF RID: 7855 RVA: 0x000713AC File Offset: 0x0006F5AC
		private void addFoliage(FoliageInfoAsset foliageAsset, float weightMultiplier)
		{
			if (foliageAsset == null)
			{
				return;
			}
			bool flag = false;
			float num = 3.1415927f * this.brushRadius * this.brushRadius;
			float num2 = (DevkitFoliageToolOptions.instance.densityTarget > 0.0001f) ? Mathf.Sqrt(foliageAsset.density / DevkitFoliageToolOptions.instance.densityTarget / 3.1415927f) : 0f;
			float num3;
			if (!this.addWeights.TryGetValue(foliageAsset, ref num3))
			{
				this.addWeights.Add(foliageAsset, 0f);
			}
			num3 += DevkitFoliageToolOptions.addSensitivity * num * this.brushStrength * weightMultiplier * Time.deltaTime;
			if (num3 > 1f)
			{
				this.previewSamples.Clear();
				int num4 = Mathf.FloorToInt(num3);
				num3 -= (float)num4;
				for (int i = 0; i < num4; i++)
				{
					float num5 = this.brushRadius * Random.value;
					float brushAlpha = this.getBrushAlpha(num5);
					if (Random.value >= brushAlpha)
					{
						float f = 6.2831855f * Random.value;
						float x = Mathf.Cos(f) * num5;
						float z = Mathf.Sin(f) * num5;
						RaycastHit raycastHit;
						if (Physics.Raycast(new Ray(this.brushWorldPosition + new Vector3(x, this.brushRadius, z), new Vector3(0f, -1f, 0f)), out raycastHit, this.brushRadius * 2f, (int)DevkitFoliageToolOptions.instance.surfaceMask))
						{
							if (num2 > 0.0001f)
							{
								SphereVolume sphereVolume = new SphereVolume(raycastHit.point, num2);
								if (foliageAsset.getInstanceCountInVolume(sphereVolume) > 0)
								{
									goto IL_18A;
								}
							}
							foliageAsset.addFoliageToSurface(raycastHit.point, raycastHit.normal, false, true);
							flag = true;
						}
					}
					IL_18A:;
				}
			}
			this.addWeights[foliageAsset] = num3;
			if (flag)
			{
				LevelHierarchy.MarkDirty();
			}
		}

		// Token: 0x06001EB0 RID: 7856 RVA: 0x00071568 File Offset: 0x0006F768
		private void removeInstances(FoliageTile foliageTile, FoliageInstanceList list, float sqrBrushRadius, float sqrBrushFalloffRadius, bool allowRemoveBaked, ref int sampleCount)
		{
			bool flag = false;
			for (int i = list.matrices.Count - 1; i >= 0; i--)
			{
				List<Matrix4x4> list2 = list.matrices[i];
				List<bool> list3 = list.clearWhenBaked[i];
				for (int j = list2.Count - 1; j >= 0; j--)
				{
					if (!list3[j] || allowRemoveBaked)
					{
						Vector3 position = list2[j].GetPosition();
						float sqrMagnitude = (position - this.brushWorldPosition).sqrMagnitude;
						if (sqrMagnitude < sqrBrushRadius)
						{
							bool flag2 = sqrMagnitude < sqrBrushFalloffRadius;
							this.previewSamples.Add(new FoliagePreviewSample(position, flag2 ? Color.red : (Color.red / 2f)));
							if (InputEx.GetKey(KeyCode.Mouse0) && flag2 && sampleCount > 0)
							{
								foliageTile.removeInstance(list, i, j);
								sampleCount--;
								flag = true;
							}
						}
					}
				}
			}
			if (flag)
			{
				LevelHierarchy.MarkDirty();
			}
		}

		// Token: 0x06001EB1 RID: 7857 RVA: 0x00071674 File Offset: 0x0006F874
		public void update()
		{
			Ray ray = EditorInteract.ray;
			RaycastHit raycastHit;
			this.isPointerOnWorld = Physics.Raycast(ray, out raycastHit, 8192f, (int)DevkitFoliageToolOptions.instance.surfaceMask);
			this.pointerWorldPosition = raycastHit.point;
			this.previewSamples.Clear();
			if (!EditorInteract.isFlying && Glazier.Get().ShouldGameProcessInput)
			{
				if (InputEx.GetKeyDown(KeyCode.Q))
				{
					this.mode = FoliageEditor.EFoliageMode.PAINT;
				}
				if (InputEx.GetKeyDown(KeyCode.W))
				{
					this.mode = FoliageEditor.EFoliageMode.EXACT;
				}
				if (InputEx.GetKeyDown(KeyCode.E))
				{
					this.mode = FoliageEditor.EFoliageMode.BAKE;
				}
				if (this.mode == FoliageEditor.EFoliageMode.PAINT)
				{
					if (InputEx.GetKeyDown(KeyCode.B))
					{
						this.isChangingBrushRadius = true;
						this.beginChangeHotkeyTransaction();
					}
					if (InputEx.GetKeyDown(KeyCode.F))
					{
						this.isChangingBrushFalloff = true;
						this.beginChangeHotkeyTransaction();
					}
					if (InputEx.GetKeyDown(KeyCode.V))
					{
						this.isChangingBrushStrength = true;
						this.beginChangeHotkeyTransaction();
					}
				}
			}
			if (InputEx.GetKeyUp(KeyCode.B))
			{
				this.isChangingBrushRadius = false;
				this.endChangeHotkeyTransaction();
			}
			if (InputEx.GetKeyUp(KeyCode.F))
			{
				this.isChangingBrushFalloff = false;
				this.endChangeHotkeyTransaction();
			}
			if (InputEx.GetKeyUp(KeyCode.V))
			{
				this.isChangingBrushStrength = false;
				this.endChangeHotkeyTransaction();
			}
			if (this.isChangingBrush)
			{
				Plane plane = default(Plane);
				plane.SetNormalAndPosition(Vector3.up, this.brushWorldPosition);
				float d;
				plane.Raycast(ray, out d);
				this.changePlanePosition = ray.origin + ray.direction * d;
				if (this.isChangingBrushRadius)
				{
					this.brushRadius = (this.changePlanePosition - this.brushWorldPosition).magnitude;
				}
				if (this.isChangingBrushFalloff)
				{
					this.brushFalloff = Mathf.Clamp01((this.changePlanePosition - this.brushWorldPosition).magnitude / this.brushRadius);
				}
				if (this.isChangingBrushStrength)
				{
					this.brushStrength = (this.changePlanePosition - this.brushWorldPosition).magnitude / this.brushRadius;
				}
			}
			else
			{
				this.brushWorldPosition = this.pointerWorldPosition;
			}
			this.isBrushVisible = (this.isPointerOnWorld || this.isChangingBrush);
			if (!EditorInteract.isFlying && Glazier.Get().ShouldGameProcessInput)
			{
				if (this.mode == FoliageEditor.EFoliageMode.PAINT)
				{
					Bounds worldBounds = new Bounds(this.brushWorldPosition, new Vector3(this.brushRadius * 2f, 0f, this.brushRadius * 2f));
					float num = this.brushRadius * this.brushRadius;
					float num2 = num * this.brushFalloff * this.brushFalloff;
					float num3 = 3.1415927f * this.brushRadius * this.brushRadius;
					bool key = InputEx.GetKey(KeyCode.LeftControl);
					bool flag = key || InputEx.GetKey(KeyCode.LeftAlt);
					if (key || flag || InputEx.GetKey(KeyCode.LeftShift))
					{
						this.removeWeight += DevkitFoliageToolOptions.removeSensitivity * num3 * this.brushStrength * Time.deltaTime;
						int num4 = 0;
						if (this.removeWeight > 1f)
						{
							num4 = Mathf.FloorToInt(this.removeWeight);
							this.removeWeight -= (float)num4;
						}
						FoliageBounds foliageBounds = new FoliageBounds(worldBounds);
						for (int i = foliageBounds.min.x; i <= foliageBounds.max.x; i++)
						{
							for (int j = foliageBounds.min.y; j <= foliageBounds.max.y; j++)
							{
								FoliageTile tile = FoliageSystem.getTile(new FoliageCoord(i, j));
								if (tile != null)
								{
									if (key)
									{
										if (this.selectedInstanceAsset != null)
										{
											FoliageInstanceList list;
											if (tile.instances.TryGetValue(this.selectedInstanceAsset.getReferenceTo<FoliageInstancedMeshInfoAsset>(), ref list))
											{
												this.removeInstances(tile, list, num, num2, flag, ref num4);
												goto IL_46D;
											}
											goto IL_46D;
										}
										else
										{
											if (this.selectedCollectionAsset == null)
											{
												goto IL_46D;
											}
											using (List<FoliageInfoCollectionAsset.FoliageInfoCollectionElement>.Enumerator enumerator = this.selectedCollectionAsset.elements.GetEnumerator())
											{
												while (enumerator.MoveNext())
												{
													FoliageInfoCollectionAsset.FoliageInfoCollectionElement foliageInfoCollectionElement = enumerator.Current;
													FoliageInstancedMeshInfoAsset foliageInstancedMeshInfoAsset = Assets.find<FoliageInfoAsset>(foliageInfoCollectionElement.asset) as FoliageInstancedMeshInfoAsset;
													FoliageInstanceList list2;
													if (foliageInstancedMeshInfoAsset != null && tile.instances.TryGetValue(foliageInstancedMeshInfoAsset.getReferenceTo<FoliageInstancedMeshInfoAsset>(), ref list2))
													{
														this.removeInstances(tile, list2, num, num2, flag, ref num4);
													}
												}
												goto IL_46D;
											}
										}
									}
									foreach (KeyValuePair<AssetReference<FoliageInstancedMeshInfoAsset>, FoliageInstanceList> keyValuePair in tile.instances)
									{
										FoliageInstanceList value = keyValuePair.Value;
										this.removeInstances(tile, value, num, num2, flag, ref num4);
									}
								}
								IL_46D:;
							}
						}
						RegionBounds regionBounds = new RegionBounds(worldBounds);
						for (byte b = regionBounds.min.x; b <= regionBounds.max.x; b += 1)
						{
							for (byte b2 = regionBounds.min.y; b2 <= regionBounds.max.y; b2 += 1)
							{
								List<ResourceSpawnpoint> list3 = LevelGround.trees[(int)b, (int)b2];
								for (int k = list3.Count - 1; k >= 0; k--)
								{
									ResourceSpawnpoint resourceSpawnpoint = list3[k];
									if (!resourceSpawnpoint.isGenerated || flag)
									{
										if (key)
										{
											if (this.selectedInstanceAsset != null)
											{
												FoliageResourceInfoAsset foliageResourceInfoAsset = this.selectedInstanceAsset as FoliageResourceInfoAsset;
												if (foliageResourceInfoAsset == null)
												{
													goto IL_642;
												}
												if (!foliageResourceInfoAsset.resource.isReferenceTo(resourceSpawnpoint.asset))
												{
													goto IL_642;
												}
											}
											else if (this.selectedCollectionAsset != null)
											{
												bool flag2 = false;
												foreach (FoliageInfoCollectionAsset.FoliageInfoCollectionElement foliageInfoCollectionElement2 in this.selectedCollectionAsset.elements)
												{
													FoliageResourceInfoAsset foliageResourceInfoAsset2 = Assets.find<FoliageInfoAsset>(foliageInfoCollectionElement2.asset) as FoliageResourceInfoAsset;
													if (foliageResourceInfoAsset2 != null && foliageResourceInfoAsset2.resource.isReferenceTo(resourceSpawnpoint.asset))
													{
														flag2 = true;
														break;
													}
												}
												if (!flag2)
												{
													goto IL_642;
												}
											}
										}
										float sqrMagnitude = (resourceSpawnpoint.point - this.brushWorldPosition).sqrMagnitude;
										if (sqrMagnitude < num)
										{
											bool flag3 = sqrMagnitude < num2;
											this.previewSamples.Add(new FoliagePreviewSample(resourceSpawnpoint.point, flag3 ? Color.red : (Color.red / 2f)));
											if (InputEx.GetKey(KeyCode.Mouse0) && flag3 && num4 > 0)
											{
												resourceSpawnpoint.destroy();
												list3.RemoveAt(k);
												num4--;
											}
										}
									}
									IL_642:;
								}
								bool flag4 = false;
								List<LevelObject> list4 = LevelObjects.objects[(int)b, (int)b2];
								for (int l = list4.Count - 1; l >= 0; l--)
								{
									LevelObject levelObject = list4[l];
									if (levelObject.placementOrigin == ELevelObjectPlacementOrigin.PAINTED)
									{
										if (key)
										{
											if (this.selectedInstanceAsset != null)
											{
												FoliageObjectInfoAsset foliageObjectInfoAsset = this.selectedInstanceAsset as FoliageObjectInfoAsset;
												if (foliageObjectInfoAsset == null)
												{
													goto IL_7CD;
												}
												if (!foliageObjectInfoAsset.obj.isReferenceTo(levelObject.asset))
												{
													goto IL_7CD;
												}
											}
											else if (this.selectedCollectionAsset != null)
											{
												bool flag5 = false;
												foreach (FoliageInfoCollectionAsset.FoliageInfoCollectionElement foliageInfoCollectionElement3 in this.selectedCollectionAsset.elements)
												{
													FoliageObjectInfoAsset foliageObjectInfoAsset2 = Assets.find<FoliageInfoAsset>(foliageInfoCollectionElement3.asset) as FoliageObjectInfoAsset;
													if (foliageObjectInfoAsset2 != null && foliageObjectInfoAsset2.obj.isReferenceTo(levelObject.asset))
													{
														flag5 = true;
														break;
													}
												}
												if (!flag5)
												{
													goto IL_7CD;
												}
											}
										}
										float sqrMagnitude2 = (levelObject.transform.position - this.brushWorldPosition).sqrMagnitude;
										if (sqrMagnitude2 < num)
										{
											bool flag6 = sqrMagnitude2 < num2;
											this.previewSamples.Add(new FoliagePreviewSample(levelObject.transform.position, flag6 ? Color.red : (Color.red / 2f)));
											if (InputEx.GetKey(KeyCode.Mouse0) && flag6 && num4 > 0)
											{
												flag4 = true;
												LevelObjects.removeObject(levelObject.transform);
												num4--;
											}
										}
									}
									IL_7CD:;
								}
								if (flag4)
								{
									LevelHierarchy.MarkDirty();
								}
							}
						}
						return;
					}
					if (!InputEx.GetKey(KeyCode.Mouse0))
					{
						return;
					}
					if (this.selectedInstanceAsset != null)
					{
						this.addFoliage(this.selectedInstanceAsset, 1f);
						return;
					}
					if (this.selectedCollectionAsset == null)
					{
						return;
					}
					using (List<FoliageInfoCollectionAsset.FoliageInfoCollectionElement>.Enumerator enumerator = this.selectedCollectionAsset.elements.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							FoliageInfoCollectionAsset.FoliageInfoCollectionElement foliageInfoCollectionElement4 = enumerator.Current;
							this.addFoliage(Assets.find<FoliageInfoAsset>(foliageInfoCollectionElement4.asset), foliageInfoCollectionElement4.weight);
						}
						return;
					}
				}
				if (this.mode == FoliageEditor.EFoliageMode.EXACT && InputEx.GetKeyDown(KeyCode.Mouse0))
				{
					if (this.selectedInstanceAsset != null)
					{
						if (this.selectedInstanceAsset != null)
						{
							this.selectedInstanceAsset.addFoliageToSurface(raycastHit.point, raycastHit.normal, false, false);
							LevelHierarchy.MarkDirty();
							return;
						}
					}
					else if (this.selectedCollectionAsset != null)
					{
						FoliageInfoAsset foliageInfoAsset = Assets.find<FoliageInfoAsset>(this.selectedCollectionAsset.elements[Random.Range(0, this.selectedCollectionAsset.elements.Count)].asset);
						if (foliageInfoAsset != null)
						{
							foliageInfoAsset.addFoliageToSurface(raycastHit.point, raycastHit.normal, false, false);
							LevelHierarchy.MarkDirty();
						}
					}
				}
			}
		}

		// Token: 0x06001EB2 RID: 7858 RVA: 0x00072008 File Offset: 0x00070208
		public void equip()
		{
			GLRenderer.render += this.handleGLRender;
		}

		// Token: 0x06001EB3 RID: 7859 RVA: 0x0007201B File Offset: 0x0007021B
		public void dequip()
		{
			GLRenderer.render -= this.handleGLRender;
		}

		/// <summary>
		/// Get brush strength multiplier where strength decreases past falloff. Use this method so that different falloffs e.g. linear, curved can be added.
		/// </summary>
		/// <param name="distance">Percentage of <see cref="P:SDG.Unturned.FoliageEditor.brushRadius" />.</param>
		// Token: 0x06001EB4 RID: 7860 RVA: 0x0007202E File Offset: 0x0007022E
		private float getBrushAlpha(float distance)
		{
			if (distance < this.brushFalloff)
			{
				return 1f;
			}
			return (1f - distance) / (1f - this.brushFalloff);
		}

		// Token: 0x06001EB5 RID: 7861 RVA: 0x00072054 File Offset: 0x00070254
		private void handleGLRender()
		{
			if (this.isBrushVisible && Glazier.Get().ShouldGameProcessInput)
			{
				GLUtility.matrix = MathUtility.IDENTITY_MATRIX;
				if ((long)this.previewSamples.Count <= (long)((ulong)this.maxPreviewSamples))
				{
					GLUtility.LINE_FLAT_COLOR.SetPass(0);
					GL.Begin(4);
					float num = Mathf.Lerp(0.25f, 1f, this.brushRadius / 256f);
					Vector3 size = new Vector3(num, num, num);
					foreach (FoliagePreviewSample foliagePreviewSample in this.previewSamples)
					{
						GL.Color(foliagePreviewSample.color);
						GLUtility.boxSolid(foliagePreviewSample.position, size);
					}
					GL.End();
				}
				if (this.mode == FoliageEditor.EFoliageMode.PAINT)
				{
					GL.LoadOrtho();
					GLUtility.LINE_FLAT_COLOR.SetPass(0);
					GL.Begin(1);
					Color color;
					if (this.isChangingBrushStrength)
					{
						color = Color.Lerp(Color.red, Color.green, this.brushStrength);
					}
					else
					{
						color = Color.yellow;
					}
					Vector3 vector = MainCamera.instance.WorldToViewportPoint(this.brushWorldPosition);
					vector.z = 0f;
					Vector3 a = MainCamera.instance.WorldToViewportPoint(this.brushWorldPosition + MainCamera.instance.transform.right * this.brushRadius);
					a.z = 0f;
					Vector3 a2 = MainCamera.instance.WorldToViewportPoint(this.brushWorldPosition + MainCamera.instance.transform.up * this.brushRadius);
					a2.z = 0f;
					Vector3 a3 = MainCamera.instance.WorldToViewportPoint(this.brushWorldPosition + MainCamera.instance.transform.right * this.brushRadius * this.brushFalloff);
					a3.z = 0f;
					Vector3 a4 = MainCamera.instance.WorldToViewportPoint(this.brushWorldPosition + MainCamera.instance.transform.up * this.brushRadius * this.brushFalloff);
					a4.z = 0f;
					GL.Color(color / 2f);
					GLUtility.circle(vector, 1f, a - vector, a2 - vector, 64f);
					GL.Color(color);
					GLUtility.circle(vector, 1f, a3 - vector, a4 - vector, 64f);
					GL.End();
					return;
				}
				if (this.mode == FoliageEditor.EFoliageMode.EXACT)
				{
					GLUtility.matrix = Matrix4x4.TRS(this.brushWorldPosition, MathUtility.IDENTITY_QUATERNION, new Vector3(1f, 1f, 1f));
					GLUtility.LINE_FLAT_COLOR.SetPass(0);
					GL.Begin(1);
					GL.Color(Color.yellow);
					GLUtility.line(new Vector3(-1f, 0f, 0f), new Vector3(1f, 0f, 0f));
					GLUtility.line(new Vector3(0f, -1f, 0f), new Vector3(0f, 1f, 0f));
					GLUtility.line(new Vector3(0f, 0f, -1f), new Vector3(0f, 0f, 1f));
					GL.End();
				}
			}
		}

		// Token: 0x04000F12 RID: 3858
		public FoliageInfoCollectionAsset selectedCollectionAsset;

		// Token: 0x04000F13 RID: 3859
		public FoliageInfoAsset selectedInstanceAsset;

		// Token: 0x04000F14 RID: 3860
		public FoliageEditor.EFoliageMode mode = FoliageEditor.EFoliageMode.BAKE;

		// Token: 0x04000F15 RID: 3861
		private Vector3 pointerWorldPosition;

		// Token: 0x04000F16 RID: 3862
		private Vector3 brushWorldPosition;

		// Token: 0x04000F17 RID: 3863
		private Vector3 changePlanePosition;

		// Token: 0x04000F18 RID: 3864
		private bool isPointerOnWorld;

		// Token: 0x04000F19 RID: 3865
		private bool isBrushVisible;

		// Token: 0x04000F1A RID: 3866
		private Dictionary<FoliageInfoAsset, float> addWeights = new Dictionary<FoliageInfoAsset, float>();

		// Token: 0x04000F1B RID: 3867
		private float removeWeight;

		// Token: 0x04000F1C RID: 3868
		private List<FoliagePreviewSample> previewSamples = new List<FoliagePreviewSample>();

		// Token: 0x04000F1D RID: 3869
		private bool isChangingBrushRadius;

		// Token: 0x04000F1E RID: 3870
		private bool isChangingBrushFalloff;

		// Token: 0x04000F1F RID: 3871
		private bool isChangingBrushStrength;

		// Token: 0x02000935 RID: 2357
		public enum EFoliageMode
		{
			// Token: 0x040032B1 RID: 12977
			PAINT,
			// Token: 0x040032B2 RID: 12978
			EXACT,
			/// <summary>
			/// This is a bit of a hack in order to simplify the foliage menu when most of the time editors are either
			/// manually placing foliage or automatically baking it.
			/// </summary>
			// Token: 0x040032B3 RID: 12979
			BAKE
		}
	}
}
