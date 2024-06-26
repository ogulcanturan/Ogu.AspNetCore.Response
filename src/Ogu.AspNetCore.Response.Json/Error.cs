﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace Ogu.AspNetCore.Response.Json
{
    public class Error : IError
    {
        public Error(string title, string description, string details, string code, string helpLink, IValidationFailure[] validationFailures, ErrorType type)
        {
            Title = title;
            Description = description;
            Details = details;
            Code = code; 
            HelpLink = helpLink;
            ValidationFailures = validationFailures;
            Type = type;
        }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Title { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Description { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Details { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Code { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string HelpLink { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IValidationFailure[] ValidationFailures { get; set; }

        public ErrorType Type { get; set; }

        public static IErrorBuilder Builder => new ErrorBuilder();

        public static IError Validation(string title, string description, params IValidationFailure[] validationFailures)
            => Builder
                .WithTitle(title)
                .WithDescription(description)
                .WithValidationFailures(validationFailures)
                .Build();

        public static IError Custom(string title, string description = null, string details = null, string code = null, string helpLink = null)
            => Builder
                .WithTitle(title)
                .WithDescription(description)
                .WithDetails(details)
                .WithCode(code)
                .WithHelpLink(helpLink)
                .WithErrorType(ErrorType.Custom)
                .Build();

        public static IError Custom<TEnum>(TEnum @enum, string description, string details, string helpLink) where TEnum : struct, Enum
            => Builder
                .WithTitle(@enum.ToString())
                .WithDescription(description ?? ResponseExtensions.GetDescription(@enum))
                .WithDetails(details)
                .WithCode(ResponseExtensions.GetValue(@enum).ToString())
                .WithHelpLink(helpLink ?? ResponseExtensions.GetHelpLink(@enum))
                .WithErrorType(ErrorType.Custom)
                .Build();

        public static IError[] Custom<TEnum>(params TEnum[] @enums) where TEnum : struct, Enum
            => @enums.Select(e => 
                    Builder
                        .WithTitle(e.ToString())
                        .WithDescription(ResponseExtensions.GetDescription(e))
                        .WithCode(ResponseExtensions.GetValue(e).ToString())
                        .WithHelpLink(ResponseExtensions.GetHelpLink(e))
                        .WithErrorType(ErrorType.Custom)
                        .Build()).ToArray();

        public static IError[] Custom<TEnum>(IList<TEnum> @enums) where TEnum : struct, Enum
            => @enums.Select(e =>
                Builder
                    .WithTitle(e.ToString())
                    .WithDescription(ResponseExtensions.GetDescription(e))
                    .WithCode(ResponseExtensions.GetValue(e).ToString())
                    .WithHelpLink(ResponseExtensions.GetHelpLink(e))
                    .WithErrorType(ErrorType.Custom)
                    .Build()).ToArray();

        public static IError Exception(Exception exception, bool includeTraces = false)
        {
            var builder = Builder
                .WithTitle(exception.GetType().Name)
                .WithDescription(exception.Message)
                .WithCode(exception.HResult.ToString())
                .WithHelpLink(exception.HelpLink)
                .WithErrorType(ErrorType.Exception);

            if (includeTraces)
                builder = builder.WithDetails(exception.ToString());

            return builder.Build();
        }

        public static IError[] Exception(Exception[] exceptions, bool includeTraces = false)
        {
            return exceptions.Select(e => Exception(e, includeTraces)).ToArray();
        }

        public static IError[] Exception(IList<Exception> exceptions, bool includeTraces = false)
        {
            return exceptions.Select(e => Exception(e, includeTraces)).ToArray();
        }
    }
}