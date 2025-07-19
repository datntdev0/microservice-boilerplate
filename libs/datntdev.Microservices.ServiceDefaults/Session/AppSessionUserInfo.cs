﻿namespace datntdev.Microservices.ServiceDefaults.Session
{
    public class AppSessionUserInfo
    {
        public long UserId { get; set; }
        public string EmailAddress { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
    }
}
