﻿using Ogu.Response.Abstractions;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ogu.Response.Json
{
    public class JsonResponse<T> : IJsonResponse<T>
    {
        [JsonConstructor]
        public JsonResponse(T data, bool success, HttpStatusCode status, IDictionary<string, object> extensions, List<IResponseError> errors, object serializedResponse, JsonSerializerOptions serializerOptions)
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
        public T Data { get; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<IResponseError> Errors { get; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IDictionary<string, object> Extensions { get; }

        public JsonSerializerOptions SerializerOptions { get; }

        public static JsonResponse<T> Failure(HttpStatusCode statusCode, List<IResponseError> errors, JsonSerializerOptions serializerOptions = null)
        {
            return new JsonResponse<T>(default, false, statusCode, null, errors, null, serializerOptions);
        }

        public static implicit operator JsonResponse(JsonResponse<T> response)
        {
            return new JsonResponse(response.Data, response.Success, response.Status, response.Extensions, response.Errors, response.SerializedResponse, response.SerializerOptions);
        }
    }
}