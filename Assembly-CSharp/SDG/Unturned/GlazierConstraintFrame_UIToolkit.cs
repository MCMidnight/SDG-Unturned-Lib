using System;
using UnityEngine.UIElements;

namespace SDG.Unturned
{
	/// <summary>
	/// UITK implementation consists of a container element which respects the regular position and size
	/// properties, and a child content element which fits itself in the container.
	/// </summary>
	// Token: 0x02000196 RID: 406
	internal class GlazierConstraintFrame_UIToolkit : GlazierElementBase_UIToolkit, ISleekConstraintFrame, ISleekElement
	{
		// Token: 0x170001F7 RID: 503
		// (get) Token: 0x06000C09 RID: 3081 RVA: 0x00028965 File Offset: 0x00026B65
		// (set) Token: 0x06000C0A RID: 3082 RVA: 0x00028972 File Offset: 0x00026B72
		public ESleekConstraint Constraint
		{
			get
			{
				return this.contentElement.constraint;
			}
			set
			{
				if (this.contentElement.constraint != null)
				{
					throw new NotSupportedException();
				}
				this.contentElement.constraint = value;
			}
		}

		// Token: 0x170001F8 RID: 504
		// (get) Token: 0x06000C0B RID: 3083 RVA: 0x00028993 File Offset: 0x00026B93
		// (set) Token: 0x06000C0C RID: 3084 RVA: 0x000289A0 File Offset: 0x00026BA0
		public float AspectRatio
		{
			get
			{
				return this.contentElement.aspectRatio;
			}
			set
			{
				this.contentElement.aspectRatio = value;
			}
		}

		// Token: 0x06000C0D RID: 3085 RVA: 0x000289B0 File Offset: 0x00026BB0
		public GlazierConstraintFrame_UIToolkit(Glazier_UIToolkit glazier) : base(glazier)
		{
			this.containerElement = new ConstraintFrameParentElement();
			this.containerElement.pickingMode = 1;
			this.containerElement.userData = this;
			this.containerElement.AddToClassList("unturned-constraint-frame-container");
			this.containerElement._contentContainerOverride = this.containerElement;
			this.contentElement = new ConstraintFrameChildElement();
			this.contentElement.pickingMode = 1;
			this.contentElement.userData = this;
			this.contentElement.AddToClassList("unturned-constraint-frame-content");
			this.containerElement.Add(this.contentElement);
			this.containerElement.RegisterCallback<GeometryChangedEvent>(new EventCallback<GeometryChangedEvent>(this.contentElement.OnParentGeometryChanged), 0);
			this.containerElement._contentContainerOverride = this.contentElement;
			this.visualElement = this.containerElement;
		}

		// Token: 0x0400049A RID: 1178
		private ConstraintFrameParentElement containerElement;

		// Token: 0x0400049B RID: 1179
		private ConstraintFrameChildElement contentElement;
	}
}
