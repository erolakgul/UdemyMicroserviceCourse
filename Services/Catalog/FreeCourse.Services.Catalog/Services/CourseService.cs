using AutoMapper;
using FreeCourse.Services.Catalog.Dto;
using FreeCourse.Services.Catalog.Model;
using FreeCourse.Services.Catalog.Services.Interfaces;
using FreeCourse.Services.Catalog.Settings;
using FreeCourse.Shared.Dtos;
using MongoDB.Driver;

namespace FreeCourse.Services.Catalog.Services
{
    public class CourseService : ICourseService
    {
        private readonly IMongoCollection<Category> _categoryCollection; // mongo erişim için
        private readonly IMongoCollection<Course> _courseCollection; 
        private readonly IMapper _mapper;  // dönüştürme için

        // alt + enter tuşları ile üstte yazdığımız parametreler için constructor ı inşa eder
        public CourseService(IMapper mapper, ICustomDatabaseSettings customDatabaseSettings)
        {
            #region mongo access
            var client = new MongoClient(customDatabaseSettings.ConnectionString);
            var database = client.GetDatabase(customDatabaseSettings.DatabaseName);
            _courseCollection = database.GetCollection<Course>(customDatabaseSettings.CourseCollectionName);
            _categoryCollection = database.GetCollection<Category>(customDatabaseSettings.CategoryCollectionName);
            #endregion

            _mapper = mapper;
        }


        public async Task<Response<List<CourseDto>>> GetAllAsync()
        {
            var courses = await _courseCollection.Find<Course>(c => true).ToListAsync();

            if (courses.Any())
            {
                foreach (var course in courses)
                {
                    course.Category = await _categoryCollection.Find(c => c.Id == course.CategoryId).FirstAsync();
                }
            }
            else
            {
                courses = new List<Course>();
            }

            // response un success static methodunu kullanıp, mapper üzerinden koleksiyonu dönüyoruz
            return Response<List<CourseDto>>.Success(_mapper.Map<List<CourseDto>>(courses), 200);
        }


        public async Task<Response<CourseDto>> GetByIdAsync(string id)
        {
            var course = await _courseCollection.Find(c => true && c.Id == id).FirstOrDefaultAsync();

            if (course is null)
            {
                return Response<CourseDto>.Fail("Course collection not found", 404);
            }
            // manuel olarak çekiyoruz yine
            course.Category = await _categoryCollection.Find<Category>(c=> c.Id == course.CategoryId).FirstOrDefaultAsync(); 

            // response un success static methodunu kullanıp, mapper üzerinden koleksiyonu dönüyoruz
            return Response<CourseDto>.Success(_mapper.Map<CourseDto>(course), 200);
        }

        public async Task<Response<List<CourseDto>>> GetAllByUserIdAsync(string userId)
        {
            var courses = await _courseCollection.Find(c => c.UserId == userId).ToListAsync();

            if (courses.Any())
            {
                foreach (var course in courses)
                {
                    course.Category = await _categoryCollection.Find(c => c.Id == course.CategoryId).FirstAsync();
                }
            }
            else
            {
                courses = new List<Course>();
            }

            // response un success static methodunu kullanıp, mapper üzerinden koleksiyonu dönüyoruz
            return Response<List<CourseDto>>.Success(_mapper.Map<List<CourseDto>>(courses), 200);
        }

        public async Task<Response<CourseDto>> CreateAsync(CourseCreateDto courseCreateDto)
        {
            var newCourse = _mapper.Map<Course>(courseCreateDto);
            newCourse.CreatedTime = DateTime.Now;

            await _courseCollection.InsertOneAsync(newCourse);

            return Response<CourseDto>.Success(_mapper.Map<CourseDto>(newCourse), 200);
        }

        public async Task<Response<NoContent>> UpdateAsync(CourseUpdateDto courseUpdateDto)
        {
            var updateCourse = _mapper.Map<Course>(courseUpdateDto);

            var result = await _courseCollection.FindOneAndReplaceAsync(x => x.Id == courseUpdateDto.Id,updateCourse);

            if (result is null)
            {
                return Response<NoContent>.Fail("Not found", 404);
            }
            return Response<NoContent>.Success(204);
        }

        public async Task<Response<NoContent>> DeleteAsync(string id)
        {
            var result = await _courseCollection.DeleteOneAsync<Course>(c=> c.Id == id);

            if (result.DeletedCount > 0)
            {
                return Response<NoContent>.Success(204);
            }
            return Response<NoContent>.Fail("Course not found",404);
        }
    }
}
