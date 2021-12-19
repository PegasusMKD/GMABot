namespace GMABot.Models.WebSocket
{

    public enum DiscordEventType
    {
        // Will always activate on start-up
        READY,
        GUILD_MEMBER_UPDATE,

        // Implemented
        INTERACTION_CREATE, 

        // Not implemented (no functionality currently correlates with these events)
        RESUMED,
        VOICE_SERVER_UPDATE,
        USER_UPDATE,
        APPLICATION_COMMAND_CREATE,
        APPLICATION_COMMAND_UPDATE,
        APPLICATION_COMMAND_DELETE,
        GUILD_CREATE,
        GUILD_DELETE,
        GUILD_ROLE_CREATE,
        GUILD_ROLE_UPDATE,
        GUILD_ROLE_DELETE,
        THREAD_CREATE,
        THREAD_UPDATE,
        THREAD_DELETE,
        THREAD_LIST_SYNC,
        THREAD_MEMBER_UPDATE,
        THREAD_MEMBERS_UPDATE,
        CHANNEL_CREATE,
        CHANNEL_UPDATE,
        CHANNEL_DELETE,
        CHANNEL_PINS_UPDATE,
        MESSAGE_CREATE,
        MESSAGE_UPDATE,
        MESSAGE_DELETE,
        MESSAGE_DELETE_BULK,
        HEARTBEAT
    }
}
