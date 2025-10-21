﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;

namespace Heads_23
{
    class Agent
    {
        List<Node> path;
        Node current;
        Node target;

        Actor owner;
        
        public int X { get { return Convert.ToInt32(owner.Position.X); } }
        public int Y { get { return Convert.ToInt32(owner.Position.Y); } }
        public Node Target { get { return target; } set { target = value; } }

        // Path Drawing
        Sprite pathSpr;
        protected Vector4 pathColor;

        // Sprite and Lists to Draw and Set Sight Nodes
        Sprite sightSpr;
        public List<Node> SightConeDebug;
        public List<Node> SightCone;

        public Agent(Actor owner)
        {
            this.owner = owner;
            target = null;

            // Path
            pathSpr = new Sprite(0.25f, 0.25f);
            pathSpr.pivot = new Vector2(pathSpr.Width * 0.5f, pathSpr.Height * 0.5f);
            pathColor = new Vector4(0.9f, 0.9f, 0.0f, 1.0f);

            // Sight Cone
            sightSpr = new Sprite(0.9f, 0.9f);
            SightConeDebug = new List<Node>();
            SightCone = new List<Node>();
        }

        public virtual void SetPath(List<Node> newPath)
        {
            path = newPath;

            // Can't reach target but have a path, goes to first path node
            if (target == null && path.Count > 0)
            {
                target = path[0];
                path.RemoveAt(0);
            }
            // Can reach target
            else if (path.Count > 0)
            {
                // Heuristic between current Node and target Node
                int dist = Math.Abs(path[0].X - target.X) + Math.Abs(path[0].Y - target.Y);

                // if dist > 1 means we're actually jumping a Node (diag),
                // so we add it again
                if (dist > 1)
                {
                    path.Insert(0, current);
                }
            }
        }

        public virtual void Update(float speed)
        {
            if (target != null)
            {
                Vector2 destination = new Vector2(target.X, target.Y);
                Vector2 direction = (destination - owner.Position);
                float distance = direction.Length;

                SetSightConeNodes();

                if (distance < 0.01f)
                {
                    current = target;
                    owner.Position = destination;

                    if (path.Count == 0)
                    {
                        target = null;
                        owner.RigidBody.Velocity = Vector2.Zero;
                    }
                    else
                    {
                        target = path[0];
                        path.RemoveAt(0);
                    }
                }
                else
                {
                    owner.Position += direction.Normalized() * speed * Game.DeltaTime;
                    owner.Forward = direction;
                }
            }
        }

        public void SetSightConeNodes()
        {
            SightConeDebug.Clear();
            SightCone.Clear();

            Vector2 dir = owner.Forward.Normalized();

            if ((int)dir.X == 0)
            {
                for (int y = 1; y <= 3; y++)
                {
                    for (int x = -y + 1; x < y; x++)
                    {
                        // 1st It -> x =         0;         y = 1;
                        // 2nd It -> x =     -1; 0; 1;      y = 2;
                        // 3rd It -> x = -2; -1; 0; 1; 2;   y = 3;

                        Node n = ((PlayScene)Game.CurrentScene).map.GetNode((int)owner.Position.X + x, (int)owner.Position.Y + y * (int)owner.Forward.Y);
                        
                        SetVisibleNodes(n);

                        SightConeDebug.Add(n);
                    }
                }
            }
            else if ((int)dir.Y == 0)
            {
                for (int x = 1; x <= 3; x++)
                {
                    for (int y = -x + 1; y < x; y++)
                    {
                        Node n = ((PlayScene)Game.CurrentScene).map.GetNode((int)owner.Position.X + x * (int)owner.Forward.X, (int)owner.Position.Y + y);

                        SetVisibleNodes(n);

                        SightConeDebug.Add(n);
                    }
                }
            }

            //Draw "Correct" Vision Cone
            SetCorrectSightCone();
        }

        void SetVisibleNodes(Node n)
        {
            if (n != null)
            {
                if (n.Cost == int.MaxValue)
                {
                    n.Color = new Vector4(1.0f, 0.0f, 0.0f, 0.01f);
                }
                else
                {
                    n.Color = new Vector4(0.0f, 1.0f, 0.0f, 0.01f);
                    SightCone.Add(n);
                }

                if (((Enemy)owner).Rival != null && n == ((PlayScene)Game.CurrentScene).map.GetNode((int)(((Enemy)owner).Rival.Position.X + 0.5f), (int)(((Enemy)owner).Rival.Position.Y + 0.5f)))
                {
                    n.Color = new Vector4(0.0f, 0.0f, 1.0f, 0.01f);
                    SightCone.Add(n);
                }
            }
        }

        private void SetCorrectSightCone()
        {
            if (SightConeDebug.Count > 0)
            {
                // 1st Node is obscured
                if (SightConeDebug[0].Cost == int.MaxValue)
                {
                    for (int i = 1; i < SightConeDebug.Count; i++)
                    {
                        if (SightConeDebug[i] != null)
                        {
                            SightConeDebug[i].Color = new Vector4(1.0f, 0.0f, 0.0f, 0.01f);
                        }
                    }

                    SightCone.Clear();

                    return;
                }

                // Remove obscured Nodes from visible Nodes
                // Right Node is obscured
                if (SightConeDebug[1].Cost == int.MaxValue)
                {
                    SightConeDebug[4].Color = new Vector4(1.0f, 0.0f, 0.0f, 0.01f);
                    SightConeDebug[5].Color = new Vector4(1.0f, 0.0f, 0.0f, 0.01f);

                    SightCone.Remove(SightConeDebug[4]);
                    SightCone.Remove(SightConeDebug[5]);
                }

                // Middle Node is obscured
                if (SightConeDebug[2].Cost == int.MaxValue)
                {
                    SightConeDebug[6].Color = new Vector4(1.0f, 0.0f, 0.0f, 0.01f);

                    SightCone.Remove(SightConeDebug[6]);
                }

                // Left Node is obscured
                if (SightConeDebug[3].Cost == int.MaxValue)
                {
                    SightConeDebug[7].Color = new Vector4(1.0f, 0.0f, 0.0f, 0.01f);
                    SightConeDebug[8].Color = new Vector4(1.0f, 0.0f, 0.0f, 0.01f);

                    SightCone.Remove(SightConeDebug[7]);
                    SightCone.Remove(SightConeDebug[8]);
                }
            }
        }

        public void DrawPath()
        {
            // Draw Sight Cone Debug
            //foreach (Node n in SightConeDebug)
            //{
            //    if (n != null)
            //    {
            //        sightSpr.position = new Vector2(n.X - sightSpr.Width * 0.5f, n.Y - sightSpr.Height * 0.5f);
            //        sightSpr.DrawColor(n.Color);
            //    }
            //}

            // Draw Actual Sight Cone
            foreach (Node n in SightCone)
            {
                if (n != null)
                {
                    sightSpr.position = new Vector2(n.X - sightSpr.Width * 0.5f, n.Y - sightSpr.Height * 0.5f);
                    sightSpr.DrawColor(n.Color);
                }
            }

            if (path != null && path.Count > 0)
            {
                foreach (Node n in path)
                {
                    pathSpr.position = new Vector2(n.X, n.Y);
                    pathSpr.DrawColor(pathColor);
                }
            }
        }
    }
}
