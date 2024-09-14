using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200078D RID: 1933
	public class MenuConfigurationGraphicsUI
	{
		// Token: 0x06003FED RID: 16365 RVA: 0x00145212 File Offset: 0x00143412
		public static void open()
		{
			if (MenuConfigurationGraphicsUI.active)
			{
				return;
			}
			MenuConfigurationGraphicsUI.active = true;
			MenuConfigurationGraphicsUI.container.AnimateIntoView();
		}

		// Token: 0x06003FEE RID: 16366 RVA: 0x0014522C File Offset: 0x0014342C
		public static void close()
		{
			if (!MenuConfigurationGraphicsUI.active)
			{
				return;
			}
			MenuConfigurationGraphicsUI.active = false;
			MenuSettings.SaveGraphicsIfLoaded();
			MenuConfigurationGraphicsUI.container.AnimateOutOfView(0f, 1f);
		}

		// Token: 0x06003FEF RID: 16367 RVA: 0x00145255 File Offset: 0x00143455
		private static void onToggledAmbientOcclusion(ISleekToggle toggle, bool state)
		{
			GraphicsSettings.isAmbientOcclusionEnabled = state;
			GraphicsSettings.apply("changed ambient occlusion");
		}

		// Token: 0x06003FF0 RID: 16368 RVA: 0x00145267 File Offset: 0x00143467
		private static void onToggledBloomToggle(ISleekToggle toggle, bool state)
		{
			GraphicsSettings.bloom = state;
			GraphicsSettings.apply("changed bloom");
		}

		// Token: 0x06003FF1 RID: 16369 RVA: 0x00145279 File Offset: 0x00143479
		private static void onToggledChromaticAberrationToggle(ISleekToggle toggle, bool state)
		{
			GraphicsSettings.chromaticAberration = state;
			GraphicsSettings.apply("changed chromatic aberration");
		}

		// Token: 0x06003FF2 RID: 16370 RVA: 0x0014528B File Offset: 0x0014348B
		private static void onToggledFilmGrainToggle(ISleekToggle toggle, bool state)
		{
			GraphicsSettings.filmGrain = state;
			GraphicsSettings.apply("changed film grain");
		}

		// Token: 0x06003FF3 RID: 16371 RVA: 0x0014529D File Offset: 0x0014349D
		private static void onToggledBlendToggle(ISleekToggle toggle, bool state)
		{
			GraphicsSettings.blend = state;
			GraphicsSettings.apply("changed blend");
		}

		// Token: 0x06003FF4 RID: 16372 RVA: 0x001452AF File Offset: 0x001434AF
		private static void onToggledGrassDisplacementToggle(ISleekToggle toggle, bool state)
		{
			GraphicsSettings.grassDisplacement = state;
			GraphicsSettings.apply("changed grass displacement");
		}

		// Token: 0x06003FF5 RID: 16373 RVA: 0x001452C1 File Offset: 0x001434C1
		private static void onToggledFoliageFocusToggle(ISleekToggle toggle, bool state)
		{
			GraphicsSettings.foliageFocus = state;
			GraphicsSettings.apply("changed foliage focus");
		}

		// Token: 0x06003FF6 RID: 16374 RVA: 0x001452D3 File Offset: 0x001434D3
		private static void onSwappedLandmarkState(SleekButtonState button, int index)
		{
			GraphicsSettings.landmarkQuality = (EGraphicQuality)index;
			GraphicsSettings.apply("changed landmark quality");
			MenuConfigurationGraphicsUI.updatePerfWarnings();
		}

		// Token: 0x06003FF7 RID: 16375 RVA: 0x001452EA File Offset: 0x001434EA
		private static void onToggledRagdollsToggle(ISleekToggle toggle, bool state)
		{
			GraphicsSettings.ragdolls = state;
			GraphicsSettings.apply("changed ragdolls");
		}

		// Token: 0x06003FF8 RID: 16376 RVA: 0x001452FC File Offset: 0x001434FC
		private static void onToggledDebrisToggle(ISleekToggle toggle, bool state)
		{
			GraphicsSettings.debris = state;
			GraphicsSettings.apply("changed debris");
		}

		// Token: 0x06003FF9 RID: 16377 RVA: 0x0014530E File Offset: 0x0014350E
		private static void onToggledBlastToggle(ISleekToggle toggle, bool state)
		{
			GraphicsSettings.blast = state;
			GraphicsSettings.apply("changed blastmarks");
		}

		// Token: 0x06003FFA RID: 16378 RVA: 0x00145320 File Offset: 0x00143520
		private static void onToggledPuddleToggle(ISleekToggle toggle, bool state)
		{
			GraphicsSettings.puddle = state;
			GraphicsSettings.apply("changed puddles");
		}

		// Token: 0x06003FFB RID: 16379 RVA: 0x00145332 File Offset: 0x00143532
		private static void onToggledGlitterToggle(ISleekToggle toggle, bool state)
		{
			GraphicsSettings.glitter = state;
			GraphicsSettings.apply("changed glitter");
		}

		// Token: 0x06003FFC RID: 16380 RVA: 0x00145344 File Offset: 0x00143544
		private static void onToggledTriplanarToggle(ISleekToggle toggle, bool state)
		{
			GraphicsSettings.triplanar = state;
			GraphicsSettings.apply("changed triplanar");
		}

		// Token: 0x06003FFD RID: 16381 RVA: 0x00145356 File Offset: 0x00143556
		private static void onToggledSkyboxReflectionToggle(ISleekToggle toggle, bool state)
		{
			GraphicsSettings.skyboxReflection = state;
			GraphicsSettings.apply("changed skybox reflection");
		}

		// Token: 0x06003FFE RID: 16382 RVA: 0x00145368 File Offset: 0x00143568
		private static void onToggledItemIconAntiAliasingToggle(ISleekToggle toggle, bool state)
		{
			GraphicsSettings.IsItemIconAntiAliasingEnabled = state;
			GraphicsSettings.apply("changed item icon anti-aliasing");
		}

		// Token: 0x06003FFF RID: 16383 RVA: 0x0014537A File Offset: 0x0014357A
		private static void OnDraggedFarClipDistanceSlider(ISleekSlider slider, float state)
		{
			GraphicsSettings.NormalizedFarClipDistance = state;
			GraphicsSettings.apply("changed far clip distance");
			MenuConfigurationGraphicsUI.farClipDistanceSlider.UpdateLabel(MenuConfigurationGraphicsUI.localization.format("Far_Clip_Slider_Label", 50 + Mathf.RoundToInt(state * 150f)));
		}

		// Token: 0x06004000 RID: 16384 RVA: 0x001453B9 File Offset: 0x001435B9
		private static void onDraggedDistanceSlider(ISleekSlider slider, float state)
		{
			GraphicsSettings.normalizedDrawDistance = state;
			GraphicsSettings.apply("changed draw distance");
			MenuConfigurationGraphicsUI.distanceSlider.UpdateLabel(MenuConfigurationGraphicsUI.localization.format("Distance_Slider_Label", 25 + (int)(state * 75f)));
		}

		// Token: 0x06004001 RID: 16385 RVA: 0x001453F4 File Offset: 0x001435F4
		private static void onDraggedLandmarkSlider(ISleekSlider slider, float state)
		{
			GraphicsSettings.normalizedLandmarkDrawDistance = state;
			GraphicsSettings.apply("changed landmark draw distance");
			MenuConfigurationGraphicsUI.landmarkSlider.UpdateLabel(MenuConfigurationGraphicsUI.localization.format("Landmark_Slider_Label", (int)(state * 100f)));
		}

		// Token: 0x06004002 RID: 16386 RVA: 0x0014542C File Offset: 0x0014362C
		private static void onSwappedAntiAliasingState(SleekButtonState button, int index)
		{
			GraphicsSettings.antiAliasingType = (EAntiAliasingType)index;
			GraphicsSettings.apply("changed anti-aliasing type");
		}

		// Token: 0x06004003 RID: 16387 RVA: 0x0014543E File Offset: 0x0014363E
		private static void onSwappedAnisotropicFilteringState(SleekButtonState button, int index)
		{
			GraphicsSettings.anisotropicFilteringMode = (EAnisotropicFilteringMode)index;
			GraphicsSettings.apply("changed anisotropic filtering mode");
		}

		// Token: 0x06004004 RID: 16388 RVA: 0x00145450 File Offset: 0x00143650
		private static void onSwappedEffectState(SleekButtonState button, int index)
		{
			GraphicsSettings.effectQuality = index + EGraphicQuality.LOW;
			GraphicsSettings.apply("changed effect quality");
		}

		// Token: 0x06004005 RID: 16389 RVA: 0x00145464 File Offset: 0x00143664
		private static void onSwappedFoliageState(SleekButtonState button, int index)
		{
			GraphicsSettings.foliageQuality = index + EGraphicQuality.LOW;
			GraphicsSettings.apply("changed foliage quality");
			MenuConfigurationGraphicsUI.updatePerfWarnings();
		}

		// Token: 0x06004006 RID: 16390 RVA: 0x0014547D File Offset: 0x0014367D
		private static void onSwappedSunShaftsState(SleekButtonState button, int index)
		{
			GraphicsSettings.sunShaftsQuality = (EGraphicQuality)index;
			GraphicsSettings.apply("changed sun shafts quality");
		}

		// Token: 0x06004007 RID: 16391 RVA: 0x0014548F File Offset: 0x0014368F
		private static void onSwappedLightingState(SleekButtonState button, int index)
		{
			GraphicsSettings.lightingQuality = (EGraphicQuality)index;
			GraphicsSettings.apply("changed lighting quality");
		}

		// Token: 0x06004008 RID: 16392 RVA: 0x001454A1 File Offset: 0x001436A1
		private static void onSwappedReflectionState(SleekButtonState button, int index)
		{
			GraphicsSettings.reflectionQuality = (EGraphicQuality)index;
			GraphicsSettings.apply("changed reflection quality");
		}

		// Token: 0x06004009 RID: 16393 RVA: 0x001454B3 File Offset: 0x001436B3
		private static void onSwappedPlanarReflectionState(SleekButtonState button, int index)
		{
			GraphicsSettings.planarReflectionQuality = index + EGraphicQuality.LOW;
			GraphicsSettings.apply("changed planar reflection quality");
			MenuConfigurationGraphicsUI.updatePerfWarnings();
		}

		// Token: 0x0600400A RID: 16394 RVA: 0x001454CC File Offset: 0x001436CC
		private static void onSwappedWaterState(SleekButtonState button, int index)
		{
			GraphicsSettings.waterQuality = index + EGraphicQuality.LOW;
			GraphicsSettings.apply("changed water quality");
			MenuConfigurationGraphicsUI.updatePerfWarnings();
		}

		// Token: 0x0600400B RID: 16395 RVA: 0x001454E5 File Offset: 0x001436E5
		private static void onSwappedScopeState(SleekButtonState button, int index)
		{
			GraphicsSettings.scopeQuality = (EGraphicQuality)index;
			GraphicsSettings.apply("changed scope quality");
			MenuConfigurationGraphicsUI.updatePerfWarnings();
		}

		// Token: 0x0600400C RID: 16396 RVA: 0x001454FC File Offset: 0x001436FC
		private static void onSwappedOutlineState(SleekButtonState button, int index)
		{
			GraphicsSettings.outlineQuality = index + EGraphicQuality.LOW;
			GraphicsSettings.apply("changed outline quality");
		}

		// Token: 0x0600400D RID: 16397 RVA: 0x00145510 File Offset: 0x00143710
		private static void onSwappedTerrainState(SleekButtonState button, int index)
		{
			GraphicsSettings.terrainQuality = index + EGraphicQuality.LOW;
			GraphicsSettings.apply("changed terrain quality");
		}

		// Token: 0x0600400E RID: 16398 RVA: 0x00145524 File Offset: 0x00143724
		private static void onSwappedWindState(SleekButtonState button, int index)
		{
			GraphicsSettings.windQuality = (EGraphicQuality)index;
			GraphicsSettings.apply("changed wind quality");
		}

		// Token: 0x0600400F RID: 16399 RVA: 0x00145536 File Offset: 0x00143736
		private static void onSwappedTreeModeState(SleekButtonState button, int index)
		{
			GraphicsSettings.treeMode = (ETreeGraphicMode)index;
			MenuConfigurationGraphicsUI.updatePerfWarnings();
		}

		// Token: 0x06004010 RID: 16400 RVA: 0x00145543 File Offset: 0x00143743
		private static void onSwappedRenderState(SleekButtonState button, int index)
		{
			GraphicsSettings.renderMode = (ERenderMode)index;
			GraphicsSettings.apply("changed render mode");
			MenuConfigurationGraphicsUI.updatePerfWarnings();
		}

		// Token: 0x06004011 RID: 16401 RVA: 0x0014555A File Offset: 0x0014375A
		private static void onClickedBackButton(ISleekElement button)
		{
			if (Player.player != null)
			{
				PlayerPauseUI.open();
			}
			else if (Level.isEditor)
			{
				EditorPauseUI.open();
			}
			else
			{
				MenuConfigurationUI.open();
			}
			MenuConfigurationGraphicsUI.close();
		}

		// Token: 0x06004012 RID: 16402 RVA: 0x00145588 File Offset: 0x00143788
		private static void onClickedDefaultButton(ISleekElement button)
		{
			GraphicsSettings.restoreDefaults();
			MenuConfigurationGraphicsUI.updateAll();
		}

		// Token: 0x06004013 RID: 16403 RVA: 0x00145594 File Offset: 0x00143794
		private static void updateAll()
		{
			MenuConfigurationGraphicsUI.ambientOcclusionToggle.Value = GraphicsSettings.isAmbientOcclusionEnabled;
			MenuConfigurationGraphicsUI.bloomToggle.Value = GraphicsSettings.bloom;
			MenuConfigurationGraphicsUI.chromaticAberrationToggle.Value = GraphicsSettings.chromaticAberration;
			MenuConfigurationGraphicsUI.filmGrainToggle.Value = GraphicsSettings.filmGrain;
			MenuConfigurationGraphicsUI.grassDisplacementToggle.Value = GraphicsSettings.grassDisplacement;
			MenuConfigurationGraphicsUI.foliageFocusToggle.Value = GraphicsSettings.foliageFocus;
			MenuConfigurationGraphicsUI.landmarkButton.state = (int)GraphicsSettings.landmarkQuality;
			MenuConfigurationGraphicsUI.ragdollsToggle.Value = GraphicsSettings.ragdolls;
			MenuConfigurationGraphicsUI.debrisToggle.Value = GraphicsSettings.debris;
			MenuConfigurationGraphicsUI.blastToggle.Value = GraphicsSettings.blast;
			MenuConfigurationGraphicsUI.puddleToggle.Value = GraphicsSettings.puddle;
			MenuConfigurationGraphicsUI.glitterToggle.Value = GraphicsSettings.glitter;
			MenuConfigurationGraphicsUI.triplanarToggle.Value = GraphicsSettings.triplanar;
			MenuConfigurationGraphicsUI.skyboxReflectionToggle.Value = GraphicsSettings.skyboxReflection;
			MenuConfigurationGraphicsUI.itemIconAntiAliasingToggle.Value = GraphicsSettings.IsItemIconAntiAliasingEnabled;
			MenuConfigurationGraphicsUI.farClipDistanceSlider.Value = GraphicsSettings.NormalizedFarClipDistance;
			MenuConfigurationGraphicsUI.farClipDistanceSlider.UpdateLabel(MenuConfigurationGraphicsUI.localization.format("Far_Clip_Slider_Label", 50 + Mathf.RoundToInt(GraphicsSettings.NormalizedFarClipDistance * 150f)));
			MenuConfigurationGraphicsUI.distanceSlider.Value = GraphicsSettings.normalizedDrawDistance;
			MenuConfigurationGraphicsUI.distanceSlider.UpdateLabel(MenuConfigurationGraphicsUI.localization.format("Distance_Slider_Label", 25 + (int)(GraphicsSettings.normalizedDrawDistance * 75f)));
			MenuConfigurationGraphicsUI.landmarkSlider.Value = GraphicsSettings.normalizedLandmarkDrawDistance;
			MenuConfigurationGraphicsUI.landmarkSlider.UpdateLabel(MenuConfigurationGraphicsUI.localization.format("Landmark_Slider_Label", (int)(GraphicsSettings.normalizedLandmarkDrawDistance * 100f)));
			MenuConfigurationGraphicsUI.antiAliasingButton.state = (int)GraphicsSettings.antiAliasingType;
			MenuConfigurationGraphicsUI.anisotropicFilteringButton.state = (int)GraphicsSettings.anisotropicFilteringMode;
			MenuConfigurationGraphicsUI.effectButton.state = GraphicsSettings.effectQuality - EGraphicQuality.LOW;
			MenuConfigurationGraphicsUI.foliageButton.state = GraphicsSettings.foliageQuality - EGraphicQuality.LOW;
			MenuConfigurationGraphicsUI.sunShaftsButton.state = (int)GraphicsSettings.sunShaftsQuality;
			MenuConfigurationGraphicsUI.lightingButton.state = (int)GraphicsSettings.lightingQuality;
			MenuConfigurationGraphicsUI.reflectionButton.state = (int)GraphicsSettings.reflectionQuality;
			MenuConfigurationGraphicsUI.planarReflectionButton.state = GraphicsSettings.planarReflectionQuality - EGraphicQuality.LOW;
			MenuConfigurationGraphicsUI.waterButton.state = GraphicsSettings.waterQuality - EGraphicQuality.LOW;
			MenuConfigurationGraphicsUI.scopeButton.state = (int)GraphicsSettings.scopeQuality;
			MenuConfigurationGraphicsUI.outlineButton.state = GraphicsSettings.outlineQuality - EGraphicQuality.LOW;
			MenuConfigurationGraphicsUI.terrainButton.state = GraphicsSettings.terrainQuality - EGraphicQuality.LOW;
			MenuConfigurationGraphicsUI.windButton.state = (int)GraphicsSettings.windQuality;
			MenuConfigurationGraphicsUI.treeModeButton.state = (int)GraphicsSettings.treeMode;
			MenuConfigurationGraphicsUI.renderButton.state = (int)GraphicsSettings.renderMode;
			MenuConfigurationGraphicsUI.updatePerfWarnings();
		}

		// Token: 0x06004014 RID: 16404 RVA: 0x0014582C File Offset: 0x00143A2C
		private static void updatePerfWarnings()
		{
			MenuConfigurationGraphicsUI.landmarkSlider.IsInteractable = (GraphicsSettings.landmarkQuality > EGraphicQuality.OFF);
			MenuConfigurationGraphicsUI.foliagePerf.IsVisible = !SystemInfo.supportsInstancing;
			MenuConfigurationGraphicsUI.grassDisplacementToggle.IsInteractable = (GraphicsSettings.foliageQuality > EGraphicQuality.OFF);
			MenuConfigurationGraphicsUI.foliageFocusToggle.IsInteractable = (GraphicsSettings.foliageQuality > EGraphicQuality.OFF);
			MenuConfigurationGraphicsUI.waterPerf.IsVisible = (GraphicsSettings.waterQuality == EGraphicQuality.ULTRA);
			MenuConfigurationGraphicsUI.planarReflectionButton.isInteractable = (GraphicsSettings.waterQuality == EGraphicQuality.ULTRA);
			MenuConfigurationGraphicsUI.scopePerf.IsVisible = (GraphicsSettings.scopeQuality > EGraphicQuality.OFF);
			MenuConfigurationGraphicsUI.treePerf.IsVisible = (GraphicsSettings.treeMode > ETreeGraphicMode.LEGACY);
			MenuConfigurationGraphicsUI.reflectionButton.isInteractable = (GraphicsSettings.renderMode == ERenderMode.DEFERRED);
			MenuConfigurationGraphicsUI.blastToggle.IsInteractable = (GraphicsSettings.renderMode == ERenderMode.DEFERRED);
		}

		// Token: 0x06004015 RID: 16405 RVA: 0x001458F0 File Offset: 0x00143AF0
		public MenuConfigurationGraphicsUI()
		{
			MenuConfigurationGraphicsUI.localization = Localization.read("/Menu/Configuration/MenuConfigurationGraphics.dat");
			Bundle bundle = Bundles.getBundle("/Bundles/Textures/Menu/Icons/Configuration/MenuConfigurationGraphics/MenuConfigurationGraphics.unity3d");
			MenuConfigurationGraphicsUI.container = new SleekFullscreenBox();
			MenuConfigurationGraphicsUI.container.PositionOffset_X = 10f;
			MenuConfigurationGraphicsUI.container.PositionOffset_Y = 10f;
			MenuConfigurationGraphicsUI.container.PositionScale_Y = 1f;
			MenuConfigurationGraphicsUI.container.SizeOffset_X = -20f;
			MenuConfigurationGraphicsUI.container.SizeOffset_Y = -20f;
			MenuConfigurationGraphicsUI.container.SizeScale_X = 1f;
			MenuConfigurationGraphicsUI.container.SizeScale_Y = 1f;
			if (Provider.isConnected)
			{
				PlayerUI.container.AddChild(MenuConfigurationGraphicsUI.container);
			}
			else if (Level.isEditor)
			{
				EditorUI.window.AddChild(MenuConfigurationGraphicsUI.container);
			}
			else
			{
				MenuUI.container.AddChild(MenuConfigurationGraphicsUI.container);
			}
			Color32 color = new Color32(240, 240, 240, byte.MaxValue);
			Color32 color2 = new Color32(180, 180, 180, byte.MaxValue);
			MenuConfigurationGraphicsUI.active = false;
			MenuConfigurationGraphicsUI.graphicsBox = Glazier.Get().CreateScrollView();
			MenuConfigurationGraphicsUI.graphicsBox.PositionOffset_X = -425f;
			MenuConfigurationGraphicsUI.graphicsBox.PositionOffset_Y = 100f;
			MenuConfigurationGraphicsUI.graphicsBox.PositionScale_X = 0.5f;
			MenuConfigurationGraphicsUI.graphicsBox.SizeOffset_X = 680f;
			MenuConfigurationGraphicsUI.graphicsBox.SizeOffset_Y = -200f;
			MenuConfigurationGraphicsUI.graphicsBox.SizeScale_Y = 1f;
			MenuConfigurationGraphicsUI.graphicsBox.ScaleContentToWidth = true;
			MenuConfigurationGraphicsUI.container.AddChild(MenuConfigurationGraphicsUI.graphicsBox);
			int num = 0;
			MenuConfigurationGraphicsUI.farClipDistanceSlider = Glazier.Get().CreateSlider();
			MenuConfigurationGraphicsUI.farClipDistanceSlider.PositionOffset_X = 205f;
			MenuConfigurationGraphicsUI.farClipDistanceSlider.PositionOffset_Y = (float)num;
			MenuConfigurationGraphicsUI.farClipDistanceSlider.SizeOffset_X = 200f;
			MenuConfigurationGraphicsUI.farClipDistanceSlider.SizeOffset_Y = 20f;
			MenuConfigurationGraphicsUI.farClipDistanceSlider.Orientation = 0;
			MenuConfigurationGraphicsUI.farClipDistanceSlider.AddLabel(MenuConfigurationGraphicsUI.localization.format("Far_Clip_Slider_Label", 50 + Mathf.RoundToInt(GraphicsSettings.NormalizedFarClipDistance * 150f)), 1);
			MenuConfigurationGraphicsUI.farClipDistanceSlider.OnValueChanged += new Dragged(MenuConfigurationGraphicsUI.OnDraggedFarClipDistanceSlider);
			MenuConfigurationGraphicsUI.graphicsBox.AddChild(MenuConfigurationGraphicsUI.farClipDistanceSlider);
			num += 30;
			MenuConfigurationGraphicsUI.farClipDistanceSlider.SideLabel.SizeOffset_X += 100f;
			MenuConfigurationGraphicsUI.distanceSlider = Glazier.Get().CreateSlider();
			MenuConfigurationGraphicsUI.distanceSlider.PositionOffset_X = 205f;
			MenuConfigurationGraphicsUI.distanceSlider.PositionOffset_Y = (float)num;
			MenuConfigurationGraphicsUI.distanceSlider.SizeOffset_X = 200f;
			MenuConfigurationGraphicsUI.distanceSlider.SizeOffset_Y = 20f;
			MenuConfigurationGraphicsUI.distanceSlider.Orientation = 0;
			MenuConfigurationGraphicsUI.distanceSlider.AddLabel(MenuConfigurationGraphicsUI.localization.format("Distance_Slider_Label", 25 + (int)(GraphicsSettings.normalizedDrawDistance * 75f)), 1);
			MenuConfigurationGraphicsUI.distanceSlider.OnValueChanged += new Dragged(MenuConfigurationGraphicsUI.onDraggedDistanceSlider);
			MenuConfigurationGraphicsUI.graphicsBox.AddChild(MenuConfigurationGraphicsUI.distanceSlider);
			num += 30;
			MenuConfigurationGraphicsUI.distanceSlider.SideLabel.SizeOffset_X += 100f;
			MenuConfigurationGraphicsUI.landmarkButton = new SleekButtonState(new GUIContent[]
			{
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("Off")),
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("Low")),
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("Medium")),
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("High")),
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("Ultra"))
			});
			MenuConfigurationGraphicsUI.landmarkButton.PositionOffset_X = 205f;
			MenuConfigurationGraphicsUI.landmarkButton.PositionOffset_Y = (float)num;
			MenuConfigurationGraphicsUI.landmarkButton.SizeOffset_X = 200f;
			MenuConfigurationGraphicsUI.landmarkButton.SizeOffset_Y = 30f;
			MenuConfigurationGraphicsUI.landmarkButton.AddLabel(MenuConfigurationGraphicsUI.localization.format("Landmark_Button_Label"), 1);
			MenuConfigurationGraphicsUI.landmarkButton.tooltip = RichTextUtil.wrapWithColor(MenuConfigurationGraphicsUI.localization.format("Landmark_Button_Tooltip"), color) + RichTextUtil.wrapWithColor(string.Concat(new string[]
			{
				"\n",
				MenuConfigurationGraphicsUI.localization.format("Landmark_Low", MenuConfigurationGraphicsUI.localization.format("Low")),
				"\n",
				MenuConfigurationGraphicsUI.localization.format("Landmark_Medium", MenuConfigurationGraphicsUI.localization.format("Medium")),
				"\n",
				MenuConfigurationGraphicsUI.localization.format("Landmark_High", MenuConfigurationGraphicsUI.localization.format("High")),
				"\n",
				MenuConfigurationGraphicsUI.localization.format("Landmark_Ultra", MenuConfigurationGraphicsUI.localization.format("Ultra"))
			}), color2);
			MenuConfigurationGraphicsUI.landmarkButton.onSwappedState = new SwappedState(MenuConfigurationGraphicsUI.onSwappedLandmarkState);
			MenuConfigurationGraphicsUI.graphicsBox.AddChild(MenuConfigurationGraphicsUI.landmarkButton);
			num += 40;
			MenuConfigurationGraphicsUI.landmarkSlider = Glazier.Get().CreateSlider();
			MenuConfigurationGraphicsUI.landmarkSlider.PositionOffset_X = 205f;
			MenuConfigurationGraphicsUI.landmarkSlider.PositionOffset_Y = (float)num;
			MenuConfigurationGraphicsUI.landmarkSlider.SizeOffset_X = 200f;
			MenuConfigurationGraphicsUI.landmarkSlider.SizeOffset_Y = 20f;
			MenuConfigurationGraphicsUI.landmarkSlider.Orientation = 0;
			MenuConfigurationGraphicsUI.landmarkSlider.AddLabel(MenuConfigurationGraphicsUI.localization.format("Landmark_Slider_Label", 25 + (int)(GraphicsSettings.normalizedLandmarkDrawDistance * 75f)), 1);
			MenuConfigurationGraphicsUI.landmarkSlider.OnValueChanged += new Dragged(MenuConfigurationGraphicsUI.onDraggedLandmarkSlider);
			MenuConfigurationGraphicsUI.graphicsBox.AddChild(MenuConfigurationGraphicsUI.landmarkSlider);
			num += 30;
			MenuConfigurationGraphicsUI.landmarkSlider.SideLabel.SizeOffset_X += 100f;
			MenuConfigurationGraphicsUI.ragdollsToggle = Glazier.Get().CreateToggle();
			MenuConfigurationGraphicsUI.ragdollsToggle.PositionOffset_X = 205f;
			MenuConfigurationGraphicsUI.ragdollsToggle.PositionOffset_Y = (float)num;
			MenuConfigurationGraphicsUI.ragdollsToggle.SizeOffset_X = 40f;
			MenuConfigurationGraphicsUI.ragdollsToggle.SizeOffset_Y = 40f;
			MenuConfigurationGraphicsUI.ragdollsToggle.AddLabel(MenuConfigurationGraphicsUI.localization.format("Ragdolls_Toggle_Label"), 1);
			MenuConfigurationGraphicsUI.ragdollsToggle.TooltipText = MenuConfigurationGraphicsUI.localization.format("Ragdolls_Tooltip");
			MenuConfigurationGraphicsUI.ragdollsToggle.OnValueChanged += new Toggled(MenuConfigurationGraphicsUI.onToggledRagdollsToggle);
			MenuConfigurationGraphicsUI.graphicsBox.AddChild(MenuConfigurationGraphicsUI.ragdollsToggle);
			num += 50;
			MenuConfigurationGraphicsUI.debrisToggle = Glazier.Get().CreateToggle();
			MenuConfigurationGraphicsUI.debrisToggle.PositionOffset_X = 205f;
			MenuConfigurationGraphicsUI.debrisToggle.PositionOffset_Y = (float)num;
			MenuConfigurationGraphicsUI.debrisToggle.SizeOffset_X = 40f;
			MenuConfigurationGraphicsUI.debrisToggle.SizeOffset_Y = 40f;
			MenuConfigurationGraphicsUI.debrisToggle.AddLabel(MenuConfigurationGraphicsUI.localization.format("Debris_Toggle_Label"), 1);
			MenuConfigurationGraphicsUI.debrisToggle.TooltipText = MenuConfigurationGraphicsUI.localization.format("Debris_Tooltip");
			MenuConfigurationGraphicsUI.debrisToggle.OnValueChanged += new Toggled(MenuConfigurationGraphicsUI.onToggledDebrisToggle);
			MenuConfigurationGraphicsUI.graphicsBox.AddChild(MenuConfigurationGraphicsUI.debrisToggle);
			num += 50;
			MenuConfigurationGraphicsUI.ambientOcclusionToggle = Glazier.Get().CreateToggle();
			MenuConfigurationGraphicsUI.ambientOcclusionToggle.PositionOffset_X = 205f;
			MenuConfigurationGraphicsUI.ambientOcclusionToggle.PositionOffset_Y = (float)num;
			MenuConfigurationGraphicsUI.ambientOcclusionToggle.SizeOffset_X = 40f;
			MenuConfigurationGraphicsUI.ambientOcclusionToggle.SizeOffset_Y = 40f;
			MenuConfigurationGraphicsUI.ambientOcclusionToggle.AddLabel(MenuConfigurationGraphicsUI.localization.format("Ambient_Occlusion_Label"), 1);
			MenuConfigurationGraphicsUI.ambientOcclusionToggle.TooltipText = MenuConfigurationGraphicsUI.localization.format("Ambient_Occlusion_Tooltip");
			MenuConfigurationGraphicsUI.ambientOcclusionToggle.OnValueChanged += new Toggled(MenuConfigurationGraphicsUI.onToggledAmbientOcclusion);
			MenuConfigurationGraphicsUI.graphicsBox.AddChild(MenuConfigurationGraphicsUI.ambientOcclusionToggle);
			num += 50;
			MenuConfigurationGraphicsUI.bloomToggle = Glazier.Get().CreateToggle();
			MenuConfigurationGraphicsUI.bloomToggle.PositionOffset_X = 205f;
			MenuConfigurationGraphicsUI.bloomToggle.PositionOffset_Y = (float)num;
			MenuConfigurationGraphicsUI.bloomToggle.SizeOffset_X = 40f;
			MenuConfigurationGraphicsUI.bloomToggle.SizeOffset_Y = 40f;
			MenuConfigurationGraphicsUI.bloomToggle.AddLabel(MenuConfigurationGraphicsUI.localization.format("Bloom_Toggle_Label"), 1);
			MenuConfigurationGraphicsUI.bloomToggle.TooltipText = MenuConfigurationGraphicsUI.localization.format("Bloom_Tooltip");
			MenuConfigurationGraphicsUI.bloomToggle.OnValueChanged += new Toggled(MenuConfigurationGraphicsUI.onToggledBloomToggle);
			MenuConfigurationGraphicsUI.graphicsBox.AddChild(MenuConfigurationGraphicsUI.bloomToggle);
			num += 50;
			MenuConfigurationGraphicsUI.filmGrainToggle = Glazier.Get().CreateToggle();
			MenuConfigurationGraphicsUI.filmGrainToggle.PositionOffset_X = 205f;
			MenuConfigurationGraphicsUI.filmGrainToggle.PositionOffset_Y = (float)num;
			MenuConfigurationGraphicsUI.filmGrainToggle.SizeOffset_X = 40f;
			MenuConfigurationGraphicsUI.filmGrainToggle.SizeOffset_Y = 40f;
			MenuConfigurationGraphicsUI.filmGrainToggle.AddLabel(MenuConfigurationGraphicsUI.localization.format("Film_Grain_Toggle_Label"), 1);
			MenuConfigurationGraphicsUI.filmGrainToggle.TooltipText = MenuConfigurationGraphicsUI.localization.format("Film_Grain_Tooltip");
			MenuConfigurationGraphicsUI.filmGrainToggle.OnValueChanged += new Toggled(MenuConfigurationGraphicsUI.onToggledFilmGrainToggle);
			MenuConfigurationGraphicsUI.graphicsBox.AddChild(MenuConfigurationGraphicsUI.filmGrainToggle);
			num += 50;
			MenuConfigurationGraphicsUI.blendToggle = Glazier.Get().CreateToggle();
			MenuConfigurationGraphicsUI.blendToggle.PositionOffset_X = 205f;
			MenuConfigurationGraphicsUI.blendToggle.PositionOffset_Y = (float)num;
			MenuConfigurationGraphicsUI.blendToggle.SizeOffset_X = 40f;
			MenuConfigurationGraphicsUI.blendToggle.SizeOffset_Y = 40f;
			MenuConfigurationGraphicsUI.blendToggle.AddLabel(MenuConfigurationGraphicsUI.localization.format("Blend_Toggle_Label"), 1);
			MenuConfigurationGraphicsUI.blendToggle.TooltipText = MenuConfigurationGraphicsUI.localization.format("Blend_Tooltip");
			MenuConfigurationGraphicsUI.blendToggle.Value = GraphicsSettings.blend;
			MenuConfigurationGraphicsUI.blendToggle.OnValueChanged += new Toggled(MenuConfigurationGraphicsUI.onToggledBlendToggle);
			MenuConfigurationGraphicsUI.graphicsBox.AddChild(MenuConfigurationGraphicsUI.blendToggle);
			num += 50;
			MenuConfigurationGraphicsUI.grassDisplacementToggle = Glazier.Get().CreateToggle();
			MenuConfigurationGraphicsUI.grassDisplacementToggle.PositionOffset_X = 205f;
			MenuConfigurationGraphicsUI.grassDisplacementToggle.PositionOffset_Y = (float)num;
			MenuConfigurationGraphicsUI.grassDisplacementToggle.SizeOffset_X = 40f;
			MenuConfigurationGraphicsUI.grassDisplacementToggle.SizeOffset_Y = 40f;
			MenuConfigurationGraphicsUI.grassDisplacementToggle.AddLabel(MenuConfigurationGraphicsUI.localization.format("Grass_Displacement_Toggle_Label"), 1);
			MenuConfigurationGraphicsUI.grassDisplacementToggle.TooltipText = MenuConfigurationGraphicsUI.localization.format("Grass_Displacement_Tooltip");
			MenuConfigurationGraphicsUI.grassDisplacementToggle.OnValueChanged += new Toggled(MenuConfigurationGraphicsUI.onToggledGrassDisplacementToggle);
			MenuConfigurationGraphicsUI.graphicsBox.AddChild(MenuConfigurationGraphicsUI.grassDisplacementToggle);
			num += 50;
			MenuConfigurationGraphicsUI.foliageFocusToggle = Glazier.Get().CreateToggle();
			MenuConfigurationGraphicsUI.foliageFocusToggle.PositionOffset_X = 205f;
			MenuConfigurationGraphicsUI.foliageFocusToggle.PositionOffset_Y = (float)num;
			MenuConfigurationGraphicsUI.foliageFocusToggle.SizeOffset_X = 40f;
			MenuConfigurationGraphicsUI.foliageFocusToggle.SizeOffset_Y = 40f;
			MenuConfigurationGraphicsUI.foliageFocusToggle.AddLabel(MenuConfigurationGraphicsUI.localization.format("Foliage_Focus_Toggle_Label"), 1);
			MenuConfigurationGraphicsUI.foliageFocusToggle.OnValueChanged += new Toggled(MenuConfigurationGraphicsUI.onToggledFoliageFocusToggle);
			MenuConfigurationGraphicsUI.foliageFocusToggle.TooltipText = MenuConfigurationGraphicsUI.localization.format("Foliage_Focus_Tooltip");
			MenuConfigurationGraphicsUI.graphicsBox.AddChild(MenuConfigurationGraphicsUI.foliageFocusToggle);
			num += 50;
			MenuConfigurationGraphicsUI.blastToggle = Glazier.Get().CreateToggle();
			MenuConfigurationGraphicsUI.blastToggle.PositionOffset_X = 205f;
			MenuConfigurationGraphicsUI.blastToggle.PositionOffset_Y = (float)num;
			MenuConfigurationGraphicsUI.blastToggle.SizeOffset_X = 40f;
			MenuConfigurationGraphicsUI.blastToggle.SizeOffset_Y = 40f;
			MenuConfigurationGraphicsUI.blastToggle.AddLabel(MenuConfigurationGraphicsUI.localization.format("Blast_Toggle_Label"), 1);
			MenuConfigurationGraphicsUI.blastToggle.TooltipText = MenuConfigurationGraphicsUI.localization.format("Blast_Toggle_Tooltip");
			MenuConfigurationGraphicsUI.blastToggle.OnValueChanged += new Toggled(MenuConfigurationGraphicsUI.onToggledBlastToggle);
			MenuConfigurationGraphicsUI.graphicsBox.AddChild(MenuConfigurationGraphicsUI.blastToggle);
			num += 50;
			MenuConfigurationGraphicsUI.puddleToggle = Glazier.Get().CreateToggle();
			MenuConfigurationGraphicsUI.puddleToggle.PositionOffset_X = 205f;
			MenuConfigurationGraphicsUI.puddleToggle.PositionOffset_Y = (float)num;
			MenuConfigurationGraphicsUI.puddleToggle.SizeOffset_X = 40f;
			MenuConfigurationGraphicsUI.puddleToggle.SizeOffset_Y = 40f;
			MenuConfigurationGraphicsUI.puddleToggle.AddLabel(MenuConfigurationGraphicsUI.localization.format("Puddle_Toggle_Label"), 1);
			MenuConfigurationGraphicsUI.puddleToggle.TooltipText = MenuConfigurationGraphicsUI.localization.format("Puddle_Tooltip");
			MenuConfigurationGraphicsUI.puddleToggle.OnValueChanged += new Toggled(MenuConfigurationGraphicsUI.onToggledPuddleToggle);
			MenuConfigurationGraphicsUI.graphicsBox.AddChild(MenuConfigurationGraphicsUI.puddleToggle);
			num += 50;
			MenuConfigurationGraphicsUI.glitterToggle = Glazier.Get().CreateToggle();
			MenuConfigurationGraphicsUI.glitterToggle.PositionOffset_X = 205f;
			MenuConfigurationGraphicsUI.glitterToggle.PositionOffset_Y = (float)num;
			MenuConfigurationGraphicsUI.glitterToggle.SizeOffset_X = 40f;
			MenuConfigurationGraphicsUI.glitterToggle.SizeOffset_Y = 40f;
			MenuConfigurationGraphicsUI.glitterToggle.AddLabel(MenuConfigurationGraphicsUI.localization.format("Glitter_Toggle_Label"), 1);
			MenuConfigurationGraphicsUI.glitterToggle.TooltipText = MenuConfigurationGraphicsUI.localization.format("Glitter_Tooltip");
			MenuConfigurationGraphicsUI.glitterToggle.OnValueChanged += new Toggled(MenuConfigurationGraphicsUI.onToggledGlitterToggle);
			MenuConfigurationGraphicsUI.graphicsBox.AddChild(MenuConfigurationGraphicsUI.glitterToggle);
			num += 50;
			MenuConfigurationGraphicsUI.triplanarToggle = Glazier.Get().CreateToggle();
			MenuConfigurationGraphicsUI.triplanarToggle.PositionOffset_X = 205f;
			MenuConfigurationGraphicsUI.triplanarToggle.PositionOffset_Y = (float)num;
			MenuConfigurationGraphicsUI.triplanarToggle.SizeOffset_X = 40f;
			MenuConfigurationGraphicsUI.triplanarToggle.SizeOffset_Y = 40f;
			MenuConfigurationGraphicsUI.triplanarToggle.AddLabel(MenuConfigurationGraphicsUI.localization.format("Triplanar_Toggle_Label"), 1);
			MenuConfigurationGraphicsUI.triplanarToggle.TooltipText = MenuConfigurationGraphicsUI.localization.format("Triplanar_Tooltip");
			MenuConfigurationGraphicsUI.triplanarToggle.OnValueChanged += new Toggled(MenuConfigurationGraphicsUI.onToggledTriplanarToggle);
			MenuConfigurationGraphicsUI.graphicsBox.AddChild(MenuConfigurationGraphicsUI.triplanarToggle);
			num += 50;
			MenuConfigurationGraphicsUI.skyboxReflectionToggle = Glazier.Get().CreateToggle();
			MenuConfigurationGraphicsUI.skyboxReflectionToggle.PositionOffset_X = 205f;
			MenuConfigurationGraphicsUI.skyboxReflectionToggle.PositionOffset_Y = (float)num;
			MenuConfigurationGraphicsUI.skyboxReflectionToggle.SizeOffset_X = 40f;
			MenuConfigurationGraphicsUI.skyboxReflectionToggle.SizeOffset_Y = 40f;
			MenuConfigurationGraphicsUI.skyboxReflectionToggle.AddLabel(MenuConfigurationGraphicsUI.localization.format("Skybox_Reflection_Label"), 1);
			MenuConfigurationGraphicsUI.skyboxReflectionToggle.TooltipText = MenuConfigurationGraphicsUI.localization.format("Skybox_Reflection_Tooltip");
			MenuConfigurationGraphicsUI.skyboxReflectionToggle.OnValueChanged += new Toggled(MenuConfigurationGraphicsUI.onToggledSkyboxReflectionToggle);
			MenuConfigurationGraphicsUI.graphicsBox.AddChild(MenuConfigurationGraphicsUI.skyboxReflectionToggle);
			num += 50;
			MenuConfigurationGraphicsUI.itemIconAntiAliasingToggle = Glazier.Get().CreateToggle();
			MenuConfigurationGraphicsUI.itemIconAntiAliasingToggle.PositionOffset_X = 205f;
			MenuConfigurationGraphicsUI.itemIconAntiAliasingToggle.PositionOffset_Y = (float)num;
			MenuConfigurationGraphicsUI.itemIconAntiAliasingToggle.SizeOffset_X = 40f;
			MenuConfigurationGraphicsUI.itemIconAntiAliasingToggle.SizeOffset_Y = 40f;
			MenuConfigurationGraphicsUI.itemIconAntiAliasingToggle.AddLabel(MenuConfigurationGraphicsUI.localization.format("Item_Icon_Anti_Aliasing_Label"), 1);
			MenuConfigurationGraphicsUI.itemIconAntiAliasingToggle.TooltipText = MenuConfigurationGraphicsUI.localization.format("Item_Icon_Anti_Aliasing_Tooltip");
			MenuConfigurationGraphicsUI.itemIconAntiAliasingToggle.OnValueChanged += new Toggled(MenuConfigurationGraphicsUI.onToggledItemIconAntiAliasingToggle);
			MenuConfigurationGraphicsUI.graphicsBox.AddChild(MenuConfigurationGraphicsUI.itemIconAntiAliasingToggle);
			num += 50;
			MenuConfigurationGraphicsUI.chromaticAberrationToggle = Glazier.Get().CreateToggle();
			MenuConfigurationGraphicsUI.chromaticAberrationToggle.PositionOffset_X = 205f;
			MenuConfigurationGraphicsUI.chromaticAberrationToggle.PositionOffset_Y = (float)num;
			MenuConfigurationGraphicsUI.chromaticAberrationToggle.SizeOffset_X = 40f;
			MenuConfigurationGraphicsUI.chromaticAberrationToggle.SizeOffset_Y = 40f;
			MenuConfigurationGraphicsUI.chromaticAberrationToggle.AddLabel(MenuConfigurationGraphicsUI.localization.format("Chromatic_Aberration_Toggle_Label"), 1);
			MenuConfigurationGraphicsUI.chromaticAberrationToggle.TooltipText = MenuConfigurationGraphicsUI.localization.format("Chromatic_Aberration_Tooltip");
			MenuConfigurationGraphicsUI.chromaticAberrationToggle.OnValueChanged += new Toggled(MenuConfigurationGraphicsUI.onToggledChromaticAberrationToggle);
			MenuConfigurationGraphicsUI.graphicsBox.AddChild(MenuConfigurationGraphicsUI.chromaticAberrationToggle);
			num += 50;
			MenuConfigurationGraphicsUI.antiAliasingButton = new SleekButtonState(new GUIContent[]
			{
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("Off")),
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("FXAA")),
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("TAA"))
			});
			MenuConfigurationGraphicsUI.antiAliasingButton.PositionOffset_X = 205f;
			MenuConfigurationGraphicsUI.antiAliasingButton.PositionOffset_Y = (float)num;
			MenuConfigurationGraphicsUI.antiAliasingButton.SizeOffset_X = 200f;
			MenuConfigurationGraphicsUI.antiAliasingButton.SizeOffset_Y = 30f;
			MenuConfigurationGraphicsUI.antiAliasingButton.AddLabel(MenuConfigurationGraphicsUI.localization.format("Anti_Aliasing_Button_Label"), 1);
			MenuConfigurationGraphicsUI.antiAliasingButton.tooltip = MenuConfigurationGraphicsUI.localization.format("Anti_Aliasing_Button_Tooltip");
			MenuConfigurationGraphicsUI.antiAliasingButton.onSwappedState = new SwappedState(MenuConfigurationGraphicsUI.onSwappedAntiAliasingState);
			MenuConfigurationGraphicsUI.graphicsBox.AddChild(MenuConfigurationGraphicsUI.antiAliasingButton);
			num += 40;
			MenuConfigurationGraphicsUI.anisotropicFilteringButton = new SleekButtonState(new GUIContent[]
			{
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("AF_Disabled")),
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("AF_Per_Texture")),
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("AF_Forced_On"))
			});
			MenuConfigurationGraphicsUI.anisotropicFilteringButton.PositionOffset_X = 205f;
			MenuConfigurationGraphicsUI.anisotropicFilteringButton.PositionOffset_Y = (float)num;
			MenuConfigurationGraphicsUI.anisotropicFilteringButton.SizeOffset_X = 200f;
			MenuConfigurationGraphicsUI.anisotropicFilteringButton.SizeOffset_Y = 30f;
			MenuConfigurationGraphicsUI.anisotropicFilteringButton.AddLabel(MenuConfigurationGraphicsUI.localization.format("Anisotropic_Filtering_Button_Label"), 1);
			MenuConfigurationGraphicsUI.anisotropicFilteringButton.tooltip = MenuConfigurationGraphicsUI.localization.format("Anisotropic_Filtering_Button_Tooltip");
			MenuConfigurationGraphicsUI.anisotropicFilteringButton.onSwappedState = new SwappedState(MenuConfigurationGraphicsUI.onSwappedAnisotropicFilteringState);
			MenuConfigurationGraphicsUI.graphicsBox.AddChild(MenuConfigurationGraphicsUI.anisotropicFilteringButton);
			num += 40;
			MenuConfigurationGraphicsUI.effectButton = new SleekButtonState(new GUIContent[]
			{
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("Low")),
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("Medium")),
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("High")),
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("Ultra"))
			});
			MenuConfigurationGraphicsUI.effectButton.PositionOffset_X = 205f;
			MenuConfigurationGraphicsUI.effectButton.PositionOffset_Y = (float)num;
			MenuConfigurationGraphicsUI.effectButton.SizeOffset_X = 200f;
			MenuConfigurationGraphicsUI.effectButton.SizeOffset_Y = 30f;
			MenuConfigurationGraphicsUI.effectButton.AddLabel(MenuConfigurationGraphicsUI.localization.format("Effect_Button_Label"), 1);
			MenuConfigurationGraphicsUI.effectButton.tooltip = MenuConfigurationGraphicsUI.localization.format("Effect_Button_Tooltip");
			MenuConfigurationGraphicsUI.effectButton.tooltip = RichTextUtil.wrapWithColor(MenuConfigurationGraphicsUI.localization.format("Effect_Button_Tooltip"), color) + RichTextUtil.wrapWithColor(string.Concat(new string[]
			{
				"\n",
				MenuConfigurationGraphicsUI.localization.format("Effect_Tier", MenuConfigurationGraphicsUI.localization.format("Low"), 16f),
				"\n",
				MenuConfigurationGraphicsUI.localization.format("Effect_Tier", MenuConfigurationGraphicsUI.localization.format("Medium"), 32f),
				"\n",
				MenuConfigurationGraphicsUI.localization.format("Effect_Tier", MenuConfigurationGraphicsUI.localization.format("High"), 48f),
				"\n",
				MenuConfigurationGraphicsUI.localization.format("Effect_Tier", MenuConfigurationGraphicsUI.localization.format("Ultra"), 64f)
			}), color2);
			MenuConfigurationGraphicsUI.effectButton.onSwappedState = new SwappedState(MenuConfigurationGraphicsUI.onSwappedEffectState);
			MenuConfigurationGraphicsUI.graphicsBox.AddChild(MenuConfigurationGraphicsUI.effectButton);
			num += 40;
			MenuConfigurationGraphicsUI.foliageButton = new SleekButtonState(new GUIContent[]
			{
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("Low")),
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("Medium")),
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("High")),
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("Ultra"))
			});
			MenuConfigurationGraphicsUI.foliageButton.PositionOffset_X = 205f;
			MenuConfigurationGraphicsUI.foliageButton.PositionOffset_Y = (float)num;
			MenuConfigurationGraphicsUI.foliageButton.SizeOffset_X = 200f;
			MenuConfigurationGraphicsUI.foliageButton.SizeOffset_Y = 30f;
			MenuConfigurationGraphicsUI.foliageButton.AddLabel(MenuConfigurationGraphicsUI.localization.format("Foliage_Button_Label"), 1);
			MenuConfigurationGraphicsUI.foliageButton.tooltip = MenuConfigurationGraphicsUI.localization.format("Foliage_Button_Tooltip");
			MenuConfigurationGraphicsUI.foliageButton.onSwappedState = new SwappedState(MenuConfigurationGraphicsUI.onSwappedFoliageState);
			MenuConfigurationGraphicsUI.graphicsBox.AddChild(MenuConfigurationGraphicsUI.foliageButton);
			MenuConfigurationGraphicsUI.foliagePerf = new SleekBoxIcon(bundle.load<Texture2D>("Perf"));
			MenuConfigurationGraphicsUI.foliagePerf.PositionOffset_X = 175f;
			MenuConfigurationGraphicsUI.foliagePerf.PositionOffset_Y = (float)num;
			MenuConfigurationGraphicsUI.foliagePerf.SizeOffset_X = 30f;
			MenuConfigurationGraphicsUI.foliagePerf.SizeOffset_Y = 30f;
			MenuConfigurationGraphicsUI.foliagePerf.iconColor = 2;
			MenuConfigurationGraphicsUI.foliagePerf.tooltip = RichTextUtil.wrapWithColor(MenuConfigurationGraphicsUI.localization.format("Perf_Foliage_Instancing_Not_Supported"), new Color(1f, 0.5f, 0f));
			MenuConfigurationGraphicsUI.graphicsBox.AddChild(MenuConfigurationGraphicsUI.foliagePerf);
			num += 40;
			MenuConfigurationGraphicsUI.sunShaftsButton = new SleekButtonState(new GUIContent[]
			{
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("Off")),
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("Medium")),
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("High")),
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("Ultra"))
			});
			MenuConfigurationGraphicsUI.sunShaftsButton.PositionOffset_X = 205f;
			MenuConfigurationGraphicsUI.sunShaftsButton.PositionOffset_Y = (float)num;
			MenuConfigurationGraphicsUI.sunShaftsButton.SizeOffset_X = 200f;
			MenuConfigurationGraphicsUI.sunShaftsButton.SizeOffset_Y = 30f;
			MenuConfigurationGraphicsUI.sunShaftsButton.AddLabel(MenuConfigurationGraphicsUI.localization.format("Sun_Shafts_Button_Label"), 1);
			MenuConfigurationGraphicsUI.sunShaftsButton.tooltip = MenuConfigurationGraphicsUI.localization.format("Sun_Shafts_Button_Tooltip");
			MenuConfigurationGraphicsUI.sunShaftsButton.onSwappedState = new SwappedState(MenuConfigurationGraphicsUI.onSwappedSunShaftsState);
			MenuConfigurationGraphicsUI.graphicsBox.AddChild(MenuConfigurationGraphicsUI.sunShaftsButton);
			num += 40;
			MenuConfigurationGraphicsUI.lightingButton = new SleekButtonState(new GUIContent[]
			{
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("Off")),
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("Low")),
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("Medium")),
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("High")),
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("Ultra"))
			});
			MenuConfigurationGraphicsUI.lightingButton.PositionOffset_X = 205f;
			MenuConfigurationGraphicsUI.lightingButton.PositionOffset_Y = (float)num;
			MenuConfigurationGraphicsUI.lightingButton.SizeOffset_X = 200f;
			MenuConfigurationGraphicsUI.lightingButton.SizeOffset_Y = 30f;
			MenuConfigurationGraphicsUI.lightingButton.AddLabel(MenuConfigurationGraphicsUI.localization.format("Lighting_Button_Label"), 1);
			MenuConfigurationGraphicsUI.lightingButton.tooltip = MenuConfigurationGraphicsUI.localization.format("Lighting_Button_Tooltip");
			MenuConfigurationGraphicsUI.lightingButton.onSwappedState = new SwappedState(MenuConfigurationGraphicsUI.onSwappedLightingState);
			MenuConfigurationGraphicsUI.graphicsBox.AddChild(MenuConfigurationGraphicsUI.lightingButton);
			num += 40;
			MenuConfigurationGraphicsUI.reflectionButton = new SleekButtonState(new GUIContent[]
			{
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("Off")),
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("Low")),
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("Medium")),
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("High")),
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("Ultra"))
			});
			MenuConfigurationGraphicsUI.reflectionButton.PositionOffset_X = 205f;
			MenuConfigurationGraphicsUI.reflectionButton.PositionOffset_Y = (float)num;
			MenuConfigurationGraphicsUI.reflectionButton.SizeOffset_X = 200f;
			MenuConfigurationGraphicsUI.reflectionButton.SizeOffset_Y = 30f;
			MenuConfigurationGraphicsUI.reflectionButton.AddLabel(MenuConfigurationGraphicsUI.localization.format("Reflection_Button_Label"), 1);
			MenuConfigurationGraphicsUI.reflectionButton.tooltip = MenuConfigurationGraphicsUI.localization.format("Reflection_Button_Tooltip");
			MenuConfigurationGraphicsUI.reflectionButton.onSwappedState = new SwappedState(MenuConfigurationGraphicsUI.onSwappedReflectionState);
			MenuConfigurationGraphicsUI.graphicsBox.AddChild(MenuConfigurationGraphicsUI.reflectionButton);
			num += 40;
			MenuConfigurationGraphicsUI.waterButton = new SleekButtonState(new GUIContent[]
			{
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("Low")),
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("Medium")),
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("High")),
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("Ultra"))
			});
			MenuConfigurationGraphicsUI.waterButton.PositionOffset_X = 205f;
			MenuConfigurationGraphicsUI.waterButton.PositionOffset_Y = (float)num;
			MenuConfigurationGraphicsUI.waterButton.SizeOffset_X = 200f;
			MenuConfigurationGraphicsUI.waterButton.SizeOffset_Y = 30f;
			MenuConfigurationGraphicsUI.waterButton.AddLabel(MenuConfigurationGraphicsUI.localization.format("Water_Button_Label"), 1);
			MenuConfigurationGraphicsUI.waterButton.tooltip = MenuConfigurationGraphicsUI.localization.format("Water_Button_Tooltip");
			MenuConfigurationGraphicsUI.waterButton.onSwappedState = new SwappedState(MenuConfigurationGraphicsUI.onSwappedWaterState);
			MenuConfigurationGraphicsUI.graphicsBox.AddChild(MenuConfigurationGraphicsUI.waterButton);
			MenuConfigurationGraphicsUI.waterPerf = new SleekBoxIcon(bundle.load<Texture2D>("Perf"));
			MenuConfigurationGraphicsUI.waterPerf.PositionOffset_X = 175f;
			MenuConfigurationGraphicsUI.waterPerf.PositionOffset_Y = (float)num;
			MenuConfigurationGraphicsUI.waterPerf.SizeOffset_X = 30f;
			MenuConfigurationGraphicsUI.waterPerf.SizeOffset_Y = 30f;
			MenuConfigurationGraphicsUI.waterPerf.iconColor = 2;
			MenuConfigurationGraphicsUI.waterPerf.tooltip = RichTextUtil.wrapWithColor(MenuConfigurationGraphicsUI.localization.format("Perf_Water_Reflections"), new Color(1f, 0.5f, 0f));
			MenuConfigurationGraphicsUI.graphicsBox.AddChild(MenuConfigurationGraphicsUI.waterPerf);
			num += 40;
			MenuConfigurationGraphicsUI.planarReflectionButton = new SleekButtonState(new GUIContent[]
			{
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("Low")),
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("Medium")),
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("High")),
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("Ultra"))
			});
			MenuConfigurationGraphicsUI.planarReflectionButton.PositionOffset_X = 205f;
			MenuConfigurationGraphicsUI.planarReflectionButton.PositionOffset_Y = (float)num;
			MenuConfigurationGraphicsUI.planarReflectionButton.SizeOffset_X = 200f;
			MenuConfigurationGraphicsUI.planarReflectionButton.SizeOffset_Y = 30f;
			MenuConfigurationGraphicsUI.planarReflectionButton.AddLabel(MenuConfigurationGraphicsUI.localization.format("Planar_Reflection_Button_Label"), 1);
			MenuConfigurationGraphicsUI.planarReflectionButton.tooltip = RichTextUtil.wrapWithColor(MenuConfigurationGraphicsUI.localization.format("Planar_Reflection_Button_Tooltip"), color) + RichTextUtil.wrapWithColor(string.Concat(new string[]
			{
				"\n",
				MenuConfigurationGraphicsUI.localization.format("Planar_Reflection_Low", MenuConfigurationGraphicsUI.localization.format("Low")),
				"\n",
				MenuConfigurationGraphicsUI.localization.format("Planar_Reflection_Medium", MenuConfigurationGraphicsUI.localization.format("Medium")),
				"\n",
				MenuConfigurationGraphicsUI.localization.format("Planar_Reflection_High", MenuConfigurationGraphicsUI.localization.format("High")),
				"\n",
				MenuConfigurationGraphicsUI.localization.format("Planar_Reflection_Ultra", MenuConfigurationGraphicsUI.localization.format("Ultra"))
			}), color2);
			MenuConfigurationGraphicsUI.planarReflectionButton.onSwappedState = new SwappedState(MenuConfigurationGraphicsUI.onSwappedPlanarReflectionState);
			MenuConfigurationGraphicsUI.graphicsBox.AddChild(MenuConfigurationGraphicsUI.planarReflectionButton);
			num += 40;
			MenuConfigurationGraphicsUI.scopeButton = new SleekButtonState(new GUIContent[]
			{
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("Off")),
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("Low")),
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("Medium")),
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("High")),
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("Ultra"))
			});
			MenuConfigurationGraphicsUI.scopeButton.PositionOffset_X = 205f;
			MenuConfigurationGraphicsUI.scopeButton.PositionOffset_Y = (float)num;
			MenuConfigurationGraphicsUI.scopeButton.SizeOffset_X = 200f;
			MenuConfigurationGraphicsUI.scopeButton.SizeOffset_Y = 30f;
			MenuConfigurationGraphicsUI.scopeButton.AddLabel(MenuConfigurationGraphicsUI.localization.format("Scope_Button_Label"), 1);
			MenuConfigurationGraphicsUI.scopeButton.tooltip = MenuConfigurationGraphicsUI.localization.format("Scope_Button_Tooltip");
			MenuConfigurationGraphicsUI.scopeButton.onSwappedState = new SwappedState(MenuConfigurationGraphicsUI.onSwappedScopeState);
			MenuConfigurationGraphicsUI.graphicsBox.AddChild(MenuConfigurationGraphicsUI.scopeButton);
			MenuConfigurationGraphicsUI.scopePerf = new SleekBoxIcon(bundle.load<Texture2D>("Perf"));
			MenuConfigurationGraphicsUI.scopePerf.PositionOffset_X = 175f;
			MenuConfigurationGraphicsUI.scopePerf.PositionOffset_Y = (float)num;
			MenuConfigurationGraphicsUI.scopePerf.SizeOffset_X = 30f;
			MenuConfigurationGraphicsUI.scopePerf.SizeOffset_Y = 30f;
			MenuConfigurationGraphicsUI.scopePerf.iconColor = 2;
			MenuConfigurationGraphicsUI.scopePerf.tooltip = RichTextUtil.wrapWithColor(MenuConfigurationGraphicsUI.localization.format("Perf_Dual_Render_Scope"), new Color(1f, 0.5f, 0f));
			MenuConfigurationGraphicsUI.graphicsBox.AddChild(MenuConfigurationGraphicsUI.scopePerf);
			num += 40;
			MenuConfigurationGraphicsUI.outlineButton = new SleekButtonState(new GUIContent[]
			{
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("Low")),
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("Medium")),
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("High")),
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("Ultra"))
			});
			MenuConfigurationGraphicsUI.outlineButton.PositionOffset_X = 205f;
			MenuConfigurationGraphicsUI.outlineButton.PositionOffset_Y = (float)num;
			MenuConfigurationGraphicsUI.outlineButton.SizeOffset_X = 200f;
			MenuConfigurationGraphicsUI.outlineButton.SizeOffset_Y = 30f;
			MenuConfigurationGraphicsUI.outlineButton.AddLabel(MenuConfigurationGraphicsUI.localization.format("Outline_Button_Label"), 1);
			MenuConfigurationGraphicsUI.outlineButton.tooltip = MenuConfigurationGraphicsUI.localization.format("Outline_Button_Tooltip");
			MenuConfigurationGraphicsUI.outlineButton.onSwappedState = new SwappedState(MenuConfigurationGraphicsUI.onSwappedOutlineState);
			MenuConfigurationGraphicsUI.graphicsBox.AddChild(MenuConfigurationGraphicsUI.outlineButton);
			num += 40;
			MenuConfigurationGraphicsUI.terrainButton = new SleekButtonState(new GUIContent[]
			{
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("Low")),
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("Medium")),
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("High")),
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("Ultra"))
			});
			MenuConfigurationGraphicsUI.terrainButton.PositionOffset_X = 205f;
			MenuConfigurationGraphicsUI.terrainButton.PositionOffset_Y = (float)num;
			MenuConfigurationGraphicsUI.terrainButton.SizeOffset_X = 200f;
			MenuConfigurationGraphicsUI.terrainButton.SizeOffset_Y = 30f;
			MenuConfigurationGraphicsUI.terrainButton.AddLabel(MenuConfigurationGraphicsUI.localization.format("Terrain_Button_Label"), 1);
			MenuConfigurationGraphicsUI.terrainButton.tooltip = MenuConfigurationGraphicsUI.localization.format("Terrain_Button_Tooltip");
			MenuConfigurationGraphicsUI.terrainButton.onSwappedState = new SwappedState(MenuConfigurationGraphicsUI.onSwappedTerrainState);
			MenuConfigurationGraphicsUI.graphicsBox.AddChild(MenuConfigurationGraphicsUI.terrainButton);
			num += 40;
			MenuConfigurationGraphicsUI.windButton = new SleekButtonState(new GUIContent[]
			{
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("Off")),
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("Low")),
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("Medium")),
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("High")),
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("Ultra"))
			});
			MenuConfigurationGraphicsUI.windButton.PositionOffset_X = 205f;
			MenuConfigurationGraphicsUI.windButton.PositionOffset_Y = (float)num;
			MenuConfigurationGraphicsUI.windButton.SizeOffset_X = 200f;
			MenuConfigurationGraphicsUI.windButton.SizeOffset_Y = 30f;
			MenuConfigurationGraphicsUI.windButton.AddLabel(MenuConfigurationGraphicsUI.localization.format("Wind_Button_Label"), 1);
			MenuConfigurationGraphicsUI.windButton.tooltip = RichTextUtil.wrapWithColor(MenuConfigurationGraphicsUI.localization.format("Wind_Button_Tooltip"), color) + RichTextUtil.wrapWithColor("\n" + MenuConfigurationGraphicsUI.localization.format("Wind_Low", MenuConfigurationGraphicsUI.localization.format("Low")) + "\n" + MenuConfigurationGraphicsUI.localization.format("Wind_Medium", MenuConfigurationGraphicsUI.localization.format("Medium")), color2);
			MenuConfigurationGraphicsUI.windButton.onSwappedState = new SwappedState(MenuConfigurationGraphicsUI.onSwappedWindState);
			MenuConfigurationGraphicsUI.graphicsBox.AddChild(MenuConfigurationGraphicsUI.windButton);
			num += 40;
			MenuConfigurationGraphicsUI.treeModeButton = new SleekButtonState(new GUIContent[]
			{
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("TM_Legacy")),
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("TM_SpeedTree_Fade_None")),
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("TM_SpeedTree_Fade_SpeedTree"))
			});
			MenuConfigurationGraphicsUI.treeModeButton.PositionOffset_X = 205f;
			MenuConfigurationGraphicsUI.treeModeButton.PositionOffset_Y = (float)num;
			MenuConfigurationGraphicsUI.treeModeButton.SizeOffset_X = 200f;
			MenuConfigurationGraphicsUI.treeModeButton.SizeOffset_Y = 30f;
			MenuConfigurationGraphicsUI.treeModeButton.AddLabel(MenuConfigurationGraphicsUI.localization.format("Tree_Mode_Button_Label"), 1);
			MenuConfigurationGraphicsUI.treeModeButton.tooltip = MenuConfigurationGraphicsUI.localization.format("Tree_Mode_Button_Tooltip");
			MenuConfigurationGraphicsUI.treeModeButton.onSwappedState = new SwappedState(MenuConfigurationGraphicsUI.onSwappedTreeModeState);
			MenuConfigurationGraphicsUI.graphicsBox.AddChild(MenuConfigurationGraphicsUI.treeModeButton);
			MenuConfigurationGraphicsUI.treePerf = new SleekBoxIcon(bundle.load<Texture2D>("Perf"));
			MenuConfigurationGraphicsUI.treePerf.PositionOffset_X = 175f;
			MenuConfigurationGraphicsUI.treePerf.PositionOffset_Y = (float)num;
			MenuConfigurationGraphicsUI.treePerf.SizeOffset_X = 30f;
			MenuConfigurationGraphicsUI.treePerf.SizeOffset_Y = 30f;
			MenuConfigurationGraphicsUI.treePerf.iconColor = 2;
			MenuConfigurationGraphicsUI.treePerf.tooltip = RichTextUtil.wrapWithColor(MenuConfigurationGraphicsUI.localization.format("Perf_SpeedTrees"), new Color(1f, 0.5f, 0f));
			MenuConfigurationGraphicsUI.graphicsBox.AddChild(MenuConfigurationGraphicsUI.treePerf);
			num += 40;
			MenuConfigurationGraphicsUI.renderButton = new SleekButtonState(new GUIContent[]
			{
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("Deferred")),
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("Forward"))
			});
			MenuConfigurationGraphicsUI.renderButton.PositionOffset_X = 205f;
			MenuConfigurationGraphicsUI.renderButton.PositionOffset_Y = (float)num;
			MenuConfigurationGraphicsUI.renderButton.SizeOffset_X = 200f;
			MenuConfigurationGraphicsUI.renderButton.SizeOffset_Y = 30f;
			MenuConfigurationGraphicsUI.renderButton.AddLabel(MenuConfigurationGraphicsUI.localization.format("Render_Mode_Button_Label"), 1);
			MenuConfigurationGraphicsUI.renderButton.tooltip = MenuConfigurationGraphicsUI.localization.format("Render_Mode_Button_Tooltip");
			MenuConfigurationGraphicsUI.renderButton.onSwappedState = new SwappedState(MenuConfigurationGraphicsUI.onSwappedRenderState);
			MenuConfigurationGraphicsUI.graphicsBox.AddChild(MenuConfigurationGraphicsUI.renderButton);
			num += 40;
			MenuConfigurationGraphicsUI.graphicsBox.ContentSizeOffset = new Vector2(0f, (float)(num - 10));
			MenuConfigurationGraphicsUI.backButton = new SleekButtonIcon(MenuDashboardUI.icons.load<Texture2D>("Exit"));
			MenuConfigurationGraphicsUI.backButton.PositionOffset_Y = -50f;
			MenuConfigurationGraphicsUI.backButton.PositionScale_Y = 1f;
			MenuConfigurationGraphicsUI.backButton.SizeOffset_X = 200f;
			MenuConfigurationGraphicsUI.backButton.SizeOffset_Y = 50f;
			MenuConfigurationGraphicsUI.backButton.text = MenuDashboardUI.localization.format("BackButtonText");
			MenuConfigurationGraphicsUI.backButton.tooltip = MenuDashboardUI.localization.format("BackButtonTooltip");
			MenuConfigurationGraphicsUI.backButton.onClickedButton += new ClickedButton(MenuConfigurationGraphicsUI.onClickedBackButton);
			MenuConfigurationGraphicsUI.backButton.fontSize = 3;
			MenuConfigurationGraphicsUI.backButton.iconColor = 2;
			MenuConfigurationGraphicsUI.container.AddChild(MenuConfigurationGraphicsUI.backButton);
			MenuConfigurationGraphicsUI.defaultButton = Glazier.Get().CreateButton();
			MenuConfigurationGraphicsUI.defaultButton.PositionOffset_X = -200f;
			MenuConfigurationGraphicsUI.defaultButton.PositionOffset_Y = -50f;
			MenuConfigurationGraphicsUI.defaultButton.PositionScale_X = 1f;
			MenuConfigurationGraphicsUI.defaultButton.PositionScale_Y = 1f;
			MenuConfigurationGraphicsUI.defaultButton.SizeOffset_X = 200f;
			MenuConfigurationGraphicsUI.defaultButton.SizeOffset_Y = 50f;
			MenuConfigurationGraphicsUI.defaultButton.Text = MenuPlayConfigUI.localization.format("Default");
			MenuConfigurationGraphicsUI.defaultButton.TooltipText = MenuPlayConfigUI.localization.format("Default_Tooltip");
			MenuConfigurationGraphicsUI.defaultButton.OnClicked += new ClickedButton(MenuConfigurationGraphicsUI.onClickedDefaultButton);
			MenuConfigurationGraphicsUI.defaultButton.FontSize = 3;
			MenuConfigurationGraphicsUI.container.AddChild(MenuConfigurationGraphicsUI.defaultButton);
			MenuConfigurationGraphicsUI.updateAll();
		}

		// Token: 0x040028BB RID: 10427
		private static Local localization;

		// Token: 0x040028BC RID: 10428
		private static SleekFullscreenBox container;

		// Token: 0x040028BD RID: 10429
		public static bool active;

		// Token: 0x040028BE RID: 10430
		private static SleekButtonIcon backButton;

		// Token: 0x040028BF RID: 10431
		private static ISleekButton defaultButton;

		// Token: 0x040028C0 RID: 10432
		private static ISleekScrollView graphicsBox;

		// Token: 0x040028C1 RID: 10433
		private static ISleekToggle ambientOcclusionToggle;

		// Token: 0x040028C2 RID: 10434
		private static ISleekToggle bloomToggle;

		// Token: 0x040028C3 RID: 10435
		private static ISleekToggle chromaticAberrationToggle;

		// Token: 0x040028C4 RID: 10436
		private static ISleekToggle filmGrainToggle;

		// Token: 0x040028C5 RID: 10437
		private static ISleekToggle blendToggle;

		// Token: 0x040028C6 RID: 10438
		private static ISleekToggle grassDisplacementToggle;

		// Token: 0x040028C7 RID: 10439
		private static ISleekToggle foliageFocusToggle;

		// Token: 0x040028C8 RID: 10440
		private static ISleekToggle ragdollsToggle;

		// Token: 0x040028C9 RID: 10441
		private static ISleekToggle debrisToggle;

		// Token: 0x040028CA RID: 10442
		private static ISleekToggle blastToggle;

		// Token: 0x040028CB RID: 10443
		private static ISleekToggle puddleToggle;

		// Token: 0x040028CC RID: 10444
		private static ISleekToggle glitterToggle;

		// Token: 0x040028CD RID: 10445
		private static ISleekToggle triplanarToggle;

		// Token: 0x040028CE RID: 10446
		private static ISleekToggle skyboxReflectionToggle;

		// Token: 0x040028CF RID: 10447
		private static ISleekToggle itemIconAntiAliasingToggle;

		// Token: 0x040028D0 RID: 10448
		private static ISleekSlider farClipDistanceSlider;

		// Token: 0x040028D1 RID: 10449
		private static ISleekSlider distanceSlider;

		// Token: 0x040028D2 RID: 10450
		private static ISleekSlider landmarkSlider;

		// Token: 0x040028D3 RID: 10451
		private static SleekButtonState landmarkButton;

		// Token: 0x040028D4 RID: 10452
		public static SleekButtonState antiAliasingButton;

		// Token: 0x040028D5 RID: 10453
		public static SleekButtonState anisotropicFilteringButton;

		// Token: 0x040028D6 RID: 10454
		private static SleekButtonState effectButton;

		// Token: 0x040028D7 RID: 10455
		private static SleekBoxIcon foliagePerf;

		// Token: 0x040028D8 RID: 10456
		private static SleekButtonState foliageButton;

		// Token: 0x040028D9 RID: 10457
		private static SleekButtonState sunShaftsButton;

		// Token: 0x040028DA RID: 10458
		private static SleekButtonState lightingButton;

		// Token: 0x040028DB RID: 10459
		private static SleekButtonState ambientOcclusionButton;

		// Token: 0x040028DC RID: 10460
		private static SleekButtonState reflectionButton;

		// Token: 0x040028DD RID: 10461
		private static SleekButtonState planarReflectionButton;

		// Token: 0x040028DE RID: 10462
		private static SleekButtonState waterButton;

		// Token: 0x040028DF RID: 10463
		private static SleekBoxIcon waterPerf;

		// Token: 0x040028E0 RID: 10464
		private static SleekButtonState scopeButton;

		// Token: 0x040028E1 RID: 10465
		private static SleekBoxIcon scopePerf;

		// Token: 0x040028E2 RID: 10466
		private static SleekButtonState outlineButton;

		// Token: 0x040028E3 RID: 10467
		private static SleekButtonState terrainButton;

		// Token: 0x040028E4 RID: 10468
		private static SleekButtonState windButton;

		// Token: 0x040028E5 RID: 10469
		private static SleekButtonState treeModeButton;

		// Token: 0x040028E6 RID: 10470
		private static SleekBoxIcon treePerf;

		// Token: 0x040028E7 RID: 10471
		private static SleekButtonState renderButton;
	}
}
