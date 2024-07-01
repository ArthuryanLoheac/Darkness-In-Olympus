using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RoomGenerator : MonoBehaviour
{
    public GameObject[] Grounds;
    public GameObject WallDown;
    public GameObject WallDownRight;
    public GameObject WallDownRightLink;
    public GameObject WallDownLeft;
    public GameObject WallDownLeftLink;
    public GameObject WallUp;
    public GameObject WallUpBlock;
    public GameObject WallLeft;
    public GameObject WallRight;
    public GameObject WallFull;
    public GameObject WallFullRightLeft;
    public GameObject WallFullRight;
    public GameObject WallFullLeft;
    public GameObject WallFullUp;
    public GameObject Background;
    private List<Vector2> lst;

    private LoadingManager LManager;
    public Vector2 Min;
    public Vector2 Max;

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

    int getValueatPos(float x, float y, List<Vector2> PositionGrounds)
    {
        bool Up = isInList(x, y - 0.16f, PositionGrounds);
        bool Down = isInList(x, y + 0.16f, PositionGrounds);
        bool Left = isInList(x + 0.16f, y, PositionGrounds);
        bool Right = isInList(x - 0.16f, y, PositionGrounds);
        bool UpRight = isInList(x + 0.16f, y + 0.16f, PositionGrounds);
        bool UpLeft = isInList(x - 0.16f, y + 0.16f, PositionGrounds);
        bool DownRight = isInList(x + 0.16f, y - 0.16f, PositionGrounds);
        bool DownLeft = isInList(x - 0.16f, y - 0.16f, PositionGrounds);

        if (Up && Down && Left && Right)
            return 1;
        else if (Up)
            return 2;
        else if (Down)
            return 3;
        else if (Left)
            return 4;
        else if (Right)
            return 5;
        else if (UpRight)
            return 6;
        else if (UpLeft)
            return 7;
        else if (DownRight)
            return 8;
        else if (DownLeft)
            return 9;
        return 0;
    }

    void GenerateRightWall(float x, float y, List<Vector2> PositionGrounds)
    {
        bool Up = isInList(x, y - 0.16f, PositionGrounds);
        bool Down = isInList(x, y + 0.16f, PositionGrounds);
        bool Left = isInList(x + 0.16f, y, PositionGrounds);
        bool Right = isInList(x - 0.16f, y, PositionGrounds);
        bool UpRight = isInList(x + 0.16f, y + 0.16f, PositionGrounds);
        bool UpLeft = isInList(x - 0.16f, y + 0.16f, PositionGrounds);
        bool DownRight = isInList(x + 0.16f, y - 0.16f, PositionGrounds);
        bool DownLeft = isInList(x - 0.16f, y - 0.16f, PositionGrounds);

        if (Up && Down && Left && Right) {
            if (!Down)
                Instantiate(WallUpBlock, new Vector3(x, y, 0), Quaternion.identity, transform);
            else
                Instantiate(WallUp, new Vector3(x, y, 0), Quaternion.identity, transform);
        } else if (Left && Right && Up) {
            if (!Down)
                Instantiate(WallUpBlock, new Vector3(x, y, 0), Quaternion.identity, transform);
            else
                Instantiate(WallUp, new Vector3(x, y, 0), Quaternion.identity, transform);
        } else if (Left && Right && Down)
            Instantiate(WallFullUp, new Vector3(x, y, 0), Quaternion.identity, transform);
        else if (Left && Right)
            Instantiate(WallFull, new Vector3(x, y, 0), Quaternion.identity, transform);
        else if (Up) {
            if (!Down)
                Instantiate(WallUpBlock, new Vector3(x, y, 0), Quaternion.identity, transform);
            else
                Instantiate(WallUp, new Vector3(x, y, 0), Quaternion.identity, transform);
        } else if (Down) {
            if (getValueatPos(x, y - 0.16f, PositionGrounds) == 4 || getValueatPos(x, y - 0.16f, PositionGrounds) == 6 ||
                (getValueatPos(x, y - 0.16f, PositionGrounds) == 1 && Left) || (getValueatPos(x, y - 0.16f, PositionGrounds) == 2 && Left))
                if ((DownRight || getValueatPos(x + 0.16f, y - 0.16f, PositionGrounds) == 3) && (DownLeft || getValueatPos(x - 0.16f, y - 0.16f, PositionGrounds) == 3))
                    Instantiate(WallFullUp, new Vector3(x, y, 0), Quaternion.identity, transform);
                else
                    Instantiate(WallDownRightLink, new Vector3(x, y, 0), Quaternion.identity, transform);
            else if (getValueatPos(x, y - 0.16f, PositionGrounds) == 5 || getValueatPos(x, y - 0.16f, PositionGrounds) == 7 ||
                (getValueatPos(x, y - 0.16f, PositionGrounds) == 1 && Right) || (getValueatPos(x, y - 0.16f, PositionGrounds) == 2 && Right))
                Instantiate(WallDownLeftLink, new Vector3(x, y, 0), Quaternion.identity, transform);
            else 
                Instantiate(WallDown, new Vector3(x, y, 0), Quaternion.identity, transform);
        } else if (Left) {
            if (getValueatPos(x - 0.16f, y, PositionGrounds) == 2)
                Instantiate(WallFull, new Vector3(x, y, 0), Quaternion.identity, transform);
            else if (getValueatPos(x - 0.16f, y, PositionGrounds) == 3)
                Instantiate(WallFullLeft, new Vector3(x, y, 0), Quaternion.identity, transform);
            else
                Instantiate(WallLeft, new Vector3(x, y, 0), Quaternion.identity, transform);
        } else if (Right) {
            if (getValueatPos(x + 0.16f, y, PositionGrounds) == 2)
                Instantiate(WallFull, new Vector3(x, y, 0), Quaternion.identity, transform);
            else if (getValueatPos(x + 0.16f, y, PositionGrounds) == 3)
                Instantiate(WallFullRight, new Vector3(x, y, 0), Quaternion.identity, transform);
            else
                Instantiate(WallRight, new Vector3(x, y, 0), Quaternion.identity, transform);
        } else if (UpRight) {
            if (getValueatPos(x, y - 0.16f, PositionGrounds) == 4)
                Instantiate(WallLeft, new Vector3(x, y, 0), Quaternion.identity, transform);
            else
                Instantiate(WallDownLeft, new Vector3(x, y, 0), Quaternion.identity, transform);
        } else if (UpLeft) {
            if (getValueatPos(x, y - 0.16f, PositionGrounds) == 4)
                Instantiate(WallRight, new Vector3(x, y, 0), Quaternion.identity, transform);
            else
                Instantiate(WallDownRight, new Vector3(x, y, 0), Quaternion.identity, transform);
        } else if (DownRight){
            if (getValueatPos(x + 0.16f, y, PositionGrounds) == 2 && getValueatPos(x - 0.16f, y, PositionGrounds) == 2)
                Instantiate(WallFull, new Vector3(x, y, 0), Quaternion.identity, transform);
            else 
                Instantiate(WallLeft, new Vector3(x, y, 0), Quaternion.identity, transform);
        }else if (DownLeft){
            Instantiate(WallRight, new Vector3(x, y, 0), Quaternion.identity, transform);
        } else {
            Instantiate(Background, new Vector3(x, y, 0), Quaternion.identity, transform);
        }
    }

    public void SetExternWalls()
    {
        for (float x = Max.x; x < Max.x + 1.6f; x += 0.16f) {
            for (float y = Min.y - 1.6f; y < Max.y + 1.6f; y += 0.16f) {
                Instantiate(Background, new Vector3(x, y, 0), Quaternion.identity, transform);
            }   
        }
        for (float y = Max.y; y < Max.y + 1.6f; y += 0.16f) {
            for (float x = Min.x; x < Max.x; x += 0.16f) {
                Instantiate(Background, new Vector3(x, y, 0), Quaternion.identity, transform);
            }   
        }
        for (float x = Min.x - 1.6f; x < Min.x; x += 0.16f) {
            for (float y = Min.y - 1.6f; y < Max.y + 1.6f; y += 0.16f) {
                Instantiate(Background, new Vector3(x, y, 0), Quaternion.identity, transform);
            }   
        }
        for (float y = Min.y - 1.6f; y < Min.y; y += 0.16f) {
            for (float x = Min.x; x < Max.x; x += 0.16f) {
                Instantiate(Background, new Vector3(x, y, 0), Quaternion.identity, transform);
            }   
        }
    }

    public void ComputeMinMaxValue(List<Vector2> PositionGrounds)
    {
        lst = PositionGrounds;
        List<float> ValueX = new List<float>();
        List<float> ValueY = new List<float>();

        foreach (Vector2 vect in PositionGrounds) {
            ValueX.Add(vect.x);
            ValueY.Add(vect.y);
        }

        Min = new Vector2(ValueX.Min() - 0.16f, ValueY.Min() - 0.16f);
        Max = new Vector2(ValueX.Max() + 0.16f, ValueY.Max() + 0.16f);
    }

    public void GenerateWalls(List<Vector2> PositionGrounds, float x)
    {
        for (float y = Min.y; y < Max.y; y += 0.16f) {
            if (!isInList(x, y, PositionGrounds))
                GenerateRightWall(x, y, PositionGrounds);
        }
    }

    void Awake()
    {
        LManager = GameObject.Find("LoadingManager").GetComponent<LoadingManager>();
    }
}
