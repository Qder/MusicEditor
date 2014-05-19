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

using SoundTrackChanges;


using NHibernate.Criterion;

using User = DevBridge.Templates.WebProject.DataEntities.Entities.User;

namespace PawellsMusicEditor.Controllers
{
    public class MusicEditorController : Controller
    {
        private static readonly ISessionFactoryProvider SessionFactoryProvider = new SessionFactoryProvider();

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


        public ActionResult RemoveSoundTrack(int id, string songTitle)
        {
            IRepository repository = new Repository(SessionFactoryProvider);
            repository.Delete<SoundTracks>(id);
            repository.Commit();

            System.IO.File.Delete("C:\\Users\\Administratorius\\Documents\\GitHub\\MusicEditor\\PawellsMusicEditor\\PawellsMusicEditor\\Content\\Songs\\" + songTitle);
            return RedirectToAction("Index");
        }

        //path - file name
        public ActionResult Crop(string path, int from, int to)
        {
            //ppath - directory
            string ppath = "C:\\Users\\Administratorius\\Documents\\GitHub\\MusicEditor\\PawellsMusicEditor\\PawellsMusicEditor\\Content\\Songs\\";
            string editedFile = "croped " + path;
            for (int i = 0; i < 100; i++)
            {
                if (!System.IO.File.Exists(ppath + (i + 1) + editedFile))
                {
                    editedFile = (i + 1) + editedFile;
                    WavFileUtils.TrimWavFile(ppath + path, ppath + editedFile, TimeSpan.FromSeconds(from), TimeSpan.FromSeconds(to));
                    break;
                }
            }


            IRepository repository = new Repository(SessionFactoryProvider);

            var soundTracks = new SoundTracks
            {
                SoundTrackName = editedFile
            };
            if (editedFile.Length < 200)
            {
                repository.Save(soundTracks);
            }
            else
            {
            }
            return RedirectToAction("Index");
        }

        public ActionResult Download(string fileName)
        {
            return File(@"C:\Users\Administratorius\Documents\GitHub\MusicEditor\PawellsMusicEditor\PawellsMusicEditor\Content\Songs\" + fileName, "application/mp3", fileName);
        }


        //path - file name
        public ActionResult LowPassFilter(string path)
        {
            //ppath - directory
            string ppath = "C:\\Users\\Administratorius\\Documents\\GitHub\\MusicEditor\\PawellsMusicEditor\\PawellsMusicEditor\\Content\\Songs\\";
            string editedFile = "filtered " + path;
            for (int i = 0; i < 100; i++)
            {
                if (!System.IO.File.Exists(ppath + (i + 1) + editedFile))
                {
                    editedFile = (i + 1) + editedFile;
                    WavFileUtils.LowPassFilter(ppath + path, ppath + editedFile);
                    break;
                }
            }


            IRepository repository = new Repository(SessionFactoryProvider);

            var soundTracks = new SoundTracks
            {
                SoundTrackName = editedFile
            };
            if (editedFile.Length < 200)
            {
                repository.Save(soundTracks);
            }
            else
            {
            }
            return RedirectToAction("Index");
        }


        //path - file name
        public ActionResult HighPassFilter(string path)
        {
            //ppath - directory
            string ppath = "C:\\Users\\Administratorius\\Documents\\GitHub\\MusicEditor\\PawellsMusicEditor\\PawellsMusicEditor\\Content\\Songs\\";
            string editedFile = "filtered " + path;
            for (int i = 0; i < 100; i++)
            {
                if (!System.IO.File.Exists(ppath + (i + 1) + editedFile))
                {
                    editedFile = (i + 1) + editedFile;
                    WavFileUtils.HighPassFilter(ppath + path, ppath + editedFile);
                    break;
                }
            }


            IRepository repository = new Repository(SessionFactoryProvider);

            var soundTracks = new SoundTracks
            {
                SoundTrackName = editedFile
            };
            if (editedFile.Length < 200)
            {
                repository.Save(soundTracks);
            }
            else
            {
            }
            return RedirectToAction("Index");
        }
    }
}