﻿namespace QLGB.API.Models;

public class UserLogin
{
    public int UserId { get; set;}
    public string? Username { get; set; }
    public string? AccessToken { get; set; }
}
