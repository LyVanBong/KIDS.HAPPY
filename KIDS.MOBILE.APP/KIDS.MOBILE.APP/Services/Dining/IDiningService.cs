using KIDS.MOBILE.APP.Models.Dining;
using KIDS.MOBILE.APP.Models.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KIDS.MOBILE.APP.Services.Dining
{
    public interface IDiningService
    {
        /// <summary>
        /// Cập nhật an uống các bữa
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userCreate"></param>
        /// <param name="mealComment0"></param>
        /// <param name="mealComment1"></param>
        /// <param name="mealComment2"></param>
        /// <param name="mealComment3"></param>
        /// <param name="mealComment4"></param>
        /// <param name="mealComment5"></param>
        /// <returns></returns>
        Task<ResponseModel<int>> UpdateDining(string id, string userCreate, string mealComment0, string mealComment1, string mealComment2, string mealComment3, string mealComment4, string mealComment5);

        /// <summary>
        /// lấy danh sách các mon ăn theo từng bữa
        /// </summary>
        /// <param name="buaAn"></param>
        /// <param name="khoi"></param>
        /// <param name="ngay"></param>
        /// <returns></returns>
        Task<ResponseModel<IEnumerable<ListOfDishesModel>>> GetListOfDishes(string buaAn, string khoi, string ngay);

        /// <summary>
        /// Lấy tên món ăn từ ID món ăn
        /// </summary>
        /// <param name="id">id món ăn</param>
        /// <returns></returns>
        Task<ResponseModel<IEnumerable<DishModel>>> GetDishDetail(string id);

        /// <summary>
        /// Danh sách các bữa ăn (tạo các tab bữa ăn)
        /// </summary>
        /// <returns></returns>
        Task<ResponseModel<IEnumerable<MealListModel>>> GetMealList();
    }
}