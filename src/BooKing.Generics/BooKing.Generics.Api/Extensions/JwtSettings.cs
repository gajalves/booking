﻿namespace BooKing.Generics.Api.Extensions;
public class JwtSettings
{
    public string Secret { get; set; }
    public int ExpiresInHours { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
}
