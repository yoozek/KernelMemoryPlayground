using Microsoft.Extensions.Configuration;
using Microsoft.KernelMemory;

var configuration = new ConfigurationBuilder()
    .AddUserSecrets<Program>()
    .Build();

var openAiApiKey = configuration["OPENAI_API_KEY"] ?? throw new InvalidOperationException("OPENAI_API_KEY");

var memory = new KernelMemoryBuilder()
    .WithOpenAIDefaults(openAiApiKey)
    .WithQdrantMemoryDb("http://localhost:6333/")
    .Build<MemoryServerless>();

var importDocuments = false;
if (importDocuments)
{
    foreach (var filesFile in Directory.GetFiles("Resources"))
    {
        await memory.ImportDocumentAsync(filesFile);
    }
}

while (true)
{
    Console.Write("Question:");
    var question = Console.ReadLine();
    if (string.IsNullOrEmpty(question)) break;

    var answer = await memory.AskAsync(question);
    Console.WriteLine($"AI: {answer.Result}");
}