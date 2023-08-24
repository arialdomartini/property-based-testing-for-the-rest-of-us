namespace PropertyBasedTesting.Resources;

internal static class Serializer
{
    internal static string Serialize<T>(T @object) => 
        System.Text.Json.JsonSerializer.Serialize(@object);

    internal static T Deserialize<T>(string json) => 
        System.Text.Json.JsonSerializer.Deserialize<T>(json);
}
