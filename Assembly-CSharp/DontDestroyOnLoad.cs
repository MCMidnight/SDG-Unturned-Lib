using System;
using UnityEngine;

// Token: 0x02000003 RID: 3
public class DontDestroyOnLoad : MonoBehaviour
{
	// Token: 0x06000003 RID: 3 RVA: 0x00002065 File Offset: 0x00000265
	private void OnEnable()
	{
		Object.DontDestroyOnLoad(base.gameObject);
	}
}
