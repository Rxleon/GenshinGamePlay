﻿using Nino.Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace TaoTie
{
    [NinoType(false)][LabelText("*柱体")][Tooltip("支持不完善")]
    public partial class ConfigPrismShape: ConfigShape
    {
        [NinoMember(1)] [LabelText("底")][NotNull]
        public ConfigShape2D ConfigShape2D;
        [NinoMember(2)][MinValue(0.1f)]
        public float Height = 1;

        public override Collider CreateCollider(GameObject obj, bool isTrigger)
        {
            var collider = obj.AddComponent<MeshCollider>();
            collider.convex = true;
            collider.isTrigger = isTrigger;
            collider.sharedMesh = CreateMesh();
            return collider;
        }


        private Mesh CreateMesh()
        {
            Mesh mesh = new Mesh();
            mesh.name = "ZonePolygon";
            mesh.triangles = null;
            mesh.uv = null;
            mesh.vertices = null;
            mesh.tangents = null;

            ListComponent<int> triangles = ListComponent<int>.Create();
            ListComponent<Vector3> vertices = ListComponent<Vector3>.Create();
            ConfigShape2D.GetMeshData(triangles,vertices);
            var count = vertices.Count;
            var countT = triangles.Count;
            
            for (int i = 0; i < countT; i++)
            {
                triangles.Add(triangles[i] + count);
            }

            for (int i = 0; i < count; i++)
            {
                var offset = Vector3.up * Height / 2;
                vertices.Add(vertices[i]+offset);
                vertices[i] -= offset;
                
                triangles.Add(i);
                triangles.Add((i + 1) % count);
                triangles.Add(i + count);
                triangles.Add(i + count);
                triangles.Add((i + 1) % count);
                triangles.Add((i + 1) % count + count);
            }
            
            mesh.vertices = vertices.ToArray();
            mesh.triangles = triangles.ToArray();
            triangles.Dispose();
            vertices.Dispose();
            return mesh;
        }

        public override bool Contains(Vector3 target)
        {
            var y = Height / 2;
            if (target.y < -y || target.y > y) return false;
            var p = new Vector2(target.x, target.z);
            return ConfigShape2D.Contains(p);
        }

        /// <summary>
        /// 线与形状相交
        /// </summary>
        /// <param name="start">转换过坐标系的点</param>
        /// <param name="end">转换过坐标系的点</param>
        /// <returns></returns>
        public override bool ContainsLine(Vector3 start, Vector3 end)
        {
            if (start.y < -Height / 2 && end.y < -Height / 2 || start.y > Height / 2 && end.y > Height / 2)
                return false;
            
            var pS = new Vector2(start.x, start.z);
            var pE = new Vector2(start.x, start.z);
            return ConfigShape2D.ContainsLine(pS, pE);
        }

        public override float Distance(Vector3 target)
        {
            var distance = Mathf.Sqrt(SqrMagnitude(target, out bool inner));
            return inner ? -distance : distance;
        }

        public override float SqrMagnitude(Vector3 target)
        {
            var y = Height / 2;
            var p = new Vector2(target.x, target.z);
            bool bottom = target.y < -y;
            bool up = target.y > y;
            if (bottom || up)
            {
                var sqrH = Mathf.Pow(bottom ? (-y - target.y) : (target.y - y), 2);
                var sqrMagnitude = ConfigShape2D.SqrMagnitude(p, out bool inner2d);
                if (inner2d)
                {
                    return sqrH;
                }
                else
                {
                    return sqrMagnitude + sqrH;
                }
            }
            else
            {
                return ConfigShape2D.SqrMagnitude(p);
            }
        }

        public override float SqrMagnitude(Vector3 target, out bool inner)
        {
            inner = false;
            var y = Height / 2;
            var p = new Vector2(target.x, target.z);
            bool bottom = target.y < -y;
            bool up = target.y > y;
            if (bottom || up)
            {
                var sqrH = Mathf.Pow(bottom ? (-y - target.y) : (target.y - y), 2);
                var sqrMagnitude = ConfigShape2D.SqrMagnitude(p, out bool inner2d);
                if (inner2d)
                {
                    return sqrH;
                }
                else
                {
                    return sqrMagnitude + sqrH;
                }
            }
            else
            {
                return ConfigShape2D.SqrMagnitude(p, out inner);
            }
        }

        public override int RaycastHitInfo(Vector3 pos, Quaternion rot, EntityType[] filter, out HitInfo[] hitInfos)
        {
            var dis = GetAABBRange();
            var res = PhysicsHelper.OverlapBoxNonAllocHitInfo(pos, new Vector3(dis, Height, dis) * 0.5f, rot, filter,
                CheckHitLayerType.OnlyHitBox, out hitInfos);
            //todo: 碰撞判断，此处为粗略计算
            int count = 0;
            for (int i = 0; i < res; i++)
            {
                var collider = PhysicsHelper.Colliders[i];
                if (collider != null)
                {
                    Vector3[] vertex = new Vector3[8]
                    {
                        collider.bounds.min,
                        new Vector3(collider.bounds.min.x,collider.bounds.min.y,collider.bounds.max.z),
                        new Vector3(collider.bounds.min.x,collider.bounds.max.y,collider.bounds.min.z),
                        new Vector3(collider.bounds.max.x,collider.bounds.min.y,collider.bounds.min.z),
                        new Vector3(collider.bounds.max.x,collider.bounds.max.y,collider.bounds.min.z),
                        new Vector3(collider.bounds.max.x,collider.bounds.min.y,collider.bounds.max.z),
                        new Vector3(collider.bounds.min.x,collider.bounds.max.y,collider.bounds.max.z),
                        collider.bounds.max,
                    };
                    for (int j = 0; j < vertex.Length; j++)
                    {
                        var targetPos = PhysicsHelper.Transformation(pos, rot, vertex[j]);
                        if (Contains(targetPos))
                        {
                            hitInfos[count] = hitInfos[i];
                            count++;
                            break;
                        }
                    }
                }
            }
            return count;
        }

        public override float GetAABBRange()
        {
            var d1 = ConfigShape2D.GetAABBRange();
            return Mathf.Sqrt(Height * Height + d1 * d1);
        }
    }
}