using Microsoft.AspNetCore.Mvc;
using OnlineCinema.Interfaces;
using OnlineCinema.Models;
using System.Collections.Generic;

namespace OnlineCinema.Contollers
{
    public class CommentController : Controller
    {
        private readonly ICinemaRepository _repository;
        public CommentController(ICinemaRepository repository)
            => _repository = repository;

        public IEnumerable<Comment> GetComments(int movieId)
            => _repository.GetComments(movieId);

        public IActionResult AddComment(Comment comment)
        {
            _repository.AddComment(comment);
            return Ok();
        }
    }
}