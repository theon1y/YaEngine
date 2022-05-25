using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using YaEcs;
using YaEcs.MicrosoftDependencyInjectionExtensions;
using YaEngine;
using YaEngine.Bootstrap;
using YaEngine.ImGui;

var configurationBuilder = new ConfigurationBuilder();
configurationBuilder.AddInMemoryCollection(new Dictionary<string, string>()
{
    { "WindowConfig:Width", "1280" },
    { "WindowConfig:Height", "720" },
    { "WindowConfig:Title", "YaEngine" },
});
var configuration = configurationBuilder.Build();
var services = new ServiceCollection();

services
    .AddSilk(configuration)
    .AddEcs(configuration)
    .AddDefaultSystems()
    .AddOpenGl()
    .AddOpenAl()
    .AddScoped<IInitializeSystem, BuildSceneSystem>()
    .AddScoped<IUpdateSystem, EnableEffectsSystem>()
    .AddScoped<IUpdateSystem, EnableMusicSystem>()
    .AddScoped<IUpdateSystem, SwitchAnimationsSystem>()
    .AddScoped<IUpdateSystem, MoveCameraSystem>()
    .AddScoped<IUpdateSystem, QuitSystem>()
    .AddScoped<IImGuiSystem, ShowTransformsGuiSystem>()
    .AddScoped<IUpdateSystem, MoveLightSystem>();
    
var serviceProvider = services.BuildServiceProvider();
var bootstrapper = serviceProvider.GetService<SilkBootstrapper>();
bootstrapper.Run();