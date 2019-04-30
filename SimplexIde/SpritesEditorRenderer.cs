﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Forms.Controls;
using SimplexCore;
using ButtonState = Microsoft.Xna.Framework.Input.ButtonState;
using Color = Microsoft.Xna.Framework.Color;
using Point = System.Drawing.Point;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using RectangleF = MonoGame.Extended.RectangleF;

namespace SimplexIde
{
    public class SpritesEditorRenderer : MonoGameControl
    {
        private SimplexCore.OrthographicCamera c;
        SimplexCamera cam = new SimplexCamera();
        Matrix world = Matrix.Identity;
        private Color BackgroundColor;
        public Vector2 MousePosition;
        public static DynamicVertexBuffer vertexBuffer;
        public static BasicEffect basicEffect;
        private Matrix m;
        private bool mouseLocked = false;
        private bool panView = false;
        Vector2 MousePrevious = Vector2.One;
        Vector2 helpVec = Vector2.One;
        Vector2 MousePositionTranslated = Vector2.One;
        public DrawTest mainForm = null;
        private GameObject representativeGameObject = null;
        public Texture2D selectedImage = null;
        public Sprites_manager parentForm = null;
        int selectedXIndex = -1;
        private int selectedYIndex = -1;
        RenderTarget2D gridSurface = null;
        public int selectedImageIndex = 0;
        private RenderTarget2D imageOverlay = null;
        private MouseState ms;
        private MouseState msPrev;
        private Texture2D pixel = null;
        public Tools activeTool = Tools.None;
        public enum Tools { None, Pixel, SprayPaint, Dropper, Rubber, Line, Rectangle, Ellipse, RoundedRectangle, Polygon, Star, Text, Fill, Spray };
        public Color penColor = Color.White;
        public Color penColorRight = Color.FromNonPremultiplied(0, 0, 1, 255);
        public AnimationFrame selectedFrame = null;
        public int selectedLayer = 0;
        public List<AnimationFrame> Frames = new List<AnimationFrame>();
        private RenderTarget2D previewGrid;
        private int animateIn = 0;
        List<Vector2> occupiedPositions = new List<Vector2>();
        public Color penColorLast = Color.White;
        public Color penColorRightLast = Color.FromNonPremultiplied(0, 0, 1, 255);
        Vector2 tempVector = Vector2.One;
        Vector2 toolOrigin = Vector2.One;
        bool toolPreview = false;
        Vector2 safe = Vector2.One;
        private bool over = false;
        private Vector2 toolOriginSubpixel;
        private Vector2 mouseSubpixel;
        Vector2 toolOriginINP = Vector2.One;
        private int ttl = 0;
        public int size = 1;
        int num;
        private List<int> sizesx = new List<int>() { -2, -2, -1, -1, -1, -1, 0, 0, 0, 1, 1, -3, -3, -3, -3, -2, -2, -2, -2, -1, -1, 0, 0, 1, 1, 1, 1, 2, 2, 2, 2, -5, -5, -5, -5, -4, -4, -4, -4, -4, -4, -3, -3, -3, -3, -2, -2, -2, -2, -1, -1, -1, -1, 0, 0, 0, 0, 1, 1, 1, 1, 2, 2, 2, 2, 3, 3, 3, 3, 3, 3, 4, 4, 4, 4, -7, -7, -7, -7, -6, -6, -6, -6, -6, -6, -6, -6, -5, -5, -5, -5, -5, -5, -4, -4, -4, -4, -4, -4, -3, -3, -3, -3, -2, -2, -2, -2, -1, -1, -1, -1, 0, 0, 0, 0, 1, 1, 1, 1, 2, 2, 2, 2, 3, 3, 3, 3, 3, 3, 4, 4, 4, 4, 4, 4, 5, 5, 5, 5, 5, 5, 5, 5, 6, 6, 6, 6, -8, -8, -8, -8, -8, -8, -7, -7, -7, -7, -7, -7, -6, -6, -6, -6, -5, -5, -5, -5, -4, -4, -3, -3, -3, -3, -2, -2, -1, -1, 0, 0, 1, 1, 2, 2, 2, 2, 3, 3, 4, 4, 4, 4, 5, 5, 5, 5, 6, 6, 6, 6, 6, 6, 7, 7, 7, 7, 7, 7 };
        private List<int> sizesy = new List<int>() { -1, 0, -2, -1, 0, 1, -2, -1, 1, -1, 0, -2, -1, 0, 1, -3, -2, 1, 2, -3, 2, -3, 2, -3, -2, 1, 2, -2, -1, 0, 1, -2, -1, 0, 1, -3, -2, -1, 0, 1, 2, -4, -3, 2, 3, -5, -4, 3, 4, -5, -4, 3, 4, -5, -4, 3, 4, -5, -4, 3, 4, -4, -3, 2, 3, -3, -2, -1, 0, 1, 2, -2, -1, 0, 1, -2, -1, 0, 1, -4, -3, -2, -1, 0, 1, 2, 3, -5, -4, -3, 2, 3, 4, -6, -5, -4, 3, 4, 5, -6, -5, 4, 5, -7, -6, 5, 6, -7, -6, 5, 6, -7, -6, 5, 6, -7, -6, 5, 6, -6, -5, 4, 5, -6, -5, -4, 3, 4, 5, -5, -4, -3, 2, 3, 4, -4, -3, -2, -1, 0, 1, 2, 3, -2, -1, 0, 1, -3, -2, -1, 0, 1, 2, -5, -4, -3, 2, 3, 4, -6, -5, 4, 5, -7, -6, 5, 6, -7, 6, -8, -7, 6, 7, -8, 7, -8, 7, -8, 7, -8, 7, -8, -7, 6, 7, -7, 6, -7, -6, 5, 6, -6, -5, 4, 5, -5, -4, -3, 2, 3, 4, -3, -2, -1, 0, 1, 2 };

        public void AddEmptyFrame()
        {
            AnimationFrame af = new AnimationFrame();

            RenderTarget2D s = Sgml.surface_create(selectedImage.Width, selectedImage.Height);
            af.previewLayer = new AnimationLayer();
            af.previewLayer.texture = Sgml.surface_create(selectedImage.Width, selectedImage.Height);

            af.layers.Add(new AnimationLayer() {texture = s });
            Frames.Add(af);
        }

        public void AddFrameFromTexture(Texture2D texture)
        {
            AnimationFrame af = new AnimationFrame();
            af.texture = texture;
            af.previewLayer = new AnimationLayer();
            af.previewLayer.texture = Sgml.surface_create(texture.Width, texture.Height);

            RenderTarget2D surf = Sgml.surface_create(texture.Width, texture.Height);
            Sgml.surface_set_target(surf);
            Sgml.draw_sprite(texture, -2, Vector2.Zero);
            Sgml.surface_reset_target();
            af.layers.Add(new AnimationLayer() {texture = surf});
            selectedFrame = af;
            Frames.Add(af);
        }

        public void SelectFrame(int index)
        {
            if (Frames.Count > index)
            {
                selectedFrame = Frames[index];
                parentForm.darkGroupBox4.Text = "Animation (" + (index + 1) + "/" + Frames.Count + ")";
            }
        }




        protected override void Initialize()
        {
            base.Initialize();
            // MouseHoverUpdatesOnly = true;

            representativeGameObject = new GameObject();
            representativeGameObject.Sprite.TextureCellsPerRow = 1;

            c = new SimplexCore.OrthographicCamera(Editor.graphics);
            cam.Camera = c;
            cam.Position = Vector2.Zero;
            cam.TransformSpeed = 0.1f;

            vertexBuffer = new DynamicVertexBuffer(GraphicsDevice, typeof(VertexPositionColor), 1000, BufferUsage.WriteOnly);
            basicEffect = new BasicEffect(GraphicsDevice);
            msPrev = Mouse.GetState();

            GraphicsDevice.PresentationParameters.RenderTargetUsage = RenderTargetUsage.PreserveContents;

            m = Matrix.CreateOrthographicOffCenter(0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, 0, 0, -1);
            Sgml.GraphicsDevice = GraphicsDevice;

            pixel = new Texture2D(GraphicsDevice, 1, 1);
            pixel.SetData(new[] {Color.White});
          
            Rsize();
            UpdateGrid();

            previewGrid = Sgml.surface_create(80, 80);
        }

        public void UpdateColors(int value)
        {
            penColor = penColorLast * (value / 255f);
            penColorRight = penColorRightLast * (value / 255f);
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            cam.UpdatePosition();

            var originalPos = cam.Camera.Position;
            var originalZoom = cam.Camera.Zoom;

            Matrix view = cam.Camera.GetViewMatrix();

            cam.Camera.Position = new Vector2(0, 0);
            cam.Camera.Zoom = 1;

            Matrix normalizedMatrix = cam.Camera.GetViewMatrix();

            cam.Camera.Position = originalPos;
            cam.Camera.Zoom = originalZoom;

            Sgml.world = world;
            Sgml.view = view;
            Sgml.normalizedMatrix = normalizedMatrix;

            if (selectedFrame != null)
            {
                if (ms.LeftButton == ButtonState.Pressed ^ ms.RightButton == ButtonState.Pressed)
                {

                    if (activeTool == Tools.Pixel || activeTool == Tools.Rubber || activeTool == Tools.Fill)
                    {
                        ToolDraw(ms.LeftButton == ButtonState.Pressed ? penColor : penColorRight);
                    }
                }

                if ((ms.LeftButton == ButtonState.Released && msPrev.LeftButton == ButtonState.Pressed && toolPreview) ^ (ms.RightButton == ButtonState.Released && msPrev.RightButton == ButtonState.Pressed && toolPreview))
                {
                    if (activeTool == Tools.Ellipse || activeTool == Tools.Rectangle || activeTool == Tools.Line || activeTool == Tools.RoundedRectangle)
                    {
                        ToolDraw(msPrev.LeftButton == ButtonState.Pressed ? penColor : penColorRight);
                        over = true;
                    }

                    toolPreview = false;
                }

                Sgml.draw_set_color(ms.LeftButton == ButtonState.Pressed ? penColor : penColorRight);

                // render preview
                toolOriginSubpixel.X = toolOrigin.X;//(float)Math.Floor(toolOrigin.X >= Sgml.mouse.X ? toolOrigin.X + .5f : toolOrigin.X - .5f);
                toolOriginSubpixel.Y = toolOrigin.Y;//(float)Math.Floor(toolOrigin.Y >= Sgml.mouse.Y ? toolOrigin.Y + .5f : toolOrigin.Y - .5f);

                mouseSubpixel.X = (float)Math.Floor(Sgml.mouse.X);
                mouseSubpixel.Y = (float)Math.Floor(Sgml.mouse.Y);

                mouseSubpixel.X += mouseSubpixel.X > toolOriginSubpixel.X ? 1 : 0;
                mouseSubpixel.Y += mouseSubpixel.Y > toolOriginSubpixel.Y ? 1 : 0;


                Sgml.surface_set_target(selectedFrame.previewLayer.texture);
                Sgml.draw_clear_transparent();
                Sgml.draw_set_color(ms.LeftButton == ButtonState.Pressed ? penColor : penColorRight);


                if (activeTool == Tools.Pixel)
                {
                    Sgml.draw_rectangle(new Vector2((float)Sgml.round(Sgml.mouse.X - .5f), (float)Sgml.round(Sgml.mouse.Y - .5f)), new Vector2((float)Sgml.round(Sgml.mouse.X + .5f), (float)Sgml.round(Sgml.mouse.Y + .5f)), false);
                }

                if (toolPreview)
                {
                    if (activeTool == Tools.Ellipse)
                    {
                        Sgml.draw_ellipse(new Vector2(Sgml.mouse.X - toolOrigin.X, Sgml.mouse.Y - toolOrigin.Y), toolOrigin, 1);
                    }

                    else if (activeTool == Tools.Rectangle)
                    {
                        float my = Sgml.mouse.Y;
                        my = (float)Sgml.floor(my) + .5f;

                        Sgml.draw_rectangle(toolOrigin, new Vector2(Sgml.mouse.X, my), true);
                    }

                    else if (activeTool == Tools.Line)
                    {
                        Sgml.draw_rectangle(new Vector2((float)Sgml.round(toolOriginINP.X - .5f), (float)Sgml.round(toolOriginINP.Y - .5f)), new Vector2((float)Sgml.round(toolOriginINP.X + .5f), (float)Sgml.round(toolOriginINP.Y + .5f)), false);
                        Sgml.draw_line(mouseSubpixel, toolOriginSubpixel);

                    }
                    else if (activeTool == Tools.RoundedRectangle)
                    {
                        Sgml.draw_roundrect(toolOrigin, new Vector2(Sgml.mouse.X, Sgml.mouse.Y), true, 1);
                    }
                }

                Sgml.surface_reset_target();

                if ((ms.LeftButton == ButtonState.Pressed && !toolPreview)^(ms.RightButton == ButtonState.Pressed && !toolPreview))
                {
                    if (!over)
                    {
                        toolOriginINP = new Vector2(Sgml.mouse.X, Sgml.mouse.Y);
                        toolOrigin.X = (float)Sgml.round(Sgml.mouse.X - 0.5f) + .5f;
                        toolOrigin.Y = (float)Sgml.round(Sgml.mouse.Y - 0.5f) + .5f;

                        toolPreview = true;
                    }
                    else
                    {
                        over = false;
                    }
                }
            }


            // Update frame if user pushed autoplay btn 
            if (parentForm.darkButton33.Pushed)
            {
                animateIn--;

                if (animateIn <= 0)
                {
                    animateIn = (int)parentForm.darkNumericUpDown5.Value;

                    selectedImageIndex++;

                    if (selectedImageIndex >= Frames.Count)
                    {
                        selectedImageIndex = 0;
                    }

                    selectedFrame = Frames[selectedImageIndex];
                    parentForm.darkImageIndex1.SelectedFrame = selectedImageIndex;
                    parentForm.darkImageIndex1.Invalidate();
                }
            }

            msPrev = ms;
        }

        bool PixelFree(float x, float y)
        {
            tempVector.X = (float)Sgml.round(x - .5f);
            tempVector.Y = (float)Sgml.round(y - .5f);
            return !occupiedPositions.Contains(tempVector);
        }

        bool inTexture(int x, int y)
        {
            return selectedFrame.layers[0].texture.Height * y + x >= 0 && selectedFrame.layers[0].texture.Height * y + x < selectedFrame.layers[0].texture.Width * selectedFrame.layers[0].texture.Height;
        }

        bool inTextureP(int x, int y)
        {
            return x >= 0 && x < selectedFrame.layers[0].texture.Width && y >= 0 && y < selectedFrame.layers[0].texture.Height;
        }

        private static T[,] Make2DArray<T>(T[] input, int height, int width)
        {
            T[,] output = new T[height, width];
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    output[i, j] = input[i * width + j];
                }
            }
            return output;
        }


        bool pixelsSimilar(int color1, int color2, int threshold)
        {
            // threshold = sum of rgb diffs 
            int dr = Math.Abs(((color1 >> 16) & 0xff) - ((color2 >> 16) & 0xff));
            int dg = Math.Abs(((color1 >> 8) & 0xff) - ((color2 >> 8) & 0xff));
            int db = Math.Abs(((color1 >> 0) & 0xff) - ((color2 >> 0) & 0xff));
            return (dr + dg + db) <= threshold;
        }

        private void ToolDraw(Color penColor)
        {
            Sgml.surface_set_target(selectedFrame.layers[0].texture);
            Sgml.draw_set_aa(false);
            Sgml.draw_set_color(penColor);

            int x1 = 0;
            int y1 = 0;

            
            if (activeTool == Tools.Pixel)
            {
                if (PixelFree(Sgml.mouse.X, Sgml.mouse.Y))
                {
                    Sgml.draw_sprite(pixel, -2, tempVector);
                    occupiedPositions.Add(tempVector);
                }
                
                if (size == 1) { num = 0; }
                if (size == 2) { num = 11; }
                if (size == 3) { num = 31; }
                if (size == 4) { num = 75; }
                if (size == 5) { num = 147; }
                if (size == 6) { num = 207; }

                for (int i = 0; i < num; i++)
                {
                    if (PixelFree(Sgml.mouse.X + sizesx[i], Sgml.mouse.Y + sizesy[i]))
                    {
                        Sgml.draw_sprite(pixel, -2, tempVector);
                    }
                }

                
            }
            else if (activeTool == Tools.Ellipse)
            {
                Sgml.draw_ellipse(new Vector2(Sgml.mouse.X - toolOrigin.X, Sgml.mouse.Y - toolOrigin.Y), toolOrigin, 1);
            }
            else if (activeTool == Tools.Fill)
            {
                // flood fill
                var data = new Color[selectedFrame.layers[0].texture.Width * selectedFrame.layers[0].texture.Height];
                selectedFrame.layers[0].texture.GetData(data);

                var x = (int) Sgml.round(Sgml.mouse.X - .5f);
                var y = (int) Sgml.round(Sgml.mouse.Y - .5f);
                int w = Math.Max(selectedFrame.layers[0].texture.Height, selectedFrame.layers[0].texture.Width);

                if (inTexture(x, y) && inTextureP(x, y))
                {
                    var pixels = new Stack<Point>();
                    var used = new List<Point>();

                    var targetColor = data[w * y + x];
                    pixels.Push(new Point(x, y));
                    var i = 0;
                    var max = data.Length - 1;
                    var flag = !(data[w * y + x] == penColor);

                    if (flag)
                    {
                        while (pixels.Count > 0 && i < max)
                        {
                            var a = pixels.Pop();
                            var xx = a.X;

                            if (!inTextureP(a.X, a.Y)) {break;}

                            while (xx >= 0 && data[w * a.Y + xx] == targetColor)
                            {
                                xx--;
                            }

                            xx++;
                            var spanAbove = false;
                            var spanBelow = false;

                            while (xx < selectedFrame.layers[0].texture.Width && data[w * a.Y + xx] == targetColor)
                            {
                                data[w * a.Y + xx] = penColor;

                                if (!spanAbove && a.Y > 0 && data[w * (a.Y - 1) + xx] == targetColor)
                                {
                                    if (!used.Contains(new Point(xx, a.Y - 1)))
                                    {
                                        pixels.Push(new Point(xx, a.Y - 1));
                                        used.Add(new Point(xx, a.Y - 1));
                                        spanAbove = true;
                                    }
                                }
                                else if (spanAbove && a.Y > 0 && data[w * (a.Y - 1) + xx] != targetColor)
                                {
                                    spanAbove = false;
                                }

                                if (!spanBelow && a.Y < selectedFrame.layers[0].texture.Height - 1 && data[w * (a.Y + 1) + xx] == targetColor)
                                {
                                    if (!used.Contains(new Point(xx, a.Y + 1)))
                                    {
                                        pixels.Push(new Point(xx, a.Y + 1));
                                        used.Add(new Point(xx, a.Y + 1));
                                        spanBelow = true;
                                    }
                                }
                                else if (spanBelow && a.Y < selectedFrame.layers[0].texture.Height - 1 && data[w * (a.Y + 1) + xx] != targetColor)
                                {
                                    spanBelow = false;
                                }

                                xx++;
                            }

                            i++;
                        }

                        selectedFrame.layers[0].texture.SetData(data);
                    }
                }
            }
            else if (activeTool == Tools.Dropper)
            {

            }
            else if (activeTool == Tools.Line)
            {
                Sgml.draw_rectangle(new Vector2((float)Sgml.round(toolOriginINP.X - .5f), (float)Sgml.round(toolOriginINP.Y - .5f)), new Vector2((float)Sgml.round(toolOriginINP.X + .5f), (float)Sgml.round(toolOriginINP.Y + .5f)), false);
                Sgml.draw_line(mouseSubpixel, toolOriginSubpixel);

            }
            else if (activeTool == Tools.Polygon)
            {

            }
            else if (activeTool == Tools.Rectangle)
            {
                float my = Sgml.mouse.Y;
                my = (float)Sgml.floor(my) + .5f;

                Sgml.draw_rectangle(toolOrigin, new Vector2(Sgml.mouse.X, my), true);
            }
            else if (activeTool == Tools.RoundedRectangle)
            {
                Sgml.draw_roundrect(toolOrigin, new Vector2(Sgml.mouse.X, Sgml.mouse.Y), true, 1);
            }
            else if (activeTool == Tools.Rubber)
            {
                Color[] data = new Color[selectedFrame.layers[0].texture.Width * selectedFrame.layers[0].texture.Height];
                selectedFrame.layers[0].texture.GetData(data);

                int x = (int)Sgml.round(Sgml.mouse.X - .5f);
                int y = (int)Sgml.round(Sgml.mouse.Y - .5f);
                int w = Math.Max(selectedFrame.layers[0].texture.Height, selectedFrame.layers[0].texture.Width);

                if (inTextureP(x, y))
                {
                    if (selectedFrame.layers[0].texture.Height * y + x >= 0 && selectedFrame.layers[0].texture.Height * y + x < selectedFrame.layers[0].texture.Width * selectedFrame.layers[0].texture.Height)
                    {
                        data[w * y + x] = Color.Transparent;
                        selectedFrame.layers[0].texture.SetData(data);
                    }

                    occupiedPositions.Add(new Vector2((float) Sgml.round(Sgml.mouse.X - .5f) - x1, (float) Sgml.round(Sgml.mouse.Y - .5f) - y1));
                }
            }
            else if (activeTool == Tools.Spray)
            {

            }
            else if (activeTool == Tools.SprayPaint)
            {

            }
            else if (activeTool == Tools.Star)
            {

            }
            else if (activeTool == Tools.Text)
            {

            }

            Sgml.surface_reset_target();
            Sgml.draw_set_color(Color.White);

            UpdatePreview(selectedImageIndex);
        }

        public void UpdatePreview(int index)
        {
            // later
          /*  RenderTarget2D finalSurface = Sgml.surface_create(selectedFrame.layers[0].texture.Width, selectedFrame.layers[0].texture.Height);
            Sgml.surface_set_target(finalSurface);
            Sgml.draw_surface(Vector2.Zero, selectedFrame.layers[0].texture);
            Sgml.surface_reset_target();*/

            MemoryStream ms = Sgml.surface_save_ext_memory(selectedFrame.layers[0].texture, 80, 80);

            Bitmap b = (Bitmap)Image.FromStream(ms);

            if (parentForm.darkImageIndex1.Frames.Count > index)
            {
                parentForm.darkImageIndex1.Frames[index].bmp = b;
            }

            parentForm.darkImageIndex1.Invalidate();
        }

        public void SaveChanges()
        {
            // Save physical images
            RenderTarget2D finalSurface = null;
            int index = 0;

            foreach (AnimationFrame af in Frames)
            {
                finalSurface = Sgml.surface_create(selectedFrame.previewLayer.texture.Width, selectedFrame.previewLayer.texture.Height);
                Sgml.surface_set_target(finalSurface);
                Sgml.draw_surface(Vector2.Zero, af.layers[0].texture);
                Sgml.surface_reset_target();
                Sgml.surface_save(finalSurface, parentForm.owner.currentProject.RootPath + "/Content/Sprites/texture" + "_frame" + index);

                index++;
            }
            
            finalSurface?.Dispose();
        }

        public void ScaleToFit(int percent)
        {
            if (selectedFrame != null)
            {
                cam.TargetZoom = (Sgml.min(Width, Height) / (float) Sgml.max(selectedFrame.previewLayer.texture.Width, selectedFrame.previewLayer.texture.Height) / 100f) * percent;
                cam.TargetPosition = new Vector2(-Width / 2f + selectedFrame.layers[0].texture.Width / 2f, -Height / 2f + selectedFrame.layers[0].texture.Height / 2f);
            }
        }

        protected override void Draw()
        {
            if (parentForm != null)
            {
                ms = Mouse.GetState();

                if (ttl < 10)
                {
                    if (ttl == 9)
                    {
                        ScaleToFit(90);
                    }

                    ttl++;
                }

                base.Draw();
                double framerate = Editor.GetFrameRate;
                Matrix transformMatrix = cam.Camera.GetViewMatrix();
                MousePositionTranslated = cam.Camera.ScreenToWorld(new Vector2(ms.X, ms.Y));

                BackgroundColor = Color.Black;
                Editor.graphics.Clear(BackgroundColor);
                Input.MousePosition = MousePositionTranslated;
                MousePosition = new Vector2(ms.X, ms.Y);

                Sgml.sb = Editor.spriteBatch;
                Sgml.vb = vertexBuffer;
                Sgml.be = basicEffect;
                Sgml.m = transformMatrix;
                Sgml.currentObject = representativeGameObject;

                Matrix view = cam.Camera.GetViewMatrix();
                Matrix projection = m;

                basicEffect.World = world;
                basicEffect.View = view;
                basicEffect.Projection = projection;
                basicEffect.VertexColorEnabled = true;
                
                Sgml.mouse = MousePositionTranslated;

                // Actual logic
                int cellSize = 16;
                int x = 0;
                int y = 0;

                RectangleF rect = RectangleF.Empty;

                Color c1 = Color.FromNonPremultiplied(68, 68, 68, 255);
                Color c2 = Color.FromNonPremultiplied(77, 77, 77, 255);

                bool flag = true;
                bool lastFlag = flag;

                cam.Camera.Origin = new Vector2(Width / 2f, Height / 2f);
                

                basicEffect.View = Matrix.Identity;
                Sgml.m = Matrix.Identity;
                Vector2 origin = Vector2.Zero;

                Sgml.draw_surface(origin, gridSurface);

                basicEffect.View = view;
                Sgml.m = transformMatrix;

                if (selectedFrame != null)
                {
                    Sgml.draw_set_aa(!parentForm.drawModeOn);

                    if (selectedFrame.layers[0].texture != null)
                    {
                        Sgml.draw_surface(origin, selectedFrame.layers[selectedLayer].texture);
                        Sgml.draw_surface(origin, selectedFrame.previewLayer.texture);
                    }

                    Sgml.draw_set_aa(true);

                    // draw cells
                    int xx = 0;
                    int yy = 0;
                    int xIndex = 0;
                    int yIndex = 0;
                    RectangleF temp = RectangleF.Empty;

                    float x1 = origin.X;
                    float y1 = origin.Y;
                    float x2 = x1 +  (int)parentForm.darkNumericUpDown1.Value;
                    float y2 = y1 +  (int)parentForm.darkNumericUpDown2.Value;

                    Sgml.draw_set_alpha(0.8);
                    Sgml.draw_set_color(Color.Black);
                    for (var i = 0; i < parentForm.darkNumericUpDown1.Value + 1; i++)
                    {
                        Sgml.draw_line(x1, y1 + i, x2, y1 + i);
                    }

                    for (var i = 0; i < parentForm.darkNumericUpDown2.Value + 1; i++)
                    {
                        Sgml.draw_line(x1 + i, y1, x1 + i, y2);
                    }

                    Sgml.draw_set_alpha(1);
                    Sgml.draw_set_color(Color.White);

                    Sgml.draw_set_aa(false);
              

                    if (selectedFrame.layers.Count > 0)
                    {
                        if (selectedFrame.layers[0].texture != null)
                        {
                            Sgml.draw_rectangle(origin, new Vector2(selectedFrame.layers[0].texture.Width, selectedFrame.layers[0].texture.Height), true);
                        }
                    }
                }

                basicEffect.View = Matrix.Identity;
                Sgml.m = Matrix.Identity;

                Sgml.draw_set_color(Color.White);
                Sgml.draw_text(new Vector2(10, 10), framerate.ToString());
                Sgml.draw_text(new Vector2(10, 30), "[X: " + Sgml.round(Sgml.mouse.X - 0.5f) + " Y: " + Sgml.round(Sgml.mouse.Y - 0.5f) + "]");
               // Sgml.draw_text(new Vector2(10, 50), parentForm.darkNumericUpDown1.Value.ToString());
                Sgml.draw_text(new Vector2(10, 70), cam.Zoom.ToString());
                Sgml.draw_text(new Vector2(10, 90), "DIR: " + Sgml.point_direction(toolOriginSubpixel, mouseSubpixel));
                Sgml.draw_text(new Vector2(10, 110), "CLICK: " + toolOriginSubpixel.X + "x " + toolOriginSubpixel.Y + "y");
            }
        }

        void UpdateGrid()
        {
            int cellSize = 16;
            int x = 0;
            int y = 0;

            RectangleF rect = RectangleF.Empty;

            Color c1 = Color.FromNonPremultiplied(68, 68, 68, 255);
            Color c2 = Color.FromNonPremultiplied(77, 77, 77, 255);

            bool flag = true;
            bool lastFlag = flag;

            gridSurface = Sgml.surface_create(Width, Height);
            Sgml.surface_set_target(gridSurface);
            for (var i = 0; i < Height / cellSize + 1; i++)
            {
                for (var j = 0; j < Width / cellSize + 1; j++)
                {
                    if (j == 0)
                    {
                        lastFlag = flag;
                    }

                    rect.Size = new Size2(cellSize, cellSize);
                    rect.Position = new Point2(x, y);

                    if (flag)
                    {
                        Sgml.draw_set_color(c1);
                    }
                    else
                    {
                        Sgml.draw_set_color(c2);
                    }

                    Sgml.draw_rectangle(rect, false);
                    x += cellSize;
                    flag = !flag;
                }

                x = 0;
                y += cellSize;
                lastFlag = !lastFlag;
                flag = lastFlag;
            }
            Sgml.surface_reset_target();
        }


        public void Rsize()
        {
            // Fix weird errors folks are getting with this method
            if (Editor != null)
            {
                Editor.graphics.Viewport = new Viewport(0, 0, this.Width, this.Height);
                m = Matrix.CreateOrthographicOffCenter(0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height,
                    0, 0, -1);
                UpdateGrid();
            }
        }

        public void WheelDown()
        {
            cam.TargetZoom -= 0.1f;
        }

        public void WheelUp()
        {
            cam.TargetZoom += 0.1f;
        }

        public void ClickLock(MouseButtons btn)
        {
            mouseLocked = true;
            MousePrevious = Sgml.mouse;

            if (btn == MouseButtons.Middle)
            {
                panView = true;
                helpVec = cam.Camera.ScreenToWorld(MousePosition);
            }
        }

        public void ClickUp()
        {
            panView = false;
            occupiedPositions.Clear();
        }

        public void MouseDrag(System.Drawing.Point pos)
        {
          
        }

        public void AaToggled()
        {
            if (parentForm == null)
            {
                if (selectedXIndex != -1 && selectedYIndex != -1)
                {
                    selectedImageIndex = 1;
                   // imageOverlay = Sgml.surface_create((int)parentForm.darkNumericUpDown1.Value, (int)parentForm.darkNumericUpDown2.Value);

                    Sgml.surface_set_target(imageOverlay);
                    Sgml.draw_clear_transparent();
                    Sgml.surface_reset_target();
                }
                return;
            }

            if (parentForm.drawModeOn)
            {
                if (selectedXIndex != -1 && selectedYIndex != -1)
                {
                    selectedImageIndex = 1;
                   // imageOverlay = Sgml.surface_create((int)parentForm.darkNumericUpDown1.Value, (int)parentForm.darkNumericUpDown2.Value);
                   
                    Sgml.surface_set_target(imageOverlay);
                    Sgml.draw_clear_transparent();
                    Sgml.surface_reset_target();
                }
            }
        }

        public void MoveView()
        {
            if (panView)
            {
                cam.TargetPosition = new Vector2(cam.Position.X + helpVec.X - MousePositionTranslated.X, cam.Position.Y + helpVec.Y - MousePositionTranslated.Y);
            }
        }

    }
}
