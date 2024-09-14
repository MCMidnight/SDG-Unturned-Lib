using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Unturned.SystemEx;

namespace SDG.Unturned
{
	// Token: 0x02000562 RID: 1378
	public class HousingConnections
	{
		// Token: 0x06002BE3 RID: 11235 RVA: 0x000BB64C File Offset: 0x000B984C
		public HousingConnections()
		{
			this.edgesGrid = new RegionList<HousingEdge>();
			this.verticesGrid = new RegionList<HousingVertex>();
		}

		// Token: 0x06002BE4 RID: 11236 RVA: 0x000BB6C4 File Offset: 0x000B98C4
		internal void OnLogMemoryUsage(List<string> results)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			foreach (HousingEdge housingEdge in this.edgesGrid.EnumerateAllItems())
			{
				num++;
				int num4 = num2;
				List<StructureDrop> walls = housingEdge.walls;
				num2 = num4 + ((walls != null) ? walls.Count : 0);
				int num5 = num3;
				List<StructureDrop> forwardFloors = housingEdge.forwardFloors;
				num3 = num5 + ((forwardFloors != null) ? forwardFloors.Count : 0);
				int num6 = num3;
				List<StructureDrop> backwardFloors = housingEdge.backwardFloors;
				num3 = num6 + ((backwardFloors != null) ? backwardFloors.Count : 0);
			}
			results.Add(string.Format("Housing connection edges: {0}", num));
			results.Add(string.Format("Housing connection edge walls: {0}", num2));
			results.Add(string.Format("Housing connection edge floors: {0}", num3));
			int num7 = 0;
			int num8 = 0;
			int num9 = 0;
			int num10 = 0;
			foreach (HousingVertex housingVertex in this.verticesGrid.EnumerateAllItems())
			{
				num7++;
				int num11 = num8;
				List<StructureDrop> pillars = housingVertex.pillars;
				num8 = num11 + ((pillars != null) ? pillars.Count : 0);
				int num12 = num9;
				List<StructureDrop> floors = housingVertex.floors;
				num9 = num12 + ((floors != null) ? floors.Count : 0);
				int num13 = num10;
				List<HousingEdge> edges = housingVertex.edges;
				num10 = num13 + ((edges != null) ? edges.Count : 0);
			}
			results.Add(string.Format("Housing connection vertices: {0}", num7));
			results.Add(string.Format("Housing connection vertex pillars: {0}", num8));
			results.Add(string.Format("Housing connection vertex floors: {0}", num9));
			results.Add(string.Format("Housing connection vertex edges: {0}", num10));
		}

		/// <summary>
		/// Called when a housing item is spawned or after moving an existing item.
		/// </summary>
		// Token: 0x06002BE5 RID: 11237 RVA: 0x000BB898 File Offset: 0x000B9A98
		internal void LinkConnections(StructureDrop drop)
		{
			switch (drop.asset.construct)
			{
			case EConstruct.FLOOR:
				this.LinkSquareFloor(drop);
				return;
			case EConstruct.WALL:
				this.LinkWall(drop, -2.125f);
				return;
			case EConstruct.RAMPART:
				this.LinkWall(drop, -0.9f);
				return;
			case EConstruct.ROOF:
				this.LinkSquareFloor(drop);
				return;
			case EConstruct.PILLAR:
				this.LinkPillar(drop, drop.model.position + new Vector3(0f, -2.125f, 0f));
				return;
			case EConstruct.POST:
				this.LinkPillar(drop, drop.model.position + new Vector3(0f, -0.9f, 0f));
				return;
			case EConstruct.FLOOR_POLY:
				this.LinkTriangularFloor(drop);
				return;
			case EConstruct.ROOF_POLY:
				this.LinkTriangularFloor(drop);
				return;
			default:
				UnturnedLog.error(string.Format("Link housing connection unhandled: {0}", drop.asset.construct));
				return;
			}
		}

		/// <summary>
		/// Called before a housing item is destroyed or before moving a housing item.
		/// </summary>
		// Token: 0x06002BE6 RID: 11238 RVA: 0x000BB990 File Offset: 0x000B9B90
		internal void UnlinkConnections(StructureDrop drop)
		{
			switch (drop.asset.construct)
			{
			case EConstruct.FLOOR:
				this.UnlinkSquareFloor(drop);
				return;
			case EConstruct.WALL:
			case EConstruct.RAMPART:
				this.UnlinkWall(drop);
				return;
			case EConstruct.ROOF:
				this.UnlinkSquareFloor(drop);
				return;
			case EConstruct.PILLAR:
			case EConstruct.POST:
				this.UnlinkPillar(drop);
				return;
			case EConstruct.FLOOR_POLY:
				this.UnlinkTriangleFloor(drop);
				return;
			case EConstruct.ROOF_POLY:
				this.UnlinkTriangleFloor(drop);
				return;
			default:
				UnturnedLog.error(string.Format("Unlink housing connection unhandled: {0}", drop.asset.construct));
				return;
			}
		}

		/// <summary>
		/// Search grid for existing vertex at approximately equal position.
		/// Considers adjacent grid cells if near cell boundary to avoid issues with floating point inaccuracy. 
		/// </summary>
		// Token: 0x06002BE7 RID: 11239 RVA: 0x000BBA20 File Offset: 0x000B9C20
		private HousingVertex FindVertex(Vector3 position)
		{
			foreach (HousingVertex housingVertex in this.verticesGrid.EnumerateItemsInSquare(position, 0.02f))
			{
				if (housingVertex.position.IsNearlyEqual(position, 0.02f))
				{
					return housingVertex;
				}
			}
			return null;
		}

		/// <summary>
		/// Search grid for existing edge at approximately equal position.
		/// Considers adjacent grid cells if near cell boundary to avoid issues with floating point inaccuracy. 
		/// </summary>
		// Token: 0x06002BE8 RID: 11240 RVA: 0x000BBA8C File Offset: 0x000B9C8C
		private HousingEdge FindEdge(Vector3 position)
		{
			foreach (HousingEdge housingEdge in this.edgesGrid.EnumerateItemsInSquare(position, 0.02f))
			{
				if (housingEdge.position.IsNearlyEqual(position, 0.02f))
				{
					return housingEdge;
				}
			}
			return null;
		}

		// Token: 0x06002BE9 RID: 11241 RVA: 0x000BBAF8 File Offset: 0x000B9CF8
		private void RemoveVertex(HousingVertex vertex)
		{
			foreach (HousingEdge housingEdge in vertex.edges)
			{
				if (housingEdge.vertex0 == vertex)
				{
					housingEdge.vertex0 = null;
				}
				else if (housingEdge.vertex1 == vertex)
				{
					housingEdge.vertex1 = null;
				}
			}
			vertex.edges.Clear();
			if (vertex.upperVertex != null)
			{
				vertex.upperVertex.lowerVertex = null;
				vertex.upperVertex = null;
			}
			if (vertex.lowerVertex != null)
			{
				vertex.lowerVertex.upperVertex = null;
				vertex.lowerVertex = null;
			}
			this.verticesGrid.RemoveFast(vertex.position, vertex, 0.02f);
		}

		// Token: 0x06002BEA RID: 11242 RVA: 0x000BBBC0 File Offset: 0x000B9DC0
		private void RemoveEdge(HousingEdge edge)
		{
			if (edge.vertex0 != null)
			{
				edge.vertex0.edges.RemoveFast(edge);
				edge.vertex0 = null;
			}
			if (edge.vertex1 != null)
			{
				edge.vertex1.edges.RemoveFast(edge);
				edge.vertex1 = null;
			}
			if (edge.upperEdge != null)
			{
				edge.upperEdge.lowerEdge = null;
				edge.upperEdge = null;
			}
			if (edge.lowerEdge != null)
			{
				edge.lowerEdge.upperEdge = null;
				edge.lowerEdge = null;
			}
			this.edgesGrid.RemoveFast(edge.position, edge, 0.02f);
		}

		// Token: 0x06002BEB RID: 11243 RVA: 0x000BBC60 File Offset: 0x000B9E60
		private Vector3 GetFloorCenter(StructureDrop floor)
		{
			if (floor.asset.construct != EConstruct.FLOOR_POLY && floor.asset.construct != EConstruct.ROOF_POLY)
			{
				return floor.model.position;
			}
			return floor.model.TransformPoint(0f, 1.2679492f, 0f);
		}

		// Token: 0x06002BEC RID: 11244 RVA: 0x000BBCB4 File Offset: 0x000B9EB4
		private HousingEdge LinkSquareEdge(StructureDrop drop, Vector3 direction, float rotation)
		{
			Vector3 edgePosition = drop.model.position + direction * 3f;
			return this.LinkFloorEdge(drop, edgePosition, direction, rotation);
		}

		/// <summary>
		/// Find existing edge and add connection, or add new empty edge.
		/// </summary>
		// Token: 0x06002BED RID: 11245 RVA: 0x000BBCE8 File Offset: 0x000B9EE8
		private HousingEdge LinkFloorEdge(StructureDrop floor, Vector3 edgePosition, Vector3 direction, float rotation)
		{
			HousingEdge housingEdge = this.FindEdge(edgePosition);
			if (housingEdge != null)
			{
				if (housingEdge.forwardFloors.IsEmpty<StructureDrop>() && housingEdge.backwardFloors.IsEmpty<StructureDrop>())
				{
					housingEdge.direction = direction;
					housingEdge.rotation = HousingConnections.GetModelYaw(floor.model) + rotation;
					housingEdge.backwardFloors.Add(floor);
				}
				else if (Vector3.Dot(direction, housingEdge.direction) > 0f)
				{
					housingEdge.backwardFloors.Add(floor);
				}
				else
				{
					housingEdge.forwardFloors.Add(floor);
				}
			}
			else
			{
				housingEdge = new HousingEdge();
				housingEdge.position = edgePosition;
				housingEdge.direction = direction;
				housingEdge.rotation = HousingConnections.GetModelYaw(floor.model) + rotation;
				housingEdge.forwardFloors = new List<StructureDrop>(1);
				HousingEdge housingEdge2 = housingEdge;
				List<StructureDrop> list = new List<StructureDrop>(1);
				list.Add(floor);
				housingEdge2.backwardFloors = list;
				housingEdge.walls = new List<StructureDrop>(1);
				this.edgesGrid.Add(edgePosition, housingEdge);
			}
			return housingEdge;
		}

		/// <summary>
		/// Find existing vertex and add connection, or add new empty vertex.
		/// </summary>
		// Token: 0x06002BEE RID: 11246 RVA: 0x000BBDD8 File Offset: 0x000B9FD8
		private HousingVertex LinkFloorVertex(StructureDrop floor, Vector3 vertexPosition)
		{
			HousingVertex housingVertex = this.FindVertex(vertexPosition);
			if (housingVertex != null)
			{
				if (housingVertex.floors.Count < 1)
				{
					housingVertex.rotation = HousingConnections.GetModelYaw(floor.model);
				}
				housingVertex.floors.Add(floor);
			}
			else
			{
				housingVertex = new HousingVertex();
				housingVertex.position = vertexPosition;
				housingVertex.rotation = HousingConnections.GetModelYaw(floor.model);
				housingVertex.floors.Add(floor);
				this.verticesGrid.Add(vertexPosition, housingVertex);
			}
			return housingVertex;
		}

		// Token: 0x06002BEF RID: 11247 RVA: 0x000BBE55 File Offset: 0x000BA055
		private void UnlinkFloorEdge(StructureDrop floor, HousingEdge edge)
		{
			if (!edge.forwardFloors.RemoveFast(floor))
			{
				edge.backwardFloors.RemoveFast(floor);
			}
			if (edge.ShouldBeRemoved)
			{
				this.RemoveEdge(edge);
			}
		}

		// Token: 0x06002BF0 RID: 11248 RVA: 0x000BBE81 File Offset: 0x000BA081
		private void UnlinkFloorVertex(StructureDrop floor, HousingVertex vertex)
		{
			vertex.floors.RemoveFast(floor);
			if (vertex.ShouldBeRemoved)
			{
				this.RemoveVertex(vertex);
			}
		}

		// Token: 0x06002BF1 RID: 11249 RVA: 0x000BBEA0 File Offset: 0x000BA0A0
		private void UnlinkSquareFloor(StructureDrop floor)
		{
			HousingSquareFloorConnections housingSquareFloorConnections = (HousingSquareFloorConnections)floor.housingConnectionData;
			floor.housingConnectionData = null;
			this.UnlinkFloorEdge(floor, housingSquareFloorConnections.edge0);
			this.UnlinkFloorEdge(floor, housingSquareFloorConnections.edge1);
			this.UnlinkFloorEdge(floor, housingSquareFloorConnections.edge2);
			this.UnlinkFloorEdge(floor, housingSquareFloorConnections.edge3);
			this.UnlinkFloorVertex(floor, housingSquareFloorConnections.vertex0);
			this.UnlinkFloorVertex(floor, housingSquareFloorConnections.vertex1);
			this.UnlinkFloorVertex(floor, housingSquareFloorConnections.vertex2);
			this.UnlinkFloorVertex(floor, housingSquareFloorConnections.vertex3);
		}

		// Token: 0x06002BF2 RID: 11250 RVA: 0x000BBF28 File Offset: 0x000BA128
		private void UnlinkTriangleFloor(StructureDrop floor)
		{
			HousingTriangleFloorConnections housingTriangleFloorConnections = (HousingTriangleFloorConnections)floor.housingConnectionData;
			floor.housingConnectionData = null;
			this.UnlinkFloorEdge(floor, housingTriangleFloorConnections.edge0);
			this.UnlinkFloorEdge(floor, housingTriangleFloorConnections.edge1);
			this.UnlinkFloorEdge(floor, housingTriangleFloorConnections.edge2);
			this.UnlinkFloorVertex(floor, housingTriangleFloorConnections.vertex0);
			this.UnlinkFloorVertex(floor, housingTriangleFloorConnections.vertex1);
			this.UnlinkFloorVertex(floor, housingTriangleFloorConnections.vertex2);
		}

		// Token: 0x06002BF3 RID: 11251 RVA: 0x000BBF98 File Offset: 0x000BA198
		private void LinkEdgeWithVertices(HousingEdge edge, HousingVertex vertex0, HousingVertex vertex1)
		{
			if (edge.vertex0 != vertex0 && edge.vertex1 != vertex0)
			{
				if (edge.vertex0 == null)
				{
					edge.vertex0 = vertex0;
				}
				else
				{
					edge.vertex1 = vertex0;
				}
				vertex0.edges.Add(edge);
			}
			if (edge.vertex0 != vertex1 && edge.vertex1 != vertex1)
			{
				if (edge.vertex0 == null)
				{
					edge.vertex0 = vertex1;
				}
				else
				{
					edge.vertex1 = vertex1;
				}
				vertex1.edges.Add(edge);
			}
		}

		// Token: 0x06002BF4 RID: 11252 RVA: 0x000BC014 File Offset: 0x000BA214
		private void LinkSquareFloor(StructureDrop drop)
		{
			HousingSquareFloorConnections housingSquareFloorConnections = new HousingSquareFloorConnections();
			drop.housingConnectionData = housingSquareFloorConnections;
			housingSquareFloorConnections.edge0 = this.LinkSquareEdge(drop, drop.model.TransformDirection(new Vector3(1f, 0f, 0f)), 270f);
			housingSquareFloorConnections.edge1 = this.LinkSquareEdge(drop, drop.model.TransformDirection(new Vector3(-1f, 0f, 0f)), 90f);
			housingSquareFloorConnections.edge2 = this.LinkSquareEdge(drop, drop.model.TransformDirection(new Vector3(0f, 1f, 0f)), 0f);
			housingSquareFloorConnections.edge3 = this.LinkSquareEdge(drop, drop.model.TransformDirection(new Vector3(0f, -1f, 0f)), 180f);
			housingSquareFloorConnections.vertex0 = this.LinkFloorVertex(drop, drop.model.TransformPoint(new Vector3(3f, 3f, 0f)));
			housingSquareFloorConnections.vertex1 = this.LinkFloorVertex(drop, drop.model.TransformPoint(new Vector3(-3f, 3f, 0f)));
			housingSquareFloorConnections.vertex2 = this.LinkFloorVertex(drop, drop.model.TransformPoint(new Vector3(3f, -3f, 0f)));
			housingSquareFloorConnections.vertex3 = this.LinkFloorVertex(drop, drop.model.TransformPoint(new Vector3(-3f, -3f, 0f)));
			this.LinkEdgeWithVertices(housingSquareFloorConnections.edge0, housingSquareFloorConnections.vertex0, housingSquareFloorConnections.vertex2);
			this.LinkEdgeWithVertices(housingSquareFloorConnections.edge1, housingSquareFloorConnections.vertex1, housingSquareFloorConnections.vertex3);
			this.LinkEdgeWithVertices(housingSquareFloorConnections.edge2, housingSquareFloorConnections.vertex0, housingSquareFloorConnections.vertex1);
			this.LinkEdgeWithVertices(housingSquareFloorConnections.edge3, housingSquareFloorConnections.vertex2, housingSquareFloorConnections.vertex3);
		}

		// Token: 0x06002BF5 RID: 11253 RVA: 0x000BC204 File Offset: 0x000BA404
		private void LinkTriangularFloor(StructureDrop drop)
		{
			HousingTriangleFloorConnections housingTriangleFloorConnections = new HousingTriangleFloorConnections();
			drop.housingConnectionData = housingTriangleFloorConnections;
			Vector3 vector = drop.model.TransformDirection(new Vector3(0f, 1f, 0f));
			housingTriangleFloorConnections.edge0 = this.LinkFloorEdge(drop, drop.model.position + vector * 3f, vector, 0f);
			Vector3 edgePosition = drop.model.TransformPoint(new Vector3(1.5f, 0.40192378f, 0f));
			Vector3 direction = drop.model.TransformDirection(this.leftLocalDirection);
			housingTriangleFloorConnections.edge1 = this.LinkFloorEdge(drop, edgePosition, direction, 240f);
			Vector3 edgePosition2 = drop.model.TransformPoint(new Vector3(-1.5f, 0.40192378f, 0f));
			Vector3 direction2 = drop.model.TransformDirection(this.rightLocalDirection);
			housingTriangleFloorConnections.edge2 = this.LinkFloorEdge(drop, edgePosition2, direction2, 120f);
			housingTriangleFloorConnections.vertex0 = this.LinkFloorVertex(drop, drop.model.TransformPoint(new Vector3(3f, 3f, 0f)));
			housingTriangleFloorConnections.vertex1 = this.LinkFloorVertex(drop, drop.model.TransformPoint(new Vector3(-3f, 3f, 0f)));
			housingTriangleFloorConnections.vertex2 = this.LinkFloorVertex(drop, drop.model.TransformPoint(new Vector3(0f, -2.1961524f, 0f)));
			this.LinkEdgeWithVertices(housingTriangleFloorConnections.edge0, housingTriangleFloorConnections.vertex0, housingTriangleFloorConnections.vertex1);
			this.LinkEdgeWithVertices(housingTriangleFloorConnections.edge1, housingTriangleFloorConnections.vertex0, housingTriangleFloorConnections.vertex2);
			this.LinkEdgeWithVertices(housingTriangleFloorConnections.edge2, housingTriangleFloorConnections.vertex1, housingTriangleFloorConnections.vertex2);
		}

		/// <summary>
		/// Find existing edge and set associated wall, or add an empty edge at wall's location.
		/// </summary>
		// Token: 0x06002BF6 RID: 11254 RVA: 0x000BC3C8 File Offset: 0x000BA5C8
		private void LinkWall(StructureDrop wall, float pivotOffset)
		{
			Vector3 vector = wall.model.position + new Vector3(0f, pivotOffset, 0f);
			Vector3 position = vector + new Vector3(0f, 4.25f, 0f);
			HousingEdge housingEdge = this.FindEdge(vector);
			HousingEdge housingEdge2 = this.FindEdge(position);
			HousingEdge housingEdge3 = housingEdge;
			if (housingEdge3 == null)
			{
				Vector3 position2 = wall.model.TransformPoint(3f, 0f, 0f) + new Vector3(0f, pivotOffset, 0f);
				Vector3 position3 = wall.model.TransformPoint(-3f, 0f, 0f) + new Vector3(0f, pivotOffset, 0f);
				HousingVertex housingVertex = this.FindVertex(position2);
				if (housingVertex == null)
				{
					housingVertex = new HousingVertex();
					housingVertex.position = position2;
					housingVertex.rotation = HousingConnections.GetModelYaw(wall.model);
					this.verticesGrid.Add(position2, housingVertex);
				}
				HousingVertex housingVertex2 = this.FindVertex(position3);
				if (housingVertex2 == null)
				{
					housingVertex2 = new HousingVertex();
					housingVertex2.position = position3;
					housingVertex2.rotation = HousingConnections.GetModelYaw(wall.model);
					this.verticesGrid.Add(position3, housingVertex2);
				}
				housingEdge3 = new HousingEdge();
				housingEdge3.position = vector;
				housingEdge3.direction = wall.model.TransformDirection(0f, 1f, 0f);
				housingEdge3.rotation = HousingConnections.GetModelYaw(wall.model);
				housingEdge3.walls = new List<StructureDrop>(1);
				housingEdge3.forwardFloors = new List<StructureDrop>(1);
				housingEdge3.backwardFloors = new List<StructureDrop>(1);
				housingEdge3.vertex0 = housingVertex;
				housingVertex.edges.Add(housingEdge3);
				housingEdge3.vertex1 = housingVertex2;
				housingVertex2.edges.Add(housingEdge3);
				this.edgesGrid.Add(vector, housingEdge3);
			}
			HousingEdge housingEdge4 = housingEdge2;
			if (housingEdge4 == null)
			{
				Vector3 position4 = wall.model.TransformPoint(3f, 0f, 0f) + new Vector3(0f, pivotOffset + 4.25f, 0f);
				Vector3 position5 = wall.model.TransformPoint(-3f, 0f, 0f) + new Vector3(0f, pivotOffset + 4.25f, 0f);
				HousingVertex housingVertex3 = this.FindVertex(position4);
				if (housingVertex3 == null)
				{
					housingVertex3 = new HousingVertex();
					housingVertex3.position = position4;
					housingVertex3.rotation = HousingConnections.GetModelYaw(wall.model);
					this.verticesGrid.Add(position4, housingVertex3);
				}
				HousingVertex housingVertex4 = this.FindVertex(position5);
				if (housingVertex4 == null)
				{
					housingVertex4 = new HousingVertex();
					housingVertex4.position = position5;
					housingVertex4.rotation = HousingConnections.GetModelYaw(wall.model);
					this.verticesGrid.Add(position5, housingVertex4);
				}
				housingEdge4 = new HousingEdge();
				housingEdge4.position = position;
				housingEdge4.direction = wall.model.TransformDirection(0f, 1f, 0f);
				housingEdge4.rotation = HousingConnections.GetModelYaw(wall.model);
				housingEdge4.walls = new List<StructureDrop>(1);
				housingEdge4.forwardFloors = new List<StructureDrop>(1);
				housingEdge4.backwardFloors = new List<StructureDrop>(1);
				housingEdge4.vertex0 = housingVertex3;
				housingVertex3.edges.Add(housingEdge4);
				housingEdge4.vertex1 = housingVertex4;
				housingVertex4.edges.Add(housingEdge4);
				this.edgesGrid.Add(position, housingEdge4);
			}
			housingEdge4.lowerEdge = housingEdge3;
			housingEdge3.upperEdge = housingEdge4;
			HousingWallConnections housingWallConnections = new HousingWallConnections();
			wall.housingConnectionData = housingWallConnections;
			housingWallConnections.lowerEdge = housingEdge3;
			housingWallConnections.upperEdge = housingEdge4;
			housingEdge3.walls.Add(wall);
		}

		/// <summary>
		/// Find slot occupied by wall and remove if no longer attached to anything.
		/// </summary>
		// Token: 0x06002BF7 RID: 11255 RVA: 0x000BC778 File Offset: 0x000BA978
		private void UnlinkWall(StructureDrop wall)
		{
			HousingWallConnections housingWallConnections = (HousingWallConnections)wall.housingConnectionData;
			wall.housingConnectionData = null;
			housingWallConnections.lowerEdge.walls.RemoveFast(wall);
			if (housingWallConnections.lowerEdge.ShouldBeRemoved)
			{
				HousingVertex vertex = housingWallConnections.lowerEdge.vertex0;
				HousingVertex vertex2 = housingWallConnections.lowerEdge.vertex1;
				this.RemoveEdge(housingWallConnections.lowerEdge);
				if (vertex.ShouldBeRemoved)
				{
					this.RemoveVertex(vertex);
				}
				if (vertex2.ShouldBeRemoved)
				{
					this.RemoveVertex(vertex2);
				}
			}
			if (housingWallConnections.upperEdge.ShouldBeRemoved)
			{
				HousingVertex vertex3 = housingWallConnections.upperEdge.vertex0;
				HousingVertex vertex4 = housingWallConnections.upperEdge.vertex1;
				this.RemoveEdge(housingWallConnections.upperEdge);
				if (vertex3.ShouldBeRemoved)
				{
					this.RemoveVertex(vertex3);
				}
				if (vertex4.ShouldBeRemoved)
				{
					this.RemoveVertex(vertex4);
				}
			}
		}

		/// <summary>
		/// Find existing vertex and set associated pillar, or add an empty vertex at pillar's location.
		/// </summary>
		// Token: 0x06002BF8 RID: 11256 RVA: 0x000BC84C File Offset: 0x000BAA4C
		private void LinkPillar(StructureDrop pillar, Vector3 lowerVertexPosition)
		{
			Vector3 position = lowerVertexPosition + new Vector3(0f, 4.25f, 0f);
			HousingVertex housingVertex = this.FindVertex(lowerVertexPosition);
			if (housingVertex == null)
			{
				housingVertex = new HousingVertex();
				housingVertex.position = lowerVertexPosition;
				housingVertex.rotation = HousingConnections.GetModelYaw(pillar.model);
				this.verticesGrid.Add(lowerVertexPosition, housingVertex);
			}
			HousingVertex housingVertex2 = this.FindVertex(position);
			if (housingVertex2 == null)
			{
				housingVertex2 = new HousingVertex();
				housingVertex2.position = position;
				housingVertex2.rotation = HousingConnections.GetModelYaw(pillar.model);
				this.verticesGrid.Add(position, housingVertex2);
			}
			housingVertex.upperVertex = housingVertex2;
			housingVertex2.lowerVertex = housingVertex;
			HousingPillarConnections housingPillarConnections = new HousingPillarConnections();
			pillar.housingConnectionData = housingPillarConnections;
			housingPillarConnections.lowerVertex = housingVertex;
			housingPillarConnections.upperVertex = housingVertex2;
			housingVertex.pillars.Add(pillar);
		}

		/// <summary>
		/// Find slot occupied by pillar and remove if no longer attached to anything.
		/// </summary>
		// Token: 0x06002BF9 RID: 11257 RVA: 0x000BC918 File Offset: 0x000BAB18
		private void UnlinkPillar(StructureDrop pillar)
		{
			HousingPillarConnections housingPillarConnections = (HousingPillarConnections)pillar.housingConnectionData;
			pillar.housingConnectionData = null;
			housingPillarConnections.lowerVertex.pillars.RemoveFast(pillar);
			if (housingPillarConnections.lowerVertex.ShouldBeRemoved)
			{
				this.RemoveVertex(housingPillarConnections.lowerVertex);
			}
			if (housingPillarConnections.upperVertex.ShouldBeRemoved)
			{
				this.RemoveVertex(housingPillarConnections.upperVertex);
			}
		}

		// Token: 0x06002BFA RID: 11258 RVA: 0x000BC97C File Offset: 0x000BAB7C
		internal bool DoesHitCountAsTerrain(RaycastHit hit)
		{
			if (hit.transform == null)
			{
				return false;
			}
			if (hit.transform.CompareTag("Ground"))
			{
				return true;
			}
			ObjectAsset asset = LevelObjects.getAsset(hit.transform);
			return asset != null && asset.allowStructures;
		}

		// Token: 0x06002BFB RID: 11259 RVA: 0x000BC9C8 File Offset: 0x000BABC8
		private float ScorePosition(Ray ray, Vector3 testPosition)
		{
			float sqrMagnitude = (testPosition - ray.origin).sqrMagnitude;
			if (sqrMagnitude > 64f)
			{
				return 65f;
			}
			float num = Vector3.Dot((testPosition - ray.origin).normalized, ray.direction);
			if (num < 0.9f)
			{
				return 65f;
			}
			return (1f - num) * sqrMagnitude;
		}

		// Token: 0x06002BFC RID: 11260 RVA: 0x000BCA34 File Offset: 0x000BAC34
		internal bool FindEmptyFloorSlot(Ray ray, bool isRoof, out Vector3 position, out float rotation)
		{
			position = default(Vector3);
			rotation = 0f;
			float num = 64f;
			bool result = false;
			foreach (HousingEdge housingEdge in this.edgesGrid.EnumerateItemsInSquare(ray.origin, 8f))
			{
				if (!isRoof || housingEdge.CanAttachRoof)
				{
					if (housingEdge.forwardFloors.IsEmpty<StructureDrop>())
					{
						Vector3 testPosition = housingEdge.position + housingEdge.direction * 3f * 0.5f;
						float num2 = this.ScorePosition(ray, testPosition);
						if (num2 < num)
						{
							num = num2;
							position = housingEdge.position + housingEdge.direction * 3f;
							rotation = housingEdge.rotation + 180f;
							result = true;
						}
					}
					if (housingEdge.backwardFloors.IsEmpty<StructureDrop>())
					{
						Vector3 testPosition2 = housingEdge.position - housingEdge.direction * 3f * 0.5f;
						float num3 = this.ScorePosition(ray, testPosition2);
						if (num3 < num)
						{
							num = num3;
							position = housingEdge.position - housingEdge.direction * 3f;
							rotation = housingEdge.rotation;
							result = true;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06002BFD RID: 11261 RVA: 0x000BCBB0 File Offset: 0x000BADB0
		internal bool FindEmptyWallSlot(Ray ray, out Vector3 position, out float rotation)
		{
			position = default(Vector3);
			rotation = 0f;
			float num = 64f;
			bool result = false;
			foreach (HousingEdge housingEdge in this.edgesGrid.EnumerateItemsInSquare(ray.origin, 8f))
			{
				if (housingEdge.walls.IsEmpty<StructureDrop>())
				{
					Vector3 vector = housingEdge.position + new Vector3(0f, 2.125f, 0f);
					float num2 = this.ScorePosition(ray, vector);
					if (num2 < num)
					{
						num = num2;
						position = vector;
						rotation = housingEdge.rotation;
						result = true;
					}
				}
				if (housingEdge.lowerEdge == null || housingEdge.lowerEdge.walls.IsEmpty<StructureDrop>())
				{
					Vector3 vector2 = housingEdge.position + new Vector3(0f, -2.125f, 0f);
					float num3 = this.ScorePosition(ray, vector2);
					if (num3 < num)
					{
						num = num3;
						position = vector2;
						rotation = housingEdge.rotation;
						result = true;
					}
				}
			}
			return result;
		}

		// Token: 0x06002BFE RID: 11262 RVA: 0x000BCCD8 File Offset: 0x000BAED8
		internal bool FindEmptyPillarSlot(Ray ray, out Vector3 position, out float rotation)
		{
			position = default(Vector3);
			rotation = 0f;
			float num = 64f;
			bool result = false;
			foreach (HousingVertex housingVertex in this.verticesGrid.EnumerateItemsInSquare(ray.origin, 8f))
			{
				if (housingVertex.pillars.IsEmpty<StructureDrop>())
				{
					Vector3 vector = housingVertex.position + new Vector3(0f, 2.125f, 0f);
					float num2 = this.ScorePosition(ray, vector);
					if (num2 < num)
					{
						num = num2;
						position = vector;
						rotation = housingVertex.rotation;
						result = true;
					}
				}
				if (housingVertex.lowerVertex == null || housingVertex.lowerVertex.pillars.IsEmpty<StructureDrop>())
				{
					Vector3 vector2 = housingVertex.position + new Vector3(0f, -2.125f, 0f);
					float num3 = this.ScorePosition(ray, vector2);
					if (num3 < num)
					{
						num = num3;
						position = vector2;
						rotation = housingVertex.rotation;
						result = true;
					}
				}
			}
			return result;
		}

		// Token: 0x06002BFF RID: 11263 RVA: 0x000BCE00 File Offset: 0x000BB000
		private bool SnapFloorPlacementToEdge(bool isRoof, ref Vector3 placementPosition)
		{
			foreach (HousingEdge housingEdge in this.edgesGrid.EnumerateItemsInSquare(placementPosition, 3.02f))
			{
				if ((housingEdge.backwardFloors.IsEmpty<StructureDrop>() || housingEdge.forwardFloors.IsEmpty<StructureDrop>()) && (!isRoof || housingEdge.CanAttachRoof))
				{
					Vector3 vector = housingEdge.position + housingEdge.direction * 3f;
					if (vector.IsNearlyEqual(placementPosition, 0.02f))
					{
						placementPosition = vector;
						return true;
					}
					Vector3 vector2 = housingEdge.position - housingEdge.direction * 3f;
					if (vector2.IsNearlyEqual(placementPosition, 0.02f))
					{
						placementPosition = vector2;
						return true;
					}
				}
			}
			return false;
		}

		/// <summary>
		/// Hack to prevent ignoring floor which might be overlapping pending floor placement.
		/// For example when placing a square floor on the opposite edge of a spot which has a triangular floor
		/// we do not want to ignore the triangular floor during the physics query.
		/// </summary>
		// Token: 0x06002C00 RID: 11264 RVA: 0x000BCF00 File Offset: 0x000BB100
		private void IgnoreVertexFloorsExceptNearPosition(HousingVertex vertex, Vector3 overlapCenter, float pendingItemRadius)
		{
			if (vertex == null)
			{
				return;
			}
			foreach (StructureDrop structureDrop in vertex.floors)
			{
				bool flag = structureDrop.asset.construct == EConstruct.FLOOR_POLY || structureDrop.asset.construct == EConstruct.ROOF_POLY;
				Vector3 a = flag ? structureDrop.model.TransformPoint(0f, 1.2679492f, 0f) : structureDrop.model.position;
				float num = (flag ? 1.7320508f : 3f) * 0.95f;
				float num2 = pendingItemRadius + num;
				float num3 = num2 * num2;
				if ((a - overlapCenter).GetHorizontalSqrMagnitude() > num3)
				{
					this.ignoreDrops.Add(structureDrop);
				}
			}
		}

		// Token: 0x06002C01 RID: 11265 RVA: 0x000BCFDC File Offset: 0x000BB1DC
		private void IgnoreVertexPillarsFloorsAndWalls(HousingVertex vertex)
		{
			if (vertex == null)
			{
				return;
			}
			HashSetEx.AddAny<StructureDrop>(this.ignoreDrops, vertex.pillars);
			HashSetEx.AddAny<StructureDrop>(this.ignoreDrops, vertex.floors);
			foreach (HousingEdge housingEdge in vertex.edges)
			{
				HashSetEx.AddAny<StructureDrop>(this.ignoreDrops, housingEdge.walls);
			}
		}

		// Token: 0x06002C02 RID: 11266 RVA: 0x000BD064 File Offset: 0x000BB264
		private void IgnoreVertexPillarsAndWalls(HousingVertex vertex)
		{
			if (vertex == null)
			{
				return;
			}
			HashSetEx.AddAny<StructureDrop>(this.ignoreDrops, vertex.pillars);
			foreach (HousingEdge housingEdge in vertex.edges)
			{
				HashSetEx.AddAny<StructureDrop>(this.ignoreDrops, housingEdge.walls);
			}
		}

		// Token: 0x06002C03 RID: 11267 RVA: 0x000BD0D8 File Offset: 0x000BB2D8
		private void IgnoreVertexFloors(HousingVertex vertex)
		{
			if (vertex == null)
			{
				return;
			}
			HashSetEx.AddAny<StructureDrop>(this.ignoreDrops, vertex.floors);
		}

		// Token: 0x06002C04 RID: 11268 RVA: 0x000BD0F0 File Offset: 0x000BB2F0
		private bool CanIgnoreOverlaps(int overlapCount, ref string obstructionHint)
		{
			for (int i = 0; i < overlapCount; i++)
			{
				Transform root = this.overlapBuffer[i].transform.root;
				StructureDrop structureDrop = StructureDrop.FindByTransformFastMaybeNull(root);
				if (structureDrop == null)
				{
					BarricadeDrop barricadeDrop = BarricadeDrop.FindByTransformFastMaybeNull(root);
					if (barricadeDrop != null && barricadeDrop.asset != null)
					{
						obstructionHint = barricadeDrop.asset.itemName;
					}
					return false;
				}
				if (!this.ignoreDrops.Contains(structureDrop))
				{
					obstructionHint = structureDrop.asset.itemName;
					return false;
				}
			}
			return true;
		}

		// Token: 0x06002C05 RID: 11269 RVA: 0x000BD168 File Offset: 0x000BB368
		private bool IsFloorAboveGround(Vector3 center, float testHeight)
		{
			RaycastHit hit;
			return Physics.Raycast(center + Vector3.up, Vector3.down, out hit, testHeight + 1f, 1146880) && this.DoesHitCountAsTerrain(hit);
		}

		/// <summary>
		/// Used by triangular floor and roof validation to test for collisions.
		/// </summary>
		// Token: 0x06002C06 RID: 11270 RVA: 0x000BD1A4 File Offset: 0x000BB3A4
		private bool TestTriangleOverlapsCommon(Vector3 center, float placementRotation, float overlapPositionOffset, float overlapHalfHeight, ref string obstructionHint)
		{
			HousingConnections.<>c__DisplayClass64_0 CS$<>8__locals1;
			CS$<>8__locals1.center = center;
			CS$<>8__locals1.overlapPositionOffset = overlapPositionOffset;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.outerHalfExtents = new Vector3(3.02f, overlapHalfHeight, 0.32000002f);
			CS$<>8__locals1.innerHalfExtents = new Vector3(1.9699999f, overlapHalfHeight, 0.59499997f);
			CS$<>8__locals1.characterHalfExtents = new Vector3(3.25f, overlapHalfHeight + 0.25f, 1.15f);
			return this.<TestTriangleOverlapsCommon>g__TestOverlaps|64_0(Quaternion.Euler(0f, placementRotation, 0f), ref obstructionHint, ref CS$<>8__locals1) && this.<TestTriangleOverlapsCommon>g__TestOverlaps|64_0(Quaternion.Euler(0f, placementRotation + 120f, 0f), ref obstructionHint, ref CS$<>8__locals1) && this.<TestTriangleOverlapsCommon>g__TestOverlaps|64_0(Quaternion.Euler(0f, placementRotation - 120f, 0f), ref obstructionHint, ref CS$<>8__locals1);
		}

		// Token: 0x06002C07 RID: 11271 RVA: 0x000BD280 File Offset: 0x000BB480
		internal EHousingPlacementResult ValidateSquareFloorPlacement(float terrainTestHeight, ref Vector3 placementPosition, float placementRotation, ref string obstructionHint)
		{
			this.SnapFloorPlacementToEdge(false, ref placementPosition);
			if (!this.IsFloorAboveGround(placementPosition, terrainTestHeight))
			{
				return EHousingPlacementResult.MissingGround;
			}
			Vector3 vector = placementPosition + new Vector3(0f, -4.975f, 0f);
			Vector3 vector2 = new Vector3(3.02f, 5.075f, 3.02f);
			Quaternion quaternion = Quaternion.Euler(0f, placementRotation, 0f);
			Vector3 vector3 = placementPosition + quaternion * new Vector3(3f, 0f, 3f);
			Vector3 vector4 = placementPosition + quaternion * new Vector3(-3f, 0f, 3f);
			Vector3 vector5 = placementPosition + quaternion * new Vector3(3f, 0f, -3f);
			Vector3 vector6 = placementPosition + quaternion * new Vector3(-3f, 0f, -3f);
			HousingVertex vertex = this.FindVertex(vector3);
			HousingVertex vertex2 = this.FindVertex(vector4);
			HousingVertex vertex3 = this.FindVertex(vector5);
			HousingVertex vertex4 = this.FindVertex(vector6);
			this.ignoreDrops.Clear();
			this.IgnoreVertexPillarsAndWalls(vertex);
			this.IgnoreVertexPillarsAndWalls(vertex2);
			this.IgnoreVertexPillarsAndWalls(vertex3);
			this.IgnoreVertexPillarsAndWalls(vertex4);
			this.IgnoreVertexFloorsExceptNearPosition(vertex, vector, 3f);
			this.IgnoreVertexFloorsExceptNearPosition(vertex2, vector, 3f);
			this.IgnoreVertexFloorsExceptNearPosition(vertex3, vector, 3f);
			this.IgnoreVertexFloorsExceptNearPosition(vertex4, vector, 3f);
			HousingVertex vertex5 = this.FindVertex(vector3 + new Vector3(0f, 4.25f, 0f));
			HousingVertex vertex6 = this.FindVertex(vector4 + new Vector3(0f, 4.25f, 0f));
			HousingVertex vertex7 = this.FindVertex(vector5 + new Vector3(0f, 4.25f, 0f));
			HousingVertex vertex8 = this.FindVertex(vector6 + new Vector3(0f, 4.25f, 0f));
			this.IgnoreVertexFloors(vertex5);
			this.IgnoreVertexFloors(vertex6);
			this.IgnoreVertexFloors(vertex7);
			this.IgnoreVertexFloors(vertex8);
			int overlapCount = Physics.OverlapBoxNonAlloc(vector, vector2, this.overlapBuffer, quaternion, 402653184, QueryTriggerInteraction.Collide);
			if (!this.CanIgnoreOverlaps(overlapCount, ref obstructionHint))
			{
				return EHousingPlacementResult.Obstructed;
			}
			Vector3 halfExtents = vector2 + new Vector3(0.25f, 0.25f, 0.25f);
			if (Physics.CheckBox(vector, halfExtents, quaternion, 83887616, QueryTriggerInteraction.Collide))
			{
				return EHousingPlacementResult.Obstructed;
			}
			return EHousingPlacementResult.Success;
		}

		// Token: 0x06002C08 RID: 11272 RVA: 0x000BD518 File Offset: 0x000BB718
		internal EHousingPlacementResult ValidateTriangleFloorPlacement(float terrainTestHeight, ref Vector3 placementPosition, float placementRotation, ref string obstructionHint)
		{
			this.SnapFloorPlacementToEdge(false, ref placementPosition);
			if (!this.IsFloorAboveGround(placementPosition, terrainTestHeight))
			{
				return EHousingPlacementResult.MissingGround;
			}
			Quaternion rotation = Quaternion.Euler(0f, placementRotation, 0f);
			Vector3 vector = placementPosition + rotation * new Vector3(0f, 0f, -1.2679492f);
			Vector3 vector2 = placementPosition + rotation * new Vector3(3f, 0f, -3f);
			Vector3 vector3 = placementPosition + rotation * new Vector3(-3f, 0f, -3f);
			Vector3 vector4 = placementPosition + rotation * new Vector3(0f, 0f, 2.1961524f);
			HousingVertex vertex = this.FindVertex(vector2);
			HousingVertex vertex2 = this.FindVertex(vector3);
			HousingVertex vertex3 = this.FindVertex(vector4);
			this.ignoreDrops.Clear();
			this.IgnoreVertexPillarsAndWalls(vertex);
			this.IgnoreVertexPillarsAndWalls(vertex2);
			this.IgnoreVertexPillarsAndWalls(vertex3);
			this.IgnoreVertexFloorsExceptNearPosition(vertex, vector, 1.7320508f);
			this.IgnoreVertexFloorsExceptNearPosition(vertex2, vector, 1.7320508f);
			this.IgnoreVertexFloorsExceptNearPosition(vertex3, vector, 1.7320508f);
			HousingVertex vertex4 = this.FindVertex(vector2 + new Vector3(0f, 4.25f, 0f));
			HousingVertex vertex5 = this.FindVertex(vector3 + new Vector3(0f, 4.25f, 0f));
			HousingVertex vertex6 = this.FindVertex(vector4 + new Vector3(0f, 4.25f, 0f));
			this.IgnoreVertexFloors(vertex4);
			this.IgnoreVertexFloors(vertex5);
			this.IgnoreVertexFloors(vertex6);
			if (!this.TestTriangleOverlapsCommon(vector, placementRotation, -4.975f, 5.075f, ref obstructionHint))
			{
				return EHousingPlacementResult.Obstructed;
			}
			return EHousingPlacementResult.Success;
		}

		// Token: 0x06002C09 RID: 11273 RVA: 0x000BD6EC File Offset: 0x000BB8EC
		internal EHousingPlacementResult ValidateSquareRoofPlacement(ref Vector3 placementPosition, float placementRotation, ref string obstructionHint)
		{
			if (!this.SnapFloorPlacementToEdge(true, ref placementPosition))
			{
				return EHousingPlacementResult.MissingSlot;
			}
			Vector3 vector = placementPosition;
			Vector3 vector2 = new Vector3(3.02f, 0.15f, 3.02f);
			Quaternion quaternion = Quaternion.Euler(0f, placementRotation, 0f);
			Vector3 vector3 = placementPosition + quaternion * new Vector3(3f, 0f, 3f);
			Vector3 vector4 = placementPosition + quaternion * new Vector3(-3f, 0f, 3f);
			Vector3 vector5 = placementPosition + quaternion * new Vector3(3f, 0f, -3f);
			Vector3 vector6 = placementPosition + quaternion * new Vector3(-3f, 0f, -3f);
			HousingVertex vertex = this.FindVertex(vector3);
			HousingVertex vertex2 = this.FindVertex(vector4);
			HousingVertex vertex3 = this.FindVertex(vector5);
			HousingVertex vertex4 = this.FindVertex(vector6);
			HousingVertex vertex5 = this.FindVertex(vector3 + new Vector3(0f, -4.25f, 0f));
			HousingVertex vertex6 = this.FindVertex(vector4 + new Vector3(0f, -4.25f, 0f));
			HousingVertex vertex7 = this.FindVertex(vector5 + new Vector3(0f, -4.25f, 0f));
			HousingVertex vertex8 = this.FindVertex(vector6 + new Vector3(0f, -4.25f, 0f));
			this.ignoreDrops.Clear();
			this.IgnoreVertexPillarsAndWalls(vertex);
			this.IgnoreVertexPillarsAndWalls(vertex2);
			this.IgnoreVertexPillarsAndWalls(vertex3);
			this.IgnoreVertexPillarsAndWalls(vertex4);
			this.IgnoreVertexFloorsExceptNearPosition(vertex, vector, 3f);
			this.IgnoreVertexFloorsExceptNearPosition(vertex2, vector, 3f);
			this.IgnoreVertexFloorsExceptNearPosition(vertex3, vector, 3f);
			this.IgnoreVertexFloorsExceptNearPosition(vertex4, vector, 3f);
			this.IgnoreVertexPillarsAndWalls(vertex5);
			this.IgnoreVertexPillarsAndWalls(vertex6);
			this.IgnoreVertexPillarsAndWalls(vertex7);
			this.IgnoreVertexPillarsAndWalls(vertex8);
			HousingVertex vertex9 = this.FindVertex(vector3 + new Vector3(0f, 4.25f, 0f));
			HousingVertex vertex10 = this.FindVertex(vector4 + new Vector3(0f, 4.25f, 0f));
			HousingVertex vertex11 = this.FindVertex(vector5 + new Vector3(0f, 4.25f, 0f));
			HousingVertex vertex12 = this.FindVertex(vector6 + new Vector3(0f, 4.25f, 0f));
			this.IgnoreVertexFloors(vertex9);
			this.IgnoreVertexFloors(vertex10);
			this.IgnoreVertexFloors(vertex11);
			this.IgnoreVertexFloors(vertex12);
			int overlapCount = Physics.OverlapBoxNonAlloc(vector, vector2, this.overlapBuffer, quaternion, 402653184, QueryTriggerInteraction.Collide);
			if (!this.CanIgnoreOverlaps(overlapCount, ref obstructionHint))
			{
				return EHousingPlacementResult.Obstructed;
			}
			Vector3 halfExtents = vector2 + new Vector3(0.25f, 0.25f, 0.25f);
			if (Physics.CheckBox(vector, halfExtents, quaternion, 83887616, QueryTriggerInteraction.Collide))
			{
				return EHousingPlacementResult.Obstructed;
			}
			return EHousingPlacementResult.Success;
		}

		// Token: 0x06002C0A RID: 11274 RVA: 0x000BDA04 File Offset: 0x000BBC04
		internal EHousingPlacementResult ValidateTriangleRoofPlacement(ref Vector3 placementPosition, float placementRotation, ref string obstructionHint)
		{
			if (!this.SnapFloorPlacementToEdge(true, ref placementPosition))
			{
				return EHousingPlacementResult.MissingSlot;
			}
			Quaternion rotation = Quaternion.Euler(0f, placementRotation, 0f);
			Vector3 vector = placementPosition + rotation * new Vector3(0f, 0f, -1.2679492f);
			Vector3 vector2 = placementPosition + rotation * new Vector3(3f, 0f, -3f);
			Vector3 vector3 = placementPosition + rotation * new Vector3(-3f, 0f, -3f);
			Vector3 vector4 = placementPosition + rotation * new Vector3(0f, 0f, 2.1961524f);
			HousingVertex vertex = this.FindVertex(vector2);
			HousingVertex vertex2 = this.FindVertex(vector3);
			HousingVertex vertex3 = this.FindVertex(vector4);
			HousingVertex vertex4 = this.FindVertex(vector2 + new Vector3(0f, -4.25f, 0f));
			HousingVertex vertex5 = this.FindVertex(vector3 + new Vector3(0f, -4.25f, 0f));
			HousingVertex vertex6 = this.FindVertex(vector4 + new Vector3(0f, -4.25f, 0f));
			this.ignoreDrops.Clear();
			this.IgnoreVertexPillarsAndWalls(vertex);
			this.IgnoreVertexPillarsAndWalls(vertex2);
			this.IgnoreVertexPillarsAndWalls(vertex3);
			this.IgnoreVertexFloorsExceptNearPosition(vertex, vector, 1.7320508f);
			this.IgnoreVertexFloorsExceptNearPosition(vertex2, vector, 1.7320508f);
			this.IgnoreVertexFloorsExceptNearPosition(vertex3, vector, 1.7320508f);
			this.IgnoreVertexPillarsAndWalls(vertex4);
			this.IgnoreVertexPillarsAndWalls(vertex5);
			this.IgnoreVertexPillarsAndWalls(vertex6);
			HousingVertex vertex7 = this.FindVertex(vector2 + new Vector3(0f, 4.25f, 0f));
			HousingVertex vertex8 = this.FindVertex(vector3 + new Vector3(0f, 4.25f, 0f));
			HousingVertex vertex9 = this.FindVertex(vector4 + new Vector3(0f, 4.25f, 0f));
			this.IgnoreVertexFloors(vertex7);
			this.IgnoreVertexFloors(vertex8);
			this.IgnoreVertexFloors(vertex9);
			if (!this.TestTriangleOverlapsCommon(vector, placementRotation, 0f, 0.2f, ref obstructionHint))
			{
				return EHousingPlacementResult.Obstructed;
			}
			return EHousingPlacementResult.Success;
		}

		/// <summary>
		/// Ensure wall fits in an empty slot.
		/// </summary>
		// Token: 0x06002C0B RID: 11275 RVA: 0x000BDC48 File Offset: 0x000BBE48
		internal EHousingPlacementResult ValidateWallPlacement(ref Vector3 pendingPlacementPosition, float pivotOffset, bool requiresPillars, bool requiresFullHeightPillars, ref string obstructionHint)
		{
			Vector3 vector = pendingPlacementPosition + new Vector3(0f, -pivotOffset, 0f);
			HousingEdge housingEdge = this.FindEdge(vector);
			Vector3 vector2;
			Vector3 vector3;
			HousingVertex housingVertex;
			HousingVertex housingVertex2;
			float rotation;
			if (housingEdge == null)
			{
				HousingEdge housingEdge2 = this.FindEdge(vector + new Vector3(0f, 4.25f, 0f));
				if (housingEdge2 == null)
				{
					return EHousingPlacementResult.MissingSlot;
				}
				if (housingEdge2.vertex0 == null || housingEdge2.vertex1 == null)
				{
					return EHousingPlacementResult.MissingPillar;
				}
				vector = housingEdge2.position + new Vector3(0f, -4.25f, 0f);
				vector2 = housingEdge2.vertex0.position + new Vector3(0f, -4.25f, 0f);
				vector3 = housingEdge2.vertex1.position + new Vector3(0f, -4.25f, 0f);
				housingVertex = this.FindVertex(vector2);
				housingVertex2 = this.FindVertex(vector3);
				rotation = housingEdge2.rotation;
			}
			else
			{
				if (!housingEdge.walls.IsEmpty<StructureDrop>())
				{
					return EHousingPlacementResult.Obstructed;
				}
				if (housingEdge.vertex0 == null || housingEdge.vertex1 == null)
				{
					return EHousingPlacementResult.MissingPillar;
				}
				vector = housingEdge.position;
				vector2 = housingEdge.vertex0.position;
				vector3 = housingEdge.vertex1.position;
				housingVertex = housingEdge.vertex0;
				housingVertex2 = housingEdge.vertex1;
				rotation = housingEdge.rotation;
			}
			pendingPlacementPosition = vector + new Vector3(0f, pivotOffset, 0f);
			if (requiresPillars)
			{
				if (housingVertex == null || housingVertex2 == null || housingVertex.pillars.IsEmpty<StructureDrop>() || housingVertex2.pillars.IsEmpty<StructureDrop>())
				{
					return EHousingPlacementResult.MissingPillar;
				}
				if (requiresFullHeightPillars && (!housingVertex.HasFullHeightPillar() || !housingVertex2.HasFullHeightPillar()))
				{
					return EHousingPlacementResult.MissingPillar;
				}
			}
			Vector3 position = vector2 + new Vector3(0f, pivotOffset * 2f, 0f);
			Vector3 position2 = vector3 + new Vector3(0f, pivotOffset * 2f, 0f);
			Vector3 position3 = vector + new Vector3(0f, pivotOffset * 2f, 0f);
			if (!UndergroundAllowlist.IsPositionBuildable(position) && !UndergroundAllowlist.IsPositionBuildable(position2) && !UndergroundAllowlist.IsPositionBuildable(position3))
			{
				return EHousingPlacementResult.ObstructedByGround;
			}
			HousingVertex vertex = this.FindVertex(vector2 + new Vector3(0f, 4.25f, 0f));
			HousingVertex vertex2 = this.FindVertex(vector2 + new Vector3(0f, -4.25f, 0f));
			HousingVertex vertex3 = this.FindVertex(vector3 + new Vector3(0f, 4.25f, 0f));
			HousingVertex vertex4 = this.FindVertex(vector3 + new Vector3(0f, -4.25f, 0f));
			Vector3 center = vector + new Vector3(0f, 2.125f, 0f);
			Vector3 vector4 = new Vector3(3.02f, 2.145f, 0.27f);
			Quaternion orientation = Quaternion.Euler(0f, rotation, 0f);
			this.ignoreDrops.Clear();
			this.IgnoreVertexPillarsFloorsAndWalls(housingVertex);
			this.IgnoreVertexPillarsFloorsAndWalls(housingVertex2);
			this.IgnoreVertexPillarsFloorsAndWalls(vertex);
			this.IgnoreVertexPillarsFloorsAndWalls(vertex3);
			this.IgnoreVertexPillarsAndWalls(vertex2);
			this.IgnoreVertexPillarsAndWalls(vertex4);
			HousingVertex vertex5 = this.FindVertex(vector2 + new Vector3(0f, 8.5f, 0f));
			HousingVertex vertex6 = this.FindVertex(vector3 + new Vector3(0f, 8.5f, 0f));
			this.IgnoreVertexFloors(vertex5);
			this.IgnoreVertexFloors(vertex6);
			int overlapCount = Physics.OverlapBoxNonAlloc(center, vector4, this.overlapBuffer, orientation, 402653184, QueryTriggerInteraction.Collide);
			if (!this.CanIgnoreOverlaps(overlapCount, ref obstructionHint))
			{
				return EHousingPlacementResult.Obstructed;
			}
			Vector3 halfExtents = vector4 + new Vector3(0.25f, 0.25f, 0.25f);
			if (Physics.CheckBox(center, halfExtents, orientation, 83887616, QueryTriggerInteraction.Collide))
			{
				return EHousingPlacementResult.Obstructed;
			}
			return EHousingPlacementResult.Success;
		}

		/// <summary>
		/// Ensure pillar fits in an empty slot.
		/// </summary>
		// Token: 0x06002C0C RID: 11276 RVA: 0x000BE02C File Offset: 0x000BC22C
		internal EHousingPlacementResult ValidatePillarPlacement(ref Vector3 pendingPlacementPosition, float pivotOffset, ref string obstructionHint)
		{
			Vector3 vector = pendingPlacementPosition + new Vector3(0f, -pivotOffset, 0f);
			HousingVertex housingVertex = this.FindVertex(vector);
			if (housingVertex == null)
			{
				HousingVertex housingVertex2 = this.FindVertex(vector + new Vector3(0f, 4.25f, 0f));
				if (housingVertex2 == null)
				{
					return EHousingPlacementResult.MissingSlot;
				}
				vector = housingVertex2.position + new Vector3(0f, -4.25f, 0f);
			}
			else
			{
				if (!housingVertex.pillars.IsEmpty<StructureDrop>())
				{
					return EHousingPlacementResult.Obstructed;
				}
				vector = housingVertex.position;
			}
			pendingPlacementPosition = vector + new Vector3(0f, pivotOffset, 0f);
			if (!UndergroundAllowlist.IsPositionBuildable(vector + new Vector3(0f, pivotOffset * 2f, 0f)))
			{
				return EHousingPlacementResult.ObstructedByGround;
			}
			HousingVertex vertex = this.FindVertex(vector + new Vector3(0f, 4.25f, 0f));
			HousingVertex vertex2 = this.FindVertex(vector + new Vector3(0f, -4.25f, 0f));
			Vector3 center = vector + new Vector3(0f, 2.125f, 0f);
			Vector3 vector2 = new Vector3(0.37f, 2.145f, 0.37f);
			this.ignoreDrops.Clear();
			this.IgnoreVertexPillarsFloorsAndWalls(housingVertex);
			this.IgnoreVertexPillarsFloorsAndWalls(vertex);
			this.IgnoreVertexPillarsAndWalls(vertex2);
			HousingVertex vertex3 = this.FindVertex(vector + new Vector3(0f, 8.5f, 0f));
			this.IgnoreVertexFloors(vertex3);
			int overlapCount = Physics.OverlapBoxNonAlloc(center, vector2, this.overlapBuffer, Quaternion.identity, 402653184, QueryTriggerInteraction.Collide);
			if (!this.CanIgnoreOverlaps(overlapCount, ref obstructionHint))
			{
				return EHousingPlacementResult.Obstructed;
			}
			Vector3 halfExtents = vector2 + new Vector3(0.25f, 0.25f, 0.25f);
			if (Physics.CheckBox(center, halfExtents, Quaternion.identity, 83887616, QueryTriggerInteraction.Collide))
			{
				return EHousingPlacementResult.Obstructed;
			}
			return EHousingPlacementResult.Success;
		}

		// Token: 0x06002C0D RID: 11277 RVA: 0x000BE223 File Offset: 0x000BC423
		internal void DrawGizmos()
		{
		}

		/// <summary>
		/// Nelson 2024-06-26: With structure rotation replicated as a quaternion we need to be smarter about extracting
		/// yaw from model transform. Quaternion.eulerAngles.y isn't necessarily the yaw anymore.
		/// </summary>
		// Token: 0x06002C0E RID: 11278 RVA: 0x000BE228 File Offset: 0x000BC428
		internal static float GetModelYaw(Transform modelTransform)
		{
			Vector3 vector = modelTransform.TransformDirection(Vector3.up);
			if (vector.y * vector.y > 0.999f)
			{
				return 0f;
			}
			Vector2 normalized = new Vector2(-vector.z, -vector.x).normalized;
			float num = Mathf.Atan2(normalized.y, normalized.x);
			return 57.29578f * num;
		}

		// Token: 0x06002C0F RID: 11279 RVA: 0x000BE290 File Offset: 0x000BC490
		[CompilerGenerated]
		private bool <TestTriangleOverlapsCommon>g__TestOverlaps|64_0(Quaternion edgeRotation, ref string obstructionHint, ref HousingConnections.<>c__DisplayClass64_0 A_3)
		{
			Vector3 a = edgeRotation * Vector3.forward;
			Vector3 center = A_3.center + new Vector3(0f, A_3.overlapPositionOffset, 0f) + a * -1.4320507f;
			Vector3 center2 = A_3.center + new Vector3(0f, A_3.overlapPositionOffset, 0f) + a * -0.55705076f;
			int overlapCount = Physics.OverlapBoxNonAlloc(center, A_3.outerHalfExtents, this.overlapBuffer, edgeRotation, 402653184, QueryTriggerInteraction.Collide);
			bool flag = this.CanIgnoreOverlaps(overlapCount, ref obstructionHint);
			if (flag)
			{
				int overlapCount2 = Physics.OverlapBoxNonAlloc(center2, A_3.innerHalfExtents, this.overlapBuffer, edgeRotation, 402653184, QueryTriggerInteraction.Collide);
				flag &= this.CanIgnoreOverlaps(overlapCount2, ref obstructionHint);
			}
			return flag && !Physics.CheckBox(A_3.center + new Vector3(0f, A_3.overlapPositionOffset, 0f) + a * -0.8320508f, A_3.characterHalfExtents, edgeRotation, 83887616, QueryTriggerInteraction.Collide);
		}

		/// <summary>
		/// Side length of square and triangular floor/roof.
		/// Walls can be slightly less, but we treat them as if they are the full length.
		/// </summary>
		// Token: 0x04001787 RID: 6023
		public const float EDGE_LENGTH = 6f;

		// Token: 0x04001788 RID: 6024
		public const float HALF_EDGE_LENGTH = 3f;

		// Token: 0x04001789 RID: 6025
		public const float WALL_HEIGHT = 4.25f;

		// Token: 0x0400178A RID: 6026
		public const float HALF_WALL_HEIGHT = 2.125f;

		/// <summary>
		/// Vertical distance from edge center to wall pivot.
		/// </summary>
		// Token: 0x0400178B RID: 6027
		public const float WALL_PIVOT_OFFSET = 2.125f;

		/// <summary>
		/// Vertical distance from edge center to rampart pivot.
		/// </summary>
		// Token: 0x0400178C RID: 6028
		public const float RAMPART_PIVOT_OFFSET = 0.9f;

		// Token: 0x0400178D RID: 6029
		private const float FOUNDATION_HEIGHT = 10.25f;

		// Token: 0x0400178E RID: 6030
		private const float HALF_FOUNDATION_HEIGHT = 5.125f;

		// Token: 0x0400178F RID: 6031
		private const float FOUNDATION_CENTER_OFFSET = -4.875f;

		/// <summary>
		/// If position is nearly equal within this threshold then edges/vertices will connect.
		/// </summary>
		// Token: 0x04001790 RID: 6032
		private const float LINK_TOLERANCE = 0.02f;

		/// <summary>
		/// Maximum distance from player's viewpoint to allow placement.
		/// </summary>
		// Token: 0x04001791 RID: 6033
		internal const float MAX_PLACEMENT_DISTANCE = 16f;

		// Token: 0x04001792 RID: 6034
		internal const float MAX_PLACEMENT_SQR_DISTANCE = 256f;

		/// <summary>
		/// How far to search for empty slot best match.
		/// </summary>
		// Token: 0x04001793 RID: 6035
		private const float MAX_FIND_EMPTY_SLOT_DISTANCE = 8f;

		// Token: 0x04001794 RID: 6036
		private const float MAX_FIND_EMPTY_SLOT_SQR_DISTANCE = 64f;

		/// <summary>
		/// Cosine of the angle between ray direction and direction toward slot must be greater than this.
		/// </summary>
		// Token: 0x04001795 RID: 6037
		private const float MIN_FIND_EMPTY_SLOT_COSINE = 0.9f;

		/// <summary>
		/// When validating item placement expand physics overlap this much.
		/// Useful to ensure slightly-touching overlaps (e.g. pillar touching the pillar above) are handled properly.
		/// </summary>
		// Token: 0x04001796 RID: 6038
		private const float PLACEMENT_OVERLAP_PADDING = 0.02f;

		/// <summary>
		/// Ensure players, vehicles, zombies, animals, etc are not within this distance of pending placement.
		/// </summary>
		// Token: 0x04001797 RID: 6039
		private const float CHARACTER_OVERLAP_PADDING = 0.25f;

		// Token: 0x04001798 RID: 6040
		private const float HALF_CHARACTER_OVERLAP_PADDING = 0.125f;

		/// <summary>
		/// Distance from triangle pivot to apex of triangle.
		/// </summary>
		// Token: 0x04001799 RID: 6041
		private const float TRIANGLE_APEX_PIVOT_OFFSET = 2.1961524f;

		/// <summary>
		/// Radius of circle within triangle edges.
		/// </summary>
		// Token: 0x0400179A RID: 6042
		private const float TRIANGLE_INNER_RADIUS = 1.7320508f;

		/// <summary>
		/// Distance from triangle pivot to center of triangle.
		/// </summary>
		// Token: 0x0400179B RID: 6043
		internal const float TRIANGLE_CENTER_PIVOT_OFFSET = -1.2679492f;

		/// <summary>
		/// Small threshold to allow placing even with existing barricades on the floor.
		/// </summary>
		// Token: 0x0400179C RID: 6044
		private const float FOUNDATION_TOP_MARGIN = 0.1f;

		// Token: 0x0400179D RID: 6045
		private const float HALF_FOUNDATION_TOP_MARGIN = 0.05f;

		// Token: 0x0400179E RID: 6046
		private const float ROOF_THICKNESS = 0.5f;

		// Token: 0x0400179F RID: 6047
		private const float HALF_ROOF_THICKNESS = 0.25f;

		/// <summary>
		/// House overlap is approximately the same size as the housing item's collider(s), and is intended to check whether
		/// any pre-existing barricades or structural items are in the way. For example whether a wall cannot be placed because
		/// there is a storage crate in the way, or if a foundation is blocked by another slightly rotated foundation.
		/// </summary>
		// Token: 0x040017A0 RID: 6048
		private const int HOUSE_OVERLAP_LAYER_MASK = 402653184;

		/// <summary>
		/// Character overlap is slightly larger than the house overlap, and checks whether any players, vehicles, animals, zombies, etc
		/// are nearby. This is necessary because when house and characters were combined in a single physics query it was possible to
		/// stand *just* close enough to step into the collider as it was spawned.
		/// </summary>
		// Token: 0x040017A1 RID: 6049
		private const int CHARACTER_OVERLAP_LAYER_MASK = 83887616;

		// Token: 0x040017A2 RID: 6050
		private readonly Vector3 leftLocalDirection = new Vector3(0.8660254f, -0.5f, 0f);

		// Token: 0x040017A3 RID: 6051
		private readonly Vector3 rightLocalDirection = new Vector3(-0.8660254f, -0.5f, 0f);

		// Token: 0x040017A4 RID: 6052
		private RegionList<HousingEdge> edgesGrid;

		// Token: 0x040017A5 RID: 6053
		private RegionList<HousingVertex> verticesGrid;

		// Token: 0x040017A6 RID: 6054
		private Collider[] overlapBuffer = new Collider[50];

		/// <summary>
		/// Working buffer for placement overlap tests.
		/// </summary>
		// Token: 0x040017A7 RID: 6055
		private HashSet<StructureDrop> ignoreDrops = new HashSet<StructureDrop>();
	}
}
