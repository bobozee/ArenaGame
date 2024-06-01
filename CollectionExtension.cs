using ArenaGame;

public static class CollectionExtension
{
    public static T RandomElement<T>(this IList<T> list)
    {
        return list[Random.Shared.Next(list.Count)];
    }

    public static T RandomElement<T>(this T[] array)
    {
        return array[Random.Shared.Next(array.Length)];
    }

    public static void Shuffle<T>(this IList<T> list)  
    {  
        int n = list.Count;  
        while (n > 1) {  
            n--;  
            int k = Random.Shared.Next(n + 1);  
            T value = list[k];  
            list[k] = list[n];  
            list[n] = value;  
        }  
    }

    public static void Shuffle<T>(this T[] list)  
    {  
        int n = list.Length;  
        while (n > 1) {  
            n--;  
            int k = Random.Shared.Next(n + 1);  
            T value = list[k];  
            list[k] = list[n];  
            list[n] = value;  
        }  
    }
    
    public static void addOnto(this List<MovedAttack> list, IAttackable target, MoveValue[] newVals) {
        MoveValueCollection combined = new MoveValueCollection(list.get(target).getMoveValues(), newVals);
        list[list.FindIndex(x => x.getTarget() == target)] = new MovedAttack(list.get(target), combined);
    }

    public static MovedAttack get(this List<MovedAttack> list, IAttackable target) {
        return list[list.FindIndex(x => x.getTarget() == target)];
    }
}