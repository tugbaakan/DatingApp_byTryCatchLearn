using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class PhotoRepository : IPhotoRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;

        public PhotoRepository (DataContext context, IMapper mapper, IPhotoService photoService )
        {
            _context = context;
            _mapper = mapper;
            _photoService = photoService;
        }

        public async Task<IEnumerable<PhotoForApprovalDto>> GetUnapprovedPhotos()
        {
            return await _context.Photos
                .IgnoreQueryFilters()
                .Where(p => p.IsApproved == false)
                .ProjectTo<PhotoForApprovalDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<Photo> GetPhotoById(int id)
        {
             return await _context.Photos
                .IgnoreQueryFilters()
                .SingleOrDefaultAsync(x => x.Id == id );

        }

        public async Task RemovePhoto(Photo photo)
        {
            if (photo.PublicId != null)
            {
                var result = await _photoService.DeletePhotoAsync(photo.PublicId);
                
                if (result.Result == "ok")
                {
                    _context.Photos.Remove(photo);
                }
            }
            else
            {
                _context.Photos.Remove(photo);
            }

        }
        public void Update(Photo photo)
        {
            _context.Entry(photo).State = EntityState.Modified;
        }
        

    }
}