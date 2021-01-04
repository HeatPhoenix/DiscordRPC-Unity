using UnityEngine;

[System.Serializable]
public class DiscordJoinEvent : UnityEngine.Events.UnityEvent<string> { }

[System.Serializable]
public class DiscordSpectateEvent : UnityEngine.Events.UnityEvent<string> { }

[System.Serializable]
public class DiscordJoinRequestEvent : UnityEngine.Events.UnityEvent<DiscordRpc.DiscordUser> { }

public class DiscordController : MonoBehaviour
{
    public DiscordRpc.RichPresence presence = new DiscordRpc.RichPresence();
    public string applicationId = "702225413794955445";
    public string optionalSteamId;
    public int clickCounter;
    public DiscordRpc.DiscordUser joinRequest;
    public UnityEngine.Events.UnityEvent onConnect = new UnityEngine.Events.UnityEvent();
    public UnityEngine.Events.UnityEvent onDisconnect = new UnityEngine.Events.UnityEvent();
    public UnityEngine.Events.UnityEvent hasResponded = new UnityEngine.Events.UnityEvent();
    public DiscordJoinEvent onJoin;
    public DiscordJoinEvent onSpectate;
    public DiscordJoinRequestEvent onJoinRequest;

    DiscordRpc.EventHandlers handlers;

    public void OnClick()
    {
        Debug.Log("Discord: on click!");
        clickCounter++;

        presence.state = "1000 Points";
        presence.details = "Vs. Magnezone";
        //presence.endTimestamp = 1507665886;
        presence.largeImageKey = "icon";
        presence.largeImageText = "Vikavoltius,";
        presence.smallImageText = "Let's go!";

        DiscordRpc.UpdatePresence(presence);
    }

    public void RequestRespondYes()
    {
        Debug.Log("Discord: responding yes to Ask to Join request");
        DiscordRpc.Respond(joinRequest.userId, DiscordRpc.Reply.Yes);
        hasResponded.Invoke();
    }

    public void RequestRespondNo()
    {
        Debug.Log("Discord: responding no to Ask to Join request");
        DiscordRpc.Respond(joinRequest.userId, DiscordRpc.Reply.No);
        hasResponded.Invoke();
    }

    public void ReadyCallback(ref DiscordRpc.DiscordUser connectedUser)
    {
       Debug.Log(string.Format("Discord: connected to {0}#{1}: {2}", connectedUser.username, connectedUser.discriminator, connectedUser.userId));
        onConnect.Invoke();
    }

    public void DisconnectedCallback(int errorCode, string message)
    {
        Debug.Log(string.Format("Discord: disconnect {0}: {1}", errorCode, message));
        onDisconnect.Invoke();
    }

    public void ErrorCallback(int errorCode, string message)
    {
        Debug.Log(string.Format("Discord: error {0}: {1}", errorCode, message));
    }

    public void JoinCallback(string secret)
    {
        Debug.Log(string.Format("Discord: join ({0})", secret));
        onJoin.Invoke(secret);
    }

    public void SpectateCallback(string secret)
    {
        Debug.Log(string.Format("Discord: spectate ({0})", secret));
        onSpectate.Invoke(secret);
    }

    public void RequestCallback(ref DiscordRpc.DiscordUser request)
    {
        Debug.Log(string.Format("Discord: join request {0}#{1}: {2}", request.username, request.discriminator, request.userId));
        joinRequest = request;
        onJoinRequest.Invoke(request);
    }

    void Start()
    {
    }

    void Update()
    {
        DiscordRpc.RunCallbacks();
    }

    void OnEnable()
    {
        Debug.Log("Discord: init");
        handlers = new DiscordRpc.EventHandlers();
        handlers.readyCallback += ReadyCallback;
        handlers.disconnectedCallback += DisconnectedCallback;
        handlers.errorCallback += ErrorCallback;
        handlers.joinCallback += JoinCallback;
        handlers.spectateCallback += SpectateCallback;
        handlers.requestCallback += RequestCallback;        
        DiscordRpc.Initialize(applicationId, ref handlers, true, optionalSteamId);
    }

    void discordInit()
    {
        OnEnable();
    }

    void OnDisable()
    {
        Debug.Log("Discord: shutdown");
        DiscordRpc.Shutdown();
    }

    void OnDestroy()
    {

    }



    static bool initialized = false;
    static GameObject controller = null;
    static DiscordRpc.RichPresence currentPresence = null;

    private static void Initialize(DiscordRpc.RichPresence newPresence)
    {
        initialized = true;
        controller = new GameObject();
        controller.AddComponent<DiscordController>();
        updatePresence(newPresence);
    }

    public static DiscordRpc.RichPresence getCurrentPresence()
    {
        if (initialized)
            return currentPresence;
        else
        {
            //your default presence, basically.
            DiscordRpc.RichPresence presence = new DiscordRpc.RichPresence();
            presence.state = DiscordConfig.state;
            presence.details = DiscordConfig.details;

            System.DateTime epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
            int cur_time = (int)(System.DateTime.UtcNow - epochStart).TotalSeconds;

            presence.startTimestamp = cur_time;
            presence.largeImageKey = DiscordConfig.largeImageKey;
            presence.largeImageText = DiscordConfig.largeImageText;
            presence.smallImageText = DiscordConfig.smallImageText;
            return presence;
        }
    }

    public static void updatePresence(DiscordRpc.RichPresence newPresence)
    {
        if(!initialized || controller == null)
        {
            Initialize(newPresence);
            return;
        }

        Debug.Log("Discord: update presence (Z)");
        controller.GetComponentInChildren<DiscordController>().presence = newPresence;
        currentPresence = newPresence;
        DiscordRpc.UpdatePresence(newPresence);
    }
}