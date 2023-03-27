#region assembly Fusion.Runtime, Version=1.1.5.0, Culture=neutral, PublicKeyToken=null
// C:\Users\Dré\Desktop\Project-Dion_Teste\Assets\Photon\Fusion\Assemblies\Fusion.Runtime.dll
#endregion

namespace Fusion
{
    //
    // Resumo:
    //     Fusion Game Mode. Used to select how the local simulation will act.
    public enum GameMode
    {
        //
        // Resumo:
        //     Single Player Mode: it works very similar to Fusion.GameMode.Host Mode, but don't
        //     accept any connections.
        Single = 1,
        //
        // Resumo:
        //     Shared Mode: starts a Game Client, which will connect to a Game Server running
        //     in the Photon Cloud using the Fusion Plugin.
        Shared = 2,
        //
        // Resumo:
        //     Server Mode: starts a Dedicated Game Server with no local player.
        Server = 3,
        //
        // Resumo:
        //     Host Mode: starts a Game Server and allows a local player.
        Host = 4,
        //
        // Resumo:
        //     Client Mode: starts a Game Client, which will connect to a peer in either Fusion.GameMode.Server
        //     or Fusion.GameMode.Host Modes.
        Client = 5,
        //
        // Resumo:
        //     Automatically start as Host or Client. The first peer to connect to a room will
        //     be started as a Host, all others will connect as clients.
        AutoHostOrClient = 6
    }
}
