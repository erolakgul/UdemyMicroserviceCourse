using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FreeCourse.Shared.Dtos
{
    public class Response<T>   // T yi kısıtlamıyoruz
    {
        public T Data { get; private set; }

        [JsonIgnore] // yazılım içerisinde kullanıcam ama serialize edilmesine gerek yok
        public int StatusCode { get;private set; }
        [JsonIgnore]  // client a bunu da dönmeyeceğiz
        public bool IsSucces { get;private set; }
        public List<string>? Errors { get;private set; }

        // static factory pattern
        public static Response<T> Success(T data,int StatusCode)
        {
            return new Response<T> { Data = data, StatusCode = StatusCode , IsSucces = true};
        }

        public static Response<T> Success(int StatusCode)
        {
            return new Response<T> { Data = default(T), StatusCode = StatusCode, IsSucces = true };
        }

        public static Response<T> Fail(List<string> errors,int StatusCode)
        {
            return new Response<T> { Errors = errors, StatusCode = StatusCode, IsSucces = false };
        }

        public static Response<T> Fail(string error, int StatusCode)
        {
            return new Response<T> { Errors = new List<string>() { error}, StatusCode = StatusCode, IsSucces = false };
        }

    }
}
