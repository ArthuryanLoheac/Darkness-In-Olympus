using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class MapGenerator : MonoBehaviour
{
    public GameObject Room_Indicator;
    public int nb_rooms = 20;
    private RoomGenerator RG;
    public GameObject Player;
    public GenerateMobs GenMobs;

    private int iGen = 0;
    private List<EdgeVect> newedges2;
    private List<Vector2> PositionGrounds;

    private LoadingManager LManager;
    private float xVal;

    private void Generate_Map()
    {
        Vector3 position;
        for (int i = 0; i < nb_rooms; i++) {
            position = new Vector3 (Random.Range(-(nb_rooms / 4f), (nb_rooms / 4f)), Random.Range(-(nb_rooms / 4f), (nb_rooms / 4f)), 0f);
            position = new Vector3 (Mathf.FloorToInt(position.x / 0.16f) * 0.16f, Mathf.FloorToInt(position.y / 0.16f) * 0.16f, 0f);
            GameObject Room = Instantiate(Room_Indicator, position, Quaternion.identity, this.transform);
            RG.Spawn_Rectangle(position, Room);
        }
    }

    private bool IsAnyRoomOverLapped()
    {
        for (int current = 0; current < transform.childCount; current++) {
            for (int other = 0; other < transform.childCount; other++) {
                if (other != current) {
                    if (transform.GetChild(current).GetComponent<RoomStats>().isOverLapping(transform.GetChild(other).gameObject))
                        return true;
                }
            }
        }
        return false;
    }

    private void Seperate_Rooms()
    {
        do {
            for (int current = 0; current < transform.childCount; current++) {
                for (int other = 0; other < transform.childCount; other++) {
                    if (other == current || !transform.GetChild(current).GetComponent<RoomStats>().isOverLapping(transform.GetChild(other).gameObject)) continue;
                    
                    var direction = (transform.GetChild(other).GetComponent<RoomStats>().Middle - transform.GetChild(current).GetComponent<RoomStats>().Middle).normalized;

                    transform.GetChild(current).GetComponent<RoomStats>().Move(-direction, 0.16f);
                    transform.GetChild(other).GetComponent<RoomStats>().Move(direction, 0.16f);
                }
            }
        } while (IsAnyRoomOverLapped());
    }

    void DrawTriangles(List<EdgeVect> triangles)
    {
        foreach (EdgeVect t in triangles)
        {
            Debug.DrawLine(t.p1, t.p2, Color.red, 100f);
        }
    }

    List<EdgeVect> Make_Triangulation()
    {
        List<Point> points = new List<Point>();
        //Get All Center of rooms
        for (int i = 0; i < transform.childCount; i++) {
            points.Add(new Point(transform.GetChild(i).position.x + (transform.GetChild(i).gameObject.GetComponent<RoomStats>().sizeX * 0.16f / 2f),
                                transform.GetChild(i).position.y + (transform.GetChild(i).gameObject.GetComponent<RoomStats>().sizeY * 0.16f / 2f),
                                transform.GetChild(i).gameObject.GetComponent<RoomStats>().sizeX,
                                transform.GetChild(i).gameObject.GetComponent<RoomStats>().sizeY));
        }

        //Compute Triangles
        List<Triangle> triangles = DelaunayTriangulation.Triangulate(points);
        //Get Edges From triangles
        List<EdgeVect> edges = EdgeVect.getEdgesFromTriangles(triangles);
        // Get Minimal Edges 
        List<EdgeVect> newedges = MinimalSpanningTree.ComputeMinimalSpanningTree(edges, nb_rooms);
        List<EdgeVect> newedges2 = MinimalSpanningTree.AddRandomEdges(edges, newedges);
        return newedges2;
    }

    void Instantiate_horline(float a, float b, GameObject obj, float x, float maxa, float maxb)
    {
        if (a < b) {
            for (float y = a; y <= maxb; y += 0.16f)
                Instantiate(RG.Grounds[Random.Range(0, RG.Grounds.Length)], new Vector3(x, y, 0), Quaternion.identity, obj.transform);
        } else {
            for (float y = b; y <= maxa; y += 0.16f)
                Instantiate(RG.Grounds[Random.Range(0, RG.Grounds.Length)], new Vector3(x, y, 0), Quaternion.identity, obj.transform);
        }
    }

    void Instantiate_verline(float a, float b, GameObject obj, float y, float maxa, float maxb)
    {
        if (a < b) {
            for (float x = a - 0.16f; x <= maxb; x += 0.16f)
                Instantiate(RG.Grounds[Random.Range(0, RG.Grounds.Length)], new Vector3(x, y, 0), Quaternion.identity, obj.transform);
        } else {
            for (float x = b - 0.16f; x <= maxa; x += 0.16f)
                Instantiate(RG.Grounds[Random.Range(0, RG.Grounds.Length)], new Vector3(x, y, 0), Quaternion.identity, obj.transform);
        }
    }

    bool IsCollidingInX(Vector3 p1pos, Vector3 p2pos, Vector2 p1size, Vector2 p2size)
    {
        if (p2pos.x - (p2size.x / 2f * 0.16f) > p1pos.x - (p1size.x / 2f * 0.16f) &&
            p2pos.x - (p2size.x / 2f * 0.16f) <= p1pos.x + (p1size.x / 2f * 0.16f)) {
            return true;
        }
        if (p1pos.x - (p1size.x / 2f * 0.16f) > p2pos.x - (p2size.x / 2f * 0.16f) &&
            p1pos.x - (p1size.x / 2f * 0.16f) <= p2pos.x + (p2size.x / 2f * 0.16f)) {
            return true;
        }
        return false;
    }

    void Make_Couloirs(List<EdgeVect> newedges2)
    {
        Vector3 p1pos, p2pos, maxp1pos, maxp2pos;
        Vector2 p1size, p2size;
        int i = 0;

        foreach (EdgeVect vect in newedges2) {
            i++;
            maxp1pos = vect.p1;
            maxp2pos = vect.p2;
            //rounded position to clip to grid
            p1pos = new Vector3 (Mathf.FloorToInt(vect.p1.x / 0.16f) * 0.16f, Mathf.FloorToInt(vect.p1.y / 0.16f) * 0.16f, vect.p1.z);
            p2pos = new Vector3 (Mathf.FloorToInt(vect.p2.x / 0.16f) * 0.16f, Mathf.FloorToInt(vect.p2.y / 0.16f) * 0.16f, vect.p2.z);
            p1size = vect.sizep1;
            p2size = vect.sizep2;
            GameObject Couloir = new GameObject("Couloir_" + i.ToString());
            Couloir.transform.SetParent(transform);
            Instantiate_horline(p1pos.y, p2pos.y, Couloir, p1pos.x, maxp1pos.y, maxp2pos.y);
            Instantiate_horline(p1pos.y, p2pos.y, Couloir, p1pos.x - 0.16f, maxp1pos.y, maxp2pos.y);
            Instantiate_verline(p1pos.x, p2pos.x, Couloir, p2pos.y, maxp1pos.x, maxp2pos.x);
            Instantiate_verline(p1pos.x, p2pos.x, Couloir, p2pos.y - 0.16f, maxp1pos.x, maxp2pos.x);
        }
    }

    List<Vector2> GetPositions()
    {
        List<Vector2> PositionGrounds = new List<Vector2>();

        for (int idChildren = 0; idChildren < transform.childCount; idChildren++) {
            GameObject Children = transform.GetChild(idChildren).gameObject;
            for (int IdGround = 0; IdGround < Children.transform.childCount; IdGround++) {
                GameObject GroundChild = Children.transform.GetChild(IdGround).gameObject;
                PositionGrounds.Add(new Vector2(GroundChild.transform.position.x, GroundChild.transform.position.y));
            }
        }
        return PositionGrounds;
    }

    private void SetPlayerCenterFirstRoom()
    {
        Player.transform.position = new Vector3(
                transform.GetChild(0).position.x + (transform.GetChild(0).GetComponent<RoomStats>().sizeX / 2) * 0.16f,
                transform.GetChild(0).position.y + (transform.GetChild(0).GetComponent<RoomStats>().sizeY / 2) * 0.16f,
                transform.GetChild(0).position.z);
    }

    void Update()
    {
        if (iGen == 0) {
            Generate_Map();
            Seperate_Rooms();
            newedges2 = Make_Triangulation();
            Make_Couloirs(newedges2);
            PositionGrounds = GetPositions();
            RG.ComputeMinMaxValue(PositionGrounds);
            xVal = RG.Min.x;
            iGen++;
        } if (iGen == 1) {
            RG.GenerateWalls(PositionGrounds, xVal);
            xVal += 0.16f;
            LManager.setLoadingValue((xVal - RG.Min.x) / (RG.Max.x - RG.Min.x));
            if (!(xVal < RG.Max.x))
                iGen++;
        } else if (iGen == 2) {
            RG.SetExternWalls();
            SetPlayerCenterFirstRoom();
            GenMobs.GenerateMobInRooms(nb_rooms);
            iGen++;
        } else if (iGen == 3){
            LManager.HideLoading();
            iGen++;
        }
    }

    void Awake()
    {
        LManager = GameObject.Find("LoadingManager").GetComponent<LoadingManager>();
        GenMobs = this.GetComponent<GenerateMobs>();
        RG = this.GetComponent<RoomGenerator>();
        LManager.ShowLoading();
    }
}
