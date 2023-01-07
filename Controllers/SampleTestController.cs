using QA.Entities.Business_Entities;
using QA.Entities.Session_Entities;
using QA.Entities.View_Entities;
using QA.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Web.Mvc;
namespace QA.Controllers
{
    [Authorize]
    [RoutePrefix("SampleTest")]
    public class SampleTestController : BaseController
    {
        [AuthorizeUser(UserPermission.SampleTest_Create)]
        [Route("Create")]
        public ActionResult Create()
        {
            return View(new Vw_SampleTest()
            {
                SampleTest = new Ent_SampleTest() { ID = -1, isActive = true }
            ,
                lOVSampleTestCategories = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.Sample_Categories)
            });
        }

        [HttpPost]
        [AuthorizeUser(UserPermission.SampleTest_Create)]
        [Route("Create")]
        public ActionResult Create(Vw_SampleTest formControl)
        {
            ResponseMessage response = new ResponseMessage();
            try
            {
                bool isUpdateForm = (formControl.SampleTest.ID != -1) ? true : false;
                if (ModelState.IsValid)
                {
                    formControl.SampleTest.makerID = User.Identity.Name;
                    response = new Mdl_SampleTest().insert_updateSampleTest(formControl.SampleTest, isUpdateForm);
                    if (response.responseStatus)
                    {
                        Session["Status"] = response.responseStatus;
                        Session["EndMessage"] = response.endUserMessage = formControl.SampleTest.name
                            + ": " + Localization.SampleTest.SampleTestAddedSuccessfully;
                    }
                    else
                    {
                        Session["Status"] = response.responseStatus;
                        Session["EndMessage"] = response.endUserMessage;
                    }

                    return RedirectToAction("Create");
                }
                else
                {
                    Session["Status"] = response.responseStatus = false;
                    Session["EndMessage"] = response.endUserMessage = Localization.ErrorMessages.UnhandledErrorOccured;
                    response.comments = null;
                    response.errorMessage = ModelState.ToString();

                    return View();
                }
            }
            catch (Exception ex)
            {
                Session["Status"] = response.responseStatus = false;
                Session["EndMessage"] = response.endUserMessage = Localization.ErrorMessages.UnhandledErrorOccured;
                response.comments = ex.StackTrace;
                response.errorMessage = ex.Message;

                return View();
            }
            finally
            {
                Mdl_Log log = new Mdl_Log(formControl.SampleTest.makerID, "SampleTest_Create"
                    , formControl.SampleTest.ToString(), response.ToString(), response.responseStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }

        [HttpPost]
        [AuthorizeUser(UserPermission.SampleTest_Create)]
        [Route("IsValidUSampleTest")]
        public JsonResult IsValidSampleTest(Vw_SampleTest formControl)
        {
            try
            {
                bool status = true;

                status = new Mdl_NameExist().validateNameExist(Mdl_NameExist.searchEntities.SAMPLE_TEST
                    , formControl.SampleTest.ID, formControl.SampleTest.name);

                return Json(status);
            }
            catch (Exception ex)
            {
                return Json("Data Base Connection issue Occured !");
            }
        }

        // GET: SampleTest/Search
        [AuthorizeUser(UserPermission.SampleTest_Search)]
        [Route("Search")]
        public ActionResult Search()
        {
            return View(new Vw_SampleTest()
            {
                lOVSampleTestCategories = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.Sample_Categories)
            });
        }

        [AuthorizeUser(UserPermission.SampleTest_Search)]
        [Route("searchSampleTest")]
        public ActionResult searchSampleTest(string searchName, string searchDescription, int searchSampleTestCategoryID
            , int searchIsActive)
        {
            ResponseMessage response = new ResponseMessage();
            List<Ent_SampleTest> searchResult = new List<Ent_SampleTest>();

            try
            {
                searchResult = new Mdl_SampleTest().searchSampleTest(searchName, searchDescription
                    , searchSampleTestCategoryID, searchIsActive);

                response.responseStatus = true;
                response.endUserMessage = Localization.Global.searchResultsRetrievedSucessfully;

                return PartialView("_searchResults", searchResult);
            }
            catch (Exception ex)
            {
                response.responseStatus = false;
                response.endUserMessage = Localization.ErrorMessages.UnhandledErrorOccured;
                response.comments = ex.StackTrace;
                response.errorMessage = ex.Message;

                return Json(new { Status = false, Message = Localization.ErrorMessages.UnhandledErrorOccured });
            }
            finally
            {
                Mdl_Log log = new Mdl_Log(User.Identity.Name, "SampleTest_Search", "Search name: " + searchName
                    + "~ Search Desc: " + searchDescription, response.ToString(), response.responseStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }

        [AuthorizeUser(UserPermission.SampleTest_View)]
        [Route("View")]
        public ActionResult View(int ID)
        {
            ResponseMessage response = new ResponseMessage();
            Ent_SampleTest SampleTest = new Ent_SampleTest();

            try
            {
                SampleTest = new Mdl_SampleTest().viewSampleTest(ID);

                response.responseStatus = true;
                response.endUserMessage = Localization.Global.ViewResultsRetrievedSucessfully;

                return PartialView("_viewSearchResult", SampleTest);
            }
            catch (Exception ex)
            {
                response.responseStatus = false;
                response.endUserMessage = Localization.ErrorMessages.UnhandledErrorOccured;
                response.comments = ex.StackTrace;
                response.errorMessage = ex.Message;

                return Json(new { Status = false, Message = Localization.ErrorMessages.UnhandledErrorOccured });
            }
            finally
            {
                Mdl_Log log = new Mdl_Log(User.Identity.Name, "SampleTest_View", "FT ID: " + ID
                    , response.ToString(), response.responseStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }

        [AuthorizeUser(UserPermission.SampleTest_Edit)]
        [Route("Edit/{ID?}")]
        public ActionResult Edit(int ID)
        {
            ResponseMessage response = new ResponseMessage();
            Ent_SampleTest SampleTest = new Ent_SampleTest();
            Vw_SampleTest vw_SampleTest = null;
            try
            {
                SampleTest = new Mdl_SampleTest().viewSampleTest(ID);
                if (SampleTest.name == null)
                {
                    Session["Status"] = response.responseStatus = false;
                    Session["EndMessage"] = response.endUserMessage = Localization.SampleTest.sampleTestID_Not_Available_inDB;
                }
                else
                {
                    Session["Status"] = response.responseStatus = true;
                    Session["EndMessage"] = response.endUserMessage = Localization.Global.ViewDataRetrievedSuccessfully;

                    vw_SampleTest = new Vw_SampleTest()
                    {
                        SampleTest = SampleTest
                    ,
                        lOVSampleTestCategories = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.Sample_Categories)
                    };

                    return View("Create", vw_SampleTest);
                }

                return View("Create", vw_SampleTest);
            }
            catch (Exception ex)
            {
                Session["Status"] = response.responseStatus = false;
                Session["EndMessage"] = response.endUserMessage = Localization.ErrorMessages.UnhandledErrorOccured;
                response.comments = ex.StackTrace;
                response.errorMessage = ex.Message;

                return View("Create");
            }
            finally
            {
                Mdl_Log log = new Mdl_Log(User.Identity.Name, "SampleTest_View", "MT ID: " + ID
                    , response.ToString(), response.responseStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }
    }
}