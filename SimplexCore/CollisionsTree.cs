﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace SimplexCore
{
    public static class CollisionsTree
    {
        public static Dictionary<CollisionPairExtended, CollisionPair> DefinedCollisionPairs = new Dictionary<CollisionPairExtended, CollisionPair>();
        public static List<Type> CollisionActiveTypes = new List<Type>();

        public static void DrawPairsDebug()
        {
            int index = 0;

            Sgml.draw_text(new Vector2(20, 0), "[defined collisions]");

            foreach (var k in DefinedCollisionPairs)
            {
                Sgml.draw_text(new Vector2(20, (index + 1) * 30), k.Key.Object + "(" + k.Key.ColliderName + ")" + " - " + k.Value.Object + "(" + k.Value.ColliderName + ")" + " -->" + k.Key.CollisionAction);
                index++;
            }
        }

        public static void ComputeActiveColliders()
        {
            foreach (var k in DefinedCollisionPairs)
            {
                if (!CollisionActiveTypes.Contains(k.Key.Object))
                {
                    CollisionActiveTypes.Add(k.Key.Object);
                }
            }
        }
    }

    public class CollisionPair
    {
        public Type Object;
        public string ColliderName;
    }

    public class CollisionPairExtended : CollisionPair
    {
        public Action<GameObject, GameObject> CollisionAction;
    }
}
