using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point
{
    public float x, y;

    public Point(float x, float y)
    {
        this.x = x;
        this.y = y;
    }
}

public class Triangle
{
    public Point a, b, c;

    public Triangle(Point a, Point b, Point c)
    {
        this.a = a;
        this.b = b;
        this.c = c;
    }

    public bool ContainsVertex(Point p)
    {
        return a == p || b == p || c == p;
    }

    public bool CircumCircleContains(Point p)
    {
        float ab = (a.x * a.x) + (a.y * a.y);
        float cd = (b.x * b.x) + (b.y * b.y);
        float ef = (c.x * c.x) + (c.y * c.y);

        float circumX = (ab * (c.y - b.y) + cd * (a.y - c.y) + ef * (b.y - a.y)) / (a.x * (c.y - b.y) + b.x * (a.y - c.y) + c.x * (b.y - a.y)) / 2f;
        float circumY = (ab * (c.x - b.x) + cd * (a.x - c.x) + ef * (b.x - a.x)) / (a.y * (c.x - b.x) + b.y * (a.x - c.x) + c.y * (b.x - a.x)) / 2f;
        float circumRadius = Mathf.Sqrt(((a.x - circumX) * (a.x - circumX)) + ((a.y - circumY) * (a.y - circumY)));

        float dist = Mathf.Sqrt(((p.x - circumX) * (p.x - circumX)) + ((p.y - circumY) * (p.y - circumY)));
        return dist <= circumRadius;
    }
}

public class DelaunayTriangulation
{
    public static List<Triangle> Triangulate(List<Point> points)
    {
        List<Triangle> triangles = new List<Triangle>();

        // Super triangle
        float minX = points[0].x;
        float minY = points[0].y;
        float maxX = minX;
        float maxY = minY;

        //compute minimal and max
        foreach (Point p in points)
        {
            if (p.x < minX) minX = p.x;
            if (p.y < minY) minY = p.y;
            if (p.x > maxX) maxX = p.x;
            if (p.y > maxY) maxY = p.y;
        }

        float dx = maxX - minX;
        float dy = maxY - minY;
        float deltaMax = Mathf.Max(dx, dy);
        float midx = (minX + maxX) / 2f;
        float midy = (minY + maxY) / 2f;

        Point p1 = new Point(midx - 20 * deltaMax, midy - deltaMax);
        Point p2 = new Point(midx, midy + 20 * deltaMax);
        Point p3 = new Point(midx + 20 * deltaMax, midy - deltaMax);

        triangles.Add(new Triangle(p1, p2, p3));

        foreach (Point p in points)
        {
            List<Triangle> badTriangles = new List<Triangle>();

            foreach (Triangle t in triangles)
            {
                if (t.CircumCircleContains(p))
                {
                    badTriangles.Add(t);
                }
            }

            List<Edge> polygon = new List<Edge>();

            foreach (Triangle t in badTriangles)
            {
                AddEdge(t.a, t.b, polygon);
                AddEdge(t.b, t.c, polygon);
                AddEdge(t.c, t.a, polygon);
            }

            foreach (Triangle t in badTriangles)
            {
                triangles.Remove(t);
            }

            foreach (Edge e in polygon)
            {
                triangles.Add(new Triangle(e.p1, e.p2, p));
            }
        }

        List<Triangle> finalTriangles = new List<Triangle>();

        foreach (Triangle t in triangles)
        {
            if (!t.ContainsVertex(p1) && !t.ContainsVertex(p2) && !t.ContainsVertex(p3))
            {
                finalTriangles.Add(t);
            }
        }

        return finalTriangles;
    }

    private static void AddEdge(Point p1, Point p2, List<Edge> edges)
    {
        Edge edge = new Edge(p1, p2);

        if (edges.Contains(edge))
        {
            edges.Remove(edge);
        }
        else
        {
            edges.Add(edge);
        }
    }
}

public class Edge
{
    public Point p1, p2;

    public Edge(Point p1, Point p2)
    {
        this.p1 = p1;
        this.p2 = p2;
    }

    public override bool Equals(object obj)
    {
        Edge e = obj as Edge;
        return (e.p1 == p1 && e.p2 == p2) || (e.p1 == p2 && e.p2 == p1);
    }

    public override int GetHashCode()
    {
        return p1.GetHashCode() ^ p2.GetHashCode();
    }
}
