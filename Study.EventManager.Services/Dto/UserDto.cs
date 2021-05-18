﻿using Study.EventManager.Model.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Study.EventManager.Services.Dto
{
    public class UserDto
    { 
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Middlename { get; set; }

        public DateTime BirthDate { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public UserSex Sex { get; set; }

        public string Username { get; set; }
    }
}