﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using SimplexCore;
using static SimplexCore.Sgml;

namespace SimplexResources.Objects
{
    public class oCryoSample : GameObject
    {
        public oCryoSample()
        {
            EditorPath = "Actors";
        }

        public override void EvtCreate()
        {                                                        

        }

        public override void EvtStep()
        {
          Debug.WriteLine(real("2 + 8 + 12 * (4 - 3)", true));
        }
    }
}
