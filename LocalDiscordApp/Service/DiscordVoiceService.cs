using Discord;
using Discord.Audio;
using Discord.WebSocket;
using LocalDiscordApp.Sounds;
using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace LocalDiscordApp.Service;

public class DiscordVoiceService : IDiscordVoiceService
{
    private readonly DiscordSocketClient _client;
    private readonly SoundLibrary _soundLibrary;
    private readonly AudioOptions _audioOptions;
    private readonly SemaphoreSlim _playLock = new(1, 1);

    private IAudioClient? _audioClient;
    private AudioOutStream? _discordStream;

    public DiscordVoiceService(
        DiscordSocketClient client,
        SoundLibrary soundLibrary,
        IOptions<AudioOptions> audioOptions)
    {
        _client = client;
        _soundLibrary = soundLibrary;
        _audioOptions = audioOptions.Value;
    }

    public bool IsConnected => _discordStream is not null;

    public async Task JoinAsync(ulong guildId, ulong voiceChannelId)
    {
        var guild = _client.GetGuild(guildId)
            ?? throw new InvalidOperationException($"Guild {guildId} was not found.");

        var channel = guild.GetVoiceChannel(voiceChannelId)
            ?? throw new InvalidOperationException($"Voice channel {voiceChannelId} was not found.");

        _audioClient = await channel.ConnectAsync();
        _discordStream = _audioClient.CreatePCMStream(AudioApplication.Mixed);
    }

    public async Task PlaySoundAsync(
        string soundKey,
        CancellationToken cancellationToken = default)
    {
        if (_audioClient is null || _discordStream is null)
            throw new InvalidOperationException("Bot is not connected to a voice channel.");

        var filePath = _soundLibrary.GetSoundPath(soundKey);

        if (!File.Exists(filePath))
            throw new FileNotFoundException($"Sound file not found: {filePath}");

        await _playLock.WaitAsync(cancellationToken);

        try
        {
            using var ffmpeg = Process.Start(new ProcessStartInfo
            {
                FileName = _audioOptions.FfmpegPath,
                Arguments = $"-hide_banner -loglevel error -i \"{filePath}\" -ac 2 -f s16le -ar 48000 pipe:1",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            });

            if (ffmpeg is null)
                throw new InvalidOperationException("Could not start FFmpeg.");

            await ffmpeg.StandardOutput.BaseStream.CopyToAsync(
                _discordStream,
                cancellationToken);

            await _discordStream.FlushAsync(cancellationToken);

            await ffmpeg.WaitForExitAsync(cancellationToken);

            var error = await ffmpeg.StandardError.ReadToEndAsync();

            if (ffmpeg.ExitCode != 0)
                Console.WriteLine($"FFmpeg failed with exit code {ffmpeg.ExitCode}: {error}");
        }
        finally
        {
            _playLock.Release();
        }
    }

    public async Task DisconnectAsync()
    {
        if (_discordStream is not null)
        {
            await _discordStream.DisposeAsync();
            _discordStream = null;
        }

        if (_audioClient is not null)
        {
            await _audioClient.StopAsync();
            _audioClient = null;
        }
    }
}