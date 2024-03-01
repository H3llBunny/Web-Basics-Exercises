using System.Net;
using System.Net.Sockets;
using System.Text;
using TweetExercise.Data;
using TweetExercise.Data.Models;

namespace TweetExercise
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var db = new TweetDbContext();
            db.Database.EnsureCreated();

            const string NewLine = "\r\n";
            var tcpListener = new TcpListener(IPAddress.Loopback, 6969);
            tcpListener.Start();

            while (true)
            {
                var client = tcpListener.AcceptTcpClient();
                using (var stream = client.GetStream())
                {
                    byte[] buffer = new byte[1000000];
                    var length = stream.Read(buffer, 0, buffer.Length);

                    string requestString = Encoding.UTF8.GetString(buffer, 0, length);

                    Console.WriteLine(requestString);

                    if (requestString.Contains("POST /tweet"))
                    {
                        // Extracting username and tweet from requst body
                        string[] parts = requestString.Split(new[] { "\r\n\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                        if (parts.Length > 1)
                        {
                            string[] formData = parts[1].Split('&');
                            string username = formData[0].Split('=')[1];
                            string tweet = formData[1].Split('=')[1];

                            if (username != null & tweet != null)
                            {
                                tweet = tweet.Replace("+", " ");
                                // Saving tweet to db
                                db.Tweets.Add(new Tweet { Username = username, TweetMessage = tweet });
                                db.SaveChanges();
                            }
                        }

                        string redirectHtml = "<html><head><meta http-equiv='refresh' content='0; url=/tweets' /></head></html>";
                        string response = "HTTP/1.1 302 Found" + NewLine +
                                          "Server: Blago2024" + NewLine +
                                          "Location: /tweets" + NewLine +
                                          "Content-Type: text/html; charset=utf-8" + NewLine +
                                          "Content-Length: " + redirectHtml.Length + NewLine +
                                          NewLine +
                                          redirectHtml + NewLine;

                        byte[] responseBytes = Encoding.UTF8.GetBytes(response);
                        stream.Write(responseBytes);
                    }
                    else if (requestString.Contains("GET /tweets"))
                    {
                        var tweets = db.Tweets.ToList();

                        var htmlBuilder = new StringBuilder();
                        htmlBuilder.Append("<h1>All Tweets</h1>");

                        foreach (var tweet in tweets)
                        {
                            htmlBuilder.Append($"<p>{tweet.Username}: {tweet.TweetMessage}</p>");
                        }

                        // Button to return to initial form
                        htmlBuilder.Append($"<form action='/' method='get'>");
                        htmlBuilder.Append($"<input type='submit' value='Return to Form'>");
                        htmlBuilder.Append($"</form>");

                        string html = htmlBuilder.ToString();

                        string response = "HTTP/1.1 200 OK" + NewLine +
                                          "Server: Blago2024" + NewLine +
                                          "Content-Type: text/html; charset=utf-8" + NewLine +
                                          "Content-Length: " + html.Length + NewLine +
                                          NewLine +
                                          html + NewLine;
                        byte[] responseBytes = Encoding.UTF8.GetBytes(response);
                        stream.Write(responseBytes);
                    }
                    else
                    {
                        string html = $"<h1>Hello {DateTime.Now}</h1>" +
                        $"<form action=/tweet method=post><input name=username /><input name=tweet />" +
                        $"<input type=submit value='Tweet'></form>";

                        string response = "HTTP/1.1 200 OK" + NewLine +
                            "Server: Blago2024" + NewLine +
                            "Content-Type: text/html; charset=utf-8" + NewLine +
                            "Content-Lenght " + html.Length + NewLine +
                            NewLine +
                            html + NewLine;

                        byte[] responseBytes = Encoding.UTF8.GetBytes(response);
                        stream.Write(responseBytes);
                    }
                }
            }
        }
    }
}
