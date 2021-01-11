using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;

namespace API.Interfaces
{
    public interface IPhotoRepository
    {
        Task<IEnumerable<PhotoForApprovalDto>> GetUnapprovedPhotos();
        //         Task<PagedList<PhotoForApprovalDto>> GetPhotosForApproval();
        Task<Photo> GetPhotoById(int id);
        Task RemovePhoto(Photo photo);
        void Update(Photo photo);

    }
}