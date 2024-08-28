using api.DTOs.Comment;
using api.Models;

namespace api.Mappers
{
    public static class CommentMapper
    {
        public static CommentDto ToCommentDto(this Comment commentModel)
        {
            return new CommentDto {
                Id = commentModel.Id,
                Title = commentModel.Title,
                Content = commentModel.Content,
                CreatedOn = commentModel.CreatedOn,
                StockId = commentModel.StockId,
                CreatedBy = commentModel.AppUser.UserName 
            };
        }

        public static Comment ToCommentFromCreateDto(this CreateCommentRequestDto commentDto, int stockId)
        {
            return new Comment {
                Title = commentDto.Title,
                Content = commentDto.Content,
                StockId = stockId
            };
        }

        public static Comment ToCommentFromUpdateDto(this UpdateCommentRequestDto commentDto)
        {
            return new Comment {
                Title = commentDto.Title,
                Content = commentDto.Content
            };
        }
    }
}