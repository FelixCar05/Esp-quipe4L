using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class DungeonGen : MonoBehaviour
{
    [Header("Rooms")]
    public GameObject one;
    public GameObject two;
    public GameObject three;
    public GameObject four;
    public GameObject five;

    public int NbRoomsMin = 2;

    int GroupsOfEnnemies = 3;
    int NbEnnemies = 5;




    void Start()
    {
        PlaceRooms();//and lock doors

    }
    private void GenMap()
    {
        //not needed
    }

    private void PlaceRooms()
    {
        //room 1(start)
        GameObject start = Instantiate(two, Vector3.zero, Quaternion.identity);
        start.name = "0";
        //pastRoom.tag = pastRoom.name;
        System.Random rand = new();


        //fill with 2-3-4-5 door rooms
        List<GameObject> portesPossibles = new() { two, three, four };

        //GameObject pastRoom = null;
        Transform Door1 = null;//initialization
        GameObject newRoom = null;
        Vector3 bouger = Vector3.one;
        List<GameObject> Rooms = new(20);
        Rooms.Add(start);

        for (int i = 1; i < NbRoomsMin; i++)
        {
            Door1 = Rooms[i-1].GetComponent<Transform>().Find("porte1");//past room
           
            //choose random doors
            newRoom = Instantiate(three, Door1.position, Quaternion.identity);

            newRoom.transform.rotation = new Quaternion(0, Door1.rotation.y + 180, 0, 0);

            bouger = newRoom.GetComponent<Transform>().Find("porte2").position - Door1.position;

            newRoom.transform.position -= bouger;
            //correct possition with vector and choose random door

            Rooms.Add(newRoom);
            newRoom.name = i.ToString();
            //Debug.Log("Past Room: " + pastRoom.name);
            //fix with 1-2-3 door rooms(descending)






            //bossroom/upgrade
            //endroom

        }
    }
}
