﻿using KIDS.MOBILE.APP.Models.Login;
using System.Threading.Tasks;

namespace KIDS.MOBILE.APP.Services.Database
{
    public interface IDatabaseService
    {
        /// <summary>
        /// Lưu tài khoản người dùng vào database local
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<bool> SetAccountUser(UserModel user);

        /// <summary>
        /// lấy tài khoản người dùng từ database local
        /// </summary>
        /// <returns></returns>
        Task<UserModel> GetAccountUser();

        /// <summary>
        /// xóa tài khoản người dùng trong database local
        /// </summary>
        /// <returns></returns>
        Task DeleteAccontUser();

        /// <summary>
        /// cập tài khoản người dùng trong database local
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<bool> UpdateAccountUser(UserModel user);
    }
}