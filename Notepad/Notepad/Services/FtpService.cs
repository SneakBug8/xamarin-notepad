using Notepad.Models;
using Notepad.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Notepad.Services
{
    public static class FtpService
    {
        private static readonly String ftphost= "lextorg.myjino.ru";
        private static readonly String ftpuser = "lextorg_telegram";
        private static readonly String ftppassword = "ujhy4567";

        private static readonly string directory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        public static async Task Upload(NoteModel model)
        {
            try
            {
                // File creation
                string filePath = Path.Combine(directory, $"{model.Name}.txt");
                File.WriteAllText(filePath, model._Content);

                // Get the object used to communicate with the server.
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create($"ftp://{ftphost}/temp/{model.Name}.txt");
                request.Method = WebRequestMethods.Ftp.UploadFile;

                // This example assumes the FTP site uses anonymous logon.
                request.Credentials = new NetworkCredential(ftpuser, ftppassword);

                FtpWebRequest reqFTP;
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri($"ftp://{ftphost}/temp/{model.Name}.txt"));
                reqFTP.Credentials = new NetworkCredential(ftpuser, ftppassword);
                reqFTP.KeepAlive = false;
                reqFTP.Method = WebRequestMethods.Ftp.UploadFile;
                reqFTP.UseBinary = true;
                reqFTP.Proxy = null;
                reqFTP.UsePassive = true;

                FileStream fs = File.OpenRead(filePath);
                byte[] buffer = new byte[fs.Length];
                fs.Read(buffer, 0, buffer.Length);
                fs.Close();
                Stream ftpstream = reqFTP.GetRequestStream();
                ftpstream.Write(buffer, 0, buffer.Length);
                ftpstream.Close();

                await BaseViewModel.displayService.AlertAsync("Upload complete",
                    $"Upload File Complete", "OK");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                await BaseViewModel.displayService.AlertAsync("Something happened",
                    $"{e.Message}", "OK");
            }
        }

        public static async Task<string> Download(string path)
        {
            try
            {
                // Get the object used to communicate with the server.
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create($"ftp://{ftphost}/temp/{path}.txt");
                request.Method = WebRequestMethods.Ftp.DownloadFile;

                // This example assumes the FTP site uses anonymous logon.
                request.Credentials = new NetworkCredential(ftpuser, ftppassword);

                FtpWebResponse response = (FtpWebResponse)request.GetResponse();

                Stream responseStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream);

                /*var note = new NoteModel();
                note.Name = path;
                note.Content = reader.ReadToEnd();

                await BaseViewModel.noteService.Create(note);*/

                await BaseViewModel.displayService.AlertAsync("Download complete",
                    $"Download Complete, status {response.StatusDescription}", "OK");

                Console.WriteLine($"Download Complete, status {response.StatusDescription}");

                var res = reader.ReadToEnd();

                reader.Close();
                response.Close();

                return res;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                await BaseViewModel.displayService.AlertAsync("Something happened",
                    $"{e.Message}", "OK");
                throw e;
            }
}
    }
}
