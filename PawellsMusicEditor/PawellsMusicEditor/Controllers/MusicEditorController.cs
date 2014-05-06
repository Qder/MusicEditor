using System;
using System.Transactions;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PawellsMusicEditor.Models;
using System.IO;


using DevBridge.Templates.WebProject.Data;
using DevBridge.Templates.WebProject.Data.DataContext;
using DevBridge.Templates.WebProject.DataContracts;
using DevBridge.Templates.WebProject.DataEntities.Entities;


using NHibernate.Criterion;

using User = DevBridge.Templates.WebProject.DataEntities.Entities.User;

namespace PawellsMusicEditor.Controllers
{
    public class MusicEditorController : Controller
    {
        private static readonly ISessionFactoryProvider SessionFactoryProvider = new SessionFactoryProvider();
        //
        // GET: /MusicEditor/

        public ActionResult Index()
        {
            
            

            IRepository repository = new Repository(SessionFactoryProvider);

            SoundTracks soundTrackAlias = null;
            var list = repository.AsQueryOver(() => soundTrackAlias).List();

            //User userAlias = null;
            //var list = repository
            //    .AsQueryOver(() => userAlias)
            //  .Where(Restrictions.On(() => userAlias.FirstName).IsLike("Paulius"))
            //    .List();

            ViewBag.Songs = list;



            return View();
        }

        [HttpPost]
        public ActionResult Index(SoundTrack soundTrack)
        {
            if (soundTrack.Failas.ContentLength > 0)
            {
                var fileName = Path.GetFileName(soundTrack.Failas.FileName);
                var path = Path.Combine(Server.MapPath("~/Content/Songs/"), fileName);
                soundTrack.Failas.SaveAs(path);

                 IRepository repository = new Repository(SessionFactoryProvider);

                 var soundTracks = new SoundTracks
                 {
                     SoundTrackName = fileName
                 };
                 repository.Save(soundTracks);
            }
            return RedirectToAction("Index");
        }

        
        public ActionResult RemoveSoundTrack(int id)
        {
            IRepository repository = new Repository(SessionFactoryProvider);
                repository.Delete<SoundTracks>(id);
                repository.Commit();
            
           
            return RedirectToAction("Index");
        }
    }
}