using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class LikesController : BaseApiController
    {
        private readonly ILikesRepository _likesRepository;
        private readonly IUserRepository _userRepository;
        public LikesController(ILikesRepository likesRepository, IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _likesRepository = likesRepository;
        }

        [HttpPost("{username}")]
        public async Task<ActionResult> AddLike(string username) {
            var sourceUserId = User.GetUserId();
            var likedUser = await _userRepository.GetUserByUsernameAsync(username);
            var sourceUser = await _likesRepository.GetUserWithLikes(sourceUserId);

            if (likedUser == null) return NotFound();
            if (sourceUser.UserName == username) return BadRequest("You cannot like yourself");

            var userLike = await _likesRepository.GetUserLike(sourceUserId, likedUser.Id);

            if (userLike != null) return BadRequest("You already like this user!");

            userLike = new AppUserLike
            {
                SourceUserId = sourceUserId,
                LikedUserId = likedUser.Id,
            };

            sourceUser.LikedUsers.Add(userLike);
            //temporary implementation
            if (await _userRepository.SaveAllAsync()) return Ok();

            return BadRequest("Failed to like user");
        }
        //TODO: Implement removing like from user
        // [HttpDelete("remove-like/{username}")]

        // public async Task<ActionResult> RemoveLike(string username) {
        //     //implemented ability to undo like user action
        //     // if (userLike != null) sourceUser.LikedUsers.Remove(userLike);
        // }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LikeDto>>> GetUserLikes([FromQuery]LikesParams likesParams){

            likesParams.UserId = User.GetUserId();
            var users = await _likesRepository.GetUserLikes(likesParams);

            Response.AddPaginationHeader(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages);

            return Ok(users);
        }
    }
}