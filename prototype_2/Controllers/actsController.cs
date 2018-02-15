using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using prototype_2;
using Word = Microsoft.Office.Interop.Word;
using System.Data.SqlClient;

namespace prototype_2.Controllers
{
    public class actsController : Controller
    {
        private actsEntities1 db = new actsEntities1();

        // GET: acts
        public ActionResult Index()
        {
            var acts = db.acts.Include(a => a.users);
            return View(acts.ToList());
        }

        // GET: acts/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            acts acts = db.acts.Find(id);
            if (acts == null)
            {
                return HttpNotFound();
            }
            return View(acts);
        }

        //// GET: acts/Create
        public ActionResult Create()
        {
            ViewBag.user_id = new SelectList(db.users, "user_id", "name");
            return View();
        }

        // POST: acts/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "act_id,number,date,name,location,geo_location,photo,doc,extra_info,user_id")] acts acts, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                acts.act_id = Guid.NewGuid();

                acts.photo = new byte[file.ContentLength];
                file.InputStream.Read(acts.photo, 0, file.ContentLength);

                db.acts.Add(acts);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.user_id = new SelectList(db.users, "user_id", "name", acts.user_id);
            return View(acts);
        }

        // GET: acts/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            acts acts = db.acts.Find(id);
            if (acts == null)
            {
                return HttpNotFound();
            }
            ViewBag.user_id = new SelectList(db.users, "user_id", "name", acts.user_id);
            return View(acts);
        }

        // POST: acts/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "act_id,number,date,name,location,geo_location,photo,doc,extra_info,user_id")] acts acts)
        {
            if (ModelState.IsValid)
            {
                db.Entry(acts).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.user_id = new SelectList(db.users, "user_id", "name", acts.user_id);
            return View(acts);
        }

        // GET: acts/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            acts acts = db.acts.Find(id);
            if (acts == null)
            {
                return HttpNotFound();
            }
            return View(acts);
        }

        // POST: acts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            acts acts = db.acts.Find(id);
            db.acts.Remove(acts);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult ViewPhoto(Guid id)
        {
            acts act = db.acts.Find(id);
            if (act == null)
            {
                return HttpNotFound();
            }
            return File(act.photo, "image/png");
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public readonly string TemplateFileName = "/Content/template.docx";
        public ActionResult ExportToWord(Guid id)
        {
            acts act = db.acts.Find(id);

            var NumberAct1 = db.acts.Where(p => p.act_id == id).Select(p => p.number);

            string[] tempNA = NumberAct1.ToArray();
            string NumberAct = tempNA[0];

            var Date1 = db.acts.Where(o => o.act_id == id).Select(o => o.date);

            var Name1 = db.acts.Where(l => l.act_id == id).Select(l => l.name);
            string[] tempN = Name1.ToArray();
            string Name = tempN[0];
            var Coordinates1 = db.acts.Where(m => m.act_id == id).Select(m => m.location);
            string[] tempC = Coordinates1.ToArray();
            string Coordinates = tempC[0];
            var Information1 = db.acts.Where(n => n.act_id == id).Select(n => n.extra_info);
            string[] tempI = Information1.ToArray();
            string Information = tempI[0];
            var Resp1 = db.acts.Where(k => k.act_id == id).Select(k => k.users.name);
            string[] tempR = Resp1.ToArray();
            string Resp = tempR[0];


            var WordApp = new Word.Application();
            WordApp.Visible = false;

            var wordDocument = WordApp.Documents.Open(TemplateFileName);
            ReplaceWordStub("{NumberAct}", NumberAct, wordDocument);
            //ReplaceWordStub("{Date}", Date, wordDocument);
            ReplaceWordStub("{Name}", Name, wordDocument);
            ReplaceWordStub("{Coordinates}", Coordinates, wordDocument);
            ReplaceWordStub("{Information}", Information, wordDocument);
            ReplaceWordStub("{Resp}", Resp, wordDocument);
            wordDocument.SaveAs("/Content/Act.docx");
            wordDocument.Close();

            byte[] wordDocumentBytes = System.IO.File.ReadAllBytes("/Content/Act.docx");
            WordApp.Visible = true;
            return File(wordDocumentBytes, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "Act.docx");
        }
        public void ReplaceWordStub(string stubToReplace, string text, Microsoft.Office.Interop.Word.Document wordDocument)
        {
            var range = wordDocument.Content;
            range.Find.ClearFormatting();
            range.Find.Execute(FindText: stubToReplace, ReplaceWith: text);

        }
    }
}
