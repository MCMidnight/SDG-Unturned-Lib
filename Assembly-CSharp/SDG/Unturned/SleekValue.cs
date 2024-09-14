using System;

namespace SDG.Unturned
{
	// Token: 0x02000731 RID: 1841
	public class SleekValue : SleekWrapper
	{
		// Token: 0x17000B3B RID: 2875
		// (get) Token: 0x06003CA3 RID: 15523 RVA: 0x0012076D File Offset: 0x0011E96D
		// (set) Token: 0x06003CA4 RID: 15524 RVA: 0x00120775 File Offset: 0x0011E975
		public float state
		{
			get
			{
				return this._state;
			}
			set
			{
				this._state = value;
				this.field.Value = this.state;
				this.slider.Value = this.state;
			}
		}

		// Token: 0x06003CA5 RID: 15525 RVA: 0x001207A0 File Offset: 0x0011E9A0
		private void onTypedSingleField(ISleekFloat32Field field, float state)
		{
			Valued valued = this.onValued;
			if (valued != null)
			{
				valued(this, state);
			}
			this._state = state;
			this.slider.Value = state;
		}

		// Token: 0x06003CA6 RID: 15526 RVA: 0x001207C8 File Offset: 0x0011E9C8
		private void onDraggedSlider(ISleekSlider slider, float state)
		{
			Valued valued = this.onValued;
			if (valued != null)
			{
				valued(this, state);
			}
			this._state = state;
			this.field.Value = state;
		}

		// Token: 0x06003CA7 RID: 15527 RVA: 0x001207F0 File Offset: 0x0011E9F0
		public SleekValue()
		{
			this.field = Glazier.Get().CreateFloat32Field();
			this.field.SizeOffset_X = -5f;
			this.field.SizeScale_X = 0.4f;
			this.field.SizeScale_Y = 1f;
			this.field.OnValueChanged += new TypedSingle(this.onTypedSingleField);
			base.AddChild(this.field);
			this.slider = Glazier.Get().CreateSlider();
			this.slider.PositionOffset_X = 5f;
			this.slider.PositionOffset_Y = -10f;
			this.slider.PositionScale_X = 0.4f;
			this.slider.PositionScale_Y = 0.5f;
			this.slider.SizeOffset_X = -5f;
			this.slider.SizeOffset_Y = 20f;
			this.slider.SizeScale_X = 0.6f;
			this.slider.Orientation = 0;
			this.slider.OnValueChanged += new Dragged(this.onDraggedSlider);
			base.AddChild(this.slider);
		}

		// Token: 0x040025F7 RID: 9719
		public Valued onValued;

		// Token: 0x040025F8 RID: 9720
		private float _state;

		// Token: 0x040025F9 RID: 9721
		private ISleekFloat32Field field;

		// Token: 0x040025FA RID: 9722
		private ISleekSlider slider;
	}
}
