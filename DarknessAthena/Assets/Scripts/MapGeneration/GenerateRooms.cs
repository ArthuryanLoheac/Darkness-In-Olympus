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

    public GameObject[] LstLittleRooms;
    public GameObject[] LstBigRooms;

    public GameObject Spikes;
    public GameObject ArrowThower;

    public RoomGenerator RG;

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

    private GameObject ChooseKeepMonster(GameObject Monster, int pourcent)
    {
        int nb = Random.Range(0, 100);
        if (nb <= pourcent)
            return GetMonster();
        return Monster;
    }

    private GameObject GetRoom(RoomStats stats)
    {
        if (stats.sizeX < 8 || stats.sizeY < 8) 
            return LstLittleRooms[Random.Range(0, LstLittleRooms.Length)];
        return LstBigRooms[Random.Range(0, LstBigRooms.Length)];
    }

    private Vector3 PosToGRid(Vector3 pos, float y = 0f, float x = 0f)
    {
        return new Vector3(Mathf.FloorToInt(pos.x / 0.16f) * 0.16f + x,
                           Mathf.FloorToInt(pos.y / 0.16f) * 0.16f + y,
                           pos.z);
    }

    private void SpawnMonster(RoomStats stats, Transform Children)
    {
        GameObject RoomSpawn = GetRoom(stats);
        GameObject Monster = GetMonster();
        Vector3 Pos = stats.transform.position;
        GameObject Room = Instantiate(RoomSpawn,
            new Vector3 (Pos.x + ((stats.sizeX/2) - 0.5f) * 0.16f, Pos.y + ((stats.sizeY/2) - 0.5f) * 0.16f, 0),
            Quaternion.identity, Children);
        Room.transform.localScale = new Vector3(0.16f * stats.sizeX, 0.16f * stats.sizeY, 0.16f);

        for (int i = 0; i < Room.transform.childCount; i++) {
            switch (Room.transform.GetChild(i).gameObject.tag) {
                case "PosMob":
                    Instantiate(ChooseKeepMonster(Monster, 33), Room.transform.GetChild(i).position, Quaternion.identity);
                    break;
                case "PosSpikes":
                    Instantiate(Spikes, PosToGRid(Room.transform.GetChild(i).position, -0.001f), Quaternion.identity);
                    break;
                default:
                    break;
            }
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

    public void MakeSpikesCouloir(GameObject coul) {
        for (int i = 0; i < coul.transform.childCount; i++) {
            if (coul.transform.GetChild(i).tag == "Ground" && Random.Range(0, 20) == 1) {
                Instantiate(Spikes, PosToGRid(coul.transform.GetChild(i).position, -0.001f), Quaternion.identity);
            }
        }
    }

    public void GenSpikesCouloirs()
    {
        GameObject[] LstCouloirs = GameObject.FindGameObjectsWithTag("Couloir");

        foreach (GameObject Coul in LstCouloirs) {
            if (Random.Range(0, 100) < 10) {
                MakeSpikesCouloir(Coul);
            }
        }
    }

    public void GenRooms(int nb_rooms, RoomGenerator RoomGen)
    {
        RoomStats stats;
        Transform Children;
        RG = RoomGen;
        
        GenRoomsChandelier(Random.Range(1 + (nb_rooms/10), nb_rooms/5), nb_rooms);
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
        GenSpikesCouloirs();
    }
}
