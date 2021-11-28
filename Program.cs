using GMABot.Http;
using GMABot.Models;
using Newtonsoft.Json;
using System.Text;

string configJson;
Console.OutputEncoding = Encoding.Unicode;

using (StreamReader r = new StreamReader("./config.json"))
    configJson = r.ReadToEnd();

var config = JsonConvert.DeserializeObject<Configuration>(configJson);
MessageScheduler scheduler = new(config);
scheduler.Start();
