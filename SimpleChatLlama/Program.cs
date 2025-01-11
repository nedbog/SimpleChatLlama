
using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder();

builder.Services.AddChatClient(new OllamaChatClient(new Uri("http://localhost:11434"), "llama3"));

var app = builder.Build();

Console.WriteLine("This is a simple chat client application which needs ollama with llama3 model");
Console.WriteLine("-----------------------------------------------------------------------------");

var readMePath = Path.Join(AppContext.BaseDirectory, "README.md");
var readMeText = File.ReadAllText(readMePath);

Console.WriteLine(readMeText);
Console.WriteLine("-----------------------------------------------------------------------------");

var chatClient = app.Services.GetRequiredService<IChatClient>();

var chatHistory = new List<ChatMessage>();

Console.Write("You: ");
var prompt = Console.ReadLine() ?? "";

while (!string.IsNullOrWhiteSpace(prompt) && prompt != "quit")
{
    chatHistory.Add(new ChatMessage { Text = prompt, Role = ChatRole.User });

    var chatCompletion = await chatClient.CompleteAsync(prompt);
    Console.Write("Llama: ");
    var response = "";

    await foreach (var item in chatClient.CompleteStreamingAsync(chatHistory))
    {
        Console.Write(item.Text);
        response += item.Text;
    }

    chatHistory.Add(new ChatMessage { Text = response, Role = ChatRole.Assistant });
    Console.WriteLine();

    Console.Write("You: ");
    prompt = Console.ReadLine() ?? "";
}
