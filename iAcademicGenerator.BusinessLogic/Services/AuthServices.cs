using iAcademicGenerator.DataAccess.Repositories.AUTH;
using iAcademicGenerator.Models.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using iAcademicGenerator.BusinessLogic.Configuration;

namespace iAcademicGenerator.BusinessLogic.Services
{
    public class AuthServices
    {
        private readonly AuthRepository _authRepository;

        public AuthServices(AuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        public ServiceResult Login(LoginRequestDTO loginRequest)
        {
            var result = new ServiceResult();

            try
            {
                // Validaciones
                if (loginRequest == null)
                    return result.BadRequest("Login data is required");

                if (string.IsNullOrWhiteSpace(loginRequest.user))
                    return result.BadRequest("User is required");

                if (string.IsNullOrWhiteSpace(loginRequest.password))
                    return result.BadRequest("Password is required");

                var loginResponse = _authRepository.Login(loginRequest);

                if (loginResponse == null)
                    return result.Error("Error connecting to database");

                switch (loginResponse.resultado)
                {
                    case "LOGIN_EXITOSO":
                        var token = GenerateJwtToken(loginResponse);

                        var jwtResponse = new JwtResponseDTO
                        {
                            token = token,
                            usu_codigo = loginResponse.usu_codigo,
                            usu_nombre = loginResponse.usu_nombre,
                            usu_email = loginResponse.usu_email,
                            tipo_usuario = loginResponse.tipo_usuario,
                            roles = loginResponse.roles,
                            expires_at = DateTime.UtcNow.AddHours(JwtSettings.ExpirationHours)
                        };

                        return result.Ok(jwtResponse);

                    case "USUARIO_NO_ENCONTRADO":
                    case "USUARIO_BLOQUEADO":
                    case "USUARIO_INACTIVO":
                    case "USUARIO_BLOQUEADO_POR_INTENTOS":
                    case "CREDENCIALES_INCORRECTAS":
                        // Usar Unauthorized en lugar de Error
                        result.Unauthorized(loginResponse.mensaje);
                        result.Data = new
                        {
                            Code = loginResponse.resultado,
                            Message = loginResponse.mensaje,
                            IntentosRestantes = loginResponse.intentos_restantes
                        };
                        return result;

                    default:
                        return result.Error("Unknown login result");
                }
            }
            catch (Exception ex)
            {
                return result.Error($"Unexpected error during login: {ex.Message}");
            }
        }

        private string GenerateJwtToken(LoginResponseDTO loginResponse)
        {
            var securityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(JwtSettings.Key)
            );
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, loginResponse.usu_codigo),
                new Claim(ClaimTypes.Name, loginResponse.usu_nombre),
                new Claim(ClaimTypes.Email, loginResponse.usu_email ?? ""),
                new Claim("tipo_usuario", loginResponse.tipo_usuario)
            };

            if (!string.IsNullOrEmpty(loginResponse.roles))
            {
                var roles = loginResponse.roles.Split(',');
                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role.Trim()));
                }
            }

            if (!string.IsNullOrEmpty(loginResponse.est_codigo))
                claims.Add(new Claim("est_codigo", loginResponse.est_codigo));

            if (!string.IsNullOrEmpty(loginResponse.doc_codigo))
                claims.Add(new Claim("doc_codigo", loginResponse.doc_codigo));

            var token = new JwtSecurityToken(
                issuer: JwtSettings.Issuer,
                audience: JwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(JwtSettings.ExpirationHours),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
