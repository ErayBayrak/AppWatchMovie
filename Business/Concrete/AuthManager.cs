using Business.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
	public class AuthManager : IAuthService
	{
		
		private readonly IUserService _userService;

		public AuthManager(IUserService userService)
        {
			_userService = userService;
		}
        public User Login(UserForLoginDto request)
		{
			var userToCheck = _userService.GetByMail(request.Email);
			
			if (userToCheck.Email != request.Email)
			{
				throw new Exception("User not found.");
			}

			if (!VerifyPasswordHash(request.Password, userToCheck.PasswordHash, userToCheck.PasswordSalt))
			{
				throw new Exception("Wrong password.");
			}
			return userToCheck;
		}

		public User Register(UserForRegisterDto request)
		{
			CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

			var user = new User
			{
				Email = request.Email,
				FirstName = request.FirstName,
				LastName = request.LastName,
				PasswordHash = passwordHash,
				PasswordSalt = passwordSalt,
				Status = true
			};

			_userService.Add(user);
			return user;
		}
		//todo
		public void UserExists(string email)
		{
			throw new NotImplementedException();
		}
		

		private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
		{
			using (var hmac = new HMACSHA512())
			{
				passwordSalt = hmac.Key;
				passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
			}
		}
		private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
		{
			using (var hmac = new HMACSHA512(passwordSalt))
			{
				var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
				return computedHash.SequenceEqual(passwordHash);
			}
		}
	}
}
