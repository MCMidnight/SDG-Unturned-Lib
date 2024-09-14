using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200050A RID: 1290
	public class Road
	{
		/// <summary>
		/// Only set in play mode for determing if we should cache brute force lengths.
		/// </summary>
		// Token: 0x17000827 RID: 2087
		// (get) Token: 0x06002867 RID: 10343 RVA: 0x000AA9F4 File Offset: 0x000A8BF4
		// (set) Token: 0x06002868 RID: 10344 RVA: 0x000AA9FC File Offset: 0x000A8BFC
		public ushort roadIndex { get; protected set; }

		// Token: 0x17000828 RID: 2088
		// (get) Token: 0x06002869 RID: 10345 RVA: 0x000AAA05 File Offset: 0x000A8C05
		public Transform road
		{
			get
			{
				return this._road;
			}
		}

		// Token: 0x17000829 RID: 2089
		// (get) Token: 0x0600286A RID: 10346 RVA: 0x000AAA0D File Offset: 0x000A8C0D
		// (set) Token: 0x0600286B RID: 10347 RVA: 0x000AAA15 File Offset: 0x000A8C15
		public bool isLoop
		{
			get
			{
				return this._isLoop;
			}
			set
			{
				this._isLoop = value;
				this.updatePoints();
			}
		}

		// Token: 0x1700082A RID: 2090
		// (get) Token: 0x0600286C RID: 10348 RVA: 0x000AAA24 File Offset: 0x000A8C24
		public List<RoadJoint> joints
		{
			get
			{
				return this._joints;
			}
		}

		// Token: 0x1700082B RID: 2091
		// (get) Token: 0x0600286D RID: 10349 RVA: 0x000AAA2C File Offset: 0x000A8C2C
		// (set) Token: 0x0600286E RID: 10350 RVA: 0x000AAA34 File Offset: 0x000A8C34
		public float trackSampledLength { get; protected set; }

		// Token: 0x1700082C RID: 2092
		// (get) Token: 0x0600286F RID: 10351 RVA: 0x000AAA3D File Offset: 0x000A8C3D
		public List<RoadPath> paths
		{
			get
			{
				return this._paths;
			}
		}

		// Token: 0x06002870 RID: 10352 RVA: 0x000AAA48 File Offset: 0x000A8C48
		public void setEnabled(bool isEnabled)
		{
			this.line.gameObject.SetActive(isEnabled);
			for (int i = 0; i < this.paths.Count; i++)
			{
				this.paths[i].vertex.gameObject.SetActive(isEnabled);
			}
		}

		// Token: 0x06002871 RID: 10353 RVA: 0x000AAA98 File Offset: 0x000A8C98
		public void getTrackData(float trackPosition, out Vector3 position, out Vector3 normal, out Vector3 direction)
		{
			if (this.trackSamples.Count > 1)
			{
				TrackSample trackSample = this.trackSamples[0];
				for (int i = 1; i < this.trackSamples.Count; i++)
				{
					TrackSample trackSample2 = this.trackSamples[i];
					if (trackPosition >= trackSample.distance && trackPosition <= trackSample2.distance)
					{
						float t = (trackPosition - trackSample.distance) / (trackSample2.distance - trackSample.distance);
						position = Vector3.Lerp(trackSample.position, trackSample2.position, t);
						normal = Vector3.Lerp(trackSample.normal, trackSample2.normal, t);
						direction = Vector3.Lerp(trackSample.direction, trackSample2.direction, t);
						return;
					}
					trackSample = trackSample2;
				}
				if (this.isLoop)
				{
					TrackSample trackSample3 = this.trackSamples[0];
					if (trackSample != trackSample3)
					{
						float t2 = (trackPosition - trackSample.distance) / (this.trackSampledLength - trackSample.distance);
						position = Vector3.Lerp(trackSample.position, trackSample3.position, t2);
						normal = Vector3.Lerp(trackSample.normal, trackSample3.normal, t2);
						direction = Vector3.Lerp(trackSample.direction, trackSample3.direction, t2);
						return;
					}
				}
			}
			position = Vector3.zero;
			normal = Vector3.up;
			direction = Vector3.forward;
		}

		// Token: 0x06002872 RID: 10354 RVA: 0x000AAC08 File Offset: 0x000A8E08
		public void getTrackPosition(int index, float t, out Vector3 position, out Vector3 normal)
		{
			position = this.getPosition(index, t);
			normal = Vector3.up;
			if (!this.joints[index].ignoreTerrain)
			{
				position.y = LevelGround.getHeight(position);
				normal = LevelGround.getNormal(position);
			}
			position += normal * (LevelRoads.materials[(int)this.material].depth + LevelRoads.materials[(int)this.material].offset);
		}

		// Token: 0x06002873 RID: 10355 RVA: 0x000AACA8 File Offset: 0x000A8EA8
		public void getTrackPosition(float t, out int index, out Vector3 position, out Vector3 normal)
		{
			position = this.getPosition(t, out index);
			normal = Vector3.up;
			if (!this.joints[index].ignoreTerrain)
			{
				position.y = LevelGround.getHeight(position);
				normal = LevelGround.getNormal(position);
			}
			position += normal * (LevelRoads.materials[(int)this.material].depth + LevelRoads.materials[(int)this.material].offset);
		}

		// Token: 0x06002874 RID: 10356 RVA: 0x000AAD48 File Offset: 0x000A8F48
		public Vector3 getPosition(float t)
		{
			int num;
			return this.getPosition(t, out num);
		}

		// Token: 0x06002875 RID: 10357 RVA: 0x000AAD60 File Offset: 0x000A8F60
		public Vector3 getPosition(float t, out int index)
		{
			if (this.isLoop)
			{
				index = (int)(t * (float)this.joints.Count);
				t = t * (float)this.joints.Count - (float)index;
				return this.getPosition(index, t);
			}
			index = (int)(t * (float)(this.joints.Count - 1));
			t = t * (float)(this.joints.Count - 1) - (float)index;
			return this.getPosition(index, t);
		}

		// Token: 0x06002876 RID: 10358 RVA: 0x000AADD8 File Offset: 0x000A8FD8
		public Vector3 getPosition(int index, float t)
		{
			index = Mathf.Clamp(index, 0, this.joints.Count - 1);
			t = Mathf.Clamp01(t);
			RoadJoint roadJoint = this.joints[index];
			RoadJoint roadJoint2;
			if (index == this.joints.Count - 1)
			{
				roadJoint2 = this.joints[0];
			}
			else
			{
				roadJoint2 = this.joints[index + 1];
			}
			Vector3 tangent = roadJoint.getTangent(1);
			Vector3 tangent2 = roadJoint2.getTangent(0);
			if (Vector3.Dot(tangent.normalized, tangent2.normalized) < -0.999f)
			{
				return Vector3.Lerp(roadJoint.vertex, roadJoint2.vertex, t);
			}
			return BezierTool.getPosition(roadJoint.vertex, roadJoint.vertex + tangent, roadJoint2.vertex + tangent2, roadJoint2.vertex, t);
		}

		// Token: 0x06002877 RID: 10359 RVA: 0x000AAEA8 File Offset: 0x000A90A8
		public Vector3 getVelocity(float t)
		{
			if (this.isLoop)
			{
				int num = (int)(t * (float)this.joints.Count);
				t = t * (float)this.joints.Count - (float)num;
				return this.getVelocity(num, t);
			}
			int num2 = (int)(t * (float)(this.joints.Count - 1));
			t = t * (float)(this.joints.Count - 1) - (float)num2;
			return this.getVelocity(num2, t);
		}

		// Token: 0x06002878 RID: 10360 RVA: 0x000AAF18 File Offset: 0x000A9118
		public Vector3 getVelocity(int index, float t)
		{
			index = Mathf.Clamp(index, 0, this.joints.Count - 1);
			t = Mathf.Clamp01(t);
			RoadJoint roadJoint = this.joints[index];
			RoadJoint roadJoint2;
			if (index == this.joints.Count - 1)
			{
				roadJoint2 = this.joints[0];
			}
			else
			{
				roadJoint2 = this.joints[index + 1];
			}
			return BezierTool.getVelocity(roadJoint.vertex, roadJoint.vertex + roadJoint.getTangent(1), roadJoint2.vertex + roadJoint2.getTangent(0), roadJoint2.vertex, t);
		}

		// Token: 0x06002879 RID: 10361 RVA: 0x000AAFB4 File Offset: 0x000A91B4
		public float getLengthEstimate()
		{
			double num = 0.0;
			for (int i = 0; i < this.joints.Count - 1 + (this.isLoop ? 1 : 0); i++)
			{
				num += (double)this.getLengthEstimate(i);
			}
			return (float)num;
		}

		// Token: 0x0600287A RID: 10362 RVA: 0x000AB000 File Offset: 0x000A9200
		public float getLengthEstimate(int index)
		{
			index = Mathf.Clamp(index, 0, this.joints.Count - 1);
			RoadJoint roadJoint = this.joints[index];
			RoadJoint roadJoint2;
			if (index == this.joints.Count - 1)
			{
				roadJoint2 = this.joints[0];
			}
			else
			{
				roadJoint2 = this.joints[index + 1];
			}
			Vector3 tangent = roadJoint.getTangent(1);
			Vector3 tangent2 = roadJoint2.getTangent(0);
			if (Vector3.Dot(tangent.normalized, tangent2.normalized) < -0.999f)
			{
				return (roadJoint2.vertex - roadJoint.vertex).magnitude;
			}
			return BezierTool.getLengthEstimate(roadJoint.vertex, roadJoint.vertex + tangent, roadJoint2.vertex + tangent2, roadJoint2.vertex);
		}

		// Token: 0x0600287B RID: 10363 RVA: 0x000AB0CC File Offset: 0x000A92CC
		[Obsolete]
		public Transform addPoint(Transform origin, Vector3 point)
		{
			RoadJoint roadJoint = new RoadJoint(point);
			if (origin == null || origin == this.paths[this.paths.Count - 1].vertex)
			{
				if (this.joints.Count > 0)
				{
					roadJoint.setTangent(0, (this.joints[this.joints.Count - 1].vertex - point).normalized * 2.5f);
				}
				this.joints.Add(roadJoint);
				Transform transform = ((GameObject)Object.Instantiate(Resources.Load("Edit/Path"))).transform;
				transform.name = "Path_" + (this.joints.Count - 1).ToString();
				transform.parent = this.line;
				RoadPath roadPath = new RoadPath(transform);
				this.paths.Add(roadPath);
				this.updatePoints();
				return roadPath.vertex;
			}
			if (origin == this.paths[0].vertex)
			{
				for (int i = 0; i < this.joints.Count; i++)
				{
					this.paths[i].vertex.name = "Path_" + (i + 1).ToString();
				}
				if (this.joints.Count > 0)
				{
					roadJoint.setTangent(1, (this.joints[0].vertex - point).normalized * 2.5f);
				}
				this.joints.Insert(0, roadJoint);
				Transform transform2 = ((GameObject)Object.Instantiate(Resources.Load("Edit/Path"))).transform;
				transform2.name = "Path_0";
				transform2.parent = this.line;
				RoadPath roadPath2 = new RoadPath(transform2);
				this.paths.Insert(0, roadPath2);
				this.updatePoints();
				return roadPath2.vertex;
			}
			return null;
		}

		// Token: 0x0600287C RID: 10364 RVA: 0x000AB2D4 File Offset: 0x000A94D4
		public Transform addVertex(int vertexIndex, Vector3 point)
		{
			RoadJoint roadJoint = new RoadJoint(point);
			for (int i = vertexIndex; i < this.joints.Count; i++)
			{
				this.paths[i].vertex.name = "Path_" + (i + 1).ToString();
			}
			if (this.joints.Count == 1)
			{
				this.joints[0].setTangent(1, (point - this.joints[0].vertex).normalized * 2.5f);
				roadJoint.setTangent(0, (this.joints[0].vertex - point).normalized * 2.5f);
			}
			else if (this.joints.Count > 1)
			{
				if (vertexIndex == 0)
				{
					if (this.isLoop)
					{
						RoadJoint roadJoint2 = this.joints[this.joints.Count - 1];
						RoadJoint roadJoint3 = this.joints[0];
						roadJoint.setTangent(1, (roadJoint3.vertex - roadJoint2.vertex).normalized * 2.5f);
					}
					else
					{
						roadJoint.setTangent(1, (this.joints[0].vertex - point).normalized * 2.5f);
					}
				}
				else if (vertexIndex == this.joints.Count)
				{
					if (this.isLoop)
					{
						RoadJoint roadJoint4 = this.joints[this.joints.Count - 1];
						RoadJoint roadJoint5 = this.joints[0];
						roadJoint.setTangent(1, (roadJoint5.vertex - roadJoint4.vertex).normalized * 2.5f);
					}
					else
					{
						roadJoint.setTangent(0, (this.joints[this.joints.Count - 1].vertex - point).normalized * 2.5f);
					}
				}
				else
				{
					RoadJoint roadJoint6 = this.joints[vertexIndex - 1];
					RoadJoint roadJoint7 = this.joints[vertexIndex];
					roadJoint.setTangent(1, (roadJoint7.vertex - roadJoint6.vertex).normalized * 2.5f);
				}
			}
			this.joints.Insert(vertexIndex, roadJoint);
			Transform transform = ((GameObject)Object.Instantiate(Resources.Load("Edit/Path"))).transform;
			transform.name = "Path_" + vertexIndex.ToString();
			transform.parent = this.line;
			RoadPath roadPath = new RoadPath(transform);
			this.paths.Insert(vertexIndex, roadPath);
			this.updatePoints();
			return roadPath.vertex;
		}

		// Token: 0x0600287D RID: 10365 RVA: 0x000AB5C0 File Offset: 0x000A97C0
		[Obsolete]
		public void removePoint(Transform select)
		{
			if (this.joints.Count < 2)
			{
				LevelRoads.removeRoad(this);
				return;
			}
			for (int i = 0; i < this.paths.Count; i++)
			{
				if (this.paths[i].vertex == select)
				{
					for (int j = i + 1; j < this.paths.Count; j++)
					{
						this.paths[j].vertex.name = "Path_" + (j - 1).ToString();
					}
					Object.Destroy(select.gameObject);
					this.joints.RemoveAt(i);
					this.paths.RemoveAt(i);
					this.updatePoints();
					return;
				}
			}
		}

		// Token: 0x0600287E RID: 10366 RVA: 0x000AB684 File Offset: 0x000A9884
		public void removeVertex(int vertexIndex)
		{
			if (this.joints.Count < 2)
			{
				LevelRoads.removeRoad(this);
				return;
			}
			for (int i = vertexIndex + 1; i < this.paths.Count; i++)
			{
				this.paths[i].vertex.name = "Path_" + (i - 1).ToString();
			}
			this.paths[vertexIndex].remove();
			this.paths.RemoveAt(vertexIndex);
			this.joints.RemoveAt(vertexIndex);
			this.updatePoints();
		}

		// Token: 0x0600287F RID: 10367 RVA: 0x000AB718 File Offset: 0x000A9918
		public void remove()
		{
			Object.Destroy(this.road.gameObject);
			Object.Destroy(this.line.gameObject);
		}

		// Token: 0x06002880 RID: 10368 RVA: 0x000AB73C File Offset: 0x000A993C
		[Obsolete]
		public void movePoint(Transform select, Vector3 point)
		{
			for (int i = 0; i < this.paths.Count; i++)
			{
				if (this.paths[i].vertex == select)
				{
					this.joints[i].vertex = point;
					this.updatePoints();
					return;
				}
			}
		}

		// Token: 0x06002881 RID: 10369 RVA: 0x000AB791 File Offset: 0x000A9991
		public void moveVertex(int vertexIndex, Vector3 point)
		{
			this.joints[vertexIndex].vertex = point;
			this.updatePoints();
		}

		// Token: 0x06002882 RID: 10370 RVA: 0x000AB7AB File Offset: 0x000A99AB
		public void moveTangent(int vertexIndex, int tangentIndex, Vector3 point)
		{
			this.joints[vertexIndex].setTangent(tangentIndex, point);
			this.updatePoints();
		}

		// Token: 0x06002883 RID: 10371 RVA: 0x000AB7C8 File Offset: 0x000A99C8
		public void buildMesh()
		{
			for (int i = 0; i < this.road.childCount; i++)
			{
				Object.Destroy(this.road.GetChild(i).gameObject);
			}
			if (this.joints.Count < 2)
			{
				return;
			}
			this.updateSamples();
			if (!Level.isEditor)
			{
				bool flag = false;
				using (List<LevelTrainAssociation>.Enumerator enumerator = Level.info.configData.Trains.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.RoadIndex == this.roadIndex)
						{
							flag = true;
							break;
						}
					}
				}
				if (flag)
				{
					this.updateTrackSamples();
				}
			}
			Vector3[] array = new Vector3[this.samples.Count * 4 + (this.isLoop ? 0 : 8)];
			Vector3[] array2 = new Vector3[this.samples.Count * 4 + (this.isLoop ? 0 : 8)];
			float num = 0f;
			Vector3 b = Vector3.zero;
			Vector3 vector = Vector3.zero;
			Vector3 vector2 = Vector3.zero;
			Vector3 vector3 = Vector3.zero;
			Vector3 a = Vector3.zero;
			Vector2 zero = Vector2.zero;
			int j;
			for (j = 0; j < this.samples.Count; j++)
			{
				RoadSample roadSample = this.samples[j];
				RoadJoint roadJoint = this.joints[roadSample.index];
				vector = this.getPosition(roadSample.index, roadSample.time);
				if (!roadJoint.ignoreTerrain)
				{
					vector.y = LevelGround.getHeight(vector);
				}
				vector2 = this.getVelocity(roadSample.index, roadSample.time).normalized;
				if (roadJoint.ignoreTerrain)
				{
					vector3 = Vector3.up;
				}
				else
				{
					vector3 = LevelGround.getNormal(vector);
				}
				a = Vector3.Cross(vector2, vector3).normalized;
				if (!roadJoint.ignoreTerrain)
				{
					Vector3 vector4 = vector + a * LevelRoads.materials[(int)this.material].width;
					float num2 = LevelGround.getHeight(vector4) - vector4.y;
					if (num2 > 0f)
					{
						vector.y += num2;
					}
					Vector3 vector5 = vector - a * LevelRoads.materials[(int)this.material].width;
					float num3 = LevelGround.getHeight(vector5) - vector5.y;
					if (num3 > 0f)
					{
						vector.y += num3;
					}
				}
				if (roadSample.index < this.joints.Count - 1)
				{
					vector.y += Mathf.Lerp(roadJoint.offset, this.joints[roadSample.index + 1].offset, roadSample.time);
				}
				else if (this.isLoop)
				{
					vector.y += Mathf.Lerp(roadJoint.offset, this.joints[0].offset, roadSample.time);
				}
				else
				{
					vector.y += roadJoint.offset;
				}
				array[(this.isLoop ? 0 : 4) + j * 4] = vector + a * (LevelRoads.materials[(int)this.material].width + LevelRoads.materials[(int)this.material].depth * 2f) - vector3 * LevelRoads.materials[(int)this.material].depth + vector3 * LevelRoads.materials[(int)this.material].offset;
				array[(this.isLoop ? 0 : 4) + j * 4 + 1] = vector + a * LevelRoads.materials[(int)this.material].width + vector3 * LevelRoads.materials[(int)this.material].depth + vector3 * LevelRoads.materials[(int)this.material].offset;
				array[(this.isLoop ? 0 : 4) + j * 4 + 2] = vector - a * LevelRoads.materials[(int)this.material].width + vector3 * LevelRoads.materials[(int)this.material].depth + vector3 * LevelRoads.materials[(int)this.material].offset;
				array[(this.isLoop ? 0 : 4) + j * 4 + 3] = vector - a * (LevelRoads.materials[(int)this.material].width + LevelRoads.materials[(int)this.material].depth * 2f) - vector3 * LevelRoads.materials[(int)this.material].depth + vector3 * LevelRoads.materials[(int)this.material].offset;
				array2[(this.isLoop ? 0 : 4) + j * 4] = vector3;
				array2[(this.isLoop ? 0 : 4) + j * 4 + 1] = vector3;
				array2[(this.isLoop ? 0 : 4) + j * 4 + 2] = vector3;
				array2[(this.isLoop ? 0 : 4) + j * 4 + 3] = vector3;
				if (j == 0)
				{
					if (!this.isLoop)
					{
						array[j * 4] = vector + a * (LevelRoads.materials[(int)this.material].width + LevelRoads.materials[(int)this.material].depth * 2f) - vector3 * LevelRoads.materials[(int)this.material].depth + vector3 * LevelRoads.materials[(int)this.material].offset - vector2 * LevelRoads.materials[(int)this.material].depth * 4f;
						array[j * 4 + 1] = vector + a * LevelRoads.materials[(int)this.material].width - vector3 * LevelRoads.materials[(int)this.material].depth + vector3 * LevelRoads.materials[(int)this.material].offset - vector2 * LevelRoads.materials[(int)this.material].depth * 4f;
						array[j * 4 + 2] = vector - a * LevelRoads.materials[(int)this.material].width - vector3 * LevelRoads.materials[(int)this.material].depth + vector3 * LevelRoads.materials[(int)this.material].offset - vector2 * LevelRoads.materials[(int)this.material].depth * 4f;
						array[j * 4 + 3] = vector - a * (LevelRoads.materials[(int)this.material].width + LevelRoads.materials[(int)this.material].depth * 2f) - vector3 * LevelRoads.materials[(int)this.material].depth + vector3 * LevelRoads.materials[(int)this.material].offset - vector2 * LevelRoads.materials[(int)this.material].depth * 4f;
						array2[j * 4] = vector3;
						array2[j * 4 + 1] = vector3;
						array2[j * 4 + 2] = vector3;
						array2[j * 4 + 3] = vector3;
					}
					b = vector;
				}
				else
				{
					num += (vector - b).magnitude;
					b = vector;
				}
			}
			if (!this.isLoop)
			{
				array[4 + j * 4] = vector + a * (LevelRoads.materials[(int)this.material].width + LevelRoads.materials[(int)this.material].depth * 2f) - vector3 * LevelRoads.materials[(int)this.material].depth + vector3 * LevelRoads.materials[(int)this.material].offset + vector2 * LevelRoads.materials[(int)this.material].depth * 4f;
				array[4 + j * 4 + 1] = vector + a * LevelRoads.materials[(int)this.material].width - vector3 * LevelRoads.materials[(int)this.material].depth + vector3 * LevelRoads.materials[(int)this.material].offset + vector2 * LevelRoads.materials[(int)this.material].depth * 4f;
				array[4 + j * 4 + 2] = vector - a * LevelRoads.materials[(int)this.material].width - vector3 * LevelRoads.materials[(int)this.material].depth + vector3 * LevelRoads.materials[(int)this.material].offset + vector2 * LevelRoads.materials[(int)this.material].depth * 4f;
				array[4 + j * 4 + 3] = vector - a * (LevelRoads.materials[(int)this.material].width + LevelRoads.materials[(int)this.material].depth * 2f) - vector3 * LevelRoads.materials[(int)this.material].depth + vector3 * LevelRoads.materials[(int)this.material].offset + vector2 * LevelRoads.materials[(int)this.material].depth * 4f;
				array2[4 + j * 4] = vector3;
				array2[4 + j * 4 + 1] = vector3;
				array2[4 + j * 4 + 2] = vector3;
				array2[4 + j * 4 + 3] = vector3;
			}
			int num4 = 0;
			for (int k = 0; k < this.samples.Count; k += 20)
			{
				int num5 = Mathf.Min(k + 20, this.samples.Count - 1);
				int num6 = num5 - k + 1;
				if (!this.isLoop)
				{
					if (k == 0)
					{
						num6++;
					}
					if (num5 == this.samples.Count - 1)
					{
						num6++;
					}
				}
				Vector3[] array3 = new Vector3[num6 * 4];
				Vector3[] array4 = new Vector3[num6 * 4];
				Vector2[] uv = null;
				int[] array5 = new int[num6 * 18];
				int num7 = k;
				if (!this.isLoop && k > 0)
				{
					num7++;
				}
				Array.Copy(array, num7 * 4, array3, 0, array3.Length);
				Array.Copy(array2, num7 * 4, array4, 0, array3.Length);
				for (int l = 0; l < num6 - 1; l++)
				{
					array5[l * 18] = l * 4 + 5;
					array5[l * 18 + 1] = l * 4 + 1;
					array5[l * 18 + 2] = l * 4 + 4;
					array5[l * 18 + 3] = l * 4;
					array5[l * 18 + 4] = l * 4 + 4;
					array5[l * 18 + 5] = l * 4 + 1;
					array5[l * 18 + 6] = l * 4 + 6;
					array5[l * 18 + 7] = l * 4 + 2;
					array5[l * 18 + 8] = l * 4 + 5;
					array5[l * 18 + 9] = l * 4 + 1;
					array5[l * 18 + 10] = l * 4 + 5;
					array5[l * 18 + 11] = l * 4 + 2;
					array5[l * 18 + 12] = l * 4 + 7;
					array5[l * 18 + 13] = l * 4 + 3;
					array5[l * 18 + 14] = l * 4 + 6;
					array5[l * 18 + 15] = l * 4 + 2;
					array5[l * 18 + 16] = l * 4 + 6;
					array5[l * 18 + 17] = l * 4 + 3;
				}
				Transform transform = new GameObject().transform;
				transform.name = "Segment_" + num4.ToString();
				transform.parent = this.road;
				transform.tag = "Environment";
				transform.gameObject.layer = 19;
				transform.gameObject.AddComponent<MeshCollider>();
				if (LevelRoads.materials[(int)this.material].isConcrete)
				{
					transform.GetComponent<Collider>().sharedMaterial = (PhysicMaterial)Resources.Load("Physics/Concrete_Static");
				}
				else
				{
					transform.GetComponent<Collider>().sharedMaterial = (PhysicMaterial)Resources.Load("Physics/Gravel_Static");
				}
				Mesh mesh = new Mesh();
				mesh.name = "Road_Segment_" + num4.ToString();
				mesh.vertices = array3;
				mesh.normals = array4;
				mesh.uv = uv;
				mesh.triangles = array5;
				transform.GetComponent<MeshCollider>().sharedMesh = mesh;
				num4++;
			}
		}

		// Token: 0x06002884 RID: 10372 RVA: 0x000AC5CC File Offset: 0x000AA7CC
		private void updateSamples()
		{
			this.samples.Clear();
			float num = 0f;
			for (int i = 0; i < this.joints.Count - 1 + (this.isLoop ? 1 : 0); i++)
			{
				float lengthEstimate = this.getLengthEstimate(i);
				float num2;
				for (num2 = num; num2 < lengthEstimate; num2 += 5f)
				{
					float time = num2 / lengthEstimate;
					RoadSample roadSample = new RoadSample();
					roadSample.index = i;
					roadSample.time = time;
					this.samples.Add(roadSample);
				}
				num = num2 - lengthEstimate;
			}
			if (this.isLoop)
			{
				RoadSample roadSample2 = new RoadSample();
				roadSample2.index = 0;
				roadSample2.time = 0f;
				this.samples.Add(roadSample2);
				return;
			}
			RoadSample roadSample3 = new RoadSample();
			roadSample3.index = this.joints.Count - 2;
			roadSample3.time = 1f;
			this.samples.Add(roadSample3);
		}

		// Token: 0x06002885 RID: 10373 RVA: 0x000AC6BC File Offset: 0x000AA8BC
		private void updateTrackSamples()
		{
			this.trackSamples.Clear();
			if (this.samples.Count < 2)
			{
				return;
			}
			Vector3 vector = Vector3.zero;
			Vector3 up = Vector3.up;
			double num = 0.0;
			int num2 = this.isLoop ? (this.samples.Count - 1) : this.samples.Count;
			for (int i = 1; i < num2; i++)
			{
				RoadSample roadSample = this.samples[i];
				TrackSample trackSample = null;
				if (i == 1)
				{
					RoadSample roadSample2 = this.samples[0];
					this.getTrackPosition(roadSample2.index, roadSample2.time, out vector, out up);
					trackSample = new TrackSample();
					trackSample.position = vector;
					trackSample.normal = up;
					this.trackSamples.Add(trackSample);
				}
				Vector3 vector2;
				Vector3 normal;
				this.getTrackPosition(roadSample.index, roadSample.time, out vector2, out normal);
				Vector3 a = vector2 - vector;
				float magnitude = a.magnitude;
				Vector3 direction = a / magnitude;
				TrackSample trackSample2 = new TrackSample();
				trackSample2.distance = (float)num;
				trackSample2.position = vector2;
				trackSample2.normal = normal;
				trackSample2.direction = direction;
				this.trackSamples.Add(trackSample2);
				if (trackSample != null)
				{
					trackSample.direction = direction;
				}
				vector = vector2;
				num += (double)magnitude;
			}
			if (this.isLoop)
			{
				num += (double)(this.trackSamples[0].position - vector).magnitude;
			}
			this.trackSampledLength = (float)num;
		}

		// Token: 0x06002886 RID: 10374 RVA: 0x000AC84C File Offset: 0x000AAA4C
		public void updatePoints()
		{
			for (int i = 0; i < this.joints.Count; i++)
			{
				RoadJoint roadJoint = this.joints[i];
				if (!roadJoint.ignoreTerrain)
				{
					roadJoint.vertex.y = LevelGround.getHeight(roadJoint.vertex);
				}
			}
			for (int j = 0; j < this.joints.Count; j++)
			{
				RoadPath roadPath = this.paths[j];
				roadPath.vertex.position = this.joints[j].vertex;
				roadPath.tangents[0].gameObject.SetActive(j > 0 || this.isLoop);
				roadPath.tangents[1].gameObject.SetActive(j < this.joints.Count - 1 || this.isLoop);
				roadPath.setTangent(0, this.joints[j].getTangent(0));
				roadPath.setTangent(1, this.joints[j].getTangent(1));
			}
			if (this.joints.Count < 2)
			{
				this.lineRenderer.positionCount = 0;
				return;
			}
			this.updateSamples();
			this.lineRenderer.positionCount = this.samples.Count;
			for (int k = 0; k < this.samples.Count; k++)
			{
				RoadSample roadSample = this.samples[k];
				RoadJoint roadJoint2 = this.joints[roadSample.index];
				Vector3 position = this.getPosition(roadSample.index, roadSample.time);
				if (!roadJoint2.ignoreTerrain)
				{
					position.y = LevelGround.getHeight(position);
				}
				if (roadSample.index < this.joints.Count - 1)
				{
					position.y += Mathf.Lerp(roadJoint2.offset, this.joints[roadSample.index + 1].offset, roadSample.time);
				}
				else if (this.isLoop)
				{
					position.y += Mathf.Lerp(roadJoint2.offset, this.joints[0].offset, roadSample.time);
				}
				else
				{
					position.y += roadJoint2.offset;
				}
				this.lineRenderer.SetPosition(k, position);
			}
		}

		// Token: 0x06002887 RID: 10375 RVA: 0x000ACAA3 File Offset: 0x000AACA3
		public Road(byte newMaterial, ushort newRoadIndex) : this(newMaterial, newRoadIndex, false, new List<RoadJoint>())
		{
		}

		// Token: 0x06002888 RID: 10376 RVA: 0x000ACAB4 File Offset: 0x000AACB4
		public Road(byte newMaterial, ushort newRoadIndex, bool newLoop, List<RoadJoint> newJoints)
		{
			this.material = newMaterial;
			this.roadIndex = newRoadIndex;
			this._road = new GameObject().transform;
			this.road.name = "Road";
			this.road.tag = "Environment";
			this.road.gameObject.layer = 19;
			this._isLoop = newLoop;
			this._joints = newJoints;
			this.samples = new List<RoadSample>();
			this.trackSamples = new List<TrackSample>();
			if (Level.isEditor)
			{
				this.line = ((GameObject)Object.Instantiate(Resources.Load("Edit/Road"))).transform;
				this.line.name = "Line";
				this._paths = new List<RoadPath>();
				this.lineRenderer = this.line.GetComponent<LineRenderer>();
				for (int i = 0; i < this.joints.Count; i++)
				{
					Transform transform = ((GameObject)Object.Instantiate(Resources.Load("Edit/Path"))).transform;
					transform.name = "Path_" + i.ToString();
					transform.parent = this.line;
					RoadPath roadPath = new RoadPath(transform);
					this.paths.Add(roadPath);
				}
			}
		}

		// Token: 0x04001580 RID: 5504
		public byte material;

		// Token: 0x04001582 RID: 5506
		private Transform _road;

		// Token: 0x04001583 RID: 5507
		private Transform line;

		// Token: 0x04001584 RID: 5508
		private LineRenderer lineRenderer;

		// Token: 0x04001585 RID: 5509
		private bool _isLoop;

		// Token: 0x04001586 RID: 5510
		private List<RoadJoint> _joints;

		// Token: 0x04001587 RID: 5511
		private List<RoadSample> samples;

		// Token: 0x04001588 RID: 5512
		private List<TrackSample> trackSamples;

		// Token: 0x0400158A RID: 5514
		private List<RoadPath> _paths;
	}
}
