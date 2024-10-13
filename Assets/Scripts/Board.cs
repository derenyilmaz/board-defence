using UnityEngine;

[RequireComponent(typeof(LevelManager))]
public class Board : MonoBehaviour
{
    private LevelManager _levelManager;
 
    private void Start()
    {
        EventManager.OnLevelStarted += LevelStartedEventHandler;
    
        _levelManager = GetComponent<LevelManager>();
        
        _levelManager.Configure();
    }

    private void OnDestroy()
    {
        EventManager.OnLevelStarted -= LevelStartedEventHandler;
    }

    private void LevelStartedEventHandler(object sender, EventManager.LevelStartedEventArgs args)
    {
        // todo: magic numbers
        var scaleFactor = Mathf.Min(4f / args.LevelFormat.width, 7f / args.LevelFormat.height);
        transform.localScale *= scaleFactor;
        // transform.position = new Vector3(-(args.LevelFormat.width - 1) / 2f, -5f, 0);
    }

}                                       
