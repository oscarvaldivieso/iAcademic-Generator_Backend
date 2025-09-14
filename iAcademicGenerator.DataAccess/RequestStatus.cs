using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iAcademicGenerator.DataAccess
{
    // Enums para códigos de estado más específicos
    public enum StatusCode
    {
        Success = 200,
        Created = 201,
        BadRequest = 400,
        NotFound = 404,
        Conflict = 409,
        InternalServerError = 500,
        DatabaseError = 501,
        ValidationError = 422
    }

    // RequestStatus mejorado
    public class RequestStatus
    {
        public StatusCode CodeStatus { get; set; }
        public string MessageStatus { get; set; }
        public bool IsSuccess => CodeStatus == StatusCode.Success || CodeStatus == StatusCode.Created;
        public List<string> Errors { get; set; } = new List<string>();

        public RequestStatus()
        {
        }

        public RequestStatus(StatusCode code, string message)
        {
            CodeStatus = code;
            MessageStatus = message;
        }

        public static RequestStatus Success(string message = "Operación exitosa")
            => new(StatusCode.Success, message);

        public static RequestStatus Created(string message = "Registro creado exitosamente")
            => new(StatusCode.Created, message);

        public static RequestStatus Error(StatusCode code, string message, List<string> errors = null)
            => new(code, message) { Errors = errors ?? new List<string>() };

        public static RequestStatus NotFound(string message = "Registro no encontrado")
            => new(StatusCode.NotFound, message);

        public static RequestStatus DatabaseError(string message = "Error en la base de datos")
            => new(StatusCode.DatabaseError, message);

        public static RequestStatus ValidationError(string message, List<string> errors = null)
            => new(StatusCode.ValidationError, message) { Errors = errors ?? new List<string>() };
    }

    // Para operaciones que necesitan devolver datos
    public class RequestStatus<T> : RequestStatus
    {
        public T Data { get; set; }

        public RequestStatus() : base()
        {
        }

        public RequestStatus(StatusCode code, string message, T data = default) : base(code, message)
        {
            Data = data;
        }

        public static RequestStatus<T> Success(T data, string message = "Operación exitosa")
            => new(StatusCode.Success, message, data);

        public static RequestStatus<T> Created(T data, string message = "Registro creado exitosamente")
            => new(StatusCode.Created, message, data);

        public static new RequestStatus<T> Error(StatusCode code, string message, List<string> errors = null)
            => new(code, message) { Errors = errors ?? new List<string>() };

        public static new RequestStatus<T> NotFound(string message = "Registro no encontrado")
            => new(StatusCode.NotFound, message);

        public static new RequestStatus<T> DatabaseError(string message = "Error en la base de datos")
            => new(StatusCode.DatabaseError, message);
    }
}
