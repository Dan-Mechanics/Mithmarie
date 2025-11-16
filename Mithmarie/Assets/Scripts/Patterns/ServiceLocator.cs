using System;

namespace Mithmarie
{
    public static class ServiceLocator<T>
    {
        private static T instance;

        public static T Locate()
        {
            if (!HasBeenProvided())
                throw new Exception("Cannot locate service because it has not been provided yet.");

            return instance;
        }

        public static bool HasBeenProvided() => instance != null;
        public static void Provide(T service) => instance = service;
    }
}