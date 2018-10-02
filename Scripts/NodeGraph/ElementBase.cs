using UnityEngine;

[System.Serializable]
public class ElementBase
{
    // -- PUBLIC

    public string Name
    {
        get { return _Name; }
        protected set { _Name = value; }
    }

    public int Index
    {
        get { return _Index; }
        protected set { _Index = value; }
    }

    public ElementBase()
    {
    }

    // -- PROTECTED

    [SerializeField]
    protected string _Name;
    [SerializeField]
    protected int _Index;
}
