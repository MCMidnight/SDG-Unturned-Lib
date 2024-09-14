using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200052F RID: 1327
	public class PackInfo
	{
		// Token: 0x17000849 RID: 2121
		// (get) Token: 0x0600297D RID: 10621 RVA: 0x000B0C34 File Offset: 0x000AEE34
		// (set) Token: 0x0600297E RID: 10622 RVA: 0x000B0C3C File Offset: 0x000AEE3C
		public List<AnimalSpawnpoint> spawns { get; private set; }

		// Token: 0x1700084A RID: 2122
		// (get) Token: 0x0600297F RID: 10623 RVA: 0x000B0C45 File Offset: 0x000AEE45
		// (set) Token: 0x06002980 RID: 10624 RVA: 0x000B0C4D File Offset: 0x000AEE4D
		public List<Animal> animals { get; private set; }

		// Token: 0x1700084B RID: 2123
		// (get) Token: 0x06002981 RID: 10625 RVA: 0x000B0C56 File Offset: 0x000AEE56
		// (set) Token: 0x06002982 RID: 10626 RVA: 0x000B0C5E File Offset: 0x000AEE5E
		public float wanderAngle
		{
			get
			{
				return this._wanderAngle;
			}
			set
			{
				this._wanderAngle = value;
				this.wanderNormal = new Vector3(Mathf.Cos(0.017453292f * this.wanderAngle), 0f, Mathf.Sin(0.017453292f * this.wanderAngle));
			}
		}

		// Token: 0x06002983 RID: 10627 RVA: 0x000B0C99 File Offset: 0x000AEE99
		public Vector3 getWanderDirection()
		{
			return this.wanderNormal;
		}

		// Token: 0x06002984 RID: 10628 RVA: 0x000B0CA4 File Offset: 0x000AEEA4
		public Vector3 getAverageSpawnPoint()
		{
			if (this.avgSpawnPoint == null)
			{
				this.avgSpawnPoint = new Vector3?(Vector3.zero);
				for (int i = 0; i < this.spawns.Count; i++)
				{
					AnimalSpawnpoint animalSpawnpoint = this.spawns[i];
					if (animalSpawnpoint != null)
					{
						this.avgSpawnPoint += animalSpawnpoint.point;
					}
				}
				this.avgSpawnPoint /= (float)this.spawns.Count;
			}
			return this.avgSpawnPoint.Value;
		}

		// Token: 0x06002985 RID: 10629 RVA: 0x000B0D80 File Offset: 0x000AEF80
		public Vector3 getAverageAnimalPoint()
		{
			if (Time.frameCount > this.avgAnimalPointRecalculation)
			{
				this.avgAnimalPoint = Vector3.zero;
				for (int i = 0; i < this.animals.Count; i++)
				{
					Animal animal = this.animals[i];
					if (!(animal == null))
					{
						this.avgAnimalPoint += animal.transform.position;
					}
				}
				this.avgAnimalPoint /= (float)this.animals.Count;
				this.avgAnimalPointRecalculation = Time.frameCount;
			}
			return this.avgAnimalPoint;
		}

		// Token: 0x06002986 RID: 10630 RVA: 0x000B0E1B File Offset: 0x000AF01B
		public PackInfo()
		{
			this.spawns = new List<AnimalSpawnpoint>();
			this.animals = new List<Animal>();
			this.wanderAngle = Random.Range(0f, 360f);
		}

		// Token: 0x04001664 RID: 5732
		private Vector3 wanderNormal;

		// Token: 0x04001665 RID: 5733
		private float _wanderAngle;

		// Token: 0x04001666 RID: 5734
		private Vector3? avgSpawnPoint;

		// Token: 0x04001667 RID: 5735
		private int avgAnimalPointRecalculation;

		// Token: 0x04001668 RID: 5736
		private Vector3 avgAnimalPoint;
	}
}
