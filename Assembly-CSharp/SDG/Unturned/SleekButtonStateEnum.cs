using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000708 RID: 1800
	public class SleekButtonStateEnum<T> : SleekButtonState where T : struct, Enum
	{
		// Token: 0x06003BA6 RID: 15270 RVA: 0x001176C1 File Offset: 0x001158C1
		public T GetEnum()
		{
			return (T)((object)Enum.ToObject(typeof(T), base.state));
		}

		// Token: 0x06003BA7 RID: 15271 RVA: 0x001176DD File Offset: 0x001158DD
		public void SetEnum(T value)
		{
			base.state = Convert.ToInt32(value);
		}

		// Token: 0x06003BA8 RID: 15272 RVA: 0x001176F0 File Offset: 0x001158F0
		protected override void onClickedState(ISleekElement button)
		{
			base.onClickedState(button);
			this.OnSwappedEnum.Invoke(this, this.GetEnum());
		}

		// Token: 0x06003BA9 RID: 15273 RVA: 0x0011770B File Offset: 0x0011590B
		protected override void onRightClickedState(ISleekElement button)
		{
			base.onRightClickedState(button);
			this.OnSwappedEnum.Invoke(this, this.GetEnum());
		}

		// Token: 0x06003BAA RID: 15274 RVA: 0x00117728 File Offset: 0x00115928
		public SleekButtonStateEnum() : base(Array.Empty<GUIContent>())
		{
			string[] names = Enum.GetNames(typeof(T));
			GUIContent[] array = new GUIContent[names.Length];
			for (int i = 0; i < names.Length; i++)
			{
				array[i] = new GUIContent(names[i]);
			}
			base.setContent(array);
		}

		// Token: 0x0400254E RID: 9550
		public Action<SleekButtonStateEnum<T>, T> OnSwappedEnum;
	}
}
