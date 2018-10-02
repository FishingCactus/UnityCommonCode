using UnityEngine;

[System.Serializable]
public class NodeElement : ElementBase
{
    // -- PUBLIC

    [SerializeField]
    public Vector2 Position;
    [SerializeField]
    public bool ItIsCompleted = false;

    public NodeElement() : base()
    {
    }

    public void Setup(
        string name_base,
        int index,
        Vector2 position
        )
    {
        Name = $"{name_base}_{index}";

        Position = position;
        Index = index;

        ItIsCompleted = false;
    }
}
