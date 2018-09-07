
[System.Serializable]
public class Faction
{

    public enum Type
    {
        Player,
        Enemy
    }

    public Type value;

    public Faction(Type value)
    {
        this.value = value;
    }
}
