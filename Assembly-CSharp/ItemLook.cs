using System;
using UnityEngine;

// Token: 0x0200000E RID: 14
public class ItemLook : MonoBehaviour
{
	// Token: 0x06000030 RID: 48 RVA: 0x00002BE0 File Offset: 0x00000DE0
	private void Update()
	{
		if (this.target == null)
		{
			return;
		}
		Bounds bounds = default(Bounds);
		bool flag = false;
		Collider[] componentsInChildren = this.target.GetComponentsInChildren<Collider>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			Bounds bounds2 = componentsInChildren[i].bounds;
			if (flag)
			{
				bounds.Encapsulate(bounds2);
			}
			else
			{
				bounds = bounds2;
				flag = true;
			}
		}
		Vector3 center = bounds.center;
		float d = bounds.extents.magnitude * 2.25f;
		this._yaw = Mathf.Lerp(this._yaw, this.yaw, 4f * Time.deltaTime);
		this.inspectCamera.transform.rotation = Quaternion.Euler(20f, this._yaw, 0f);
		this.inspectCamera.transform.position = center - this.inspectCamera.transform.forward * d;
	}

	// Token: 0x04000020 RID: 32
	public Camera inspectCamera;

	// Token: 0x04000021 RID: 33
	public float _yaw;

	// Token: 0x04000022 RID: 34
	public float yaw;

	// Token: 0x04000023 RID: 35
	public GameObject target;
}
