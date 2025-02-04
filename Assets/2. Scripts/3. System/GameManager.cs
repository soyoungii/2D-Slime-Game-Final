using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance => instance;
    internal List<Monster> monsters = new List<Monster>();   //씬에 존재하는 전체 적 List
    internal Slime slime;  //씬에 존재하는 슬라임 객체

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            DestroyImmediate(this);
            return;
        }
        DontDestroyOnLoad(gameObject);

        slime = FindObjectOfType<Slime>();
    }

    private void Start()
    {
        Screen.SetResolution(1920, 1080, true);
    }
}
