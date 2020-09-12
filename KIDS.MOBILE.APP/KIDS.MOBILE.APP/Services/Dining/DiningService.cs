using KIDS.MOBILE.APP.Models.Dining;
using KIDS.MOBILE.APP.Models.RequestProvider;
using KIDS.MOBILE.APP.Models.Response;
using KIDS.MOBILE.APP.Services.RequestProvider;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KIDS.MOBILE.APP.Services.Dining
{
    public class DiningService : IDiningService
    {
        private IRequestProvider _requestProvider;

        public DiningService(IRequestProvider requestProvider)
        {
            _requestProvider = requestProvider;
        }

        public async Task<ResponseModel<int>> UpdateDining(string id, string userCreate, string mealComment0, string mealComment1, string mealComment2,
            string mealComment3, string mealComment4, string mealComment5)
        {
            try
            {
                var parameters = new List<RequestParameter>()
                {
                    new RequestParameter("ID",id),
                    new RequestParameter("UserCreate",userCreate),
                    new RequestParameter("MealComment0",mealComment0),
                    new RequestParameter("MealComment1",mealComment1),
                    new RequestParameter("MealComment2",mealComment2),
                    new RequestParameter("MealComment3",mealComment3),
                    new RequestParameter("MealComment4",mealComment4),
                    new RequestParameter("MealComment5",mealComment5),
                };
                var data = await _requestProvider.PostAsync<int>("Daily/UpdateMeal", parameters);
                return data;
            }
            catch (Exception)
            {
                
                return null;
            }
        }

        public async Task<ResponseModel<IEnumerable<ListOfDishesModel>>> GetListOfDishes(string buaAn, string khoi, string ngay)
        {
            try
            {
                var parameters = new List<RequestParameter>
                {
                    new RequestParameter("BuaAn",buaAn),
                    new RequestParameter("Khoi",khoi),
                    new RequestParameter("Ngay",ngay),
                };
                var data = await _requestProvider.GetAsync<IEnumerable<ListOfDishesModel>>("Meal/Select/MonAn", parameters);
                return data;
            }
            catch (Exception)
            {
                
                return null;
            }
        }

        public async Task<ResponseModel<IEnumerable<DishModel>>> GetDishDetail(string id)
        {
            try
            {
                var parameter = new List<RequestParameter>
                {
                    new RequestParameter("ID",id),
                };
                var data = await _requestProvider.GetAsync<IEnumerable<DishModel>>("Meal/Select/TenMonAn", parameter);
                return data;
            }
            catch (Exception)
            {
                
                return null;
            }
        }

        public async Task<ResponseModel<IEnumerable<MealListModel>>> GetMealList()
        {
            try
            {
                var data = await _requestProvider.GetAsync<IEnumerable<MealListModel>>("Meal/Select");
                return data;
            }
            catch (Exception)
            {
                
                return null;
            }
        }
    }
}