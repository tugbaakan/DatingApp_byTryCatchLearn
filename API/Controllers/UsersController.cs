using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Authorize]
    public class UsersController : BaseAPIController
    {
        /*private readonly DataContext _context;
        public UsersController(DataContext context)
        {
            _context = context;

        }*/

        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public UsersController(IUserRepository userRepository, IMapper mapper)
        {
            _mapper = mapper;
            _userRepository = userRepository;
        }

        [HttpGet]
        //[AllowAnonymous]
       /* public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
        {
            //return await _context.Users.ToListAsync();
            return Ok(await _userRepository.GetUsersAsync());
        }*/

        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
        {
            var users = await _userRepository.GetMembersAsync();
            return Ok(users);
        }

        /*[HttpGet("{username}")]
        public async Task<ActionResult<AppUser>> GetUser(string username)
        {
            //return  _context.Users.FindAsync(id);
            return await _userRepository.GetUserByUsernameAsync (username);
        }*/

        [HttpGet("{username}")]
        public async Task<ActionResult<MemberDto>> GetUser(string username)
        {
            return await _userRepository.GetMemberAsync(username);
        }
        
        
        //[Authorize]
        // api/users/id
       /* [HttpGet("{id}")]
        public async Task<ActionResult<AppUser>> GetUser(int id)
        {
            //return  _context.Users.FindAsync(id);
            return await _userRepository.GetUserByIdAsync(id);
        }*/

    }
}