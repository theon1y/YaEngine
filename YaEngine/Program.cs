using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using YaEcs;
using YaEcs.MicrosoftDependencyInjectionExtensions;
using YaEngine;
using YaEngine.Bootstrap;
using YaEngine.Bootstrap.OpenGL;
using YaEngine.ImGui;
using YaEngine.Input;

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
    .AddScoped<IInitializeSystem, BuildSceneSystem>()
    .AddScoped<IUpdateSystem, MoveCameraSystem>()
    .AddScoped<IUpdateSystem, QuitSystem>()
    .AddScoped<IImGuiSystem, ShowTransformsGuiSystem>()
    .AddScoped<IUpdateSystem, MoveLightSystem>();
    
var serviceProvider = services.BuildServiceProvider();
var bootstrapper = serviceProvider.GetService<SilkBootstrapper>();
bootstrapper.Run();