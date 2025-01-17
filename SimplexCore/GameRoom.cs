﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;

namespace SimplexCore
{
    [Serializable]
    public class GameRoom
    {
        public Vector2 Size { get; set; }
        public string Name { get; set; }
        public Vector2 ViewSize { get; set; }
        public List<RoomLayer> Layers { get; set; }
        public bool Persistent { get; set; }

        [XmlIgnore]
        public Rectangle Rect;

        public GameRoom()
        {
            Size = new Vector2(1024, 768);
            Name = "Unnamed room";
            ViewSize = new Vector2(Size.X, Size.Y);
            Layers = new List<RoomLayer>();
            Persistent = false;

            Rect = new Rectangle(Point.Zero, new Point((int)Size.X, (int)Size.Y));
        }
    }
}
