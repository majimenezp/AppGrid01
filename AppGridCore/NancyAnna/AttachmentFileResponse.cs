using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nancy;
using System.IO;
namespace NancyAnna
{
    public class AttachmentFileResponse : Response
    {
        public AttachmentFileResponse(string filePath) :
            this(filePath, Path.GetFileName(filePath), MimeTypes.GetMimeType(filePath))
        {
        }
        public AttachmentFileResponse(string filePath, string name)
            : this(filePath, name, MimeTypes.GetMimeType(filePath))
        {
        }
        public AttachmentFileResponse(string filePath, string name, string contentType)
        {
            InitializeAttachmentFileResponse(filePath, name, contentType);
        }

        private void InitializeAttachmentFileResponse(string filePath, string fileName, string contentType)
        {
            if (string.IsNullOrEmpty(filePath) ||
               !File.Exists(filePath) ||
               !Path.HasExtension(filePath))
            {
                this.StatusCode = HttpStatusCode.NotFound;
            }
            else
            {
                var fi = new FileInfo(filePath);
                // TODO - set a standard caching time and/or public?
                this.Headers["ETag"] = fi.LastWriteTimeUtc.Ticks.ToString("x");
                this.Headers["Last-Modified"] = fi.LastWriteTimeUtc.ToString("R");
                this.Headers["Content-Disposition"] = "attachment; filename=" + fileName;
                this.Contents = GetFileContent(filePath);
                this.ContentType = contentType;
                this.StatusCode = HttpStatusCode.OK;
            }
        }

        private Action<Stream> GetFileContent(string filePath)
        {
            return stream =>
            {
                using (var file = File.OpenRead(filePath))
                {
                    var buffer = new byte[4096];
                    var read = -1;
                    while (read != 0)
                    {
                        read = file.Read(buffer, 0, buffer.Length);
                        stream.Write(buffer, 0, read);
                    }
                }
            };
        }
    }
}
