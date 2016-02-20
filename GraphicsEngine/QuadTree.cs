using Microsoft.Xna.Framework;
using Sample;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GraphicsEngine
{
    public class QuadTree
    {
        public Vector3 Size { get; set; }
        public BoundingBox Bounds { get; set; }
        public Vector3 Position { get; set; }
        public QuadTree NodeRUF { get; set; }
        public QuadTree NodeRUB { get; set; }
        public QuadTree NodeRDF { get; set; }
        public QuadTree NodeRDB { get; set; }
        public QuadTree NodeLUF { get; set; }
        public QuadTree NodeLUB { get; set; }
        public QuadTree NodeLDF { get; set; }
        public QuadTree NodeLDB { get; set; }
        public int ObjectLimit { get; set; }
        public List<SimpleModel> Objects = new List<SimpleModel>();
        public List<QuadTree> Nodes = new List<QuadTree>();
        public bool Visible = true;
        public List<SimpleModel> ActiveObjects = new List<SimpleModel>();
        public List<SimpleModel> ChildObjects = new List<SimpleModel>();

        public QuadTree(Vector3 position, Vector3 size)
        {
            Size = size;
            Position = position;
            Bounds = new BoundingBox(Position - Size / 2, (Position - Size / 2) + Size);
            ObjectLimit = 0;
            DebugEngine.AddBoundingBox(Bounds, Color.Yellow, 1000);

        }
        public void SubDivide()
        {
            NodeLDB = new QuadTree(Position + (new Vector3(-Size.X / 4, -Size.Y / 4, -Size.Z / 4)), Size / 2);
            NodeLDF = new QuadTree(Position + (new Vector3(-Size.X / 4, -Size.Y / 4, Size.Z / 4)), Size / 2);

            NodeLUB = new QuadTree(Position + (new Vector3(-Size.X / 4, Size.Y / 4, -Size.Z / 4)), Size / 2);
            NodeLUF = new QuadTree(Position + (new Vector3(-Size.X / 4, Size.Y / 4, Size.Z / 4)), Size / 2);

            NodeRDB = new QuadTree(Position + (new Vector3(Size.X / 4, -Size.Y / 4, -Size.Z / 4)), Size / 2);
            NodeRDF = new QuadTree(Position + (new Vector3(Size.X / 4, -Size.Y / 4, Size.Z / 4)), Size / 2);

            NodeRUB = new QuadTree(Position + (new Vector3(Size.X / 4, Size.Y / 4, -Size.Z / 4)), Size / 2);
            NodeRUF = new QuadTree(Position + (new Vector3(Size.X / 4, Size.Y / 4, Size.Z / 4)), Size / 2);

            Nodes.Add(NodeLDB);
            Nodes.Add(NodeLDF);
            Nodes.Add(NodeLUB);
            Nodes.Add(NodeLUF);
            Nodes.Add(NodeRDB);
            Nodes.Add(NodeRDF);
            Nodes.Add(NodeRUB);
            Nodes.Add(NodeRUF);


            foreach (QuadTree node in Nodes)
            {
                DebugEngine.AddBoundingBox(node.Bounds, Color.Yellow, 1000);
            }
        }
        public void AddObject(SimpleModel model)
       {
            if (Nodes.Count == 0)
            {
                if (Objects.Count > ObjectLimit)
                {
                    SubDivide();
                    Distribute(model);
                    foreach (SimpleModel Object in Objects)
                        Distribute(Object);
                    Objects.Clear();
                }
                else
                {
                    Objects.Add(model);
                }
            }
            else
            {
                Distribute(model);
                Objects.Clear();
            }
        }
        public void Distribute(SimpleModel model)
        {

            foreach (QuadTree node in Nodes)
            {
                if (node.Bounds.Contains(model.World.Translation) != ContainmentType.Disjoint)
                {
                    node.AddObject(model);
                    break;
                }
            }
        }
        public List<SimpleModel> Process(BoundingFrustum Frustum)
        {
            ActiveObjects.Clear();
            if (Nodes.Count == 0)
            {
                return Objects;
            }
            else
            {
                List<SimpleModel> ChildObject = new List<SimpleModel>();
                foreach (QuadTree node in Nodes)
                {
                    ChildObjects = node.Process(Frustum);
                    foreach (SimpleModel Object in ChildObjects)
                    {
                        ActiveObjects.Add(Object);
                    }
                }
                return ActiveObjects;
            }
        }
        public void Clear()
        {
            Objects.Clear();
            foreach (QuadTree node in Nodes)
            {
                ClearNode(node);
            }
            Nodes.Clear();
        }
        public void ClearNode(QuadTree node)
        {
            if (node != null)
            {
                node.Clear();
                node = null;
            }
        }
    }
}