﻿using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Ogu.AspNetCore.Response.Json
{
    public class Result : IResult
    {
        public Result(string title, string detail, int? status, string type, string instance, string code, IDictionary<string, object> extensions)
        {
            Title = title;
            Detail = detail;
            Status = status;
            Type = type;
            Instance = instance;
            Code = code;
            Extensions = extensions;
        }

        /// <summary>
        /// A URI reference that identifies the result type.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Type { get; set; }

        /// <summary>
        /// A short, human-readable title for the result.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Title { get; set; }

        /// <summary>
        /// The HTTP status code associated with the result.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? Status { get; set; }

        /// <summary>
        /// The code associated with the result.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Code { get; set; }

        /// <summary>
        /// A detailed description or explanation of the result
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Detail { get; set; }

        /// <summary>
        /// The URI of the specific instance where the result occurred.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Instance { get; set; }

        /// <summary>
        /// Additional custom properties or information associated with the result.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IDictionary<string, object> Extensions { get; set; }

        public static ResultBuilder Builder => new ResultBuilder();

        public static IResult ValidationFailure(IValidationFailure[] validationFailures, string instance = null,
            string type = null, string code = null, int? status = 400, string title = "Bad Request",
            string detail = "Validation failed.")
            => Builder
                .WithErrors(Error.Validation(null, null, validationFailures))
                .WithInstance(instance)
                .WithType(type)
                .WithCode(code)
                .WithStatus(status)
                .WithType(type)
                .WithTitle(title)
                .WithDetail(detail).Build();

        public static IResult BasicFailure<TEnum>(TEnum @enum, string instance = null,
            string type = null, int? status = 400, string title = "Bad Request",
            string detail = "Custom failure occurred.")
            where TEnum : struct, System.Enum
            => Builder
                .WithInstance(instance)
                .WithType(type)
                .WithStatus(status)
                .WithTitle(title)
                .WithDetail(detail)
                .WithErrors(Error.Basic(@enum, null))
                .Build();
    }
}