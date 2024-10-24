﻿using Ogu.Response.Abstractions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;

namespace Ogu.Response.Json
{
    public class JsonResponse : IJsonResponse
    {
        [JsonConstructor]
        public JsonResponse(object data, bool success, HttpStatusCode status, IDictionary<string, object> extensions, List<IResponseError> errors, object serializedResponse, JsonSerializerOptions serializerOptions)
        {
            Data = data;
            Success = success;
            Status = status;
            Errors = errors ?? new List<IResponseError>();
            Extensions = extensions ?? new Dictionary<string, object>();
            SerializedResponse = serializedResponse;
            SerializerOptions = serializerOptions ?? Constants.DefaultJsonSerializerOptions;
        }

        public bool Success { get; }

        public HttpStatusCode Status { get; }

        [JsonIgnore]
        public object SerializedResponse { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public object Data { get; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<IResponseError> Errors { get; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IDictionary<string, object> Extensions { get; }
        
        public JsonSerializerOptions SerializerOptions { get; }

        public static JsonResponse Failure(HttpStatusCode statusCode, List<IResponseError> errors, JsonSerializerOptions serializerOptions = null)
        {
            return new JsonResponse(null, false, statusCode, null, errors, null, serializerOptions);
        }
    }
}