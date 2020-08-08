using UnityEngine;

public class GameMaster : Singelton<GameMaster>
{
    public Transform Managers
    {
        get;
        private set;
    }

    //public Transform UIManager { get; private set; }
    //public Transform Terrain { get; private set; }

    private void Awake()
    {
        Initialize();   
    }

    private void Initialize()
    {
        Managers = transform.Find("Managers");

        //UIManager = transform.Find("UIManager");
        //Terrain = transform.Find("Terrain");

        Managers.gameObject.SetActive(true);
    }
}
