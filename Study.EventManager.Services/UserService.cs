﻿using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Study.EventManager.Data.Contract;
using Study.EventManager.Model;
using Study.EventManager.Services.Contract;
using Study.EventManager.Services.Dto;
using Study.EventManager.Services.Exceptions;
using Study.EventManager.Services.Wrappers.Contracts;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;


namespace Study.EventManager.Services
{
    internal class UserService : IUserService
    {
        private IEmailWrapper _emailWrapper;
        private IContextManager _contextManager;

        public UserService(IContextManager contextManager, IEmailWrapper emailWrapper)
        {
            _emailWrapper = emailWrapper;
            _contextManager = contextManager;
        }

        public UserDto Authenticate(string email, string password)
        {
            var repo = _contextManager.CreateRepositiry<IUserRepo>();
            var user = repo.GetByUserName(email, password);

            if (user == null)
            {
                throw new ValidationException("Incorrect Username/Password combination");
            }

            if (!user.IsVerified)
            {
                throw new ValidationException("Email not verified");
            }


            ValidateUser(user.FirstName, user.LastName, user.Email);
            var result = MapToDto(user);
            return result;
        }

        public UserDto GetUser(int id)
        {
            var repo = _contextManager.CreateRepositiry<IUserRepo>();
            var data = repo.GetById(id);
            var result = MapToDto(data);
            return result;
        }

        public UserDto CreateUser(UserCreateDto dto)
        {
            ValidateUser(dto.FirstName, dto.LastName, dto.Email);

            User entity = new User(dto.Username, dto.Password, dto.FirstName, dto.LastName, dto.Email);
            var repo = _contextManager.CreateRepositiry<IUserRepo>();
            repo.Add(entity);
            _contextManager.Save();
            
            SendWelcomeEmail(dto);

            return MapToDto(entity);
        }

        public UserDto UpdateUser(int id, UserDto dto)
        {
            ValidateUser(dto.FirstName, dto.LastName, dto.Email);

            var repo = _contextManager.CreateRepositiry<IUserRepo>();
            var data = repo.GetById(id);

            data.FirstName = dto.FirstName;
            data.LastName = dto.LastName;
            data.Middlename = dto.Middlename;
            data.BirthDate = dto.BirthDate;
            data.Email = dto.Email;
            data.Phone = dto.Phone;
            data.Sex = dto.Sex;


            _contextManager.Save();
            return MapToDto(data);
        }

        public void DeleteUser(int id)
        {
            var repo = _contextManager.CreateRepositiry<IUserRepo>();
            var data = repo.GetById(id);
            var entity = repo.Delete(data);
            _contextManager.Save();
        }

        public IEnumerable<UserDto> GetAll()
        {
            var repo = _contextManager.CreateRepositiry<IUserRepo>();
            var data = repo.GetAll();
            return data.Select(x => MapToDto(x)).ToList();
        }

        //public void VerifyEmail(string email, string verificationCode)
        //{
        //    const string secretKey = "adsghfd;flasghsd;lgndfsgndfklngfsde";
        //    var email = "aaa@gmail.com";
        //    var validTo = "2021-05-20";
        //    var toVerifyCode = hash(email + "_" + validTo+ "_" + secretKey);

        //    "https://site.com/validateUser?email=aaa@gmail.com&validTo=2021-05-21&code={toVerifyCode}";
        //}

        public void ValidateUser(string FirstName, string LastName, string Email)
        {
            if (Email == null)
            {
                throw new ValidationException("Email is incorect");
            }

            if (string.IsNullOrEmpty(FirstName) || string.IsNullOrEmpty(LastName))
            {
                throw new ValidationException("FirstName or LastName incorect");
            }
        }

        private void SendWelcomeEmail(UserCreateDto dto)
        {
            string FilePath = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "WelcomeTemplate.html");
            StreamReader str = new StreamReader(FilePath);
            string MailText = str.ReadToEnd();
            str.Close();

            var mailText = MailText.Replace("[username]", dto.FirstName + dto.LastName).Replace("[email]", dto.Email);

            // send email 
            var emailModel = new EmailDto
            {
                Subject = $"Welcome {dto.Email}",
                Body = mailText,
                ToAddress = dto.Email,
                ToName = dto.Username
            };

            _emailWrapper.SendEmail(emailModel);
            
        }

        private UserDto MapToDto(User entity)
        {
            if (entity == null)
            {
                return null;
            }
            return new UserDto
            {
                Id = entity.Id,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                Email = entity.Email,
                Username = entity.Username
            };
        }
        private User MapToEntity(UserCreateDto dto)
        {
            return new User
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Username = dto.Username,
                Password = dto.Password
            };
        }
    }
}