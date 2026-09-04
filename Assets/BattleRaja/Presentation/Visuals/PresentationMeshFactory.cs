using System.Collections.Generic;
using UnityEngine;

namespace BattleRaja.Presentation.Visuals
{
    /// <summary>
    /// Small, allocation-free-after-first-use mesh library for render-only feedback.
    /// The production art pipeline saves authored meshes into the project; this runtime
    /// library is intentionally limited to emergency/readability geometry and never
    /// creates Unity primitive components or gameplay colliders.
    /// </summary>
    public static class PresentationMeshFactory
    {
        private static readonly Dictionary<string, Mesh> Cache = new Dictionary<string, Mesh>(16);

        public static Mesh Box(string name = "PresentationBox")
        {
            return GetOrCreate(name, () => CreateBox(name));
        }

        public static Mesh Cylinder(string name = "PresentationCylinder", int sides = 16)
        {
            return GetOrCreate(name + sides, () => CreateCylinder(name, Mathf.Clamp(sides, 8, 32)));
        }

        public static Mesh FacetedOrb(string name = "PresentationOrb", int rings = 4, int sides = 12)
        {
            return GetOrCreate(name + rings + "x" + sides, () => CreateFacetedOrb(name, Mathf.Clamp(rings, 2, 8), Mathf.Clamp(sides, 8, 24)));
        }

        public static Mesh Ring(string name = "PresentationRing", float innerRadius = 0.42f, float outerRadius = 0.5f, int sides = 24)
        {
            return GetOrCreate(name + innerRadius + "x" + outerRadius + "x" + sides,
                () => CreateRing(name, innerRadius, outerRadius, Mathf.Clamp(sides, 8, 48)));
        }

        public static Mesh Disc(string name = "PresentationDisc", int sides = 32)
        {
            return GetOrCreate(name + sides, () => CreateDisc(name, Mathf.Clamp(sides, 8, 64)));
        }

        /// <summary>
        /// A small six-point crystal used for render-only identity markers and
        /// objective accents.  It intentionally lives beside the emergency mesh
        /// library rather than in the gameplay domain so it can never become a
        /// collider or an authority anchor.
        /// </summary>
        public static Mesh Diamond(string name = "PresentationDiamond")
        {
            return GetOrCreate(name, () => CreateDiamond(name));
        }

        private static Mesh GetOrCreate(string key, System.Func<Mesh> creator)
        {
            if (Cache.TryGetValue(key, out var mesh) && mesh != null) return mesh;
            mesh = creator();
            mesh.hideFlags = HideFlags.HideAndDontSave;
            Cache[key] = mesh;
            return mesh;
        }

        private static Mesh CreateBox(string name)
        {
            var vertices = new[]
            {
                new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(0.5f, -0.5f, -0.5f),
                new Vector3(0.5f, -0.5f, 0.5f), new Vector3(-0.5f, -0.5f, 0.5f),
                new Vector3(-0.5f, 0.5f, -0.5f), new Vector3(0.5f, 0.5f, -0.5f),
                new Vector3(0.5f, 0.5f, 0.5f), new Vector3(-0.5f, 0.5f, 0.5f)
            };
            var triangles = new[]
            {
                0, 2, 1, 0, 3, 2, 4, 5, 6, 4, 6, 7,
                0, 1, 5, 0, 5, 4, 1, 2, 6, 1, 6, 5,
                2, 3, 7, 2, 7, 6, 3, 0, 4, 3, 4, 7
            };
            return CreateMesh(name, vertices, triangles, BoxUv(vertices.Length));
        }

        private static Mesh CreateCylinder(string name, int sides)
        {
            var vertices = new Vector3[sides * 2 + 2];
            var uvs = new Vector2[vertices.Length];
            for (var i = 0; i < sides; i++)
            {
                var angle = i * Mathf.PI * 2f / sides;
                var x = Mathf.Cos(angle) * 0.5f;
                var z = Mathf.Sin(angle) * 0.5f;
                vertices[i] = new Vector3(x, -0.5f, z);
                vertices[sides + i] = new Vector3(x, 0.5f, z);
                uvs[i] = new Vector2((float)i / sides, 0f);
                uvs[sides + i] = new Vector2((float)i / sides, 1f);
            }

            var bottomCenter = sides * 2;
            var topCenter = bottomCenter + 1;
            vertices[bottomCenter] = new Vector3(0f, -0.5f, 0f);
            vertices[topCenter] = new Vector3(0f, 0.5f, 0f);
            uvs[bottomCenter] = new Vector2(0.5f, 0f);
            uvs[topCenter] = new Vector2(0.5f, 1f);

            var triangles = new List<int>(sides * 12);
            for (var i = 0; i < sides; i++)
            {
                var next = (i + 1) % sides;
                triangles.Add(i); triangles.Add(next); triangles.Add(sides + next);
                triangles.Add(i); triangles.Add(sides + next); triangles.Add(sides + i);
                triangles.Add(bottomCenter); triangles.Add(next); triangles.Add(i);
                triangles.Add(topCenter); triangles.Add(sides + i); triangles.Add(sides + next);
            }

            return CreateMesh(name, vertices, triangles.ToArray(), uvs);
        }

        private static Mesh CreateFacetedOrb(string name, int rings, int sides)
        {
            var vertexCount = (rings + 1) * sides + 2;
            var vertices = new Vector3[vertexCount];
            var uvs = new Vector2[vertexCount];
            var top = vertexCount - 2;
            var bottom = vertexCount - 1;
            vertices[top] = Vector3.up * 0.5f;
            vertices[bottom] = Vector3.down * 0.5f;
            uvs[top] = new Vector2(0.5f, 1f);
            uvs[bottom] = new Vector2(0.5f, 0f);
            for (var ring = 0; ring <= rings; ring++)
            {
                var v = (float)(ring + 1) / (rings + 2);
                var latitude = Mathf.PI * v;
                var y = Mathf.Cos(latitude) * 0.5f;
                var radius = Mathf.Sin(latitude) * 0.5f;
                for (var side = 0; side < sides; side++)
                {
                    var angle = side * Mathf.PI * 2f / sides;
                    var index = ring * sides + side;
                    vertices[index] = new Vector3(Mathf.Cos(angle) * radius, y, Mathf.Sin(angle) * radius);
                    uvs[index] = new Vector2((float)side / sides, 1f - v);
                }
            }

            var triangles = new List<int>(sides * (rings + 2) * 6);
            for (var side = 0; side < sides; side++)
            {
                var next = (side + 1) % sides;
                triangles.Add(top); triangles.Add(next); triangles.Add(side);
            }
            for (var ring = 0; ring < rings; ring++)
            {
                for (var side = 0; side < sides; side++)
                {
                    var next = (side + 1) % sides;
                    var a = ring * sides + side;
                    var b = ring * sides + next;
                    var c = (ring + 1) * sides + next;
                    var d = (ring + 1) * sides + side;
                    triangles.Add(a); triangles.Add(b); triangles.Add(c);
                    triangles.Add(a); triangles.Add(c); triangles.Add(d);
                }
            }
            var lastRing = rings * sides;
            for (var side = 0; side < sides; side++)
            {
                var next = (side + 1) % sides;
                triangles.Add(bottom); triangles.Add(lastRing + side); triangles.Add(lastRing + next);
            }

            return CreateMesh(name, vertices, triangles.ToArray(), uvs);
        }

        private static Mesh CreateRing(string name, float innerRadius, float outerRadius, int sides)
        {
            var vertices = new Vector3[sides * 2];
            var uvs = new Vector2[vertices.Length];
            for (var i = 0; i < sides; i++)
            {
                var angle = i * Mathf.PI * 2f / sides;
                var direction = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle));
                vertices[i * 2] = direction * innerRadius;
                vertices[i * 2 + 1] = direction * outerRadius;
                uvs[i * 2] = new Vector2(0f, (float)i / sides);
                uvs[i * 2 + 1] = new Vector2(1f, (float)i / sides);
            }

            var triangles = new int[sides * 6];
            for (var i = 0; i < sides; i++)
            {
                var next = (i + 1) % sides;
                var t = i * 6;
                triangles[t] = i * 2;
                triangles[t + 1] = next * 2;
                triangles[t + 2] = i * 2 + 1;
                triangles[t + 3] = i * 2 + 1;
                triangles[t + 4] = next * 2;
                triangles[t + 5] = next * 2 + 1;
            }
            return CreateMesh(name, vertices, triangles, uvs);
        }

        private static Mesh CreateDisc(string name, int sides)
        {
            var vertices = new Vector3[sides + 1];
            var uvs = new Vector2[vertices.Length];
            vertices[0] = Vector3.zero;
            uvs[0] = new Vector2(0.5f, 0.5f);
            for (var i = 0; i < sides; i++)
            {
                var angle = i * Mathf.PI * 2f / sides;
                vertices[i + 1] = new Vector3(Mathf.Cos(angle) * 0.5f, 0f, Mathf.Sin(angle) * 0.5f);
                uvs[i + 1] = new Vector2(vertices[i + 1].x + 0.5f, vertices[i + 1].z + 0.5f);
            }
            var triangles = new int[sides * 3];
            for (var i = 0; i < sides; i++)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = (i + 1) % sides + 1;
            }
            return CreateMesh(name, vertices, triangles, uvs);
        }

        private static Mesh CreateDiamond(string name)
        {
            var vertices = new[]
            {
                Vector3.up * 0.5f,
                Vector3.right * 0.5f,
                Vector3.forward * 0.5f,
                Vector3.left * 0.5f,
                Vector3.back * 0.5f,
                Vector3.down * 0.5f
            };
            var triangles = new[]
            {
                0, 1, 2, 0, 2, 3, 0, 3, 4, 0, 4, 1,
                5, 2, 1, 5, 3, 2, 5, 4, 3, 5, 1, 4
            };
            return CreateMesh(name, vertices, triangles, BoxUv(vertices.Length));
        }

        private static Vector2[] BoxUv(int count)
        {
            var uv = new Vector2[count];
            for (var i = 0; i < count; i++) uv[i] = new Vector2((i & 1) == 0 ? 0f : 1f, (i & 2) == 0 ? 0f : 1f);
            return uv;
        }

        private static Mesh CreateMesh(string name, Vector3[] vertices, int[] triangles, Vector2[] uvs)
        {
            var mesh = new Mesh { name = name };
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.uv = uvs;
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            return mesh;
        }
    }
}
