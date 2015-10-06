using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
    public class RegionControllerTests
    {
        UserInfo _userInfo { get; set; }

        IUserInfoProvider _userInfoProvider { get; set; }

        IRegionService _regionService { get; set; }

        RegionController _regionController { get; set; }

        [SetUp]
        public void Init()
        {
            AutoMapperConfiguration.Configure();
            _regionService = Substitute.For<IRegionService>();
            _regionController = new RegionController(_regionService);
            _regionController.CrytexContext = Substitute.For<ICrytexContext>();

            _userInfo = new UserInfo() { UserId = "userId" };
            _userInfoProvider = Substitute.For<IUserInfoProvider>();
            _userInfoProvider.GetUserId().Returns(_userInfo.UserId);

            _regionController.CrytexContext.UserInfoProvider.Returns(_userInfoProvider);

            ControllerHelper.SetupControllerForTests(_regionController);
        }

        [Test]
        public void GetResponseOkWithListDataWhenCallGetWithValidParams()
        {
            var regions = (IEnumerable<Region>)new List<Region>()
                                    {
                                            new Region() {Id = 1},
                                            new Region() {Id = 2},
                                            new Region() {Id = 3},
                                    };
            var viewRegions = (IEnumerable<RegionViewModel>)new List<RegionViewModel>()
                                    {
                                            new RegionViewModel() {Id = 1},
                                            new RegionViewModel() {Id = 2},
                                            new RegionViewModel() {Id = 3},
                                    };
            _regionService.GetAllRegions().Returns(regions);

            var actionResult = _regionController.Get() as OkNegotiatedContentResult<IEnumerable<RegionViewModel>>;

            IsNotNull(actionResult);
            var model = actionResult.Content;
            IsNotNull(model);
            That(model.Select(x => x.Id), Is.EquivalentTo(viewRegions.Select(x => x.Id)));
            

            _regionService.Received(1).GetAllRegions();
            _regionService.ClearReceivedCalls();
        }

        [Test]
        public void GetResponseOkWithDataWhenCallGetWithValidId()
        {
            var id = 5;
            _regionService.GetRegionById(id).Returns(new Region() { Id = id });

            var actionResult = _regionController.Get(id) as OkNegotiatedContentResult<RegionViewModel>;

            IsNotNull(actionResult);
            var model = actionResult.Content;
            IsNotNull(model);
            AreEqual(id, model.Id);
        }

        [Test]
        public void GetResponseBadRequestWhenPostDataAndModelIsNotValid()
        {
            var regionViewModel = new RegionViewModel();
            _regionController.ModelState.AddModelError("keyError", "messageError");

            var actionResult = _regionController.Post(regionViewModel) as InvalidModelStateResult;

            IsNotNull(actionResult);
            var error = actionResult.ModelState["keyError"].Errors.Single(x => x.ErrorMessage == "messageError");
            IsNotNull(error);

            _regionController.ModelState.Clear();
        }

        [Test]
        public void PostModelAndGetResponseOkWhenTryPostModelWithValidModel()
        {
            var regionViewModel = new RegionViewModel { Id = 0, Area = "Area", Name = "Name", Enable = true};

            var regionAfter = new Region { Id = 5, Area = regionViewModel.Area, Name = regionViewModel.Name, Enable = regionViewModel.Enable};
            _regionService.CreateRegion(Arg.Is<Region>(x => (
                    x.Enable.Equals(regionViewModel.Enable) &&
                    x.Name.Equals(regionViewModel.Name) && 
                    x.Area.Equals(regionViewModel.Area))))
                .Returns(regionAfter);

            var actionResult = _regionController.Post(regionViewModel) as OkNegotiatedContentResult<Region>;

            IsNotNull(actionResult);
            _regionService.Received(1).CreateRegion(Arg.Any<Region>());
            _regionService.ClearReceivedCalls();
            var model = actionResult.Content;
            IsNotNull(model);
            AreEqual(regionAfter, model);
        }

        [Test]
        public void GetResponseBadRequestWhenTryPutNotValidData()
        {
            var id = 54;
            var regionViewModel = new RegionViewModel();
            _regionController.ModelState.AddModelError("keyError", "messageError");

            var actionResult = _regionController.Put(id,regionViewModel) as InvalidModelStateResult;

            IsNotNull(actionResult);
            var error = actionResult.ModelState["keyError"].Errors.Single(x => x.ErrorMessage == "messageError");
            IsNotNull(error);

            _regionController.ModelState.Clear();
        }

        [Test]
        public void PutModelAndGetResponseOkWhenTryPutWithValidModel()
        {
            var id = 54;
            var regionViewModel = new RegionViewModel() { Id = 0, Name = "Name"};

            Region model = null;
            _regionService.WhenForAnyArgs(x => x.UpdateRegion(id, null)).Do(x => { model = (Region)(x[1]); });

            var actionResult = _regionController.Put(id, regionViewModel) as OkResult;

            IsNotNull(actionResult);
            IsNotNull(model);
            AreEqual(model.Id, id);
            AreEqual(model.Name, regionViewModel.Name);

            _regionService.ClearReceivedCalls();
        }

        [Test]
        public void DeleteDataWhenTryDeleteData()
        {
            var id = 6;

            var actionResult = _regionController.Delete(id) as OkResult;

            IsNotNull(actionResult);
            _regionService.Received(1).DeleteRegionById(id);
            _regionService.ClearReceivedCalls();
        }




    }
}