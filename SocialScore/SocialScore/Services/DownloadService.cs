using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SocialScore.Services
{
    public class DownloadService
    {
        private HttpClient _client;

        private readonly IFileService _fileService;

        private int bufferSize = 4095;


        public DownloadService(IFileService fileService)
        {
            _client = new HttpClient();
            _fileService = fileService;
        }

        public async Task DownloadFileAsync(string url)
        {
            try
            {
                // Step 1 : Get call
                var response = await _client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(string.Format("The request returned with HTTP status code {0}", response.StatusCode));
                }

                // Step 2 : Filename
                var fileName = response.Content.Headers?.ContentDisposition?.FileName ?? "SocialVideo.mp4";

                // Step 3 : Get total of data
                var totalData = response.Content.Headers.ContentLength.GetValueOrDefault(-1L);

                // Step 4 : Get total of data
                var filePath = Path.Combine(_fileService.GetStorageFolderPath(), fileName);

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                // Step 5 : Download data
                using (var fileStream = OpenStream(filePath))
                {
                    using (var stream = await response.Content.ReadAsStreamAsync())
                    {
                        var totalRead = 0L;
                        var buffer = new byte[bufferSize];
                        var isMoreDataToRead = true;

                        do
                        {
                            var read = await stream.ReadAsync(buffer, 0, buffer.Length);

                            if (read == 0)
                            {
                                isMoreDataToRead = false;
                            }
                            else
                            {
                                // Write data on disk.
                                await fileStream.WriteAsync(buffer, 0, read);

                                totalRead += read;
                            }
                        } while (isMoreDataToRead);
                    }
                }

                App.mngPreferences.SetPreferenceStringValue("video_Path", filePath);
            }
            catch (Exception e)
            {
                // Manage the exception as you need here.
                System.Diagnostics.Debug.WriteLine(e.ToString());
            }
        }

        private Stream OpenStream(string path)
        {
            return new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None, bufferSize);
        }
    }
}
