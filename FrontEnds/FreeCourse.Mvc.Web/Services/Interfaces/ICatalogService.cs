using FreeCourse.Mvc.Web.Models.ViewModel.Catalog;

namespace FreeCourse.Mvc.Web.Services.Interfaces
{
    public interface ICatalogService
    {
        Task<List<CategoryViewModel>> GetAllCategoryAsync();
        Task<List<CourseViewModel>> GetAllCourseAsync();
        Task<List<CourseViewModel>> GetAllCourseByUserIdAsync(string userId);
        Task<CourseViewModel> GetByCourseIdAsync(string courseId);


        Task<bool> CreateCourseAsync(CourseCreateInput courseCreateInput); 
        Task<bool> UpdateCourseAsync(CourseUpdateInput courseUpdateInput);
        Task<bool> DeleteCourseAsync(string courseId);

    }
}
