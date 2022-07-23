namespace API.Interfaces
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository {get;}

        IMessageRepository MessageRepository {get;}

        ILikesRepository LikesRepository {get;}

        IPhotoRepository PhotoRepository {get;}

        // method to save changes
        Task<bool> Complete();
        // method to see if EF has been tracking any changes
        bool HasChanges();
    }
}