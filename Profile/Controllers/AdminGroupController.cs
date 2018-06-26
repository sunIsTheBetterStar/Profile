﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using Profile.Models.Database;
using Profile.ViewModels.Admin.Groups;

namespace Profile.Controllers
{
    public class AdminGroupController : Controller
    {
        DBWorkerAdminGroup DBWorker = new DBWorkerAdminGroup();
        public ActionResult Index()
        {
            List<Group> model = new List<Group>();
            model = DBWorker.GetGroupList();
            var tt = model;
            return View(model);
        } // end Index()

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(ViewModel_CreateGroup model)
        {
            if (ModelState.IsValid)
            {
                model.group.ImageFileSystemPath = Path.Combine(Server.MapPath("~/Content/UserImages/"), model.image.FileName);
                model.group.ImageProgectLinkPath = "~/Content/UserImages/" + model.image.FileName;
                model.image.SaveAs(model.group.ImageFileSystemPath);
                model.group.CreationDate = DateTime.Now.ToShortDateString();
                DBWorker.CreateGroup(model.group);
                return RedirectToAction("Index");
            }
            else return View(model);
        } // end Add()

        [HttpGet]
        public ActionResult Edit(int id)
        {
            try
            {
                Group group = DBWorker.GetGroupList().Find(f => f.GroupId == id);
            }
            catch
            {
                string[] errorMessages = { "This group not fond" };
                return View("Error", errorMessages);
            }

            return View();
        }
        [HttpPost]
        public ActionResult Edit(Group group)
        {
            DBWorker.EditGroup(group);
            return RedirectToAction("Index");
        } // end Edit

        [HttpPost]
        public ActionResult Delete(Group group)
        {
            if (group.ImageFileSystemPath != null && System.IO.File.Exists(group.ImageFileSystemPath))
            {
                try
                {
                    System.IO.File.Delete(group.ImageFileSystemPath);
                }
                catch { }  
            }
            DBWorker.RemoveGroup(group.GroupId);
            return RedirectToAction("Index");
        } // end Delete

    } // end controller
} // end namespace