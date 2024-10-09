using UnityEngine;

[RequireComponent(typeof(LevelManager))]
public class Board : MonoBehaviour
{
    private LevelManager _levelManager;
 
    private void Awake()
    {
        _levelManager = GetComponent<LevelManager>();
        
        _levelManager.Configure();
    }

    private void Start()
    {
    }
    

    private void OnDestroy()
    {
    }

}                                       
