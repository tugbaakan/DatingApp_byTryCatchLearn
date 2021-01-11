using System.Linq;
using System.Threading.Tasks;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AdminController : BaseAPIController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public AdminController(UserManager<AppUser> userManager, IUnitOfWork unitOfWork
            , IMapper mapper )
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("users-with-roles")]
        public async Task<ActionResult> GetUsersWithRoles()
        {
            var users = await _userManager.Users
                .Include(r => r.UserRoles)
                .ThenInclude(r => r.Role)
                .OrderBy(u => u.UserName)
                .Select(u => new
                {
                    u.Id,
                    UserName = u.UserName,
                    Roles = u.UserRoles.Select(r => r.Role.Name).ToList()
                })
                .ToListAsync();

            return Ok(users);
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost("edit-roles/{username}")]
        public async Task<ActionResult> EditRoles(string userName, [FromQuery] string roles)
        {
            var selectedRoles = roles.Split(",").ToArray();

            var user = await _userManager.FindByNameAsync(userName);

            if (user == null) return NotFound("Could not find user");

            var userRoles = await _userManager.GetRolesAsync(user);

            var result = await _userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles));

            if (!result.Succeeded) return BadRequest("Failed to add to roles");

            result = await _userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles));

            if (!result.Succeeded) return BadRequest("Failed to remove from roles");

            return Ok(await _userManager.GetRolesAsync(user));
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("photos-to-moderate")]
        public  async Task<ActionResult> GetPhotosForModeration()
        {
           var photos = await _unitOfWork.PhotoRepository.GetUnapprovedPhotos();

            return Ok(photos);
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost("approve-photo/{id}")]
        public async Task<ActionResult> ApprovePhoto( int id )
        {
            var photo = await _unitOfWork.PhotoRepository.GetPhotoById(id);

            if (photo == null) return NotFound("Could not find photo");

            photo.IsApproved = true;

            // check if this is the only photo the member has            
            var user = await _unitOfWork.UserRepository.GetUserByIdAsync(photo.AppUserId);

            if ( user.Photos.Count == 1 )
                photo.IsMain = true;

            _unitOfWork.PhotoRepository.Update(photo);
        
            if( await _unitOfWork.Complete()) return NoContent();
            return BadRequest("Failed to approve photo");
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost("reject-photo/{id}")]
        public async Task<ActionResult> RejectPhoto( int id )
        {
            var photo = await _unitOfWork.PhotoRepository.GetPhotoById(id);

            await _unitOfWork.PhotoRepository.RemovePhoto(photo);
                
            if( await _unitOfWork.Complete()) return NoContent();
            return BadRequest("Failed to reject photo");
        }


    }
}