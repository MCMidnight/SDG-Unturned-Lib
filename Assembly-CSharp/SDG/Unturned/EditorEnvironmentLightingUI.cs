using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000772 RID: 1906
	public class EditorEnvironmentLightingUI
	{
		// Token: 0x06003E46 RID: 15942 RVA: 0x0012F8A3 File Offset: 0x0012DAA3
		public static void open()
		{
			if (EditorEnvironmentLightingUI.active)
			{
				return;
			}
			EditorEnvironmentLightingUI.active = true;
			EditorEnvironmentLightingUI.container.AnimateIntoView();
		}

		// Token: 0x06003E47 RID: 15943 RVA: 0x0012F8BD File Offset: 0x0012DABD
		public static void close()
		{
			if (!EditorEnvironmentLightingUI.active)
			{
				return;
			}
			EditorEnvironmentLightingUI.active = false;
			EditorEnvironmentLightingUI.container.AnimateOutOfView(1f, 0f);
		}

		// Token: 0x06003E48 RID: 15944 RVA: 0x0012F8E1 File Offset: 0x0012DAE1
		private static void onDraggedAzimuthSlider(ISleekSlider slider, float state)
		{
			LevelLighting.azimuth = state * 360f;
		}

		// Token: 0x06003E49 RID: 15945 RVA: 0x0012F8EF File Offset: 0x0012DAEF
		private static void onDraggedBiasSlider(ISleekSlider slider, float state)
		{
			LevelLighting.bias = state;
		}

		// Token: 0x06003E4A RID: 15946 RVA: 0x0012F8F7 File Offset: 0x0012DAF7
		private static void onDraggedFadeSlider(ISleekSlider slider, float state)
		{
			LevelLighting.fade = state;
		}

		// Token: 0x06003E4B RID: 15947 RVA: 0x0012F8FF File Offset: 0x0012DAFF
		private static void onValuedSeaLevelSlider(SleekValue slider, float state)
		{
			LevelLighting.seaLevel = state;
		}

		// Token: 0x06003E4C RID: 15948 RVA: 0x0012F907 File Offset: 0x0012DB07
		private static void onValuedSnowLevelSlider(SleekValue slider, float state)
		{
			LevelLighting.snowLevel = state;
		}

		// Token: 0x06003E4D RID: 15949 RVA: 0x0012F90F File Offset: 0x0012DB0F
		private static void onToggledRainToggle(ISleekToggle toggle, bool state)
		{
			LevelLighting.canRain = state;
		}

		// Token: 0x06003E4E RID: 15950 RVA: 0x0012F917 File Offset: 0x0012DB17
		private static void onToggledSnowToggle(ISleekToggle toggle, bool state)
		{
			LevelLighting.canSnow = state;
		}

		// Token: 0x06003E4F RID: 15951 RVA: 0x0012F91F File Offset: 0x0012DB1F
		private static void onTypedRainFreqField(ISleekFloat32Field field, float state)
		{
			LevelLighting.rainFreq = state;
		}

		// Token: 0x06003E50 RID: 15952 RVA: 0x0012F927 File Offset: 0x0012DB27
		private static void onTypedRainDurField(ISleekFloat32Field field, float state)
		{
			LevelLighting.rainDur = state;
		}

		// Token: 0x06003E51 RID: 15953 RVA: 0x0012F92F File Offset: 0x0012DB2F
		private static void onTypedSnowFreqField(ISleekFloat32Field field, float state)
		{
			LevelLighting.snowFreq = state;
		}

		// Token: 0x06003E52 RID: 15954 RVA: 0x0012F937 File Offset: 0x0012DB37
		private static void onTypedSnowDurField(ISleekFloat32Field field, float state)
		{
			LevelLighting.snowDur = state;
		}

		// Token: 0x06003E53 RID: 15955 RVA: 0x0012F940 File Offset: 0x0012DB40
		private static void onDraggedMoonSlider(ISleekSlider slider, float state)
		{
			byte b = (byte)(state * (float)LevelLighting.MOON_CYCLES);
			if (b >= LevelLighting.MOON_CYCLES)
			{
				b = LevelLighting.MOON_CYCLES - 1;
			}
			LevelLighting.moon = b;
		}

		// Token: 0x06003E54 RID: 15956 RVA: 0x0012F96E File Offset: 0x0012DB6E
		private static void onDraggedTimeSlider(ISleekSlider slider, float state)
		{
			LevelLighting.time = state;
		}

		// Token: 0x06003E55 RID: 15957 RVA: 0x0012F978 File Offset: 0x0012DB78
		private static void onClickedTimeButton(ISleekElement button)
		{
			int num = 0;
			while (num < EditorEnvironmentLightingUI.timeButtons.Length && EditorEnvironmentLightingUI.timeButtons[num] != button)
			{
				num++;
			}
			EditorEnvironmentLightingUI.selectedTime = (ELightingTime)num;
			EditorEnvironmentLightingUI.updateSelection();
			switch (EditorEnvironmentLightingUI.selectedTime)
			{
			case ELightingTime.DAWN:
				LevelLighting.time = 0f;
				break;
			case ELightingTime.MIDDAY:
				LevelLighting.time = LevelLighting.bias / 2f;
				break;
			case ELightingTime.DUSK:
				LevelLighting.time = LevelLighting.bias;
				break;
			case ELightingTime.MIDNIGHT:
				LevelLighting.time = 1f - (1f - LevelLighting.bias) / 2f;
				break;
			}
			EditorEnvironmentLightingUI.timeSlider.Value = LevelLighting.time;
		}

		// Token: 0x06003E56 RID: 15958 RVA: 0x0012FA20 File Offset: 0x0012DC20
		private static void OnClickedPreviewWeather(ISleekElement button)
		{
			Guid guid;
			WeatherAssetBase weatherAssetBase;
			if (Guid.TryParse(EditorEnvironmentLightingUI.weatherGuidField.Text, ref guid))
			{
				weatherAssetBase = Assets.find<WeatherAssetBase>(new AssetReference<WeatherAssetBase>(guid));
			}
			else
			{
				weatherAssetBase = null;
			}
			WeatherAssetBase activeWeatherAsset = LevelLighting.GetActiveWeatherAsset();
			if (activeWeatherAsset != null && (activeWeatherAsset == weatherAssetBase || activeWeatherAsset.GUID == guid))
			{
				LevelLighting.SetActiveWeatherAsset(null, 0f, default(NetId));
				return;
			}
			LevelLighting.SetActiveWeatherAsset(weatherAssetBase, 0f, default(NetId));
		}

		// Token: 0x06003E57 RID: 15959 RVA: 0x0012FA94 File Offset: 0x0012DC94
		private static void onPickedColorPicker(SleekColorPicker picker, Color state)
		{
			int num = 0;
			while (num < EditorEnvironmentLightingUI.colorPickers.Length && EditorEnvironmentLightingUI.colorPickers[num] != picker)
			{
				num++;
			}
			LevelLighting.times[(int)EditorEnvironmentLightingUI.selectedTime].colors[num] = state;
			LevelLighting.updateLighting();
		}

		// Token: 0x06003E58 RID: 15960 RVA: 0x0012FADC File Offset: 0x0012DCDC
		private static void onDraggedSingleSlider(ISleekSlider slider, float state)
		{
			int num = 0;
			while (num < EditorEnvironmentLightingUI.singleSliders.Length && EditorEnvironmentLightingUI.singleSliders[num] != slider)
			{
				num++;
			}
			LevelLighting.times[(int)EditorEnvironmentLightingUI.selectedTime].singles[num] = state;
			LevelLighting.updateLighting();
		}

		// Token: 0x06003E59 RID: 15961 RVA: 0x0012FB20 File Offset: 0x0012DD20
		private static void updateSelection()
		{
			for (int i = 0; i < EditorEnvironmentLightingUI.colorPickers.Length; i++)
			{
				EditorEnvironmentLightingUI.colorPickers[i].state = LevelLighting.times[(int)EditorEnvironmentLightingUI.selectedTime].colors[i];
			}
			for (int j = 0; j < EditorEnvironmentLightingUI.singleSliders.Length; j++)
			{
				EditorEnvironmentLightingUI.singleSliders[j].Value = LevelLighting.times[(int)EditorEnvironmentLightingUI.selectedTime].singles[j];
			}
		}

		// Token: 0x06003E5A RID: 15962 RVA: 0x0012FB94 File Offset: 0x0012DD94
		public EditorEnvironmentLightingUI()
		{
			Local local = Localization.read("/Editor/EditorEnvironmentLighting.dat");
			EditorEnvironmentLightingUI.container = new SleekFullscreenBox();
			EditorEnvironmentLightingUI.container.PositionOffset_X = 10f;
			EditorEnvironmentLightingUI.container.PositionOffset_Y = 10f;
			EditorEnvironmentLightingUI.container.PositionScale_X = 1f;
			EditorEnvironmentLightingUI.container.SizeOffset_X = -20f;
			EditorEnvironmentLightingUI.container.SizeOffset_Y = -20f;
			EditorEnvironmentLightingUI.container.SizeScale_X = 1f;
			EditorEnvironmentLightingUI.container.SizeScale_Y = 1f;
			EditorUI.window.AddChild(EditorEnvironmentLightingUI.container);
			EditorEnvironmentLightingUI.active = false;
			EditorEnvironmentLightingUI.selectedTime = ELightingTime.DAWN;
			EditorEnvironmentLightingUI.azimuthSlider = Glazier.Get().CreateSlider();
			EditorEnvironmentLightingUI.azimuthSlider.PositionOffset_X = -230f;
			EditorEnvironmentLightingUI.azimuthSlider.PositionOffset_Y = 80f;
			EditorEnvironmentLightingUI.azimuthSlider.PositionScale_X = 1f;
			EditorEnvironmentLightingUI.azimuthSlider.SizeOffset_X = 230f;
			EditorEnvironmentLightingUI.azimuthSlider.SizeOffset_Y = 20f;
			EditorEnvironmentLightingUI.azimuthSlider.Value = LevelLighting.azimuth / 360f;
			EditorEnvironmentLightingUI.azimuthSlider.Orientation = 0;
			EditorEnvironmentLightingUI.azimuthSlider.AddLabel(local.format("AzimuthSliderLabelText"), 0);
			EditorEnvironmentLightingUI.azimuthSlider.OnValueChanged += new Dragged(EditorEnvironmentLightingUI.onDraggedAzimuthSlider);
			EditorEnvironmentLightingUI.container.AddChild(EditorEnvironmentLightingUI.azimuthSlider);
			EditorEnvironmentLightingUI.biasSlider = Glazier.Get().CreateSlider();
			EditorEnvironmentLightingUI.biasSlider.PositionOffset_X = -230f;
			EditorEnvironmentLightingUI.biasSlider.PositionOffset_Y = 110f;
			EditorEnvironmentLightingUI.biasSlider.PositionScale_X = 1f;
			EditorEnvironmentLightingUI.biasSlider.SizeOffset_X = 230f;
			EditorEnvironmentLightingUI.biasSlider.SizeOffset_Y = 20f;
			EditorEnvironmentLightingUI.biasSlider.Value = LevelLighting.bias;
			EditorEnvironmentLightingUI.biasSlider.Orientation = 0;
			EditorEnvironmentLightingUI.biasSlider.AddLabel(local.format("BiasSliderLabelText"), 0);
			EditorEnvironmentLightingUI.biasSlider.OnValueChanged += new Dragged(EditorEnvironmentLightingUI.onDraggedBiasSlider);
			EditorEnvironmentLightingUI.container.AddChild(EditorEnvironmentLightingUI.biasSlider);
			EditorEnvironmentLightingUI.fadeSlider = Glazier.Get().CreateSlider();
			EditorEnvironmentLightingUI.fadeSlider.PositionOffset_X = -230f;
			EditorEnvironmentLightingUI.fadeSlider.PositionOffset_Y = 140f;
			EditorEnvironmentLightingUI.fadeSlider.PositionScale_X = 1f;
			EditorEnvironmentLightingUI.fadeSlider.SizeOffset_X = 230f;
			EditorEnvironmentLightingUI.fadeSlider.SizeOffset_Y = 20f;
			EditorEnvironmentLightingUI.fadeSlider.Value = LevelLighting.fade;
			EditorEnvironmentLightingUI.fadeSlider.Orientation = 0;
			EditorEnvironmentLightingUI.fadeSlider.AddLabel(local.format("FadeSliderLabelText"), 0);
			EditorEnvironmentLightingUI.fadeSlider.OnValueChanged += new Dragged(EditorEnvironmentLightingUI.onDraggedFadeSlider);
			EditorEnvironmentLightingUI.container.AddChild(EditorEnvironmentLightingUI.fadeSlider);
			EditorEnvironmentLightingUI.lightingScrollBox = Glazier.Get().CreateScrollView();
			EditorEnvironmentLightingUI.lightingScrollBox.PositionOffset_X = -470f;
			EditorEnvironmentLightingUI.lightingScrollBox.PositionOffset_Y = 170f;
			EditorEnvironmentLightingUI.lightingScrollBox.PositionScale_X = 1f;
			EditorEnvironmentLightingUI.lightingScrollBox.SizeOffset_X = 470f;
			EditorEnvironmentLightingUI.lightingScrollBox.SizeOffset_Y = -170f;
			EditorEnvironmentLightingUI.lightingScrollBox.SizeScale_Y = 1f;
			EditorEnvironmentLightingUI.lightingScrollBox.ScaleContentToWidth = true;
			EditorEnvironmentLightingUI.container.AddChild(EditorEnvironmentLightingUI.lightingScrollBox);
			EditorEnvironmentLightingUI.seaLevelSlider = new SleekValue();
			EditorEnvironmentLightingUI.seaLevelSlider.PositionOffset_Y = -130f;
			EditorEnvironmentLightingUI.seaLevelSlider.PositionScale_Y = 1f;
			EditorEnvironmentLightingUI.seaLevelSlider.SizeOffset_X = 200f;
			EditorEnvironmentLightingUI.seaLevelSlider.SizeOffset_Y = 30f;
			EditorEnvironmentLightingUI.seaLevelSlider.state = LevelLighting.seaLevel;
			EditorEnvironmentLightingUI.seaLevelSlider.AddLabel(local.format("Sea_Level_Slider_Label"), 1);
			EditorEnvironmentLightingUI.seaLevelSlider.onValued = new Valued(EditorEnvironmentLightingUI.onValuedSeaLevelSlider);
			EditorEnvironmentLightingUI.container.AddChild(EditorEnvironmentLightingUI.seaLevelSlider);
			EditorEnvironmentLightingUI.snowLevelSlider = new SleekValue();
			EditorEnvironmentLightingUI.snowLevelSlider.PositionOffset_Y = -90f;
			EditorEnvironmentLightingUI.snowLevelSlider.PositionScale_Y = 1f;
			EditorEnvironmentLightingUI.snowLevelSlider.SizeOffset_X = 200f;
			EditorEnvironmentLightingUI.snowLevelSlider.SizeOffset_Y = 30f;
			EditorEnvironmentLightingUI.snowLevelSlider.state = LevelLighting.snowLevel;
			EditorEnvironmentLightingUI.snowLevelSlider.AddLabel(local.format("Snow_Level_Slider_Label"), 1);
			EditorEnvironmentLightingUI.snowLevelSlider.onValued = new Valued(EditorEnvironmentLightingUI.onValuedSnowLevelSlider);
			EditorEnvironmentLightingUI.container.AddChild(EditorEnvironmentLightingUI.snowLevelSlider);
			EditorEnvironmentLightingUI.rainFreqField = Glazier.Get().CreateFloat32Field();
			EditorEnvironmentLightingUI.rainFreqField.PositionOffset_Y = -370f;
			EditorEnvironmentLightingUI.rainFreqField.PositionScale_Y = 1f;
			EditorEnvironmentLightingUI.rainFreqField.SizeOffset_X = 100f;
			EditorEnvironmentLightingUI.rainFreqField.SizeOffset_Y = 30f;
			EditorEnvironmentLightingUI.rainFreqField.Value = LevelLighting.rainFreq;
			EditorEnvironmentLightingUI.rainFreqField.AddLabel(local.format("Rain_Freq_Label"), 1);
			EditorEnvironmentLightingUI.rainFreqField.OnValueChanged += new TypedSingle(EditorEnvironmentLightingUI.onTypedRainFreqField);
			EditorEnvironmentLightingUI.container.AddChild(EditorEnvironmentLightingUI.rainFreqField);
			EditorEnvironmentLightingUI.rainDurField = Glazier.Get().CreateFloat32Field();
			EditorEnvironmentLightingUI.rainDurField.PositionOffset_Y = -330f;
			EditorEnvironmentLightingUI.rainDurField.PositionScale_Y = 1f;
			EditorEnvironmentLightingUI.rainDurField.SizeOffset_X = 100f;
			EditorEnvironmentLightingUI.rainDurField.SizeOffset_Y = 30f;
			EditorEnvironmentLightingUI.rainDurField.Value = LevelLighting.rainDur;
			EditorEnvironmentLightingUI.rainDurField.AddLabel(local.format("Rain_Dur_Label"), 1);
			EditorEnvironmentLightingUI.rainDurField.OnValueChanged += new TypedSingle(EditorEnvironmentLightingUI.onTypedRainDurField);
			EditorEnvironmentLightingUI.container.AddChild(EditorEnvironmentLightingUI.rainDurField);
			EditorEnvironmentLightingUI.snowFreqField = Glazier.Get().CreateFloat32Field();
			EditorEnvironmentLightingUI.snowFreqField.PositionOffset_Y = -290f;
			EditorEnvironmentLightingUI.snowFreqField.PositionScale_Y = 1f;
			EditorEnvironmentLightingUI.snowFreqField.SizeOffset_X = 100f;
			EditorEnvironmentLightingUI.snowFreqField.SizeOffset_Y = 30f;
			EditorEnvironmentLightingUI.snowFreqField.Value = LevelLighting.snowFreq;
			EditorEnvironmentLightingUI.snowFreqField.AddLabel(local.format("Snow_Freq_Label"), 1);
			EditorEnvironmentLightingUI.snowFreqField.OnValueChanged += new TypedSingle(EditorEnvironmentLightingUI.onTypedSnowFreqField);
			EditorEnvironmentLightingUI.container.AddChild(EditorEnvironmentLightingUI.snowFreqField);
			EditorEnvironmentLightingUI.snowDurField = Glazier.Get().CreateFloat32Field();
			EditorEnvironmentLightingUI.snowDurField.PositionOffset_Y = -250f;
			EditorEnvironmentLightingUI.snowDurField.PositionScale_Y = 1f;
			EditorEnvironmentLightingUI.snowDurField.SizeOffset_X = 100f;
			EditorEnvironmentLightingUI.snowDurField.SizeOffset_Y = 30f;
			EditorEnvironmentLightingUI.snowDurField.Value = LevelLighting.snowDur;
			EditorEnvironmentLightingUI.snowDurField.AddLabel(local.format("Snow_Dur_Label"), 1);
			EditorEnvironmentLightingUI.snowDurField.OnValueChanged += new TypedSingle(EditorEnvironmentLightingUI.onTypedSnowDurField);
			EditorEnvironmentLightingUI.container.AddChild(EditorEnvironmentLightingUI.snowDurField);
			EditorEnvironmentLightingUI.weatherGuidField = Glazier.Get().CreateStringField();
			EditorEnvironmentLightingUI.weatherGuidField.PositionOffset_Y = -210f;
			EditorEnvironmentLightingUI.weatherGuidField.PositionScale_Y = 1f;
			EditorEnvironmentLightingUI.weatherGuidField.SizeOffset_X = 200f;
			EditorEnvironmentLightingUI.weatherGuidField.SizeOffset_Y = 30f;
			EditorEnvironmentLightingUI.weatherGuidField.MaxLength = 32;
			EditorEnvironmentLightingUI.container.AddChild(EditorEnvironmentLightingUI.weatherGuidField);
			EditorEnvironmentLightingUI.previewWeatherButton = Glazier.Get().CreateButton();
			EditorEnvironmentLightingUI.previewWeatherButton.PositionOffset_X = 210f;
			EditorEnvironmentLightingUI.previewWeatherButton.PositionOffset_Y = -210f;
			EditorEnvironmentLightingUI.previewWeatherButton.PositionScale_Y = 1f;
			EditorEnvironmentLightingUI.previewWeatherButton.SizeOffset_X = 200f;
			EditorEnvironmentLightingUI.previewWeatherButton.SizeOffset_Y = 30f;
			EditorEnvironmentLightingUI.previewWeatherButton.Text = local.format("WeatherPreview_Label");
			EditorEnvironmentLightingUI.previewWeatherButton.OnClicked += new ClickedButton(EditorEnvironmentLightingUI.OnClickedPreviewWeather);
			EditorEnvironmentLightingUI.container.AddChild(EditorEnvironmentLightingUI.previewWeatherButton);
			EditorEnvironmentLightingUI.rainToggle = Glazier.Get().CreateToggle();
			EditorEnvironmentLightingUI.rainToggle.PositionOffset_Y = -175f;
			EditorEnvironmentLightingUI.rainToggle.PositionScale_Y = 1f;
			EditorEnvironmentLightingUI.rainToggle.SizeOffset_X = 40f;
			EditorEnvironmentLightingUI.rainToggle.SizeOffset_Y = 40f;
			EditorEnvironmentLightingUI.rainToggle.Value = LevelLighting.canRain;
			EditorEnvironmentLightingUI.rainToggle.AddLabel(local.format("Rain_Toggle_Label"), 1);
			EditorEnvironmentLightingUI.rainToggle.OnValueChanged += new Toggled(EditorEnvironmentLightingUI.onToggledRainToggle);
			EditorEnvironmentLightingUI.container.AddChild(EditorEnvironmentLightingUI.rainToggle);
			EditorEnvironmentLightingUI.snowToggle = Glazier.Get().CreateToggle();
			EditorEnvironmentLightingUI.snowToggle.PositionOffset_X = 110f;
			EditorEnvironmentLightingUI.snowToggle.PositionOffset_Y = -175f;
			EditorEnvironmentLightingUI.snowToggle.PositionScale_Y = 1f;
			EditorEnvironmentLightingUI.snowToggle.SizeOffset_X = 40f;
			EditorEnvironmentLightingUI.snowToggle.SizeOffset_Y = 40f;
			EditorEnvironmentLightingUI.snowToggle.Value = LevelLighting.canSnow;
			EditorEnvironmentLightingUI.snowToggle.AddLabel(local.format("Snow_Toggle_Label"), 1);
			EditorEnvironmentLightingUI.snowToggle.OnValueChanged += new Toggled(EditorEnvironmentLightingUI.onToggledSnowToggle);
			EditorEnvironmentLightingUI.container.AddChild(EditorEnvironmentLightingUI.snowToggle);
			EditorEnvironmentLightingUI.moonSlider = Glazier.Get().CreateSlider();
			EditorEnvironmentLightingUI.moonSlider.PositionOffset_Y = -50f;
			EditorEnvironmentLightingUI.moonSlider.PositionScale_Y = 1f;
			EditorEnvironmentLightingUI.moonSlider.SizeOffset_X = 200f;
			EditorEnvironmentLightingUI.moonSlider.SizeOffset_Y = 20f;
			EditorEnvironmentLightingUI.moonSlider.Value = (float)LevelLighting.moon / (float)LevelLighting.MOON_CYCLES;
			EditorEnvironmentLightingUI.moonSlider.Orientation = 0;
			EditorEnvironmentLightingUI.moonSlider.AddLabel(local.format("MoonSliderLabelText"), 1);
			EditorEnvironmentLightingUI.moonSlider.OnValueChanged += new Dragged(EditorEnvironmentLightingUI.onDraggedMoonSlider);
			EditorEnvironmentLightingUI.container.AddChild(EditorEnvironmentLightingUI.moonSlider);
			EditorEnvironmentLightingUI.timeSlider = Glazier.Get().CreateSlider();
			EditorEnvironmentLightingUI.timeSlider.PositionOffset_Y = -20f;
			EditorEnvironmentLightingUI.timeSlider.PositionScale_Y = 1f;
			EditorEnvironmentLightingUI.timeSlider.SizeOffset_X = 200f;
			EditorEnvironmentLightingUI.timeSlider.SizeOffset_Y = 20f;
			EditorEnvironmentLightingUI.timeSlider.Value = LevelLighting.time;
			EditorEnvironmentLightingUI.timeSlider.Orientation = 0;
			EditorEnvironmentLightingUI.timeSlider.AddLabel(local.format("TimeSliderLabelText"), 1);
			EditorEnvironmentLightingUI.timeSlider.OnValueChanged += new Dragged(EditorEnvironmentLightingUI.onDraggedTimeSlider);
			EditorEnvironmentLightingUI.container.AddChild(EditorEnvironmentLightingUI.timeSlider);
			EditorEnvironmentLightingUI.timeButtons = new ISleekButton[4];
			for (int i = 0; i < EditorEnvironmentLightingUI.timeButtons.Length; i++)
			{
				ISleekButton sleekButton = Glazier.Get().CreateButton();
				sleekButton.PositionOffset_X = 240f;
				sleekButton.PositionOffset_Y = (float)(i * 40);
				sleekButton.SizeOffset_X = 200f;
				sleekButton.SizeOffset_Y = 30f;
				sleekButton.Text = local.format("Time_" + i.ToString());
				sleekButton.OnClicked += new ClickedButton(EditorEnvironmentLightingUI.onClickedTimeButton);
				EditorEnvironmentLightingUI.lightingScrollBox.AddChild(sleekButton);
				EditorEnvironmentLightingUI.timeButtons[i] = sleekButton;
			}
			EditorEnvironmentLightingUI.infoBoxes = new ISleekBox[12];
			EditorEnvironmentLightingUI.colorPickers = new SleekColorPicker[EditorEnvironmentLightingUI.infoBoxes.Length];
			EditorEnvironmentLightingUI.singleSliders = new ISleekSlider[5];
			for (int j = 0; j < EditorEnvironmentLightingUI.colorPickers.Length; j++)
			{
				ISleekBox sleekBox = Glazier.Get().CreateBox();
				sleekBox.PositionOffset_X = 240f;
				sleekBox.PositionOffset_Y = (float)(EditorEnvironmentLightingUI.timeButtons.Length * 40 + j * 170);
				sleekBox.SizeOffset_X = 200f;
				sleekBox.SizeOffset_Y = 30f;
				sleekBox.Text = local.format("Color_" + j.ToString());
				EditorEnvironmentLightingUI.lightingScrollBox.AddChild(sleekBox);
				EditorEnvironmentLightingUI.infoBoxes[j] = sleekBox;
				SleekColorPicker sleekColorPicker = new SleekColorPicker();
				sleekColorPicker.PositionOffset_X = 200f;
				sleekColorPicker.PositionOffset_Y = (float)(EditorEnvironmentLightingUI.timeButtons.Length * 40 + j * 170 + 40);
				SleekColorPicker sleekColorPicker2 = sleekColorPicker;
				sleekColorPicker2.onColorPicked = (ColorPicked)Delegate.Combine(sleekColorPicker2.onColorPicked, new ColorPicked(EditorEnvironmentLightingUI.onPickedColorPicker));
				EditorEnvironmentLightingUI.lightingScrollBox.AddChild(sleekColorPicker);
				EditorEnvironmentLightingUI.colorPickers[j] = sleekColorPicker;
			}
			for (int k = 0; k < EditorEnvironmentLightingUI.singleSliders.Length; k++)
			{
				ISleekSlider sleekSlider = Glazier.Get().CreateSlider();
				sleekSlider.PositionOffset_X = 240f;
				sleekSlider.PositionOffset_Y = (float)(EditorEnvironmentLightingUI.timeButtons.Length * 40 + EditorEnvironmentLightingUI.colorPickers.Length * 170 + k * 30);
				sleekSlider.SizeOffset_X = 200f;
				sleekSlider.SizeOffset_Y = 20f;
				sleekSlider.Orientation = 0;
				sleekSlider.AddLabel(local.format("Single_" + k.ToString()), 0);
				sleekSlider.OnValueChanged += new Dragged(EditorEnvironmentLightingUI.onDraggedSingleSlider);
				EditorEnvironmentLightingUI.lightingScrollBox.AddChild(sleekSlider);
				EditorEnvironmentLightingUI.singleSliders[k] = sleekSlider;
			}
			EditorEnvironmentLightingUI.lightingScrollBox.ContentSizeOffset = new Vector2(0f, (float)(EditorEnvironmentLightingUI.timeButtons.Length * 40 + EditorEnvironmentLightingUI.colorPickers.Length * 170 + EditorEnvironmentLightingUI.singleSliders.Length * 30 - 10));
			EditorEnvironmentLightingUI.updateSelection();
		}

		// Token: 0x04002723 RID: 10019
		private static SleekFullscreenBox container;

		// Token: 0x04002724 RID: 10020
		public static bool active;

		// Token: 0x04002725 RID: 10021
		private static ISleekScrollView lightingScrollBox;

		// Token: 0x04002726 RID: 10022
		private static ISleekSlider azimuthSlider;

		// Token: 0x04002727 RID: 10023
		private static ISleekSlider biasSlider;

		// Token: 0x04002728 RID: 10024
		private static ISleekSlider fadeSlider;

		// Token: 0x04002729 RID: 10025
		private static ISleekButton[] timeButtons;

		// Token: 0x0400272A RID: 10026
		private static ISleekBox[] infoBoxes;

		// Token: 0x0400272B RID: 10027
		private static SleekColorPicker[] colorPickers;

		// Token: 0x0400272C RID: 10028
		private static ISleekSlider[] singleSliders;

		// Token: 0x0400272D RID: 10029
		private static ELightingTime selectedTime;

		// Token: 0x0400272E RID: 10030
		private static SleekValue seaLevelSlider;

		// Token: 0x0400272F RID: 10031
		private static SleekValue snowLevelSlider;

		// Token: 0x04002730 RID: 10032
		private static ISleekFloat32Field rainFreqField;

		// Token: 0x04002731 RID: 10033
		private static ISleekFloat32Field rainDurField;

		// Token: 0x04002732 RID: 10034
		private static ISleekFloat32Field snowFreqField;

		// Token: 0x04002733 RID: 10035
		private static ISleekFloat32Field snowDurField;

		// Token: 0x04002734 RID: 10036
		private static ISleekToggle rainToggle;

		// Token: 0x04002735 RID: 10037
		private static ISleekToggle snowToggle;

		// Token: 0x04002736 RID: 10038
		private static ISleekField weatherGuidField;

		// Token: 0x04002737 RID: 10039
		private static ISleekButton previewWeatherButton;

		// Token: 0x04002738 RID: 10040
		private static ISleekSlider moonSlider;

		// Token: 0x04002739 RID: 10041
		private static ISleekSlider timeSlider;
	}
}
