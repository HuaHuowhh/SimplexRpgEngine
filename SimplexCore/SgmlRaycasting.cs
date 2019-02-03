﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace SimplexCore
{
    public static partial class Sgml
    {
        public static bool point_in_rectangle(Point2 point, RectangleF rectangle)
        {
            return rectangle.Contains(point);
        }

        public static bool point_in_rectangle_rotated(Point2 point, Rectangle rectangle, double angle, Vector2 origin)
        {
            RotatedRectangle rr = rectangle_rotate(rectangle, origin, angle);
           // currentObject.rr = rr;
            return rr.Contains(point);
        }

        public static bool point_in_rectangle_rotated(Point2 point, RectangleF rectangle, double angle, Vector2 origin)
        {
            RotatedRectangle rr = rectangle_rotate(rectangle, origin, angle);
            //currentObject.rr = rr;
            return rr.Contains(point);
        }

        public static bool point_in_triangle(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p)
        {
            var s = p0.Y * p2.X - p0.X * p2.Y + (p2.Y - p0.Y) * p.X + (p0.X - p2.X) * p.Y;
            var t = p0.X * p1.Y - p0.Y * p1.X + (p0.Y - p1.Y) * p.X + (p1.X - p0.X) * p.Y;

            if ((s < 0) != (t < 0))
                return false;

            var A = -p1.Y * p2.X + p0.Y * (p2.X - p1.X) + p0.X * (p1.Y - p2.Y) + p1.X * p2.Y;

            return A < 0 ? (s <= 0 && s + t >= A) : (s >= 0 && s + t <= A);
        }


        public static RotatedRectangle rectangle_rotate(Rectangle r, Vector2 point, double angle)
        {
            Vector2 p1 = new Vector2(0, 0);
            p1 = rotate_vector2(p1, point, angle);
            p1.X += r.X;
            p1.Y += r.Y;

            Vector2 p2 = new Vector2(r.Width, 0);
            p2 = rotate_vector2(p2, point, angle);
            p2.X += r.X;
            p2.Y += r.Y;

            Vector2 p3 = new Vector2(0, r.Height);
            p3 = rotate_vector2(p3, point, angle);
            p3.X += r.X;
            p3.Y += r.Y;

            Vector2 p4 = new Vector2(r.Width, r.Height);
            p4 = rotate_vector2(p4, point, angle);
            p4.X += r.X;
            p4.Y += r.Y;

            return new RotatedRectangle(p1, p3, p4, p2);
        }

        public static RotatedRectangle rectangle_rotate(RectangleF r, Vector2 point, double angle)
        {
            Vector2 p1 = new Vector2(0, 0);
            p1 = rotate_vector2(p1, point, angle);
            p1.X += r.X;
            p1.Y += r.Y;

            Vector2 p2 = new Vector2(r.Width, 0);
            p2 = rotate_vector2(p2, point, angle);
            p2.X += r.X;
            p2.Y += r.Y;

            Vector2 p3 = new Vector2(0, r.Height);
            p3 = rotate_vector2(p3, point, angle);
            p3.X += r.X;
            p3.Y += r.Y;

            Vector2 p4 = new Vector2(r.Width, r.Height);
            p4 = rotate_vector2(p4, point, angle);
            p4.X += r.X;
            p4.Y += r.Y;

            return new RotatedRectangle(p1, p3, p4, p2);
        }

        public static Vector2? line_intersects_rectangle_rotate(Vector2 p1, Vector2 p2, RotatedRectangle r)
        {
            if (r != null)
            {
                Vector2? temp = Vector2.One;
                Vector2? lastScs = null;

                temp = line_intersets_line_point(p1, p2, r.Point1, r.Point2);
                if (temp.HasValue)
                {
                    lastScs = temp;
                }

                temp = line_intersets_line_point(p1, p2, r.Point2, r.Point3);
                if (temp.HasValue)
                {
                    if (lastScs == null)
                    {
                        lastScs = temp;
                    }
                    else
                    {
                        double k = point_distance(p1, temp.Value);

                        if (k < point_distance(p1, lastScs.Value))
                        {
                            lastScs = temp;
                        }
                    }
                }

                temp = line_intersets_line_point(p1, p2, r.Point3, r.Point4);
                if (temp.HasValue)
                {
                    if (lastScs == null)
                    {
                        lastScs = temp;
                    }
                    else
                    {
                        double k = point_distance(p1, temp.Value);

                        if (k < point_distance(p1, lastScs.Value))
                        {
                            lastScs = temp;
                        }
                    }
                }

                temp = line_intersets_line_point(p1, p2, r.Point4, r.Point1);
                if (temp.HasValue)
                {
                    if (lastScs == null)
                    {
                        lastScs = temp;
                    }
                    else
                    {
                        double k = point_distance(p1, temp.Value);

                        if (k < point_distance(p1, lastScs.Value))
                        {
                            lastScs = temp;
                        }
                    }
                }

                return lastScs;
            }

            return null;
        }

        public static Vector2? line_intersets_line_point(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, double tolerance = 0.001)
        {
            double x1 = p1.X, y1 = p1.Y;
            double x2 = p2.X, y2 = p2.Y;

            double x3 = p3.X, y3 = p3.Y;
            double x4 = p4.X, y4 = p4.Y;

            // equations of the form x = c (two vertical lines)
            if (Math.Abs(x1 - x2) < tolerance && Math.Abs(x3 - x4) < tolerance && Math.Abs(x1 - x3) < tolerance)
            {
                return Vector2.Zero;
            }

            //equations of the form y=c (two horizontal lines)
            if (Math.Abs(y1 - y2) < tolerance && Math.Abs(y3 - y4) < tolerance && Math.Abs(y1 - y3) < tolerance)
            {
                return Vector2.Zero;
            }

            //equations of the form x=c (two vertical lines)
            if (Math.Abs(x1 - x2) < tolerance && Math.Abs(x3 - x4) < tolerance)
            {
                return null;
            }

            //equations of the form y=c (two horizontal lines)
            if (Math.Abs(y1 - y2) < tolerance && Math.Abs(y3 - y4) < tolerance)
            {
                return null;
            }

            double x, y;

            if (Math.Abs(x1 - x2) < tolerance)
            {
                double m2 = (y4 - y3) / (x4 - x3);
                double c2 = -m2 * x3 + y3;

                x = x1;
                y = c2 + m2 * x1;
            }
            else if (Math.Abs(x3 - x4) < tolerance)
            {
                double m1 = (y2 - y1) / (x2 - x1);
                double c1 = -m1 * x1 + y1;

                x = x3;
                y = c1 + m1 * x3;
            }
            else
            {
                double m1 = (y2 - y1) / (x2 - x1);
                double c1 = -m1 * x1 + y1;

                double m2 = (y4 - y3) / (x4 - x3);
                double c2 = -m2 * x3 + y3;

                x = (c1 - c2) / (m2 - m1);
                y = c2 + m2 * x;

                if (!(Math.Abs(-m1 * x + y - c1) < tolerance && Math.Abs(-m2 * x + y - c2) < tolerance))
                {
                    return null;
                }
            }

            if (point_in_line(p1, p2, x, y) && point_in_line(p3, p4, x, y))
            {
                return new Vector2 { X = (float)x, Y = (float)y };
            }

            return null;
        }

        public static bool point_in_line(Vector2 p1, Vector2 p2, double x, double y)
        {
            return (x >= p1.X && x <= p2.X || x >= p2.X && x <= p1.X) && (y >= p1.Y && y <= p2.Y || y >= p2.Y && y <= p1.Y);
        }
    }
}
