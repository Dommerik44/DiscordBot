using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Upload;
using Google.Apis.Util.Store;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;

namespace Youtube
{
    public class YouTube
    {
        [STAThread]
        static void Main(string[] args)
        {
            Console.WriteLine("YouTube Data API: Playlist Updates");
            Console.WriteLine("==================================");

            try
            {
                new YouTube(VideoID).Run(VideoID).Wait();
            }
            catch (AggregateException ex)
            {
                foreach (var e in ex.InnerExceptions)
                {
                    Console.WriteLine("Error: " + e.Message);
                }
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
        private static string videoID;

        public YouTube(string id)
        {
            videoID = id;
           
        }

        public static string VideoID
        {
            get { return videoID; }
        }
        private async Task Run(string videoID)
        {
            UserCredential credential;
            using (var stream = new FileStream("client_secrets.json", FileMode.Open, FileAccess.Read))
            {
                credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    new[] { YouTubeService.Scope.Youtube },
                    "user",
                    CancellationToken.None,
                    new FileDataStore(this.GetType().ToString())
                );
            }

            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = this.GetType().ToString()
            });

            // Add a video to the newly created playlist.
            var PlaylistItem = new PlaylistItem();
            PlaylistItem.Snippet = new PlaylistItemSnippet();
            PlaylistItem.Snippet.PlaylistId = "PL0l1yX2nUgbc0jiNBbWUCeW8HRHXHxRM6";
            PlaylistItem.Snippet.ResourceId = new ResourceId();
            PlaylistItem.Snippet.ResourceId.Kind = "youtube#video";
            PlaylistItem.Snippet.ResourceId.VideoId = videoID;
            PlaylistItem = await youtubeService.PlaylistItems.Insert(PlaylistItem, "snippet").ExecuteAsync();

            Console.WriteLine("Playlist item id {0} was added to playlist id {1}.", PlaylistItem.Id, "PL0l1yX2nUgbc0jiNBbWUCeW8HRHXHxRM6");
        }
    }
}