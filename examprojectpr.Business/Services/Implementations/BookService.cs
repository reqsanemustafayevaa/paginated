using examprojectpr.Business.Exceptions;
using examprojectpr.Business.Extentions;
using examprojectpr.Business.Services.Interfaces;
using examprojectpr.Core.Models;
using examprojectpr.Core.Repostories.Interfaces;
using examprojectpr.Data.Repostories.Implementations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace examprojectpr.Business.Services.Implementations
{
    public class BookService : IBookService
    {
        private readonly IGenreRepository _genreRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IAuthorRepository _authorrepository;
        private readonly ITagRepository _tagRepository;
        private readonly IBookTagsRepository _bookTagsRepository;
        private readonly IWebHostEnvironment _env;
        private readonly IBookImagesRepository _bookImagesRepository;

        public BookService(IBookRepository bookRepository,
            IGenreRepository genreRepository,
            IAuthorRepository authorRepository,
            ITagRepository tagRepository,
            IBookTagsRepository bookTagsRepository,
            IWebHostEnvironment env,
            IBookImagesRepository bookImagesRepository)
        {
            _bookRepository = bookRepository;
            _genreRepository = genreRepository;
            _authorrepository = authorRepository;
            _tagRepository= tagRepository;
            _bookTagsRepository= bookTagsRepository;
            _env = env;
            _bookImagesRepository = bookImagesRepository;
            

        }
        public async Task CreateAsync(Book entity)
        {
            if (!_genreRepository.Table.Any(x => x.Id == entity.GenreId))
            {
                throw new NotFoundException("GenreId", "Genre not found!");
            }

            if (!_authorrepository.Table.Any(x => x.Id == entity.AuthorId))
            {
                throw new NotFoundException("AuthorId", "Author not found!");
            }


            bool check = false;

            if (entity.TagIds != null)
            {
                foreach (var tagId in entity.TagIds)
                {
                    if (!_tagRepository.Table.Any(x => x.Id == tagId))
                    {
                        check = true;
                        break;
                    }
                }
            }

            if (check)
            {
                throw new NotFoundException("TagId", "Tag not found!");
            }
            else
            {
                if (entity.TagIds != null)
                {
                    foreach (var tagId in entity.TagIds)
                    {
                        BookTag bookTag = new BookTag
                        {
                            Book = entity,
                            TagId = tagId
                        };

                        await _bookTagsRepository.CreateAsync(bookTag);
                    }
                }
            }

            if (entity.BookPosterImagefiles != null)
            {
                if (entity.BookPosterImagefiles.ContentType != "image/jpeg" && entity.BookPosterImagefiles.ContentType != "image/png")
                {
                    throw new InvalidImageContentException("BookPosterImagefiles", "File must be .png or .jpeg (.jpg)");
                }
                if (entity.BookPosterImagefiles.Length > 2097152)
                {
                    throw new NotFoundException("BookPosterImagefiles", "File size must be lower than 2mb!");
                }

                BookImage bookImage = new BookImage
                {
                    Book = entity,
                    ImageUrl = Helper.SaveFile(_env.WebRootPath, "uploads/books", entity.BookPosterImagefiles),
                    IsPoster = true
                };

                await _bookImagesRepository.CreateAsync(bookImage);
            }

            if (entity.BookHoverImageFiles != null)
            {
                if (entity.BookHoverImageFiles.ContentType != "image/jpeg" && entity.BookHoverImageFiles.ContentType != "image/png")
                {
                    throw new InvalidImageContentException("BookHoverImageFiles", "File must be .png or .jpeg (.jpg)");
                }
                if (entity.BookHoverImageFiles.Length > 2097152)
                {
                    throw new InvalidImageContentException("BookHoverImageFiles", "File size must be lower than 2mb)");
                }

                BookImage bookImage = new BookImage
                {
                    Book = entity,
                    ImageUrl = Helper.SaveFile(_env.WebRootPath, "uploads/books", entity.BookHoverImageFiles),
                    IsPoster = false
                };

                await _bookImagesRepository.CreateAsync(bookImage);
            }


            if (entity.BookImageFiles != null)
            {
                foreach (var imageFile in entity.BookImageFiles)
                {
                    if (imageFile.ContentType != "image/jpeg" && imageFile.ContentType != "image/png")
                    {
                        throw new InvalidImageContentException("BookImageFiles", "File must be .png or .jpeg (.jpg)");
                    }
                    if (imageFile.Length > 2097152)
                    {
                        throw new InvalidImageContentException("BookImageFiles", "File size must be lower than 2mb)");
                    }

                    BookImage bookImage = new BookImage
                    {
                        Book = entity,
                        ImageUrl = Helper.SaveFile(_env.WebRootPath, "uploads/books", imageFile),
                        IsPoster = null
                    };

                    await _bookImagesRepository.CreateAsync(bookImage);
                }
            }

            await _bookRepository.CreateAsync(entity);
            await _bookRepository.CommitAsync();
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }



        public async Task<List<Book>> GetAllAsync()
        {
            return await _bookRepository.GetAllAsync(x => x.IsDeleted == false, "BookImages", "author");
        }

        public Task<List<Book>> GetAllAsync(Expression<Func<Book, bool>>? expression = null)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Book>> GetAllRelatedBooksAsync(Book book)
        {
            return await _bookRepository.GetAllAsync(x => x.GenreId == book.GenreId && !x.IsDeleted && x.Id != book.Id, "Author", "BookImages");
        }


        public async Task<Book> GetByIdAsync(int id)
        {
            var entity = await _bookRepository.GetByIdAsync(x => x.Id == id && x.IsDeleted == false, "author", "BookImages", "BookTags.Tag");

            if (entity is null) throw new NullReferenceException();

            return entity;
        }

        public async Task SoftDelete(int id)
        {
            var entity = await _bookRepository.GetByIdAsync(x => x.Id == id && x.IsDeleted == false);

            if (entity is null) throw new NullReferenceException();

            entity.IsDeleted = true;
            await _bookRepository.CommitAsync();
        }

        public Task UpdateAsync(Book entity)
        {
            throw new NotImplementedException();
        }
    }
}
