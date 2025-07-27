using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlacedUIImages", menuName = "Tools/Placed UI Images Data", order = 1)]
public class PlacedUIImageData : ScriptableObject
{
    [System.Serializable]
    public class Entry
    {
        public Vector2 position;
        public Vector2 size;
    }

    public List<Entry> placedEntries = new List<Entry>();

    public void Add(Vector2 pos, Vector2 size)
    {
        placedEntries.Add(new Entry { position = pos, size = size });
    }

    public void Clear()
    {
        placedEntries.Clear();
    }
}