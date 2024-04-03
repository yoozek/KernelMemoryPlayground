# KernelMemoryPlayground

Playground console app testing KernelMemory package with generating knowledge database using markdown files from aidevs.pl course.

## How to run

1. Run https://qdrant.tech/ with default ports
1. Add `OPENAI_API_KEY` to user secrets of KernelMemoryPlayground.csproj
1. Upload markdown files to folder KernelMemoryPlayground/Resources/ - [you can get them from here](https://bravecourses.circle.so/c/informacje-ai2r/tresci-lekcji-w-formacie-tekstowym-markdown-do-pobrania)
1. Change flag importDocuments to true
1. Run the app using dotnet