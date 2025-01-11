using System.Collections.Generic;

public static class ManagerRegistry
{
    private static Dictionary<System.Type, object> managers = new Dictionary<System.Type, object>();

    public static void Register<T>(T manager) where T : class
    {
        managers[typeof(T)] = manager;
    }

    public static T Get<T>() where T : class
    {
        if (managers.TryGetValue(typeof(T), out var manager))
        {
            return manager as T;
        }
        return null;
    }
}
