﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
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
    public class SnapShotVmControllerTests
    {
        private UserInfo _userInfo { get; set; }

        private ISnapshotVmService _snapshotVmService;

        private IUserVmService _userVmService;

        private IUserInfoProvider _userInfoProvider { get; set; }

        private SnapShotVmController _snapShotVmController { get; set; }

        [SetUp]
        public void Init()
        {
            AutoMapperConfiguration.Configure();
            _snapshotVmService = Substitute.For<ISnapshotVmService>();
            _userVmService = Substitute.For<IUserVmService>();
            _snapShotVmController = new SnapShotVmController(_snapshotVmService, _userVmService);
            _snapShotVmController.CrytexContext = Substitute.For<ICrytexContext>();

            _userInfo = new UserInfo() {UserId = "userId"};
            _userInfoProvider = Substitute.For<IUserInfoProvider>();
            _userInfoProvider.GetUserId().Returns(_userInfo.UserId);


            _snapShotVmController.CrytexContext.UserInfoProvider.Returns(_userInfoProvider);

            ControllerHelper.SetupControllerForTests(_snapShotVmController);
        }


        [Test]
        public void GetResponseBadRequestWhenCallGetWithInvalidParams()
        {
            Guid VmIdNull = Guid.Empty; //this true and for incorrect value
            var actionResult = _snapShotVmController.Get(VmIdNull) as BadRequestErrorMessageResult;

            IsNotNull(actionResult);
            AreEqual(actionResult.Message, "Incorrect VmID");


            Guid VmID = Guid.NewGuid();
            var VM = new UserVm
            {
                Id = VmID,
                UserId = _userInfo.UserId + "NotAccessUser"
            };
            _userInfoProvider.IsCurrentUserInRole("Admin").Returns(false);
            _userInfoProvider.IsCurrentUserInRole("Support").Returns(false);
            _userVmService.GetVmById(VmID).Returns(VM);

            actionResult = _snapShotVmController.Get(VmID) as BadRequestErrorMessageResult;
            IsNotNull(actionResult);
            AreEqual(actionResult.Message, "Are not allowed for this action");

        }

        [Test]
        public void GetResponseOkWithIEnumerableDataWhenCallGetWithValidParams()
        {
            Guid VmID = Guid.NewGuid();
            var snapShotVmRequests = (IEnumerable<SnapshotVm>) new List<SnapshotVm>()
            {
                new SnapshotVm() {Id = 1, VmId = VmID},
                new SnapshotVm() {Id = 2, VmId = VmID},
                new SnapshotVm() {Id = 3, VmId = VmID},
            };
            var snapShotVmRequestsView = (IEnumerable<SnapshotVmViewModel>) new List<SnapshotVmViewModel>()
            {
                new SnapshotVmViewModel() {Id = 1, VmId = VmID},
                new SnapshotVmViewModel() {Id = 2, VmId = VmID},
                new SnapshotVmViewModel() {Id = 3, VmId = VmID},
            };
            var VM = new UserVm
            {
                Id = VmID,
                UserId = _userInfo.UserId
            }; // valid user with access

            _userVmService.GetVmById(VmID).Returns(VM);

            _snapshotVmService.GetAllByVmId(VmID).Returns(snapShotVmRequests);

            var actionResult =
                _snapShotVmController.Get(VmID) as OkNegotiatedContentResult<IEnumerable<SnapshotVmViewModel>>;

            IsNotNull(actionResult);
            var model = actionResult.Content;
            IsNotNull(model);
            That(snapShotVmRequestsView.Select(x => x.Id), Is.EquivalentTo(model.Select(x => x.Id)));

            _snapshotVmService.Received(1).GetAllByVmId(VmID);
            _snapshotVmService.ClearReceivedCalls();

            ///////////////////////////////////////////////
            _userInfoProvider.IsCurrentUserInRole("Admin").Returns(true); // Admin User
            VM.UserId = "AnyAdminId";

            _userVmService.GetVmById(VmID).Returns(VM);

            _snapshotVmService.GetAllByVmId(VmID).Returns(snapShotVmRequests);

            actionResult =
                _snapShotVmController.Get(VmID) as OkNegotiatedContentResult<IEnumerable<SnapshotVmViewModel>>;

            IsNotNull(actionResult);
            model = actionResult.Content;
            IsNotNull(model);
            That(snapShotVmRequestsView.Select(x => x.Id), Is.EquivalentTo(model.Select(x => x.Id)));

            _snapshotVmService.Received(1).GetAllByVmId(VmID);
            _snapshotVmService.ClearReceivedCalls();

            ///////////////////////////////////////////////
            _userInfoProvider.IsCurrentUserInRole("Support").Returns(false); // Support User
            VM.UserId = "AnySupportId";

            _userVmService.GetVmById(VmID).Returns(VM);

            _snapshotVmService.GetAllByVmId(VmID).Returns(snapShotVmRequests);

            actionResult =
                _snapShotVmController.Get(VmID) as OkNegotiatedContentResult<IEnumerable<SnapshotVmViewModel>>;

            IsNotNull(actionResult);
            model = actionResult.Content;
            IsNotNull(model);
            That(snapShotVmRequestsView.Select(x => x.Id), Is.EquivalentTo(model.Select(x => x.Id)));

            _snapshotVmService.Received(1).GetAllByVmId(VmID);
            _snapshotVmService.ClearReceivedCalls();
        }

    }
}