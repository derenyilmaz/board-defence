using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelLibrary", menuName = "ScriptableObjects/LevelLibrary", order = 1)]
public class LevelLibrary : ScriptableObject
{
    public List<LevelFormat> levelList;

    public LevelFormat GetLevelByIndex(int index)
    {
        if (index < 0)
        {
            return null;
        }

        return index >= levelList.Count ? levelList[^1] : levelList[index];
    }
}
