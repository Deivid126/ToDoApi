﻿namespace ToDo.Application.DTOs
{
    public class UserRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsEdit { get; set; }
    }
}
