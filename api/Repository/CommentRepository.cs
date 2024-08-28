using api.Data;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDBContext context;

        public CommentRepository(ApplicationDBContext context)
        {
            this.context = context;
        }

        public async Task<Comment> CreateAsync(Comment commentModel)
        {
            await context.Comments.AddAsync(commentModel);
            await context.SaveChangesAsync();

            return commentModel;
        }

        public async Task<Comment?> DeleteAsync(int id)
        {
            var commentModel = await context.Comments.FirstOrDefaultAsync(c => c.Id == id);

            if (commentModel is null)
                return null;

            context.Comments.Remove(commentModel);
            await context.SaveChangesAsync();

            return commentModel;
        }

        public async Task<List<Comment>> GetAllAsync()
        {
            return await context.Comments.Include(c => c.AppUser).ToListAsync();
        }

        public async Task<Comment?> GetByIdAsync(int id)
        {
            return await context.Comments.Include(c => c.AppUser).FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Comment?> UpdateAsync(int id, Comment commentModel)
        {
            var existingComment = await context.Comments.FindAsync(id);

            if (existingComment is null)
                return null;

            existingComment.Title = commentModel.Title;
            existingComment.Content = commentModel.Content;

            await context.SaveChangesAsync();

            return existingComment;
        }
    }
}