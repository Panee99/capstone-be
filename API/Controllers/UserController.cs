﻿using API.Configurations.Filter;
using Application.Interfaces;
using Application.RequestModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.RequestModels.User;


namespace API.Controllers
{
    [Authentication]
    [Route("user")]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [Route("profile")]
        [HttpGet]
        public IActionResult GetProfile()
        {
            return _userService.GetProfile();
        }

        [Route("permissions")]
        [HttpGet]
        public IActionResult GetPermissions()
        {
            return _userService.GetPermissions();
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserModel model)
        {
            return await _userService.CreateUser(model);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser(UpdateUserModel model)
        {
            return await _userService.UpdateUser(model);
        }

        // [HttpDelete]
        // public async Task<IActionResult> RemoveUser(Guid id)
        // {
        //     return await _userService.RemoveUser(id);
        // }   

        [HttpDelete]
        public async Task<IActionResult> RemoveMulUser(List<Guid> ids)
        {
            return await _userService.RemoveMulUser(ids);
        }

        [Route("search")]
        [HttpPost]
        public IActionResult SearchUser(SearchUserModel model)
        {
            return _userService.SearchUser(model);
        }

        [Route("fetch")]
        [HttpPost]
        public IActionResult FetchUser(FetchModel model)
        {
            return _userService.FetchUser(model);
        }

        [Route("all")]
        [HttpGet]
        public IActionResult GetAllUser()
        {
            return _userService.GetAllUser();
        }

        [Route("me")]
        [HttpPut]
        public async Task<IActionResult> SelfUpdate(SelfUpdateModel model)
        {
            return await _userService.SelfUpdate(model);
        }

        [Route("authuser")]
        [HttpGet]
        public IActionResult GetAuthUser()
        {
            return _userService.GetAuthUser();
        }


        [HttpGet]
        public IActionResult GetUser(Guid id)
        {
            return _userService.GetUser(id);
        }

        [PermissionRequired("Permission.System.Master")]
        [Route("set-user-group")]
        [HttpPut]
        public async Task<IActionResult> SetUserGroup(SetUserPermissionModel model)
        {
            return await _userService.SetUserGroup(model);
        }

        [PermissionRequired("Permission.User.Update")]
        [Route("set-employee-group")]
        [HttpPut]
        public async Task<IActionResult> SetEmployeeGroup(  SetUserPermissionModel model)
        {
            return await _userService.SetEmployeeGroup(model);
        }
    }
}