﻿using CleanArchitecture.Application.Abstractions;
using CleanArchitecture.Application.Features.AuthFeatures.Commands.Login;
using CleanArchitecture.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Infrastructure.Authentication
{
    public sealed class JwtProvider : IJwtProvider
    {
        private readonly JwtOptions _jwtOptions;
        private readonly UserManager<User> _userManager;

        public JwtProvider(IOptions<JwtOptions> jwtOptions, UserManager<User> userManager)
        {
            _jwtOptions = jwtOptions.Value;
            _userManager = userManager;
        }

        public async Task<LoginCommandResponse> CreateTokenAsync(User user)
        {
            var claims = new Claim[]
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Name, user.UserName),
                new Claim("NameLastName", user.NameLastName)
            };

            DateTime expires = DateTime.Now.AddHours(1);
            JwtSecurityToken jwtSecurityToken = new(
                issuer: "",
                audience: "",
                claims: claims,
                notBefore: DateTime.Now,//tokenın ne zaman oluşacağını belirliyor
                expires: expires,//tokenın ne kadar süre geçerli olduğunu belirliyor
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey
                (Encoding.UTF8.GetBytes
                (_jwtOptions.SecretKey)),SecurityAlgorithms.HmacSha256));

            string token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            string refreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpires = expires.AddMinutes(15);
            await _userManager.UpdateAsync(user);

            LoginCommandResponse response = new(
                token,
                refreshToken,
                user.RefreshTokenExpires,
                user.Id,
                user.UserName,
                user.NameLastName,
                user.Email);

            return response;
        }
    }
}
