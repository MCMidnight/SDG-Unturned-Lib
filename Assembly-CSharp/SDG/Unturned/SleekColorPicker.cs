using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200070F RID: 1807
	public class SleekColorPicker : SleekWrapper
	{
		// Token: 0x17000B19 RID: 2841
		// (get) Token: 0x06003BC3 RID: 15299 RVA: 0x0011831B File Offset: 0x0011651B
		// (set) Token: 0x06003BC4 RID: 15300 RVA: 0x00118323 File Offset: 0x00116523
		public Color state
		{
			get
			{
				return this.color;
			}
			set
			{
				this.color = value;
				this.updateColor();
				this.updateColorText();
				this.updateColorSlider();
			}
		}

		// Token: 0x06003BC5 RID: 15301 RVA: 0x0011833E File Offset: 0x0011653E
		private void updateColor()
		{
			this.colorImage.TintColor = this.color;
		}

		// Token: 0x06003BC6 RID: 15302 RVA: 0x00118358 File Offset: 0x00116558
		private void updateColorText()
		{
			this.rField.Value = (byte)(this.color.r * 255f);
			this.gField.Value = (byte)(this.color.g * 255f);
			this.bField.Value = (byte)(this.color.b * 255f);
			this.aField.Value = (byte)(this.color.a * 255f);
		}

		// Token: 0x06003BC7 RID: 15303 RVA: 0x001183DC File Offset: 0x001165DC
		private void updateColorSlider()
		{
			this.rSlider.Value = this.color.r;
			this.gSlider.Value = this.color.g;
			this.bSlider.Value = this.color.b;
			this.aSlider.Value = this.color.a;
		}

		// Token: 0x06003BC8 RID: 15304 RVA: 0x00118441 File Offset: 0x00116641
		private void onTypedRField(ISleekUInt8Field field, byte value)
		{
			this.color.r = (float)value / 255f;
			this.updateColor();
			this.updateColorSlider();
			ColorPicked colorPicked = this.onColorPicked;
			if (colorPicked == null)
			{
				return;
			}
			colorPicked(this, this.color);
		}

		// Token: 0x06003BC9 RID: 15305 RVA: 0x00118479 File Offset: 0x00116679
		private void onTypedGField(ISleekUInt8Field field, byte value)
		{
			this.color.g = (float)value / 255f;
			this.updateColor();
			this.updateColorSlider();
			ColorPicked colorPicked = this.onColorPicked;
			if (colorPicked == null)
			{
				return;
			}
			colorPicked(this, this.color);
		}

		// Token: 0x06003BCA RID: 15306 RVA: 0x001184B1 File Offset: 0x001166B1
		private void onTypedBField(ISleekUInt8Field field, byte value)
		{
			this.color.b = (float)value / 255f;
			this.updateColor();
			this.updateColorSlider();
			ColorPicked colorPicked = this.onColorPicked;
			if (colorPicked == null)
			{
				return;
			}
			colorPicked(this, this.color);
		}

		// Token: 0x06003BCB RID: 15307 RVA: 0x001184E9 File Offset: 0x001166E9
		private void onTypedAField(ISleekUInt8Field field, byte value)
		{
			this.color.a = (float)value / 255f;
			this.updateColor();
			this.updateColorSlider();
			ColorPicked colorPicked = this.onColorPicked;
			if (colorPicked == null)
			{
				return;
			}
			colorPicked(this, this.color);
		}

		// Token: 0x06003BCC RID: 15308 RVA: 0x00118521 File Offset: 0x00116721
		private void onDraggedRSlider(ISleekSlider slider, float state)
		{
			this.color.r = state;
			this.updateColor();
			this.updateColorText();
			ColorPicked colorPicked = this.onColorPicked;
			if (colorPicked == null)
			{
				return;
			}
			colorPicked(this, this.color);
		}

		// Token: 0x06003BCD RID: 15309 RVA: 0x00118552 File Offset: 0x00116752
		private void onDraggedGSlider(ISleekSlider slider, float state)
		{
			this.color.g = state;
			this.updateColor();
			this.updateColorText();
			ColorPicked colorPicked = this.onColorPicked;
			if (colorPicked == null)
			{
				return;
			}
			colorPicked(this, this.color);
		}

		// Token: 0x06003BCE RID: 15310 RVA: 0x00118583 File Offset: 0x00116783
		private void onDraggedBSlider(ISleekSlider slider, float state)
		{
			this.color.b = state;
			this.updateColor();
			this.updateColorText();
			ColorPicked colorPicked = this.onColorPicked;
			if (colorPicked == null)
			{
				return;
			}
			colorPicked(this, this.color);
		}

		// Token: 0x06003BCF RID: 15311 RVA: 0x001185B4 File Offset: 0x001167B4
		private void onDraggedASlider(ISleekSlider slider, float state)
		{
			this.color.a = state;
			this.updateColor();
			this.updateColorText();
			ColorPicked colorPicked = this.onColorPicked;
			if (colorPicked == null)
			{
				return;
			}
			colorPicked(this, this.color);
		}

		// Token: 0x06003BD0 RID: 15312 RVA: 0x001185E8 File Offset: 0x001167E8
		public void SetAllowAlpha(bool allowAlpha)
		{
			this.aField.IsVisible = allowAlpha;
			this.aSlider.IsVisible = allowAlpha;
			if (allowAlpha)
			{
				base.SizeOffset_Y = 150f;
				this.rField.SizeOffset_X = 50f;
				this.gField.PositionOffset_X = this.rField.PositionOffset_X + this.rField.SizeOffset_X;
				this.gField.SizeOffset_X = 50f;
				this.bField.PositionOffset_X = this.gField.PositionOffset_X + this.gField.SizeOffset_X;
				this.bField.SizeOffset_X = 50f;
				this.aField.PositionOffset_X = this.bField.PositionOffset_X + this.bField.SizeOffset_X;
				return;
			}
			base.SizeOffset_Y = 120f;
			this.rField.SizeOffset_X = 60f;
			this.gField.PositionOffset_X = this.rField.PositionOffset_X + this.rField.SizeOffset_X + 10f;
			this.gField.SizeOffset_X = 60f;
			this.bField.PositionOffset_X = this.gField.PositionOffset_X + this.gField.SizeOffset_X + 10f;
			this.bField.SizeOffset_X = 60f;
		}

		// Token: 0x06003BD1 RID: 15313 RVA: 0x00118740 File Offset: 0x00116940
		public SleekColorPicker()
		{
			this.color = Color.black;
			base.SizeOffset_X = 240f;
			this.colorImage = Glazier.Get().CreateImage();
			this.colorImage.SizeOffset_X = 30f;
			this.colorImage.SizeOffset_Y = 30f;
			this.colorImage.Texture = GlazierResources.PixelTexture;
			base.AddChild(this.colorImage);
			this.rField = Glazier.Get().CreateUInt8Field();
			this.rField.PositionOffset_X = 40f;
			this.rField.SizeOffset_Y = 30f;
			this.rField.TextColor = Palette.COLOR_R;
			this.rField.OnValueChanged += new TypedByte(this.onTypedRField);
			base.AddChild(this.rField);
			this.gField = Glazier.Get().CreateUInt8Field();
			this.gField.SizeOffset_Y = 30f;
			this.gField.TextColor = Palette.COLOR_G;
			this.gField.OnValueChanged += new TypedByte(this.onTypedGField);
			base.AddChild(this.gField);
			this.bField = Glazier.Get().CreateUInt8Field();
			this.bField.SizeOffset_Y = 30f;
			this.bField.TextColor = Palette.COLOR_B;
			this.bField.OnValueChanged += new TypedByte(this.onTypedBField);
			base.AddChild(this.bField);
			this.aField = Glazier.Get().CreateUInt8Field();
			this.aField.SizeOffset_X = 50f;
			this.aField.SizeOffset_Y = 30f;
			this.aField.TextColor = Palette.COLOR_W;
			this.aField.OnValueChanged += new TypedByte(this.onTypedAField);
			this.aField.IsVisible = false;
			base.AddChild(this.aField);
			this.rSlider = Glazier.Get().CreateSlider();
			this.rSlider.PositionOffset_X = 40f;
			this.rSlider.PositionOffset_Y = 40f;
			this.rSlider.SizeOffset_X = 200f;
			this.rSlider.SizeOffset_Y = 20f;
			this.rSlider.Orientation = 0;
			this.rSlider.AddLabel("R", Palette.COLOR_R, 0);
			this.rSlider.OnValueChanged += new Dragged(this.onDraggedRSlider);
			base.AddChild(this.rSlider);
			this.gSlider = Glazier.Get().CreateSlider();
			this.gSlider.PositionOffset_X = 40f;
			this.gSlider.PositionOffset_Y = 70f;
			this.gSlider.SizeOffset_X = 200f;
			this.gSlider.SizeOffset_Y = 20f;
			this.gSlider.Orientation = 0;
			this.gSlider.AddLabel("G", Palette.COLOR_G, 0);
			this.gSlider.OnValueChanged += new Dragged(this.onDraggedGSlider);
			base.AddChild(this.gSlider);
			this.bSlider = Glazier.Get().CreateSlider();
			this.bSlider.PositionOffset_X = 40f;
			this.bSlider.PositionOffset_Y = 100f;
			this.bSlider.SizeOffset_X = 200f;
			this.bSlider.SizeOffset_Y = 20f;
			this.bSlider.Orientation = 0;
			this.bSlider.AddLabel("B", Palette.COLOR_B, 0);
			this.bSlider.OnValueChanged += new Dragged(this.onDraggedBSlider);
			base.AddChild(this.bSlider);
			this.aSlider = Glazier.Get().CreateSlider();
			this.aSlider.PositionOffset_X = 40f;
			this.aSlider.PositionOffset_Y = 130f;
			this.aSlider.SizeOffset_X = 200f;
			this.aSlider.SizeOffset_Y = 20f;
			this.aSlider.Orientation = 0;
			this.aSlider.AddLabel("A", Palette.COLOR_W, 0);
			this.aSlider.OnValueChanged += new Dragged(this.onDraggedASlider);
			this.aSlider.IsVisible = false;
			base.AddChild(this.aSlider);
			this.SetAllowAlpha(false);
		}

		// Token: 0x04002563 RID: 9571
		public ColorPicked onColorPicked;

		// Token: 0x04002564 RID: 9572
		private ISleekImage colorImage;

		// Token: 0x04002565 RID: 9573
		private ISleekUInt8Field rField;

		// Token: 0x04002566 RID: 9574
		private ISleekUInt8Field gField;

		// Token: 0x04002567 RID: 9575
		private ISleekUInt8Field bField;

		// Token: 0x04002568 RID: 9576
		private ISleekUInt8Field aField;

		// Token: 0x04002569 RID: 9577
		private ISleekSlider rSlider;

		// Token: 0x0400256A RID: 9578
		private ISleekSlider gSlider;

		// Token: 0x0400256B RID: 9579
		private ISleekSlider bSlider;

		// Token: 0x0400256C RID: 9580
		private ISleekSlider aSlider;

		// Token: 0x0400256D RID: 9581
		private Color color;
	}
}
