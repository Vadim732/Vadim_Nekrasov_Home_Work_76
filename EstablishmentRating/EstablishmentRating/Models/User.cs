﻿using Microsoft.AspNetCore.Identity;

namespace EstablishmentRating.Models;

public class User : IdentityUser<int>
{
    public string Avatar { get; set; }
    public DateTime DateOfBirth { get; set; }
}