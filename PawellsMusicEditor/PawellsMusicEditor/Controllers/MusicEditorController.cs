using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PawellsMusicEditor.Models;
using System.IO;

namespace PawellsMusicEditor.Controllers
{
    public class MusicEditorController : Controller
    {
        //
        // GET: /MusicEditor/

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(SoundTrack soundTrack)
        {
            if (soundTrack.File.ContentLength > 0)
            {
                var fileName = Path.GetFileName(soundTrack.File.FileName);
                var path = Path.Combine(Server.MapPath("~/Songs/"), fileName);
            }
            return RedirectToAction("Index");
        }
    }
}
