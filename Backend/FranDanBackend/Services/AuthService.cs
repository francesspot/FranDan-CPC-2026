using FranDanBackend;
using FranDanBackend.DTO;
using FranDanBackend.Models;
using Humanizer;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Net.Mail;

namespace FranDanBackend.Services
{
    public class AuthService
    {
        public MyContext context {  get; set; }
        public JWTGenerator generator { get; set; }
        public AuthService(MyContext _context, JWTGenerator _generator)
        {
            context = _context;
            generator = _generator;
        }
        public void addUser(AuthRegisterDTO dto)
        {
            if (context.existUserByUsername(dto.username))
                throw new Exception("User with this username exists.");
            if (context.existUserByEmail(dto.email))
                throw new Exception("User with this email exists.");
            User newUser =new User(
                dto.username.Trim(),
                dto.email.Trim().ToLower(),
                dto.emailNotifications,
                dto.occupation,
                JWTGenerator.createHash(dto.password),
                dto.birthday
                );
            MailSender.sendCode(newUser);
            context.Users.Add(newUser);
            context.SaveChanges();
        }
        public void verify(AuthVerifyDTO dto)
        {
            User foundUser = context.getUserByUsernameOrEmail(dto.usernameOrEmail);
            if (foundUser.verifier.verified) throw new Exception("You are already verified! Log in!");
            if (!foundUser.verifier.verify(dto.code)) throw new Exception("Wrong code!");
            context.SaveChanges();
        }
        public JwtDTO login(AuthLoginDTO dto)
        {
            User foundUser = context.getUserByUsernameOrEmail(dto.usernameOrEmail);
            if (!foundUser.verifier.verified) throw new Exception("Verify email before loging in!");
            if (!JWTGenerator.verifyHash(dto.password,foundUser.passwordHash)) throw new Exception("Wrong password!");
            JwtDTO jwt=new JwtDTO();
            jwt.jwtKey= generator.GenerateJWTToken(foundUser);
            return jwt;
        }
    }
}
