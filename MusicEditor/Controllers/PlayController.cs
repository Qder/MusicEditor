using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MusicEditor.Controllers
{
    public class PlayController : Controller
    {
        //
        // GET: /Play/

        public ActionResult Music()
        {
            return View();
        }
    [HttpPost]
        public ActionResult Music()
        {
            return;
        }

    }
}
