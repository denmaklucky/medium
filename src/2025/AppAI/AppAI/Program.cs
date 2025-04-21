using Microsoft.Extensions.AI;

var chatClient = new OllamaChatClient(new Uri("http://localhost:11434/"), "deepseek-r1:1.5b");

List<ChatMessage> chatHistory = [];

Console.WriteLine("User:");
var userRequest = Console.ReadLine();

chatHistory.Add(new ChatMessage(ChatRole.User, userRequest));

Console.WriteLine("AI:");
var aiResponse = string.Empty;

await foreach (var update in chatClient.GetStreamingResponseAsync(chatHistory))
{
    Console.Write(update.Text);
    aiResponse += update.Text;
}

chatHistory.Add(new ChatMessage(ChatRole.Assistant, aiResponse));