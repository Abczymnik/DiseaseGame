public sealed class GWorld
{
    private static readonly object instanceLock = new object();
    public WorldStates World { get; private set; }

    private static GWorld _instance;
    public static GWorld Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (instanceLock)
                {
                    if (_instance == null) _instance = new GWorld();
                }
            }
            return _instance;
        }
    }

    private GWorld()
    {
        World = new WorldStates();
    }
}
