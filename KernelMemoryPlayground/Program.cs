using Microsoft.Extensions.Configuration;
using Microsoft.KernelMemory;

var configuration = new ConfigurationBuilder()
    .AddUserSecrets<Program>()
    .Build();

var openAiApiKey = configuration["OPENAI_API_KEY"] ?? throw new InvalidOperationException("OPENAI_API_KEY");


var memory = new KernelMemoryBuilder()
    .WithOpenAI(new OpenAIConfig
    {
        APIKey = openAiApiKey,
        EmbeddingModel = "text-embedding-3-large",
        EmbeddingModelMaxTokenTotal = 8191,
        MaxRetries = 3,
        TextModel = "gpt-3.5-turbo-0125",
        TextModelMaxTokenTotal = 4096
    })
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
    Console.Write("\nQuestion: ");
    var question = Console.ReadLine();
    if (string.IsNullOrEmpty(question)) break;

    var answer = await memory.AskAsync(question);
    Console.WriteLine($"AI: {answer.Result}");

    if (answer.RelevantSources.Any()) 
        Console.WriteLine("Source:");

    foreach (var x in answer.RelevantSources)
    {
        Console.WriteLine($"  * {x.SourceName} -- {x.Partitions.First().LastUpdate:D}");
    }
}