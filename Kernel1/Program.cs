// Copyright (c) Microsoft. All rights reserved.

using Kernel1;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;

var configBuilder = new ConfigurationBuilder();
configBuilder.AddUserSecrets(typeof(Program).Assembly);
var conf = configBuilder.Build();

const string ModelGpt3Id = "gpt-3.5-turbo-1106";
const string ModelGpt4Id = "gpt-4-1106-preview";
const string EmbeddingModelId = "text-embedding-ada-002";
string ApiKey = conf["OpenAI:ApiKey"]!;

var builder = Kernel.CreateBuilder();
builder.AddOpenAIChatCompletion(ModelGpt3Id, ApiKey);
builder.AddOpenAITextEmbeddingGeneration(EmbeddingModelId, ApiKey);
builder.Services.AddLogging(c => c.AddDebug().SetMinimumLevel(LogLevel.Trace));
builder.Services.AddHttpClient();
builder.Plugins.AddFromType<LightPlugin>();
var kernel = builder.Build();

var logger = kernel.GetRequiredService<ILogger<Program>>();

Console.WriteLine("START");

var executionSettings = new OpenAIPromptExecutionSettings()
{
    ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions,
};

string prompt = "Sprawdź stan światła i zmień go na włączone.";
var arguments = new KernelArguments(executionSettings);

var result = await kernel.InvokePromptAsync(prompt, arguments);
Console.WriteLine(result);

Console.WriteLine("\nEND");
Console.ReadKey();
return;

void PrintFunction(KernelFunctionMetadata func)
{
    Console.WriteLine($"Plugin: {func.PluginName}");
    Console.WriteLine($"   {func.Name}: {func.Description}");

    if (func.Parameters.Count > 0)
    {
        Console.WriteLine("      Params:");
        foreach (var p in func.Parameters)
        {
            Console.WriteLine($"      - {p.Name}: {p.Description}");
            Console.WriteLine($"        default: '{p.DefaultValue}'");
        }
    }

    Console.WriteLine();
}