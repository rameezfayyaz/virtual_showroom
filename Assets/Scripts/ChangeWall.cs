// using System.Collections;
// using System.Collections.Generic;
// using System.Linq;
// using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;

public class ChangeWall : MonoBehaviour
{
    public GameObject[] Walls;
    public Image[] buttonImages;
    public Color SelectedColor;
    public Color DeselectedColor;
    public Material[] WallMaterials;
    private int ActiveWall = 0;
    public void WallSelected(int wallID) {
        for (int i=0; i<Walls.Length; i++) {
            if (i == wallID) {
                Walls[i].SetActive(true);
                buttonImages[i].color = SelectedColor;
                ActiveWall = i;
            }
            else {
                Walls[i].SetActive(false);
                buttonImages[i].color = DeselectedColor;
            }
        }
    }
    public void MaterialSelected(int materialID) {
        Walls[ActiveWall].GetComponent<Transform>().GetChild(0)
        .GetComponent<MeshRenderer>().material = WallMaterials[materialID];
    }
}
