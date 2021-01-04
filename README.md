### DiscordRPC-Unity

Drag and drop solution to Discord Rich Presence for Unity/C#. Tested on Unity 4.6 (yes, really). Uses the codebase from https://github.com/discord/discord-rpc/. 

## Instructions

# Step 0 - Registering with Discord

Create your application at Discord's Developer Portal here: https://discord.com/developers/applications/me.
Follow all the relevant steps until you can access the Rich Presence menu. Upload relevant art assets, and maybe play around with the visualizer.

# Step 1 - Files
Drag and drop these files into your assets folder (only the Windows plugins are included, add according to your needs). 
You can get them from cloning the repo, or from downloading the release. Make sure everything is dropped into the "Assets" folder.

# Step 2 - Modification
Change the default settings to values relevant to your own project in the file [DiscordConfig.cs]

You can take most of these from the visualizer on the Discord dev. portal, or you can manually fill them in. This is what, by default, will be used to populate the Rich Presence when your game is ran and Discord's rich presence is activated in some way. The **most important variable is the `application_id`** (`client_id`), this is what will link your application to the Discord backend. It can be found on the "General Information" page under your application in the Discord developer portal. It is called the `client_id` there.

# Step 3 - Testing

Login to Discord, run your game and make sure to stick `DiscordController.updatePresence()` or `DiscordController.getPresence()` in some GameObject that's actually part of your scene. Check your Discord profile and it should look something like this:

![alt text](https://i.imgur.com/0TkpMwM.png "Discord Rich Presence")

# Step 4 - Usage examples
Just basically call `DiscordController.updatePresence(<presence>)` where necessary. Updates are possible once per 15 seconds, but updating more often will just queue them up, as per Discord's library's implementation. Use `DiscordController.getCurrentPresence()` to retrieve the default/previous presence, then edit where necessary and feed back into `DiscordController.updatePresence()`.

Example:

```C#
DiscordRpc.RichPresence presence = DiscordController.getCurrentPresence();
presence.state = players[0].GetComponentInChildren<Vikavolt>().score.ToString() + " Pts. | HP: " + teamLives;
presence.details = "My Level";//level
DiscordController.updatePresence(presence);
```


# Other thoughts
If there's a better structure I could've used, feel free to let me know. If you get stuck, you can reach me on twitter at twitter.com/heatphoenix.

Good luck!
