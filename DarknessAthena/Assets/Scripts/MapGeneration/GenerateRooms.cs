using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GenerateRooms : MonoBehaviour
{
    private int nbChandelier;
    public GameObject ChandelierHigh;
    public GameObject ChandelierLow;

    public GameObject Skeletton_1;
    public GameObject Skeletton_2;
    public GameObject Vampire;
    public GameObject Skull;

    public GameObject Chest;

    public GameObject[] LstRooms;

    private GameObject GetMonster()
    {
        int Rand = Random.Range(0, 4);
        if (Rand == 0)
            return Skull;
        if (Rand == 1)
            return Vampire;
        if (Rand == 2)
            return Skeletton_2;
        else
            return Skeletton_1;
    }

    private GameObject GetChandelier()
    {
        int Rand = Random.Range(0, 1);
        if (Rand == 0)
            return ChandelierHigh;
        else
            return ChandelierLow;
    }

    private void SpawnMonster(RoomStats stats, Transform Children)
    {
        GameObject RoomSpawn = LstRooms[Random.Range(0, LstRooms.Length)];
        GameObject Monster = GetMonster();
        Vector3 Pos = stats.transform.position;
        GameObject Room = Instantiate(RoomSpawn,
            new Vector3 (Pos.x + ((stats.sizeX/2) - 0.5f) * 0.16f, Pos.y + ((stats.sizeY/2) - 0.5f) * 0.16f, 0),
            Quaternion.identity, Children);
        Room.transform.localScale = new Vector3(0.16f * stats.sizeX, 0.16f * stats.sizeY, 0.16f);

        for (int i = 0; i < Room.transform.childCount; i++) {
            Instantiate(Monster, Room.transform.GetChild(i).position, Quaternion.identity);
        }
    }

    private void SpawnChest(RoomStats stats, Transform Children)
    {
        Instantiate(Chest,
            stats.Middle,
            Quaternion.identity, Children);
    }

    private void SpawnChandelier(RoomStats stats, Transform Children)
    {
        Instantiate(GetChandelier(),
            stats.Middle,
            Quaternion.identity, Children);
    }

    public void GenRoomsChandelier(int i, int nb_rooms)
    {
        while (i > 0) {
            int room = Random.Range(1, nb_rooms);
            RoomStats stats = transform.GetChild(room).gameObject.GetComponent<RoomStats>();
            if (stats.idRoom == 0) {
                stats.idRoom = 1;
                i--;
            } 
        }
    }

    public void GenRoomsChest(int i, int nb_rooms)
    {
        while (i > 0) {
            int room = Random.Range(1, nb_rooms);
            RoomStats stats = transform.GetChild(room).gameObject.GetComponent<RoomStats>();
            if (stats.idRoom == 0) {
                stats.idRoom = 2;
                i--;
            } 
        }
    }

    public void GenerateMobInRooms(int nb_rooms)
    {
        RoomStats stats;
        Transform Children;
        
        GenRoomsChandelier(Random.Range(3, 6), nb_rooms);
        GenRoomsChest(Random.Range(1, 2), nb_rooms);
        for (int child = 1; child < nb_rooms; child++) {
            Children = transform.GetChild(child);
            stats = Children.gameObject.GetComponent<RoomStats>();
            if (stats.idRoom == 1){
                SpawnChandelier(stats, Children);
            } else if (stats.idRoom == 2) {
                SpawnChest(stats, Children);
            } else {
                SpawnMonster(stats, Children);
            }
        }
    }
}
