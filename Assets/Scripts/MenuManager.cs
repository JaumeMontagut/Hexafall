using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;

    [SerializeField] GameObject[] menus;

    private void Awake()
    {
        Instance = this;
    }

    public void OpenMenu (string menuName)
    {
        foreach (GameObject m in menus)
        {
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
}
