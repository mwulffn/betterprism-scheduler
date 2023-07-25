using Odk.BluePrism;
using Odk.Scheduler.Database;
using Odk.Scheduler.Database.Models;
using Odk.Scheduler.Dto;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Odk.Scheduler.Controllers
{
    [RoutePrefix("api/screenshot")]
    public class ScreenshotController : BaseController<Guid, ScreenshotItem>
    {
        private readonly ISessionRepository sessionRepository;
        private readonly IScreenshotRepository screenshotRepository;

        public ScreenshotController(ISessionRepository sessionRepository, IScreenshotRepository screenshotRepository, IBluePrism bluePrism) : base(bluePrism)
        {
            this.sessionRepository = sessionRepository;
            this.screenshotRepository = screenshotRepository;
        }

        public override ScreenshotItem Post([FromBody] ScreenshotItem obj)
        {
            var session = sessionRepository.SessionFromBPSession(obj.BPSessionId);

            var sc = new Screenshot()
            {
                BPSessionId = obj.BPSessionId,
                Mimetype = obj.Mimetype,
                ScreenshotId = Guid.NewGuid(),
                ScreenshotData = obj.ScreenshotBin,
                Created = DateTime.Now,
                SessionId = session?.SessionId,
                ItemKey = obj.ItemKey
            };

            screenshotRepository.Insert(sc);

            return obj;
        }

        [Route("sessiongroups")]
        [HttpGet]
        public IEnumerable<ScreenshotSessionGroup> SessionGroups(string filter = "")
        {
            return this.screenshotRepository.SessionGroups(filter);
        }

        [Route("screenshotsForSession/{id}")]
        [HttpGet]
        public IEnumerable<Screenshot> ScreenshotsForSession(Guid id)
        {
            var screenshots = this.screenshotRepository.ScreenshotsBySession(id);

            foreach (var screenshot in screenshots)
                screenshot.ScreenshotData = null;

            return screenshots;
        }

        [Route("thumbnail/{id}")]
        [HttpGet]
        public HttpResponseMessage Thumbnail(Guid id)
        {
            var sc = screenshotRepository.SingleOrDefault(id);

            if (sc == null)
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }

            var resultstream = new MemoryStream();
            var ms = new MemoryStream(sc.ScreenshotData);
            var img = Bitmap.FromStream(ms);
            var scaleFactor = 300.0 / img.Width;
            var bitmap = ResizeImage(img, 300, (int)(img.Height * scaleFactor));
            bitmap.Save(resultstream, ImageFormat.Png);
            resultstream.Position = 0;
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StreamContent(resultstream);
            response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/png");

            return response;
        }

        [Route("full/{id}")]
        [HttpGet]
        public HttpResponseMessage Full(Guid id)
        {
            var sc = screenshotRepository.SingleOrDefault(id);

            if (sc == null)
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }

            var ms = new MemoryStream(sc.ScreenshotData);
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StreamContent(ms);
            response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/png");

            return response;
        }

        /// <summary>
        /// Resize the image to the specified width and height.
        /// </summary>
        /// <param name="image">The image to resize.</param>
        /// <param name="width">The width to resize to.</param>
        /// <param name="height">The height to resize to.</param>
        /// <returns>The resized image.</returns>
        public Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);
            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }
    }
}