﻿namespace DotNetCore_New.Model
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int UserTypeId { get; set; }
        public bool IsActive { get; set; }

    }
}
