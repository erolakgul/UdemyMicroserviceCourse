using AutoMapper;
using FreeCourse.Services.Catalog.Dto;
using FreeCourse.Services.Catalog.Model;
using FreeCourse.Services.Catalog.Services.Interfaces;
using FreeCourse.Services.Catalog.Settings;
using FreeCourse.Shared.Dtos;
using MongoDB.Driver;

namespace FreeCourse.Services.Catalog.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IMongoCollection<Category> _categoryCollection; // mongo erişim için
        private readonly IMapper _mapper;  // dönüştürme için

        // alt + enter tuşları ile üstte yazdığımız parametreler için constructor ı inşa eder
        public CategoryService(IMapper mapper,ICustomDatabaseSettings customDatabaseSettings)
        {
            #region mongo access
            var client = new MongoClient(customDatabaseSettings.ConnectionString);
            var database = client.GetDatabase(customDatabaseSettings.DatabaseName);
            _categoryCollection = database.GetCollection<Category>(customDatabaseSettings.CategoryCollectionName);
            #endregion

            _mapper = mapper;
        }

        public async Task<Response<List<CategoryDto>>> GetAllAsync()
        {
            var collection = await _categoryCollection.Find(c => true).ToListAsync();
            // response un success static methodunu kullanıp, mapper üzerinden koleksiyonu dönüyoruz
            return Response<List<CategoryDto>>.Success(_mapper.Map<List<CategoryDto>>(collection),200);
        }

        public async Task<Response<CategoryDto>> CreateAsync(CategoryCreateDto categoryCreateDto)
        {
            var newCategory = _mapper.Map<Category>(categoryCreateDto);

            await _categoryCollection.InsertOneAsync(newCategory);

            return Response<CategoryDto>.Success(_mapper.Map<CategoryDto>(newCategory), 200);
        }

        public async Task<Response<CategoryDto>> GetByIdAsync(string id)
        {
            var collection = await _categoryCollection.Find(c => true && c.Id == id).FirstOrDefaultAsync();

            if (collection is null)
            {
                return Response<CategoryDto>.Fail("Category collection not found", 404);
            }

            // response un success static methodunu kullanıp, mapper üzerinden koleksiyonu dönüyoruz
            return Response<CategoryDto>.Success(_mapper.Map<CategoryDto>(collection), 200);
        }

    }
}
