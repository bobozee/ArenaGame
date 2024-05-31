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
}