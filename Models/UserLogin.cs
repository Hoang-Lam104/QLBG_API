﻿namespace QLGB.API.Models;

public class UserLogin
{
    public string? Username { get; set; }
    public string? Password { get; set; }
    public string? AccessToken { get; set; }
}