using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelLibrary", menuName = "ScriptableObjects/LevelLibrary", order = 1)]
public class LevelLibrary : ScriptableObject
{
    public List<LevelFormat> levelList;

    public LevelFormat GetLevelByIndex(int index)
    {
        if (index < 0 || index >= levelList.Count)
        {
            return null;
        }

        return levelList[index];
    }
}
