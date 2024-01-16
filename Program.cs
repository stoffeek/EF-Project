using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using EFpro;
using System.Globalization;

using var db = new BloggingContext();


Console.WriteLine($"Database path: {db.DbPath}.");


List<User> users = new List<User>();
List<Blog> blogs = new List<Blog>();
List<Post> posts = new List<Post>();

//User
using (var reader = new StreamReader(@"C:\Users\stoff\source\repos\EFpro\Data\Users.csv"))
{
    reader.ReadLine(); // Skip header
    while (!reader.EndOfStream)
    {
        var line = reader.ReadLine();
        var split = line.Split(',');

        var userId = int.Parse(split[0]);
        var user = users.FirstOrDefault(u => u.UserId == userId);
        if (user == null)
        {
            user = new User
            {
                UserId = userId,
                Username = split[1],
                Password = split[2],
                Posts = new List<Post>()
            };
            users.Add(user);
        }



    }

}

//Blog
using (var reader = new StreamReader(@"C:\Users\stoff\source\repos\EFpro\Data\Blogs.csv"))
{
    reader.ReadLine(); // Skip header
    while (!reader.EndOfStream)
    {
        var line = reader.ReadLine();
        var split = line.Split(',');

        var blogId = int.Parse(split[0]);
        if (!blogs.Any(b => b.BlogId == blogId))
        {
            blogs.Add(new Blog
            {
                BlogId = blogId,
                Url = split[1],
                Name = split[2]
            });
        }
    }
}


foreach (var post in posts)
{
    var blog = blogs.FirstOrDefault(b => b.BlogId == post.BlogId);
    if (blog != null)
    {
        blog.Posts.Add(post);
    }
}

//Post
using (var reader = new StreamReader(@"C:\Users\stoff\source\repos\EFpro\Data\Posts.csv"))
{
    reader.ReadLine(); // Skip header
    while (!reader.EndOfStream)
    {
        var line = reader.ReadLine();
        var split = line.Split(',');

        var post = new Post
        {
            PostId = int.Parse(split[0]),
            Title = split[1],
            Content = split[2],
            PublishedOn = DateTime.ParseExact(split[3], "yyyymmdd", CultureInfo.InvariantCulture),
            UserId = int.Parse(split[4]),
	    BlogId = int.Parse(split[5])
        };

        posts.Add(post);

        var user = users.FirstOrDefault(u => u.UserId == post.UserId);
        if (user != null)
        {
            user.Posts.Add(post);
        }

        var blog = blogs.FirstOrDefault(b => b.BlogId == post.BlogId);
        if (blog != null)
        {
            blog.Posts.Add(post);
        }
    }
}

using (var context = new BloggingContext())
{
    context.Users.RemoveRange(context.Users);
    context.Blogs.RemoveRange(context.Blogs);
    context.Posts.RemoveRange(context.Posts);
    context.SaveChanges();

    context.Users.AddRange(users);
    context.Blogs.AddRange(blogs);
    context.Posts.AddRange(posts);
    context.SaveChanges();
}
using (var context = new BloggingContext())
{
    var allUsers = context.Users.Include(u => u.Posts).ThenInclude(p => p.Blog).ToList();

    foreach (var user in allUsers)
    {
        Console.WriteLine($"User: {user.Username}, UserId: {user.UserId}");
        foreach (var post in user.Posts)
        {
            Console.WriteLine($" Blog: {post.Blog.Name} ,Content: {post.Content} ,Title: {post.Title},PostId: {post.PostId}, Published: {post.PublishedOn}");
        }
    }
}
