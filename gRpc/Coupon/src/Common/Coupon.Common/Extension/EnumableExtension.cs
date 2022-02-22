namespace System.Collections.Generic
{
    public static class EnumableExtension
    {
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> list)
        {
            return list == null || !list.Any();
        }

        public static bool IsNotNullOrEmpty<T>(this IEnumerable<T> list)
        {
            return list != null && list.Any();
        }

    }
}
