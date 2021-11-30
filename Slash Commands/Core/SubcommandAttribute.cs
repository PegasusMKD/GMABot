namespace GMABot.Slash_Commands
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    internal class SubcommandAttribute : Attribute
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Group { get; set; }
    }
}
