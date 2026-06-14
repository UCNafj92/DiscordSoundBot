using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using EventBus.InMemory;
using FileReader.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WowCombatLog;

namespace LocalDiscordApp.Service
{
    /// <summary>
    /// Handles the core functionality of the Discord bot, including connecting to the Discord gateway, 
    /// handling interactions, and managing scheduled tasks like daily birthday checks.
    /// </summary>
    public class BotService
    {
        private DiscordSocketClient _client;
        private InteractionService _interactionService;
        private IServiceProvider _services;
        private string _token;
        private ulong _guildId;
        private bool _startedCombatLogMonitor;


        /// <summary>
        /// Singleton instance of <see cref="BotService"/> to ensure a single running instance.
        /// </summary>
        private static readonly Lazy<BotService> lazy = new Lazy<BotService>(() => new BotService());

        /// <summary>
        /// Gets the singleton instance of the bot service.
        /// </summary>
        public static BotService Instance
        {
            get
            {
                return lazy.Value;
            }
        }

        /// <summary>
        /// Private constructor to enforce singleton pattern.
        /// </summary>
        private BotService()
        {
        }

        /// <summary>
        /// Initializes and starts the bot.
        /// </summary>
        /// <param name="token">The bot token.</param>
        /// <param name="guildId">The ID of the guild (server) where the bot will operate.</param>
        /// <param name="ownerIds">Array of IDs for bot owners.</param>
        public async Task RunBotAsync(string token, ulong guildId)
        {
            _token = token;
            _guildId = guildId;

            // ✅ Configure DiscordSocketClient with UseInteractionSnowflakeDate = false
            var config = new DiscordSocketConfig
            {
                EnableVoiceDaveEncryption = true,
                LogLevel = LogSeverity.Info,
                UseInteractionSnowflakeDate = false, // 🔹 Prevents timestamp issues causing timeouts
                GatewayIntents = GatewayIntents.Guilds | GatewayIntents.GuildMessages | GatewayIntents.GuildMembers | GatewayIntents.GuildVoiceStates
            };

            _client = new DiscordSocketClient(config);
            _interactionService = new InteractionService(_client);

            // Build configuration once
            var cfg = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            _services = new ServiceCollection()
            .AddSingleton(_client)
            .AddSingleton(_interactionService)
            .AddSingleton<IConfiguration>(cfg)
            .AddEventBus()
            .AddFileReaderWindows()
            .AddWowCombatLog(cfg)
            .AddLocalDiscordApp(cfg)
            .BuildServiceProvider();

            _services.UseWowCombatLog();
            _services.UseLocalDiscordApp();


            _client.Log += Log;
            _client.Ready += OnReady;

            _client.InteractionCreated += async (interaction) =>
            {
                Console.WriteLine($"🔹 Received interaction of type: {interaction.Type}");

                var ctx = new SocketInteractionContext(_client, interaction);
                var result = await _interactionService.ExecuteCommandAsync(ctx, _services);

                if (!result.IsSuccess)
                {
                    Console.WriteLine($"❌ Command execution failed: {result.ErrorReason}");
                }
                else
                {
                    Console.WriteLine("✅ Command executed successfully!");
                }
            };

            await _client.LoginAsync(TokenType.Bot, _token);
            await _client.StartAsync();

            await Task.Delay(-1);
        }

        /// <summary>
        /// Logs messages from the Discord client to the console.
        /// </summary>
        /// <param name="msg">The log message.</param>
        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg);
            return Task.CompletedTask;
        }


        /// <summary>
        /// Called when the bot connects to the Discord gateway.
        /// Loads interaction modules and registers commands.
        /// </summary>
        private async Task OnReady()
        {
            Console.WriteLine($"{_client.CurrentUser} is online and connected!");

            Console.WriteLine("🔹 Loading interaction modules...");
            await _interactionService.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
            Console.WriteLine($"🔹 Loaded {_interactionService.Modules.Count} modules.");

            await _interactionService.RegisterCommandsToGuildAsync(_guildId);
            Console.WriteLine("✅ Slash commands registered!");

            if (!_startedCombatLogMonitor)
            {
                _startedCombatLogMonitor = true;

                var combatLogMonitor =
                    _services.GetRequiredService<CombatLogMonitorService>();

                _ = Task.Run(async () =>
                {
                    try
                    {
                        await combatLogMonitor.StartAsync();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Combat log monitor crashed: {ex}");
                    }
                });
            }
        }
    }
}
