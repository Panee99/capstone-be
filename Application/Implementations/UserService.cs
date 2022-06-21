﻿using Application.Interfaces;
using Application.RequestModels;
using Application.ViewModels;
using Data.Entities;
using Data_EF.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.RequestModels.User;
using Data.Implements;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Utilities.Constants;
using Utilities.Extensions;
using Utilities.Helper;

namespace Application.Implementations
{
    public class UserService : BaseService, IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IQueryable<User> _userQueryable;

        private readonly ILogger<UserService> _logger;

        public UserService(IServiceProvider provider, ILogger<UserService> logger) : base(provider)
        {
            _userRepository = _unitOfWork.User;
            _userQueryable = _userRepository.GetMany(x => x.IsDeleted != true);
            _logger = logger;
        }

        public IActionResult GetProfile()
        {
            var user = _userQueryable.Where(x => x.Id == CurrentUser.Id).Select(x => new ProfileViewModel
            {
                Id = x.Id,
                Email = x.Email,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Avatar = x.Avatar,
                Gender = x.Gender.ToString(),
                CreatedAt = x.CreatedAt,
                PhoneNumber = x.PhoneNumber,
                InWarehouseId = x.InWarehouseId
            }).FirstOrDefault();

            if (user == null) return ApiResponse.NotFound(MessageConstant.ProfileNotFound);

            return ApiResponse.Ok(user);
        }

        public IActionResult GetPermissions()
        {
            return ApiResponse.Ok(CurrentUser.Permissions);
        }

        public async Task<IActionResult> CreateUser(CreateUserModel model)
        {
            var user = _userQueryable.FirstOrDefault(x => x.Username == model.Username || x.Email == model.Email);
            if (user != null)
            {
                var msg = user.Username == model.Username ? MessageConstant.UserUsernameExisted : null;
                msg = user.Email == model.Email
                    ? (msg == null ? MessageConstant.UserEmailExisted : msg.Concat(MessageConstant.UserEmailExisted))
                    : null;
                return ApiResponse.BadRequest(msg);
            }

            var newUser = new User
            {
                Username = model.Username,
                Password = PasswordHelper.Hash(model.Password),
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                Gender = model.Gender
            };

            _userRepository.Add(newUser);

            await _unitOfWork.SaveChanges();

            return ApiResponse.Ok();
        }

        public async Task<IActionResult> UpdateUser(UpdateUserModel model)
        {
            var user = _userQueryable.FirstOrDefault(x => x.Id == model.Id);
            if (user == null) return ApiResponse.BadRequest(MessageConstant.UserNotFound);

            if (model.Email != null && model.Email != user.Email)
            {
                var userConflictEmail = _userQueryable.FirstOrDefault(x => x.Email == model.Email);
                if (userConflictEmail != null)
                {
                    return ApiResponse.BadRequest(MessageConstant.UserEmailExisted);
                }
            }

            user.Password = PasswordHelper.Hash(model.Password) ?? user.Password;
            user.Email = model.Email ?? user.Email;
            user.FirstName = model.FirstName ?? user.FirstName;
            user.LastName = model.LastName ?? user.LastName;
            user.PhoneNumber = model.PhoneNumber ?? user.PhoneNumber;
            user.Gender = model.Gender ?? user.Gender;
            user.IsActive = model.IsActive ?? user.IsActive;
            user.InWarehouseId = model.InWarehouseId ?? user.InWarehouseId;

            _userRepository.Update(user);
            await _unitOfWork.SaveChanges();

            return ApiResponse.Ok();
        }

        public async Task<IActionResult> RemoveUser(Guid id)
        {
            var user = _userQueryable.Where(x => x.Id == id).Include(x => x.UserInGroups).FirstOrDefault();
            if (user == null) return ApiResponse.BadRequest(MessageConstant.UserNotFound);

            user.IsDeleted = true;
            user.UserInGroups.Clear();

            _userRepository.Update(user);
            await _unitOfWork.SaveChanges();

            return ApiResponse.Ok();
        }

        public IActionResult SearchUser(SearchUserModel model)
        {
            var query = _userQueryable.Where(x =>
                    string.IsNullOrWhiteSpace(model.Name) || (x.FirstName + x.LastName).Contains(model.Name))
                .AsNoTracking();

            switch (model.OrderByName)
            {
                case "":
                    query = query.OrderByDescending(x => x.CreatedAt);
                    break;
                case "NAME":
                    query = model.IsSortAsc
                        ? query.OrderBy(x => x.FirstName).ThenByDescending(x => x.CreatedAt)
                        : query.OrderByDescending(x => x.FirstName).ThenByDescending(x => x.CreatedAt);
                    break;
                case "CREATEDAT":
                    query = model.IsSortAsc
                        ? query.OrderBy(x => x.CreatedAt)
                        : query.OrderByDescending(x => x.CreatedAt);
                    break;
                default:
                    return ApiResponse.BadRequest(MessageConstant.OrderByInvalid.WithValues("Name, CreatedAt"));
            }

            var data = query.Include(x => x.InWarehouse).Select(x => new SearchUserViewModel()
            {
                Id = x.Id,
                Username = x.Username,
                Email = x.Email,
                Gender = x.Gender,
                Password = x.Password,
                FirstName = x.FirstName,
                LastName = x.LastName,
                PhoneNumber = x.PhoneNumber,
                InWarehouseId = x.InWarehouseId != null
                    ? new FetchWarehouseViewModel() { Id = x.InWarehouseId!.Value, Name = x.InWarehouse.Name }
                    : null,
                IsActive = x.IsActive,
            }).ToPagination(model.PageIndex, model.PageSize);

            return ApiResponse.Ok(data);
        }

        public IActionResult FetchUser(FetchModel model)
        {
            var user = _userQueryable.AsNoTracking().Where(x =>
                    string.IsNullOrWhiteSpace(model.Keyword) || (x.FirstName + x.LastName).Contains(model.Keyword))
                .Take(model.Size).Select(x => new FetchUserViewModel()
                {
                    Id = x.Id,
                    Name = x.FirstName + x.LastName
                }).ToList();

            return ApiResponse.Ok(user);
        }

        public IActionResult GetAllUser()
        {
            var users = _userQueryable.Include(x => x.InWarehouse).Select(x => new UserViewModel()
            {
                Id = x.Id,
                Username = x.Username,
                Email = x.Email,
                Gender = x.Gender.ToString(),
                FirstName = x.FirstName,
                LastName = x.LastName,
                PhoneNumber = x.PhoneNumber,
                InWarehouse = x.InWarehouseId != null
                    ? new FetchWarehouseViewModel() { Id = x.InWarehouseId!.Value, Name = x.InWarehouse.Name }
                    : null,
                IsActive = x.IsActive,
            }).ToList();

            return ApiResponse.Ok(users);
        }

        public async Task<IActionResult> SelfUpdate(SelfUpdateModel model)
        {
            var user = _userQueryable.FirstOrDefault(x => x.Id == CurrentUser.Id);
            if (user == null) return ApiResponse.BadRequest(MessageConstant.UserNotFound);

            user.Password = PasswordHelper.Hash(model.Password) ?? user.Password;
            user.FirstName = model.FirstName ?? user.FirstName;
            user.LastName = model.LastName ?? user.LastName;
            user.PhoneNumber = model.PhoneNumber ?? user.PhoneNumber;
            user.Gender = model.Gender ?? user.Gender;

            _userRepository.Update(user);
            await _unitOfWork.SaveChanges();

            return ApiResponse.Ok();
        }

        public IActionResult GetAuthUser()
        {
            var user = _userQueryable.Include(x => x.UserInGroups).ThenInclude(x => x.Group)
                .FirstOrDefault(x => x.Id == CurrentUser.Id);

            if (user == null) return ApiResponse.Unauthorized();

            var groups = user.UserInGroups?.Select(x => x.Group).Select(x => new AuthUserGroup()
            {
                Name = x.Name,
                Type = x.Type.ToString()
            }).ToList() ?? new List<AuthUserGroup>();


            var authUser = new AuthUserViewModel
            {
                Avatar = user.Avatar,
                Name = user.FullName,
                Groups = groups
            };

            return ApiResponse.Ok(authUser);
        }

        public IActionResult GetUser()
        {
            var user = _userQueryable.Where(x => x.Id == CurrentUser.Id).Select(x => new UserViewModel()
            {
                Id = x.Id,
                Email = x.Email,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Gender = x.Gender.ToString(),
                PhoneNumber = x.PhoneNumber,
                InWarehouse = x.InWarehouseId != null
                    ? new FetchWarehouseViewModel
                    {
                        Id = x.InWarehouse.Id,
                        Name = x.InWarehouse.Name
                    }
                    : null
            }).FirstOrDefault();

            if (user == null) return ApiResponse.NotFound(MessageConstant.ProfileNotFound);

            return ApiResponse.Ok(user);
        }
    }
}