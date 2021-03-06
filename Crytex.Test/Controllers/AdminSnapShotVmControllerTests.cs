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
using Crytex.Core.Service;
using Crytex.Model.Models;
using Crytex.Service.IService;
using Crytex.Web;
using Crytex.Web.Areas.Admin;
using Crytex.Web.Areas.User;
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
    public class AdminSnapShotVmControllerTests
    {
        private ISnapshotVmService _snapshotVmService;

        private IUserVmService _userVmService;
        private string _userId;

        private IUserInfoProvider _userInfoProvider { get; set; }

        private AdminSnapShotVmController _snapShotVmController { get; set; }

        [SetUp]
        public void Init()
        {
            _userId = "userId";
            AutoMapperConfiguration.Configure();
            _snapshotVmService = Substitute.For<ISnapshotVmService>();
            _snapShotVmController = new AdminSnapShotVmController(_snapshotVmService);
            _snapShotVmController.CrytexContext = Substitute.For<ICrytexContext>();

            _userInfoProvider = Substitute.For<IUserInfoProvider>();
            _userInfoProvider.GetUserId().Returns(_userId);


            _snapShotVmController.CrytexContext.UserInfoProvider.Returns(_userInfoProvider);

            ControllerHelper.SetupControllerForTests(_snapShotVmController);
        }


        [Test]
        public void GetResponseBadRequestWhenCallGetWithInvalidParams()
        {
            int pageNumber = 1;
            int pageSize = 1;
            string VmIdIncorrect = "ahskdjghasdkjg"; //this true and for incorrect value
            var actionResultIncorrectId = _snapShotVmController.Get(pageNumber, pageSize, VmIdIncorrect) as InvalidModelStateResult;
            IsNotNull(actionResultIncorrectId);
            AreEqual(actionResultIncorrectId.ModelState.Keys.FirstOrDefault(), "id");
            AreEqual(actionResultIncorrectId.ModelState.Values.First().Errors.First().ErrorMessage, "Invalid Guid format");
        }

        [Test]
        public void GetResponseOkWithIEnumerableDataWhenCallGetWithValidParams()
        {
            int pageNumber = 1;
            int pageSize = 1;
            Guid VmID = Guid.NewGuid();
            var snapShotVmRequests = new List<SnapshotVm>()
            {
                new SnapshotVm() {Id = Guid.NewGuid(), VmId = VmID},
                new SnapshotVm() {Id = Guid.NewGuid(), VmId = VmID},
                new SnapshotVm() {Id = Guid.NewGuid(), VmId = VmID},
            };

            _snapshotVmService.GetAllByVmId(VmID, pageNumber, pageSize).Returns(new PagedList<SnapshotVm>(snapShotVmRequests, pageNumber, pageSize));

            var actionResult =
                _snapShotVmController.Get(pageNumber, pageSize, VmID.ToString()) as OkNegotiatedContentResult<PageModel<SnapshotVmViewModel>>;

            IsNotNull(actionResult);
            var model = actionResult.Content;
            IsNotNull(model);
            That(snapShotVmRequests.Select(x => x.Id), Is.EquivalentTo(model.Items.Select(x => x.Id)));

            _snapshotVmService.Received(1).GetAllByVmId(VmID, pageNumber, pageSize);
            _snapshotVmService.ClearReceivedCalls();
        }

    }
}