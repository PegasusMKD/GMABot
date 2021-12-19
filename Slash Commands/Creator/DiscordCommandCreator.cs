using GMABot.HTTP;
using GMABot.Slash_Commands.Core;
using GMABot.WebSocket;
using Newtonsoft.Json;
using System.Reflection;

namespace GMABot.Slash_Commands.Creator
{
    static internal class DiscordCommandCreator
    {
        public static void CreateCommands()
        {

            // Get all annotated types & classes
            // Generate objects which will store this information
            Assembly assembly = Assembly.GetExecutingAssembly();

            var typesWithMyAttribute =
                from type in assembly.GetTypes().AsParallel()
                let attributes = type.GetCustomAttributes(typeof(SubcommandAttribute), true)
                where attributes != null && attributes.Length > 0
                select (Type: type, Attribute: attributes.Cast<SubcommandAttribute>().First());

            List<KeyValuePair<string, ISubcommand>> subcommands = typesWithMyAttribute.AsParallel().Select(type => KeyValuePair.Create(type.Attribute.Name!,(Activator.CreateInstance(type.Type) as ISubcommand)!)).ToList();
            DiscordSubcommand mainCommand = GenerateMainCommand(typesWithMyAttribute);

            // Send payload for "Create Global Application Command" ( POST /applications/{application.id}/commands)
            DiscordCreateCommand(mainCommand);

            // Populate "commands" dictionary
            foreach (var subcommand in subcommands)
                EventDispatcher.commands.Add(subcommand.Key!, subcommand.Value!);

            Console.WriteLine($"[{DateTime.Now}] Added commands to dispatcher.");
        }

        private static void DiscordCreateCommand(DiscordSubcommand mainCommand)
        {
            String? localCommandJson = null;
            if(File.Exists("./command.json"))
                using (StreamReader r = new StreamReader("./command.json"))
                    localCommandJson = r.ReadToEnd();

            String latestCommandJson = JsonConvert.SerializeObject(mainCommand);
            if(!String.Equals(latestCommandJson,localCommandJson, StringComparison.Ordinal))
            {
                Console.WriteLine($"[{DateTime.Now}] Updated commands.");
                DiscordHttpClient.CreateCommand(mainCommand);
                File.WriteAllText("./command.json", latestCommandJson);
            }
        }

        private static DiscordSubcommand GenerateMainCommand(ParallelQuery<(Type Type, SubcommandAttribute Attribute)> typesWithMyAttribute)
        {
            List<DiscordSubcommand> discordSubcommands = typesWithMyAttribute.AsParallel().Select(type =>
                new DiscordSubcommand { name = type.Attribute.Name!, description = type.Attribute.Description!, options = GetOptions(type.Type), group = type.Attribute.Group! }
            ).OrderBy(val => val.name).ToList();

            List<DiscordSubcommand> toGroup = discordSubcommands.FindAll(subcommand => subcommand.group != null);
            discordSubcommands.RemoveAll(subcommand => subcommand.group != null);

            List<DiscordSubcommand> grouped = toGroup.GroupBy(subcommand => subcommand.group).Select(group =>
            {
                string _group = group.Key!;
                foreach (var subcommand in group)
                    subcommand.group = null;
                return new DiscordSubcommand { name = _group, description = _group, type = 2, options = group.ToArray() };
            }).ToList();
            discordSubcommands.AddRange(grouped);

            return new DiscordSubcommand { name = "baba", description = "Communicate with \"GMA\" bot", options = discordSubcommands.ToArray(), type = null };
        }

        private static DiscordSubcommand[]? GetOptions(Type type) =>
            (Activator.CreateInstance(type) as ISubcommand)!.parameters?.Select(instance => new DiscordSubcommand(instance)).ToArray();
    }
}
