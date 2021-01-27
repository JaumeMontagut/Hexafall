using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;

    [SerializeField] GameObject[] menus;

    public Material MainMenuskybox;
    private void Awake()
    {
        Instance = this;
        RenderSettings.skybox = MainMenuskybox;
    }

    public void OpenMenu(string menuName)
    {
        foreach (GameObject m in menus)
        {
            Debug.Log(m.name);
            m.SetActive(false);
        }

        GameObject menu = Array.Find(menus, m => m.name == menuName);
        if (menu == null)
        {
            Debug.LogWarning("Menu with name " + menuName + " not found on the menu array");
        }
        else
        {
            menu.SetActive(true);
        }
    }

    public void Quit()
    {
        Application.Quit();
    }

    public bool AllCanvasDesactivated()
    {
        bool ret = true;

        foreach (GameObject m in menus)
        {
            if(m.activeSelf)
            {
                ret = false;
                return ret;
            }
        }

        return ret;

    }
}