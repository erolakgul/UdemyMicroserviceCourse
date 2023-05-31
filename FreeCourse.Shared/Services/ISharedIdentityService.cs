namespace FreeCourse.Shared.Services
{
    public interface ISharedIdentityService
    {
        // set eidlmeyecek sadece veri döndürecek
        public string? GetUserId { get;}
    }
}
