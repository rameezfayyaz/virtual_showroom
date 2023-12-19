// using System.Collections;
// using System.Collections.Generic;
// using System.Linq;
// using Microsoft.Unity.VisualStudio.Editor;
// using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ChangeWall : MonoBehaviour
{
    public GameObject[] Walls;
    public GameObject[] AlternateWalls;
    public Image[] buttonImages;
    public Sprite[] FirstTexture, SecondTexture;
    public Image[] ColorButtons;
    public GameObject[] ColorSelectBar;
    public Color SelectedColor;
    public Color DeselectedColor;
    private int ActiveWall = 0, ActiveColor = 0;
    private bool changingWall = false;
    private bool changingColor = false;
    public void WallSelected(int wallID) {
        
        if (changingWall) return;
        if (wallID == ActiveWall) return;
        changingWall = true;
        Invoke("EnableWallSelection", 0.75f);

        for (int i=0; i<Walls.Length; i++) {
            if (i == wallID) {
                // if (Walls[i].activeSelf) return;
                Walls[i].SetActive(true);
                buttonImages[i].color = SelectedColor;
                ActiveWall = i;
                ActiveColor = 0;
                UpdateColors();
            }
            else {
                Walls[i].SetActive(false);
                buttonImages[i].color = DeselectedColor;
            }
        }
        foreach (GameObject obj in AlternateWalls) {
            obj.SetActive(false);
        }
    }
    private void UpdateColors() {
        ColorButtons[0].sprite = FirstTexture[ActiveWall];
        ColorButtons[1].sprite = SecondTexture[ActiveWall];
    }
    private void EnableWallSelection() {
        changingWall = false;
    }
    private void EnableColorSelection() {
        changingColor = false;
    }
    public void ColorSelected(int colorIndex) {
        if (changingColor) return;
        if (colorIndex == ActiveColor) return;
        changingColor = true;
        Invoke("EnableColorSelection", 0.75f);
        switch (colorIndex) {
            case 0:
                Walls[ActiveWall].SetActive(true);
                AlternateWalls[ActiveWall].SetActive(false);
                ColorBar(true);
                ActiveColor = 0;
            break;
            case 1:
                Walls[ActiveWall].SetActive(false);
                AlternateWalls[ActiveWall].SetActive(true);
                ColorBar(false);
                ActiveColor = 1;
            break;
            default: break;
        }
    }
    private void ColorBar(bool first) {
        if (first) {
            ColorSelectBar[0].SetActive(true);
            ColorSelectBar[1].SetActive(false);
        } else {
            ColorSelectBar[1].SetActive(true);
            ColorSelectBar[0].SetActive(false);
        }
    }
}
