using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000418 RID: 1048
	public class ReunObjectTransform : IReun
	{
		// Token: 0x17000654 RID: 1620
		// (get) Token: 0x06001ECB RID: 7883 RVA: 0x000726D3 File Offset: 0x000708D3
		// (set) Token: 0x06001ECC RID: 7884 RVA: 0x000726DB File Offset: 0x000708DB
		public int step { get; private set; }

		// Token: 0x06001ECD RID: 7885 RVA: 0x000726E4 File Offset: 0x000708E4
		public Transform redo()
		{
			if (this.model != null)
			{
				LevelObjects.transformObject(this.model, this.toPosition, this.toRotation, this.toScale, this.fromPosition, this.fromRotation, this.fromScale);
			}
			return this.model;
		}

		// Token: 0x06001ECE RID: 7886 RVA: 0x00072734 File Offset: 0x00070934
		public void undo()
		{
			if (this.model != null)
			{
				LevelObjects.transformObject(this.model, this.fromPosition, this.fromRotation, this.fromScale, this.toPosition, this.toRotation, this.toScale);
			}
		}

		// Token: 0x06001ECF RID: 7887 RVA: 0x00072774 File Offset: 0x00070974
		public ReunObjectTransform(int newStep, Transform newModel, Vector3 newFromPosition, Quaternion newFromRotation, Vector3 newFromScale, Vector3 newToPosition, Quaternion newToRotation, Vector3 newToScale)
		{
			this.step = newStep;
			this.model = newModel;
			this.fromPosition = newFromPosition;
			this.fromRotation = newFromRotation;
			this.fromScale = newFromScale;
			this.toPosition = newToPosition;
			this.toRotation = newToRotation;
			this.toScale = newToScale;
		}

		// Token: 0x04000F30 RID: 3888
		private Transform model;

		// Token: 0x04000F31 RID: 3889
		private Vector3 fromPosition;

		// Token: 0x04000F32 RID: 3890
		private Quaternion fromRotation;

		// Token: 0x04000F33 RID: 3891
		private Vector3 fromScale;

		// Token: 0x04000F34 RID: 3892
		private Vector3 toPosition;

		// Token: 0x04000F35 RID: 3893
		private Quaternion toRotation;

		// Token: 0x04000F36 RID: 3894
		private Vector3 toScale;
	}
}
