using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces
{
    public interface ILikesRepository
    {
        //method to get an individual like
        Task<AppUserLike> GetUserLike(int SourceUserId, int LikedUserId);
        //method to get a user with their likes
        Task<AppUser> GetUserWithLikes(int userId);
        //method to go and get the likes for a user, whether the users have liked or who'll have been liked by
        Task<PagedList<LikeDto>> GetUserLikes(LikesParams likesParams);
    }
}