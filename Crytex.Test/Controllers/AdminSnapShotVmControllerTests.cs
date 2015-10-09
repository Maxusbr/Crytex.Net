using System;
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
        private UserInfo _userInfo { get; set; }

        private ISnapshotVmService _snapshotVmService;

        private IUserVmService _userVmService;

        private IUserInfoProvider _userInfoProvider { get; set; }

        private AdminSnapShotVmController _snapShotVmController { get; set; }

        [SetUp]
        public void Init()
        {
            AutoMapperConfiguration.Configure();
            _snapshotVmService = Substitute.For<ISnapshotVmService>();
            _userVmService = Substitute.For<IUserVmService>();
            _snapShotVmController = new AdminSnapShotVmController(_snapshotVmService, _userVmService);
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
            string VmIdIncorrect = "ahskdjghasdkjg"; //this true and for incorrect value
            var actionResultIncorrectId = _snapShotVmController.Get(VmIdIncorrect) as InvalidModelStateResult;
            IsNotNull(actionResultIncorrectId);
            AreEqual(actionResultIncorrectId.ModelState.Keys.FirstOrDefault(), "id");
            AreEqual(actionResultIncorrectId.ModelState.Values.First().Errors.First().ErrorMessage, "Invalid Guid format");
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

            _snapshotVmService.GetAllByVmId(VmID).Returns(snapShotVmRequests);

            var actionResult =
                _snapShotVmController.Get(VmID.ToString()) as OkNegotiatedContentResult<IEnumerable<SnapshotVmViewModel>>;

            IsNotNull(actionResult);
            var model = actionResult.Content;
            IsNotNull(model);
            That(snapShotVmRequestsView.Select(x => x.Id), Is.EquivalentTo(model.Select(x => x.Id)));

            _snapshotVmService.Received(1).GetAllByVmId(VmID);
            _snapshotVmService.ClearReceivedCalls();
        }

    }
}