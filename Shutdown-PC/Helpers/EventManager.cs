namespace ShutdownPC.Helpers
{
    public static class EventManager
    {
        public static event Action OnTickEvent;

        public static void RaiseTickEvent()
        {
            OnTickEvent?.Invoke();
        }
    }
}
