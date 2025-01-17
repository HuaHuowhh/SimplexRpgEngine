﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;

namespace SimplexCore
{
    public class SimplexProjectStructure
    {
       public SimplexProjectInfo Info { get; set; }

       public List<SimplexProjectItem> Objects { get; set; } 
       public List<SimplexProjectItem> Rooms { get; set; }
       public List<SimplexProjectItem> Sprites { get; set; }
       public List<SimplexProjectItem> Tilesets { get; set; }
       public List<SimplexProjectItem> Sounds { get; set; }
       public List<SimplexProjectItem> Shaders { get; set; }
       public List<SimplexProjectItem> Extensions { get; set; }
       public List<SimplexProjectItem> Scripts { get; set; }
       public List<SimplexProjectItem> Paths { get; set; }
       public List<SimplexProjectItem> DataFiles { get; set; }
       public List<SimplexProjectItem> Videos { get; set; }

       [XmlIgnore]
       [JsonIgnore]
       public string[] Parts { get; set; }

       [JsonIgnore]
       [XmlIgnore]
       public string RootPath { get; set; }

       [JsonIgnore]
       [XmlIgnore]
       public string ProjectPath { get; set; }

       [XmlIgnore]
       public List<Type> RoomTypes;
    }

    public class SimplexProjectItem
    {
        public string name { get; set; }
        public string path { get; set; }
        public string tag { get; set; }
        public System.Drawing.Color tagColor { get; set; }
        public System.Drawing.Color nameColor { get; set; }
    }

    public class SimplexProjectInfo
    {
        public float EngineVersion { get; set; }
        public string Author { get; set; }
        public float ProjectVersion { get; set; }
        public string ProjectLoadMessage { get; set; }       
    }
}
