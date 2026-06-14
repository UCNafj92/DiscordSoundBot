using Discord;
using Discord.Interactions;
using Microsoft.Extensions.Configuration;

public class SlashCommands : InteractionModuleBase<SocketInteractionContext>
{
    
    private readonly IConfiguration _config;


    public SlashCommands(IConfiguration config)
    {
        _config = config;
    }

    [SlashCommand("ping", "Check if the bot is responsive.")]
    public async Task PingCommand()
    {
        Console.WriteLine("🔹 Ping command executed.");

        try
        {
            await RespondAsync("🏓 Pong!", ephemeral: true);
            Console.WriteLine("✅ RespondAsync() completed successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ RespondAsync() failed: {ex}");
        }
    }

}

