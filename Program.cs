using System.Net;
using Newtonsoft.Json;

PrintLogo();

string bhvrSession = "";
while(!CheckBhvrSession(bhvrSession))
{
    Console.Write("bhvrSession=");
    bhvrSession = Console.ReadLine();
    Console.Clear();
    PrintLogo();
}

while (true)
{
    await Loop(bhvrSession);
    Task.Delay(4000).Wait();
}

Console.ReadLine();

async Task Loop(string bhvrSession)
{
    var res = await GetBloodpointsCount(bhvrSession);

    if(res != -1)
        Console.WriteLine("Current Bloodpoints: " + res);
}

async Task<int> GetBloodpointsCount(string bhvrSession)
{
    int bloodpoints = -1;

    try
    {
        string url = "https://brill.live.bhvrdbd.com/api/v1/wallet/currencies";

        var headers = GetHeaders(bhvrSession);
        var resp = await GetRequestAsync(url, headers);

        dynamic json = JsonConvert.DeserializeObject(resp);

        for(int i = 0; i < json.list.Count; i++)
        {
            var item = json.list[i];
            if(item.currency == "Bloodpoints")
            {
                bloodpoints = item.balance;
                break;
            }
        }
    } catch (Exception ex)
    {
        Console.WriteLine("Invalid bhvrSession! (" + ex.Message + ")");
    }

    return bloodpoints;
}

async Task<string> GetRequestAsync(string url, WebHeaderCollection headers)
{
    string result = "";

    try
    {
        WebRequest request = WebRequest.Create(url);
        request.Method = "GET";
        request.Headers = headers;

        WebResponse response = await request.GetResponseAsync();
        using (Stream stream = response.GetResponseStream())
        {
            using (StreamReader reader = new StreamReader(stream))
            {
                result = reader.ReadToEnd();
            }
        }
        response.Close();
    } catch (Exception ex)
    {
        result = ex.Message;
    }

    return result;
}

WebHeaderCollection GetHeaders(string bhvrSession)
{
    var headers = new WebHeaderCollection();

    headers["Cookie"] = "bhvrSession=" + bhvrSession;
    headers["Accept"] = "*/*";
    headers["Accept-Encoding"] = "gzip, deflate, br";
    headers["Connection"] = "keep-alive";
    headers["Content-Type"] = "application/json";
    headers["x-kraken-client-platform"] = "egs";
    headers["x-kraken-client-provider"] = "egs";
    headers["x-kraken-client-resolution"] = "1536x864";
    headers["x-kraken-client-timezone-offset"] = "-180";
    headers["x-kraken-client-os"] = "10.0.19043.1.256.64bit";
    headers["x-kraken-client-version"] = "6.2.2";
    headers["User-Agent"] = "DeadByDaylight/++DeadByDaylight+Live-CL-620318 EGS/10.0.19043.1.256.64bit";

    return headers;
}

bool CheckBhvrSession(string bhvrSession)
{
    if (bhvrSession.Length != 346) return false;
    return true;
}

void PrintLogo()
{
    Console.Title = "BloodpointChecker by NightStranger";

    Console.WriteLine("██████╗ ██████╗      ██████╗██╗  ██╗███████╗ ██████╗██╗  ██╗███████╗██████╗ ");
    Console.WriteLine("██╔══██╗██╔══██╗    ██╔════╝██║  ██║██╔════╝██╔════╝██║ ██╔╝██╔════╝██╔══██╗");
    Console.WriteLine("██████╔╝██████╔╝    ██║     ███████║█████╗  ██║     █████╔╝ █████╗  ██████╔╝");
    Console.WriteLine("██╔══██╗██╔═══╝     ██║     ██╔══██║██╔══╝  ██║     ██╔═██╗ ██╔══╝  ██╔══██╗");
    Console.WriteLine("██████╔╝██║         ╚██████╗██║  ██║███████╗╚██████╗██║  ██╗███████╗██║  ██║");
    Console.WriteLine("╚═════╝ ╚═╝          ╚═════╝╚═╝  ╚═╝╚══════╝ ╚═════╝╚═╝  ╚═╝╚══════╝╚═╝  ╚═╝");
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("\nv1.0.0");
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine("By NightStranger\n\n");
    Console.ForegroundColor = ConsoleColor.White;
}

async void CheckKey()
{
    while (true)
    {
        ConsoleKeyInfo key = Console.ReadKey(true);

        if (key.Key == ConsoleKey.Enter || key.Key == ConsoleKey.Spacebar)
        {
            await Loop(bhvrSession);
        }
    }
}