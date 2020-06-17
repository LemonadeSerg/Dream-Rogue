using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class TitleMenu : MonoBehaviour
{
    public GameObject[] Menus;
    public InputField worldNameIn;
    public InputField seedIn;
    public int roomMaker;
    public WorldGenerationManager worldGenManager;
    public DateTime worldGenStartTime;

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void selectMenu(int menuID)
    {
        for (int i = 0; i < Menus.Length; i++)
        {
            if (menuID == i)
            {
                Menus[i].SetActive(true);
            }
            else
            {
                Menus[i].SetActive(false);
            }
        }
    }

    public void Play_Button_Click()
    {
        selectMenu(1);
    }

    public void Settings_Button_Click()
    {
        selectMenu(2);
    }

    public void Exit_Button_Click()
    {
        Application.Quit();
    }

    public void Back_Button_Click()
    {
        selectMenu(0);
    }

    public void worldNameTyping(string text)
    {
        print(text);
    }

    public void RoomMaker()
    {
        SceneManager.LoadScene(1);
    }

    public void genNewWorld()
    {
        worldGenStartTime = System.DateTime.Now;
        worldGenManager.gen(int.Parse(seedIn.text), 4);
        print("Time taken to generate world :" + (worldGenStartTime - System.DateTime.Now).Seconds.ToString() + "." + (System.DateTime.Now - worldGenStartTime).Milliseconds.ToString() + " Seconds");
    }

    public void loadLevel()
    {
        ScenePersistantData.worldName = seedIn.text;
        SceneManager.LoadScene(2);
    }
}