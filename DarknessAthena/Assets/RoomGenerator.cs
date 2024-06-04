using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RoomGenerator : MonoBehaviour
{
    public GameObject[] Grounds;
    public GameObject WallDown;
    public GameObject WallUp;
    public GameObject WallLeft;
    public GameObject WallLeftDown;
    public GameObject WallRight;
    public GameObject WallRightDown;
    public List<Vector2> lst;

    public void Spawn_Rectangle(Vector3 position, GameObject Room)
    {
        Vector2 size = new Vector2 (Random.Range(4, 20), Random.Range(4, 20));
        
        RoomStats stats = Room.GetComponent<RoomStats>();
        stats.SetStats(size.x, size.y, position);
        for (int x = 0; x < size.x; x++) {
            for (int y = 0; y < size.y; y++) {
                Vector3 positionUpt = new Vector3 (position.x + (x * 0.16f), position.y + (y * 0.16f), 0f);
                Instantiate(Grounds[Random.Range(0, Grounds.Length)], positionUpt, Quaternion.identity, Room.transform);
            }
        }
    }

    float GridValue(float v)
    {
        return Mathf.FloorToInt(v / 0.16f) * 0.16f;
    }

    bool isInList(float x, float y, List<Vector2> PositionGrounds)
    {
        foreach (Vector2 vect in PositionGrounds) {
            if (Vector2.Distance(vect, new Vector2(x, y)) <= 0.1f)
                return true;
        }
        return false;
    }

    public void GenerateWalls(List<Vector2> PositionGrounds)
    {
        lst = PositionGrounds;
        Vector2 Min;
        Vector2 Max;
        List<float> ValueX = new List<float>();
        List<float> ValueY = new List<float>();

        foreach (Vector2 vect in PositionGrounds) {
            ValueX.Add(vect.x);
            ValueY.Add(vect.y);
        }
        Min = new Vector2(ValueX.Min() - 0.16f, ValueY.Min() - 0.16f);
        Max = new Vector2(ValueX.Max() + 0.16f, ValueY.Max() + 0.16f);
        Debug.Log(Min);
        Debug.Log(Max);
        for (float x = Min.x; x < Max.x; x += 0.16f) {
            for (float y = Min.y; y < Max.y; y += 0.16f) {
                if (!isInList(x, y, PositionGrounds)) {
                    Instantiate(WallDown, new Vector3(x, y, 0), Quaternion.identity, transform);
                }
            }   
        }
    }
}
