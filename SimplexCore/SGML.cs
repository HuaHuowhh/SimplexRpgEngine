﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MoreLinq.Extensions;
using SharpDX;
using SharpDX.MediaFoundation.DirectX;
using SimplexIde;
using SimplexResources.Objects;
using Color = Microsoft.Xna.Framework.Color;
using Matrix = System.Drawing.Drawing2D.Matrix;
using Point = Microsoft.Xna.Framework.Point;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using RectangleF = MonoGame.Extended.RectangleF;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace SimplexCore
{

    public static partial class Sgml
    {
        public static List<GameObject> SceneObjects = new List<GameObject>();
        public static List<GameObject> SceneColliders = new List<GameObject>();
        public static List<TextureReference> Textures = new List<TextureReference>();
        public static List<SoundReference> Sounds = new List<SoundReference>();
        public static List<Effect> Shaders = new List<Effect>();
        private static int _randomSeed = DateTime.Now.Millisecond;
        public static Random _random = new Random();
        private static float _epsilon = 0.000001f;
        public static SpriteBatch sb;
        private static Color _color = Color.White;
        public static SpriteFont drawFont;
        public static DynamicVertexBuffer vb;
        public static BasicEffect be;
        public static Microsoft.Xna.Framework.Matrix m;
        public static List<RoomLayer> roomLayers = new List<RoomLayer>();
        public static GameRoom currentRoom;
        public static GameObject realObject = null;
        public static Effect effect;
        static Rectangle tRect = Rectangle.Empty;
        public static SimplexProjectStructure currentProject;
      //  public static List<TimeLines> all_time_lines = new List<TimeLines>();

        public static Color DrawColor
        {
            get { return FinalizeColor(_color); }
            set { _color = value; }
        }

        private static float drawAlpha = 1;
        private static int drawCirclePrecision = 32;

        private static Color FinalizeColor(Color c)
        {
            c *= drawAlpha;
            return c;
        }
        
        public static bool place_empty(Vector2 position,Type[] obj, bool self = false)
        {
            Guid blockedId = new Guid();
            if (self)
            {
                blockedId = currentObject.Id;
            }

            foreach (GameObject g in SceneObjects)
            {
                if (g.CollidingWithPoint(position))
                {
                    if (!(self && g.Id == blockedId) && obj.Contains(g.GetType()))
                    {
                        return false;
                    }
                }

            }
            return true;
        }
        
        public static bool place_empty(Vector2 position, bool self = false)
        {
            Guid blockedId = new Guid();
            if (self)
            {
                blockedId = currentObject.Id;
            }

            foreach (GameObject g in SceneObjects)
            {
                if (g.CollidingWithPoint(position))
                {
                    if (!(self && g.Id == blockedId))
                    {
                        return false;
                    }
                }

            }
            return true;
        }

        public static bool place_empty(Vector2 position)
        {
            foreach (GameObject g in SceneObjects)
            {
                if (g.CollidingWithPoint(position))
                {
                    return false;
                }

            }

            return true;
        }

        public static bool place_empty_rectangle(RectangleF rectangle)
        {
            return PlaceEmptyRectangle(rectangle);
        }

        public static bool PlaceEmptyRectangle(RectangleF rr)
        {
            foreach (RoomLayer rl in roomLayers)
            {
                if (rl.Visible)
                {
                    if (rl is ObjectLayer)
                    {
                        ObjectLayer ol = (ObjectLayer)rl;
                        foreach (GameObject g in ol.Objects)
                        {
                            if (g == currentObject)
                            {
                                continue;
                            }

                            RectangleF r = new Rectangle((int)g.Position.X, (int)g.Position.Y, g.Sprite.ImageRectangle.Width, g.Sprite.ImageRectangle.Height);
                            if (r.Intersects(rr))
                            {
                                return false;
                            }
                        }
                    }
                }
            }

            return true;
        }

        public static GameObject instance_place(Vector2 vec, Type go)
        {
            List<GameObject> appliable = SceneObjects.FindAll(x => x.GetType() == go);
            Point s = currentObject.Sprite.ImageRectangle.Size;
            Rectangle fr = new Rectangle((int)vec.X, (int)vec.Y, s.X, s.Y);

            foreach (GameObject g in appliable)
            {
                RectangleF r = new Rectangle((int)g.Position.X, (int)g.Position.Y, g.Sprite.ImageRectangle.Width, g.Sprite.ImageRectangle.Height);
                if (r.Intersects(fr))
                {
                    return g;
                }
            }

            return null;
        }
        
        public static Color RandomColor()
        {
            return new Color(_random.Next(255), _random.Next(255), _random.Next(255));
        }

        public static void DrawRectangle(float x1, float y1, float x2, float y2, bool outline)
        {
            if (!outline)
            {
                sb.FillRectangle(new RectangleF(x1, y1, Math.Abs(x1 - x2), Math.Abs(y1 - y2)), DrawColor);
            }
            else
            {
                sb.DrawRectangle(new RectangleF(x1, y1, Math.Abs(x1 - x2), Math.Abs(y1 - y2)), DrawColor);
            }          
        }

        public static void DrawRectangle(Vector2 position, Vector2 size, bool outline)
        {
            if (!outline)
            {
                sb.FillRectangle(new RectangleF(position.X, position.Y, size.X, size.Y), DrawColor);
            }
            else
            {
                sb.DrawRectangle(new RectangleF(position.X, position.Y, size.X, size.Y), DrawColor);
            }
                    
        }

        public static void DrawRectangle(Vector2 position, Vector2 size, bool outline, float lineThickness)
        {
            if (!outline)
            {
                sb.FillRectangle(new RectangleF(position.X, position.Y, size.X, size.Y), DrawColor);
            }
            else
            {
                sb.DrawRectangle(new RectangleF(position.X, position.Y, size.X, size.Y), DrawColor, lineThickness);
            }
        }

        public static void draw_set_alpha(double alpha)
        {
            alpha = alpha.Clamp(0, 1);
            drawAlpha = (float)alpha;
        }

        public static void draw_set_color(Color c)
        {
            DrawColor = c;
        }

        public static double DrawGetAlpha()
        {
            return drawAlpha;
        }

        public static Color DrawGetColor()
        {
            return DrawColor;
        }

        /* ----------------------------------------------------------------------------------------------------
         * Math
         * ----------------------------------------------------------------------------------------------------
         */


        /*
         *  ------------------------------------- Random ------------------------------------
         */
        public static T choose<T>(params T[] items)
        {
            return items[_random.Next(0, items.Length)];
        }

        public static float random(float n)
        {
            return _random.NextFloat(0f, n);
        }

        public static float random_range(float n1, float n2)
        {
            return _random.NextFloat(n1, n2);
        }

        public static int irandom(int n)
        {
            return _random.Next(n);
        }

        public static int irandom_range(int n1, int n2)
        {
            return _random.Next(n1, n2);
        }

        public static void random_set_seed(int seed)
        {
            _randomSeed = seed;
            _random = new Random(_randomSeed);
        }

        public static int random_get_seed()
        {
            return _randomSeed;
        }

        public static void randomize()
        {
            _randomSeed = DateTime.Now.Millisecond;
            _random = new Random(_randomSeed);
        }

        public static double boxstep(double a, double b, double amt)
        {
            double p;

            if (Math.Abs(a - b) < 0.001)
            {
                return -1;
            }

            p = (amt - a) / (b - a);
            if (p <= 0) {return 0;}
            if (p >= 1) {return 1;}
            return p;
        }

        public static double smoothstep(double a, double b, double amt)
        {
            // 3x2−2x3
            double p;
            if (amt < a) {return 0;}
            if (amt >= b) {return 1;}
            if (Math.Abs(a - b) < 0.001) {return -1;}

            p = (amt - a) / (b - a);
            return (p * p * (3 - 2 * p));
        }

        public static int factorial(int n)
        {
            if (n <= 1) {return 1;}

            return n * factorial(n - 1);
        }

        public static int factorial_stirling(int n)
        {
            if (n == 1)
            {
                return 1;
            }

            double z;
            double e = Math.E;

            z = Math.Sqrt(2 * Math.PI * n) * Math.Pow((n / e), n);
            return (int)(z);
        }

        public static int permutations(int set, int subset)
        {
            int f, k, l, m, n;
            n = set;
            k = subset;

            m = n - k;
            if (m < 0) {return -1;}

            f = 1;
            for (l = n; l > m; l -= 1)
            {
                f *= l;
            }
            return f;          
        }

        public static int fibonacci(int n)
        {
            return (int)round(power((1 + sqrt(5)) / 2, n) / sqrt(5));
        }

        public static int lcm(int a, int b)
        {
            int c, r;

            a = max(a, b);
            b = min(a, b);
            c = a * b;
            while (b != 0)
            {
                r = a % b;
                a = b;
                b = r;
            }
            return (c / a);
        }

        public static int gcd(int a, int b)
        {
            int r;

            a = max(a, b);
            b = min(a, b);
            while (b != 0)
            {
                r = a % b;
                a = b;
                b = r;
            }

            return a;
        }

        public static int next_pow2(int a)
        {          
            a--;
            a |= a >> 1;
            a |= a >> 2;
            a |= a >> 4;
            a |= a >> 8;
            a |= a >> 16;
            a++;

            return a;
        }

        public static int irandom_weighted(params int[] p)
        {
            int sum = 0;

            foreach (var t in p)
            {
                sum += t;
            }

            int rnd = irandom(sum);

            for (int i = 0; i < p.Length; i++)
            {
                if (rnd < p[i])
                {
                    return i;
                }

                rnd -= p[i];
            }

            return int.MinValue;
        }

        public static bool is_prime(int number)
        {
            if (number <= 1) return false;
            if (number == 2) return true;
            if (number % 2 == 0) return false;

            var boundary = (int)Math.Floor(Math.Sqrt(number));

            for (int i = 3; i <= boundary; i += 2)
            {
                if (number % i == 0)
                {
                    return false;
                }
            }

            return true;
        }

        public static List<int> primes_smaller(int n)
        {
            List<int> k = new List<int>();
            bool[] prime = new bool[n + 1];

            for (int i = 0; i < n; i++)
            {
                prime[i] = true;
            }

            for (int p = 2; p * p <= n; p++)
            {
                if (prime[p])
                {
                    for (int i = p * p; i <= n; i += p)
                    {
                        prime[i] = false;
                    }
                }
            }

            for (int i = 2; i <= n; i++)
            {
                if (prime[i])
                {
                    k.Add(i);
                }
            }

            return k;
        }

        /*
         *  ------------------------------------- Trigonometry ------------------------------------
         */
        public static double arccos(double x)
        {
            return ApplyEpsilon(Math.Acos(x));
        }

        public static double arcsin(double x)
        {
            return ApplyEpsilon(Math.Asin(x));
        }

        public static double arctan(double x)
        {
            return ApplyEpsilon(Math.Atan(x));
        }

        public static double arctan2(double x, double y)
        {
            return ApplyEpsilon(Math.Atan2(x, y));
        }

        public static double sin(double x)
        {
            return ApplyEpsilon(Math.Sin(x));
        }

        public static double cos(double x)
        {
            return ApplyEpsilon(Math.Cos(x));
        }

        public static double tan(double x)
        {
            return ApplyEpsilon(Math.Tan(x));
        }

        public static double dsin(double x)
        {
            return ApplyEpsilon(Math.Sin(MathHelper.ToRadians((float)x)));
        }

        public static double dcos(double x)
        {
            return ApplyEpsilon(Math.Cos(MathHelper.ToRadians((float)x)));
        }

        public static double dtan(double x)
        {
            return ApplyEpsilon(Math.Tan(MathHelper.ToRadians((float)x)));
        }

        public static double darcsin(double x)
        {
            return ApplyEpsilon(Math.Asin(Math.Asin(Math.Sin(MathHelper.ToRadians((float)x))) * (180 / Math.PI)) * (180 / Math.PI));
        }

        public static double darccos(double x)
        {
            return ApplyEpsilon(Math.Acos(Math.Acos(Math.Cos(MathHelper.ToRadians((float)x))) * (180 / Math.PI)) * (180 / Math.PI));
        }

        public static double darctan(double x)
        {
            return ApplyEpsilon(Math.Atan(Math.Atan(Math.Tan(MathHelper.ToRadians((float)x))) * (180 / Math.PI)) * (180 / Math.PI));
        }

        public static double darctan2(double y, double x)
        {
            return ApplyEpsilon(Math.Atan2(MathHelper.ToRadians((float)y), MathHelper.ToRadians((float)x)));
        }

        /*
         *  ------------------------------------- Rounding ------------------------------------
         */
        public static double degtorad(double x)
        {
            return ApplyEpsilon(MathHelper.ToRadians((float)x));
        }

        public static double radtodeg(double x)
        {
            return ApplyEpsilon(MathHelper.ToDegrees((float)x));
        }

        public static double lengthdir_x(double len, double dir)
        {
            dir = 360 - dir;
            return ApplyEpsilon(Math.Cos(dir * Math.PI / 180) * len);
        }

        public static double lengthdir_y(double len, double dir)
        {
            dir = 360 - dir;
            return ApplyEpsilon(Math.Sin(dir * Math.PI / 180) * len);
        }

        public static double round(double n)
        {
            return ApplyEpsilon(Math.Round(n));
        }

        public static double floor(double n)
        {
            return ApplyEpsilon(Math.Floor(n));
        }

        public static double frac(double n)
        {
            return ApplyEpsilon(n - Math.Truncate(n));
        }

        public static double abs(double n)
        {
            return ApplyEpsilon(Math.Abs(n));
        }

        public static int sign(double n)
        {
            return Math.Sign(n);
        }

        public static double ceil(double n)
        {
            return ApplyEpsilon(Math.Ceiling(n));
        }

        public static T max<T>(params T[] items)
        {
            return items.Max();
        }

        public static double mean(params double[] items)
        {
            return ApplyEpsilon(items.Average());
        }

        public static double median(params double[] items)
        {
            Array.Sort(items);
            return items[items.Length / 2];
        }

        public static T min<T>(params T[] items)
        {
            return items.Min();
        }

        public static double lerp(double a, double b, double amt)
        {
            return ApplyEpsilon(MathHelper.Lerp((float)a, (float)b, (float)amt));
        }

        public static double lerp_aggressive(double a, double b, double amt)
        {
            return ApplyEpsilonAggressive(MathHelper.Lerp((float)a, (float)b, (float)amt));
        }

        public static double clamp(double val, double min, double max)
        {
            return ApplyEpsilon(MathHelper.Clamp((float) val, (float) min, (float) max));
        }

        /*
         *  ------------------------------------- Misc ------------------------------------
         */

        public static double exp(double n)
        {
            return ApplyEpsilon(Math.Pow(Math.E, n));
        }

        public static double ln(double n)
        {
            return ApplyEpsilon(Math.Log(n, Math.E));
        }

        public static double power(double x, int n)
        {
            return ApplyEpsilon(Math.Pow(x, n));
        }

        public static double sqr(double x)
        {
            return ApplyEpsilon(Math.Pow(x, 2));
        }

        public static double sqrt(double x)
        {
            return ApplyEpsilon(Math.Sqrt(x));
        }

        public static double log2(double n)
        {
            return ApplyEpsilon(Math.Log(n, 2));
        }

        public static double log10(double n)
        {
            return ApplyEpsilon(Math.Log(n, 10));
        }

        public static double logn(double n, double val)
        {
            return ApplyEpsilon(Math.Log(val, n));
        }

        /*
         *  ------------------------------------- Vector ------------------------------------
         */
        // helper
        static double nDir(double a)
        {
            while (a < 0)
            {
                a += Math.PI * 2;
            }

            return a;
        }

        public static double point_direction(double x1, double y1, double x2, double y2)
        {
            double xDiff = 0;
           // xDiff = x2 - x1;

            double yDiff = 0; //y2 - y1;
            return ApplyEpsilon((Math.Atan2(xDiff, yDiff) * 180.0 / Math.PI));
        }

        public static float point_direction(Vector2 point1, Vector2 point2)
        {
            float xDiff = point2.X - point1.X;
            float yDiff = point2.Y - point1.Y;
            float d = ApplyEpsilon( (360 - (nDir(Math.Atan2(yDiff, xDiff)) * 180.0 / Math.PI)));

            return d;
        }

        public static double point_distance(double x1, double y1, double x2, double y2)
        {
            return ApplyEpsilon(Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2)));
        }

        public static double point_distance(Vector2 point1, Vector2 point2)
        {
            return ApplyEpsilon(Math.Sqrt(Math.Pow(point1.X - point2.X, 2) + Math.Pow(point1.Y - point2.Y, 2)));
        }

        public static double dot_product(double x1, double y1, double x2, double y2)
        {
            return ApplyEpsilon((x1 * x2) + (y1 * y2));
        }

        public static double dot_product(Vector2 point1, Vector2 point2)
        {
            return ApplyEpsilon(point1.Dot(point2));
        }

        public static double dot_product_normalised(double x1, double y1, double x2, double y2)
        {
            double dotProduct = dot_product(x1, y1, x2, y2);
            double mag1 = sqrt(x1 * x1 + y1 * y1);
            double mag2 = sqrt(x2 * x2 + y2 * y2);
            return ApplyEpsilon((dotProduct) / (mag1 * mag2));
        }

        public static double dot_product_normalised(Vector2 point1, Vector2 point2)
        {
            double dotProduct = dot_product(point1, point2);
            double mag1 = sqrt(point1.X * point1.X + point1.Y * point1.Y);
            double mag2 = sqrt(point2.X * point2.X + point2.Y * point2.Y);
            return ApplyEpsilon((dotProduct) / (mag1 * mag2));
        }

        public static double angle_difference(double src, double dest)
        {
            dest %= 360;
            src %= 360;
            return src - dest;
        }

        /*
         *  ------------------------------------- Epsilon ------------------------------------
         */
        public static void math_set_epsilon(float epsilon)
        {
            _epsilon = epsilon;
        }

        public static float math_get_epsilon()
        {
            return ApplyEpsilon(_epsilon);
        }

        private static float ApplyEpsilon(double x)
        {
            return (float)Math.Ceiling(x * (1 / _epsilon)) / (1 / _epsilon);
        }

        private static double ApplyEpsilonAggressive(double x)
        {
            return Math.Ceiling(x * (1 / 0.001)) / (1 / 0.001);
        }

        public static void with(GameObject obj)
        {
            currentObject = obj;
        }

        public static void with_reset()
        {
            currentObject = realObject;
        }

        /*
         *  ------------------------------------- Alarms ------------------------------------
         */
        public static void alarm_set(int index, int steps)
        {
            currentObject.Alarms[index].Steps = steps;
            currentObject.Alarms[index].Running = true;
        }

        public static int alarm_get(int index)
        {
            return currentObject.Alarms[index].Steps;
        }

        public static void alarm_set_running(int index, bool running)
        {
            currentObject.Alarms[index].Running = running;
        }

        public static bool alarm_get_running(int index)
        {
            return currentObject.Alarms[index].Running;
        }

        // rotate
        public static Vector2 rotate_vector2(Vector2 vec, Vector2 origin, double angleInDegrees)
        {
            double angleInRadians = angleInDegrees * (Math.PI / 180);
            double cosTheta = Math.Cos(angleInRadians);
            double sinTheta = Math.Sin(angleInRadians);

            Vector2 toReturn = new Vector2();
            toReturn.X = (float) (cosTheta * (vec.X - origin.X) - sinTheta * (vec.Y - origin.Y) + origin.X);
            toReturn.Y = (float) (sinTheta * (vec.X - origin.X) + cosTheta * (vec.Y - origin.Y) + origin.Y);

            return toReturn;
        }
        
        //timelines
     /*   public static TimeLines timeline_add(string name)
        {
            TimeLines line = new TimeLines(){ Name = name};
            all_time_lines.Add(line);
            return line;
        }

        public static bool timeline_exists(string name)
        {
            bool containsItem = all_time_lines.Any(item => item.Name == name);
            return containsItem;
        }

        public static int timeline_count(string name)
        {
            return all_time_lines[all_time_lines.FindIndex(x => x.Name == name)].Count;
        }*/
    }
}
