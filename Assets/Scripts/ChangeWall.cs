// using System.Collections;
// using System.Collections.Generic;
// using System.Linq;
using UnityEngine;

public class ChangeWall : MonoBehaviour
{
    public GameObject[] Walls;
    public Material[] WallMaterials;
    private int ActiveWall = 0;
    public void WallSelected(int wallID) {
        for (int i=0; i<Walls.Length; i++) {
            if (i == wallID) {
                Walls[i].SetActive(true);
                ActiveWall = i;
            }
            else Walls[i].SetActive(false);
        }
    }
    public void MaterialSelected(int materialID) {
        Walls[ActiveWall].GetComponent<Transform>().GetChild(0)
        .GetComponent<MeshRenderer>().material = WallMaterials[materialID];
    }
}
