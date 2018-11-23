﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.PerformanceData;
using System.Runtime.CompilerServices;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SimplexCore;
using SimplexIde;
using static SimplexCore.Sgml;


namespace SimplexResources.Objects
{
    public class Object2 : GameObject
    {       
        public Object2()
        {
            Sprite.TextureSource = "texture";
        }
        
        // Defines what happens once instance is placed in the room editor
        public override void EvtCreate()
        {
           // DrawTest.BackgroundColor = Color.White; //RandomColor();
           
        }

        // This actual GMS-like Create event
        public override void OnCreate()
        {
            Debug.WriteLine("Hello c#");
        }
    }
}