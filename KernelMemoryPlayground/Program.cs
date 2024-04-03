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
    await memory.ImportDocumentAsync("Resources/C01L01.md", tags: new TagCollection
    {
        { "lesson", "C01L01" },
        { "title", "Wprowadzenie do Generative AI"}
    });
    await memory.ImportDocumentAsync("Resources/C01L02.md", tags: new TagCollection
    {
        { "lesson", "C01L02" },
        { "title", "Zasady działania LLM"}
    });
    await memory.ImportDocumentAsync("Resources/C01L03.md", tags: new TagCollection
    {
        { "lesson", "C01L03" },
        { "title", "Prompt Design"}
    });
    await memory.ImportDocumentAsync("Resources/C01L04.md", tags: new TagCollection
    {
        { "lesson", "C01L04" },
        { "title", "OpenAI API"}
    });
    await memory.ImportDocumentAsync("Resources/C01L05.md", tags: new TagCollection
    {
        { "lesson", "C01L05" },
        { "title", "Prompt Engineering"}
    });
}

while (true)
{
    Console.Write("Question:");
    var question = Console.ReadLine();
    if (string.IsNullOrEmpty(question)) break;

    var answer = await memory.AskAsync(question);
    Console.WriteLine($"AI: {answer.Result}");
}