using FreeCourse.Services.PhotoStock.Dto;
using FreeCourse.Shared.ControllerBases;
using FreeCourse.Shared.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace FreeCourse.Services.PhotoStock.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PhotoController : CustomBaseController
    {


        [HttpPost]
        public async Task<IActionResult> Save(IFormFile photo, CancellationToken cancellationToken)
        {
            // dosya varsa
            if (photo != null && photo.Length > 0)
            {
                // endpoint i çağıran client bize form ismini de göndermeli
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/photos", photo.Name);

                // fotoyu path e kaydet
                #region stream nesnesini kullanım sonrası bellekten düşür
                using var stream = new FileStream(path, FileMode.Create);
                await photo.CopyToAsync(stream, cancellationToken);
                #endregion

                var returnPath = "photos/" + photo.Name;

                PhotoDto photoDto = new() { Url = returnPath };

                return CreateActionResultInstance(Response<PhotoDto>.Success(photoDto, 200));
            }
            return CreateActionResultInstance(Response<PhotoDto>.Fail("photo is empty", 400));
        }
    }
}
