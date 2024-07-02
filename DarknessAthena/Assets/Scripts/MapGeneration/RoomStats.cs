using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomStats : MonoBehaviour
{
    public float sizeX;
    public float sizeY;
    public Vector3 Middle;
    public int idRoom = 0;

    public void SetStats(float x, float y, Vector3 position)
    {
        sizeX = x;
        sizeY = y;
        Middle = new Vector3((position.x + (x / 2f)), (position.y + (y / 2f)), position.z); 
    }

    public void Move(Vector2 move, float tileSize=0.16f)
    {
        transform.position = new Vector3(transform.position.x + Mathf.FloorToInt(move.x / tileSize) * tileSize,
            transform.position.y + Mathf.FloorToInt(move.y / tileSize) * tileSize, 0);
    }

    public bool isOverLapping(GameObject other)
    {
        return !(transform.position.x + (sizeX * 0.16f) <= other.GetComponent<Transform>().position.x ||
            transform.position.x >= other.GetComponent<Transform>().position.x + (other.GetComponent<RoomStats>().sizeX * 0.16f) ||
            transform.position.y + (sizeY * 0.16f) <= other.GetComponent<Transform>().position.y ||
            transform.position.y >= other.GetComponent<Transform>().position.y + (other.GetComponent<RoomStats>().sizeY * 0.16f));
    }
}
