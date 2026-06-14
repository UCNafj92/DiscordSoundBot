using Discord.Interactions;
using EventBus.Abstractions;
using WowCombatLog.Events;

namespace LocalDiscordApp.Modules;

public class TestEventModule : InteractionModuleBase<SocketInteractionContext>
{
    private readonly IEventBus _eventBus;

    public TestEventModule(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    [SlashCommand("test-death", "Publish a fake player death event", runMode: RunMode.Async)]
    public async Task TestDeathAsync()
    {
        await DeferAsync();

        await _eventBus.PublishAsync(
            new WowDeathEvent(
                "Testplayer-Realm",
                "MANUAL_TEST",
                DateTimeOffset.UtcNow));

        await FollowupAsync("Published test death event.");
    }

    [SlashCommand("test-resurrect", "Publish a fake resurrect event", runMode: RunMode.Async)]
    public async Task TestResurrectAsync()
    {
        await DeferAsync();

        await _eventBus.PublishAsync(
            new WowResurrectEvent(
                "Islandpawn-Realm",
                "Shizdy-Realm",
                "Rebirth",
                "MANUAL_TEST",
                DateTimeOffset.UtcNow));

        await FollowupAsync("Published test resurrect event.");
    }

    [SlashCommand("test-wipe", "Publish a fake wipe event", runMode: RunMode.Async)]
    public async Task TestWipeAsync()
    {
        await DeferAsync();

        await _eventBus.PublishAsync(
            new WowWipeEvent(
                new List<string>
                {
                    "PlayerOne",
                    "PlayerTwo",
                    "PlayerThree",
                    "PlayerFour",
                    "PlayerFive"
                },
                DateTimeOffset.UtcNow));

        await FollowupAsync("Published test wipe event.");
    }
}