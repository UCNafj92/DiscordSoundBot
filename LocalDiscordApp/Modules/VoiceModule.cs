using Discord.Interactions;
using LocalDiscordApp.Service;
using Microsoft.Extensions.Configuration;

namespace LocalDiscordApp.Modules;

public class VoiceModule : InteractionModuleBase<SocketInteractionContext>
{
    private readonly IDiscordVoiceService _voiceService;
    private readonly IConfiguration _configuration;

    public VoiceModule(
        IDiscordVoiceService voiceService,
        IConfiguration configuration)
    {
        _voiceService = voiceService;
        _configuration = configuration;
    }

    [SlashCommand("joinvoice", "Join the configured voice channel", runMode: RunMode.Async)]
    public async Task JoinVoiceAsync()
    {
        ulong guildId =
            ulong.Parse(_configuration["Discord:GuildId"]!);

        ulong voiceChannelId =
            ulong.Parse(_configuration["Discord:VoiceChannelId"]!);

        await _voiceService.JoinAsync(
            guildId,
            voiceChannelId);

        await RespondAsync(
            $"Joined voice channel {voiceChannelId}");
    }
}