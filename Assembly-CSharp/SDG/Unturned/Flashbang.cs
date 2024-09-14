using System;
using System.Collections;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000740 RID: 1856
	public class Flashbang : MonoBehaviour, IExplodableThrowable
	{
		// Token: 0x06003CE7 RID: 15591 RVA: 0x00121F4C File Offset: 0x0012014C
		public void Explode()
		{
			if (this.playAudioSource)
			{
				AudioSource component = base.GetComponent<AudioSource>();
				if (component != null)
				{
					component.Play();
				}
			}
			Light component2 = base.GetComponent<Light>();
			if (component2 != null && !component2.enabled)
			{
				component2.enabled = true;
				base.StartCoroutine(this.DisableLightNextFrame(component2));
			}
			if (MainCamera.instance != null)
			{
				Vector3 a = base.transform.position - MainCamera.instance.transform.position;
				if (a.sqrMagnitude < 1024f)
				{
					float num = Vector3.Dot(a.normalized, MainCamera.instance.transform.forward);
					if (num > -0.25f)
					{
						float magnitude = a.magnitude;
						RaycastHit raycastHit;
						if (magnitude < 0.5f || !Physics.Raycast(new Ray(MainCamera.instance.transform.position, a / magnitude), out raycastHit, magnitude - 0.5f, RayMasks.DAMAGE_SERVER, QueryTriggerInteraction.Ignore))
						{
							float num2;
							if (num > 0.5f)
							{
								num2 = 1f;
							}
							else
							{
								num2 = (num + 0.25f) / 0.75f;
							}
							float num3;
							if (magnitude > 8f)
							{
								num3 = 1f - (magnitude - 8f) / 24f;
							}
							else
							{
								num3 = 1f;
							}
							PlayerUI.stun(this.color, num2 * num3);
						}
					}
				}
			}
			AlertTool.alert(base.transform.position, 32f);
			Object.Destroy(base.gameObject, 2.5f);
		}

		// Token: 0x06003CE8 RID: 15592 RVA: 0x001220CE File Offset: 0x001202CE
		private void Start()
		{
			base.Invoke("Explode", this.fuseLength);
		}

		// Token: 0x06003CE9 RID: 15593 RVA: 0x001220E1 File Offset: 0x001202E1
		private IEnumerator DisableLightNextFrame(Light lightComponent)
		{
			yield return null;
			if (lightComponent != null)
			{
				lightComponent.enabled = false;
			}
			yield break;
		}

		// Token: 0x0400262E RID: 9774
		public Color color = Color.white;

		// Token: 0x0400262F RID: 9775
		public float fuseLength = 2.5f;

		// Token: 0x04002630 RID: 9776
		public bool playAudioSource = true;
	}
}
