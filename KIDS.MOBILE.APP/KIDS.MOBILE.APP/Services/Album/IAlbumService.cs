using KIDS.MOBILE.APP.Models.Album;
using KIDS.MOBILE.APP.Models.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KIDS.MOBILE.APP.Services.Album
{
    public interface IAlbumService
    {
        /// <summary>
        /// Lấy ra all album
        /// </summary>
        /// <param name="classId"></param>
        /// <param name="schoolId"></param>
        /// <returns></returns>
        Task<ResponseModel<IEnumerable<AlbumModel>>> GetAllAlbum(string classId, string schoolId);

        /// <summary>
        /// Thêm mới 1 album
        /// </summary>
        /// <param name="insert"></param>
        /// <returns></returns>
        Task<ResponseModel<int>> InsertAlbum(InsertAlbumModel insert, Dictionary<string, string> files = null);
    }
}