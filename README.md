# CombatCaster

CombatCaster is a Discord bot that monitors World of Warcraft combat logs and reacts to combat events such as player deaths, combat resurrections, and raid wipes by playing configurable sound effects in Discord voice channels.

The project uses a modular event-driven architecture with reusable file watching, event bus, and combat log parsing components.

## Quick setup

1. Clone the repository.
2. Copy `appsettings.example.json` to `appsettings.json`.
3. Fill in your Discord bot token, guild ID, voice channel ID, combat log path, FFmpeg path, and sound folder.
4. Add your sound files to the configured sound folder.
5. Make sure these native libraries are available in the output folder:
   - `libdave.dll`
   - `opus.dll`
   - `libsodium.dll`
6. Start World of Warcraft and enable combat logging.
7. Start the bot.
8. Use the join voice command, then test with the provided test commands.

## Expanding functionality

To add support for a new combat log event:

1. Create a new event type, for example `WowInterruptEvent`.
2. Create a parser implementing `ICombatLogEventParser`.
3. Register the parser in the `WowCombatLog` DI setup.
4. Create a Discord-side event handler.
5. Add a sound mapping in `appsettings.json`.

Example sound mapping:

```
"Sounds": {
  "FolderPath": "C:\\Sounds",
  "Mappings": {
    "death": "death.mp3",
    "resurrect": "resurrect.mp3",
    "wipe": "wipe.mp3"
  }
}
```
The general flow is:
```
Combat log line
→ FileWatcher
→ LineReadFromFileEvent
→ CombatLog parser
→ Wow event
→ Discord handler
→ Sound playback
```

## Combat Log Notes

CombatCaster automatically searches for the most recent WoW combat log file when it starts.

For reliable operation:

1. Start World of Warcraft first.
2. Ensure combat logging is enabled.
3. Start CombatCaster afterwards.
