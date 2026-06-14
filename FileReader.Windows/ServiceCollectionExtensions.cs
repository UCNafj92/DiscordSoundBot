using FileReader.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace FileReader.Windows;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddFileReaderWindows(
        this IServiceCollection services)
    {
        services.AddSingleton<IFileWatcher, FileWatcher>();
        return services;
    }
}