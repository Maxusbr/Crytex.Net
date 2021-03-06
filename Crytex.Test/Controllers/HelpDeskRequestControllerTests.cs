﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Results;
using Crytex.Core.Service;
using Crytex.Model.Models;


using Crytex.Web.Mappings;
using Crytex.Web.Models.JsonModels;
using NSubstitute;
using NUnit.Framework;
using PagedList;
using static NUnit.Framework.Assert;
using Crytex.Web.Areas.User;
using Crytex.Service.IService;

namespace Crytex.Test.Controllers
{
    [TestFixture]
    public class HelpDeskRequestControllerTests
    {
        private string _userId;

        IUserInfoProvider _userInfoProvider { get; set; }

        IHelpDeskRequestService _helpDeskRequestService { get; set; }

        HelpDeskRequestController _helpDeskRequestController { get; set; }

        [SetUp]
        public void Init()
        {
            _userId = "userId";
            AutoMapperConfiguration.Configure();
            _helpDeskRequestService = Substitute.For<IHelpDeskRequestService>();
            _helpDeskRequestController = new HelpDeskRequestController(_helpDeskRequestService);
            _helpDeskRequestController.CrytexContext = Substitute.For<ICrytexContext>();

            _userInfoProvider = Substitute.For<IUserInfoProvider>();
            _userInfoProvider.GetUserId().Returns(_userId);

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
            // TODO: Refactor this test
            throw new NotImplementedException();
            var pageSize = 5;
            var pageNumber = 1;
            var helpDeskRequests = new List<HelpDeskRequest>()
                                    {
                                            new HelpDeskRequest() {Id = 1},
                                            new HelpDeskRequest() {Id = 2},
                                            new HelpDeskRequest() {Id = 3},
                                    };
            //_helpDeskRequestService.GetPage(pageNumber, pageSize).Returns(new PagedList<HelpDeskRequest>(helpDeskRequests, pageNumber, pageSize));

            var actionResult = _helpDeskRequestController.Get(pageNumber, pageSize) as OkNegotiatedContentResult<PageModel<HelpDeskRequestViewModel>>;

            IsNotNull(actionResult);
            var model = actionResult.Content;
            IsNotNull(model);
            That(helpDeskRequests.Select(x => x.Id), Is.EquivalentTo(model.Items.Select(x => x.Id).OrderBy(x => x)));

            //_helpDeskRequestService.Received(1).GetPage(pageNumber, pageSize);
            _helpDeskRequestService.ClearReceivedCalls();
        }

        [Test]
        public void GetResponseOkWithDataWhenCallGetWithValidId()
        {
            var id = 5;
            _helpDeskRequestService.GetById(id).Returns(new HelpDeskRequest() { Id = id });

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
            var returnedHelpDeskRequest = new HelpDeskRequest() { Id = 5, Summary = helpDeskRequestViewModel.Summary, Details = helpDeskRequestViewModel.Details };
            var requestToCreate = new HelpDeskRequest
            {
                Summary = helpDeskRequestViewModel.Summary,
                Details = helpDeskRequestViewModel.Details,
                UserId =  _userId
            };
            _helpDeskRequestService.CreateNew(requestToCreate).Returns(returnedHelpDeskRequest);

            var actionResult = _helpDeskRequestController.Post(helpDeskRequestViewModel);

            IsNotNull(actionResult);
            var model = actionResult.GetValueProperty("Content");
            IsNotNull(model);
            AreEqual(model.GetValueProperty("id"), returnedHelpDeskRequest.Id);
            AreEqual(actionResult.GetValueProperty("Location"), "/api/HelpDeskRequest/"+returnedHelpDeskRequest.Id);
            _helpDeskRequestService.Received(1).CreateNew(requestToCreate);
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