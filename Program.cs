using GMABot.Http;
using GMABot.Models;
using GMABot.Slash_Commands.Creator;
using Newtonsoft.Json;
using System.Text;

//string configJson;
//Console.OutputEncoding = Encoding.Unicode;

//using (StreamReader r = new StreamReader("./config.json"))
//    configJson = r.ReadToEnd();

//var config = JsonConvert.DeserializeObject<Configuration>(configJson);
//MessageScheduler scheduler = new(config);
//scheduler.Start();

DiscordCommandCreator.CreateCommands();
