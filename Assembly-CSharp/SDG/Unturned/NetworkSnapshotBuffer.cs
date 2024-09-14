using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020005E1 RID: 1505
	public class NetworkSnapshotBuffer<T> where T : ISnapshotInfo<T>
	{
		// Token: 0x170008B2 RID: 2226
		// (get) Token: 0x06003031 RID: 12337 RVA: 0x000D4917 File Offset: 0x000D2B17
		// (set) Token: 0x06003032 RID: 12338 RVA: 0x000D491F File Offset: 0x000D2B1F
		public NetworkSnapshot<T>[] snapshots { get; private set; }

		// Token: 0x06003033 RID: 12339 RVA: 0x000D4928 File Offset: 0x000D2B28
		public T getCurrentSnapshot()
		{
			int num = this.writeCount - this.readCount;
			if (num <= 0)
			{
				this.readLast = Time.realtimeSinceStartup;
				return this.lastInfo;
			}
			if (num > 4)
			{
				if (this.writeIndex == 0)
				{
					this.readIndex = this.snapshots.Length - 1;
				}
				else
				{
					this.readIndex = this.writeIndex - 1;
				}
				this.readCount = this.writeCount - 1;
				this.lastInfo = this.snapshots[this.readIndex].info;
				this.readLast = Time.realtimeSinceStartup;
				return this.lastInfo;
			}
			if (Time.realtimeSinceStartup - this.readLast > this.readDuration && num > 1)
			{
				this.lastInfo = this.snapshots[this.readIndex].info;
				this.readLast += this.readDuration;
				this.incrementReadIndex();
			}
			if (Time.realtimeSinceStartup - this.snapshots[this.readIndex].timestamp < this.readDelay)
			{
				this.readLast = Time.realtimeSinceStartup;
				return this.lastInfo;
			}
			float delta = Mathf.Clamp01((Time.realtimeSinceStartup - this.readLast) / this.readDuration);
			T result;
			this.lastInfo.lerp(this.snapshots[this.readIndex].info, delta, out result);
			return result;
		}

		/// <summary>
		/// Sets the point to lerp from, should be called after resetting position or things like that.
		/// </summary>
		// Token: 0x06003034 RID: 12340 RVA: 0x000D4A88 File Offset: 0x000D2C88
		public void updateLastSnapshot(T info)
		{
			this.readIndex = 0;
			this.readCount = 0;
			this.writeIndex = 0;
			this.writeCount = 0;
			this.lastInfo = info;
			this.readLast = Time.realtimeSinceStartup;
		}

		// Token: 0x06003035 RID: 12341 RVA: 0x000D4AB8 File Offset: 0x000D2CB8
		public void addNewSnapshot(T info)
		{
			this.snapshots[this.writeIndex].info = info;
			this.snapshots[this.writeIndex].timestamp = Time.realtimeSinceStartup;
			this.incrementWriteIndex();
		}

		// Token: 0x06003036 RID: 12342 RVA: 0x000D4AF2 File Offset: 0x000D2CF2
		private void incrementReadIndex()
		{
			this.readIndex++;
			if (this.readIndex == this.snapshots.Length)
			{
				this.readIndex = 0;
			}
			this.readCount++;
		}

		// Token: 0x06003037 RID: 12343 RVA: 0x000D4B27 File Offset: 0x000D2D27
		private void incrementWriteIndex()
		{
			this.writeIndex++;
			if (this.writeIndex == this.snapshots.Length)
			{
				this.writeIndex = 0;
			}
			this.writeCount++;
		}

		// Token: 0x06003038 RID: 12344 RVA: 0x000D4B5C File Offset: 0x000D2D5C
		public NetworkSnapshotBuffer(float newDuration, float newDelay)
		{
			this.snapshots = new NetworkSnapshot<T>[8];
			this.readIndex = 0;
			this.readCount = 0;
			this.writeIndex = 0;
			this.writeCount = 0;
			this.readDuration = newDuration;
			this.readDelay = newDelay;
		}

		// Token: 0x04001A47 RID: 6727
		private int readIndex;

		// Token: 0x04001A48 RID: 6728
		private int readCount;

		// Token: 0x04001A49 RID: 6729
		private int writeIndex;

		// Token: 0x04001A4A RID: 6730
		private int writeCount;

		// Token: 0x04001A4B RID: 6731
		private T lastInfo;

		// Token: 0x04001A4C RID: 6732
		private float readLast;

		// Token: 0x04001A4D RID: 6733
		private float readDuration;

		// Token: 0x04001A4E RID: 6734
		private float readDelay;
	}
}
