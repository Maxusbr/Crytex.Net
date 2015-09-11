using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Hosting;
using System.Web.Http.Results;
using System.Web.Http.Routing;
using Crytex.Model.Models;
using Crytex.Service.IService;
using Crytex.Web;
using Crytex.Web.Controllers.Api;
using Crytex.Web.Mappings;
using Crytex.Web.Models.JsonModels;
using Crytex.Web.Service;
using NSubstitute;
using NUnit.Framework;
using PagedList;
using static NUnit.Framework.Assert;

namespace Crytex.Test.Controllers
{
    [TestFixture]
    public class HelpDeskRequestControllerTests
    {
        UserInfo _userInfo { get; set; }

        IUserInfoProvider _userInfoProvider { get; set; }

        IHelpDeskRequestService _helpDeskRequestService { get; set; }

        HelpDeskRequestController _helpDeskRequestController { get; set; }

        [SetUp]
        public void Init()
        {
            AutoMapperConfiguration.Configure();
            _helpDeskRequestService = Substitute.For<IHelpDeskRequestService>();
            _helpDeskRequestController = new HelpDeskRequestController(_helpDeskRequestService);
            _helpDeskRequestController.CrytexContext = Substitute.For<ICrytexContext>();

            _userInfo = new UserInfo() { UserId = "userId" };
            _userInfoProvider = Substitute.For<IUserInfoProvider>();
            _userInfoProvider.GetUserId().Returns(_userInfo.UserId);

            _helpDeskRequestController.CrytexContext.UserInfoProvider.Returns(_userInfoProvider);

            ControllerHelper.SetupControllerForTests(_helpDeskRequestController);
        }

       
        [Test]
        public void GetResponseBadRequestWhenCallGetWithInvalidParams()
        {
            var pageSize = 0;
            var pageIndex = 1;
            var actionResult = _helpDeskRequestController.Get(pageIndex, pageSize) as BadRequestErrorMessageResult;

            IsNotNull(actionResult);
            AreEqual(actionResult.Message, "PageNumber and PageSize must be grater than 1");

            pageSize = 1;
            pageIndex = 0;
            actionResult = _helpDeskRequestController.Get(pageIndex, pageSize) as BadRequestErrorMessageResult;

            IsNotNull(actionResult);
            AreEqual(actionResult.Message, "PageNumber and PageSize must be grater than 1");
        }

        [Test]
        public void GetResponseOkWithListDataWhenCallGetWithValidParams()
        {
            var pageSize = 5;
            var pageNumber = 1;
            var helpDeskRequests = new List<HelpDeskRequest>()
                                    {
                                            new HelpDeskRequest() {Id = 1},
                                            new HelpDeskRequest() {Id = 2},
                                            new HelpDeskRequest() {Id = 3},
                                    };
            _helpDeskRequestService.GetPage(pageNumber, pageSize).Returns(new PagedList<HelpDeskRequest>(helpDeskRequests, pageNumber, pageSize));

            var actionResult = _helpDeskRequestController.Get(pageNumber, pageSize) as OkNegotiatedContentResult<PageModel<HelpDeskRequestViewModel>>;

            IsNotNull(actionResult);
            var model = actionResult.Content;
            IsNotNull(model);
            That(helpDeskRequests.Select(x => x.Id), Is.EquivalentTo(model.Items.Select(x => x.Id).OrderBy(x => x)));

            _helpDeskRequestService.Received(1).GetPage(pageNumber, pageSize);
            _helpDeskRequestService.ClearReceivedCalls();
        }

        [Test]
        public void GetResponseOkWithDataWhenCallGetWithValidId()
        {
            var id = 5;
            _helpDeskRequestService.GeById(id).Returns(new HelpDeskRequest() { Id = id });

            var actionResult = _helpDeskRequestController.Get(id) as OkNegotiatedContentResult<HelpDeskRequestViewModel>;

            IsNotNull(actionResult);
            var model = actionResult.Content;
            IsNotNull(model);
            AreEqual(id, model.Id);
        }

        [Test]
        public void GetResponseBadRequestWhenPostDataAndModelIsNotValid()
        {
            var helpDeskRequestViewModel = new HelpDeskRequestViewModel();
            _helpDeskRequestController.ModelState.AddModelError("keyError", "messageError");

            var actionResult = _helpDeskRequestController.Post(helpDeskRequestViewModel) as InvalidModelStateResult;

            IsNotNull(actionResult);
            var error = actionResult.ModelState["keyError"].Errors.Single(x => x.ErrorMessage == "messageError");
            IsNotNull(error);

            _helpDeskRequestController.ModelState.Clear();
        }

        [Test]
        public void PostModelAndGetResponseOkWhenTryPostModelWithValidModel()
        {
            var helpDeskRequestViewModel = new HelpDeskRequestViewModel() { Summary = "summary", Details = "Details" };
            var helpDeskRequest = new HelpDeskRequest() { Id = 5, Summary = helpDeskRequestViewModel.Summary, Details = helpDeskRequestViewModel.Details };
            _helpDeskRequestService.CreateNew(helpDeskRequestViewModel.Summary, helpDeskRequestViewModel.Details, _userInfo.UserId).Returns(helpDeskRequest);

            var actionResult = _helpDeskRequestController.Post(helpDeskRequestViewModel);

            IsNotNull(actionResult);
            var model = actionResult.GetValueProperty("Content");
            IsNotNull(model);
            AreEqual(model.GetValueProperty("id"), helpDeskRequest.Id);
            AreEqual(actionResult.GetValueProperty("Location"), "/api/HelpDeskRequest/"+helpDeskRequest.Id);
            _helpDeskRequestService.Received(1).CreateNew(helpDeskRequestViewModel.Summary, helpDeskRequestViewModel.Details, _userInfo.UserId);
            _helpDeskRequestService.ClearReceivedCalls();
        }

        [Test]
        public void GetResponseBadRequestWhenTryPutNotValidData()
        {
            var id = 54;
            var helpDeskRequestViewModel = new HelpDeskRequestViewModel();
            _helpDeskRequestController.ModelState.AddModelError("keyError", "messageError");

            var actionResult = _helpDeskRequestController.Put(id, helpDeskRequestViewModel) as InvalidModelStateResult;

            IsNotNull(actionResult);
            var error = actionResult.ModelState["keyError"].Errors.Single(x => x.ErrorMessage == "messageError");
            IsNotNull(error);

            _helpDeskRequestController.ModelState.Clear();
        }

        [Test]
        public void PutModelAndGetResponseOkWhenTryPutWithValidModel()
        {
            var id = 54;
            var helpDeskRequestViewModel = new HelpDeskRequestViewModel() { Details = "Details" };

            HelpDeskRequest model = null;
            _helpDeskRequestService.WhenForAnyArgs(x => x.Update(null)).Do(x => { model = (HelpDeskRequest)(x[0]); });

            var actionResult = _helpDeskRequestController.Put(id, helpDeskRequestViewModel) as OkResult;

            IsNotNull(actionResult);
            IsNotNull(model);
            AreEqual(model.Id, id);
            AreEqual(model.Details, helpDeskRequestViewModel.Details);

            _helpDeskRequestService.ClearReceivedCalls();
        }

        [Test]
        public void DeleteDataWhenTryDeleteData()
        {
            var id = 6;

            var actionResult = _helpDeskRequestController.Delete(id) as OkResult;

            IsNotNull(actionResult);
            _helpDeskRequestService.Received(1).DeleteById(id);
            _helpDeskRequestService.ClearReceivedCalls();
        }




    }
}