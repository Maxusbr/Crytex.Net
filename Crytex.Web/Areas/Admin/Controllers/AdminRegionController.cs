﻿using System;
using Crytex.Model.Models;
using Crytex.Service.IService;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Crytex.Web.Models.JsonModels;

namespace Crytex.Web.Areas.Admin
{
    public class AdminRegionController : AdminCrytexController
    {
        private readonly IRegionService _regionService;

        public AdminRegionController(IRegionService regionService)
        {
            this._regionService = regionService;
        }

        /// <summary>
        /// Получение всех Регионов
        /// </summary>
        /// <returns></returns>
        // GET: api/Region
        [ResponseType(typeof(IEnumerable<RegionViewModel>))]
        public IHttpActionResult Get()
        {
            var regions = this._regionService.GetAllRegions();
            var viewModel = AutoMapper.Mapper.Map<IEnumerable<RegionViewModel>>(regions);

            return Ok(viewModel);
        }

        /// <summary>
        /// Получение всех Регионов по id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: api/Region/5
        [ResponseType(typeof(RegionViewModel))]
        public IHttpActionResult Get(int id)
        {
            var region = this._regionService.GetRegionById(id);
            var viewModel = AutoMapper.Mapper.Map<RegionViewModel>(region);

            return Ok(viewModel);
        }

        /// <summary>
        /// Создание нового Региона
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        // POST: api/Region
        public IHttpActionResult Post([FromBody]RegionViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var region = AutoMapper.Mapper.Map<Region>(model);
            var newRegion = this._regionService.CreateRegion(region);

            return Ok(newRegion);
        }

        /// <summary>
        /// Обновление Региона
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        // PUT: api/Region/5
        public IHttpActionResult Put(int id,[FromBody]RegionViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var region = AutoMapper.Mapper.Map<Region>(model);
            region.Id = id;
            this._regionService.UpdateRegion(id, region);

            return Ok();
        }

        /// <summary>
        /// Удаление Региона по id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // DELETE: api/Region/5
        public IHttpActionResult Delete(int id)
        {
            this._regionService.DeleteRegionById(id);
            return Ok();
        }
    }
}
