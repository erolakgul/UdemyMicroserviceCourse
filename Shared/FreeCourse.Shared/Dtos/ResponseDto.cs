using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FreeCourse.Shared.Dtos
{
    public class ResponseDto<T>   // T yi kısıtlamıyoruz
    {
        public T Data { get; private set; }

        [JsonIgnore] // yazılım içerisinde kullanıcam ama serialize edilmesine gerek yok
        public int StatusCode { get;private set; }
        [JsonIgnore]  // client a bunu da dönmeyeceğiz
        public bool IsSucces { get;private set; }
        public List<string>? Errors { get;private set; }

        // static factory pattern
        public static ResponseDto<T> Success(T data,int StatusCode)
        {
            return new ResponseDto<T> { Data = data, StatusCode = StatusCode , IsSucces = true};
        }

        public static ResponseDto<T> Success(int StatusCode)
        {
            return new ResponseDto<T> { Data = default(T), StatusCode = StatusCode, IsSucces = true };
        }

        public static ResponseDto<T> Fail(List<string> errors,int StatusCode)
        {
            return new ResponseDto<T> { Errors = errors, StatusCode = StatusCode, IsSucces = false };
        }

        public static ResponseDto<T> Fail(string error, int StatusCode)
        {
            return new ResponseDto<T> { Errors = new List<string>() { error}, StatusCode = StatusCode, IsSucces = false };
        }

    }
}
