using UnityEngine;

public class CheckPointManager : MonoBehaviour
{
    public static CheckPointManager Instance;


    [Header("CheckPoints")]
    public Transform[] CheckPointPositions;

    [Header("Enemies")]
    [SerializeField] GameObject[] CemiteryEnemies;
    [SerializeField] GameObject[] CastleEnemies;
    [SerializeField] GameObject[] SewageEnemies;
    [SerializeField] GameObject[] WachtowerEnemies;
    [SerializeField] GameObject[] CavernEnemies;
    [SerializeField] GameObject[] DungeonEnemies;

    [Header("Sky")]
    [SerializeField] GameObject[] Sky;



    public int Area;

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Area = GameController.controller.CurrentCheckpoint;
        if (Area == 5)
        {
            for (int i = 0; i < Sky.Length; i++)
            {
                Sky[i].SetActive(true);
            }
        }
        if (Area == 2)
        {
            for (int i = 0; i < Sky.Length; i++)
            {
                Sky[i].SetActive(false);
            }
        }
    }
    public void ResetArea(int area)
    {
        

        if (area == 0)
            ResetEnemies(CemiteryEnemies);


        if (area == 1 || area == 2)
            ResetEnemies(CastleEnemies);


        if (area == 3 || area == 4)
            ResetEnemies(SewageEnemies);


        if (area == 5 || area == 6 || area == 7)
            ResetEnemies(WachtowerEnemies);


        if (area == 8 || area == 9 || area == 11)
            ResetEnemies(CavernEnemies);


        if (area == 10)
            ResetEnemies(DungeonEnemies);


    }

    public void ResetEnemies(GameObject[] enemies)
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].SetActive(true);
        }
    }

}
