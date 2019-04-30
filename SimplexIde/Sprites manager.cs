﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DarkUI.Controls;
using DarkUI.Docking;
using DarkUI.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MoreLinq;
using Newtonsoft.Json;
using SimplexCore;
using Color = System.Drawing.Color;
using Rectangle = System.Drawing.Rectangle;
using static SimplexCore.Sgml;
using MouseButtons = System.Windows.Forms.MouseButtons;
using Point = SharpDX.Point;

namespace SimplexIde
{
    public partial class Sprites_manager : DarkToolWindow
    {
        public Form1 owner;
        public DarkTreeNode selectedNode = null;
        public Bitmap lastImage = null;
        public int cellW = 0;
        public int cellH = 0;
        public int rows = 0;
        public List<string> okEntries = new List<string>();
        public List<string> badEntries = new List<string>();
        Pen p = new Pen(Color.White);
        SolidBrush b = new SolidBrush(Color.FromArgb(64, Color.White));
        public double zoom = 1;
        int _totalDelta = 0;
        Vector2 mouse = Vector2.Zero;
        List<Subsprite> subsprites = new List<Subsprite>();
        private Subsprite selectedSub = null;
        private int toolMode = 0;
        private DarkTreeNode lastNode = null;
        private int lastSheetX = -1;
        private int lastSheetY = -1;
        private Tileset s;
        private float CamX = 0;
        float CamY = 0;
        public bool drawModeOn = false;
        public List<MgcbEntry> MgcbEntries = new List<MgcbEntry>();
        public Graphics WinGraphics;
        public SpritesRenderPreviewForm srpf = new SpritesRenderPreviewForm();
        private int mouseX = 0;
        public Sprites_manager()
        {
            InitializeComponent();
            this.MouseWheel += Form_MouseWheel;
        }

        private void Sprites_manager_Load(object sender, EventArgs e)
        {
            if (owner.currentProject != null)
            {
               // owner.sr.drawTest1.UpdateRunning = false;
            }

            SimplexIdeApi.TreeAddRootNode(darkTreeView1, "Layers");
            DarkTreeNode dtn = SimplexIdeApi.TreeCreateNode("Layer 1", "Layers", "layer", "", null, null, Properties.Resources.ObjectFile_16x);

            darkTreeView1.Nodes[0].Nodes.Add(dtn);

            spritesEditorRenderer1.parentForm = this;
            darkMouseTool1.LeftColor = Color.White;
            darkMouseTool1.RightColor = Color.FromArgb(0, 0, 1);
            /*  if (owner.currentProject != null)
              {
                //  owner.sr.drawTest1.UpdateRunning = false;
  
                  // first we load descriptor for all sprites
                  // Sprites = JsonConvert.DeserializeObject<List<Spritesheet>>(new StreamReader("../../../SimplexRpgEngine3/SpritesDescriptor.json").ReadToEnd());
                  var item0 = new DarkDropdownItem("Autotiling");
                  darkDropdownList2.Items.Add(item0);
                  darkDropdownList2.Items.Add(new DarkDropdownItem("Prefabs"));
                  darkDropdownList2.Items.Add(new DarkDropdownItem("Instancemasking"));
                  darkDropdownList2.SelectedItem = item0;
  
                  string[] extensions = {".png", ".jpg", ".jpeg", ".gif"};
                  foreach (string file in Directory.EnumerateFiles(owner.currentProject.RootPath + "/Content/bin/Windows/Sprites/", "*.xnb*"))
                  {
                      string name = Path.GetFileNameWithoutExtension(file);
  
                      if (owner.sr.drawTest1.Sprites.FindIndex(x => x.Name == name) != -1)
                      {
                          okEntries.Add(name);
                      }
                      else
                      {
                          badEntries.Add(name);
                      }
                  }
  
                  spritesEditorRenderer1.mainForm = RoomEditor;
                  spritesEditorRenderer1.parentForm = this;
  
                  DarkTreeNode dtn = new DarkTreeNode("Sprites");
                  dtn.Icon = (Bitmap) Properties.Resources.Folder_16x;
                  dtn.Tag = "folder";
                  darkTreeView1.Nodes.Add(dtn);
  
                  DarkTreeNode dtn2 = new DarkTreeNode("Tilesets");
                  dtn2.Icon = (Bitmap) Properties.Resources.Folder_16x;
                  dtn2.Tag = "folder";
                  darkTreeView1.Nodes.Add(dtn2);
  
                  var newList = okEntries.Concat(badEntries);
                  foreach (string s in newList)
                  {
                      DarkTreeNode d = new DarkTreeNode(s);
                      d.Tag = "item";
                      d.Icon = (Bitmap) Properties.Resources.EditCommandColumn_ActionGray_16x_32;
  
                      if (okEntries.FindIndex(x => x == d.Text) != -1)
                      {
                          d.Icon = (Bitmap) Properties.Resources.Checkmark_16x;
                      }
  
                      darkTreeView1.Nodes[0].Nodes.Add(d);
                  }
  
                  okEntries.Clear();
                  badEntries.Clear();
                  // load tilesets
                  foreach (string file in Directory.EnumerateFiles(owner.currentProject.RootPath + "/Content/bin/Windows/Sprites/Tilesets/", "*.xnb*"))
                  {
                      string name = Path.GetFileNameWithoutExtension(file);
  
                      if (owner.sr.drawTest1.Sprites.FindIndex(x => x.Name == name) != -1)
                      {
                          okEntries.Add(name);
                      }
                      else
                      {
                          badEntries.Add(name);
                      }
                  }
  
                  newList = okEntries.Concat(badEntries);
                  foreach (string s in newList)
                  {
                      DarkTreeNode d = new DarkTreeNode(s);
                      d.Tag = "item";
                      d.Icon = (Bitmap) Properties.Resources.EditCommandColumn_ActionGray_16x_32;
  
                      if (okEntries.FindIndex(x => x == d.Text) != -1)
                      {
                          d.Icon = (Bitmap) Properties.Resources.Checkmark_16x;
                      }
  
                      darkTreeView1.Nodes[1].Nodes.Add(d);
                  }
  
                  darkTreeView1.Nodes[0].Expanded = true;
                  darkTreeView1.Nodes[1].Expanded = true;
              }*/
        }

        private void darkTreeView1_Click(object sender, EventArgs e)
        {
            
        }

        private void darkTreeView1_SelectedNodesChanged(object sender, EventArgs e)
        {
            /*
            if (darkTreeView1.SelectedNodes.Count > 0)
            {
                selectedNode = darkTreeView1.SelectedNodes[0];

                if (lastNode != selectedNode)
                {
                    if ((string) selectedNode.Tag != "folder")
                    {
                        // decide if we are under sprites or tilesets
                        if (selectedNode.ParentNode.Text == "Sprites")
                        {
                            MemoryStream memoryStream = new MemoryStream();

                            try
                            {
                                // for known textures
                                Spritesheet s =
                                    owner.sr.drawTest1.Sprites.FirstOrDefault(x => x.Name == selectedNode.Text);
                                s.Texture.SaveAsPng(memoryStream, s.Texture.Width, s.Texture.Height);

                                darkNumericUpDown3.Value = s.Rows;
                                darkNumericUpDown1.Value = s.CellWidth;
                                darkNumericUpDown2.Value = s.CellHeight;

                                spritesEditorRenderer1.selectedImage = s.Texture;
                            }
                            catch (Exception ee)
                            {
                                // we need to load otherwise
                                Texture2D tex = owner.sr.drawTest1.Editor.Content.Load<Texture2D>(
                                    Path.GetFullPath(owner.projectFile + "/Content/bin/Windows/Sprites/" +
                                                     selectedNode.Text));
                                tex.SaveAsPng(memoryStream, tex.Width, tex.Height);
                                spritesEditorRenderer1.selectedImage = tex;
                            }


                            lastImage = new Bitmap(memoryStream);
                         
                        }
                        else
                        {
                            // tilesets
                            MemoryStream memoryStream = new MemoryStream();

                            try
                            {
                                // for known textures
                                s = owner.sr.drawTest1.tilesets.FirstOrDefault(x => x.Name == selectedNode.Text);
                                s.Texture.SaveAsPng(memoryStream, s.Texture.Width, s.Texture.Height);

                                foreach (AutotileDefinition ad in s.AutotileLib)
                                {
                                    darkDropdownList1.Items.Add(new DarkDropdownItem(ad.Name + " [X: " + ad.X + " Y: " + ad.Y + "]"));
                                }

                                spritesEditorRenderer1.selectedImage = s.Texture;
                            }
                            catch (Exception ee)
                            {
                                // we need to load otherwise
                                Texture2D tex = owner.sr.drawTest1.Editor.Content.Load<Texture2D>(
                                    Path.GetFullPath(owner.projectFile + "/Content/bin/Windows/Sprites/Tilesets/" +
                                        selectedNode.Text));
                                tex.SaveAsPng(memoryStream, tex.Width, tex.Height);
                                spritesEditorRenderer1.selectedImage = tex;
                            }


                            lastImage = new Bitmap(memoryStream);
                            toolMode = 2;
                        }
                    }
                }

                lastNode = selectedNode;
            }*/
        }

        private void darkSectionPanel2_Paint(object sender, PaintEventArgs e)
        {
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.Enabled = true;
            
            //Invalidate();
        }

        private void Sprites_manager_Paint(object sender, PaintEventArgs e)
        {
        }

        private void darkButton1_Click(object sender, EventArgs e)
        {
            // save descriptor
            if (toolMode == 0)
            {
                StreamReader sw = new StreamReader(owner.currentProject.RootPath + "/SpritesDescriptor.json");
                List<Spritesheet> current = JsonConvert.DeserializeObject<List<Spritesheet>>(sw.ReadToEnd());
                sw.Close();

                Spritesheet overwrite = null;

                if (selectedNode != null)
                {
                    overwrite = current.FirstOrDefault(x => x.Name == selectedNode.Text);
                }

                if (overwrite != null)
                {
                    overwrite.CellHeight = cellH;
                    overwrite.CellWidth = cellW;
                    overwrite.Rows = rows;
                }
                else
                {
                    Spritesheet s = new Spritesheet();
                    s.Name = selectedNode.Text;
                    s.CellHeight = cellH;
                    s.CellWidth = cellW;
                    s.Rows = rows;

                    okEntries.Add(s.Name);
                    badEntries.Remove(s.Name);

                    selectedNode.Icon = (Bitmap) Properties.Resources.Checkmark_16x;
                 //   darkTreeView1.Invalidate();

                    current.Add(s);
                }

                string json = JsonConvert.SerializeObject(current);

                using (StreamWriter writer = new StreamWriter(owner.currentProject.RootPath + "/SpritesDescriptor.json"))
                {
                    writer.WriteLine(json);
                    writer.Close();
                }
            }
        }

        private void Sprites_manager_MouseClick(object sender, MouseEventArgs e)
        {/*
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                if (toolMode == 2)
                {
                    // prompt user to define new autotile
                    //string ss = get_string("", "Name this autotile:");
                    if (rows > 0 && cellH > 0 && cellW > 0)
                    {
                        int cx, cy;
                        cx = darkSectionPanel1.Location.X + darkSectionPanel1.Width + 20;
                        cy = darkSectionPanel1.Location.Y;

                        int xIndex = 0;
                        int yIndex = 0;
                        for (var i = 0; i < rows; i++)
                        {
                            for (var j = 0; j < lastImage.Width / cellW; j++)
                            {
                                bool hit = false;

                                Rectangle r = new Rectangle((int)(cx * 1), (int)(cy * 1), (int)(cellW * zoom), (int)(cellH * zoom));

                                if (r.Contains(new System.Drawing.Point((int)mouse.X, (int)mouse.Y)))
                                {
                                    hit = true;
                                }

                                if (hit)
                                {
                                    lastSheetX = xIndex;
                                    lastSheetY = yIndex;
                                }

                                cx += (int)(cellW * zoom);
                                xIndex++;
                            }

                            yIndex++;
                            xIndex = 0;
                            cy += (int)Math.Round(cellH * zoom);
                            cx = darkSectionPanel1.Location.X + darkSectionPanel1.Width + 20;
                        }
                    }
                }
            }
            else if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                if (rows > 0 && cellH > 0 && cellW > 0)
                {
                    int cx, cy;
                    cx = darkSectionPanel1.Location.X + darkSectionPanel1.Width + 20;
                    cy = darkSectionPanel1.Location.Y;

                    int xIndex = 0;
                    int yIndex = 0;
                    for (var i = 0; i < rows; i++)
                    {
                        for (var j = 0; j < lastImage.Width / cellW; j++)
                        {
                            bool hit = false;

                            Rectangle r = new Rectangle((int)(cx * 1), (int)(cy * 1), (int)(cellW * zoom), (int)(cellH * zoom));

                            if (r.Contains(new System.Drawing.Point((int)mouse.X, (int)mouse.Y)))
                            {
                                hit = true;
                            }

                            if (hit)
                            {
                                if (toolMode == 0)
                                {
                                    if (selectedSub == null)
                                    {
                                        Subsprite s = new Subsprite();
                                        s.cellX = xIndex;
                                        s.cellY = yIndex;

                                        subsprites.Add(s);
                                        selectedSub = s;

                                        darkButton2.Visible = true;
                                    }
                                    else
                                    {
                                        selectedSub.cellW = xIndex - selectedSub.cellX;
                                        selectedSub.cellH = yIndex - selectedSub.cellY;
                                        selectedSub = null;
                                    }
                                }

                                if (toolMode == 1)
                                {
                                    SavePartialBitmap(lastImage, Path.GetFullPath("../../../SimplexRpgEngine3/Content/Sprites/Tilesets/tilesetSource0.png"), xIndex * cellW, yIndex * cellH, cellW, cellH, System.Drawing.Imaging.ImageFormat.Png);

                                    lastImage = new Bitmap(Path.GetFullPath("../../../SimplexRpgEngine3/Content/Sprites/Tilesets/tilesetSource0.png"));
                                    toolMode = 2;

                                    Bitmap convertedImage = new Bitmap(256, 192);
                                    #region Algorithm logic

                                    CopyRegionIntoImage(lastImage, new Rectangle(16, 48, 32, 32), ref convertedImage, new Rectangle(0, 0, 32, 32));

                                    CopyRegionIntoImage(lastImage, new Rectangle(16, 48, 32, 32), ref convertedImage, new Rectangle(32, 0, 32, 32));
                                    CopyRegionIntoImage(lastImage, new Rectangle(32, 0, 16, 16), ref convertedImage, new Rectangle(32, 0, 16, 16));

                                    CopyRegionIntoImage(lastImage, new Rectangle(16, 48, 32, 32), ref convertedImage, new Rectangle(64, 0, 32, 32));
                                    CopyRegionIntoImage(lastImage, new Rectangle(48, 0, 16, 16), ref convertedImage, new Rectangle(80, 0, 16, 16));

                                    CopyRegionIntoImage(lastImage, new Rectangle(16, 48, 32, 32), ref convertedImage, new Rectangle(96, 0, 32, 32));
                                    CopyRegionIntoImage(lastImage, new Rectangle(32, 0, 16, 16), ref convertedImage, new Rectangle(96, 0, 16, 16));
                                    CopyRegionIntoImage(lastImage, new Rectangle(48, 0, 16, 16), ref convertedImage, new Rectangle(112, 0, 16, 16));

                                    CopyRegionIntoImage(lastImage, new Rectangle(16, 48, 32, 32), ref convertedImage, new Rectangle(128, 0, 32, 32));
                                    CopyRegionIntoImage(lastImage, new Rectangle(48, 16, 16, 16), ref convertedImage, new Rectangle(144, 16, 16, 16));

                                    CopyRegionIntoImage(lastImage, new Rectangle(16, 48, 32, 32), ref convertedImage, new Rectangle(160, 0, 32, 32));
                                    CopyRegionIntoImage(lastImage, new Rectangle(48, 16, 16, 16), ref convertedImage, new Rectangle(176, 16, 16, 16));
                                    CopyRegionIntoImage(lastImage, new Rectangle(32, 0, 16, 16), ref convertedImage, new Rectangle(160, 0, 16, 16));

                                    CopyRegionIntoImage(lastImage, new Rectangle(16, 48, 32, 32), ref convertedImage, new Rectangle(192, 0, 32, 32));
                                    CopyRegionIntoImage(lastImage, new Rectangle(48, 0, 16, 16), ref convertedImage, new Rectangle(208, 0, 16, 16));
                                    CopyRegionIntoImage(lastImage, new Rectangle(48, 16, 16, 16), ref convertedImage, new Rectangle(208, 16, 16, 16));

                                    CopyRegionIntoImage(lastImage, new Rectangle(16, 48, 32, 32), ref convertedImage, new Rectangle(224, 0, 32, 32));
                                    CopyRegionIntoImage(lastImage, new Rectangle(48, 0, 16, 16), ref convertedImage, new Rectangle(240, 0, 16, 16));
                                    CopyRegionIntoImage(lastImage, new Rectangle(48, 16, 16, 16), ref convertedImage, new Rectangle(240, 16, 16, 16));
                                    CopyRegionIntoImage(lastImage, new Rectangle(32, 0, 16, 16), ref convertedImage, new Rectangle(224, 0, 16, 16));

                                    CopyRegionIntoImage(lastImage, new Rectangle(16, 48, 32, 32), ref convertedImage, new Rectangle(0, 32, 32, 32));
                                    CopyRegionIntoImage(lastImage, new Rectangle(32, 16, 16, 16), ref convertedImage, new Rectangle(0, 48, 16, 16));

                                    CopyRegionIntoImage(lastImage, new Rectangle(16, 48, 32, 32), ref convertedImage, new Rectangle(32, 32, 32, 32));
                                    CopyRegionIntoImage(lastImage, new Rectangle(32, 0, 16, 32), ref convertedImage, new Rectangle(32, 32, 16, 32));

                                    CopyRegionIntoImage(lastImage, new Rectangle(16, 48, 32, 32), ref convertedImage, new Rectangle(64, 32, 32, 32));
                                    CopyRegionIntoImage(lastImage, new Rectangle(48, 0, 16, 16), ref convertedImage, new Rectangle(80, 32, 16, 16));
                                    CopyRegionIntoImage(lastImage, new Rectangle(32, 16, 16, 16), ref convertedImage, new Rectangle(64, 48, 16, 16));

                                    CopyRegionIntoImage(lastImage, new Rectangle(16, 48, 32, 32), ref convertedImage, new Rectangle(96, 32, 32, 32));
                                    CopyRegionIntoImage(lastImage, new Rectangle(48, 0, 16, 16), ref convertedImage, new Rectangle(112, 32, 16, 16));
                                    CopyRegionIntoImage(lastImage, new Rectangle(32, 16, 16, 16), ref convertedImage, new Rectangle(96, 48, 16, 16));
                                    CopyRegionIntoImage(lastImage, new Rectangle(32, 0, 16, 16), ref convertedImage, new Rectangle(96, 32, 16, 16));

                                    CopyRegionIntoImage(lastImage, new Rectangle(16, 48, 32, 32), ref convertedImage, new Rectangle(128, 32, 32, 32));
                                    CopyRegionIntoImage(lastImage, new Rectangle(32, 16, 32, 16), ref convertedImage, new Rectangle(128, 48, 32, 16));

                                    CopyRegionIntoImage(lastImage, new Rectangle(16, 48, 32, 32), ref convertedImage, new Rectangle(160, 32, 32, 32));
                                    CopyRegionIntoImage(lastImage, new Rectangle(32, 16, 32, 16), ref convertedImage, new Rectangle(160, 48, 32, 16));
                                    CopyRegionIntoImage(lastImage, new Rectangle(32, 0, 16, 16), ref convertedImage, new Rectangle(160, 32, 16, 16));

                                    CopyRegionIntoImage(lastImage, new Rectangle(16, 48, 32, 32), ref convertedImage, new Rectangle(192, 32, 32, 32));
                                    CopyRegionIntoImage(lastImage, new Rectangle(32, 16, 32, 16), ref convertedImage, new Rectangle(192, 48, 32, 16));
                                    CopyRegionIntoImage(lastImage, new Rectangle(48, 0, 16, 16), ref convertedImage, new Rectangle(208, 32, 16, 16));

                                    CopyRegionIntoImage(lastImage, new Rectangle(32, 0, 32, 32), ref convertedImage, new Rectangle(224, 32, 32, 32));

                                    CopyRegionIntoImage(lastImage, new Rectangle(0, 48, 32, 32), ref convertedImage, new Rectangle(0, 64, 32, 32));

                                    CopyRegionIntoImage(lastImage, new Rectangle(0, 48, 32, 32), ref convertedImage, new Rectangle(32, 64, 32, 32));
                                    CopyRegionIntoImage(lastImage, new Rectangle(48, 0, 16, 16), ref convertedImage, new Rectangle(48, 64, 16, 16));

                                    CopyRegionIntoImage(lastImage, new Rectangle(0, 48, 32, 32), ref convertedImage, new Rectangle(64, 64, 32, 32));
                                    CopyRegionIntoImage(lastImage, new Rectangle(48, 16, 16, 16), ref convertedImage, new Rectangle(80, 80, 16, 16));

                                    CopyRegionIntoImage(lastImage, new Rectangle(0, 48, 32, 32), ref convertedImage, new Rectangle(96, 64, 32, 32));
                                    CopyRegionIntoImage(lastImage, new Rectangle(48, 16, 16, 16), ref convertedImage, new Rectangle(112, 80, 16, 16));
                                    CopyRegionIntoImage(lastImage, new Rectangle(48, 0, 16, 16), ref convertedImage, new Rectangle(112, 64, 16, 16));

                                    CopyRegionIntoImage(lastImage, new Rectangle(16, 32, 32, 32), ref convertedImage, new Rectangle(128, 64, 32, 32));

                                    CopyRegionIntoImage(lastImage, new Rectangle(16, 32, 32, 32), ref convertedImage, new Rectangle(160, 64, 32, 32));
                                    CopyRegionIntoImage(lastImage, new Rectangle(48, 16, 16, 16), ref convertedImage, new Rectangle(176, 80, 16, 16));

                                    CopyRegionIntoImage(lastImage, new Rectangle(16, 32, 32, 32), ref convertedImage, new Rectangle(192, 64, 32, 32));
                                    CopyRegionIntoImage(lastImage, new Rectangle(32, 16, 16, 16), ref convertedImage, new Rectangle(192, 80, 16, 16));

                                    CopyRegionIntoImage(lastImage, new Rectangle(16, 32, 32, 32), ref convertedImage, new Rectangle(224, 64, 32, 32));
                                    CopyRegionIntoImage(lastImage, new Rectangle(48, 16, 16, 16), ref convertedImage, new Rectangle(240, 80, 16, 16));
                                    CopyRegionIntoImage(lastImage, new Rectangle(32, 16, 16, 16), ref convertedImage, new Rectangle(224, 80, 16, 16));

                                    CopyRegionIntoImage(lastImage, new Rectangle(32, 48, 32, 32), ref convertedImage, new Rectangle(0, 96, 32, 32));

                                    CopyRegionIntoImage(lastImage, new Rectangle(32, 48, 32, 32), ref convertedImage, new Rectangle(32, 96, 32, 32));
                                    CopyRegionIntoImage(lastImage, new Rectangle(32, 16, 16, 16), ref convertedImage, new Rectangle(32, 112, 16, 16));

                                    CopyRegionIntoImage(lastImage, new Rectangle(32, 48, 32, 32), ref convertedImage, new Rectangle(64, 96, 32, 32));
                                    CopyRegionIntoImage(lastImage, new Rectangle(32, 0, 16, 16), ref convertedImage, new Rectangle(64, 96, 16, 16));

                                    CopyRegionIntoImage(lastImage, new Rectangle(32, 48, 32, 32), ref convertedImage, new Rectangle(96, 96, 32, 32));
                                    CopyRegionIntoImage(lastImage, new Rectangle(32, 16, 16, 16), ref convertedImage, new Rectangle(96, 112, 16, 16));
                                    CopyRegionIntoImage(lastImage, new Rectangle(32, 0, 16, 16), ref convertedImage, new Rectangle(96, 96, 16, 16));

                                    CopyRegionIntoImage(lastImage, new Rectangle(16, 64, 32, 32), ref convertedImage, new Rectangle(128, 96, 32, 32));

                                    CopyRegionIntoImage(lastImage, new Rectangle(16, 64, 32, 32), ref convertedImage, new Rectangle(160, 96, 32, 32));
                                    CopyRegionIntoImage(lastImage, new Rectangle(32, 0, 16, 16), ref convertedImage, new Rectangle(160, 96, 16, 16));

                                    CopyRegionIntoImage(lastImage, new Rectangle(16, 64, 32, 32), ref convertedImage, new Rectangle(192, 96, 32, 32));
                                    CopyRegionIntoImage(lastImage, new Rectangle(48, 0, 16, 16), ref convertedImage, new Rectangle(208, 96, 16, 16));

                                    CopyRegionIntoImage(lastImage, new Rectangle(16, 64, 32, 32), ref convertedImage, new Rectangle(224, 96, 32, 32));
                                    CopyRegionIntoImage(lastImage, new Rectangle(32, 0, 16, 16), ref convertedImage, new Rectangle(224, 96, 16, 16));
                                    CopyRegionIntoImage(lastImage, new Rectangle(48, 0, 16, 16), ref convertedImage, new Rectangle(240, 96, 16, 16));

                                    CopyRegionIntoImage(lastImage, new Rectangle(0, 48, 16, 32), ref convertedImage, new Rectangle(0, 128, 16, 32));

                                    CopyRegionIntoImage(lastImage, new Rectangle(48, 48, 16, 32), ref convertedImage, new Rectangle(16, 128, 16, 32));

                                    CopyRegionIntoImage(lastImage, new Rectangle(16, 32, 32, 16), ref convertedImage, new Rectangle(32, 128, 32, 16));

                                    CopyRegionIntoImage(lastImage, new Rectangle(16, 80, 32, 16), ref convertedImage, new Rectangle(32, 144, 32, 16));

                                    CopyRegionIntoImage(lastImage, new Rectangle(0, 32, 32, 32), ref convertedImage, new Rectangle(64, 128, 32, 32));

                                    CopyRegionIntoImage(lastImage, new Rectangle(0, 32, 32, 32), ref convertedImage, new Rectangle(96, 128, 32, 32));
                                    CopyRegionIntoImage(lastImage, new Rectangle(48, 16, 16, 16), ref convertedImage, new Rectangle(96 + 16, 128 + 16, 16, 16));

                                    CopyRegionIntoImage(lastImage, new Rectangle(32, 32, 32, 32), ref convertedImage, new Rectangle(128, 128, 32, 32));

                                    CopyRegionIntoImage(lastImage, new Rectangle(32, 32, 32, 32), ref convertedImage, new Rectangle(160, 128, 32, 32));
                                    CopyRegionIntoImage(lastImage, new Rectangle(32, 16, 16, 16), ref convertedImage, new Rectangle(160, 128 + 16, 16, 16));

                                    CopyRegionIntoImage(lastImage, new Rectangle(32, 64, 32, 32), ref convertedImage, new Rectangle(192, 128, 32, 32));

                                    CopyRegionIntoImage(lastImage, new Rectangle(32, 64, 32, 32), ref convertedImage, new Rectangle(224, 128, 32, 32));
                                    CopyRegionIntoImage(lastImage, new Rectangle(32, 0, 16, 16), ref convertedImage, new Rectangle(224, 128, 16, 16));

                                    CopyRegionIntoImage(lastImage, new Rectangle(0, 64, 32, 32), ref convertedImage, new Rectangle(0, 160, 32, 32));

                                    CopyRegionIntoImage(lastImage, new Rectangle(0, 64, 32, 32), ref convertedImage, new Rectangle(32, 160, 32, 32));
                                    CopyRegionIntoImage(lastImage, new Rectangle(48, 0, 16, 16), ref convertedImage, new Rectangle(48, 160, 16, 16));

                                    CopyRegionIntoImage(lastImage, new Rectangle(0, 32, 16, 32), ref convertedImage, new Rectangle(64, 160, 16, 32));
                                    CopyRegionIntoImage(lastImage, new Rectangle(48, 32, 16, 32), ref convertedImage, new Rectangle(80, 160, 16, 32));

                                    CopyRegionIntoImage(lastImage, new Rectangle(0, 32, 32, 16), ref convertedImage, new Rectangle(96, 160, 32, 16));
                                    CopyRegionIntoImage(lastImage, new Rectangle(0, 80, 32, 16), ref convertedImage, new Rectangle(96, 176, 32, 16));

                                    CopyRegionIntoImage(lastImage, new Rectangle(0, 64, 16, 32), ref convertedImage, new Rectangle(128, 160, 16, 32));
                                    CopyRegionIntoImage(lastImage, new Rectangle(48, 64, 16, 32), ref convertedImage, new Rectangle(144, 160, 16, 32));

                                    CopyRegionIntoImage(lastImage, new Rectangle(32, 32, 32, 16), ref convertedImage, new Rectangle(160, 160, 32, 16));
                                    CopyRegionIntoImage(lastImage, new Rectangle(32, 80, 32, 16), ref convertedImage, new Rectangle(160, 176, 32, 16));

                                    CopyRegionIntoImage(lastImage, new Rectangle(0, 0, 32, 32), ref convertedImage, new Rectangle(192, 160, 32, 32));

                                    CopyRegionIntoImage(lastImage, new Rectangle(16, 48, 32, 32), ref convertedImage, new Rectangle(224, 160, 32, 32));
                                    #endregion


                                    lastImage = convertedImage;

                                    Bitmap bpp = new Bitmap(256, 192);

                                    // r1
                                    CopyRegionIntoImage(lastImage, new Rectangle(0, 0, 32, 32), ref bpp, new Rectangle(0, 0, 32, 32));
                                    CopyRegionIntoImage(lastImage, new Rectangle(128, 160, 32, 32), ref bpp, new Rectangle(32, 0, 32, 32));
                                    CopyRegionIntoImage(lastImage, new Rectangle(160, 160, 32, 32), ref bpp, new Rectangle(64, 0, 32, 32));
                                    CopyRegionIntoImage(lastImage, new Rectangle(224, 128, 32, 32), ref bpp, new Rectangle(96, 0, 32, 32));
                                    CopyRegionIntoImage(lastImage, new Rectangle(192, 128, 32, 32), ref bpp, new Rectangle(128, 0, 32, 32));
                                    CopyRegionIntoImage(lastImage, new Rectangle(96, 160, 32, 32), ref bpp, new Rectangle(160, 0, 32, 32));
                                    CopyRegionIntoImage(lastImage, new Rectangle(32, 160, 32, 32), ref bpp, new Rectangle(192, 0, 32, 32));
                                    CopyRegionIntoImage(lastImage, new Rectangle(0, 160, 32, 32), ref bpp, new Rectangle(224, 0, 32, 32));

                                    // r2
                                    CopyRegionIntoImage(lastImage, new Rectangle(32, 128, 32, 32), ref bpp, new Rectangle(0, 32, 32, 32));
                                    CopyRegionIntoImage(lastImage, new Rectangle(224, 96, 32, 32), ref bpp, new Rectangle(32, 32, 32, 32));
                                    CopyRegionIntoImage(lastImage, new Rectangle(192, 96, 32, 32), ref bpp, new Rectangle(64, 32, 32, 32));
                                    CopyRegionIntoImage(lastImage, new Rectangle(160, 96, 32, 32), ref bpp, new Rectangle(96, 32, 32, 32));
                                    CopyRegionIntoImage(lastImage, new Rectangle(128, 96, 32, 32), ref bpp, new Rectangle(128, 32, 32, 32));
                                    CopyRegionIntoImage(lastImage, new Rectangle(64, 160, 32, 32), ref bpp, new Rectangle(160, 32, 32, 32));
                                    CopyRegionIntoImage(lastImage, new Rectangle(0, 128, 32, 32), ref bpp, new Rectangle(192, 32, 32, 32));
                                    CopyRegionIntoImage(lastImage, new Rectangle(160, 128, 32, 32), ref bpp, new Rectangle(224, 32, 32, 32));

                                    // r3
                                    CopyRegionIntoImage(lastImage, new Rectangle(96, 96, 32, 32), ref bpp, new Rectangle(0, 64, 32, 32));
                                    CopyRegionIntoImage(lastImage, new Rectangle(32, 96, 32, 32), ref bpp, new Rectangle(32, 64, 32, 32));
                                    CopyRegionIntoImage(lastImage, new Rectangle(96, 128, 32, 32), ref bpp, new Rectangle(64, 64, 32, 32));
                                    CopyRegionIntoImage(lastImage, new Rectangle(96, 64, 32, 32), ref bpp, new Rectangle(96, 64, 32, 32));
                                    CopyRegionIntoImage(lastImage, new Rectangle(64, 64, 32, 32), ref bpp, new Rectangle(128, 64, 32, 32));
                                    CopyRegionIntoImage(lastImage, new Rectangle(192, 64, 32, 32), ref bpp, new Rectangle(160, 64, 32, 32));
                                    CopyRegionIntoImage(lastImage, new Rectangle(224, 32, 32, 32), ref bpp, new Rectangle(192, 64, 32, 32));
                                    CopyRegionIntoImage(lastImage, new Rectangle(64, 32, 32, 32), ref bpp, new Rectangle(224, 64, 32, 32));

                                    // r4
                                    CopyRegionIntoImage(lastImage, new Rectangle(160, 32, 32, 32), ref bpp, new Rectangle(0, 96, 32, 32));
                                    CopyRegionIntoImage(lastImage, new Rectangle(128, 32, 32, 32), ref bpp, new Rectangle(32, 96, 32, 32));
                                    CopyRegionIntoImage(lastImage, new Rectangle(128, 128, 32, 32), ref bpp, new Rectangle(64, 96, 32, 32));
                                    CopyRegionIntoImage(lastImage, new Rectangle(64, 96, 32, 32), ref bpp, new Rectangle(96, 96, 32, 32));
                                    CopyRegionIntoImage(lastImage, new Rectangle(0, 96, 32, 32), ref bpp, new Rectangle(128, 96, 32, 32));
                                    CopyRegionIntoImage(lastImage, new Rectangle(160, 64, 32, 32), ref bpp, new Rectangle(160, 96, 32, 32));
                                    CopyRegionIntoImage(lastImage, new Rectangle(160, 0, 32, 32), ref bpp, new Rectangle(192, 96, 32, 32));
                                    CopyRegionIntoImage(lastImage, new Rectangle(192, 0, 32, 32), ref bpp, new Rectangle(224, 96, 32, 32));

                                    // r5
                                    CopyRegionIntoImage(lastImage, new Rectangle(160, 0, 32, 32), ref bpp, new Rectangle(0, 128, 32, 32));
                                    CopyRegionIntoImage(lastImage, new Rectangle(128, 0, 32, 32), ref bpp, new Rectangle(32, 128, 32, 32));
                                    CopyRegionIntoImage(lastImage, new Rectangle(64, 128, 32, 32), ref bpp, new Rectangle(64, 128, 32, 32));
                                    CopyRegionIntoImage(lastImage, new Rectangle(32, 64, 32, 32), ref bpp, new Rectangle(96, 128, 32, 32));
                                    CopyRegionIntoImage(lastImage, new Rectangle(0, 64, 32, 32), ref bpp, new Rectangle(128, 128, 32, 32));
                                    CopyRegionIntoImage(lastImage, new Rectangle(192, 64, 32, 32), ref bpp, new Rectangle(160, 128, 32, 32));
                                    CopyRegionIntoImage(lastImage, new Rectangle(96, 32, 32, 32), ref bpp, new Rectangle(192, 128, 32, 32));
                                    CopyRegionIntoImage(lastImage, new Rectangle(64, 32, 32, 32), ref bpp, new Rectangle(224, 128, 32, 32));

                                    // r6
                                    CopyRegionIntoImage(lastImage, new Rectangle(32, 32, 32, 32), ref bpp, new Rectangle(0, 160, 32, 32));
                                    CopyRegionIntoImage(lastImage, new Rectangle(0, 32, 32, 32), ref bpp, new Rectangle(32, 160, 32, 32));
                                    CopyRegionIntoImage(lastImage, new Rectangle(128, 64, 32, 32), ref bpp, new Rectangle(64, 160, 32, 32));
                                    CopyRegionIntoImage(lastImage, new Rectangle(96, 0, 32, 32), ref bpp, new Rectangle(96, 160, 32, 32));
                                    CopyRegionIntoImage(lastImage, new Rectangle(64, 0, 32, 32), ref bpp, new Rectangle(128, 160, 32, 32));
                                    CopyRegionIntoImage(lastImage, new Rectangle(32, 0, 32, 32), ref bpp, new Rectangle(160, 160, 32, 32));
                                    CopyRegionIntoImage(lastImage, new Rectangle(0, 0, 32, 32), ref bpp, new Rectangle(192, 160, 32, 32));
                                    CopyRegionIntoImage(lastImage, new Rectangle(192, 160, 32, 32), ref bpp, new Rectangle(224, 160, 32, 32));


                                    bpp.Save(Path.GetFullPath("../../../SimplexRpgEngine3/Content/Sprites/Tilesets/tileset0.png"));
                                    //lastImage.Save(Path.GetFullPath("../../../SimplexRpgEngine3/Content/Sprites/Tilesets/tileset0.png"));
                                    using (StreamWriter w = File.AppendText("../../../SimplexRpgEngine3/Content/Content.mgcb"))
                                    {

                                        w.WriteLine("#begin Sprites/Tilesets/" + "tileset0.png");
                                        w.WriteLine("/importer:TextureImporter");
                                        w.WriteLine("/processor:TextureProcessor");
                                        w.WriteLine("/processorParam:ColorKeyColor=255,0,255,255");
                                        w.WriteLine("/processorParam:ColorKeyEnabled=True");
                                        w.WriteLine("/processorParam:GenerateMipmaps=False");
                                        w.WriteLine("/processorParam:PremultiplyAlpha=True");
                                        w.WriteLine("/processorParam:ResizeToPowerOfTwo=False");
                                        w.WriteLine("/processorParam:MakeSquare=False");
                                        w.WriteLine("/processorParam:TextureFormat=Color");
                                        w.WriteLine("/build:Sprites/Tilesets/" + "tileset0.png");
                                        w.WriteLine("");
                                    }
                                }
                            }

                            cx += (int)(cellW * zoom);
                            xIndex++;
                        }

                        yIndex++;
                        xIndex = 0;
                        cy += (int)Math.Round(cellH * zoom);
                        cx = darkSectionPanel1.Location.X + darkSectionPanel1.Width + 20;
                    }
                }
            }
        */
            }

        void Form_MouseWheel(object sender, MouseEventArgs e)
        {
            _totalDelta = _totalDelta + e.Delta;

            if (e.Delta > 0)
            {
                zoom += 0.11;;
            }
            else if (e.Delta < 0)
            {
                zoom -= 0.11;
            }
        }

        private void Sprites_manager_MouseMove(object sender, MouseEventArgs e)
        {
            mouse = new Vector2(e.X, e.Y);
        }

        private void darkButton2_Click(object sender, EventArgs e)
        {
            // save all subsprites
            int index = 0;
            foreach (Subsprite s in subsprites)
            {
                SavePartialBitmap(lastImage, "../../../SimplexRpgEngine3/Content/Sprites/" + selectedNode.Text + index + ".png", s.cellX * cellW, s.cellY * cellH, (s.cellW + 1) * cellW, (s.cellH + 1) * cellH, System.Drawing.Imaging.ImageFormat.Png);
                index++;
            }

            // all sprites are saved where they need to be
            // now we import them to mgcb
            using (StreamWriter w = File.AppendText("../../../SimplexRpgEngine3/Content/Content.mgcb"))
            {
                index = 0;
                foreach (Subsprite s in subsprites)
                {
                    w.WriteLine("#begin Sprites/" + selectedNode.Text + index + ".png");
                    w.WriteLine("/importer:TextureImporter");
                    w.WriteLine("/processor:TextureProcessor");
                    w.WriteLine("/processorParam:ColorKeyColor=255,0,255,255");
                    w.WriteLine("/processorParam:ColorKeyEnabled=True");
                    w.WriteLine("/processorParam:GenerateMipmaps=False");
                    w.WriteLine("/processorParam:PremultiplyAlpha=True");
                    w.WriteLine("/processorParam:ResizeToPowerOfTwo=False");
                    w.WriteLine("/processorParam:MakeSquare=False");
                    w.WriteLine("/processorParam:TextureFormat=Color");
                    w.WriteLine("/build:Sprites/" + selectedNode.Text + index + ".png");
                    w.WriteLine("");
                    index++;
                }

            }

            // then we build content
       /*    string filename = "../../../SimplexRpgEngine3/Content/SimplexBuild/Tools/MGCB.exe";
           string filenameOutput = "../../../SimplexRpgEngine3/Content/";
            string absolute = Path.GetFullPath(filename);
            string absolute2 = Path.GetFullPath(filenameOutput);
            var proc = System.Diagnostics.Process.Start(absolute, "/outputDir:" + absolute2 + " /rebuild");*/
         //   proc.Close();

            // all good
            MessageBox.Show("Saved " + index + " subsprites");
        }

        public static void SavePartialBitmap(Bitmap b, string filename, int x, int y, int w, int h, System.Drawing.Imaging.ImageFormat format)
        {
            using (Bitmap C = new Bitmap(w, h, PixelFormat.Format32bppArgb))
            {
                Graphics G = Graphics.FromImage(C);
                Rectangle src = new Rectangle(x, y, w, h);
                Rectangle dst = new Rectangle(0, 0, w, h);
                G.DrawImage(b, dst, src, GraphicsUnit.Pixel);
                G.Dispose();
                C.Save(filename, format);
                C.Dispose();
            }
        }

        private void importSpriteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // import sprite
            openFileDialog1.Multiselect = true;
            Bitmap finalBitmap = null;
            string path = "";
            string file = "";
            string noext = "";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // Create a new bitmap from selected images if selected > 1
                if (openFileDialog1.FileNames.Length > 1)
                {
                    int w = 0;
                    int h = 0;

                    Bitmap[] bmps = new Bitmap[openFileDialog1.FileNames.Length];
                    int i = 0;

                    // Determine final bitmap size
                    foreach (string s in openFileDialog1.FileNames)
                    {
                        Bitmap b = (Bitmap)Image.FromFile(s);
                        w += b.Width;
                        h = max(b.Height, h);

                        bmps[i] = b;
                        i++;
                    }

                    // Draw each image to the final image
                    finalBitmap = new Bitmap(w, h);

                    Graphics g = Graphics.FromImage(finalBitmap);
                    w = 0;
                    foreach (Bitmap b in bmps)
                    {
                        g.DrawImage(b, new System.Drawing.Point(w, 0));
                        w += b.Width;
                    }

                    // Save final bitmap
                    finalBitmap.Save("tempImage");

                    path = "tempImage";
                    file = Path.GetFileName(path);
                    noext = Path.GetFileNameWithoutExtension(path);
                }
                else
                {
                    path = openFileDialog1.FileNames[0];
                    file = Path.GetFileName(path);
                    noext = Path.GetFileNameWithoutExtension(path);
                }


                Bitmap bmp = (Bitmap) Image.FromFile(path);
                Texture2D tx = GetTexture(Sgml.GraphicsDevice, bmp);

                srpf.spritesEditorRenderer1.selectedImage = tx;

                // Create a new entry in unsaved images
                MgcbEntry me = new MgcbEntry();
                me.BeginLine = "#begin Sprites/" + file;
                me.Build = "Sprites/" + file;
                me.ImporterType = MgcbEntry.Importer.Texture;
                me.Processor = MgcbEntry.Importer.Texture;
                me.ProcessorParams.Add("/processorParam:ColorKeyColor=255,0,255,255");
                me.ProcessorParams.Add("/processorParam:ColorKeyEnabled=True");
                me.ProcessorParams.Add("/processorParam:PremultiplyAlpha=True");
                me.ProcessorParams.Add("/processorParam:GenerateMipmaps=False");
                me.ProcessorParams.Add("/processorParam:ResizeToPowerOfTwo=False");
                me.ProcessorParams.Add("/processorParam:MakeSquare=False");
                me.ProcessorParams.Add("/processorParam:TextureFormat=Color");
                me.BitmapPhysical = bmp;
                me.Name = file;
                me.NameNoExt = noext;

                MgcbEntries.Add(me);

                // And a node for this image
                DarkTreeNode dtn = new DarkTreeNode(noext);
              //  darkTreeView1.Nodes[0].Nodes.Add(dtn);
                
            }
        }

        private Texture2D GetTexture(GraphicsDevice dev, System.Drawing.Bitmap bmp)
        {
            int[] imgData = new int[bmp.Width * bmp.Height];
            Texture2D texture = new Texture2D(dev, bmp.Width, bmp.Height);

            unsafe
            {
                // lock bitmap
                System.Drawing.Imaging.BitmapData origdata =
                    bmp.LockBits(new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, bmp.PixelFormat);

                uint* byteData = (uint*)origdata.Scan0;

                // Switch bgra -> rgba
                for (int i = 0; i < imgData.Length; i++)
                {
                    byteData[i] = (byteData[i] & 0x000000ff) << 16 | (byteData[i] & 0x0000FF00) | (byteData[i] & 0x00FF0000) >> 16 | (byteData[i] & 0xFF000000);
                }

                // copy data
                System.Runtime.InteropServices.Marshal.Copy(origdata.Scan0, imgData, 0, bmp.Width * bmp.Height);

                byteData = null;

                // unlock bitmap
                bmp.UnlockBits(origdata);
            }

            texture.SetData(imgData);

            return texture;
        }

        private void importTilesetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // import tileset
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string path = openFileDialog1.FileName;
                Bitmap bitmap = new Bitmap(path);

                lastImage = bitmap;
                toolMode = 1;
            }
        }

        public static void CopyRegionIntoImage(Bitmap srcBitmap, Rectangle srcRegion, ref Bitmap destBitmap, Rectangle destRegion)
        {
            using (Graphics grD = Graphics.FromImage(destBitmap))
            {
                grD.DrawImage(srcBitmap, destRegion, srcRegion, GraphicsUnit.Pixel);
            }
        }

        private void darkSectionPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void fileToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            // convert entire file to a tilemap
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string path = openFileDialog1.FileName;
                Bitmap bitmap = new Bitmap(path);

                Bitmap[] readyToConvert = SimplexCore.IDE.SpritesManager.BitmapToSixFortySeven(bitmap);
                Bitmap finalBitmap = SimplexCore.IDE.SpritesManager.PreTilesetsToOneBitmap(readyToConvert, bitmap.Width * 4, bitmap.Height * 2); // for 64x96 base

                finalBitmap.Save("myfile.png", System.Drawing.Imaging.ImageFormat.Png);
            }
        }

        private void darkButton5_Click(object sender, EventArgs e)
        {
            if (lastSheetX != -1 && lastSheetY != -1)
            {
                string ss = get_string("", "Name");

                AutotileDefinition ad = new AutotileDefinition();
                ad.Name = ss;
                ad.X = lastSheetX;
                ad.Y = lastSheetY;
                ad.Bits = 16;

                lastSheetX = -1;
                lastSheetY = -1;

                s.AutotileLib.Add(ad);
                owner.sr.drawTest1.tilesets.FirstOrDefault(x => x.Name == selectedNode.Text).AutotileLib = s.AutotileLib;

                // Save to file
                string json = JsonConvert.SerializeObject(owner.sr.drawTest1.tilesets);
                using (StreamWriter writer = new StreamWriter("../../../SimplexRpgEngine3/TilesetsDescriptor.json"))
                {
                    writer.WriteLine(json);
                    writer.Close();
                }
            }
        }

        private void spritesEditorRenderer1_Click(object sender, EventArgs e)
        {

        }

        private void Sprites_manager_Resize(object sender, EventArgs e)
        {
            spritesEditorRenderer1.Rsize();
        }

        private void darkToolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void spritesEditorRenderer1_OnMouseWheelDownwards(MouseEventArgs e)
        {
            spritesEditorRenderer1.WheelDown();
        }

        private void spritesEditorRenderer1_MouseUp(object sender, MouseEventArgs e)
        {
            spritesEditorRenderer1.ClickUp();
        }

        private void spritesEditorRenderer1_OnMouseWheelUpwards(MouseEventArgs e)
        {
            spritesEditorRenderer1.WheelUp();
        }

        private void spritesEditorRenderer1_MouseDown(object sender, MouseEventArgs e)
        {
            spritesEditorRenderer1.ClickLock(e.Button);
        }

        private void spritesEditorRenderer1_MouseMove(object sender, MouseEventArgs e)
        {

            if (e.Button == MouseButtons.Middle)
            {
                spritesEditorRenderer1.MoveView();
            }

            spritesEditorRenderer1.MouseDrag(e.Location);
        }

        private void darkNumericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void Sprites_manager_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Visible)
            {
                owner.sr.drawTest1.UpdateRunning = true;
                e.Cancel = true;
                Hide();
            }
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            // Toggle edit mode
            drawModeOn = !drawModeOn;
            spritesEditorRenderer1.AaToggled();
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                Color r = colorDialog1.Color;
                spritesEditorRenderer1.penColor = Microsoft.Xna.Framework.Color.FromNonPremultiplied(r.R, r.G, r.B, r.A);
            }
        }

        private void compileToolStripMenuItem_Click(object sender, EventArgs e)
        {

            using (StreamWriter w = File.AppendText(owner.currentProject.RootPath + "/Content/Content.mgcb"))
            {
                // add all unsaved entries to mgcb
                foreach (MgcbEntry me in MgcbEntries)
                {
                    w.WriteLine("");
                    w.WriteLine(me.BeginLine);
                    w.WriteLine("/importer:" + me.ImporterToString(me.ImporterType));
                    w.WriteLine("/processor:" + me.ProcessorToString(me.Processor));

                    foreach (string ln in me.ProcessorParams)
                    {
                        w.WriteLine("/processorParam:" + ln);
                    }

                    w.WriteLine("/build:" + me.Build);

                    // once entry sits in mgcb we need to copy over physical file
                    me.BitmapPhysical.Save(owner.currentProject.RootPath + "/Content/Sprites/" + me.Name);

                    // also a spritesheet needs to be generated
                    Spritesheet s = new Spritesheet();
                    s.Name = me.NameNoExt;
                    s.CellHeight = cellH;
                    s.CellWidth = cellW;
                    s.Rows = rows;

                    owner.sr.drawTest1.Sprites.Add(s);
                }
            }

            // save all spritesheets to descriptor
            File.WriteAllText(owner.currentProject.RootPath + "/SpritesDescriptor.json", JsonConvert.SerializeObject(owner.sr.drawTest1.Sprites));

            // start mgcb to finish compile process
            Process mgcb = Process.Start(owner.currentProject.RootPath + "/Content/Content.mgcb");
        }

        private void DarkMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void ToolStripButton1_Click(object sender, EventArgs e)
        {
            // SAVE CHANGES TO THE IMAGE
            spritesEditorRenderer1.SaveChanges();
        }

        private void DarkDockPanel2_Load(object sender, EventArgs e)
        {
         //   darkDockPanel2.AddContent(srpf);
           
        }

        private void SpritesEditorRenderer1_Click_1(object sender, EventArgs e)
        {

        }

        private void SpritesEditorRenderer1_MouseUp_1(object sender, MouseEventArgs e)
        {
            spritesEditorRenderer1.ClickUp();
        }

        private void SpritesEditorRenderer1_OnMouseWheelUpwards_1(MouseEventArgs e)
        {
            spritesEditorRenderer1.WheelUp();
        }

        private void SpritesEditorRenderer1_OnMouseWheelDownwards_1(MouseEventArgs e)
        {
            spritesEditorRenderer1.WheelDown();
        }

        private void SpritesEditorRenderer1_MouseMove_1(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
            {
                spritesEditorRenderer1.MoveView();
            }

            spritesEditorRenderer1.MouseDrag(e.Location);
        }

        private void SpritesEditorRenderer1_MouseDown_1(object sender, MouseEventArgs e)
        {
            spritesEditorRenderer1.ClickLock(e.Button);
        }

        private void DarkGroupBox2_Enter(object sender, EventArgs e)
        {

        }



        private void DarkColorPallete2_Click(object sender, EventArgs e)
        {
        }

        private void DarkColorPallete2_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void DarkColorPallete1_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void DarkButton22_Click(object sender, EventArgs e)
        {

        }

        private void DarkColorPallete1_Click(object sender, EventArgs e)
        {

        }

        private void DarkColorPallete1_MouseDown(object sender, MouseEventArgs e)
        {
            int index = (e.Y / 16) * 6 + e.X / 16;
            Color c = darkColorPallete1.colors[index];

         

            if (e.Button == MouseButtons.Left)
            {                             
                Microsoft.Xna.Framework.Color cc = new Microsoft.Xna.Framework.Color(c.R, c.G, c.B, c.A);
                spritesEditorRenderer1.penColorLast = cc;

                cc *= (float)(darkNumericUpDown4.Value / 255);
                spritesEditorRenderer1.penColor = cc;

                c = Color.FromArgb((int)darkNumericUpDown4.Value, c);
                darkMouseTool1.LeftColor = c;
            }
            else if (e.Button == MouseButtons.Right)
            {
                Microsoft.Xna.Framework.Color cc = new Microsoft.Xna.Framework.Color(c.R, c.G, c.B, c.A);
                spritesEditorRenderer1.penColorRightLast = cc;

                cc *= (float)(darkNumericUpDown4.Value / 255);
                spritesEditorRenderer1.penColorRight = cc;


                c = Color.FromArgb((int)darkNumericUpDown4.Value, c);
                darkMouseTool1.RightColor = c;
            }



            darkMouseTool1.Invalidate();
        }

        private void DarkColorPallete2_MouseDown(object sender, MouseEventArgs e)
        {
            Bitmap b = new Bitmap(darkColorPallete2.Width, darkColorPallete2.Height);
            darkColorPallete2.DrawToBitmap(b, new Rectangle(0, 0, b.Width, b.Height));

            Color c = b.GetPixel(e.X, e.Y);
            c = Color.FromArgb((int)darkNumericUpDown4.Value, c);

            if (e.Button == MouseButtons.Left)
            {
                Microsoft.Xna.Framework.Color cc = new Microsoft.Xna.Framework.Color(c.R, c.G, c.B, c.A);
                spritesEditorRenderer1.penColorLast = cc;
                cc *= (float)(darkNumericUpDown4.Value / 255);

                darkMouseTool1.LeftColor = c;
                spritesEditorRenderer1.penColor = cc;
            }
            else if (e.Button == MouseButtons.Right)
            {
                Microsoft.Xna.Framework.Color cc = new Microsoft.Xna.Framework.Color(c.R, c.G, c.B, c.A);
                spritesEditorRenderer1.penColorRightLast = cc;
                cc *= (float)(darkNumericUpDown4.Value / 255);

                darkMouseTool1.RightColor = c;
                spritesEditorRenderer1.penColorRight = cc;
            }

            darkMouseTool1.Invalidate();
        }

        private void DarkProgressBar1_Click(object sender, EventArgs e)
        {
           
        }

        private void DarkImageIndex1_Click(object sender, EventArgs e)
        {
            
        }

        private int holdingFrame = -1;

        private void DarkImageIndex1_MouseDown(object sender, MouseEventArgs e)
        {
            // behavior of the control itself
            int actualX = e.X + darkImageIndex1.CameraX;
            int actualY = e.Y;

            if (actualX > 0 && actualX < 80)
            {
                int k = darkImageIndex1.SelectedFrame;
                darkImageIndex1.Frames.Add(new ImageIndex() {});
                spritesEditorRenderer1.AddEmptyFrame();

                darkGroupBox4.Text = "Animation (" + k + "/" + spritesEditorRenderer1.Frames.Count + ")";
                //spritesEditorRenderer1.ScaleToFit(90);
            }
            else
            {
                darkImageIndex1.SelectedFrame = ((actualX - 96) / 96);
                spritesEditorRenderer1.selectedImageIndex = ((actualX - 96) / 96);
                
                spritesEditorRenderer1.SelectFrame(((actualX - 96) / 96));
                //spritesEditorRenderer1.ScaleToFit(90);
            }
        }

        private void DarkImageIndex1_Paint(object sender, PaintEventArgs e)
        {
            int actualX = mouseX + darkImageIndex1.CameraX;

            if (actualX > 80)
            {
                int k = ((actualX - 96) / 96) * 96;

                e.Graphics.DrawImage(Properties.Resources.red, new System.Drawing.Point(actualX, 5));
            }
        }

        private void DarkImageIndex1_MouseMove(object sender, MouseEventArgs e)
        {
            darkImageIndex1.Invalidate();
            mouseX = e.X;
        }

        private void DarkButton33_Click(object sender, EventArgs e)
        {
 
        }

        private void ToolStripButton2_Click(object sender, EventArgs e)
        {
            // clear current image
            spritesEditorRenderer1.selectedFrame.layers[0].texture = surface_create(spritesEditorRenderer1.selectedFrame.layers[0].texture.Width, spritesEditorRenderer1.selectedFrame.layers[0].texture.Height);
            spritesEditorRenderer1.UpdatePreview(spritesEditorRenderer1.selectedImageIndex);
        }

        private void DarkButton33_Click_1(object sender, EventArgs e)
        {
            darkButton33.Pushed = !darkButton33.Pushed;
        }

        private void DarkNumericUpDown4_ValueChanged(object sender, EventArgs e)
        {
            // change alpha
            darkProgressBar1.Value = (int)(((float)darkNumericUpDown4.Value / 255f) * 100);

            spritesEditorRenderer1.UpdateColors((int)darkNumericUpDown4.Value);
        }

        private void DarkProgressBar1_MouseClick(object sender, MouseEventArgs e)
        {
            // change alpha from progressbar
            int x = e.X;
            float value = x / (float)darkProgressBar1.Width;

            darkNumericUpDown4.Value = (int)((value) * 255);
            darkProgressBar1.Value = (int)(((float)darkNumericUpDown4.Value / 255f) * 100);

            spritesEditorRenderer1.UpdateColors((int)darkNumericUpDown4.Value);

            Color c = Color.FromArgb((int)darkNumericUpDown4.Value, darkMouseTool1.LeftColor);
            darkMouseTool1.LeftColor = c;
            c = Color.FromArgb((int)darkNumericUpDown4.Value, darkMouseTool1.RightColor);
            darkMouseTool1.RightColor = c;

            darkMouseTool1.Invalidate();
        }

        private void darkButton7_Click(object sender, EventArgs e)
        {
            spritesEditorRenderer1.activeTool = SpritesEditorRenderer.Tools.Pixel;
            darkButton7.Pushed = !darkButton7.Pushed;
        }

        private void darkButton26_Click(object sender, EventArgs e)
        {
            spritesEditorRenderer1.activeTool = SpritesEditorRenderer.Tools.SprayPaint;
        }

        private void darkButton10_Click(object sender, EventArgs e)
        {
            spritesEditorRenderer1.activeTool = SpritesEditorRenderer.Tools.Dropper;
        }

        private void darkButton11_Click(object sender, EventArgs e)
        {
            spritesEditorRenderer1.activeTool = SpritesEditorRenderer.Tools.Rubber;
        }

        private void darkButton17_Click(object sender, EventArgs e)
        {
            spritesEditorRenderer1.activeTool = SpritesEditorRenderer.Tools.Line;
        }

        private void darkButton9_Click(object sender, EventArgs e)
        {
            spritesEditorRenderer1.activeTool = SpritesEditorRenderer.Tools.Rectangle;
        }

        private void darkButton8_Click(object sender, EventArgs e)
        {
            spritesEditorRenderer1.activeTool = SpritesEditorRenderer.Tools.Ellipse;
        }

        private void darkButton13_Click(object sender, EventArgs e)
        {
            spritesEditorRenderer1.activeTool = SpritesEditorRenderer.Tools.RoundedRectangle;
        }

        private void darkButton16_Click(object sender, EventArgs e)
        {
            spritesEditorRenderer1.activeTool = SpritesEditorRenderer.Tools.Polygon;
        }

        private void darkButton32_Click(object sender, EventArgs e)
        {
            spritesEditorRenderer1.activeTool = SpritesEditorRenderer.Tools.Star;
        }

        private void darkButton12_Click(object sender, EventArgs e)
        {
            spritesEditorRenderer1.activeTool = SpritesEditorRenderer.Tools.Text;
        }

        private void darkButton14_Click(object sender, EventArgs e)
        {
            spritesEditorRenderer1.activeTool = SpritesEditorRenderer.Tools.Fill;
        }

        private void darkButton19_Click(object sender, EventArgs e)
        {
            spritesEditorRenderer1.activeTool = SpritesEditorRenderer.Tools.Spray;
        }

        private void DarkImageIndex1_KeyDown(object sender, KeyEventArgs e)
        {
            // handle ctrl c/v
            if (e.KeyCode == Keys.C && e.Modifiers == Keys.Control)
            {
                int frame = darkImageIndex1.SelectedFrame;

            }

            if (e.KeyCode == Keys.V && e.Modifiers == Keys.Control)
            {
                darkImageIndex1.Frames.Add(new ImageIndex() { });
                darkImageIndex1.Invalidate();
            }
        }

        private void SpritesEditorRenderer1_Resize(object sender, EventArgs e)
        {
            spritesEditorRenderer1.ScaleToFit(90);
        }

        private void DarkNumericUpDown1_ValueChanged_1(object sender, EventArgs e)
        {

        }

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void BlurToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void DarkFloatingToolbox1_QueryContinueDrag(object sender, QueryContinueDragEventArgs e)
        {
          
        }

        private void DarkFloatingToolbox1_LocationChanged(object sender, EventArgs e)
        {

        }

        private void darkButton25_Click(object sender, EventArgs e)
        {
            spritesEditorRenderer1.size = 1;
        }

        private void darkButton28_Click(object sender, EventArgs e)
        {
            spritesEditorRenderer1.size = 2;
        }

        private void darkButton27_Click(object sender, EventArgs e)
        {
            spritesEditorRenderer1.size = 3;
        }

        private void darkButton29_Click(object sender, EventArgs e)
        {
            spritesEditorRenderer1.size = 4;
        }

        private void darkButton30_Click(object sender, EventArgs e)
        {
            spritesEditorRenderer1.size = 5;
        }

        private void darkButton31_Click(object sender, EventArgs e)
        {
            spritesEditorRenderer1.size = 6;
        }
    }

    public class Subsprite
    {
        public int cellX;
        public int cellY;
        public int cellW;
        public int cellH;
        public string name;
        public SolidBrush sb;

        public Subsprite()
        {
            cellW = 0;
            cellH = 0;
            Random r = new Random();
            sb = new SolidBrush(Color.FromArgb(64, r.Next(255), r.Next(255), r.Next(255)));
        }
    }
}
